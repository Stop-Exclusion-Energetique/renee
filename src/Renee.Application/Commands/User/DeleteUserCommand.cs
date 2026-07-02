using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record DeleteUserCommand(Guid Id) : IRequest<ReneeOperationResult<bool>>;