using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public sealed class GetOrganizeAndFinanceAccompanyingFileValidationSynthesisModalByIdQuery(Guid accompanyingFileId)
	: IRequest<ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>>
{
	public Guid AccompanyingFileId { get; } = accompanyingFileId;
}