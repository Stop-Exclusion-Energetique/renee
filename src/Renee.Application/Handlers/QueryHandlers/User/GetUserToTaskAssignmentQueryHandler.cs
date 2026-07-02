using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;
using DomainUser = Renee.Domain.Entity.User;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetUserToTaskAssignmentQueryHandler(
	IUserRepository userRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetUserToTaskAssignmentQuery, ReneeOperationResult<List<GetUserToTaskAssignmentQueryObjectResult>>>
{
	public override async Task<ReneeOperationResult<List<GetUserToTaskAssignmentQueryObjectResult>>> HandleQuery(
		GetUserToTaskAssignmentQuery request)
	{
		try
		{
			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileToRetrieveSupportTeam(request.AccompanyingFileId);

			if (accompanyingFile == null) return ReneeOperationResult<List<GetUserToTaskAssignmentQueryObjectResult>>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			var supportTeam = accompanyingFile.AccompanyingFileSupportTeamNavigation;

			var users = await userRepository.GetAllSolidarBuildersFromSameReportingStructure(request.UserId);

			users.AddRange(new DomainUser?[]
			{
				supportTeam.SolidarBuilderNavigation,
				supportTeam.SecondSolidarBuilderNavigation,
				supportTeam.ThirdSolidarBuilderNavigation,
				supportTeam.TerritorialBuilderNavigation,
				supportTeam.SecondTerritorialBuilderNavigation,
				supportTeam.DiffuseCoordinatorNavigation,
				supportTeam.TargetCoordinatorNavigation,
			}.OfType<DomainUser>());

			var result = users
				.DistinctBy(u => u.Id)
				.Select(u => new GetUserToTaskAssignmentQueryObjectResult(u.Id, $"{u.FirstName} {u.LastName}")).ToList();

			return ReneeOperationResult<List<GetUserToTaskAssignmentQueryObjectResult>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<List<GetUserToTaskAssignmentQueryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}