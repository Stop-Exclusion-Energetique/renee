using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.DTOs.SiteSupervision;
using Renee.Domain.Enums;

namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult
{
	public string Reference { get; init; } = null!;
	public bool IsInTzeeProgram { get; init; }
	public AccompanyingFileStatus Status { get; init; }

	// ProjectCost
	public List<InvoiceQueryObjectResult> Invoices { get; set; } = [];
	public double? AccompanyingCost { get; set; }
	public double? HouseholdAutoFinancing { get; set; }

	//FinalFinancingPlan
	public double? GuidedPathwayBonus { get; init; }
	public double? CoOwnershipBonus { get; init; }
	public double? DecentHousingBonus { get; init; }
	public double? AdaptationBonus { get; init; }
	public double? ExitEnergySieveBonus { get; init; }
	public double? Region { get; init; }
	public double? Department { get; init; }
	public double? PublicEstablishmentsIntercommunalCooperation { get; init; }
	public double? Municipality { get; init; }
	public string? BankLoanType { get; init; }
	public double? ClassicBankLoan { get; init; }
	public double? DepartmentalHouseForDisabledPersons { get; init; }
	public double? EnergySavingCertificates { get; init; }
	public double? FamilyAllowanceFund { get; init; }
	public double? PensionFund { get; init; }
	public double? UnderprivilegedHousingFoundation { get; init; }
	public double? LeroyMerlinFoundation { get; init; }
	public double? WattForChangeFoundation { get; init; }
	public double? SocialProtectionGroup { get; init; }
	public double? StopEnergyExclusionFunds { get; init; }
	public double? HouseholdMaximumSavingAmountForRenovationProject { get; init; }
	public double? MaximumAmountSupportFamilyMembersRenovationProject { get; init; }
	public List<FundingModeDto>? FundingModes { get; init; }

	// WorkSummary
	public string? IntermediateAirtightnessTestResult { get; set; }
	public string? WaterproofingTreatmentActions { get; set; }
	public bool? HasEffectiveComplianceWithWorkRecommendations { get; set; }
	public bool? HasWorkEnablingHomeSupport { get; set; }
	public bool? HasHousingAdaptationWorks { get; set; }
	public bool? HasFinishingWorks { get; set; }
	public bool? HasSafetyWorks { get; set; }
	public bool? HasPreparationWorks { get; set; }
	public bool? HasEmergencyWorks { get; set; }
	public bool? HasUnsanitaryExit { get; set; }
	public PartlyStateTreatment? TreatedAirTightness { get; set; }
	public PartlyStateTreatment? TreatedThermalBridges { get; set; }
	public bool? HasHumidityManagement { get; set; }
	public bool? ShouldChangeFinalEstimatedDpe { get; set; }
	public DpeLabel? InitialDpe { get; set; }
	public DpeLabel? EstimatedDpe { get; set; }
	public DpeLabel? FinalDpe { get; set; }
	public int? FinalDpeClassJump { get; set; }

	// Evaluations
	public int? WellBeingRating { get; set; }
	public int? EducationalFrameworkRating { get; set; }
	public int? FamilySatisfactionWithSupport { get; set; }
	public bool? IsBackToEmployment { get; set; }

	// SiteSupervision
	public DateTime? OverallStartDate { get; set; }
	public DateTime? EstimatedOverallCompletionDate { get; set; }
	public DateTime? ActualOverallEndDate { get; set; }
	public double? OverallProgress { get; set; }
	public DateTime? NextCoordinationMeetingScheduledFor { get; set; }
	public string? OverallObservations { get; set; }
	public DateTime? AnahGrantDate { get; set; }
	public DateTime? PreSiteSupervisionMeetingDate { get; set; }
	public List<WorkParticipantDto> WorkParticipants { get; set; } = [];

	// ProjectEnd
	public DateTime? EndOfAccompanyingDate { get; set; }
	public DateTime? EndOfEncounterDate { get; set; }
	public DateTime? StartOfAccompanyingDate { get; set; }
	public AccompanyingTimeDuration? AccompanyingTimeDuration { get; set; }
	public string? ReportingStructureName { get; set; }
	public bool? IsImported { get; set; }
}

public class InvoiceQueryObjectResult
{
	public Guid Id { get; init; }
	public double? TotalCost { get; set; }
	public double? BilledWorkForce { get; set; }
}