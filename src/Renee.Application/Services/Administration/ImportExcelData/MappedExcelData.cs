using Renee.Domain.Enums;

namespace Renee.Application.Services.Administration.ImportExcelData;

public interface IMappedData
{
}

public record IdentificationMappedData(
	string Reference,
	DateTime FirstContactDate,
	DateTime StartSupportDate,
	string StreetNumberName,
	string PostalCode,
	string Municipality,
	string Department,
	string Region) : IMappedData
{
	public string Reference { get; } = Reference;
	public MarkerNature MarkerNature { get; set; }
	public string? CommentOnMarkerNature { get; set; }
	public DateTime FirstContactDate { get; set; } = FirstContactDate;
	public DateTime StartSupportDate { get; set; } = StartSupportDate;
	public List<ImportOccupantExcelData> Occupants { get; set; } = [];
	public string StreetNumberName { get; } = StreetNumberName;
	public string? AdditionnalAddress { get; set; }
	public string PostalCode { get; } = PostalCode;
	public string Municipality { get; } = Municipality;
	public string Department { get; } = Department;
	public string Region { get; } = Region;
	public GeographicalHousingAreaTypology? GeographicalHousingAreaTypology { get; set; }
	public HouseholdTypology? HouseholdTypology { get; set; }
	public OwnershipStatus? OwnershipStatus { get; set; }
	public double? IncomeTaxReference { get; set; }
	public string? AnahCategory { get; set; }
	public EnergyDeprivation? EnergyDeprivation { get; set; }
	public HousingType? HousingType { get; set; }
	public int? ConstructionYear { get; set; }
	public int? LivingSpace { get; set; }
	public UnsanitaryCoefficient? UnsanitaryCoefficient { get; set; }
	public DegradationIndex? DegradationIndex { get; set; }
	public DpeLabel? StartingDpe { get; set; }
	public GesLabel? StartingGes { get; set; }
	public double? AnnualEnergyConsumptionBeforeWork { get; set; }
	public bool? HasOverdueInvoice { get; set; }
	public string? SocialContext { get; set; }
	public int? ContactWithFamilyForIdentificationMilestones { get; set; }
}

public record OrganizeAndFinanceMappedData : IMappedData
{
	public List<Guid>? ProjectTypes { get; set; }
	public RenovationType? RenovationType { get; set; }
	public string? NextStepAndVigilancePoints { get; set; }
	public bool? IsEmergencyWorks { get; set; }
	public PartlyStateTreatment? TreatedAirTightness { get; set; }
	public PartlyStateTreatment? TreatedThermalBridge { get; set; }
	public PartlyStateTreatment? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; set; }

	public DpeLabel? EstimatedDpeLabelAfterWork { get; set; }
	public GesLabel? EstimatedGesLabelAfterWork { get; set; }
	public double? EstimatedEnergyConsumptionAfterWork { get; set; }
	public int? EstimatedDpeJumpClass { get; set; }

	public double? RegionAids { get; set; }
	public double? DepartmentAids { get; set; }
	public double? PublicEstablishmentsIntercommunalCooperation { get; set; }
	public double? MunicipalityAids { get; set; }
	public double? PrivateActors { get; set; }
	public double? PensionFunds { get; set; }
	public double? HouseholdMaximumSavingAmountForRenovationProject { get; set; }
	public double? MaximumAmountSupportFamilyMembersRenovationProject { get; set; }
	public double? EstimatedRemainingAmount { get; set; }
	public int? ContactWithFamilyForOrganizeAndFinanceMilestone { get; set; }
}

public record RealizeAndFollowMappedData(
	double? TotalCost,
	double? BilledWorkForce,
	double? HouseholdAutoFinancing,
	string? IntermediateAirtightnessTestResult,
	string? WaterproofingTreatmentActions,
	bool? HasEffectiveComplianceWithWorkRecommendations,
	bool? HasWorkEnablingHomeSupport,
	int? WellBeingRating,
	int? EducationalFrameworkRating,
	int? FamilySatisfactionWithSupport,
	bool? IsBackToEmployment,
	DateTime? EndOfAccompanyingDate,
	DateTime? EndOfEncounterDate,
	int? ContactWithFamilyForRealizeAndFollowMilestone
) : IMappedData
{
	public double? TotalCost { get; set; } = TotalCost;
	public double? BilledWorkForce { get; set; } = BilledWorkForce;
	public double? HouseholdAutoFinancing { get; set; } = HouseholdAutoFinancing;
	public string? IntermediateAirtightnessTestResult { get; set; } = IntermediateAirtightnessTestResult;
	public string? WaterproofingTreatmentActions { get; set; } = WaterproofingTreatmentActions;
	public bool? HasEffectiveComplianceWithWorkRecommendations { get; set; } = HasEffectiveComplianceWithWorkRecommendations;
	public bool? HasWorkEnablingHomeSupport { get; set; } = HasWorkEnablingHomeSupport;
	public int? WellBeingRating { get; set; } = WellBeingRating;
	public int? EducationalFrameworkRating { get; set; } = EducationalFrameworkRating;
	public int? FamilySatisfactionWithSupport { get; set; } = FamilySatisfactionWithSupport;
	public bool? IsBackToEmployment { get; set; } = IsBackToEmployment;
	public DateTime? EndOfAccompanyingDate { get; set; } = EndOfAccompanyingDate;
	public DateTime? EndOfEncounterDate { get; set; } = EndOfEncounterDate;
	public int? ContactWithFamilyForRealizeAndFollowMilestone { get; set; } = ContactWithFamilyForRealizeAndFollowMilestone;
}

public record RealizeAndFollowMappedDataV3() : IMappedData
{
	public double? TotalCost { get; set; }
	public double? BilledWorkForce { get; set; }
	public double? HouseholdAutoFinancing { get; set; } 
	public string? IntermediateAirtightnessTestResult { get; set; }
	public string? WaterproofingTreatmentActions { get; set; }
	public bool? HasEffectiveComplianceWithWorkRecommendations { get; set; }
	public bool? HasWorkEnablingHomeSupport { get; set; } 
	public int? WellBeingRating { get; set; }
	public int? EducationalFrameworkRating { get; set; }
	public int? FamilySatisfactionWithSupport { get; set; } 
	public bool? IsBackToEmployment { get; set; }
	public DateTime? EndOfAccompanyingDate { get; set; }
	public DateTime? EndOfEncounterDate { get; set; }
	public int? ContactWithFamilyForRealizeAndFollowMilestone { get; set; } 
	public bool? HasHousingAdaptationWorks { get; set; }
	public bool? HasFinishingWorks { get; set; }
	public bool? HasSafetyWorks { get; set; }
	public bool? HasPreparationWorks { get; set; }
	public bool? HasEmergencyWorks { get; set; }
	public bool? HasUnsanitaryExit { get; set; }
	public PartlyStateTreatment? TreatedAirTightness { get; set; }
	public PartlyStateTreatment? TreatedThermalBridges { get; set; }
	public bool? HasHumidityManagement { get; set; }
	
}