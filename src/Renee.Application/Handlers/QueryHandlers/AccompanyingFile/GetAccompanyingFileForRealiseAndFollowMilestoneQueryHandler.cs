using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.DTOs.SiteSupervision;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileForRealiseAndFollowMilestoneQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAccompanyingFileForRealiseAndFollowMilestoneQuery,
		ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>> HandleQuery(
		GetAccompanyingFileForRealiseAndFollowMilestoneQuery request)
	{
		try
		{
			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(request.AccompanyingFileId);

			if (accompanyingFile is null)
				return ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFile);

			if (accompanyingFile is not null && (IsUserInSupportTeam(accompanyingFile, request.UserId) || IsUserACoordinatorOrAdmin(request.UserRole)))
				return ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>.Success(CreateQueryObjectResult(accompanyingFile));

			return ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static bool IsUserInSupportTeam(Domain.Entity.AccompanyingFile accompanyingFile, Guid userId)
		=> userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.TerritorialBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SecondTerritorialBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SolidarBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SecondSolidarBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.ThirdSolidarBuilder ;

	private static bool IsUserACoordinatorOrAdmin(string userRole)
		=> userRole.Equals(Constants.DiffuseCoordinatorRole) ||
			userRole.Equals(Constants.TargetedCoordinatorRole) ||
			userRole.Equals(Constants.AdminRole);

	private static GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult CreateQueryObjectResult(
		Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return new GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult
		{
			Status = (AccompanyingFileStatus)accompanyingFile.AccompanyingFileStatus,
			Reference = accompanyingFile.AccompanyingFileReference,
			Invoices =
				accompanyingFile.Invoices.Select(
					invoice => new InvoiceQueryObjectResult
					{
						Id = invoice.Id,
						TotalCost = invoice.InvoiceCost,
						BilledWorkForce = invoice.LaborCost
					}).ToList(),
			AccompanyingCost = (GeographicalHousingAreaTypology?)accompanyingFile.AccompanyingFileHousingNavigation.GeographicAreaTypology == GeographicalHousingAreaTypology.Urban
				? 6500 : 7000,
			HouseholdAutoFinancing = GetWorkMonitoringValue(accompanyingFile, wm => wm.HouseholdSelfFinancing),
			IntermediateAirtightnessTestResult =
				GetWorkMonitoringValue(accompanyingFile, wm => wm.IntermediateAirtightnessTestResult),
			WaterproofingTreatmentActions =
				GetWorkMonitoringValue(accompanyingFile, wm => wm.JustificationAndActionsPutInPlaceIfNoTest),
			HasEffectiveComplianceWithWorkRecommendations =
				GetWorkMonitoringValue(accompanyingFile, wm => wm.HasEffectiveComplianceWithWorkRecommendations),
			HasWorkEnablingHomeSupport =
				GetWorkMonitoringValue(accompanyingFile, wm => wm.HasWorksEnabledHouseholdToStayAtHome),
			WellBeingRating = GetWorkMonitoringValue(accompanyingFile, wm => wm.WellBeingRating),
			EducationalFrameworkRating =
				GetWorkMonitoringValue(accompanyingFile, wm => wm.EducationalFrameworkRating),
			FamilySatisfactionWithSupport = GetWorkMonitoringValue(accompanyingFile, wm => wm.FamilySatisfaction),
			IsBackToEmployment = GetWorkMonitoringValue(accompanyingFile, wm => wm.ReturnToEmployment),
			EndOfAccompanyingDate = accompanyingFile.EndOfAccompanyingDate,
			EndOfEncounterDate = accompanyingFile.EndOfEncounterDate,
			StartOfAccompanyingDate = accompanyingFile.StartOfAccompanyingDate,
			AccompanyingTimeDuration = (AccompanyingTimeDuration?)accompanyingFile.AccompanyingTimeDurationForRealizeAndFollowMilestone,
			HasHousingAdaptationWorks =
				GetWorkMonitoringValue(accompanyingFile, wm => wm.HasHousingAdaptationWorks),
			HasFinishingWorks = GetWorkMonitoringValue(accompanyingFile, wm => wm.HasFinishingWorks),
			HasSafetyWorks = GetWorkMonitoringValue(accompanyingFile, wm => wm.HasSafetyWorks),
			HasPreparationWorks = GetWorkMonitoringValue(accompanyingFile, wm => wm.HasPreparationWorks),
			HasEmergencyWorks = GetWorkMonitoringValue(accompanyingFile, wm => wm.HasEmergencyWorks),
			HasUnsanitaryExit = GetWorkMonitoringValue(accompanyingFile, wm => wm.HasUnsanitaryExit),
			TreatedAirTightness =
				(PartlyStateTreatment?)GetWorkMonitoringValue(accompanyingFile, wm => wm.TreatedAirTightness),
			TreatedThermalBridges =
				(PartlyStateTreatment?)GetWorkMonitoringValue(accompanyingFile, wm => wm.TreatedThermalBridges),
			HasHumidityManagement = GetWorkMonitoringValue(accompanyingFile, wm => wm.HasHumidityManagement),
			IsInTzeeProgram = accompanyingFile.ZeroEnergyExclusionTerritoriesProgram ?? false,
			UnderprivilegedHousingFoundation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation,
			AdaptationBonus = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeAdapt,
			BankLoanType = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.SolicitedBankLoanType,
			ClassicBankLoan = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.ClassicBankLoan,
			CoOwnershipBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovCoOwnerShip,
			DecentHousingBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeLogementDecent,
			Department = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids,
			DepartmentalHouseForDisabledPersons =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MdphFinancing,
			EnergySavingCertificates = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.CeeFinancing,
			ExitEnergySieveBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.BonusForExitingEnergeticSieve,
			FamilyAllowanceFund = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.CafMsaFinancing,
			GuidedPathwayBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovGuidedPath,
			LeroyMerlinFoundation =
				accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.LeroyMerlinFoundation,
			HouseholdMaximumSavingAmountForRenovationProject =
				accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
					.HouseholdMaximumSavingAmountForRenovationProject,
			MaximumAmountSupportFamilyMembersRenovationProject =
				accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
					.OtherFamilyMemberMaximumSupportAmountForRenovationProject,
			Municipality = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids,
			PensionFund = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.PensionFund,
			PublicEstablishmentsIntercommunalCooperation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.PublicEstablishmentsForInterCommunalCooperationAids,
			Region = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.RegionalAids,
			SocialProtectionGroup =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup,
			StopEnergyExclusionFunds = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.StopEnergyExclusionFunds,
			WattForChangeFoundation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.WattForChangeFoundation,
			FundingModes = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.FundingModes
						.Select(fm => new FundingModeDto(fm.Id, fm.Label, fm.Value)).ToList(),
			IsImported = accompanyingFile.ImportRunId != null,
			ReportingStructureName = accompanyingFile.AccompanyingFileSupportTeamNavigation?.SolidarBuilderNavigation?.ReportingStructureNavigation?.Name,
			OverallStartDate = accompanyingFile.SiteSupervision?.OverallStartDate,
			EstimatedOverallCompletionDate = accompanyingFile.SiteSupervision?.EstimatedOverallCompletionDate,
			ActualOverallEndDate = accompanyingFile.SiteSupervision?.ActualOverallEndDate,
			OverallProgress = accompanyingFile.SiteSupervision?.OverallProgress,
			NextCoordinationMeetingScheduledFor = accompanyingFile.SiteSupervision?.NextCoordinationMeetingScheduledFor,
			OverallObservations = accompanyingFile.SiteSupervision?.OverallObservations,
			PreSiteSupervisionMeetingDate = accompanyingFile.SiteSupervision?.PreSiteSupervisionMeetingDate,
			WorkParticipants = GetWorkParticipants(accompanyingFile),
			ShouldChangeFinalEstimatedDpe = GetWorkMonitoringValue(accompanyingFile, wm => wm.ShouldChangeFinalEstimatedDpe),
			InitialDpe = (DpeLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe,
			EstimatedDpe = (DpeLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeafterWork,
			FinalDpe = (DpeLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpe ?? (DpeLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeafterWork,
			FinalDpeClassJump = accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpeClassJump,
			AnahGrantDate = accompanyingFile.AnahGrantDate
		};
	}

	private static T? GetWorkMonitoringValue<T>(
		Domain.Entity.AccompanyingFile accompanyingFile,
		Func<WorkMonitoring, T?> selector)
	{
		return IsWorkMonitoringNotNull(accompanyingFile)
			? selector(accompanyingFile.AccompanyingFileWorkMonitoringNavigation!)
			: default;
	}

	private static List<WorkParticipantDto> GetWorkParticipants(Domain.Entity.AccompanyingFile accompanyingFile) =>
		accompanyingFile.SiteSupervision?.WorkParticipants.Select(wp =>
			new WorkParticipantDto(
				wp.Id,
				(ParticipantType?)wp.ParticipantType,
				wp.WorkTypesLabel,
				wp.ParticipantName,
				wp.ContactAdvisor,
				wp.StartDateOfWork,
				wp.EstimatedCompletionDate,
				wp.ActualEndDate,
				wp.Progress,
				(WorkQuality?)wp.WorkQuality,
				wp.CommentOnWorkQuality,
				[.. wp.WorkParticipantDifficulties.Select(wpd => wpd.DifficultyId)],
				wp.CommentOnWorkParticipantDifficulties,
				wp.SpecificComments)).ToList() ?? [];

	private static bool IsWorkMonitoringNotNull(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return accompanyingFile.AccompanyingFileWorkMonitoringNavigation != null;
	}
}