using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Occupant;

public class GetAllMainOccupantsQueryHandler(
	IMainOccupantRepository mainOccupantRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllMainOccupantsQuery, ReneeOperationResult<IEnumerable<MainOccupantDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<MainOccupantDto>>> Handle(
		GetAllMainOccupantsQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var occupants = await mainOccupantRepository.GetAllAsync();
			var result = occupants.Select(o => new MainOccupantDto { Id = o.Id, Email = o.Email, PhoneNumber = o.PhoneNumber })
				.OrderBy(x => x.Id);
			return ReneeOperationResult<IEnumerable<MainOccupantDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<MainOccupantDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}