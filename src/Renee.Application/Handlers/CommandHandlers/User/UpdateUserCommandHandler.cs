using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class UpdateUserCommandHandler(IUserRepository userRepository, ITelemetryService telemetryService)
	: IRequestHandler<UpdateUserCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateUserCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.UserDto.Id == Guid.Empty ||
				request.UserDto.LastName == string.Empty ||
				request.UserDto.FirstName == string.Empty ||
				request.UserDto.Email == string.Empty ||
				request.UserDto.PhoneNumber == string.Empty ||
				request.UserDto.SiretNumber == string.Empty)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.InvalidUserData);

			var entity = new Domain.Entity.User
			{
				Id = request.UserDto.Id,
				FirstName = request.UserDto.FirstName,
				LastName = request.UserDto.LastName,
				Email = request.UserDto.Email,
				PhoneNumber = request.UserDto.PhoneNumber,
				TerritoryId = request.UserDto.TerritoryId,
				SiretNumber = request.UserDto.SiretNumber
			};
			var numberItemsChanged = await userRepository.UpdateUserAsync(entity);

			return numberItemsChanged > -1
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