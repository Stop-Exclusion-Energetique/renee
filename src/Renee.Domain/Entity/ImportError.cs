namespace Renee.Domain.Entity;

public class ImportError
{
	public Guid Id { get; set; }
	public int? LineNumber { get; set; }
	public string ErrorMessage { get; set; } = string.Empty;
	public Guid ImportRunId { get; set; }
	public virtual ImportRun ImportRunNavigation { get; set; } = null!;
}