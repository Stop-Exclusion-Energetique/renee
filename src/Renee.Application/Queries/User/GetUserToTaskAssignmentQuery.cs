using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetUserToTaskAssignmentQuery : IQuery<ReneeOperationResult<List<GetUserToTaskAssignmentQueryObjectResult>>>
{
	public required Guid AccompanyingFileId { get; init; }
	public required Guid UserId { get; init; }
}

public record GetUserToTaskAssignmentQueryObjectResult(Guid UserId, string FullName)
{
}