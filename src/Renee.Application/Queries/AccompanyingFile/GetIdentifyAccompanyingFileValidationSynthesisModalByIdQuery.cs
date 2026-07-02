using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public sealed class GetIdentifyAccompanyingFileValidationSynthesisModalByIdQuery(Guid accompanyingFileId)
	: IRequest<ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>>
{
	public Guid AccompanyingFileId { get; } = accompanyingFileId;
}