using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetAllUntrackedAccountDeletionRequestsForAdminQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService) 
	: QueryHandler<GetAllUntrackedAccountDeletionRequestsForAdminQuery, ReneeOperationResult<List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult>>>
{
	public override async Task<ReneeOperationResult<List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult>>> HandleQuery(
		GetAllUntrackedAccountDeletionRequestsForAdminQuery request)
	{
		try
		{
			var accountDeletionRequests = await userRepository.GetAllUntrackedAccountDeletionRequests();

			if (accountDeletionRequests.Count == 0)
				return ReneeOperationResult<List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult>>.Success([]);

			return ReneeOperationResult<List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult>>.Success(accountDeletionRequests.Select(user => new GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role.LongName,
				ReportingStructureName = user.ReportingStructureNavigation?.Name,
				UserId = user.Id
			}).ToList());
		}
		catch(Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
			return ReneeOperationResult<List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}