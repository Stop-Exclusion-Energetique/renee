using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Occupant;

public class GetAllHouseholdResourcesTypologyQueryHandler(
	IHouseholdResourcesTypologyRepository householdResourcesTypologyRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllHouseholdResourcesTypologyQuery, ReneeOperationResult<IEnumerable<HouseholdResourcesTypologyDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<HouseholdResourcesTypologyDto>>> Handle(
		GetAllHouseholdResourcesTypologyQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var categories = await householdResourcesTypologyRepository.GetAllAsync();
			var result = categories.Select(c => new HouseholdResourcesTypologyDto { Id = c.Id, Name = c.Labels })
				.OrderBy(x => x.Name);
			return ReneeOperationResult<IEnumerable<HouseholdResourcesTypologyDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<HouseholdResourcesTypologyDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}