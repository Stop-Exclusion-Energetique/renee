using Renee.Application.Abstraction.Query;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileStatisticsForAssociationMemberQueryHandler(
	IUserRepository userRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAccompanyingFileStatisticsForAssociationMemberQuery,
		ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>> HandleQuery(
		GetAccompanyingFileStatisticsForAssociationMemberQuery request)
	{
		try
		{
			var user = await userRepository.GetUserById(request.ConnectedUserId);

			if (user is null) return ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>.Success(new GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult());

			var accompanyingFiles = await accompanyingFileRepository.GetAccompanyingFileStatisticsForAssociationMember(
				request.DateFrom,
				request.DateTo,
				request.SelectedReportingStructuresIds);

			return ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>.Success(new GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult
			{
				UserFullName = $"{user.FirstName} {user.LastName}",
				OnHoldAccompanyingFile = StatisticsHelper.GetAccompanyingFilesCount(accompanyingFiles),
				FinishedAccompanyingFile =
					StatisticsHelper.GetAccompanyingFilesCount(accompanyingFiles, AccompanyingFileStage.Finished),
				EstimatedRemainingAmountAverage =
					accompanyingFiles.Count == 0
						? 0
						: (long)Math.Ceiling(
							accompanyingFiles.Average(StatisticsHelper.CalculateEstimatedRemainingAmount)),
				WorkPackageCostAverage = accompanyingFiles.Count == 0
					? 0
					: (long)Math.Ceiling(accompanyingFiles.Average(StatisticsHelper.CalculateTotalWorkPackageCosts))
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}