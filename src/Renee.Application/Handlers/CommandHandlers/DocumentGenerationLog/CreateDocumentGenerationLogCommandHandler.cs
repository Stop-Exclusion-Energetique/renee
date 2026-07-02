using Renee.Application.Abstraction.Query;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.DocumentGenerationLog;

public class CreateDocumentGenerationLogCommandHandler(
	IDocumentGenerationLogRepository documentGenerationLogRepository,
	ITelemetryService telemetryService
) : QueryHandler<CreateDocumentGenerationLogCommandInput, int>
{
	public override async Task<int> HandleQuery(CreateDocumentGenerationLogCommandInput request)
	{
		try
		{
			var logEntry = new Domain.Entity.DocumentGenerationLog
			{
				UserId = request.UserId,
				FileName = request.FileName,
				GeneratedAt = DateTime.UtcNow
			};

			return await documentGenerationLogRepository.CreateDocumentGenerationLogAsync(logEntry);	
		}
		catch (ArgumentNullException ex)
		{
			await telemetryService.TrackExceptionAsync(ex, CancellationToken.None);
			return -1;
		}
	}
}