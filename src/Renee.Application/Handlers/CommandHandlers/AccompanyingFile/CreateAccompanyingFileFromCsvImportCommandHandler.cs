using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Interfaces;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain;
using Renee.Domain.DomainExtension.ImportCsvDataUseCase;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class CreateAccompanyingFileFromCsvImportCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateAccompanyingFileFromCsvFileCommandInput, ImportCsvDataCommandResult>
{
	public async Task<ImportCsvDataCommandResult> Handle(CreateAccompanyingFileFromCsvFileCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.UserId == Guid.Empty)
				return ImportCsvDataCommandResult.Failure(
					[LineErrorReport.Create(null, Labels.Errors.UserNotFound)]);

			if (request.Input.Count == 0)
				return ImportCsvDataCommandResult.Success([CsvDataLabel.NoAccompanyingFilesToCreate]);

			var errors = new List<LineErrorReport>();
			var validParsedData = request.Input;

			(validParsedData, var solidarBuilderErrors) = FilterInvalidSolidarBuilders(validParsedData);
			errors.AddRange(solidarBuilderErrors);

			(validParsedData, var duplicateExternalReferencesInCsvErrors) = FilterDuplicateReferencesInCsv(validParsedData);
			errors.AddRange(duplicateExternalReferencesInCsvErrors);

			if (validParsedData.Count == 0)
				return ImportCsvDataCommandResult.Failure(errors);

			var accompanyingFiles = validParsedData.Select(data => ImportAccompanyingFileCsvDataExtension
					.InitializeAccompanyingFile(new ImportAccompanyingFileCsvData
					{
						UserId = request.UserId,
						Reference = data.Reference,
						ExternalReference = data.ExternalReference,
						FirstContactDate = data.FirstContactDate,
						StartSupportDate = data.StartSupportDate,
						FileOpeningDate = data.FileOpeningDate,
						FileClosingDate = data.FileClosingDate,
						EndSupportDate = data.EndSupportDate,
						EndContactDate = data.EndContactDate,
						AccompanyingTimeDurationForIdentificationMilestone = data.AccompanyingTimeDurationForIdentificationMilestone,
						AccompanyingTimeDurationForOrganizeAndFinanceMilestone = data.AccompanyingTimeDurationForOrganizeAndFinanceMilestone,
						AccompanyingTimeDurationForRealizeAndFollowMilestone = data.AccompanyingTimeDurationForRealizeAndFollowMilestone,
						ZeroEnergyExclusionTerritoriesProgram = data.ZeroEnergyExclusionTerritoriesProgram,
						AccompanyingType = data.AccompanyingType,
						TerritoryId = data.TerritoryId,
						IsDeleted = data.IsDeleted,
						ImportRunId = request.ImportRunId
					})
					.ImportSupportTeam(data.SupportTeam.CreateSupportTeam())
					.ImportHousehold(data.Household.CreateHousehold())
					.ImportHousing(data.Housing.CreateHousing())
					.ImportPreWorkPlan(data.PreWorkPlan.CreatePreWorkPlan())
					.ImportPreFinancingPlan(data.PreFinancingPlan.CreatePreFinancingPlan())
					.ImportWorkMonitoring(data.WorkMonitoring.CreateWorkMonitoring())
					.ImportSiteSupervision(new SiteSupervision())).ToList();

			var numberItemsChanged = await accompanyingFileRepository.AddAccompanyingFiles(accompanyingFiles);

			if (numberItemsChanged == -1)
				return ImportCsvDataCommandResult.Failure(
					[LineErrorReport.Create(null, CsvDataLabel.Errors.ErrorWhileCreatingAccompanyingFiles)]);

			if (numberItemsChanged == 0)
				return ImportCsvDataCommandResult.Failure(
					[LineErrorReport.Create(null, [CsvDataLabel.Errors.NoAccompanyingFilesCreated])]);

			var accompanyingFileReferencesAdded = accompanyingFiles.Select(af => 
				string.Format(CsvDataLabel.AccompanyingFileSuccessfullyCreated, af.AccompanyingFileReference, af.ExternalReference)).ToList();

			if (errors.Count > 0)
				return ImportCsvDataCommandResult.PartialSuccess(accompanyingFileReferencesAdded, errors);

			return ImportCsvDataCommandResult.Success(accompanyingFileReferencesAdded);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ImportCsvDataCommandResult.Failure(
				[LineErrorReport.Create(null, Labels.Errors.UnhandledErrorOccured)]);
		}
	}

	private static (List<ImportAccompanyingFileFromCsvCommandInput>, List<LineErrorReport>) FilterInvalidSolidarBuilders(
		List<ImportAccompanyingFileFromCsvCommandInput> items)
	{
		var errors = new List<LineErrorReport>();

		var invalidData = items
			.Where(af => af.SupportTeam.SolidarBuilderId == Guid.Empty)
			.ToList();

		if (invalidData.Count > 0)
		{
			var invalidDataErrors = invalidData.Select(i => LineErrorReport.Create(
				i.LineNumber, string.Format(CsvDataLabel.Errors.InvalidSolidarBuilder, i.ExternalReference))).ToList();

			errors.AddRange(invalidDataErrors);

			items = items.Where(af => af.SupportTeam.SolidarBuilderId != Guid.Empty).ToList();
		}

		return (items, errors);
	}

	private static (List<ImportAccompanyingFileFromCsvCommandInput>, List<LineErrorReport>) FilterDuplicateReferencesInCsv(
	List<ImportAccompanyingFileFromCsvCommandInput> items)
	{
		var errors = new List<LineErrorReport>();

		var duplicatedGroups = items
			.GroupBy(f => f.ExternalReference)
			.Where(g => g.Count() > 1)
			.ToList();

		if (duplicatedGroups.Count > 0)
		{
			var invalidDataErrors = duplicatedGroups.Select(group => LineErrorReport.Create(
				group.First().LineNumber, string.Format(CsvDataLabel.Errors.DuplicatedExternalReference, group.Key))).ToList();

			errors.AddRange(invalidDataErrors);

			items = items
				.GroupBy(f => f.ExternalReference)
				.Select(g => g.First())
				.ToList();
		}

		return (items, errors);
	}
}