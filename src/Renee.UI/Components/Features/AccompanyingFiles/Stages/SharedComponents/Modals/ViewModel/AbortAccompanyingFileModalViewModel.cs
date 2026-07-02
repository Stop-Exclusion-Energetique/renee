using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.ViewModel;

public class AbortAccompanyingFileModalViewModel
{
	[Required(ErrorMessage = Labels.Errors.RequiredAbortReason)]
	public Guid? AbortReasonLabelId { get; set; }

	[RequiredIf(nameof(IsBillingRequested))]
	public MemoryStream? AbortJustificationFileStream { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredAbortRequestDetails)]
	public string? AbortRequestDetails { get; set; } = string.Empty;

	public bool? IsBillingRequested { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfAttribute(string billingRequestedProperty) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);
		var billingProperty = validationContext.ObjectType.GetProperty(billingRequestedProperty);
		var billingRequestedValue = billingProperty?.GetValue(validationContext.ObjectInstance) as bool?;

		if (billingRequestedValue != true)
			return ValidationResult.Success;

		if (value == null)
			return new ValidationResult(Labels.Errors.RequiredAbortJustification, [validationContext.MemberName!]);

		return ValidationResult.Success;
	}
}