using Renee.Application.DTOs.Occupant;
using Renee.Domain.Enums;
using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GeneralInfo.ViewModel;

public class GeneralInfoViewModel
{
    [Required(ErrorMessage = Labels.Errors.RequiredRegionAddressInput)]
    public string? Region { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredPostalCodeAddressInput)]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredDepartmentAddressInput)]
    public string? Departement { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredMunicipalityAddressInput)]
    public string? Municipality { get; set; }

    public string? Name { get; set; }

    private AddressDto? _baseAddress;

    [Required(ErrorMessage = Labels.Errors.RequiredStreetNumberNameAddressInput)]
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

    public bool? IsManualInput { get; set; } = false;

    [Required(ErrorMessage = Labels.Errors.RequiredStreetNumberNameAddressInput)]
    public string? ManualAddress
    {
        get => Name;
        set
        {
            _baseAddress = new AddressDto { Label = value };
            Name = value ?? string.Empty;
        }
    }

    public string? AdditionalAddress { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredHousingTypologyInput)]
    public GeographicalHousingAreaTypology? Typology { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredHousingTypology)]
    public HousingType? HousingTypology { get; set; }

    public int? NumberOfLots { get; set; }

    public int? NumberOfFloor { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredHeatingType)]
    public HeatingType? HeatingType { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredPerilType)]
    public PerilType? PerilTypeLabel { get; set; }
}
