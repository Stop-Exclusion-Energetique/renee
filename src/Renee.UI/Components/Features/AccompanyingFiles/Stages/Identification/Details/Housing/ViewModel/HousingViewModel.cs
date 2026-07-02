using System.ComponentModel.DataAnnotations;
using Renee.Application.DTOs.Occupant;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.Housing.ViewModel;

public class HousingViewModel
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

	[Required(ErrorMessage = Labels.Errors.RequiredAskedPropertyStatusInput)]
	public OwnershipStatus? AskedPropertyStatus { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredAskedPropertyTypeInput)]
	public HousingType? AskedPropertyType { get; set; }

	[RequiredIfEqual(nameof(AskedPropertyType), HousingType.ResidentialCollective, ErrorMessage = Labels.Errors.RequiredCopropertyProfileInput)]
	public Guid? CopropertyProfileId { get; set; }

    [Required(ErrorMessage = Labels.Errors.RequiredAskedConstructionYear)]
    public HousingYearConstruction? AskedConstructionYear { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredSurfaceInput)]
	public double? Surface { get; set; }

	public int? NumberOfRooms { get; set; }

	public int? NumberOfFloor { get; set; }

	[Range(1500, int.MaxValue, ErrorMessage = Labels.Errors.YearOfAcquisitionRangeError)]
	[CustomYearValidation(ErrorMessage = Labels.Errors.YearOfAcquisitionRangeError)]
	public int? YearOfacquisition { get; set; }

	public string? CadastralReference { get; set; }

	public string? ArchitecturalOrUrbanStandards { get; set; } = string.Empty;

	public bool? AbfZoneYes { get; set; }

	[UnsanitaryCoefficientAndDegradationIndexValidation(
		nameof(UnsanitaryCoefficient),
		ErrorMessage = Labels.Errors.RequiredUnsanitaryCoefficientOrDegradationIndex)]
	public DegradationIndex? DegradationIndex { get; set; }

	[UnsanitaryCoefficientAndDegradationIndexValidation(
		nameof(DegradationIndex),
		ErrorMessage = Labels.Errors.RequiredUnsanitaryCoefficientOrDegradationIndex)]
	public UnsanitaryCoefficient? UnsanitaryCoefficient { get; set; }

	public bool? RenovationWorkYes { get; set; }

	[RequiredIf(nameof(RenovationWorkYes), ErrorMessage = Labels.Errors.RequiredRenovationExplanationsInput)]
	public string? RenovationExplanations { get; set; }

	public ComfortLevel? SummerComfort { get; set; }
	public ComfortLevel? WinterComfort { get; set; }
	public ComfortLevel? SoundComfort { get; set; }
	private AddressDto? _baseAddress;
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfAttribute(string propertyName) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);
		var property = validationContext.ObjectType.GetProperty(propertyName);
		var requiredIfTypeActualValue = property?.GetValue(validationContext.ObjectInstance);

		if(requiredIfTypeActualValue is bool && requiredIfTypeActualValue is true)
		{
			if (value is null || (string)value == string.Empty || (string)value == "\n")
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			else
				return ValidationResult.Success;
		}

		return ValidationResult.Success;
	}
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredIfEqualAttribute(string otherProperty, object targetValue) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);

		var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
		var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

		if (!Equals(otherValue, targetValue))
			return ValidationResult.Success;

		var hasValue = value is not null && (value is not string s || !string.IsNullOrWhiteSpace(s));
		if (hasValue)
			return ValidationResult.Success;

		return new ValidationResult(ErrorMessage, [validationContext.MemberName ?? string.Empty]);
	}
}

[AttributeUsage(AttributeTargets.Property)]
public class CustomYearValidationAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	{
		if (value is not int year) return ValidationResult.Success!;
		var currentYear = DateTime.Now.Year;
		if (year < 1500 || year > currentYear) return new ValidationResult(ErrorMessage);

		return ValidationResult.Success!;
	}
}

[AttributeUsage(AttributeTargets.Property)]
public class UnsanitaryCoefficientAndDegradationIndexValidationAttribute(string propertyName) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);
		var property = validationContext.ObjectType.GetProperty(propertyName);
		var propertyValue = property?.GetValue(validationContext.ObjectInstance);

		if (propertyValue == null && value == null)
			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

		return ValidationResult.Success;
	}
}