using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Administration;

public record ImpersonateUserCommand(Guid UserId, Guid ImpersonateUserId) : IRequest<ReneeOperationResult<RegisteredUserDto?>>;