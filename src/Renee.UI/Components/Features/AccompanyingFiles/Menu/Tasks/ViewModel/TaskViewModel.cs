using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.ViewModel;

public class TaskViewModel
{
	public Guid TaskId { get; set; }
	public TaskPriority Priority { get; set; }
	public string Title { get; set; } = string.Empty;
	public DateTime DueDate { get; set; }
	public DateTime StartDate { get; set; }
	public string AssignedUserFullName { get; set; } = string.Empty;
	public bool IsDone { get; set; }
	public ProgressTask? ProgressTask { get; set; }
}