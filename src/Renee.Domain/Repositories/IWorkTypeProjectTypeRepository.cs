using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IWorkTypeProjectTypeRepository
{
	public Task<List<WorkTypeProjectType>> GetAllAsync();
}