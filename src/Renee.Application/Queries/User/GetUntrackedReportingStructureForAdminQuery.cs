using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetUntrackedReportingStructureForAdminQuery : IQuery<ReneeOperationResult<List<GetUntrackedReportingStructureForAdminQueryObjectResult>>>
{
}

public class GetUntrackedReportingStructureForAdminQueryObjectResult
{
	public string? ReportingStructureName { get; set; }
	public DateTime? SubscriptionDate { get; set; }
	public Guid UserId { get; set; }
}
