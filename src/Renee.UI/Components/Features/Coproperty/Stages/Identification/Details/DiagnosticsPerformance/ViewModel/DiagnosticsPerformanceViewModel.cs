using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.DiagnosticsPerformance.ViewModel;

public class DiagnosticsPerformanceViewModel
{
    [ApartmentOrBuildingValidation(nameof(ApartmentDpeLabel), ErrorMessage = Labels.Errors.RequiredDpeLabel)]
    public DpeLabel? BuildingDpeLabel { get; set; }

    [RequiredIfHasValueAttribute(nameof(BuildingDpeLabel), ErrorMessage = Labels.Errors.RequiredBuildingDpeEnergy)]
    public double? BuildingDpeEnergy { get; set; }

    public DpeLabel? ApartmentDpeLabel { get; set; }

    [RequiredIfHasValueAttribute(nameof(ApartmentDpeLabel), ErrorMessage = Labels.Errors.RequiredApartmentDpeEnergy)]
    public double? ApartmentDpeEnergy { get; set; }
}


[AttributeUsage(AttributeTargets.Property)]
public class ApartmentOrBuildingValidationAttribute(string propertyName) : ValidationAttribute
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

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfHasValueAttribute(string otherProperty) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(validationContext);
        var otherProp = validationContext.ObjectType.GetProperty(otherProperty);
        var otherValue = otherProp?.GetValue(validationContext.ObjectInstance);
        if (otherValue != null)
        {
            bool hasValue = value != null && !(value is string s && string.IsNullOrWhiteSpace(s));
            if (!hasValue)
                return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}

