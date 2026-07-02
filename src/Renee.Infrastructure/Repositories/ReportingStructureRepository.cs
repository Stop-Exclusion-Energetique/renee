using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class ReportingStructureRepository(ReneeDbContext dbContext, IDbContextFactory<ReneeDbContext> contextFactory) : IReportingStructureRepository
{
	public async Task<Guid> AddReportingStructureAsync(ReportingStructure reportingStructure)
	{
		var entityEntry = dbContext.ReportingStructures.Add(reportingStructure);
		await dbContext.SaveChangesAsync();
		return entityEntry.Entity.Id;
	}

	public async Task<List<ReportingStructure>> GetAllReportingStructuresAsync()
	{
		var context = await contextFactory.CreateDbContextAsync();
		return await context.ReportingStructures.OrderBy(rs => rs.Name).ToListAsync();
	}
}