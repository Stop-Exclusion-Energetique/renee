using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension.ImportExcelDataUseCase;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class ImportAccompanyingFileFromFileInputCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateAccompanyingFileFromExcelFileCommandInput, ImportExcelDataCommandResult>
{
	public async Task<ImportExcelDataCommandResult> Handle(
		CreateAccompanyingFileFromExcelFileCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			if (request.Input is null)
				return ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.SolidarBuilderNotFound]);

			var accompanyingFileData = new ImportAccompanyingFileData
			{
				UserId = request.UserId,
				Reference = request.Input.Reference,
				FirstContactDate = request.Input.FirstContactDate,
				StartSupportDate = request.Input.StartSupportDate,
				EndSupportDate = request.Input.EndSupportDate,
				EndContactDate = request.Input.EndContactDate,
				ContactWithFamilyDuringIdentifyStage = request.Input.ContactWithFamilyForIdentificationMilestone,
				ContactWithFamilyDuringOrganizeAndFinanceStage = request.Input.ContactWithFamilyForOrganizeAndFinanceMilestone,
				ContactWithFamilyDuringRealizeAndFollowStage = request.Input.ContactWithFamilyForRealizeAndFollowMilestone,
				ZeroEnergyExclusionTerritoriesProgram = request.Input.ZeroEnergyExclusionTerritoriesProgram,
				AccompanyingType = request.Input.AccompanyingType,
				TerritoryId = request.Input.TerritoryId
			};

			var accompanyingFile = ImportAccompanyingFileExtension
				.InitializeAccompanyingFile(accompanyingFileData)
				.ImportSupportTeam(request.Input.SupportTeam.CreateSupportTeam())
				.ImportHousehold(request.Input.Household.CreateHousehold())
				.ImportHousing(request.Input.Housing.CreateHousing())
				.ImportPreWorkPlan(request.Input.PreWorkPlan.CreatePreWorkPlan())
				.ImportPreFinancingPlan(request.Input.PreFinancingPlan.CreatePreFinancingPlan())
				.ImportWorkMonitoring(request.Input.WorkMonitoring.CreateWorkMonitoring())
				.ImportInvoice(request.Input.Invoice.CreateInvoice());

			var numberItemsChanged = await accompanyingFileRepository.AddAccompanyingFile(accompanyingFile);
			return numberItemsChanged > -1
				? ImportExcelDataCommandResult.Success()
				: ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.WhileSavingFolder]);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ImportExcelDataCommandResult.Failure([Labels.Errors.UnhandledErrorOccured]);
		}
	}
}