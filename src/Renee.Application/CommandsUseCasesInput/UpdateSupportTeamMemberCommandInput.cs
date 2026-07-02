using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record UpdateSupportTeamMemberCommandInput(
	Guid AssociatedResourceId,
	string SupportTeamMemberRole,
	Guid? AssignedTo,
	bool IsCoproperty) : IRequest<ReneeOperationResult<int>>;