using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.DTOs.WorkPackage;
using Renee.Domain.Enums;

namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult
{
	public string Reference { get; init; } = null!;
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }
	public bool IsInTzeeProgram { get; init; }
	public DateTime? StartOfAccompanyingDate { get; init; }

	// HousingInitialState
	public int? RoomCounter { get; init; }
	public int? DoorCounter { get; init; }
	public int? WindowCounter { get; init; }
	public int? PatioDoorCounter { get; init; }
	public int? RoofWindowCounter { get; init; }
	public int? BayWindowCounter { get; init; }
	public double? CeilingHeight { get; init; }
	public SunExposure? SunExposure { get; init; }
	public bool? HasPestOrMold { get; init; }
	public bool? HasFaultyElectricalSystem { get; init; }
	public bool? HasVentilationSystem { get; init; }
	public bool? HasHeatingSystem { get; init; }
	public bool? HasHotWaterProduction { get; init; }
	public bool? HasOpenings { get; init; }
	public bool? HasInsulation { get; init; }
	public bool? HasHousingCover { get; init; }
	public string? DisordersObservedCommentary { get; init; }
	public DpeLabel? InitialDpeLabel { get; init; }

	// PreWorkPlan
	public List<Guid>? PreWorkPlanProjectTypes { get; init; }
	public RenovationType? RenovationType { get; init; }
	public string? NextStepAndVigilancePoints { get; init; }
	public bool? NeedTemporaryRehousingSolution { get; init; }
	public bool? InterestInPossibleSupportedSelfRehabilitationAra { get; init; }
	public List<Guid>? PreWorkPlanInsuranceTypes { get; init; }
	public bool? AraOpeningStatementSent { get; init; }
	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; init; }
	public double? EstimatedAnnualEnergyConsumptionBeforeWork { get; init; }
	public double? EstimatedAnnualGhgEmissionsAfterWork { get; init; }
	public double? EstimatedAnnualGhgEmissionsBeforeWork { get; init; }
	public DpeLabel? EstimatedEnergyDpeAfterWork { get; init; }
	public GesLabel? EstimatedEnergyGesAfterWork { get; init; }
	public int? EstimatedEnergyClassJump { get; init; }
	public bool? IsEmergencyWorks { get; init; }
	public bool? IsEnergeticsRenovationWorks { get; init; }
	public bool? IsInducedWorks { get; init; }
	public bool? IsSafetyAndHealthWorks { get; init; }
	public PartlyStateTreatment? TreatedAirTightness { get; init; }
	public PartlyStateTreatment? TreatedThermalBridge { get; init; }
	public PartlyStateTreatment? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; init; }
	public bool? IsRgeLabelUpToDate { get; init; }
	public string? OtherQualification { get; init; }
	public List<WorkPackageDto>? WorkPackages { get; init; }
	public List<WorkPackageDto>? CopropertyWorkPackages { get; init; }

	// PreFinancingPlan
	public DateTime? DateOfAgVote { get; init; }
	public double? MprCoproAids { get; init; }
	public double? ComplementaryCopropertyAids { get; init; }
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

	// SupportedSelfRehabilitation
	public bool? IsFamilyReadyForSupportedSelfRehabilitationApproach { get; init; }
	public bool? FamilyPhysicalcCapabilitiesHaveBeenTakenIntoAccount { get; init; }
	public bool? FamilyCanMobilizeSocialCircleOnConstructionSite { get; init; }
	public string? WorkDetails { get; init; }
	public string? FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite { get; init; }
	public AccompanyingTimeDuration? AccompanyingTimeDuration { get; set; }
	public bool? IsImported {  get; set; }
	public string? ReportingStructureName { get; set; }
	public string? AnahFolderNumber { get; set; }
	public DateTime? AnahFolderFilingDate { get; set; }
}