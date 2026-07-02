using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class TaskRepository(ReneeDbContext dbContext) : ITaskRepository
{
	public async Task<int> AddTask(AccompanyingFileTask task)
	{
		try
		{
			await dbContext.AccompanyingFileTasks.AddAsync(task);

			return await dbContext.SaveChangesAsync();
		}
		catch { return -1; }
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<int> DeleteTask(Guid taskId)
	{
		try
		{
			var task = await dbContext.AccompanyingFileTasks.FindAsync(taskId);

			if (task is null) return -1;

			dbContext.AccompanyingFileTasks.Remove(task);

			return await dbContext.SaveChangesAsync();
		}
		catch { return -1; }
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<List<AccompanyingFileTask>> GetAccompanyingFileTasks(Guid AccompanyingFileId) =>
		await dbContext.AccompanyingFileTasks.Include(t => t.AssignedUser).AsNoTracking()
			.Where(t => t.AccompanyingFileId == AccompanyingFileId).ToListAsync();

	public async Task<AccompanyingFileTask?> GetTask(Guid taskId) =>
		await dbContext.AccompanyingFileTasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);

	public async Task<int> UpdateTaskStatus(AccompanyingFileTask task)
	{
		try
		{
			dbContext.Entry(task).State = EntityState.Modified;

			return await dbContext.SaveChangesAsync();
		}
		catch { return -1; }
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task ActivatePendingTasks(Guid accompanyingFileId)
	{
		await dbContext.AccompanyingFileTasks
			.Where(t => t.AccompanyingFileId == accompanyingFileId
				&& t.StartDate <= DateTime.Today
				&& t.Progress == null)
			.ExecuteUpdateAsync(s => s.SetProperty(t => t.Progress, (int)ProgressTask.ToBeCompletedTask));
	}

	public async Task<List<AccompanyingFileTask>> GetAllTasksForUser(Guid userId) =>
		await dbContext.AccompanyingFileTasks
			.Include(t => t.AssignedUser)
			.Include(t => t.AccompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
			.Include(t => t.AccompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation)
			.Where(t =>
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.SolidarBuilder == userId ||
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == userId ||
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == userId ||
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.TerritorialBuilder == userId ||
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder == userId ||
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator == userId ||
				t.AccompanyingFile.AccompanyingFileSupportTeamNavigation.TargetCoordinator == userId)
			.AsNoTracking()
			.ToListAsync();
}