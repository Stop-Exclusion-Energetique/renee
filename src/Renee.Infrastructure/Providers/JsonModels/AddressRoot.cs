using System.Text.Json.Serialization;

namespace Renee.Infrastructure.Providers.JsonModels;

public class Feature
{
	[JsonPropertyName("type")] public string? type { get; set; }

	[JsonPropertyName("geometry")] public Geometry? geometry { get; set; }

	[JsonPropertyName("properties")] public Properties? properties { get; set; }
}

public class Filters
{
	[JsonPropertyName("type")] public string? type { get; set; }
}

public class Geometry
{
	[JsonPropertyName("type")] public string? type { get; set; }

	[JsonPropertyName("coordinates")] public List<double?>? coordinates { get; set; }
}

public class Properties
{
	[JsonPropertyName("label")] public string? label { get; set; }

	[JsonPropertyName("score")] public double? score { get; set; }

	[JsonPropertyName("type")] public string? type { get; set; }

	[JsonPropertyName("importance")] public double? importance { get; set; }

	[JsonPropertyName("id")] public string? id { get; set; }

	[JsonPropertyName("banId")] public string? banId { get; set; }

	[JsonPropertyName("name")] public string? name { get; set; }

	[JsonPropertyName("housenumber")] public string? housenumber { get; set; }

	[JsonPropertyName("street")] public string? street { get; set; }

	[JsonPropertyName("postcode")] public string? postcode { get; set; }

	[JsonPropertyName("citycode")] public string? citycode { get; set; }

	[JsonPropertyName("x")] public double? x { get; set; }

	[JsonPropertyName("y")] public double? y { get; set; }

	[JsonPropertyName("city")] public string? city { get; set; }

	[JsonPropertyName("context")] public string? context { get; set; }

	[JsonPropertyName("locality")] public string? locality { get; set; }
}

public class Root
{
	[JsonPropertyName("type")] public string? type { get; set; }

	[JsonPropertyName("version")] public string? version { get; set; }

	[JsonPropertyName("features")] public List<Feature>? features { get; set; }

	[JsonPropertyName("attribution")] public string? attribution { get; set; }

	[JsonPropertyName("licence")] public string? licence { get; set; }

	[JsonPropertyName("query")] public string? query { get; set; }

	[JsonPropertyName("filters")] public Filters? filters { get; set; }

	[JsonPropertyName("limit")] public int? limit { get; set; }
}