using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain.ReneeError;
using UpdateUserCommandInput = Renee.Application.CommandsUseCasesInput.UpdateUserCommandInput;

namespace Renee.Application.Services;

public sealed class UserService(IMediator mediator) : IUserService
{
	public async Task<ReneeOperationResult<bool>> DeleteUserAccount(Guid userId) => await mediator.Send(new DeleteUserAccountCommand(userId));

	public async Task<ReneeOperationResult<IEnumerable<UserDto>>> GetAllUsers() => await mediator.Send(new GetAllUsersQuery());

	public async Task<ReneeOperationResult<UserDto?>> GetRegisteredUserById(Guid id) =>
		await mediator.Send(new GetRegisteredUserByIdQuery(id));

	public async Task<ReneeOperationResult<Guid?>> GetUserIdByFullName(string fullName) =>
		await mediator.Send(new GetUserIdByFullNameQuery(fullName));

	public async Task<ReneeOperationResult<UserDto?>> GetUserByEmail(string? userEmail, string? externalReference) =>
		await mediator.Send(new GetUserByEmailQuery(userEmail, externalReference));

	public async Task<ReneeOperationResult<bool>> UpdateUser(UserDto dto) =>
		await mediator.Send(new Commands.User.UpdateUserCommandInput(dto));

	public async Task<ReneeOperationResult<bool>> UpdateUserAccountDeletionRequestState(Guid userId, bool isAccountDeletionRequest) =>
		await mediator.Send(new UpdateUserAccountDeletionRequestStateCommand(userId, isAccountDeletionRequest));

	public async Task<ReneeOperationResult<bool>> UpdateUserAD(UserDto dto, string oldEmail) =>
		await mediator.Send(new UpdateAzureUserCommand(dto, oldEmail));

	public async Task<ReneeOperationResult<bool>> UpdateUserByAdmin(UpdateUserCommandInput input) => await mediator.Send(input);

	public async Task<ReneeOperationResult<bool>> UpdateUserLastLoginDate(string email) =>
		await mediator.Send(new UpdateLastLoginDateCommand(email));

	public async Task<ReneeOperationResult<bool?>> VerifyDuplicatedEmailAsync(string mail) =>
		await mediator.Send(new GetRegisteredUserByEmailForDuplicateVerificationQuery(mail));
}