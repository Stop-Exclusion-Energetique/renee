using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IAdminConstantRepository
{
    Task<List<AdminConstant>> GetAll();
    Task<int> UpdateValue(List<AdminConstant> adminConstants);
}