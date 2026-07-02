using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface ISiteSupervisionDifficultyLabelRepository
{
	Task<List<SiteSupervisionDifficultyLabel>> GetAllAsync();
}