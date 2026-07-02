using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveAccompanyingFileRealizeAndFollowCommandInput(
	UpdatedProjectCost ProjectCost,
	List<UpdatedInvoice> Invoices,
	UpdatedWorkSummary WorkSummary,
	UpdatedEvaluation Evaluation,
	UpdatePreFinancingPlan UpdatedFinancingPlan,
	UpdatedSiteSupervision UpdatedSiteSupervision,
	UpdateHousingAfterWorkStateForRealizeAndFollowMilestone UpdatedHousingAfterWorkState,
	DateTime? EndOfAccompaniementDate,
	DateTime? EndOfFollowingDate,
	AccompanyingTimeDuration? AccompanyingTimeDuration,
	DateTime? AnahGrantDate,
	Guid AccompanyingFileId,
	Guid ConnectedUserId) : IRequest<ReneeOperationResult<bool>>
{
	public WorkMonitoring CreateWorkMonitoring()
	{
		return new WorkMonitoring
		{
			AccompanyingCost = ProjectCost.AccompanyingCost,
			WorkTotalCost = ProjectCost.WorkTotalCost,
			HouseholdSelfFinancing = ProjectCost.HouseholdAutoFinancing,
			IntermediateAirtightnessTestResult = WorkSummary.IntermediateAirtightnessTestResult,
			JustificationAndActionsPutInPlaceIfNoTest = WorkSummary.WaterproofingTreatmentActions,
			HasEffectiveComplianceWithWorkRecommendations = WorkSummary.HasEffectiveComplianceWithWorkRecommendations,
			HasWorksEnabledHouseholdToStayAtHome = WorkSummary.HasWorkEnablingHomeSupport,
			WellBeingRating = Evaluation.WellBeing,
			EducationalFrameworkRating = Evaluation.EducationnalFramework,
			FamilySatisfaction = Evaluation.FamilySatisfaction,
			ReturnToEmployment = Evaluation.IsBackToEmployment,
			HasHousingAdaptationWorks = WorkSummary.HasHousingAdaptationWorks,
			HasFinishingWorks = WorkSummary.HasFinishingWorks,
			HasSafetyWorks = WorkSummary.HasSafetyWorks,
			HasPreparationWorks = WorkSummary.HasPreparationWorks,
			HasEmergencyWorks = WorkSummary.HasEmergencyWorks,
			HasUnsanitaryExit = WorkSummary.HasUnsanitaryExit,
			TreatedAirTightness = WorkSummary.TreatedAirTightness,
			TreatedThermalBridges = WorkSummary.TreatedThermalBridges,
			HasHumidityManagement = WorkSummary.HasHumidityManagement,
			ShouldChangeFinalEstimatedDpe = WorkSummary.ShouldChangeFinalEstimatedDpe
		};
	}

	public SiteSupervision CreateSiteSupervision()
	{
		return new SiteSupervision
		{
			OverallStartDate = UpdatedSiteSupervision.OverallStartDate,
			EstimatedOverallCompletionDate = UpdatedSiteSupervision.EstimatedOverallCompletionDate,
			ActualOverallEndDate = UpdatedSiteSupervision.ActualOverallEndDate,
			OverallProgress = UpdatedSiteSupervision.OverallProgress,
			NextCoordinationMeetingScheduledFor = UpdatedSiteSupervision.NextCoordinationMeetingScheduledFor,
			OverallObservations = UpdatedSiteSupervision.OverallObservations,
			PreSiteSupervisionMeetingDate = UpdatedSiteSupervision.PreSiteSupervisionMeetingDate,
			WorkParticipants = [.. UpdatedSiteSupervision.UpdatedWorkParticipants.Select(
				uwp => uwp.CreateUpdatedWorkParticipant())]
		};
	}

	public List<Invoice> GetUpdatedInvoices()
	{
		return [.. Invoices.Select(
			invoice => new Invoice
			{
				Id = invoice.InvoiceId ?? Guid.NewGuid(),
				InvoiceCost = invoice.InvoiceCost,
				LaborCost = invoice.LaborCost
			})];
	}

	public List<FundingMode> GetUpdatedFundingModes() =>
		[.. UpdatedFinancingPlan.FundingModes.Select(ufm => ufm.CreateUpdateFundingMode())];

	public List<WorkParticipant> GetUpdatedWorkParticipants() =>
		[.. UpdatedSiteSupervision.UpdatedWorkParticipants.Select(uwp => uwp.CreateUpdatedWorkParticipant())];
}

public record UpdatedProjectCost(double? AccompanyingCost, double? HouseholdAutoFinancing, double? WorkTotalCost);

public record UpdatedInvoice(double? InvoiceCost, double? LaborCost, Guid? InvoiceId);

public record UpdatedWorkSummary(
	string? IntermediateAirtightnessTestResult,
	string? WaterproofingTreatmentActions,
	bool? HasEffectiveComplianceWithWorkRecommendations,
	bool? ShouldChangeFinalEstimatedDpe,
	bool? HasWorkEnablingHomeSupport,
	bool? HasHousingAdaptationWorks,
	bool? HasFinishingWorks,
	bool? HasSafetyWorks,
	bool? HasPreparationWorks,
	bool? HasEmergencyWorks,
	bool? HasUnsanitaryExit,
	int? TreatedAirTightness,
	int? TreatedThermalBridges,
	bool? HasHumidityManagement);

public record UpdatedEvaluation(
	int? WellBeing,
	int? EducationnalFramework,
	int? FamilySatisfaction,
	bool? IsBackToEmployment);

public record UpdatedSiteSupervision(
	DateTime? OverallStartDate,
	DateTime? EstimatedOverallCompletionDate,
	DateTime? ActualOverallEndDate,
	double? OverallProgress,
	DateTime? NextCoordinationMeetingScheduledFor,
	string? OverallObservations,
	DateTime? PreSiteSupervisionMeetingDate,
	List<UpdatedWorkParticipant> UpdatedWorkParticipants);

public record UpdatedWorkParticipant(
	Guid Id,
	ParticipantType? ParticipantType,
	string? WorkTypesLabel,
	string? ParticipantName,
	string? ContactAdvisor,
	DateTime? StartDateOfWork,
	DateTime? EstimatedCompletionDate,
	DateTime? ActualEndDate,
	double? Progress,
	WorkQuality? WorkQuality,
	string? CommentOnWorkQuality,
	List<Guid?> UpdatedWorkParticipantDifficulties,
	string? CommentOnWorkParticipantDifficulties,
	string? SpecificComments)
{
	public WorkParticipant CreateUpdatedWorkParticipant()
	{
		return new WorkParticipant
		{
			Id = Id,
			ParticipantType = (int?)ParticipantType,
			WorkTypesLabel = WorkTypesLabel,
			ParticipantName = ParticipantName,
			ContactAdvisor = ContactAdvisor,
			StartDateOfWork = StartDateOfWork,
			EstimatedCompletionDate = EstimatedCompletionDate,
			ActualEndDate = ActualEndDate,
			Progress = Progress,
			WorkQuality = (int?)WorkQuality,
			CommentOnWorkQuality = CommentOnWorkQuality,
			WorkParticipantDifficulties = [.. UpdatedWorkParticipantDifficulties.Select(
				difficultyId => new WorkParticipantDifficulty
				{
					DifficultyId = difficultyId ?? Guid.Empty
				})],
			CommentOnWorkParticipantDifficulties = CommentOnWorkParticipantDifficulties,
			SpecificComments = SpecificComments
		};
	}
}

public record UpdateHousingAfterWorkStateForRealizeAndFollowMilestone(
	DpeLabel? FinalDpe,
	int? FinalDpeClassJump);