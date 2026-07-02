using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetUntrackedReportingStructureForAdminQueryHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetUntrackedReportingStructureForAdminQuery, ReneeOperationResult<List<GetUntrackedReportingStructureForAdminQueryObjectResult>>>
{
	public override async Task<ReneeOperationResult<List<GetUntrackedReportingStructureForAdminQueryObjectResult>>> HandleQuery(GetUntrackedReportingStructureForAdminQuery request)
	{
		try
		{
			var unregisteredUser = await unregisteredUserRepository.GetUntrackedReportingStructure();

			return ReneeOperationResult<List<GetUntrackedReportingStructureForAdminQueryObjectResult>>.Success(unregisteredUser.Select(user => new GetUntrackedReportingStructureForAdminQueryObjectResult
			{
				ReportingStructureName = user.ReportingStructure,
				SubscriptionDate = user.SubscriptionDateUtc,
				UserId = user.Id
			}).ToList());
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<List<GetUntrackedReportingStructureForAdminQueryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
