using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class CreateAzureUserCommandHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateAzureUserCommand, ReneeStringOperationResult>
{
	public async Task<ReneeStringOperationResult> Handle(CreateAzureUserCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var user = new UnregisteredUser
			{
				Email = request.UserDto.Email,
				FirstName = request.UserDto.FirstName,
				LastName = request.UserDto.LastName,
				PhoneNumber = request.UserDto.PhoneNumber
			};
			var result = await unregisteredUserRepository.AddUserInAzureAd(user);
			return ReneeStringOperationResult.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileCreatingAzureUser);
		}
	}
}