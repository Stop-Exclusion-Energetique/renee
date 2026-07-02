using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface ICguVersionRepository
{
    Task<int> AddCGUVersion( CguVersion cguVersion );
    Task<CguVersion?> GetByVersionAsync(string version);
    Task<CguVersion?> GetLatestVersionAsync();
    Task<List<CguVersion>> GetAllVersionsAsync();
    Task<int> UpdateCguAsync(CguVersion cguVersion);
    Task<bool> DeleteCguAsync(Guid id);
    Task<bool?> VerifiyDuplicatedVersionAsync(string version);
}
