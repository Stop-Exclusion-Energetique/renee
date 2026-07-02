using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class MainOccupantRepository(ReneeDbContext dbContext) : IMainOccupantRepository
{
	public async Task<List<MainOccupant>> GetAllAsync() => await dbContext.MainOccupants.ToListAsync();
}