using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class WorkTypesLabelsRepository(ReneeDbContext dbContext) : IWorkTypesLabelsRepository
{
	public async Task<IEnumerable<WorkTypesLabel>> GetAllAsync() => await dbContext.WorkTypesLabels.ToListAsync();
}