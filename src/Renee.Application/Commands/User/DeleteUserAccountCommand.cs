using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record DeleteUserAccountCommand(Guid UserId) : IRequest<ReneeOperationResult<bool>>;