using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetDuplicatedAccompanyingFileByExternalReferenceQuery(List<string?> references) 
    : IRequest<ReneeOperationResult<List<ExistingAccompanyingFileDto>>>
{
    public List<string?> References { get; } = references;
}
