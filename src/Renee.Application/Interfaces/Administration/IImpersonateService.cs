using Renee.Application.Commands.Administration;
using Renee.Application.DTOs.User;
using Renee.Application.Queries.Administration;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces.Administration;

public interface IImpersonateService
{
	Task<ReneeOperationResult<RegisteredUserDto?>> GetImpersonateForRegisteredUser(
		GetImpersonateForConnectedUserQuery getImpersonateForConnectedUser);

	Task<ReneeOperationResult<RegisteredUserDto?>> ImpersonateUser(ImpersonateUserCommand impersonateUserCommand);

	Task<ReneeOperationResult<bool>> StopImpersonation(StopImpersonationCommand stopImpersonationCommand);
}