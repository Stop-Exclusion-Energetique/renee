using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IDepartmentRepository
{
	Task<List<Department>> GetAllAsync();
}