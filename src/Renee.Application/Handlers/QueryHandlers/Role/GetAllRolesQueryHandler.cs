using MediatR;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Role;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Role;

public class GetAllRolesQueryHandler(
	IRoleRepository roleRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllRolesQuery, ReneeOperationResult<IEnumerable<RoleDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var roles = await roleRepository.GetAllAsync();
			return ReneeOperationResult<IEnumerable<RoleDto>>.Success(roles.Select(c => new RoleDto { Id = c.Id, LongName = c.LongName }));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<RoleDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}