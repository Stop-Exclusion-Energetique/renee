using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.QuickAdd.ViewModels;

public class QuickAddAccompaniementViewModel
{
	public string? TrustedTierStructureName { get; set; }
	public string? TrustedTierLastName { get; set; }
	public string? TrustedTierFirstName { get; set; }
	public string? TrustedTierPhoneNumber { get; set; }

	[EmailAddress(ErrorMessage = Labels.Errors.EmailAddressFormatInput)]
	public string? TrustedTierEmail { get; set; }

	public TrustedTierRole? TrustedTierRole { get; set; }
	public string? TrustedTierRoleFreeInput { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredMarkerNature)]
	public MarkerNature? MarkerNature { get; set; }

	[CommentOnMarkerNatureValidation(ErrorMessage = Labels.Errors.RequiredCommentOnMarkerNature)]
	public string? CommentOnMarkerNature { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class CommentOnMarkerNatureValidationAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		if (validationContext.ObjectInstance is QuickAddAccompaniementViewModel { MarkerNature: MarkerNature.Other } &&
		    string.IsNullOrWhiteSpace(value as string))
			return new ValidationResult(
				ErrorMessage,
				new[] { validationContext.MemberName ?? Labels.Errors.UnknownErrorFormField });

		return ValidationResult.Success;
	}
}