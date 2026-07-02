using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class ProjectTypeRepository(ReneeDbContext dbContext) : IProjectTypeRepository
{
	public async Task<IEnumerable<ProjectType>> GetAllAsync() =>
		await dbContext.ProjectTypes.AsNoTracking().ToListAsync();
}