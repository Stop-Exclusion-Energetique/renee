using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetRealiseAndFollowSynthesisQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetRealiseAndFollowSynthesisQuery, ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>>
{
	public async Task<ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>> Handle(
		GetRealiseAndFollowSynthesisQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFileEntity =
				await accompanyingFileRepository.GetAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(request.Id);

			if (accompanyingFileEntity is null) return ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			return ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>.Success(new GetRealiseAndFollowSynthesisQueryObjectResult
			{
				Id = accompanyingFileEntity.Id,
				AccompaniyingFileReference = accompanyingFileEntity.AccompanyingFileReference,
				Stage = (AccompanyingFileStage)accompanyingFileEntity.AccompanyingFileMilestone,
				Status = (AccompanyingFileStatus)accompanyingFileEntity.AccompanyingFileStatus,
				AccompanyingCost = accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.AccompanyingCost,
				HouseholdAutoFinancing =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.HouseholdSelfFinancing,
				InvoicesSummary = GetInvoicesSummary(accompanyingFileEntity.Invoices),
				IntermediateAirtightnessTestResult =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.IntermediateAirtightnessTestResult,
				WaterproofingTreatmentActions =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation
						?.JustificationAndActionsPutInPlaceIfNoTest,
				HasEffectiveComplianceWithWorkRecommendations =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation
						?.HasEffectiveComplianceWithWorkRecommendations,
				HasWorkEnablingHomeSupport =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation
						?.HasWorksEnabledHouseholdToStayAtHome,
				WellBeingRating = accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.WellBeingRating,
				EducationalFrameworkRating =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.EducationalFrameworkRating,
				FamilySatisfactionWithSupport =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.FamilySatisfaction,
				IsBackToEmployment =
					accompanyingFileEntity.AccompanyingFileWorkMonitoringNavigation?.ReturnToEmployment,
				EndOfAccompanyingDate = accompanyingFileEntity.EndOfAccompanyingDate,
				AccompanyingTime =
					GetAccompanyingTime(
						accompanyingFileEntity.StartOfAccompanyingDate ?? DateTime.Now,
						accompanyingFileEntity.EndOfAccompanyingDate ?? DateTime.Now),
				EndOfEncounterDate = accompanyingFileEntity.EndOfEncounterDate,
				AccompanyingTimeDuration = (AccompanyingTimeDuration?)accompanyingFileEntity.AccompanyingTimeDurationForRealizeAndFollowMilestone,
				IsInTzeeProgram = (bool)accompanyingFileEntity.ZeroEnergyExclusionTerritoriesProgram!,
				SolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SolidarBuilder,
				SecondSolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder,
				ThirdSolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder,
				DiffuseCoordinator = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator,
				TargetedCoordinator = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.TargetCoordinator,
				TerritorialBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.TerritorialBuilder,
				SecondTerritorialBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static string GetAccompanyingTime(DateTime startDate, DateTime endDate)
	{
		return $"{(endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month}";
	}

	private static Dictionary<double, double> GetInvoicesSummary(ICollection<Invoice> invoices)
	{
		var summary = new Dictionary<double, double>();

		foreach (var invoice in invoices)
		{
			var invoiceCost = invoice.InvoiceCost ?? 0;
			summary[invoiceCost] = invoice.LaborCost ?? 0;
		}

		return summary;
	}
}