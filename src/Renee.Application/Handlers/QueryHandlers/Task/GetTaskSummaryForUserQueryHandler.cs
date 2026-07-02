using MediatR;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Task;

public class GetTaskSummaryForUserQueryHandler(
	ITaskRepository taskRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetTaskSummaryForUserQuery, ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>>
{
	public async Task<ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>> Handle(
		GetTaskSummaryForUserQuery request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.UserId == Guid.Empty)
				return ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>.Failure(Labels.Errors.UserNotFound);
			
			var tasks = await taskRepository.GetAllTasksForUser(request.UserId);

			return ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>.Success(
				[.. tasks
				.GroupBy(t => t.AccompanyingFileId)
				.Select(g =>
				{
					var first = g.First();
					var mainOccupant = first.AccompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation;
					var address = first.AccompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label;

					var allTasks = g.Select(t => new TaskObjectResult
					{
						TaskId = t.Id,
						TaskName = t.Title,
						Priority = (TaskPriority)t.Priority,
						DueDate = t.DueDate,
						StartDate = t.StartDate,
						AssignedUserName = $"{t.AssignedUser.FirstName} {t.AssignedUser.LastName}",
						ProgressTask = t.Progress.HasValue ? (ProgressTask)t.Progress.Value : null
					}).ToList();

					return new GetTaskSummaryForUserQueryObjectResult(
						first.AccompanyingFileId,
						$"{mainOccupant.FirstName} {mainOccupant.LastName}",
						address,
						allTasks.UpcomingTasks(),
						allTasks.ToBeCompletedTasks(),
						allTasks.CurrentTasks(),
						allTasks.DoneTasks()
					);
				})
				.OrderBy(r => r.MainOccupantFullName)
				.ThenBy(r => r.Address)]);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<List<GetTaskSummaryForUserQueryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}