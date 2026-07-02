using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IDocumentGenerationLogRepository
{
	Task<int> CreateDocumentGenerationLogAsync(DocumentGenerationLog entity);
}