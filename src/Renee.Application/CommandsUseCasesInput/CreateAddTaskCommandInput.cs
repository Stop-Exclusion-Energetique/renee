using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateAddTaskCommandInput(
	Guid AccompanyingFileId,
	int Priority,
	string Title,
	DateTime DueDate,
	DateTime StartDate,
	Guid AssignedUserId,
	int? ProgressTask,
	Guid CreatedByUserId) : IRequest<ReneeOperationResult<int>>
{
	public AccompanyingFileTask CreateTask()
	{
		return new AccompanyingFileTask
		{
			AccompanyingFileId = AccompanyingFileId,
			Priority = Priority,
			Title = Title,
			DueDate = DueDate,
			StartDate = StartDate,
			AssignedUserId = AssignedUserId,
			Progress = ProgressTask,
			CreatedByUserId = CreatedByUserId
		};
	}
}