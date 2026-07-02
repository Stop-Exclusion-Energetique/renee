using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record UpdateUserNextDateForAnahGrantCheckCommand(Guid UserId) : IRequest<ReneeOperationResult<bool>>;