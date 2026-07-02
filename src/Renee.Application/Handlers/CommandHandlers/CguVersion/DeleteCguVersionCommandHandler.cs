using MediatR;
using Renee.Application.Commands.CguVersion;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.CguVersion;

public class DeleteCguVersionCommandHandler(ICguVersionRepository cguVersionRepository,
    ITelemetryService telemetryService) : IRequestHandler<DeleteCguVersionCommand, ReneeOperationResult<bool>>
{
    public async Task<ReneeOperationResult<bool>> Handle(DeleteCguVersionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await cguVersionRepository.DeleteCguAsync(request.CguId);
            return result
                ? ReneeOperationResult<bool>.Success(true, Labels.DeleteCguVersionSuccess)
                : ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileDeletingCguVersion);
        }
        catch (Exception ex) {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}
