using Renee.Domain;
using System.ComponentModel.DataAnnotations;
using static Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.WorkPackage.ViewModel.WorkPackageViewModel.WorkType;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;

public class WorkPackageViewModel
{
	public double? TotalPrice => WorkTypes.Count > 0 ? WorkTypes.Sum(x => x.Price) : null;
	public Guid? Id { get; init; }
	public string? EnergeticsEffectOfWorks { get; set; }
	[ValidateComplexType]
	[RequiredWorkTypes]
	public List<WorkType> WorkTypes { get; init; } = [];
	public bool IsRequired { get; set; }

	public class WorkType(Guid id, string label, bool isRequired)
	{
		public Guid Id { get; } = id;
		public string Label { get; } = label;

		[RequiredPrice]
		public double? Price { get; set; }
		public string? Description { get; set; }
		public bool IsRequired { get; } = isRequired;

		[AttributeUsage(AttributeTargets.Property)]
		public class RequiredWorkTypesAttribute : ValidationAttribute
		{
			protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
			{
				if (validationContext.ObjectInstance is not WorkPackageViewModel workPackage)
					return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

				if (workPackage.IsRequired && workPackage.WorkTypes.Count == 0)
					return new ValidationResult(Labels.Errors.RequiredWorkTypes, [validationContext.MemberName!]);

				return ValidationResult.Success;
			}
		}

		[AttributeUsage(AttributeTargets.Property)]
		public class RequiredPriceAttribute : ValidationAttribute
		{
			protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
			{
				if (validationContext.ObjectInstance is not WorkType workType)
					return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

				if (workType.IsRequired && value is null)
					return new ValidationResult(Labels.Errors.RequiredWorkPackagePrice, [validationContext.MemberName!]);

				return ValidationResult.Success;
			}
		}
	}
}