using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IUserRepository
{
	Task<int> CountAllUser();
	Task<bool> DeleteUserAccount(Guid userId);
	Task<List<User>> GetAllAdminUsers();
	Task<List<User>> GetAllRegisteredUser();
	Task<List<User>> GetAllUntrackedAccountDeletionRequests();
	Task<List<User>> GetAllUsers();
	Task<User?> GetRegisteredUserByMailAsync(string registeredUserMail);
	Task<Guid?> GetSolidarBuilderUserId(string firstName, string lastName);
	Task<User?> GetUserById(Guid id);
	Task<List<User>> GetUsersByRole(string role, bool shouldDisplayFakeUser);
	Task<List<User>> GetUsersForQuickAdd();
	Task<List<User>> GetAllSolidarBuildersFromSameReportingStructure(Guid userId);
    Task<User?> GetUserByEmail(string? userEmail);
    Task<int> UpdateUserAccountDeletionRequestState(Guid userId, bool isAccountDeletionRequested);
	Task<int> UpdateUserAsync(User entity);
    Task<int> UpdateUserCGUAsync(Guid userId, string cgU);
    Task<int> UpdateUserByAdmin(User user);
	Task<bool> UpdateUserInAzureAd(User entity, string oldEmail);
	Task<int> UpdateLastLoginDate(string email);
	Task<bool?> VerifyDuplicatedEmailAsync(string? mail);
	Task<int> UpdateNextDateForAnahGrantCheck(Guid userId);
}