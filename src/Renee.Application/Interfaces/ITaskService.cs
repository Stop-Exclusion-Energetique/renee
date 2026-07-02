using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.Task;
using Renee.Application.Queries.Task;
using Renee.Domain.ReneeError;
namespace Renee.Application.Interfaces;

public interface ITaskService
{
	Task<ReneeOperationResult<int>> AddTask(CreateAddTaskCommandInput request);
	Task ActivatePendingTasks(Guid accompanyingFileId);
	Task<ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>> GetTaskSummaryForUser(Guid userId);
	Task<ReneeOperationResult<int>> DeleteTask(Guid taskId, Guid deletedByUserId);
	Task<ReneeOperationResult<TaskDto?>> GetAccompanyingFileTaskById(Guid id);
	Task<ReneeOperationResult<int>> UpdateTask(UpdateTaskCommandInput input);
}