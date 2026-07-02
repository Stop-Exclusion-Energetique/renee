namespace Renee.Application.DTOs.AI;

public class AISynthesisResult
{
    public List<AnomalyItem> Anomalies { get; init; } = [];
    public bool HasAnomalies => Anomalies.Count > 0;
}

public class AnomalyItem
{
    public string Field { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Jalon { get; init; } = string.Empty;
    public string Source { get; init; } = string.Empty;
}
