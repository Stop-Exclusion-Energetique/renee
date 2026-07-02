using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.TargetInformations.ViewModel;

public class AccompanyingFileTargetDetailsViewModel
{
    [Required(ErrorMessage = Labels.Errors.RequiredHousingTypologyInput)]
    public GeographicalHousingAreaTypology? GeographicalHousingAreaTypology { get; set; }
    [Required(ErrorMessage = Labels.Errors.RequiredAccompanyingType)]
	public AccompanyingType? AccompanyingType { get; set; }
	[RequiredIfAccompanyingType(nameof(AccompanyingType), Domain.Enums.AccompanyingType.Targeted, ErrorMessage = Labels.Errors.RequiredTerritory)]
	public Guid? AccompanyingTerritory { get; set; }
    public string MarValue { get; set; } = string.Empty;
    public bool IsInTzeeProgram { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RequiredIfAccompanyingTypeAttribute(string accompanyingTypePropertyName, AccompanyingType requiredType)
	: ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);

		var property = validationContext.ObjectType.GetProperty(accompanyingTypePropertyName);
		if (property is null) return ValidationResult.Success;

		var propertyValue = property?.GetValue(validationContext.ObjectInstance);

		if (propertyValue is AccompanyingType type && type == requiredType && (value is not Guid guid || guid == Guid.Empty))
		{
			var errorMessage = ErrorMessage ?? Labels.Errors.RequiredTerritory;
			var memberName = validationContext.MemberName ?? string.Empty;
			return string.IsNullOrEmpty(memberName)
				? new ValidationResult(errorMessage)
				: new ValidationResult(errorMessage, [memberName]);
		}

		return ValidationResult.Success;
	}
}
