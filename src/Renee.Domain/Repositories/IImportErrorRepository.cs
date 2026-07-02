using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IImportErrorRepository
{
	Task<int> AddImportErrors(List<ImportError> importErrors);
}