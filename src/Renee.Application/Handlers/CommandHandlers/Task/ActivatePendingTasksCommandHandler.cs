using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Task;

public class ActivatePendingTasksCommandHandler(
	ITaskRepository taskRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<ActivatePendingTasksCommandInput>
{
	public async System.Threading.Tasks.Task Handle(ActivatePendingTasksCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			await taskRepository.ActivatePendingTasks(request.AccompanyingFileId);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
		}
	}
}