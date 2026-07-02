using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface ITerritoryRepository
{
	Task<List<Territory>> GetAllTerritories();
}