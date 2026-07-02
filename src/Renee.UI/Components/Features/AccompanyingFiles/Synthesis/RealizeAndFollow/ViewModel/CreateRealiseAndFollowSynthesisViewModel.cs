using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.ViewModel;

public class CreateRealiseAndFollowSynthesisViewModel
{
	public Guid Id { get; init; }
	public string AccompaniyingFileReference { get; set; } = null!;
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }

	public double? AccompanyingCost { get; set; }
	public double? HouseholdAutoFinancing { get; set; }
	public Dictionary<double, double> InvoicesSummary { get; set; } = [];

	public string? IntermediateAirtightnessTestResult { get; set; }
	public string? WaterproofingTreatmentActions { get; set; }
	public bool? HasEffectiveComplianceWithWorkRecommendations { get; set; }
	public bool? HasWorkEnablingHomeSupport { get; set; }

	public int? WellBeingRating { get; set; }
	public int? EducationalFrameworkRating { get; set; }
	public int? FamilySatisfactionWithSupport { get; set; }
	public bool? IsBackToEmployment { get; set; }

	public DateTime? EndOfAccompanyingDate { get; set; }
	public string? AccompanyingTime { get; set; }
	public DateTime? EndOfEncounterDate { get; set; }
	public string? AccompanyingTimeDuration { get; set; }
	public int? OtherAccompanyingTimeDuration { get; set; }

	public bool IsInTzeeProgram { get; set; }
	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? DiffuseCoordinator { get; set; }
	public Guid? TargetedCoordinator { get; set; }
	public Guid? TerritorialBuilder { get; set; }
	public Guid? SecondTerritorialBuilder { get; set; }
}