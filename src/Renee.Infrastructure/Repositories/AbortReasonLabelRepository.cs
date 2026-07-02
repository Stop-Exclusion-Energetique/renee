using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class AbortReasonLabelRepository(ReneeDbContext dbContext) : IAbortReasonLabelRepository
{
	public async Task<List<AbortReasonLabel>> GetAllAsync() =>
		await dbContext.AbortReasonLabels.AsNoTracking().ToListAsync();
}