using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IAbortReasonLabelRepository
{
	Task<List<AbortReasonLabel>> GetAllAsync();
}