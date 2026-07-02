using System.Globalization;
using System.Text;
using System.Security.Cryptography;


namespace Renee.Application.Helpers;

public static class StringHelper
{
	public static string? BoolToString(bool? value)
	{
		if (value is null) return null;
		return value.Value ? "Oui" : "Non";
	}

	public static string? DoubleToCurrencyFormat(double? fieldValue)
	{
		return fieldValue is null
			? null
			: $"{fieldValue.Value.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"))}";
	}

	public static string GenerateRandomString(bool isMicrosoftCompatible = false)
	{
		using (var rng = RandomNumberGenerator.Create())
		{
			if (isMicrosoftCompatible)
			{
				// Génère 2 lettres majuscules, 2 minuscules, et 6 chiffres
				var randomString = new StringBuilder();

				for (var i = 0; i < 2; i++)
				{
					randomString.Append((char)GetRandomInt(rng, 65, 91));   // Majuscules A-Z
					randomString.Append((char)GetRandomInt(rng, 97, 123)); // Minuscules a-z
				}
				for (var i = 0; i < 6; i++)
				{
					randomString.Append(GetRandomInt(rng, 0, 10)); // Chiffres 0-9
				}
				return randomString.ToString();
			}

			// Nombre à 6 chiffres (de 100000 à 999999)
			int randomNumber = GetRandomInt(rng, 100000, 1000000);
			return randomNumber.ToString();
		}
	}

	private static int GetRandomInt(RandomNumberGenerator rng, int min, int max)
	{
		if (min >= max)
			throw new ArgumentException("min must be less than max");

		// Générer un entier non signé de 4 octets
		var bytes = new byte[4];
		int range = max - min;

		// Pour éviter le modulo bias, on utilise un algorithme recommandé par Microsoft
		uint limit = uint.MaxValue - (uint.MaxValue % (uint)range);

		while (true)
		{
			rng.GetBytes(bytes);
			uint value = BitConverter.ToUInt32(bytes, 0);

			if (value < limit)
				return (int)(min + (value % range));
		}
	}
}