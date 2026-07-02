using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Task;

public class CreateAddTaskCommandHandler(
	ITaskRepository taskRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateAddTaskCommandInput, ReneeOperationResult<int>>
{
	public async Task<ReneeOperationResult<int>> Handle(CreateAddTaskCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await taskRepository.AddTask(request.CreateTask());
			return ReneeOperationResult<int>.Success(result, Labels.CreateTaskSuccess);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<int>.Failure(Labels.Errors.ErrorWhileCreatingTask);
		}
	}
}