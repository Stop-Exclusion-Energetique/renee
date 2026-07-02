using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Forms;

namespace Renee.UI.Components.FormComponents;

public class ZeeInputDate<TValue> : InputDate<TValue>
{
	private const int MinYear = 1000;
	private const int DefaultYear = 2000;

	protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
	{
		if (!base.TryParseValueFromString(value, out result, out validationErrorMessage))
		{
			return false;
		}

		var year = GetYear(result);
		if (year.HasValue && year.Value < MinYear)
		{
			result = AdjustYear(result);
		}

		return true;
	}

	private static TValue AdjustYear(TValue value) => value switch
	{
		DateTime dt => (TValue)(object)dt.AddYears(DefaultYear - dt.Year),
		DateTimeOffset dto => (TValue)(object)dto.AddYears(DefaultYear - dto.Year),
		DateOnly d => (TValue)(object)d.AddYears(DefaultYear - d.Year),
		_ => value
	};

	private static int? GetYear(TValue? value) => value switch
	{
		DateTime dt => dt.Year,
		DateTimeOffset dto => dto.Year,
		DateOnly d => d.Year,
		_ => null
	};
}
