using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public record GetAllUntrackedAccountDeletionRequestsForAdminQuery 
	: IQuery<ReneeOperationResult<List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult>>>;

public class GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Role { get; set; }
	public string? ReportingStructureName { get; set; }
	public Guid UserId { get; set; }
}