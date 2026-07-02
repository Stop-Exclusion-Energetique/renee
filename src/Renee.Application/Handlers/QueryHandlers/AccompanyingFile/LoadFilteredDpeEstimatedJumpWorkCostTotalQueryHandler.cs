using Renee.Application.Abstraction.Query;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class LoadFilteredDpeEstimatedJumpWorkCostTotalQueryHandler(
	IUserRepository userRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<LoadFilteredDpeEstimatedJumpWorkCostTotalQuery, ReneeOperationResult<LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult>>
{
	public override async Task<ReneeOperationResult<LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult>> HandleQuery(
		LoadFilteredDpeEstimatedJumpWorkCostTotalQuery request)
	{
		try
		{
			var user = await userRepository.GetUserById(request.ConnectedUserId);

			if (user is null) return ReneeOperationResult<LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult>.Success(new LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult());

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

			return ReneeOperationResult<LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult>.Success(new LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult
			{
				EstimatedEnergyJumpCount = StatisticsHelper.GetAverageWorkCostByEnergyJump(
					accompanyingFiles,
					request.DpeLabelsFilter)
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}