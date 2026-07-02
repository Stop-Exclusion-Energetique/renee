namespace Renee.Domain.Entity;

public class AccompanyingFileTask
{
	public Guid Id { get; set; }

	public Guid AccompanyingFileId { get; set; }

	public int Priority { get; set; }

	public string Title { get; set; } = null!;

	public DateTime DueDate { get; set; }

	public DateTime StartDate { get; set; }

	public Guid AssignedUserId { get; set; }

	public bool IsDone { get; set; }

	public int? Progress { get; set; }

	public Guid CreatedByUserId { get; set; }

	public virtual AccompanyingFile AccompanyingFile { get; set; } = null!;

	public virtual User AssignedUser { get; set; } = null!;

	public virtual User CreatedByUser { get; set; } = null!;
}