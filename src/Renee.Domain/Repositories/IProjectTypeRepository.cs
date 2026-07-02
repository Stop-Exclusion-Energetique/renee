using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IProjectTypeRepository
{
	Task<IEnumerable<ProjectType>> GetAllAsync();
}