using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Administration;

public class ImpersonateUserCommandHandler(
	IImpersonateRepository impersonateRepository,
	IUserRepository userRepository,
	ITelemetryService telemetryService) : IRequestHandler<ImpersonateUserCommand, ReneeOperationResult<RegisteredUserDto?>>
{
	public async Task<ReneeOperationResult<RegisteredUserDto?>> Handle(ImpersonateUserCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var user = new Impersonate
			{
				UserId = request.UserId, ImpersonateUserId = request.ImpersonateUserId, CreatedAt = DateTime.UtcNow
			};
			await impersonateRepository.CreateImpersonateUserAsync(user);
			var impersonateUser = await userRepository.GetUserById(request.ImpersonateUserId);
			if (impersonateUser != null)
				return ReneeOperationResult<RegisteredUserDto?>.Success(new RegisteredUserDto
				{
					Id = impersonateUser.Id,
					UserName = impersonateUser.FirstName,
					Email = impersonateUser.Email,
					RoleId = impersonateUser.RoleId,
					RoleName = impersonateUser.Role.Name,
					GroupSid = impersonateUser.ReportingStructureNavigation!.Name,
					IsAccountDeleted = impersonateUser.IsDeleted,
					LastLoginDate = impersonateUser.LastLoginDate
				}, Labels.ImpersonateUserSuccess);
			return ReneeOperationResult<RegisteredUserDto?>.Failure(Labels.Errors.ImpersonateUserNotFound);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, CancellationToken.None);
			return ReneeOperationResult<RegisteredUserDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}