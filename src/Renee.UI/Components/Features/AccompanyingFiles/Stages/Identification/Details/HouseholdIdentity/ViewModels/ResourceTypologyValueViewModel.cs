using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

public class ResourceTypologyValueViewModel
{
	public Guid? Id { get; init; }
	public string? Name { get; init; } = string.Empty;
	[RequiredResourcesTypologyValue]
	public double? Value { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredResourcesTypologyValueAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);
		var typologie = validationContext.ObjectInstance as ResourceTypologyValueViewModel;

		if (typologie is not null && typologie.Name != EnergyDeprivationLabel.None && value is null)
		{
			return new ValidationResult($"Veuillez renseigner le champ {typologie.Name}.");
		}

		return ValidationResult.Success;
	}
}