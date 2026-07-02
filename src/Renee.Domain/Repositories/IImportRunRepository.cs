using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IImportRunRepository
{
	Task<int> AddImportRun(ImportRun importRun);
	Task<ImportRun?> GetImportRunById(Guid importRunId);
	Task<int> UpdateImportRun(ImportRun importRun);
}