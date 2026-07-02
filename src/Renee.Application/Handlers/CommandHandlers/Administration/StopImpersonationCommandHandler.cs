using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Administration;

public class StopImpersonationCommandHandler(
	IImpersonateRepository impersonateRepository,
	ITelemetryService telemetryService) : IRequestHandler<StopImpersonationCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(StopImpersonationCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await impersonateRepository.DeleteImpersonateUserAsync(request.UserId);
			return result
				? ReneeOperationResult<bool>.Success(true)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileStoppingImpersonation);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, CancellationToken.None);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}