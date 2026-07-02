using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Renee.McpServer.Models;

namespace Renee.McpServer.Services;

public class RenoHelpClient(HttpClient httpClient)
{
	private static readonly JsonSerializerOptions jsonOptions = new()
	{
		Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
	};

	public async Task<EligibilityReport> GetEligibilityAsync(PublicodeRequest payload)
	{
		var json = JsonSerializer.Serialize(payload, jsonOptions);

		var baseUrl = (httpClient.BaseAddress?.ToString().TrimEnd('/')) ?? "https://mesaides.france-renov.gouv.fr/api/v1";
		var url = $"{baseUrl}/?fields=eligibilite";

		string authToken = httpClient.DefaultRequestHeaders.TryGetValues("authorization", out var vals)
			? vals.First()
			: string.Empty;

		var raw = await CallViaNodeAsync(url, authToken, json);
		raw = raw.Replace(" ", " ");
		var items = JsonSerializer.Deserialize<List<EligibilityApiItem>>(raw, jsonOptions) ?? [];
		foreach (var item in items)
			item.MissingVariables = item.MissingVariables?
				.Where(v => !v.StartsWith("parcours d'aide", StringComparison.OrdinalIgnoreCase))
				.ToList();

		return new EligibilityReport
		{
			AidesEligibles = [.. items
				.Where(i => i.Status)
				.Select(i => new EligibleAid
				{
					Nom = i.Label,
					Type = i.Type,
					Montant = i.Value,
					Taux = i.Taux is null or "Pas encore défini" ? null : i.Taux,
					Duree = i.Duree is null or "Pas encore défini" ? null : i.Duree
				})
			],
			AidesNonEligibles = [.. items
				.Where(i => !i.Status && (i.MissingVariables?.Count ?? 0) == 0)
				.Select(i => new NonEligibleAid { Nom = i.Label, Type = i.Type })
			],
			AidesIncompletes = [.. items
				.Where(i => !i.Status && (i.MissingVariables?.Count ?? 0) > 0)
				.Select(i => new IncompleteAid
				{
					Nom = i.Label,
					Type = i.Type,
					InformationsManquantes = i.MissingVariables ?? []
				})
			]
		};
	}

	private static async Task<string> CallViaNodeAsync(string url, string authToken, string jsonPayload)
	{
		const string script = """
			let input = '';
			for await (const chunk of process.stdin) input += chunk;
			const { url, authToken, body } = JSON.parse(input);
			const res = await fetch(url, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
					'authorization': authToken
				},
				body
			});
			process.stdout.write(await res.text());
			""";

		var stdinPayload = JsonSerializer.Serialize(new { url, authToken, body = jsonPayload }, jsonOptions);

		var psi = new ProcessStartInfo("node")
		{
			RedirectStandardInput = true,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false
		};
		psi.ArgumentList.Add("--input-type=module");
		psi.ArgumentList.Add("--eval");
		psi.ArgumentList.Add(script);

		using var process = Process.Start(psi) ?? throw new InvalidOperationException("Failed to start node process");

		await process.StandardInput.WriteAsync(stdinPayload);
		process.StandardInput.Close();

		var output = await process.StandardOutput.ReadToEndAsync();
		_ = await process.StandardError.ReadToEndAsync();
		await process.WaitForExitAsync();

		if (process.ExitCode != 0)
			throw new InvalidOperationException($"Node.js process exited with code {process.ExitCode}.");

		return output;
	}
}

file class EligibilityApiItem
{
	[JsonPropertyName("label")]
	public string Label { get; set; } = string.Empty;

	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("status")]
	public bool Status { get; set; }

	[JsonPropertyName("value")]
	public string Value { get; set; } = string.Empty;

	[JsonPropertyName("taux")]
	public string? Taux { get; set; }

	[JsonPropertyName("durée")]
	public string? Duree { get; set; }

	[JsonPropertyName("missingVariables")]
	public List<string>? MissingVariables { get; set; } = [];
}
