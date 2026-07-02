using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class AddressService(IMediator mediator) : IAddressService
{
	public async Task<ReneeOperationResult<List<AddressDto>>> SearchAddressAsync(string search) =>
		await mediator.Send(new SearchAddressQuery(search));
}