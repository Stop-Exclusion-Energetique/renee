using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record CreateUserCommand(UserDto UserDto) : IRequest<ReneeOperationResult<bool>>;