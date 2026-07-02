using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record UpdateVersionCguForUserCommand(UserCguDto UserCGUDto) : IRequest<ReneeOperationResult<bool>>;