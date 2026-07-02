using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Coproperty;

public class GetCopropertyProfileForHeadBandQueryHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : QueryHandler<GetCopropertyProfileForHeadBandQuery, ReneeOperationResult<GetCopropertyProfileForHeadBandQueryObjectResult>>
{
    public override async Task<ReneeOperationResult<GetCopropertyProfileForHeadBandQueryObjectResult>> HandleQuery(
        GetCopropertyProfileForHeadBandQuery request)
    {
        try
        {
            var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileForHeadBand(request.CopropertyProfileId);

            return ReneeOperationResult<GetCopropertyProfileForHeadBandQueryObjectResult>.Success(new GetCopropertyProfileForHeadBandQueryObjectResult
            {
                CopropertyProfileReference = copropertyProfile?.CopropertyReference ?? string.Empty,
                AccompanyingFileStage = (AccompanyingFileStage?)copropertyProfile?.CopropertyMilestone ?? AccompanyingFileStage.Identify
            });
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            return ReneeOperationResult<GetCopropertyProfileForHeadBandQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}
