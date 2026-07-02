using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Role;

public sealed class GetAllRolesQuery : IRequest<ReneeOperationResult<IEnumerable<RoleDto>>>;