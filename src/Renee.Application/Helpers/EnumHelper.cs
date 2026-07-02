using System.ComponentModel;
using System.Reflection;

namespace Renee.Application.Helpers;

public static class EnumHelper
{
	public static string GetDescription(this Enum? genericEnum)
	{
		if (genericEnum is null) return string.Empty;

		var attributes = GetAttributes<DescriptionAttribute>(genericEnum);
		return attributes is { Length: not 0 }
			? ((DescriptionAttribute)attributes[0]).Description
			: genericEnum.ToString();
	}

	public static T? GetEnumValueFromDescription<T>(string? description) where T : struct, Enum
	{
		var type = typeof(T);
		if (!type.IsEnum) return null;

		foreach (var field in type.GetFields())
		{
			var attribute = field.GetCustomAttribute<DescriptionAttribute>();
			if (attribute != null)
			{
				if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
					return (T?)field.GetValue(null);
			}
			else
			{
				if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase)) return (T?)field.GetValue(null);
			}
		}

		return null;
	}

	private static object[]? GetAttributes<T>(Enum? genericEnum) where T : Attribute
	{
		if (genericEnum == null) return [];
		var genericEnumType = genericEnum.GetType();
		var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
		if (memberInfo.Length <= 0) return [];
		var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
		return attributes;
	}
}