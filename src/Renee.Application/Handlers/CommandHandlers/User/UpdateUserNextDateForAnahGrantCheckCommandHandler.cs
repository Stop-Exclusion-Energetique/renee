using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateUserNextDateForAnahGrantCheckCommandHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService) : IRequestHandler<UpdateUserNextDateForAnahGrantCheckCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateUserNextDateForAnahGrantCheckCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var updateResult = await userRepository.UpdateNextDateForAnahGrantCheck(request.UserId);
			return updateResult > 0
				? ReneeOperationResult<bool>.Success(true)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingAnahGrantCheckDate);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}