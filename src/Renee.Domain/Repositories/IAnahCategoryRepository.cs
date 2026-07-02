using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IAnahCategoryRepository
{
	Task<int> CreateAnahCategoryAsync(AnahCategory entity);
	Task<List<AnahCategory>> GetAllAnahCategoriesAsync();
	Task<int> UpdateAnahCategoryAsync(AnahCategory entity);
}