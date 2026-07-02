using MediatR;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.UserCredential;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.UserCredential;

public class GetRegisteredUserOnCredentialsQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetRegisteredUserOnCredentialsQuery, ReneeOperationResult<RegisteredUserDto?>>
{
	public async Task<ReneeOperationResult<RegisteredUserDto?>> Handle(
		GetRegisteredUserOnCredentialsQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var user = await userRepository.GetRegisteredUserByMailAsync(request.Email);

			if (user is null) return ReneeOperationResult<RegisteredUserDto?>.Success(null);

			return ReneeOperationResult<RegisteredUserDto?>.Success(new RegisteredUserDto
			{
				Id = user.Id,
				UserName = $"{user.FirstName} {user.LastName}",
				Email = user.Email,
				RoleId = user.RoleId,
				RoleName = user.Role.Name,
				GroupSid = user.ReportingStructureNavigation!.Name,
				IsAccountDeleted = user.IsDeleted,
				TerritoryId = user.TerritoryId,
				LastLoginDate = user.LastLoginDate,
				NextDateForAnahGrantCheck = user.NextDateForAnahGrantCheck
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<RegisteredUserDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}