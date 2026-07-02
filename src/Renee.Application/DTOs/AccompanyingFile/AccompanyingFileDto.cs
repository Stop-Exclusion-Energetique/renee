using Renee.Application.DTOs.AccompanyingFileDifficultyFacedByFamily;
using Renee.Application.DTOs.AccompanyingFileHouseholdHeatingEnergy;
using Renee.Application.DTOs.AccompanyingFileHouseholdResourceTypology;
using Renee.Application.DTOs.Occupant;
using Renee.Domain.Enums;
using ExpenseDto = Renee.Application.DTOs.Expense.ExpenseDto;

namespace Renee.Application.DTOs.AccompanyingFile;

public class AccompanyingFileDto
{
	public Guid Id { get; init; }
	public HousingDto Housing { get; init; } = null!;

	public bool? IsFollowedBySocialWorker { get; init; }

	public bool? HasDisabilitySituation { get; init; }

	public bool? HasPersonWithLongTermIllness { get; init; }

	public bool? HasPersonWithLossOfIndependence { get; init; }

	public bool? HasPersonFollowedByCuratorship { get; init; }

	public bool? HasPersonFollowedByGuardianship { get; init; }

	public bool? HasUnpaidEnergyBills { get; init; }

	public int? TaxIncome { get; init; }

	public string? SocialContext { get; init; }

	public string? CommentOnDifficultiesFacedByFamily { get; init; }

	public string? FamilyProject { get; init; }

	public string? FamilyAvailabilityForVisits { get; init; }

	public string? Reference { get; init; }

	public AccompanyingFileStage Stage { get; init; }

	public AccompanyingFileRiskType RiskType { get; set; }
	public string? AnahCategory { get; init; }

	public HouseholdTypology? HouseholdTypologyId { get; init; }
	public Guid EnsemblierTerritorialUserId { get; set; }

	public Guid EnsemblierSolidaireUserId { get; set; }
	public DateTime CreationDatetimeUtc { get; set; }

	public DateTime LastUpdateDatetimeUtc { get; set; }

	public DateTime? ClosedDatetimeUtc { get; set; }

	public AccompanyingFileStatus Status { get; set; }

	public string? BlockingProofComment { get; set; }

	public AccompanyingTimeDuration? AccompanyingTimeDuration { get; set; }

	public MainOccupantDto MainOccupant { get; init; } = null!;
	public List<SecondaryOccupantDto> SecondaryOccupants { get; init; } = [];
	public List<ExpenseDto> Expenses { get; init; } = [];
	public List<AccompanyingFileDifficultyFacedByFamilyDto> DifficultiesFacedByFamily { get; init; } = [];
	public List<AccompanyingFileHouseholdResourcesTypologyDto> HouseholdResourcesTypologies { get; init; } = [];
	public List<AccompanyingFileHouseholdHeatingEnergyDto> HouseholdHeatingEnergies { get; init; } = [];
	public DateTime? FirstVisitDate { get; init; }
	public DateTime? SigningHouseholdSupportDate { get; init; }
	public bool IsInTzeeProgram { get; init; }
	public string? ReportingStructureName { get; set; }
	public bool? IsImported { get; set; }
	public bool? ShouldAccompanyingFileBeSubmittedToAnah { get; set; }
}