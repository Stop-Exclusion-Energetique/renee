using Renee.Application.DTOs.Occupant;
using Renee.Domain.Enums;

namespace Renee.Application.DTOs.CopropertyProfile;

public class CopropertyProfileIdentificationDto
{
    public Guid Id { get; init; }

    public string? Reference { get; init; }

    public AccompanyingFileStage Stage { get; init; }

    public AccompanyingFileStatus Status { get; init; }

    public DateTime? CreationDatetimeUtc { get; set; }

    public DateTime? LastUpdateDatetimeUtc { get; set; }

    public bool IsInTzeeProgram { get; init; }

    public GeographicalHousingAreaTypology? Typology { get; set; }

    public HousingType? HousingType { get; init; }

    public int? NumberOfLots { get; set; }

    public int? NumberOfFloor { get; set; }

    public AddressDto? Address { get; init; } = null!;

    public string? AdditionalAddress { get; set; }

    public HeatingType? HeatingType { get; init; }

    public PerilType? PerilType { get; init; }

    public NatureOfSyndicType? NatureOfSyndicType { get; set; }

    public string? NameOfSyndic { get; set; }  

    public string? PhoneOfSyndic { get; set; }

    public string? MailOfSyndic { get; set; }

    public string? NameOfAmo { get; set; }

    public string? ContactOfAmo { get; set; }

    public int? NumberOfContacts { get; set; }

    public DpeLabel? BuildingDpeLabel { get; set; }

    public double? BuildingDpeEnergy { get; set; }

    public DpeLabel? ApartmentDpeLabel { get; set; }

    public double? ApartmentDpeEnergy { get; set; }

    public Guid? SolidarBuilderId { get; set; }

    public Guid? TerritorialBuilderId { get; set; }

    public Guid? SecondSolidarBuilderId { get; set; }

    public Guid? ThirdSolidarBuilderId { get; set; }

    public Guid? DiffuseCoordinatorId { get; set; }

    public Guid? TargetedCoordinatorId { get; set; }

    public Guid? SecondTerritorialBuilderId { get; set; }

}
