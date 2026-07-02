using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record HasAccompanyingFilesAwaitingAnahResponseQuery(Guid UserId) : IRequest<ReneeOperationResult<bool>>;
