using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetAllUsersBasedOnRoleQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAllUsersBasedOnRoleQuery, ReneeOperationResult<List<GetAllUsersBasedOnRoleQueryObjectResult>>>
{
	public override async Task<ReneeOperationResult<List<GetAllUsersBasedOnRoleQueryObjectResult>>> HandleQuery(
		GetAllUsersBasedOnRoleQuery request)
	{
		try
		{
			var connectedUser = await userRepository.GetUserById(request.ConnectedUserId);
			var users = await userRepository.GetUsersByRole(request.SupportTeamMemberRole, request.ShouldRetrieveFakeUser);

			if (connectedUser == null || users.Count == 0)
				return ReneeOperationResult<List<GetAllUsersBasedOnRoleQueryObjectResult>>.Success([]);

			if (connectedUser.Role.Name == Constants.SolidarBuilderRole &&
				request.SupportTeamMemberRole == Constants.SolidarBuilderRole)
			{
				users = GetSolidarBuildersWithSameReportingStructure(users, connectedUser);
			}

			return ReneeOperationResult<List<GetAllUsersBasedOnRoleQueryObjectResult>>.Success(users.Select(u =>
				new GetAllUsersBasedOnRoleQueryObjectResult(
					$"{u.FirstName} {u.LastName}",
					u.Id,
					u.ReportingStructureId ?? Guid.Empty)
			).ToList());
		}
		catch (Exception e)
		{
			await telemetryService.TrackExceptionAsync(e, new CancellationToken());
			return ReneeOperationResult<List<GetAllUsersBasedOnRoleQueryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static List<Domain.Entity.User> GetSolidarBuildersWithSameReportingStructure(
		List<Domain.Entity.User> usersByRole,
		Domain.Entity.User connectedUser)
		=> usersByRole.Where(u => u.ReportingStructureId == connectedUser.ReportingStructureId).ToList();
}