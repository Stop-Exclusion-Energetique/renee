using System.Text.Json;
using System.Text.Json.Serialization;

namespace Renee.McpServer.Services;

public class GeoApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<List<GeoCommune>> GetCommunesByPostalCodeAsync(string postalCode)
    {
        var url = $"communes?codePostal={Uri.EscapeDataString(postalCode)}&fields=nom,codeDepartement,codeRegion,departement,region";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<GeoCommune>>(json, JsonOptions) ?? [];
    }
}

public class GeoCommune
{
    public string Nom { get; set; } = string.Empty;
    public string CodeDepartement { get; set; } = string.Empty;
    public string CodeRegion { get; set; } = string.Empty;
    public GeoNom? Departement { get; set; }
    public GeoNom? Region { get; set; }
}

public class GeoNom
{
    public string Code { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
}
