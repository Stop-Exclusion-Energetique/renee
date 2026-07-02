using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IRoleRepository
{
	Task<List<Role>> GetAllAsync();
}