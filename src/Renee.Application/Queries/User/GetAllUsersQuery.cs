using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public sealed class GetAllUsersQuery : IRequest<ReneeOperationResult<IEnumerable<UserDto>>>;