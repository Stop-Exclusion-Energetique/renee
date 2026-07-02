using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.ReasonOfProject.ViewModel;

public sealed class ReasonOfProjectViewModel
{
	public string? DeliveryTime
	{
		get
		{
			if (FirstEncounterDate <= SigningHouseholdSupportDate)
				return
					$"{CalculateDeliveryTime(FirstEncounterDate ?? DateTime.Now, SigningHouseholdSupportDate ?? DateTime.Now)}";

			return null;
		}
	}

	public List<Guid?>? AskedDifficulties { get; set; } = [];

	[Required(ErrorMessage = Labels.Errors.EnterSocialContext)]
	public string? ReasonOfProjectSocialContext { get; set; }

	public string? VisitAvailability { get; set; }

	[Required(ErrorMessage = Labels.Errors.EnterFamilyProject)]
	public string? FamilyProject { get; set; }

	public string? DifficultiesDetails { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredAccompayingTimeDuration)]
	public AccompanyingTimeDuration? AccompanyingTimeDuration { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredDateSigningHouseholdSupport)]
	[DataType(DataType.Date, ErrorMessage = Labels.Errors.InvalidFormatDateSigningHouseholdSupport)]
	[Range(
		typeof(DateTime),
		"1950/01/01",
		"2100/12/31",
		ConvertValueInInvariantCulture = true,
		ErrorMessage = Labels.Errors.InvalidDateRange)]
	public DateTime? SigningHouseholdSupportDate { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredFirstContactDate)]
	[DataType(DataType.Date, ErrorMessage = Labels.Errors.InvalidFormatFirstContactDate)]
	[Range(
		typeof(DateTime),
		"1950/01/01",
		"2100/12/31",
		ConvertValueInInvariantCulture = true,
		ErrorMessage = Labels.Errors.InvalidDateRange)]
	public DateTime? FirstEncounterDate { get; set; }

	public bool? ShouldAccompanyingFileBeSubmittedToAnah {  get; set; }

	private static int CalculateDeliveryTime(DateTime firstEncouterDate, DateTime startSigningDate)
	{
		var months = (startSigningDate.Year - firstEncouterDate.Year) * 12 +
		             startSigningDate.Month -
		             firstEncouterDate.Month;
		var daysDifference = startSigningDate.Day - firstEncouterDate.Day;

		switch (daysDifference)
		{
			case < -1:
			{
				if ((startSigningDate.Month == firstEncouterDate.AddMonths(1).Month &&
				     startSigningDate.Year == firstEncouterDate.Year) ||
				    DateTime.DaysInMonth(firstEncouterDate.Year, firstEncouterDate.Month) -
				    firstEncouterDate.Day +
				    startSigningDate.Day <
				    15)
					months--;

				break;
			}
			case > 14:
			{
				if (startSigningDate.Year > firstEncouterDate.Year ||
				    startSigningDate.Month >= firstEncouterDate.AddMonths(1).Month)
					months++;

				break;
			}
		}

		return months;
	}
}