using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Coproperty;

public class DeleteCopropertyProfileCommandHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<DeleteCopropertyProfileCommandInput, ReneeOperationResult<bool>>
{
    public async Task<ReneeOperationResult<bool>> Handle(DeleteCopropertyProfileCommandInput request, CancellationToken cancellationToken)
    {
        try
        {
            var numberItemsChanged = await copropertyProfileRepository.DeleteCopropertyProfile(request.Id);
            return numberItemsChanged > -1
                ? ReneeOperationResult<bool>.Success(true, Labels.DeleteCopropertyProfileSuccess)
                : ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileDeletingCopropertyProfile);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}
