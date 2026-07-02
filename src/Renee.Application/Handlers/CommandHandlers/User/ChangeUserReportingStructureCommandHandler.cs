using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class ChangeUserReportingStructureCommandHandler(
	ITelemetryService telemetryService,
	IUnregisteredUserRepository unregisteredUserRepository) : IRequestHandler<ChangeUserReportingStructureCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(ChangeUserReportingStructureCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await unregisteredUserRepository.UpdateUserReportingStructure(request.UserId, request.ReportingStructureId);

			return result > 0
				? ReneeOperationResult<bool>.Success(true, Labels.ChangeReportingStructureSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileChangingReportingStructure);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken()); 
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
