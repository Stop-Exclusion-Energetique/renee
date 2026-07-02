using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateVersionCguForUserCommandHandler(IUserRepository userRepository, ITelemetryService telemetryService)
: IRequestHandler<UpdateVersionCguForUserCommand, ReneeOperationResult<bool>>
{
    public async Task<ReneeOperationResult<bool>> Handle(UpdateVersionCguForUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.UserCGUDto.IdUser == Guid.Empty ||
                request.UserCGUDto.ValidatedCGUVersion == string.Empty)
                return ReneeOperationResult<bool>.Failure(Labels.Errors.InvalidCguData);

            var numberItemsChanged = await userRepository.UpdateUserCGUAsync(request.UserCGUDto.IdUser, request.UserCGUDto.ValidatedCGUVersion!);

            return numberItemsChanged > -1
                ? ReneeOperationResult<bool>.Success(true, Labels.UpdateCguVersionSuccess)
                : ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingCguVersion);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}
