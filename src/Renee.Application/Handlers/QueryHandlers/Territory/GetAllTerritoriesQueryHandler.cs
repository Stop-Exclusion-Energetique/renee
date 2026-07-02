using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.Territory;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Territory;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Territory;

public class GetAllTerritoriesQueryHandler(ITerritoryRepository territoryRepository, ITelemetryService telemetryService)
	: QueryHandler<GetAllTerritoriesQuery, ReneeOperationResult<IEnumerable<TerritoryQueryObjectResult>>>
{
	public override async Task<ReneeOperationResult<IEnumerable<TerritoryQueryObjectResult>>> HandleQuery(GetAllTerritoriesQuery request)
	{
		try
		{
			var result = (await territoryRepository.GetAllTerritories())
				.Select(t => new TerritoryQueryObjectResult(t.Label, t.Id));
			return ReneeOperationResult<IEnumerable<TerritoryQueryObjectResult>>.Success(result);
		}
		catch (Exception e)
		{
			await telemetryService.TrackExceptionAsync(e, new CancellationToken());
			return ReneeOperationResult<IEnumerable<TerritoryQueryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}