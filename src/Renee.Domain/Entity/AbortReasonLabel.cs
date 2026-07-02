namespace Renee.Domain.Entity;

public class AbortReasonLabel
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = [];
}