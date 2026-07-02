using System.Text.Json.Serialization;

namespace Renee.McpServer.Services;

public class InseeService(HttpClient httpClient)
{
	private readonly Dictionary<string, Commune[]> _cache = [];

	public async Task<string> GetInseeCodeAsync(string codePostal, string? nomCommune = null)
	{
		var communes = await GetCommunesAsync(codePostal);

		if (communes.Length == 0)
			throw new InvalidOperationException($"Aucune commune trouvée pour le code postal {codePostal}");

		if (nomCommune is not null)
		{
			var match = communes.FirstOrDefault(c =>
				c.Nom.Contains(nomCommune, StringComparison.OrdinalIgnoreCase)
			);
			if (match is not null) return match.Code;
		}

		return communes[0].Code;
	}

	private async Task<Commune[]> GetCommunesAsync(string codePostal)
	{
		if (_cache.TryGetValue(codePostal, out var cached))
			return cached;

		var response = await httpClient.GetAsync(
			$"https://geo.api.gouv.fr/communes?codePostal={codePostal}&fields=code,nom"
		);

		response.EnsureSuccessStatusCode();

		var communes = await response.Content.ReadFromJsonAsync<Commune[]>() ?? [];
		_cache[codePostal] = communes;
		return communes;
	}
}

public record Commune(
	[property: JsonPropertyName("code")] string Code,
	[property: JsonPropertyName("nom")] string Nom
);
