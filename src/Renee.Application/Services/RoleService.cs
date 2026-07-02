using MediatR;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Role;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class RoleService(IMediator mediator) : IRoleService
{
	public async Task<ReneeOperationResult<IEnumerable<RoleDto>>> GetAllAsync() => await mediator.Send(new GetAllRolesQuery());
}