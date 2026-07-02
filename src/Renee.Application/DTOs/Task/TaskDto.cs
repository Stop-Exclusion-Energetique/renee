using Renee.Domain.Enums;

namespace Renee.Application.DTOs.Task;

public class TaskDto
{
	public Guid TaskId { get; set; }
	public string TaskName { get; set; }
	public DateTime DueDate { get; set; }
	public DateTime StartDate { get; set; }
	public Guid? AssignedTo { get; set; }
	public ProgressTask? TaskProgress { get; set; }
	public TaskPriority? Priority { get; set; }
	public Guid AccompanyingFileId { get; set; }
	public Guid CreatedByUserId { get; set; }

	public TaskDto(
		Guid taskId,
		string taskName,
		DateTime dueDate,
		DateTime startDate,
		Guid? assignedTo,
		ProgressTask? taskProgress,
		TaskPriority priority,
		Guid accompanyingFileId,
		Guid createdByUserId)
	{
		TaskId = taskId;
		TaskName = taskName;
		DueDate = dueDate;
		StartDate = startDate;
		AssignedTo = assignedTo!.Value;
		TaskProgress = taskProgress;
		Priority = priority;
		AccompanyingFileId = accompanyingFileId;
		CreatedByUserId = createdByUserId;
	}
}