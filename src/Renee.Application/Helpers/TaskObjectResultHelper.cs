using Renee.Application.Queries.Task;
using Renee.Domain.Enums;

namespace Renee.Application.Helpers;

public static class TaskObjectResultHelper
{
	public static List<TaskObjectResult> UpcomingTasks(this IEnumerable<TaskObjectResult> tasks) =>
		[.. tasks
			.Where(t => !t.ProgressTask.HasValue && t.StartDate > DateTime.Today)
			.OrderBy(t => t.StartDate)
			.ThenByDescending(t => t.Priority)];

	public static List<TaskObjectResult> ToBeCompletedTasks(this IEnumerable<TaskObjectResult> tasks) =>
		[.. tasks
			.Where(t => t.StartDate <= DateTime.Today && t.ProgressTask == ProgressTask.ToBeCompletedTask)
			.OrderBy(t => t.DueDate)
			.ThenByDescending(t => t.Priority)];

	public static List<TaskObjectResult> CurrentTasks(this IEnumerable<TaskObjectResult> tasks) =>
		[.. tasks
			.Where(t => t.StartDate <= DateTime.Today && t.ProgressTask == ProgressTask.CurrentTask)
			.OrderBy(t => t.DueDate)
			.ThenByDescending(t => t.Priority)];

	public static List<TaskObjectResult> DoneTasks(this IEnumerable<TaskObjectResult> tasks) =>
		[.. tasks
			.Where(t => t.ProgressTask == ProgressTask.DoneTask)
			.OrderBy(t => t.DueDate)
			.ThenByDescending(t => t.Priority)];
}
