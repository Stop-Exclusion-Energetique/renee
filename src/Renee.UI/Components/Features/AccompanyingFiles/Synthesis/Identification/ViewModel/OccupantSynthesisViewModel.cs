using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.ViewModel;

public class OccupantSynthesisViewModel
{
	public Guid Id { get; set; }
	public string Trigram { get; set; } = null!;
	public string? Name { get; set; }
	public string? FirstName { get; set; }
	public string? PhoneNumber { get; set; }
	public int? Age { get; set; }
	public string? SocioProfessionalCategory { get; set; }
	public int NumberOfOccupants { get; set; }
	public string? HouseholdTypology { get; set; }
	public string HouseholdDifficulties { get; set; } = null!;
	public int? TaxIncome { get; set; }
	public string? AnahCategory { get; set; }
	public double? AvailableBudget { get; set; }
	public double? EnergeticTotal { get; set; }
	public double? EnergyEffortRate { get; set; }
	public string Address { get; set; } = null!;
	public string PostalCode { get; set; } = null!;
	public string City { get; set; } = null!;
	public string? HousingType { get; set; }
	public string? OwnershipStatus { get; set; }
	public double? LivingSpaceInSquareMeter { get; set; }
	public string? BuildingYear { get; set; }
	public string? DegradationIndex { get; set; }
	public string? UnsanitaryCoefficient { get; set; }
	public string? MarClassification { get; set; }
	public string? DpeLabel { get; set; }
	public double? EnergyConsumption { get; set; }
	public string? GesLabel { get; set; }
	public double? GesEmissions { get; set; }
	public string? ElectricityDeprivation { get; set; }
	public string? DifficultiesFacedByFamily { get; set; }
	public string? AccompanyingFileReference { get; set; }
	public AccompanyingFileStage Stage { get; set; }
	public AccompanyingFileStatus Status { get; set; }
	public string? MarkerNature { get; set; }
	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? DiffuseCoordinator { get; set; }
	public Guid? TargetedCoordinator { get; set; }
	public Guid? TerritorialBuilder { get; set; }
	public Guid? SecondTerritorialBuilder { get; set; }
	public GeographicalHousingAreaTypology? GeographicAreaTypology { get; set; }
}