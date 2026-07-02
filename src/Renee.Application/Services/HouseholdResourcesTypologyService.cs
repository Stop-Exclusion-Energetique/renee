using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class HouseholdResourcesTypologyService(IMediator mediator) : IHouseholdResourcesTypologyService
{
	public async Task<ReneeOperationResult<IEnumerable<HouseholdResourcesTypologyDto>>> GetAllAsync() =>
		await mediator.Send(new GetAllHouseholdResourcesTypologyQuery());
}