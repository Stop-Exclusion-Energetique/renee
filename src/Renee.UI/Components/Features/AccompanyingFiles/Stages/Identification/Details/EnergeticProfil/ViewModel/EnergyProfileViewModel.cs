using Renee.Domain;
using Renee.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.EnergeticProfil.ViewModel;

public sealed class EnergyProfileViewModel
{
	[Required(ErrorMessage = Labels.Errors.DpeMustBeSelected)]
	public DpeLabel? AskedDpeLabel { get; set; }

	[Required(ErrorMessage = Labels.Errors.GesMustBeSelected)]
	public GesLabel? AskedGesLabel { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = Labels.EnterOnlyNumber)]
	[Required(ErrorMessage = Labels.Errors.RequiredEnergyConsumption)]
	public double? EnergyConsumption { get; set; }

	[RegularExpression("^[0-9]*$", ErrorMessage = Labels.EnterOnlyNumber)]
	public double? GesEmissions { get; set; }

    [RequiredEnergyDeprivation(nameof(IsEnergyEffortRateRequired))]
	public EnergyDeprivation? EnergyDeprivation { get; set; }

    public bool IsEnergyEffortRateRequired { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredEnergyDeprivationAttribute(string IsEnergyEffortRateRequired) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(validationContext);

        if (value is not null)
            return ValidationResult.Success;

        var property = validationContext.ObjectType.GetProperty(IsEnergyEffortRateRequired);

        if (property?.GetValue(validationContext.ObjectInstance) is not bool isRequired)
        {
            throw new InvalidOperationException("La propriété 'IsEnergyEffortRateRequired' est manquante ou n'est pas un booléen.");
        }

        if (isRequired && value is null)
        {
            return new ValidationResult(
                Labels.Errors.RequiredEnergyDeprivation, [validationContext.MemberName!] );
        }

        return ValidationResult.Success;
    }
}