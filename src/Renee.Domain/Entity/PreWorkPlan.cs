namespace Renee.Domain.Entity;

public class PreWorkPlan
{
	public Guid Id { get; set; }

	public int? RenovationType { get; set; }

	public string? NextStepAndVigilancePoint { get; set; }

	public bool? HasInterestInPossibleAraprocess { get; set; }

	public bool? HasNeedForTemporaryReHousing { get; set; }

	public bool? HasEmergencyWorks { get; set; }

	public bool? HasEnergeticsRenovationWorks { get; set; }

	public bool? HasInducedWorks { get; set; }

	public bool? HasSafetyAndHealthWorks { get; set; }

	public int? TreatedAirTightness { get; set; }

	public int? TreatedThermalBridge { get; set; }

	public int? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; set; }

	public bool? IsAraopeningStatementSent { get; set; }

	public bool? IsHouseholdReadyToStartAraprocess { get; set; }

	public bool? AreHouseholdPhysicalCapacitiesTakenIntoAccount { get; set; }

	public bool? DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean { get; set; }

	public string? WorksDetails { get; set; }

	public string? HouseholdAvailabilitiyToOrganizeArasite { get; set; }

	public bool? IsRgeLabelUpToDate { get; set; }

	public string? OtherQualification { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();

	public virtual ICollection<PreWorkPlanInsuranceType> PreWorkPlanInsuranceTypes { get; set; } =
		new List<PreWorkPlanInsuranceType>();

	public virtual ICollection<PreWorkPlanProjectType> PreWorkPlanProjectTypes { get; set; } =
		new List<PreWorkPlanProjectType>();

	public virtual ICollection<WorkPackage> WorkPackages { get; set; } = new List<WorkPackage>();
}