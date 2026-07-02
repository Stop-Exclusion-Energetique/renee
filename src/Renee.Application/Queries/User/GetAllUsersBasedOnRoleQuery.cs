using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetAllUsersBasedOnRoleQuery(
	Guid connectedUserId,
	string supportTeamMemberRole,
	bool shouldRetrieveFakeUser) 
	: IQuery<ReneeOperationResult<List<GetAllUsersBasedOnRoleQueryObjectResult>>>
{
	public Guid ConnectedUserId { get; set; } = connectedUserId;
	public string SupportTeamMemberRole { get; set; } = supportTeamMemberRole;
	public bool ShouldRetrieveFakeUser { get; set; } = shouldRetrieveFakeUser;
}

public record GetAllUsersBasedOnRoleQueryObjectResult(string FullName, Guid Id, Guid ReportingStructureId)
{
}