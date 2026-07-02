using Renee.Application.Commands.User;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IUserValidationService
{
	Task<bool> CreateUser(CreateUserCommand createUserCommand);
	Task<bool> DeleteUser(Guid id, string? mail);
	Task<ReneeOperationResult<IEnumerable<UserDto>>> GetAllSubscribedUsers();
	Task<ReneeOperationResult<UserDto?>> GetUserById(Guid id);
	Task<bool> RegisterUserInApplication(RegisterUserCommand registerUserCommand, string url);
	Task<bool> SendConfirmation(string? mail, string? randomCode);
	Task<ReneeOperationResult<bool?>> VerifyDuplicatedEmailAsync(string mail);
}