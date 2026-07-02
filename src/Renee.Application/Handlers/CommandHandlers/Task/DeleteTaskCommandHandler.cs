using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Task;

public class DeleteTaskCommandHandler(
	ITaskRepository taskRepository,
	ITelemetryService telemetryService) 
	: IRequestHandler<DeleteTaskCommandInput, ReneeOperationResult<int>>
{
	public async Task<ReneeOperationResult<int>> Handle(DeleteTaskCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			var task = await taskRepository.GetTask(request.TaskId);
			if (task is null) return ReneeOperationResult<int>.Failure(Labels.Errors.TaskNotFound);

			var result = await taskRepository.DeleteTask(request.TaskId);
			return ReneeOperationResult<int>.Success(result, Labels.DeleteTaskSuccess);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<int>.Failure(Labels.Errors.ErrorWhileDeletingTask);
		}
	}
}