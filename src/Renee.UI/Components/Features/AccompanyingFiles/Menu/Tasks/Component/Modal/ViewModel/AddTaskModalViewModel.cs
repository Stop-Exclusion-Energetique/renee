using System.ComponentModel.DataAnnotations;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.Component.Modal.ViewModel;

public class AddTaskModalViewModel
{
	[Required(ErrorMessage = Domain.AccompanyingFileMenu.Errors.RequiredTaskName)]
	public string TaskName { get; set; } = string.Empty;
	[Required(ErrorMessage = Domain.AccompanyingFileMenu.Errors.RequiredTaskDeadline)]
	public DateTime DueDate { get; set; } = DateTime.Today;
	public DateTime StartDate { get; set; } = DateTime.Today;
	[Required(ErrorMessage = Domain.AccompanyingFileMenu.Errors.RequiredTaskAssignee)]
	public Guid? AssignedTo { get; set; }
	public ProgressTask? TaskProgress { get; set; }

	public TaskPriority? Priority { get; set; }

	public Guid? TaskId { get; set; }
}