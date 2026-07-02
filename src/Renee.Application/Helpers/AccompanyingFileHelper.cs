using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.Application.Helpers;

public static class AccompanyingFileHelper
{
	public static string CreateReference(
		string ensemblierSolidaireFullName,
		string mainOccupantTrigram,
		string postalCode,
        DateTime? openingDate = null) => $"{ensemblierSolidaireFullName}-{mainOccupantTrigram.ToUpper()}-{postalCode}-{openingDate ?? DateTime.Now:dd/MM/yyyy}";

    public static string GenerateTrigram(string? firstName, string? lastName)
	{
		string firstNameInitial = !string.IsNullOrEmpty(firstName)
			? firstName[..1].ToUpper()
			: "X";

		string lastNameInitials = !string.IsNullOrEmpty(lastName) ? string.Join("", lastName.Split([' ', '-'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?[..2].ToUpper() ?? "YY") : "YY";

		return firstNameInitial + lastNameInitials;
	}

	public static int? CalculateAge(DateTime? birthdate)
	{
		if (birthdate == null) return null;

		var today = DateTime.Today;
		var age = today.Year - birthdate.Value.Year;

		if (birthdate > today.AddYears(-age)) age--;

		return age;
	}

	public static int? CalculateEnergeticClassJump(DpeLabel? estimatedDpeBeforeWork, DpeLabel? estimatedDpeAfterWork) =>
		(estimatedDpeBeforeWork is null || estimatedDpeAfterWork is null)
			? null
			: (int)estimatedDpeBeforeWork - (int)estimatedDpeAfterWork;

	public static string GetMar(DegradationIndex? degradationIndex, UnsanitaryCoefficient? unsanitaryCoefficient)
	{
		if (degradationIndex is null && unsanitaryCoefficient is null)
			return string.Empty;

		var severity = new[]
		{
			degradationIndex.HasValue ? (int?)degradationIndex.Value : null,
			unsanitaryCoefficient.HasValue ? (int?)unsanitaryCoefficient.Value : null
		}.Max()!.Value;

		return severity == (int)DegradationIndex.Low
			? Labels.ClassicMar
			: Labels.IntensiveMar;
	}
}
