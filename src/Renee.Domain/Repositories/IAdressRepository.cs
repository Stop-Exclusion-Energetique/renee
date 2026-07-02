using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IAddressRepository
{
	Task<List<Address>?> SearchAddressAsync(string search);
}