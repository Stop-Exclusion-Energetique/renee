using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileForHeadBandQueryHandler(
	IAccompanyingFileRepository repository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAccompanyingFileForHeadBandQuery, ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>> HandleQuery(
		GetAccompanyingFileForHeadBandQuery request)
	{
		try
		{
			var accompanyingFile = await repository.GetBaseAccompanyingFile(request.AccompanyingFileId);

			return ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>.Success(new GetAccompanyingFileForHeadBandQueryObjectResult
			{
				AccompanyingFileReference = accompanyingFile.AccompanyingFileReference,
				AccompanyingFileStage = (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}