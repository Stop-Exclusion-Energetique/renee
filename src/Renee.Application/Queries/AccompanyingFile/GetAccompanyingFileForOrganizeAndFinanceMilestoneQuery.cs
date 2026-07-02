using MediatR;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetAccompanyingFileForOrganizeAndFinanceMilestoneQuery(Guid id, Guid userId, string userRole)
	: IRequest<ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>>
{
	public Guid Id { get; } = id;
	public Guid UserId { get; } = userId;	
	public string UserRole { get; } = userRole;
}