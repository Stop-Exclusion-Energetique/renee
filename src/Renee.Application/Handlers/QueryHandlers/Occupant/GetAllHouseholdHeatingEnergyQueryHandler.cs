using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Occupant;

public class GetAllHouseholdHeatingEnergyQueryHandler(
	IHouseholdHeatingEnergyLabelRepository householdHeatingEnergyLabelRepository,
	ITelemetryService telemetryService) 
	: QueryHandler<GetAllHouseholdHeatingEnergyQuery, ReneeOperationResult<List<HouseholdHeatingEnergyLabel>>>
{
	public override async Task<ReneeOperationResult<List<HouseholdHeatingEnergyLabel>>> HandleQuery(GetAllHouseholdHeatingEnergyQuery request)
	{
		try
		{
			var result = await householdHeatingEnergyLabelRepository.GetAllHouseHoldHeatingEnergyLabelAsync();
			return ReneeOperationResult<List<HouseholdHeatingEnergyLabel>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<List<HouseholdHeatingEnergyLabel>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
