using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record UpdateUserAccountDeletionRequestStateCommand(
	Guid UserId,
	bool AccountDeletionRequestState) : IRequest<ReneeOperationResult<bool>>;