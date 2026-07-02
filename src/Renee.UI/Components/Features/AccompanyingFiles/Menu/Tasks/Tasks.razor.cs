using Microsoft.AspNetCore.Components;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks;

public partial class Tasks
{
	[Parameter] public IReadOnlyList<TaskViewModel> ToBeCompletedTasks { get; set; } = [];
	[Parameter] public IReadOnlyList<TaskViewModel> UndoneTasks { get; set; } = [];
	[Parameter] public IReadOnlyList<TaskViewModel> DoneTasks { get; set; } = [];
	[Parameter] public IReadOnlyList<TaskViewModel> UpcomingTasks { get; set; } = [];

	[Parameter] public EventCallback OnShowAddTaskModal { get; set; }
	[Parameter] public EventCallback<Guid> OnDeleteTask { get; set; }
	[Parameter] public EventCallback<Guid> OnShowUpdateTaskModal { get; set; }
}