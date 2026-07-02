using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateAzureUserCommandHandler(IUserRepository userRepository, ITelemetryService telemetryService)
	: IRequestHandler<UpdateAzureUserCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateAzureUserCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(request.UserDto.Email))
				return ReneeOperationResult<bool>.Failure(Labels.Errors.EmailRequired);

			var entity = new Domain.Entity.User
			{
				FirstName = request.UserDto.FirstName,
				LastName = request.UserDto.LastName,
				Email = request.UserDto.Email,
				PhoneNumber = request.UserDto.PhoneNumber
			};
			var result = await userRepository.UpdateUserInAzureAd(entity, request.OldEmail);

			return result
				? ReneeOperationResult<bool>.Success(true, Labels.UpdateUserSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingAzureUser);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}