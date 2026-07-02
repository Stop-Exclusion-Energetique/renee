using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class RegisterUserCommandHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<RegisterUserCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(request.UserDto.Email) ||
				request.UserDto.ReportingStructureDto?.Id == Guid.Empty ||
				!request.UserDto.AskedRole.HasValue)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.InvalidUserData);
			var user = new Domain.Entity.User
			{
				RoleId = request.UserDto.AskedRole.Value,
				Email = request.UserDto.Email,
				FirstName = request.UserDto.FirstName,
				Function = request.UserDto.Function,
				Id = request.UserDto.Id,
				LastName = request.UserDto.LastName,
				PhoneNumber = request.UserDto.PhoneNumber,
				ReportingStructureId = request.UserDto.ReportingStructureDto?.Id,
				TerritoryId = request.UserDto.TerritoryId,
				SiretNumber = request.UserDto.SiretNumber,
				LastValidatedCGUVersion = request.UserDto.CguVersion,
				LastValidatedCGUDate = DateTime.Now,
				NextDateForAnahGrantCheck = DateTime.UtcNow
			};
			var numberItemsChanged = await unregisteredUserRepository.AddRegisteredUser(user);

			return numberItemsChanged > -1
				? ReneeOperationResult<bool>.Success(true, Labels.RegisterUserSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileRegisteringUser);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}