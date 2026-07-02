using MediatR;
using Renee.Application.DTOs.Task;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Task;

public sealed class GetAccompanyingFileTaskByIdQueryHandler(
	ITaskRepository taskRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAccompanyingFileTaskByIdQuery, ReneeOperationResult<TaskDto?>>
{
	public async Task<ReneeOperationResult<TaskDto?>> Handle(GetAccompanyingFileTaskByIdQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var taskEntity = await taskRepository.GetTask(request.Id);
			if (taskEntity is null) return ReneeOperationResult<TaskDto?>.Failure(Labels.Errors.TaskNotFound);

			var taskDto = new TaskDto(
				taskEntity.Id,
				taskEntity.Title,
				taskEntity.DueDate,
				taskEntity.StartDate,
				taskEntity.AssignedUserId,
				(ProgressTask?)taskEntity.Progress,
				(TaskPriority)taskEntity.Priority,
				taskEntity.AccompanyingFileId,
				taskEntity.CreatedByUserId);
				
			return ReneeOperationResult<TaskDto?>.Success(taskDto);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<TaskDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}