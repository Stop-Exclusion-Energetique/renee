using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class MainOccupantService(IMediator mediator) : IMainOccupantService
{
	public async Task<ReneeOperationResult<IEnumerable<MainOccupantDto>>> GetAllMainOccupantAsync() =>
		await mediator.Send(new GetAllMainOccupantsQuery());
}