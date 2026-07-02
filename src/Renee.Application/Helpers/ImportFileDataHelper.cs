using System.ComponentModel;
using System.Globalization;
using Renee.Domain;

namespace Renee.Application.Helpers;

public static class ImportFileDataHelper
{
	public static bool TryParseValue<T>(string? value, out T result)
	{
		result = default!;
		if (string.IsNullOrEmpty(value))
		{
			return HandleEmptyValue(out result);
		}

		if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
		{
			return TryParseInt(value, out result);
		}

		if (typeof(T) == typeof(double) || typeof(T) == typeof(double?))
		{
			return TryParseDouble(value, out result);
		}

		if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
		{
			return TryParseDateTime(value, out result);
		}

		if (typeof(T) == typeof(bool) || typeof(T) == typeof(bool?))
		{
			return TryParseBool(value, out result);
		}

        if (typeof(T) == typeof(string))
		{
			result = (T)(object)value!;
			return true;
		}

		if (typeof(T).IsEnum || (IsNullableType(typeof(T)) && Nullable.GetUnderlyingType(typeof(T))!.IsEnum))
		{
			return TryParseEnum(value, out result);
		}

		return false;
	}

	private static string CleanNumberString(string value) => value.Replace(" ", "").Replace("\u00A0", "");

	private static bool HandleEmptyValue<T>(out T result)
	{
		if (typeof(T) == typeof(string) || IsNullableType(typeof(T)))
		{
			result = default!;
			return true;
		}

		result = default!;
		return false;
	}

	private static bool IsNullableType(Type type) => Nullable.GetUnderlyingType(type) != null;

	private static bool TryParseBool<T>(string value, out T result)
	{
		if (value.Equals(Labels.Yes, StringComparison.OrdinalIgnoreCase) || value.Equals(CsvDataLabel.YesValue, StringComparison.OrdinalIgnoreCase))
		{
			result = (T)(object)true!;
			return true;
		}

		if (value.Equals(Labels.No, StringComparison.OrdinalIgnoreCase) || value.Equals(CsvDataLabel.NoValue, StringComparison.OrdinalIgnoreCase))
		{
			result = (T)(object)false!;
			return true;
		}

		result = default!;
		return false;
	}

	private static bool TryParseDateTime<T>(string value, out T result)
	{
        if (DateTime.TryParseExact(
                value,
                Labels.DateFormatUtc,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedValue))
        {
            result = (T)(object)DateTime.SpecifyKind(parsedValue, DateTimeKind.Utc)!;
            return true;
        }

        result = default!;
		return false;
	}

	private static bool TryParseDouble<T>(string value, out T result)
	{
		if (double.TryParse(CleanNumberString(value), out var parsedValue))
		{
			result = (T)(object)parsedValue!;
			return true;
		}

		result = default!;
		return false;
	}

	private static bool TryParseEnum<T>(string value, out T result)
	{
		var enumType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		var success = TryParseEnumFromDescription(enumType, value, out var enumResult);
		result = success ? (T)enumResult! : default!;
		return success;
	}

	private static bool TryParseEnumFromDescription(Type enumType, string description, out object? result)
	{
		description = description.Replace(CsvDataLabel.OperatorApostrophe, CsvDataLabel.MainApostrophe);

		foreach (var field in enumType.GetFields())
			if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
			{
				if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
				{
					result = field.GetValue(null);
					return true;
				}
			}
			else
			{
				if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
				{
					result = field.GetValue(null);
					return true;
				}
			}

		result = null;
		return false;
	}

	private static bool TryParseInt<T>(string value, out T result)
	{
		if (int.TryParse(CleanNumberString(value), out var parsedValue))
		{
			result = (T)(object)parsedValue!;
			return true;
		}

		result = default!;
		return false;
	}
}