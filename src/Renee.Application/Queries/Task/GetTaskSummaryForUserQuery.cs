using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Task;

public record GetTaskSummaryForUserQuery(Guid UserId) : IRequest<ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>>;

public record GetTaskSummaryForUserQueryObjectResult(
	Guid AccompanyingFileId,
	string MainOccupantFullName,
	string Address,
	List<TaskObjectResult> UpcomingTasks,
	List<TaskObjectResult> ToBeCompletedTasks,
	List<TaskObjectResult> CurrentTasks,
	List<TaskObjectResult> DoneTasks)
{
	public int TaskCount => UpcomingTasks.Count + ToBeCompletedTasks.Count + CurrentTasks.Count + DoneTasks.Count;
}