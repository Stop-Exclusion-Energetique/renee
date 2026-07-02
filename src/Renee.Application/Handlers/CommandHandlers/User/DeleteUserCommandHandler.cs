using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class DeleteUserCommandHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<DeleteUserCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var numberItemsChanged = await unregisteredUserRepository.DeleteUser(request.Id);
			return numberItemsChanged > -1
				? ReneeOperationResult<bool>.Success(true, Labels.DeleteUserSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileDeletingUser);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}