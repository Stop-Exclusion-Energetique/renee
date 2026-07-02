using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateUserAdminCommandHandler(
	IUserRepository repository,
	ITelemetryService telemetryService) 
	: IRequestHandler<UpdateUserCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateUserCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await repository.UpdateUserByAdmin(
				new Domain.Entity.User
				{
					Id = request.UserId,
					LastName = request.LastName,
					FirstName = request.FirstName,
					RoleId = request.RoleId,
					ReportingStructureId = request.ReportingStructureId,
				}
			);

			return result > 0
				? ReneeOperationResult<bool>.Success(true, Labels.UpdateUserSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingUser);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
