using Renee.Application.Abstraction.Query;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Coproperty;

public record GetCopropertyProfileForHeadBandQuery(Guid CopropertyProfileId)
    : IQuery<ReneeOperationResult<GetCopropertyProfileForHeadBandQueryObjectResult>>;

public class GetCopropertyProfileForHeadBandQueryObjectResult
{
    public required string CopropertyProfileReference { get; set; }
    public required AccompanyingFileStage AccompanyingFileStage { get; set; }
}