using Renee.Application.Abstraction.Query;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetAccompanyingFileForRealiseAndFollowMilestoneQuery(Guid id, Guid userId, string userRole)
	: IQuery<ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>>
{
	public Guid AccompanyingFileId { get; } = id;
	public Guid UserId { get; } = userId;
	public string UserRole { get; } = userRole;
}