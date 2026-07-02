using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Task;

public class UpdateTaskCommandHandler(ITaskRepository TaskRepository, ITelemetryService telemetryService)
	: IRequestHandler<UpdateTaskCommandInput, ReneeOperationResult<int>>
{
	public async Task<ReneeOperationResult<int>> Handle(UpdateTaskCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.TaskDto.TaskId == Guid.Empty || request.TaskDto.AssignedTo == null)
				return ReneeOperationResult<int>.Failure(Labels.Errors.InvalidTaskData);

			var entity = new AccompanyingFileTask
			{
				Id = request.TaskDto.TaskId,
				Title = request.TaskDto.TaskName!,
				AssignedUserId = request.TaskDto.AssignedTo!.Value,
				DueDate = request.TaskDto.DueDate,
				StartDate = request.TaskDto.StartDate,
				Priority = (int)request.TaskDto.Priority!,
				Progress = (int?)request.TaskDto.TaskProgress,
				AccompanyingFileId = request.TaskDto.AccompanyingFileId,
				CreatedByUserId = request.TaskDto.CreatedByUserId
			};

			var result = await TaskRepository.UpdateTaskStatus(entity);
			return ReneeOperationResult<int>.Success(result, Labels.UpdateTaskSuccess);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<int>.Failure(Labels.Errors.ErrorWhileUpdatingTask);
		}
	}
}