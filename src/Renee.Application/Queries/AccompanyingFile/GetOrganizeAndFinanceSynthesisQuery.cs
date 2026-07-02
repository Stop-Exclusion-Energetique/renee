using MediatR;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetOrganizeAndFinanceSynthesisQuery(Guid id) : IRequest<ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>>
{
	public Guid Id { get; } = id;
}