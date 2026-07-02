namespace Renee.McpServer.Models;

public class EnrichmentRequest
{
	public List<EnrichmentChange> Changes { get; set; } = [];
}

public class EnrichmentChange
{
	public string Section { get; set; } = string.Empty;
	public string Field { get; set; } = string.Empty;
	public string? Value { get; set; }
}

public class EnrichmentResult
{
	public bool Success { get; set; }
	public string? ErrorCode { get; set; }
	public string? ErrorMessage { get; set; }
	public List<EnrichmentChange> AppliedChanges { get; set; } = [];
	public List<BlockedChange> BlockedChanges { get; set; } = [];
	public List<string> ModifiedPages { get; set; } = [];
	public List<string> Warnings { get; set; } = [];
}

public class BlockedChange
{
	public string Section { get; set; } = string.Empty;
	public string Field { get; set; } = string.Empty;
	public string Reason { get; set; } = string.Empty;
}
