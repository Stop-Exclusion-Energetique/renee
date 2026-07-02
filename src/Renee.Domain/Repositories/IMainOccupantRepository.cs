using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IMainOccupantRepository
{
	Task<List<MainOccupant>> GetAllAsync();
}