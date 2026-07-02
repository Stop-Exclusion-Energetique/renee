using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record UpdateLastLoginDateCommand(
	string Email) : IRequest<ReneeOperationResult<bool>>;
