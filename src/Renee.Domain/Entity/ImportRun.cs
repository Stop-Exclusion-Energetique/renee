namespace Renee.Domain.Entity;

public class ImportRun
{
	public Guid Id { get; set; }
	public DateTime StartedAt { get; set; }
	public DateTime? EndedAt { get; set; }
	public int? ImportStatus { get; set; }
	public string? Operator { get; set; }
	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = [];
	public virtual ICollection<ImportError> ImportErrors { get; set; } = [];
}