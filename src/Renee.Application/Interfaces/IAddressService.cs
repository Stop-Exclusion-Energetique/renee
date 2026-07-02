using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IAddressService
{
	Task<ReneeOperationResult<List<AddressDto>>> SearchAddressAsync(string search);
}