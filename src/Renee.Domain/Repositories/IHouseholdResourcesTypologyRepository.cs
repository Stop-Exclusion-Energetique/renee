using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IHouseholdResourcesTypologyRepository
{
	Task<List<HouseholdResourcesLabel>> GetAllAsync();
}