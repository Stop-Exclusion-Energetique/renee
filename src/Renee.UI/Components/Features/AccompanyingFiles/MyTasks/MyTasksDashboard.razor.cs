using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Domain;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AccompanyingFiles.MyTasks;

public partial class MyTasksDashboard
{
	[Inject] private ITaskService TaskService { get; set; } = null!;
	[Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;


	private List<GetTaskSummaryForUserQueryObjectResult> _taskGroups = [];

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;
		var userIdClaims = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (Guid.TryParse(userIdClaims, out var userId))
		{
			var result = await TaskService.GetTaskSummaryForUser(userId);

			if (result.IsSuccess && result.Value is not null)
				_taskGroups = result.Value;
		}
	}

	private void NavigateToAccompanyingFileMenu(Guid accompanyingFileId) =>
		NavigationManager.NavigateTo($"{Endpoints.AccompanyingFileMenu}/{accompanyingFileId}");

	private static (string HeaderClass, string Label, IReadOnlyList<TaskObjectResult> Tasks)[] GetTaskColumns(
		GetTaskSummaryForUserQueryObjectResult group) =>
	[
		("status-upcoming", AccompanyingFileMenu.UpcomingTasks,      group.UpcomingTasks),
		("status-todo",     AccompanyingFileMenu.ToBePerformedTasks, group.ToBeCompletedTasks),
		("status-current",  AccompanyingFileMenu.UndoneTasks,        group.CurrentTasks),
		("status-done",     AccompanyingFileMenu.DoneTasks,          group.DoneTasks),
	];
}