using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IWorkTypesLabelsRepository
{
	Task<IEnumerable<WorkTypesLabel>> GetAllAsync();
}