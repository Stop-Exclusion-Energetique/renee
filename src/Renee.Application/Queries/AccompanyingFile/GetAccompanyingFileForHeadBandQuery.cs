using Renee.Application.Abstraction.Query;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetAccompanyingFileForHeadBandQuery(Guid AccompanyingFileId)
	: IQuery<ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>>
{
}

public class GetAccompanyingFileForHeadBandQueryObjectResult
{
	public required string AccompanyingFileReference { get; set; }
	public required AccompanyingFileStage AccompanyingFileStage { get; set; }
}