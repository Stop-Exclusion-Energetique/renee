using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.Task;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.Component.Modal.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.Component.Modal;

public partial class AddTaskModal
{
	public AddTaskModalViewModel ViewModel { get; set; } = new();
	public EditContext? EditContext { get; set; }

	[Parameter] public Guid AccompanyingFileId { get; set; }
	[Parameter] public Guid UserId { get; set; }
	[Parameter] public bool IsUpdateModal { get; set; }
	[Parameter] public Guid? TaskId { get; set; }

	[CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; } = default!;

	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;

	[Inject] public ITaskService TaskService { get; set; } = null!;
	private List<ZeeSelectItem<TaskPriority?>> TaskPriorities { get; } = new()
	{
		new ZeeSelectItem<TaskPriority?>(TaskPriority.Low.GetDescription(), TaskPriority.Low),
		new ZeeSelectItem<TaskPriority?>(TaskPriority.Middle.GetDescription(), TaskPriority.Middle),
		new ZeeSelectItem<TaskPriority?>(TaskPriority.High.GetDescription(), TaskPriority.High)
	};
	private List<ZeeSelectItem<ProgressTask?>> ProgressTasks { get; } = new()
	{
		new ZeeSelectItem<ProgressTask?>(
			ProgressTask.ToBeCompletedTask.GetDescription(),
			ProgressTask.ToBeCompletedTask),
		new ZeeSelectItem<ProgressTask?>(ProgressTask.CurrentTask.GetDescription(), ProgressTask.CurrentTask),
		new ZeeSelectItem<ProgressTask?>(ProgressTask.DoneTask.GetDescription(), ProgressTask.DoneTask)
	};

	private List<ZeeSelectItem<Guid?>> Users { get; set; } = new();

	private bool IsStartDateInFuture => ViewModel.StartDate > DateTime.Today;

	private void OnStartDateChanged()
	{
		if (IsStartDateInFuture)
			ViewModel.TaskProgress = null;
	}

	protected override async Task OnInitializedAsync()
	{
		if (IsUpdateModal && TaskId != null)
		{
			var taskResult = await TaskService.GetAccompanyingFileTaskById(TaskId.Value);
			if (taskResult.IsSuccess && taskResult.Value is not null)
			{
				var taskDto = taskResult.Value;
				ViewModel = new AddTaskModalViewModel
				{
					TaskId = taskDto.TaskId,
					AssignedTo = taskDto.AssignedTo,
					TaskName = taskDto.TaskName!,
					DueDate = taskDto.DueDate,
					StartDate = taskDto.StartDate,
					Priority = taskDto.Priority,
					TaskProgress = taskDto.TaskProgress
				};
			}
		}

		EditContext = new EditContext(ViewModel);
		var usersResult = await SendEventQuery.Send(
			new GetUserToTaskAssignmentQuery { AccompanyingFileId = AccompanyingFileId, UserId = UserId });
		if (usersResult.IsSuccess && usersResult.Value is not null)
			Users = usersResult.Value.Select(u => new ZeeSelectItem<Guid?>(u.FullName, u.UserId)).ToList();
	}

	private async Task<bool> CreateTask()
	{
		var result = await TaskService.AddTask(
			new CreateAddTaskCommandInput(
				AccompanyingFileId,
				(int)(ViewModel.Priority ?? TaskPriority.Middle),
				ViewModel.TaskName,
				ViewModel.DueDate,
				ViewModel.StartDate,
				(Guid)ViewModel.AssignedTo!,
				(int?)ViewModel.TaskProgress,
				UserId));
				
		return result.IsSuccess && result.Value == 1;
	}


	private async Task HandleValidSubmit()
	{
		bool result;

		if (IsUpdateModal)
			result = await UpdateTask();
		else
			result = await CreateTask();

		if (result) await ModalInstance.CloseAsync(ModalResult.Ok());
	}

	private async Task<bool> UpdateTask()
	{
		var result = await TaskService.UpdateTask(
			new UpdateTaskCommandInput(
				new TaskDto(
					ViewModel.TaskId!.Value,
					ViewModel.TaskName,
					ViewModel.DueDate,
					ViewModel.StartDate,
					ViewModel.AssignedTo,
					ViewModel.TaskProgress,
					ViewModel.Priority!.Value,
					AccompanyingFileId,
					UserId)));

		return result.IsSuccess && result.Value == 1;
	}
}