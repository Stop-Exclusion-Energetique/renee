using MediatR;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Administration;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Administration;

public class GetImpersonateForConnectedUserQueryHandler(
	IImpersonateRepository impersonateRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetImpersonateForConnectedUserQuery, ReneeOperationResult<RegisteredUserDto?>>
{
	public async Task<ReneeOperationResult<RegisteredUserDto?>> Handle(
		GetImpersonateForConnectedUserQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var user = await impersonateRepository.GetRegisteredUserByIdAsync(request?.Id);

			if (user is null) return ReneeOperationResult<RegisteredUserDto?>.Failure(Labels.Errors.UserNotFound);

			return ReneeOperationResult<RegisteredUserDto?>.Success(new RegisteredUserDto
			{
				Id = user.Id,
				UserName = user.FirstName,
				Email = user.Email,
				RoleId = user.RoleId,
				RoleName = user.Role.Name,
				GroupSid = user.ReportingStructureNavigation!.Name,
				IsAccountDeleted = user.IsDeleted,
				TerritoryId = user.TerritoryId,
				LastLoginDate = user.LastLoginDate
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<RegisteredUserDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}