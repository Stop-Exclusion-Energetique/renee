using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record RegisterUserCommand(UserDto UserDto) : IRequest<ReneeOperationResult<bool>>;