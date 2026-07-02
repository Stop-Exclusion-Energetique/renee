using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record UpdateAzureUserCommand(UserDto UserDto, string OldEmail) : IRequest<ReneeOperationResult<bool>>;