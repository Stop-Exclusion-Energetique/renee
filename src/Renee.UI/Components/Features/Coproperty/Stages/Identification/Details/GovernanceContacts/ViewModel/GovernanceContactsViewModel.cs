using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GovernanceContacts.ViewModel;

public class GovernanceContactsViewModel
{
    [Required(ErrorMessage = Labels.Errors.RequiredNatureOfSyndic)]
    public NatureOfSyndicType? NatureOfSyndic { get; set; }
    [Required(ErrorMessage = Labels.Errors.RequiredNameOfSyndic)]
    public string? NameOfSyndic { get; set; }
    [RequiredIfNot(nameof(HousingTypology), HousingType.ResidentialCollective, ErrorMessage = Labels.Errors.RequiredPhoneOfSyndic)]
    public string? PhoneOfSyndic { get; set; }
    [RequiredIfNot(nameof(HousingTypology), HousingType.ResidentialCollective, ErrorMessage = Labels.Errors.RequiredMailOfSyndic)]
    public string? MailOfSyndic { get; set; }
    public string? NameOfAmo { get; set; }
    public string? ContactOfAmo { get; set; }
    public int? NumberOfContacts { get; set; }

    public HousingType? HousingTypology { get; set; }

}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredIfNotAttribute(string otherProperty, object targetValue) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(validationContext);

        var otherProp = validationContext.ObjectType.GetProperty(
            otherProperty);

        var otherValue = otherProp?.GetValue(validationContext.ObjectInstance);

        bool condition = !object.Equals(otherValue, targetValue);

        if (!condition)
            return ValidationResult.Success;

        bool hasValue = value is not null && (value is not string s || !string.IsNullOrWhiteSpace(s));
        if (hasValue)
            return ValidationResult.Success;

        var message = ErrorMessage;
        return new ValidationResult(message, [validationContext.MemberName ?? string.Empty]);
    }
}