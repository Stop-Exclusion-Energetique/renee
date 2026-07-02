using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateLastLoginDateCommandHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService) : IRequestHandler<UpdateLastLoginDateCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateLastLoginDateCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await userRepository.UpdateLastLoginDate(
				request.Email);
			return result > 0
				? ReneeOperationResult<bool>.Success(true)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingLastLoginDate);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}