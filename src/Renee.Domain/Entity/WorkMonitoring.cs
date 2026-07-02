namespace Renee.Domain.Entity;

public class WorkMonitoring
{
	public Guid Id { get; set; }

	public double? AccompanyingCost { get; set; }

	public double? HouseholdSelfFinancing { get; set; }

	public string? IntermediateAirtightnessTestResult { get; set; }

	public string? JustificationAndActionsPutInPlaceIfNoTest { get; set; }

	public bool? HasEffectiveComplianceWithWorkRecommendations { get; set; }

	public bool? HasWorksEnabledHouseholdToStayAtHome { get; set; }

	public int? WellBeingRating { get; set; }

	public int? EducationalFrameworkRating { get; set; }

	public int? FamilySatisfaction { get; set; }

	public bool? ReturnToEmployment { get; set; }

	public bool? HasHousingAdaptationWorks { get; set; }

	public bool? HasFinishingWorks { get; set; }

	public bool? HasSafetyWorks { get; set; }

	public bool? HasPreparationWorks { get; set; }

	public bool? HasEmergencyWorks { get; set; }

	public bool? HasUnsanitaryExit { get; set; }

	public int? TreatedAirTightness { get; set; }

	public int? TreatedThermalBridges { get; set; }

	public bool? HasHumidityManagement { get; set; }

	public double? WorkTotalCost { get; set; }

	public bool? ShouldChangeFinalEstimatedDpe { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();
}