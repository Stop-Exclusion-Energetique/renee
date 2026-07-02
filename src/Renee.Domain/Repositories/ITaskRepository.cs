using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface ITaskRepository
{
	Task<int> AddTask(AccompanyingFileTask task);
	Task<int> DeleteTask(Guid taskId);
	Task<List<AccompanyingFileTask>> GetAccompanyingFileTasks(Guid AccompanyingFileId);
	Task<AccompanyingFileTask?> GetTask(Guid taskId);
	Task<int> UpdateTaskStatus(AccompanyingFileTask task);
	Task ActivatePendingTasks(Guid accompanyingFileId);
	Task<List<AccompanyingFileTask>> GetAllTasksForUser(Guid userId);
}