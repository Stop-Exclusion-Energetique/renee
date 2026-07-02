using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Occupant;

public sealed class SearchAddressQuery(string search) : IRequest<ReneeOperationResult<List<AddressDto>>>
{
	public string Search { get; } = search;
}