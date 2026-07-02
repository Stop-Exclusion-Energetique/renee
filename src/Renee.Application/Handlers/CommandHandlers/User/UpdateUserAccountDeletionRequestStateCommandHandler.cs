using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateUserAccountDeletionRequestStateCommandHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService) : IRequestHandler<UpdateUserAccountDeletionRequestStateCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateUserAccountDeletionRequestStateCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await userRepository.UpdateUserAccountDeletionRequestState(
				request.UserId,
				request.AccountDeletionRequestState);

			return result > 0
				? ReneeOperationResult<bool>.Success(true, Labels.UpdateAccountDeletionRequestSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingAccountDeletionRequest);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}