using MediatR;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetAllUsersQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllUsersQuery, ReneeOperationResult<IEnumerable<UserDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		try
		{
			return ReneeOperationResult<IEnumerable<UserDto>>.Success((await userRepository.GetAllUsers()).Select(usr => new UserDto(
				usr.Id,
				usr.LastName,
				usr.FirstName,
				usr.ReportingStructureNavigation != null
					? new ReportingStructureDto(usr.ReportingStructureNavigation.Id, usr.ReportingStructureNavigation.Name, usr.ReportingStructureNavigation.NationalStructureId)
					: new ReportingStructureDto(Guid.Empty, string.Empty, null))));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<UserDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}