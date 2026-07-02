using Renee.Application.Abstraction.Query;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetStatisticsForTerritorialBuilderQuery : IQuery<ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>>
{
	public required Guid ConnectedUserId { get; set; }
	public DateTime? DateFrom { get; set; }
	public DateTime? DateTo { get; set; }
	public bool ShouldFilterOnUserAllAccompanyingFile { get; set; }
	public List<Guid?>? SelectedReportingStructuresIds { get; set; }
	public List<Guid?>? SelectedSolidarBuilderIds { get; set; }
	public List<DpeLabel?>? DpeLabelFilter { get; set; }
}

public record GetStatisticsForTerritorialBuilderQueryResult
{
	public string UserFullName { get; init; } = string.Empty;
	public int AccompanyingFileInIdentifyMilestoneCount { get; set; }
	public int AccompanyingFileInOrganizingAndFinancingMilestoneCount { get; set; }
	public int AccompanyingFileInRealizingAndFollowingMilestoneCount { get; set; }
	public double FinishedAccompanyingFile { get; set; }
	public double PassageRateFromFirstMilestoneToSecondMilestone { get; init; }
	public double PassageRateFromFirstMilestoneToThirdMilestone { get; set; }
	public Dictionary<EstimatedJumpClass, double> EstimatedEnergyJumpCount { get; init; } = new();
	public double HouseholdInCategoryAnahMCount { get; set; }
	public double HouseholdInCategoryAnahTmCount { get; set; }
	public Dictionary<AccompanyingFileStage, List<CompletionSpeedDataItem>> CompletionSpeedResult { get; set; } = [];
	public long EstimatedRemainingAmountAverage { get; set; }
	public long WorkPackageCostAverage { get; set; }
}

public class CompletionSpeedDataItem
{
	public int AccompanyingFileCount { get; set; }
	public string AbbreviatedMonth { get; set; } = string.Empty;
}