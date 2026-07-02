using Renee.Application.DTOs.AccompanyingFileHouseholdResourceTypology;
using Renee.Domain.Enums;

namespace Renee.Application.DTOs.AccompanyingFile;

public class AccompanyingFileSynthesisDto
{
	public Guid Id { get; init; }
	public string Trigram { get; init; } = null!;
	public string? Name { get; set; }
	public string? FirstName { get; set; }
	public string? PhoneNumber { get; init; }
	public int? Age { get; init; }
	public SocioProfessionalCategory? SocioProfessionalCategory { get; init; }
	public int NumberOfOccupants { get; init; }
	public HouseholdTypology? HouseholdTypology { get; init; }
	public bool IsOverIndebted { get; init; }
	public bool? HasDisabilitySituation { get; init; }
	public bool? HasPersonWithLongTermIllness { get; init; }
	public bool? HasPersonWithLossOfIndependence { get; init; }
	public bool? HasPersonFollowedByCuratorship { get; init; }
	public bool? HasPersonFollowedByGuardianship { get; init; }
	public int? TaxIncome { get; init; }
	public string? AnahCategory { get; init; }
	public double? AvailableBudget { get; init; }
	public double? EnergeticTotal { get; init; }
	public double? EnergyEffortRate { get; init; }
	public string Address { get; init; } = null!;
	public string PostalCode { get; init; } = null!;
	public string City { get; init; } = null!;
	public HousingType? HousingType { get; init; }
	public OwnershipStatus? OwnershipStatus { get; init; }
	public double? LivingSpaceInSquareMeter { get; init; }
	public string? BuildingYear { get; init; }
	public DegradationIndex? DegradationIndex { get; init; }
	public UnsanitaryCoefficient? UnsanitaryCoefficient { get; init; }
	public string? Mar { get; init; }
	public string? DpeLabel { get; init; }
	public double? EnergyConsumption { get; init; }
	public string? GesLabel { get; init; }
	public double? GesEmissions { get; init; }
	public EnergyDeprivation? ElectricityDeprivation { get; init; }
	public List<string> DifficultiesFacedByFamily { get; init; } = null!;
	public string? AccompanyingFileReference { get; init; }
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }
	public MarkerNature MarkerNature { get; init; }
	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? DiffuseCoordinator { get; set; }
	public Guid? TargetedCoordinator { get; set; }
	public Guid? TerritorialBuilder { get; set; }
	public Guid? SecondTerritorialBuilder { get; set; }
	public GeographicalHousingAreaTypology? GeographicAreaTypology { get; set; }

	public string? SocialContext { get; init; }
	public string? FamilyProject { get; init; }
	public string? CommentsOnHouseholdDifficulties { get; init; }
	public bool? IsFollowedBySocialWorker { get; init; }
	public List<AccompanyingFileHouseholdResourcesTypologyDto> HouseholdResourcesTypologies { get; init; } = [];
	public List<string> HouseholdHeatingEnergies { get; init; } = [];
}
