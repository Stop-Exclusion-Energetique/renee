using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IImpersonateRepository
{
	Task<Impersonate?> CreateImpersonateUserAsync(Impersonate impersonate);

	Task<bool> DeleteImpersonateUserAsync(Guid? userId);
	Task<User?> GetRegisteredUserByIdAsync(Guid? id);
}