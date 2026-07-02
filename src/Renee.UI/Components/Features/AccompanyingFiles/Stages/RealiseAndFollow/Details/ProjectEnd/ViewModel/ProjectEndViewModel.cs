using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectEnd.ViewModel;

public class ProjectEndViewModel
{
	public string? AccompanyingTime
	{
		get
		{
			if (StartOfAccompanyingDate <= EndOfAccompanyingDate)
				return
					$"{CalculateAccompanyingTime(StartOfAccompanyingDate ?? DateTime.Now, EndOfAccompanyingDate ?? DateTime.Now)}";

			return null;
		}
	}

	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredEndOfAccompanyingDate)]
	[Range(
		typeof(DateTime),
		"1950/01/01",
		"2100/12/31",
		ConvertValueInInvariantCulture = true,
		ErrorMessage = Labels.Errors.InvalidDateRange)]
	[DateCompare(nameof(StartOfAccompanyingDate))]
	public DateTime? EndOfAccompanyingDate { get; set; }

	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredEndOfEncounterDate)]
	[Range(
		typeof(DateTime),
		"1950/01/01",
		"2100/12/31",
		ConvertValueInInvariantCulture = true,
		ErrorMessage = Labels.Errors.InvalidDateRange)]
	[DatesCompare(nameof(StartOfAccompanyingDate), nameof(EndOfAccompanyingDate))]
	public DateTime? EndOfEncounterDate { get; set; }

	public DateTime? StartOfAccompanyingDate { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredAccompayingTimeDuration)]
	public AccompanyingTimeDuration? AccompanyingTimeDuration { get; set; }

	private static int CalculateAccompanyingTime(DateTime startDate, DateTime endDate)
	{
		return (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;
	}

	private static DateTime? GetPropertyDateValue(ValidationContext context, string propertyName)
	{
		return context.ObjectType.GetProperty(propertyName)?.GetValue(context.ObjectInstance) as DateTime?;
	}

	[AttributeUsage(AttributeTargets.Property)]
	private sealed class DateCompareAttribute(string comparisonProperty) : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var currentValue = (DateTime?)value;
			var comparisonValue = GetPropertyDateValue(validationContext, comparisonProperty);

			if (!currentValue.HasValue || !comparisonValue.HasValue) return ValidationResult.Success;

			return comparisonProperty switch
			{
				nameof(StartOfAccompanyingDate) when currentValue < comparisonValue => new ValidationResult(
					RealiseAndFollowMilestone.Errors.EndOfAccompanyingDateInvalidRange),
				_ => ValidationResult.Success
			};
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	private sealed class DatesCompareAttribute(string startOfAccompanyingDate, string endOfAccompanyingDate)
		: ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var currentValue = value as DateTime?;
			var startOfAccompanyingDateValue = GetPropertyDateValue(validationContext, startOfAccompanyingDate);
			var endOfAccompanyingDateValue = GetPropertyDateValue(validationContext, endOfAccompanyingDate);

			if (endOfAccompanyingDateValue.HasValue &&
			    (currentValue < startOfAccompanyingDateValue || currentValue < endOfAccompanyingDateValue))
				return new ValidationResult(
					RealiseAndFollowMilestone.Errors.EndOfEncounterDateNotLaterThanStartOfAccompanyingDate);

			if (!endOfAccompanyingDateValue.HasValue)
				return new ValidationResult(
					RealiseAndFollowMilestone.Errors.EndOfAccompanyingDateNotBeforeEndOfEncounterDate);

			return ValidationResult.Success;
		}
	}
}