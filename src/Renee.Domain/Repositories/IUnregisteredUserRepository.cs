using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IUnregisteredUserRepository
{
	Task<int> AddRegisteredUser(User user);
	Task<int> AddUser(UnregisteredUser user);
	Task<string?> AddUserInAzureAd(UnregisteredUser user);
	Task<int> DeleteUser(Guid id);
	Task<IEnumerable<UnregisteredUser>> GetAllSubscribedUsers();
	Task<UnregisteredUser?> GetUserById(Guid id);
	Task<bool?> VerifyDuplicateEmailAsync(string? email);
	Task<List<UnregisteredUser>> GetUntrackedReportingStructure();
	Task<int> UpdateUserReportingStructure(Guid id, Guid reportingStructureId);
}