using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class SiteSupervisionDifficultyLabelRepository(
	ReneeDbContext reneeDbContext) : ISiteSupervisionDifficultyLabelRepository
{
	public async Task<List<SiteSupervisionDifficultyLabel>> GetAllAsync() =>
		await reneeDbContext.SiteSupervisionDifficultyLabels.AsNoTracking().ToListAsync();
}