using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class WorkTypeProjectTypeRepository(ReneeDbContext dbContext) : IWorkTypeProjectTypeRepository
{
	public async Task<List<WorkTypeProjectType>> GetAllAsync() =>
		await dbContext.WorkTypeProjectTypes.AsNoTracking().ToListAsync();
}
