using Renee.Domain.Enums;

namespace Renee.Application.DTOs.Occupant;

public sealed class HousingDto
{
	public Guid Id { get; set; }

	public AddressDto? Address { get; init; }

	public string? ArchitecturalOrUrbanismStandard { get; init; }

	public bool? IsAbfZone { get; init; }

	public HousingYearConstruction? BuildingYear { get; init; }

	public double? LivingSpaceInSquareMeter { get; init; }

	public int? NumberOfRooms { get; init; }

	public int? NumberOfFloors { get; init; }

	public int? AcquisitionYear { get; init; }

	public string? CadastralReference { get; init; }

	public double? AnnualEnergyConsumption { get; init; }

	public double? AnnualGesEmissions { get; init; }

	public bool? HasPreviousBuildingWork { get; init; }

	public string? ExplanationOnPreviousBuildingWork { get; init; }

	public Guid? CopropertyProfileId { get; init; }

	public string? CopropertyProfileReference { get; init; }

	public DegradationIndex? DegradationIndex { get; init; }

	public UnsanitaryCoefficient? UnsanitaryCoefficient { get; init; }

	public EnergyDeprivation? ElectricityDeprivation { get; init; }

	public ComfortLevel? SummerThermalComfortLevel { get; init; }

	public ComfortLevel? WinterThermalComfortLevel { get; init; }

	public ComfortLevel? NoiseComfortLevel { get; init; }

	public OwnershipStatus? OwnershipStatus { get; init; }

	public HousingType? HousingType { get; init; }

	public GeographicalHousingAreaTypology? GeographicalTypology { get; init; }

	public DpeLabel? DpeLabel { get; init; }
	public GesLabel? GesLabel { get; init; }
}