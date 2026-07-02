using Renee.Domain;
using Renee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using static Renee.UI.Components.Features.AccompanyingFiles.QuickAdd.ViewModels.QuickAddFormViewModel;

namespace Renee.UI.Components.Features.Admin.ImportAccompanyingFileData.ViewModel;

public class ImportNewAccompanyingFileDataViewModel
{
	[Required(ErrorMessage = Labels.Errors.FirstNameRequiredInQuickAdd)]
	public string? FirstName { get; set; }

	[Required(ErrorMessage = Labels.Errors.LastNameRequiredInQuickAdd)]
	public string? LastName { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredZeroEnergyExclusionTerritoriesProgram)]
	public bool? ZeroEnergyExclusionTerritoriesProgram { get; set; }

	[RequiredIfOther(nameof(ZeroEnergyExclusionTerritoriesProgram))]
	public AccompanyingType? AccompanyingType { get; set; }

	[RequiredIfAccompanyingType(Labels.Errors.RequiredEtReferent, Domain.Enums.AccompanyingType.Targeted)]
	public Guid? ReferentEtId { get; set; }

	[RequiredIfAccompanyingType(Labels.Errors.RequiredDiffuseCoordinator, Domain.Enums.AccompanyingType.Diffuse)]
	public Guid? ReferentDiffuseCoordinator { get; set; }

	[RequiredIfAccompanyingType(Labels.Errors.RequiredTargetCoordinator, Domain.Enums.AccompanyingType.Targeted)]
	public Guid? ReferentTargetCoordinator { get; set; }

	[RequiredIfAccompanyingType(Labels.Errors.RequiredTerritory, Domain.Enums.AccompanyingType.Targeted)]
	public Guid? Territory { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredSolidarBuilderReferent)]
	public Guid? ReferentSolidarBuilderId { get; set; }

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
				return new ValidationResult(errorMessage, [validationContext.MemberName!]);

			if (Enum.IsDefined(typeof(AccompanyingType), requiredIfTypeActualValue) &&
				(AccompanyingType)requiredIfTypeActualValue != accompanyingTypeRequiredValue)
				return ValidationResult.Success;

			if (value is null) return new ValidationResult(errorMessage, [validationContext.MemberName!]);
			return ValidationResult.Success;
		}
	}
}