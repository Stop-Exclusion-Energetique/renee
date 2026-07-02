using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveAccompanyingFileIdentificationMilestoneCommandInput(
	Guid AccompanyingFileId,
	AccompanyingTimeDuration? AccompanyingTimeDuration,
	UpdatedHousehold UpdatedHousehold,
	UpdatedHouseholdMainOccupant UpdatedHouseholdMainOccupant,
	List<UpdatedHouseholdSecondaryOccupant> UpdatedHouseholdSecondaryOccupants,
	List<Guid?>? UpdatedHouseholdDifficulties,
	List<UpdatedHouseholdExpenses> UpdatedHouseholdExpenses,
	List<UpdatedHouseholdResource> UpdatedHouseholdResources,
	List<UpdatedHouseholdHeatingEnergy> UpdatedHouseholdHeatingEnergy,
	UpdatedHousing UpdatedHousing,
	UpdatedAddress UpdatedAddress,
	UpdatedHousingInitialStateForIdentificationMilestone UpdatedHousingInitialState,
	Guid ConnectedUserId,
	DateTime? StartAccompanyingDate,
	DateTime? FirstEncounterDate,
	int? DeliveryTime,
	bool? ShouldAccompanyingFileBeSubmittedToAnah,
	bool IsSubmission) : IRequest<ReneeOperationResult<bool>>
{
	public List<HouseholdResource> GetUpdatedHouseholdResources() =>
		UpdatedHouseholdResources.Select(uhr => uhr.CreateUpdateHouseholdResource()).ToList();

	public List<HouseholdHeatingEnergy> GetUpdatedHouseholdHeatingEnergy() =>
		UpdatedHouseholdHeatingEnergy.Select(uhr => uhr.CreateUpdateHouseholdHeatintEnergy()).ToList();

	public List<HouseholdExpense> GetUpdatedHouseholdExpenses() =>
		UpdatedHouseholdExpenses.Select(uhe => uhe.CreateUpdateHouseholdExpense()).ToList();

	public List<SecondaryOccupant> GetUpdatedSecondaryOccupants() =>
		UpdatedHouseholdSecondaryOccupants.Select(uo => uo.CreateUpdateSecondaryOccupant()).ToList();
}

public record UpdatedHousehold(
	HouseholdTypology? HouseholdTypology,
	bool? IsFollowedBySocialAnSocialWorker,
	bool? HasAnOccupantWithDisabilities,
	bool? HasAnOccupantWithLongThermIllness,
	bool? HasAnOccupantWithIndependenceLoss,
	bool? HasAnOccupantUnderCuratorShip,
	bool? HasAnOccupantUnderGardianShip,
	float? ReferenceIncomeTax,
	string? AnahCategory,
	string? SocialContext,
	string? HouseholdProject,
	string? HouseholdAvailabilityForVisits,
	string? CommentsOnHouseholdDifficulties,
	bool? HasOverdueInvoice,
	double? EnergyEffortRate)
{
	public Household CreateUpdateHousehold() =>
		new()
		{
			HouseholdTypology = HouseholdTypology is null ? null : (int)HouseholdTypology,
			IsFollowedByAnSocialWorker = IsFollowedBySocialAnSocialWorker,
			HasAnOccupantWithDisabilities = HasAnOccupantWithDisabilities,
			HasAnOccupantWithLongTermIllness = HasAnOccupantWithLongThermIllness,
			HasAnOccupantWithIndependenceLoss = HasAnOccupantWithIndependenceLoss,
			HasAnOccupantUnderCuratorship = HasAnOccupantUnderCuratorShip,
			HasAnOccupantUnderGuardianship = HasAnOccupantUnderGardianShip,
			ReferenceIncomeTax = ReferenceIncomeTax,
			AnahCategory = AnahCategory,
			SocialContext = SocialContext,
			HouseholdProject = HouseholdProject,
			HouseholdAvailabilityForVisits = HouseholdAvailabilityForVisits,
			CommentsOnHouseholdDifficulties = CommentsOnHouseholdDifficulties,
			HasOverdueInvoice = HasOverdueInvoice,
			EnergyEffortRate = EnergyEffortRate
		};
}

public record UpdatedHouseholdMainOccupant(
	Guid? Id,
	string Trigram,
	DateTime? BirthDate,
	int? Age,
	SocioProfessionalCategory? SocioprofessionalCategory,
	string? PhoneNumber,
	string? Email,
	string? Job,
	SocialProtectionFund? SocialProtectionFund,
	string? CommentOnSocialProtectionFund,
	PensionFund? PensionFund,
	string? CommentOnPensionFund,
	AdditionalFund? AdditionnalFund,
	string? CommentOnAdditionnalFund,
	string? FirstName,
	string? LastName)
{
	public MainOccupant CreateUpdateMainOccupant() =>
		new()
		{
			Id = Id ?? Guid.Empty,
			Trigram = Trigram,
			Birthdate = BirthDate,
			Age = Age,
			SocioProfessionalCategory = SocioprofessionalCategory is null ? null : (int)SocioprofessionalCategory,
			PhoneNumber = PhoneNumber,
			Email = Email,
			Job = Job,
			SocialProtectionFund = SocialProtectionFund is null ? null : (int)SocialProtectionFund,
			CommentOnSocialProtectionFund = CommentOnSocialProtectionFund,
			PensionFund = PensionFund is null ? null : (int)PensionFund,
			CommentOnPensionFund = CommentOnPensionFund,
			AdditionnalFund = AdditionnalFund is null ? null : (int)AdditionnalFund,
			CommentOnAdditionnalFund = CommentOnAdditionnalFund,
			FirstName = FirstName,
			LastName = LastName
		};
}

