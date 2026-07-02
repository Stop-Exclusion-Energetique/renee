using Renee.Application.Abstraction.Query;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetStatisticsForCoordinatorsQuery : IQuery<ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>>
{
	public required Guid ConnectedUserId { get; set; }
	public DateTime? DateFrom { get; set; }
	public DateTime? DateTo { get; set; }
	public bool ShouldFilterOnUserAllAccompanyingFile { get; set; }
	public List<Guid?>? SelectedReportingStructuresIds { get; set; }
	public List<Guid?>? SelectedSolidarBuilderIds { get; set; }
	public List<Guid?>? SelectedTerritoriesIds { get; set; }
	public bool IsTargetedCoordinator { get; set; }
}