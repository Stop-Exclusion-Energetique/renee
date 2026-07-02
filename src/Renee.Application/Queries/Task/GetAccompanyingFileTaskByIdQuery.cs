using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.Task;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Task;

public class GetAccompanyingFileTaskByIdQuery(Guid id) : IQuery<ReneeOperationResult<TaskDto?>>
{
	public Guid Id { get; } = id;
}