using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class TerritoryRepository(IDbContextFactory<ReneeDbContext> contextFactory) : ITerritoryRepository
{
	public async Task<List<Territory>> GetAllTerritories()
	{
		var dbContext = await contextFactory.CreateDbContextAsync();
		return await dbContext.Territories.AsNoTracking().ToListAsync();
	} 
}