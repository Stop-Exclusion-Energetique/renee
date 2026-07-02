namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult
{
	public string UserFullName { get; set; } = string.Empty;
	public int AccompanyingFileInIdentifyMilestoneCount { get; set; }
	public int AccompanyingFileInOrganizingAndFinancingMilestoneCount { get; set; }
	public int AccompanyingFileInRealizingAndFollowingMilestoneCount { get; set; }
	public double HouseholdInCategoryAnahMCount { get; set; }
	public double HouseholdInCategoryAnahTmCount { get; set; }
	public double OnHoldAccompanyingFile { get; set; }
	public double FinishedAccompanyingFile { get; set; }
	public long EstimatedRemainingAmountAverage { get; set; }
	public long WorkPackageCostAverage { get; set; }
}