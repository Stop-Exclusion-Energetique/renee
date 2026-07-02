using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.DTOs.Occupant;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.Identification.ViewModel;

public class IdentificationCopropertySynthesisViewModel
{
    public string? Reference { get; init; }

    public AccompanyingFileStage Stage { get; init; }

    public AccompanyingFileStatus Status { get; init; }
    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Departement { get; set; }

    public string? Municipality { get; set; }

    public string? Name { get; set; }

    private AddressDto? _baseAddress;

    public AddressDto? StreetNumberName
    {
        get => _baseAddress;
        set
        {
            _baseAddress = value;
            PostalCode = value?.PostalCode ?? string.Empty;
            Departement = value?.Department ?? string.Empty;
            Municipality = value?.City ?? string.Empty;
            Region = value?.Region ?? string.Empty;
            Name = value?.Name ?? value?.Label;
        }
    }

    public string? AdditionalAddress { get; set; }

    public GeographicalHousingAreaTypology? Typology { get; set; }


    public HousingType? HousingTypology { get; set; }

    public bool IsInTzeeProgram { get; init; }

    public int? NumberOfLots { get; set; }

    public int? NumberOfFloor { get; set; }

    public HeatingType? HeatingType { get; set; }


    public PerilType? PerilTypeLabel { get; set; }

    public NatureOfSyndicType? NatureOfSyndic { get; set; }

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

    public static IdentificationCopropertySynthesisViewModel CreateViewModelFromDto(CopropertyProfileIdentificationDto dto)
    => new()
    {
        Reference = dto.Reference,
        Stage = dto.Stage,
        Status = dto.Status,
        Region = dto.Address?.Region,
        PostalCode = dto.Address?.PostalCode,
        Departement = dto.Address?.Department,
        Municipality = dto.Address?.City,
        Name = dto.Address?.Name ?? dto.Address?.Label,
        StreetNumberName = dto.Address,
        AdditionalAddress = dto.AdditionalAddress,
        Typology = dto.Typology,
        HousingTypology = dto.HousingType,
        IsInTzeeProgram = dto.IsInTzeeProgram,
        NumberOfLots = dto.NumberOfLots,
        NumberOfFloor = dto.NumberOfFloor,
        HeatingType = dto.HeatingType,
        PerilTypeLabel = dto.PerilType,
        NatureOfSyndic = dto.NatureOfSyndicType,
        NameOfSyndic = dto.NameOfSyndic,
        PhoneOfSyndic = dto.PhoneOfSyndic,
        MailOfSyndic = dto.MailOfSyndic,
        NameOfAmo = dto.NameOfAmo,
        ContactOfAmo = dto.ContactOfAmo,
        NumberOfContacts = dto.NumberOfContacts,
        BuildingDpeLabel = dto.BuildingDpeLabel,
        BuildingDpeEnergy = dto.BuildingDpeEnergy,
        ApartmentDpeLabel = dto.ApartmentDpeLabel,
        ApartmentDpeEnergy = dto.ApartmentDpeEnergy,
        SolidarBuilderId = dto.SolidarBuilderId,
        TerritorialBuilderId = dto.TerritorialBuilderId,
        SecondSolidarBuilderId = dto.SecondSolidarBuilderId,
        ThirdSolidarBuilderId = dto.ThirdSolidarBuilderId,
        DiffuseCoordinatorId = dto.DiffuseCoordinatorId,
        TargetedCoordinatorId = dto.TargetedCoordinatorId,
        SecondTerritorialBuilderId = dto.SecondTerritorialBuilderId
    };
}
