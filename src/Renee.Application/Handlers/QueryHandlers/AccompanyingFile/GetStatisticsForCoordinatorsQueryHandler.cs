using Renee.Application.Abstraction.Query;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetStatisticsForCoordinatorsQueryHandler(
	IUserRepository userRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetStatisticsForCoordinatorsQuery, ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>> HandleQuery(
		GetStatisticsForCoordinatorsQuery request)
	{
		try
		{
			var user = await userRepository.GetUserById(request.ConnectedUserId);

			if (user is null)
				return ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Failure(Labels.Errors.UserNotFound);

			var (accompanyingFiles, durationAverage) =
				await accompanyingFileRepository.GetStatisticsForCoordinatorsIndex(
					request.ConnectedUserId,
					request.DateFrom,
					request.DateTo,
					request.ShouldFilterOnUserAllAccompanyingFile,
					new AccompanyingFilesStatisticsFilterOptions
					{
						ReportingStructures = request.SelectedReportingStructuresIds,
						SolidarBuilders = request.SelectedSolidarBuilderIds,
						Territories = request.SelectedTerritoriesIds
					},
					request.IsTargetedCoordinator);

			var averageAgeMainOccupant = StatisticsHelper.GetAverageAgeMainOccupant(accompanyingFiles);

			var averageDeliveryTime = StatisticsHelper.GetAverageDeliveryTime(accompanyingFiles);

			var averageTaxRevenue = StatisticsHelper.GetTaxRevenueAverage(accompanyingFiles);
			var averageEnergyEffortBeforeWork = StatisticsHelper.GetAverageEnergyEffortBeforeWork(accompanyingFiles);
			return ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(new GetStatisticsForCoordinatorsQueryObjectResult
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
				FinishedAccompanyingFile =
					StatisticsHelper.GetAccompanyingFilesCount(accompanyingFiles, AccompanyingFileStage.Finished),
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
				AverageAgeMainOccupant =
					accompanyingFiles.Count == 0 || averageAgeMainOccupant == null
						? 0
						: Math.Round(averageAgeMainOccupant.Value, 1),
				AverageDeliveryTime =
					accompanyingFiles.Count == 0 || averageDeliveryTime == null
						? 0
						: Math.Round(averageDeliveryTime.Value, 1),
				SocioProfessionalCategories =
					new GetStatisticsForCoordinatorsQueryObjectResult.SocioProfessionalCategoryStatistics
					{
						Farmer =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Farmer),
						Artisan =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Artisan),
						Cadre =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Cadre),
						Employee =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Employee),
						SearchingJob =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.SearchingJob),
						Worker =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Worker),
						IntermediateProfession =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.IntermediateProfession),
						Retired =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Retired),
						Unemployed =
							StatisticsHelper.SocioProfessionnalCategoryCount(
								accompanyingFiles,
								SocioProfessionalCategory.Unemployed)
					},
				HouseHoldsByMarkerNature = StatisticsHelper.GetHouseHoldsByMarkerNatureCount(accompanyingFiles),
				EnergyPrivationRate = StatisticsHelper.GetEnergyPrivationRate(accompanyingFiles),
				AverageEnergyEffortBeforeWork =
					accompanyingFiles.Count == 0 || averageEnergyEffortBeforeWork == null
						? 0
						: Math.Round(averageEnergyEffortBeforeWork.Value, 1),
				TaxRevenueAverage =
					accompanyingFiles.Count == 0 || averageTaxRevenue == null
						? 0
						: Math.Round(averageTaxRevenue.Value, 1),
				HouseHoldsByInitialDpe = StatisticsHelper.GetHouseHoldsByInitialDpeCount(accompanyingFiles),
				EstimatedEnergyClassJump = StatisticsHelper.GetEstimatedEnergyJumpCount(accompanyingFiles),
				OwnershipStatus = new GetStatisticsForCoordinatorsQueryObjectResult.OwnershipStatusStatistics
				{
					FullOwnership =
						StatisticsHelper.GetOwnershipStatusCount(
							accompanyingFiles,
							OwnershipStatus.FullOwnership),
					CoOwner =
						StatisticsHelper.GetOwnershipStatusCount(accompanyingFiles, OwnershipStatus.CoOwner),
					JointOwnership = StatisticsHelper.GetOwnershipStatusCount(accompanyingFiles, OwnershipStatus.JointOwnership),
                },
				HouseholdsByTypesOfANAH = StatisticsHelper.GetHouseholdsByTypesOfANAHCount(accompanyingFiles),
				HouseholdTypologies = StatisticsHelper.GetHouseholdTypologiesCount(accompanyingFiles),
				AverageFundingByType = StatisticsHelper.GetAverageFundingByType(accompanyingFiles),
				AverageAccompanyingDuration = durationAverage.AverageMonthsDifference ?? 0
			});
		}
		catch (Exception e)
		{
			await telemetryService.TrackExceptionAsync(e, new CancellationToken());
			return ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}