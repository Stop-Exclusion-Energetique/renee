using Renee.Application.Interfaces;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class DocumentGenerationLogRepository(
	ReneeDbContext dbContext,
	ITelemetryService telemetryService) : IDocumentGenerationLogRepository
{
	public async Task<int> CreateDocumentGenerationLogAsync(DocumentGenerationLog entity)
	{
		try
		{
			await dbContext.DocumentGenerationLogs.AddAsync(entity);
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