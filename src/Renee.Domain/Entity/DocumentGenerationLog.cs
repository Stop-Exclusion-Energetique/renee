namespace Renee.Domain.Entity;

public class DocumentGenerationLog
{
	public Guid Id { get; set; }

	public Guid UserId { get; set; }

	public string FileName { get; set; } = null!;

	public DateTime GeneratedAt { get; set; }
}