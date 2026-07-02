using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetAccompanyingFileSynthesisQuery(Guid id) : IRequest<ReneeOperationResult<AccompanyingFileSynthesisDto>>
{
	public Guid? Id { get; } = id;
}