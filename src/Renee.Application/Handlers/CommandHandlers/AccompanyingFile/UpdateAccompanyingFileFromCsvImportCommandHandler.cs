using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class UpdateAccompanyingFileFromCsvImportCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService) : IRequestHandler<UpdateAccompanyingFileFromCsvFileCommandInput, ImportCsvDataCommandResult>
{
	public async Task<ImportCsvDataCommandResult> Handle(UpdateAccompanyingFileFromCsvFileCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.UserId == Guid.Empty)
				return ImportCsvDataCommandResult.Failure(
					[LineErrorReport.Create(null, Labels.Errors.UserNotFound)]);

			var errors = new List<LineErrorReport>();
			var updatedAccompanyingFileReferences = new List<string>();
			var accompanyingFilesToUpdate = request.Input;

			foreach (var updateAccompanyingFile in accompanyingFilesToUpdate)
			{
				var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileForCsvImport(
					updateAccompanyingFile.AccompanyingFileId);


				var dataLine = updateAccompanyingFile.DataLine;

				if (accompanyingFile is null)
				{
					errors.AddRange([LineErrorReport.Create(
						dataLine.LineNumber, string.Format(CsvDataLabel.Errors.AccompanyingFileNotFoundInDataBase, dataLine.ExternalReference))]);
					continue;
				}

				accompanyingFile.UpdatedBy = request.UserId;
				accompanyingFile.LastUpdateDate = DateTime.UtcNow;
				accompanyingFile.ImportRunId = request.ImportRunId;
				accompanyingFile.IsDeleted = dataLine.IsDeleted;
				accompanyingFile.AccompanyingTimeDurationForIdentificationMilestone =
					(int?)dataLine.AccompanyingTimeDurationForIdentificationMilestone
					?? accompanyingFile.AccompanyingTimeDurationForIdentificationMilestone;

				accompanyingFile.AccompanyingTimeDurationForOrganizeAndFinanceMilestone =
					(int?)dataLine.AccompanyingTimeDurationForOrganizeAndFinanceMilestone
					?? accompanyingFile.AccompanyingTimeDurationForOrganizeAndFinanceMilestone;

				accompanyingFile.AccompanyingTimeDurationForRealizeAndFollowMilestone =
					(int?)dataLine.AccompanyingTimeDurationForRealizeAndFollowMilestone
					?? accompanyingFile.AccompanyingTimeDurationForRealizeAndFollowMilestone;

				accompanyingFile.StartOfAccompanyingDate = dataLine.StartOfAccompanyingDate ?? accompanyingFile.StartOfAccompanyingDate;
				accompanyingFile.FirstEncounterDate = dataLine.FirstEncounterDate ?? accompanyingFile.FirstEncounterDate;


				// Identification Milestone

				var updatedHouseholdDifficulties = NullOrEmptyFallback(
					updateAccompanyingFile.UpdatedHouseholdDifficulties,
					accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdDifficulties
						.Select(hd => (Guid?)hd.Difficulty)
						.ToList());

				var updatedHouseholdExpenses = NullOrEmptyFallback(
					updateAccompanyingFile.UpdatedHouseholdExpenses,
					accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdExpenses
						.Select(r => new UpdatedHouseholdExpenses(r.Id, (ExpenseType)r.Type, r.Value))
						.ToList());

				AddOrUpdateMonthlyenergeticsExpenses(updatedHouseholdExpenses, dataLine.MonthlyEnergeticsExpenses);

				var updatedHouseholdResources = NullOrEmptyFallback(
					updateAccompanyingFile.UpdatedHouseholdResources,
					accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdResources
						.Select(rtv => new UpdatedHouseholdResource(rtv.HouseholdResources, rtv.Value))
						.ToList());

				var updatedHouseholdHeatingEnergy = NullOrEmptyFallback(
					updateAccompanyingFile.UpdatedHouseholdHeatingEnergy,
					accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdHeatingEnergies
						.Select(he => new UpdatedHouseholdHeatingEnergy(he.HouseholdHeatingEnergyLabel, he.Value))
						.ToList());

				var existingSecondaryOccupants = accompanyingFile.AccompanyingFileHouseholdNavigation.SecondaryOccupants
					.Select(so => new UpdatedHouseholdSecondaryOccupant(so.Id, so.Trigram, so.Birthdate, so.Age, so.IsDependent))
					.ToList();

				var secondaryOccupants = dataLine.NumberOfOccupants != null ?
					CreateSecondaryOccupants(dataLine.NumberOfOccupants)
					: existingSecondaryOccupants;


				var inputIdentification = new SaveAccompanyingFileIdentificationMilestoneCommandInput(
					updateAccompanyingFile.AccompanyingFileId,
					dataLine.AccompanyingTimeDurationForIdentificationMilestone,
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateHousehold(dataLine, accompanyingFile.AccompanyingFileHouseholdNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateHouseholdMainOccupant(dataLine,
						accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation),
					secondaryOccupants,
					updatedHouseholdDifficulties,
					updatedHouseholdExpenses,
					updatedHouseholdResources,
					updatedHouseholdHeatingEnergy,
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdatedHousing(dataLine, accompanyingFile.AccompanyingFileHousingNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateAddress(dataLine, accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateInitialStateForIdentificationMilestone(dataLine,
					accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation),
					request.UserId,
					dataLine.StartOfAccompanyingDate ?? accompanyingFile.StartOfAccompanyingDate,
					dataLine.FirstEncounterDate ?? accompanyingFile.FirstEncounterDate,
					accompanyingFile.DeliveryTime,
					accompanyingFile.ShouldAccompanyingFileBeSubmittedToAnah,
					false);

				var identificationMilestoneDto = AccompanyingFileMilestoneUpdaterExtension.SaveIdentificationMilestoneUpdater(inputIdentification, accompanyingFile);


				// Organize And Finance Milestone

				var updatedWorkPackages =
					(accompanyingFile.AccompanyingFilePreWorkPlanNavigation.WorkPackages?
						.Select(wp => new UpdateWorkPackage(
							wp.Id,
							wp.EnergeticsEffectAfterWorks,
							wp.WorkPackageWorkTypeCosts
								?.Select(wt => new UpdateWorkTypeCost(wt.WorkType, wt.Cost, wt.Description))
								.ToList() ?? []
						)).ToList())
					?? [];

				var updatedFundingModes =
					(accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.FundingModes?
						.Select(fm => new UpdateFundingModes(fm.Id, fm.Label, fm.Value))
						.ToList())
					?? [];


				var projectTypeIds = NullOrEmptyFallback(
					updateAccompanyingFile.ProjectTypeIds,
					accompanyingFile.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes
						.Select(pt => pt.ProjectType)
						.ToList());

				var insuranceTypeIds = NullOrEmptyFallback(
					updateAccompanyingFile.InsuranceTypeIds,
					accompanyingFile.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanInsuranceTypes
						.Select(pi => pi.InsuranceType)
						.ToList());


				var inputOrganizeAndFinance = new SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput(
					updateAccompanyingFile.AccompanyingFileId,
					dataLine.AccompanyingTimeDurationForOrganizeAndFinanceMilestone,
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateHousing(dataLine, accompanyingFile.AccompanyingFileHousingNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateHousingInitialStateForOrganizeAndFinanceMilestone(dataLine, accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateHousingAfterworkState(dataLine, accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdatePreWorkPlan(dataLine, projectTypeIds, insuranceTypeIds, accompanyingFile.AccompanyingFilePreWorkPlanNavigation),
					AccompanyingFileMilestoneUpdaterExtension.ImportToUpdatePreFinancingPlan(dataLine, accompanyingFile.AccompanyingFilePreFinancingPlanNavigation),
					updatedWorkPackages,
					updatedFundingModes,
					request.UserId,
					null,
					null);

				accompanyingFile.AccompanyingFilePreWorkPlanNavigation ??= new PreWorkPlan();
				accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation ??= new HousingAfterWorkState();
				accompanyingFile.AccompanyingFilePreFinancingPlanNavigation ??= new PreFinancingPlan();

				var organizeAndFinanceMilestoneDto = AccompanyingFileMilestoneUpdaterExtension.SaveOrganizeAndFinanceMilestoneUpdater(inputOrganizeAndFinance, accompanyingFile);

				// Realize and Follow Milestone

				var invoices =
					accompanyingFile.Invoices?
						.Select(iv => new UpdatedInvoice(iv.InvoiceCost, iv.LaborCost, iv.Id))
					?? [];

				accompanyingFile.AccompanyingFileWorkMonitoringNavigation ??= new WorkMonitoring();
				accompanyingFile.SiteSupervision ??= new SiteSupervision();

				var updatedProjectCost = AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateProjectCost(dataLine, accompanyingFile.AccompanyingFileWorkMonitoringNavigation);
				var updatedWorkSummary = AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateWorkSummary(dataLine, accompanyingFile.AccompanyingFileWorkMonitoringNavigation);
				var updatedEvaluation = AccompanyingFileMilestoneUpdaterExtension.ImportToUpdateEvaluation(dataLine, accompanyingFile.AccompanyingFileWorkMonitoringNavigation);

				accompanyingFile.AccompanyingFileWorkMonitoringNavigation.UpdateWorkMonitoring(AccompanyingFileMilestoneUpdaterExtension.CreateWorkMonitoring(updatedProjectCost, updatedWorkSummary, updatedEvaluation));

				var realizeAndFollowMilestoneDto = AccompanyingFileMilestoneUpdaterExtension.SaveRealizeAndFollowMilestoneUpdater(
					new SaveAccompanyingFileRealizeAndFollowCommandInput(
						updatedProjectCost,
						[.. invoices],
						updatedWorkSummary,
						updatedEvaluation,
						AccompanyingFileMilestoneUpdaterExtension.ImportToUpdatePreFinancingPlan(dataLine, accompanyingFile.AccompanyingFilePreFinancingPlanNavigation),
						new UpdatedSiteSupervision(null, null, null, null, null, null, null, []),
						new UpdateHousingAfterWorkStateForRealizeAndFollowMilestone(null, null),
						dataLine.EndOfAccompanyingDate ?? accompanyingFile.EndOfAccompanyingDate,
						dataLine.EndOfEncounterDate ?? accompanyingFile.EndOfEncounterDate,
						dataLine.AccompanyingTimeDurationForRealizeAndFollowMilestone,
						null,
						updateAccompanyingFile.AccompanyingFileId,
						request.UserId),
					accompanyingFile);

				var numberItemsChanged = await accompanyingFileRepository.UpdateAccompanyingFileImportedWithCsvAsync(
						accompanyingFile,
						identificationMilestoneDto,
						organizeAndFinanceMilestoneDto,
						realizeAndFollowMilestoneDto);

				if (numberItemsChanged == -1)
					errors.AddRange([LineErrorReport.Create(
						dataLine.LineNumber, string.Format(CsvDataLabel.Errors.ErrorWhileUpdatingAccompanyingFile, accompanyingFile.ExternalReference))]);
				else if (numberItemsChanged == 0)
					errors.AddRange([LineErrorReport.Create(
						dataLine.LineNumber, string.Format(CsvDataLabel.Errors.NoAccompanyingFileUpdated, accompanyingFile.ExternalReference))]);
				else
					updatedAccompanyingFileReferences.Add(string.Format(CsvDataLabel.AccompanyingFileSuccessfullyUpdated, accompanyingFile.AccompanyingFileReference, accompanyingFile.ExternalReference));
			}

			if (errors.Count > 0)
				return ImportCsvDataCommandResult.PartialSuccess(updatedAccompanyingFileReferences, errors);

			return ImportCsvDataCommandResult.Success(updatedAccompanyingFileReferences);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ImportCsvDataCommandResult.Failure(
				[LineErrorReport.Create(null, Labels.Errors.UnhandledErrorOccured)]);
		}
	}

	private static List<T> NullOrEmptyFallback<T>(List<T>? list, List<T> fallback) =>
	list is null or { Count: 0 } ? fallback : list;

	private static List<UpdatedHouseholdSecondaryOccupant> CreateSecondaryOccupants(int? count)
	{
		var result = new List<UpdatedHouseholdSecondaryOccupant>();

		if (!count.HasValue || count.Value <= 0)
			return result;
		for (int i = 0; i < count.Value - 1; i++)
		{
			result.Add(new UpdatedHouseholdSecondaryOccupant(Guid.Empty, string.Empty, null, null, null));
		}
		return result;
	}

	public static void AddOrUpdateMonthlyenergeticsExpenses(
		List<UpdatedHouseholdExpenses> updatedHouseholdExpenses, double? monthlyEnergeticsExpenses)
	{
		if (monthlyEnergeticsExpenses is not double val)
			return;
		var index = updatedHouseholdExpenses
				.FindIndex(e => e.ExpenseType == ExpenseType.MonthlyEnergecticsExpenses);

		if (index == -1)
		{
			updatedHouseholdExpenses.Add(
				new UpdatedHouseholdExpenses(null, ExpenseType.MonthlyEnergecticsExpenses, val));
			return;
		}
		var existing = updatedHouseholdExpenses[index];
		updatedHouseholdExpenses[index] = new UpdatedHouseholdExpenses(
			existing.Id,
			existing.ExpenseType,
			val
		);
	}

}