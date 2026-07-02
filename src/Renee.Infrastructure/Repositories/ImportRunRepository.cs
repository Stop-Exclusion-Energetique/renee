using Microsoft.EntityFrameworkCore;
using Renee.Application.Interfaces;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class ImportRunRepository(
	ReneeDbContext dbContext,
	ITelemetryService telemetryService) : IImportRunRepository
{
	public async Task<int> AddImportRun(ImportRun importRun)
	{
		try
		{
			await dbContext.ImportRuns.AddAsync(importRun);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
			return -1;
		}
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<ImportRun?> GetImportRunById(Guid importRunId)
	{
		try
		{
			var importRun = await dbContext.ImportRuns.AsNoTracking()
				.FirstOrDefaultAsync(ir => ir.Id == importRunId);

			if (importRun is not null)
				dbContext.Entry(importRun).State = EntityState.Detached;

			return importRun;
		}
		catch (Exception) { return null; }
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<int> UpdateImportRun(ImportRun importRun)
	{
		try
		{
			dbContext.Entry(importRun).State = EntityState.Modified;

			return await dbContext.SaveChangesAsync();
		}
		catch (Exception) { return -1; }
		finally { dbContext.ChangeTracker.Clear(); }
	}
}