public record UpdatedHouseholdSecondaryOccupant(
	Guid? Id,
	string Trigram,
	DateTime? BirthDate,
	int? Age,
	bool? IsDependent)
{
	public SecondaryOccupant CreateUpdateSecondaryOccupant() =>
		new()
		{
			Id = Id ?? Guid.Empty,
			Trigram = Trigram,
			Birthdate = BirthDate,
			Age = Age,
			IsDependent = IsDependent
		};
}

public record UpdatedHouseholdExpenses(Guid? Id, ExpenseType ExpenseType, double? Value = 0)
{
	public HouseholdExpense CreateUpdateHouseholdExpense() =>
		new() { Id = Id ?? Guid.NewGuid(), Type = (int)ExpenseType, Value = Value };
}

public record UpdatedHouseholdResource(Guid? HouseholdResourceId, double? Value)
{
	public HouseholdResource CreateUpdateHouseholdResource() =>
		new() { HouseholdResources = HouseholdResourceId ?? Guid.Empty, Value = Value ?? 0 };
}

public record UpdatedHouseholdHeatingEnergy(Guid? HouseholdHeatingEnergyId, double? Value)
{
	public HouseholdHeatingEnergy CreateUpdateHouseholdHeatintEnergy() =>
		new() { HouseholdHeatingEnergyLabel = HouseholdHeatingEnergyId ?? Guid.Empty, Value = Value ?? 0 };
}

public record UpdatedHousing(
	GeographicalHousingAreaTypology? GeographicAreaTypology = null!,
	bool? IsInAbfArea = null!,
	string? ArchitecturalOrTownPlanningStandards = null!,
	OwnershipStatus? OwnershipStatus = null!,
	HousingType? HousingType = null!,
    HousingYearConstruction? ConstructionYear = null!,
	double? LivingSpace = null!,
	int? NumberOfRoom = null!,
	int? NumberOfFloor = null!,
	int? YearOfAcquisitionOrEntry = null!,
	string? CadastralReference = null!,
	bool? HasPreviousWork = null!,
	string? CommentOnPreviousWork = null!,
	Guid? CopropertyProfileId = null!)
{
	public Housing CreateUpdateHousing() =>
		new()
		{
			GeographicAreaTypology = GeographicAreaTypology is null ? null : (int)GeographicAreaTypology,
			IsInAbfarea = IsInAbfArea,
			ArchitecturalOrTownPlanningStandards = ArchitecturalOrTownPlanningStandards,
			OwnershipStatus = OwnershipStatus is null ? null : (int)OwnershipStatus,
			HousingType = HousingType is null ? null : (int)HousingType,
			ConstructionYear = ConstructionYear is null ? null : (int)ConstructionYear,
			LivingSpace = LivingSpace,
			NumberOfRoom = NumberOfRoom,
			NumberOfFloor = NumberOfFloor,
			YearOfAcquisitionOrEntry = YearOfAcquisitionOrEntry,
			CadastralReference = CadastralReference,
			HasPreviousWork = HasPreviousWork,
			CommentOnPreviousWork = CommentOnPreviousWork
		};
}

public record UpdatedAddress(
	Guid? AddressId,
	string? Label,
	string? PostalCode,
	string? City,
	string? Department,
	string? Region,
	string? AdditionalComment)
{
	public Address CreateUpdateAddress() =>
		new()
		{
			Id = AddressId ?? Guid.NewGuid(),
			Label = Label ?? string.Empty,
			PostalCode = PostalCode ?? string.Empty,
			City = City ?? string.Empty,
			Department = Department ?? string.Empty,
			Region = Region ?? string.Empty,
			AdditionnalComment = AdditionalComment
		};
}

public record UpdatedHousingInitialStateForIdentificationMilestone(
	DegradationIndex? DegradationIndex,
	UnsanitaryCoefficient? UnsanitaryCoefficientEnum,
	ComfortLevel? SummerThermalComfortLevel,
	ComfortLevel? WinterThermalComfortLevel,
	ComfortLevel? NoiseComfortLevel,
	double? AnnualEnergyConsumption,
	double? AnnualGesEmission,
	EnergyDeprivation? EnergyDeprivation,
	DpeLabel? DpeLabel,
	GesLabel? GesLabel)
{
	public HousingInitialState CreateUpdateHousingInitialState() =>
		new()
		{
			DegradationIndex = DegradationIndex is null ? null : (int)DegradationIndex,
			UnsanitaryCoefficient = UnsanitaryCoefficientEnum is null ? null : (int)UnsanitaryCoefficientEnum,
			SummerThermalComfortLevel = SummerThermalComfortLevel is null ? null : (int)SummerThermalComfortLevel,
			WinterThermalComfortLevel = WinterThermalComfortLevel is null ? null : (int)WinterThermalComfortLevel,
			NoiseComfortLevel = NoiseComfortLevel is null ? null : (int)NoiseComfortLevel,
			AnnualEnergyConsumption = AnnualEnergyConsumption,
			AnnualGesemission = AnnualGesEmission,
			EnergyDepravation = EnergyDeprivation is null ? null : (int)EnergyDeprivation,
			Dpe = DpeLabel is null ? null : (int)DpeLabel,
			Ges = GesLabel is null ? null : (int)GesLabel
		};
}