using MediatR;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetAllSubscribedUsersQueryHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllSubscribedUsersQuery, ReneeOperationResult<IEnumerable<UserDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<UserDto>>> Handle(
		GetAllSubscribedUsersQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			return ReneeOperationResult<IEnumerable<UserDto>>.Success((await unregisteredUserRepository.GetAllSubscribedUsers()).Select(
				usr => new UserDto(usr.Id, usr.SubscriptionDateUtc, usr.LastName, usr.FirstName, usr.Email)));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<UserDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}