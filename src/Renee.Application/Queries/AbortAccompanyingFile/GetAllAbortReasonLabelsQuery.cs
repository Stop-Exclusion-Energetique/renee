using MediatR;
using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.AbortReasonLabel;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AbortAccompanyingFile;

public record GetAllAbortReasonLabelsQuery : IQuery<ReneeOperationResult<List<AbortReasonLabelDto>>>;