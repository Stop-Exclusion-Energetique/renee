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

public class GetStatisticsForSolidarBuilderAndStructuralReferentQueryHandler(
	IUserRepository userRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetStatisticsForSolidarBuilderAndStructuralReferentQuery, ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>> HandleQuery(
		GetStatisticsForSolidarBuilderAndStructuralReferentQuery request)
	{
		try
		{
			var user = await userRepository.GetUserById(request.ConnectedUserId);

			if (user?.ReportingStructureId is null) return ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>.Success(new GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult());

			var accompanyingFiles = user.Role.Name switch
			{
				Constants.SolidarBuilderRole => request.ShouldFilterOnUserReportingStructure
										? await accompanyingFileRepository.GetStatisticsForSolidarBuilderReportingStructure(
											(Guid)user.ReportingStructureId, request.DateFrom, request.DateTo)
										: await accompanyingFileRepository.GetStatisticsForSolidarBuilderIndex(
											request.ConnectedUserId, request.DateFrom, request.DateTo),
				Constants.StructuralReferentRole => await accompanyingFileRepository.GetStatisticsForStructuralReferentIndex(
											(Guid)user.ReportingStructureId, request.DateFrom, request.DateTo),
				_ => throw new InvalidOperationException("User role not handled."),
			};
			return ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>.Success(new GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult
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
				HouseholdInCategoryAnahMCount =
					StatisticsHelper.GetHouseholdCategoryAnahCount(
						accompanyingFiles,
						Labels.LowIncomeHouseholdsAmount),
				HouseholdInCategoryAnahTmCount =
					StatisticsHelper.GetHouseholdCategoryAnahCount(
						accompanyingFiles,
						Labels.VeryLowIncomeHouseholdsAmount),
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
			return ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}