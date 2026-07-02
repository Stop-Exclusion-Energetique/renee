using Renee.Application.Interfaces;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class ImportErrorRepository(
	ReneeDbContext dbContext,
	ITelemetryService telemetryService) : IImportErrorRepository
{
	public async Task<int> AddImportErrors(List<ImportError> importErrors)
	{
		try
		{
			dbContext.ImportErrors.AddRange(importErrors);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
			return -1;
		}
		finally { dbContext.ChangeTracker.Clear(); }
	}
}