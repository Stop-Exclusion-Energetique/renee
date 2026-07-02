using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record UpdateUserCommandInput(
	Guid UserId, 
	string LastName, 
	string FirstName, 
	Guid RoleId, 
	Guid ReportingStructureId
) : IRequest<ReneeOperationResult<bool>>;
