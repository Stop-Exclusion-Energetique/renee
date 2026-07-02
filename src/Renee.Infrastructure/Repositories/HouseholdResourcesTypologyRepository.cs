using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class HouseholdResourcesTypologyRepository(ReneeDbContext dbContext)
	: IHouseholdResourcesTypologyRepository
{
	public async Task<List<HouseholdResourcesLabel>> GetAllAsync() =>
		await dbContext.HouseholdResourcesLabels.ToListAsync();
}