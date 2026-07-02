using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IUserService
{
	Task<ReneeOperationResult<bool>> DeleteUserAccount(Guid userId);
	Task<ReneeOperationResult<IEnumerable<UserDto>>> GetAllUsers();
	Task<ReneeOperationResult<UserDto?>> GetRegisteredUserById(Guid id);
	Task<ReneeOperationResult<Guid?>> GetUserIdByFullName(string fullName);
	Task<ReneeOperationResult<UserDto?>> GetUserByEmail(string? userEmail, string? externalReference);
	Task<ReneeOperationResult<bool>> UpdateUser(UserDto dto);
	Task<ReneeOperationResult<bool>> UpdateUserAccountDeletionRequestState(Guid userId, bool isAccountDeletionRequest);
	Task<ReneeOperationResult<bool>> UpdateUserAD(UserDto dto, string oldEmail);
	Task<ReneeOperationResult<bool>> UpdateUserByAdmin(UpdateUserCommandInput input);
	Task<ReneeOperationResult<bool>> UpdateUserLastLoginDate(string email);
	Task<ReneeOperationResult<bool?>> VerifyDuplicatedEmailAsync(string mail);
}