using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateAccompanyingFileWithQuickAddCommandInput(
	QuickAddCreatedAccompanyingFileEntities AccompanyingFileEntities,
	Guid ConnectedUserId,
	bool ZeroEnergyExclusionTerritoriesProgram,
	AccompanyingType? AccompanyingType,
	Guid? Territory) : IRequest<ReneeOperationResult<Guid?>>;

public record QuickAddCreatedAccompanyingFileEntities(
	QuickAddCreatedOccupant MainOccupant,
	QuickAddCreatedAddress HousingAddress,
	QuickAddGeographicalAreaTypology GeographicalAreaTypology,
	QuickAddSupportTeam SupportTeam,
	QuickAddHousingPropertyType HousingPropertyType);

public record QuickAddCreatedOccupant(string Trigram, string? FirstName, string? LastName, string? PhoneNumber = null!, string? Email = null!)
{
	public MainOccupant CreateOccupant()
	{
		return new MainOccupant { Trigram = Trigram, PhoneNumber = PhoneNumber, Email = Email, FirstName = FirstName, LastName = LastName };
	}
}

public record QuickAddCreatedAddress(
	string Label,
	string PostalCode,
	string City,
	string Department,
	string Region,
	string Street,
	string HouseNumber,
	string? AdditionalAddress = null!)
{
	public Address CreateAddress()
	{
		return new Address
		{
			Label = Label,
			PostalCode = PostalCode,
			City = City,
			Department = Department,
			Region = Region,
			AdditionnalComment = AdditionalAddress,
			Street = Street,
			HouseNumber = HouseNumber
		};
	}
}

public record QuickAddGeographicalAreaTypology(GeographicalHousingAreaTypology Type);

public record QuickAddSupportTeam(
	MarkerNature? Type,
	string? TrustedTierLastName,
	string? TrustedTierStructureName,
	Guid SolidarBuilder,
	string SolidarBuilderFullName,
	Guid? TerritorialBuilder,
	Guid? SecondTerritorialBuilder,
	string? CommentOnMarkerNature,
	string? TrustedTierFirstName,
	string? TrustedTierPhoneNumber,
	string? TrustedTierEmail,
	TrustedTierRole? TrustedTierRole,
	string? CommentOnTrustedTierRole,
	Guid? SecondSolidarBuilder,
	Guid? ReferentDiffuseCoordinator,
	Guid? ReferentTargetCoordinator,
	Guid? ThirdSolidarBuilder)
{
	public SupportTeam CreateSupportTeam()
	{
		return new SupportTeam
		{
			MarkerNature = Type is null ? 0 : (int)Type,
			TrustedTierFirstName = TrustedTierFirstName,
			TrustedTierStructureName = TrustedTierStructureName,
			SolidarBuilder = SolidarBuilder,
			TerritorialBuilder = TerritorialBuilder,
			SecondTerritorialBuilder = SecondTerritorialBuilder,
			CommentOnMarkerNature = CommentOnMarkerNature,
			TrustedTierLastName = TrustedTierLastName,
			TrustedTierPhoneNumber = TrustedTierPhoneNumber,
			TrustedTierEmail = TrustedTierEmail,
			TrustedTierRole = TrustedTierRole is null ? 0 : (int)TrustedTierRole,
			CommentOnTrustedTierRole = CommentOnTrustedTierRole,
			SecondSolidarBuilder = SecondSolidarBuilder,
			DiffuseCoordinator = ReferentDiffuseCoordinator,
			TargetCoordinator = ReferentTargetCoordinator,
			ThirdSolidarBuilder = ThirdSolidarBuilder
		};
	}
}

public record QuickAddHousingPropertyType(HousingType? AskedPropertyType, Guid? CopropertyProfileId)
{
	public int? GetAskedPropertyType()
	{
		return (int?)AskedPropertyType;
	}

	public Guid? GetCopropertyProfileId()
	{
		return CopropertyProfileId;
	}
}