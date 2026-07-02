using MediatR;
using Renee.Application.Commands.Mail;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class UserValidationService(IMediator mediator) : IUserValidationService
{
	public async Task<bool> CreateUser(CreateUserCommand createUserCommand)
	{
		if (string.IsNullOrEmpty(createUserCommand.UserDto.Email)) return false;
		var isUserCreated = await mediator.Send(createUserCommand);
		if (!isUserCreated.IsSuccess || !isUserCreated.Value) return false;

		var isUserMailSent = await mediator.Send(
			new SendMailCommand(MailType.InscriptionUserWaiting, createUserCommand.UserDto.Email));

		var adminMailsResult = await mediator.Send(new GetAllAdminUsersMailQuery());
		if (!adminMailsResult.IsSuccess || adminMailsResult.Value is null) return false;
		var isAdminMailSent = await mediator.Send(
			new SendMailCommand(
				MailType.InscriptionAdminWaiting, 
				adminMailsResult.Value, 
				new MailParameters { Param1 = createUserCommand.UserDto.Email }));

		return isUserMailSent.IsSuccess && isUserMailSent.Value && isAdminMailSent.IsSuccess && isAdminMailSent.Value;
	}

	public async Task<bool> DeleteUser(Guid id, string? mail)
	{
		var isDeleted = await mediator.Send(new DeleteUserCommand(id));
		if (!isDeleted.IsSuccess || !isDeleted.Value)
			return false;

		var isMailSent = await mediator.Send(new SendMailCommand(MailType.InscriptionRefused, mail));
		return isMailSent.IsSuccess && isMailSent.Value;
	}

	public async Task<ReneeOperationResult<IEnumerable<UserDto>>> GetAllSubscribedUsers() =>
		await mediator.Send(new GetAllSubscribedUsersQuery());

	public async Task<ReneeOperationResult<UserDto?>> GetUserById(Guid id) => await mediator.Send(new GetUserByIdQuery(id));

	public async Task<bool> RegisterUserInApplication(RegisterUserCommand registerUserCommand, string url)
	{
		if (string.IsNullOrEmpty(registerUserCommand.UserDto.Email)) return false;
		var defaultPassword = await mediator.Send(new CreateAzureUserCommand(registerUserCommand.UserDto));
		var isUserRegister = await mediator.Send(registerUserCommand);
		var isUserDeleted = await mediator.Send(new DeleteUserCommand(registerUserCommand.UserDto.Id));
		if (!isUserDeleted.IsSuccess || !isUserDeleted.Value || !isUserRegister.IsSuccess || !isUserRegister.Value || string.IsNullOrEmpty(defaultPassword.Value)) return false;
		var isUserMailSent = await mediator.Send(
			new SendMailCommand(
				MailType.InscriptionConfirmation,
				registerUserCommand.UserDto.Email,
				new MailParameters
				{
					Param1 = defaultPassword.Value,
					Param2 = url,
				}));

		return isUserMailSent.IsSuccess && isUserMailSent.Value;
	}

	public async Task<bool> SendConfirmation(string? mail, string? randomCode)
	{
		var result = await mediator.Send(new SendMailCommand(
			MailType.CreationCode, 
			mail,
			new MailParameters { Param1 = randomCode }));
		return result.IsSuccess && result.Value;
	}

	public async Task<ReneeOperationResult<bool?>> VerifyDuplicatedEmailAsync(string mail) =>
		await mediator.Send(new GetUserByEmailForDuplicateVerificationQuery(mail));
}