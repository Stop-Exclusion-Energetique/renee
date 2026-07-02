using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Queries.Administration;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services.Administration;

public class ImpersonateService(IMediator mediator) : IImpersonateService
{
	public async Task<ReneeOperationResult<RegisteredUserDto?>> GetImpersonateForRegisteredUser(
		GetImpersonateForConnectedUserQuery getImpersonateForConnectedUser) =>
		await mediator.Send(getImpersonateForConnectedUser);

	public async Task<ReneeOperationResult<RegisteredUserDto?>> ImpersonateUser(ImpersonateUserCommand impersonateUserCommand) =>
		await mediator.Send(impersonateUserCommand);

	public async Task<ReneeOperationResult<bool>> StopImpersonation(StopImpersonationCommand stopImpersonationCommand) =>
		await mediator.Send(stopImpersonationCommand);
}