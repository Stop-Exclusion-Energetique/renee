using Renee.Application.Abstraction.Query;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class
	GetAccompanyingFileStatisticsForAssociationMemberQuery : IQuery<
	ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>>
{
	public required Guid ConnectedUserId { get; set; }
	public DateTime? DateFrom { get; set; }
	public DateTime? DateTo { get; set; }
	public List<Guid?>? SelectedReportingStructuresIds { get; set; }
}