using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class DifficultyFacedByFamilyRepository(ReneeDbContext dbContext) : IDifficultyFacedByFamilyRepository
{
	public async Task<List<HouseholdDifficultiesLabel>> GetAllAsync() =>
		await dbContext.HouseholdDifficultiesLabels.ToListAsync();
}