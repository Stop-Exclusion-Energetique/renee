using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class DeleteUserAccountCommandHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService) : IRequestHandler<DeleteUserAccountCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(DeleteUserAccountCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await userRepository.DeleteUserAccount(request.UserId);

			return result
				? ReneeOperationResult<bool>.Success(true, Labels.DeleteUserAccountSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileDeletingUserAccount);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
