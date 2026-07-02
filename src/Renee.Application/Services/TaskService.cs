using MediatR;
using Renee.Application.Commands.Mail;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.Task;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class TaskService(IMediator mediator, IUserRepository userRepository) : ITaskService
{
	public async Task<ReneeOperationResult<int>> AddTask(CreateAddTaskCommandInput request)
	{
		var result = await mediator.Send(request);

		if (result.IsSuccess && result.Value == 1)
			await SendTaskAssignedEmail(request.AssignedUserId, request.CreatedByUserId, request.Title);

		return result;
	}

	public async Task ActivatePendingTasks(Guid accompanyingFileId) =>
		await mediator.Send(new ActivatePendingTasksCommandInput(accompanyingFileId));

	public async Task<ReneeOperationResult<int>> DeleteTask(Guid taskId, Guid deletedByUserId)
	{
		var taskResult = await GetAccompanyingFileTaskById(taskId);

		if (!taskResult.IsSuccess || taskResult.Value is null)
			return ReneeOperationResult<int>.Failure(taskResult.Message);

		var result = await mediator.Send(new DeleteTaskCommandInput(taskId));

		if (result.IsSuccess && result.Value == 1)
			await SendTaskDeletedEmail(taskResult.Value.AssignedTo!.Value, deletedByUserId, taskResult.Value.TaskName);
		return result;
	}

	public async Task<ReneeOperationResult<TaskDto?>> GetAccompanyingFileTaskById(Guid id) =>
		await mediator.Send(new GetAccompanyingFileTaskByIdQuery(id));

	public async Task<ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>> GetTaskSummaryForUser(Guid userId) =>
		await mediator.Send(new GetTaskSummaryForUserQuery(userId));

	public async Task<ReneeOperationResult<int>> UpdateTask(UpdateTaskCommandInput input)
	{
		var oldTaskResult = await GetAccompanyingFileTaskById(input.TaskDto.TaskId);

		if (!oldTaskResult.IsSuccess || oldTaskResult.Value is null) 
			return ReneeOperationResult<int>.Failure(oldTaskResult.Message);

		input.TaskDto.CreatedByUserId = oldTaskResult.Value.CreatedByUserId;

		var result = await mediator.Send(input);

		if (result.IsSuccess && result.Value == 1)
		{
			var assignedToChanged = oldTaskResult.Value.AssignedTo != input.TaskDto.AssignedTo;
			var taskCompleted = oldTaskResult.Value.TaskProgress != ProgressTask.DoneTask && 
				input.TaskDto.TaskProgress == ProgressTask.DoneTask;

			if (assignedToChanged)
				await SendTaskAssignedEmail(input.TaskDto.AssignedTo!.Value, oldTaskResult.Value.CreatedByUserId, input.TaskDto.TaskName);

			if (taskCompleted)
				await SendTaskCompletedEmail(oldTaskResult.Value.CreatedByUserId, input.TaskDto.AssignedTo!.Value, input.TaskDto.TaskName);
		}

		return result;
	}

	private async Task SendTaskAssignedEmail(Guid assigneeId, Guid creatorId, string taskTitle)
	{
		var assignedUser = await userRepository.GetUserById(assigneeId);
		var creator = await userRepository.GetUserById(creatorId);

		if (assignedUser?.Email is null || creator is null) return;

		var creatorName = $"{creator.FirstName} {creator.LastName}";

		await mediator.Send(new SendMailCommand(
			MailType.TaskAssigned,
			assignedUser.Email,
			new MailParameters { Param1 = creatorName, Param2 = taskTitle }));
	}

	private async Task SendTaskCompletedEmail(Guid creatorId, Guid assigneeId, string taskTitle)
	{
		var creator = await userRepository.GetUserById(creatorId);
		var assignedUser = await userRepository.GetUserById(assigneeId);

		if (creator?.Email is null || assignedUser is null) return;

		await mediator.Send(new SendMailCommand(
			MailType.TaskCompleted,
			creator.Email,
			new MailParameters
			{
				Param1 = taskTitle,
				Param2 = $"{assignedUser.FirstName} {assignedUser.LastName}"
			}));
	}

	private async Task SendTaskDeletedEmail(Guid assigneeId, Guid deleterId, string taskTitle)
	{
		var assignedUser = await userRepository.GetUserById(assigneeId);
		var deleter = await userRepository.GetUserById(deleterId);

		if (assignedUser?.Email is null || deleter is null) return;

		await mediator.Send(new SendMailCommand(
			MailType.TaskDeleted,
			assignedUser.Email,
			new MailParameters
			{
				Param1 = taskTitle,
				Param2 = $"{deleter.FirstName} {deleter.LastName}"
			}));
	}
}