using Renee.Application.DTOs.CguVersion;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface ICguVersionService
{
    Task<ReneeOperationResult<bool>> AddCGUVersion(CguVersionDto cguVersion);
    Task<ReneeOperationResult<CguVersionDto>> GetByVersionAsync(string version);
    Task<ReneeOperationResult<CguVersionDto>> GetLatestVersionAsync();
    Task<ReneeOperationResult<IEnumerable<CguVersionDto>>> GetAllVersionsAsync();
    Task<ReneeOperationResult<bool>> DeleteCguAsync(Guid id);
    Task<ReneeOperationResult<bool?>> VerifiyDuplicatedVersionAsync(string version);
}
