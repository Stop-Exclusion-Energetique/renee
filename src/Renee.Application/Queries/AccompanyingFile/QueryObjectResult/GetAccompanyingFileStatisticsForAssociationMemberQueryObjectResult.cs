namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult
{
	public string UserFullName { get; set; } = string.Empty;
	public double OnHoldAccompanyingFile { get; set; }
	public double FinishedAccompanyingFile { get; set; }
	public long EstimatedRemainingAmountAverage { get; set; }
	public long WorkPackageCostAverage { get; set; }
}