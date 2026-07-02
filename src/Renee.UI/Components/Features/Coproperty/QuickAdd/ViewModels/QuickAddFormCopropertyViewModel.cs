using Renee.Application.DTOs.Occupant;
using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.QuickAdd.ViewModels;

namespace Renee.UI.Components.Features.Coproperty.QuickAdd.ViewModels;

public class QuickAddFormCopropertyViewModel
{
    [Required(ErrorMessage = Labels.Errors.RequiredSolidarBuilderReferent)]
    public Guid? ReferentSolidarBuilderId { get; set; }

    [RequiredIfAccompanyingType(Labels.Errors.RequiredEtReferent, Domain.Enums.AccompanyingType.Targeted)]
    public Guid? ReferentEtId { get; set; }

    public Guid? SecondReferentEtId { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredPostalCodeAddressInput)]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredMunicipalityAddressInput)]
    public string? Municipality { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredDepartmentAddressInput)]
    public string? Department { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredRegionAddressInput)]
    public string? Region { get; set; }

    public string? Street { get; set; } = string.Empty;
    public string? HouseNumber { get; set; } = string.Empty;

    private AddressDto? _baseAddress;

    [Required(ErrorMessage = Labels.Errors.RequiredStreetNumberNameAddressInput)]
    public AddressDto? StreetNumberName
    {
        get => _baseAddress;
        set
        {
            _baseAddress = value;
            PostalCode = value?.PostalCode ?? string.Empty;
            Department = value?.Department ?? string.Empty;
            Municipality = value?.City ?? string.Empty;
            Region = value?.Region ?? string.Empty;
            Name = value?.Name ?? string.Empty;
            Street = value?.Street ?? string.Empty;
            HouseNumber = value?.HouseNumber ?? string.Empty;
        }
    }

    [Required(ErrorMessage = Labels.Errors.RequiredStreetNumberNameAddressInput)]
    public string? ManualAddressName
    {
        get => _baseAddress?.Label;
        set
        {
            _baseAddress = new AddressDto { Label = value };
            Name = value ?? string.Empty;
        }
    }

    public string? Name { get; private set; }
    public string? AdditionalAddress { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredHousingTypologyInput)]
    public GeographicalHousingAreaTypology? Typology { get; set; }

    [ValidateComplexType] public QuickAddAccompaniementViewModel AccompaniementViewModel { get; set; } = new();

    public Guid? SecondReferentSolidarBuilderId { get; set; }

    public Guid? ThirdReferentSolidarBuilderId { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredZeroEnergyExclusionTerritoriesProgram)]
    public bool? ZeroEnergyExclusionTerritoriesProgram { get; set; }

    [RequiredIfOther(nameof(ZeroEnergyExclusionTerritoriesProgram))]
    public AccompanyingType? AccompanyingType { get; set; }

    [RequiredIfAccompanyingType(Labels.Errors.RequiredDiffuseCoordinator, Domain.Enums.AccompanyingType.Diffuse)]
    public Guid? ReferentDiffuseCoordinator { get; set; }

    [RequiredIfAccompanyingType(Labels.Errors.RequiredTargetCoordinator, Domain.Enums.AccompanyingType.Targeted)]
    public Guid? ReferentTargetCoordinator { get; set; }
    [RequiredIfAccompanyingType(Labels.Errors.RequiredTerritory, Domain.Enums.AccompanyingType.Targeted)]
    public Guid? Territory { get; set; }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfOtherAttribute(string propertyName) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);
            var property = validationContext.ObjectType.GetProperty(propertyName);
            var requiredIfTypeActualValue = property?.GetValue(validationContext.ObjectInstance);

            if (requiredIfTypeActualValue is true && value == null)
                return new ValidationResult(Labels.Errors.RequiredAccompanyingType, [validationContext.MemberName!]);

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfAccompanyingTypeAttribute(
        string errorMessage,
        AccompanyingType accompanyingTypeRequiredValue) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);
            var property = validationContext.ObjectType.GetProperty("AccompanyingType");
            var requiredIfTypeActualValue = property?.GetValue(validationContext.ObjectInstance);

            if (requiredIfTypeActualValue is null)
                return ValidationResult.Success;

            if (Enum.IsDefined(typeof(AccompanyingType), requiredIfTypeActualValue) &&
                (AccompanyingType)requiredIfTypeActualValue != accompanyingTypeRequiredValue)
                return ValidationResult.Success;

            if (value is null)
                return new ValidationResult(errorMessage, [validationContext.MemberName!]);

            return ValidationResult.Success;
        }
    }
}
