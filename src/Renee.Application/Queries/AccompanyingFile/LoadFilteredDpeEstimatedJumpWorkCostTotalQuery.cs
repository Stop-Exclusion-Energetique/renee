using Renee.Application.Abstraction.Query;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class
	LoadFilteredDpeEstimatedJumpWorkCostTotalQuery : IQuery<ReneeOperationResult<LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult>>
{
	public required Guid ConnectedUserId { get; set; }
	public DateTime? DateFrom { get; set; }
	public DateTime? DateTo { get; set; }
	public bool ShouldFilterOnUserAllAccompanyingFile { get; set; }
	public List<Guid?>? SelectedReportingStructuresIds { get; set; }
	public List<Guid?>? SelectedSolidarBuilderIds { get; set; }
	public List<DpeLabel?>? DpeLabelsFilter { get; set; }
}

public record LoadFilteredDpeEstimatedJumpWorkCostTotalQueryResult
{
	public Dictionary<EstimatedJumpClass, double> EstimatedEnergyJumpCount { get; init; } = new();
}