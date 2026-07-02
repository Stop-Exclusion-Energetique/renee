using Renee.Application.Abstraction.Query;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetStatisticsForTerritorialBuilderQueryHandler(
	IUserRepository userRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetStatisticsForTerritorialBuilderQuery, ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>>
{
	public override async Task<ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>> HandleQuery(
		GetStatisticsForTerritorialBuilderQuery request)
	{
		try
		{
			var user = await userRepository.GetUserById(request.ConnectedUserId);

			if (user is null) return ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>.Success(new GetStatisticsForTerritorialBuilderQueryResult());

			var filterOptions = new AccompanyingFilesStatisticsFilterOptions
			{
				ReportingStructures = request.SelectedReportingStructuresIds,
				SolidarBuilders = request.SelectedSolidarBuilderIds
			};

			var accompanyingFiles = await accompanyingFileRepository.GetStatisticsForTerritorialBuilderIndex(
				request.ConnectedUserId,
				request.DateFrom,
				request.DateTo,
				request.ShouldFilterOnUserAllAccompanyingFile,
				filterOptions);

			return ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>.Success(new GetStatisticsForTerritorialBuilderQueryResult
			{
				UserFullName = $"{user.FirstName} {user.LastName}",
				AccompanyingFileInIdentifyMilestoneCount =
					StatisticsHelper.GetAccompanyingFilesCount(accompanyingFiles, AccompanyingFileStage.Identify),
				AccompanyingFileInOrganizingAndFinancingMilestoneCount =
					StatisticsHelper.GetAccompanyingFilesCount(
						accompanyingFiles,
						AccompanyingFileStage.OrganizingAndFinancing),
				AccompanyingFileInRealizingAndFollowingMilestoneCount =
					StatisticsHelper.GetAccompanyingFilesCount(
						accompanyingFiles,
						AccompanyingFileStage.RealisationAndFollowing),
				FinishedAccompanyingFile =
					StatisticsHelper.GetAccompanyingFilesCount(accompanyingFiles, AccompanyingFileStage.Finished),
				PassageRateFromFirstMilestoneToSecondMilestone =
					StatisticsHelper.GetPassageRateFromFirstStageToSecondStage(accompanyingFiles),
				PassageRateFromFirstMilestoneToThirdMilestone =
					StatisticsHelper.GetPassageRateFromFirstStageToThirdStage(accompanyingFiles),
				EstimatedEnergyJumpCount =
					StatisticsHelper.GetAverageWorkCostByEnergyJump(accompanyingFiles, request.DpeLabelFilter),
				HouseholdInCategoryAnahMCount =
					StatisticsHelper.GetHouseholdCategoryAnahCount(
						accompanyingFiles,
						Labels.LowIncomeHouseholdsAmount),
				HouseholdInCategoryAnahTmCount = StatisticsHelper.GetHouseholdCategoryAnahCount(
					accompanyingFiles,
					Labels.VeryLowIncomeHouseholdsAmount),
				CompletionSpeedResult = StatisticsHelper.GetCompletionSpeed(accompanyingFiles, request.DateFrom, request.DateTo),
				EstimatedRemainingAmountAverage =
					accompanyingFiles.Count == 0
						? 0
						: (long)Math.Ceiling(
							accompanyingFiles.Average(StatisticsHelper.CalculateEstimatedRemainingAmount)),
				WorkPackageCostAverage =
					accompanyingFiles.Count == 0
						? 0
						: (long)Math.Ceiling(
							accompanyingFiles.Average(StatisticsHelper.CalculateTotalWorkPackageCosts)),
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}