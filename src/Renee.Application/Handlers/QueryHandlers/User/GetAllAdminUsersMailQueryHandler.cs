using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetAllAdminUsersMailQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAllAdminUsersMailQuery, ReneeOperationResult<List<string?>>>
{
	public override async Task<ReneeOperationResult<List<string?>>> HandleQuery(GetAllAdminUsersMailQuery request)
	{
		try
		{
			var users = await userRepository.GetAllAdminUsers();

			return ReneeOperationResult<List<string?>>.Success(users.Select(u => u.Email).ToList());
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<List<string?>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}