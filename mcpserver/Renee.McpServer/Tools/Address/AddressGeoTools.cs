using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Renee.McpServer.Services;

namespace Renee.McpServer.Tools;

[McpServerToolType]
public class AddressGeoTools(GeoApiClient geoApiClient)
{
	private static readonly JsonSerializerOptions jsonSerializerOptions = new()
	{
		WriteIndented = false
	};

	[McpServerTool]
	[Description("""
        Résout un code postal français en nom de commune, code département et code région via l'API officielle geo.api.gouv.fr.
        À utiliser dès qu'un fichier contient un code postal sans ville, département ou région explicites,
        afin de compléter automatiquement ces champs dans la proposition d'enrichissement.
        Retourne un objet JSON { Commune, Departement, Region } si le code postal correspond à une seule commune.
        Retourne une erreur si le code postal est invalide, inconnu, ou couvre plusieurs communes (dans ce cas, demander à l'utilisateur de préciser la commune).
        """)]
	[Authorize]
	public async Task<CallToolResult> ResolvePostalCode(
		[Description("Code postal français à 5 chiffres (ex: 31000).")] string postalCode
	)
	{
		if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length != 5)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Code postal invalide (5 chiffres attendus)." }]
			};
		}

		List<GeoCommune> communes;
		try
		{
			communes = await geoApiClient.GetCommunesByPostalCodeAsync(postalCode);
		}
		catch (Exception ex)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Erreur API geo.gouv.fr : {ex.Message}" }]
			};
		}

		if (communes.Count == 0)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Aucune commune trouvée pour le code postal {postalCode}." }]
			};
		}

		if (communes.Count > 1)
		{
			var communeNames = string.Join(", ", communes.Select(c => c.Nom));
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Plusieurs communes trouvées pour le code postal {postalCode} : {communeNames}. Veuillez préciser la commune." }]
			};
		}

		var commune = communes[0];
		var resultContent = new
		{
			Commune = commune.Nom,
			Departement = commune.CodeDepartement,
			Region = commune.CodeRegion
		};

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(resultContent, jsonSerializerOptions)
		};
	}
}
