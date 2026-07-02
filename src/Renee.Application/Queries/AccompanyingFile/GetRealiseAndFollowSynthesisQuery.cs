using MediatR;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetRealiseAndFollowSynthesisQuery(Guid id) : IRequest<ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>>
{
	public Guid Id { get; } = id;
}