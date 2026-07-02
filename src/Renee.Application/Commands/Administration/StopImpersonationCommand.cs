using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Administration;

public record StopImpersonationCommand(Guid UserId) : IRequest<ReneeOperationResult<bool>>;