using System.ComponentModel.DataAnnotations;
using Renee.Domain;

namespace Renee.UI.Components.Features.NewUser;

public class UserViewModel
{
	[EmailAddress(ErrorMessage = Labels.Errors.EmailAddressFormatInput)]
	[Required(ErrorMessage = Labels.Errors.InvalidEmail)]
	public string? Email { get; set; }

	public string? Password { get; set; }

	private string? _lastName;

	[Required(ErrorMessage = Labels.Errors.RequiredLastnameInput)]
	public string? LastName { 
		get => _lastName; 
		set => _lastName = value?.Trim(); 
	}

	private string? _name;

	[Required(ErrorMessage = Labels.Errors.RequiredFirstnameInput)]
	public string? FirstName { 
		get => _name;
		set => _name = value?.Trim(); 
	}

	[Phone]
	[Required(ErrorMessage = Labels.Errors.RequiredPhoneNumberInput)]
	public string? PhoneNumber { get; set; }

	public Guid? TerritoryId { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredRole)]
	public Guid? AskedRole { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredAssociatedStructure)]
	public Guid? SelectedReportingStructureId { get; set; }

	public string? SelectedReportingStructureName { get; set; }

	[RequiredIfOther(
		nameof(SelectedReportingStructureName),
		Labels.Other,
		ErrorMessage = Labels.Errors.RequiredOtherReportingStructure)]
	public string? OtherReportingStructure { get; set; }

	public string? Function { get; set; }

	[RegularExpression("^[0-9]{14}$", ErrorMessage = Labels.Errors.IncorrectFormatOfSiretNumber)]
	[Required(ErrorMessage = Labels.Errors.RequiredSiretNumber)]
	public string? SiretNumber { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfOtherAttribute(string propertyName, string comparisonValue) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext? validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);

		var propertyInfo = validationContext.ObjectType.GetProperty(propertyName);
		var propertyValue = propertyInfo?.GetValue(validationContext.ObjectInstance);

		if (propertyValue is null || (string)propertyValue != comparisonValue) return ValidationResult.Success;

		if (string.IsNullOrEmpty(value?.ToString()))
		{
			return new ValidationResult(
				ErrorMessage,
				[validationContext.MemberName ?? Labels.Errors.UnknownErrorFormField]);
		}

		return ValidationResult.Success;
	}
}