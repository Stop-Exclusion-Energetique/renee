using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface ICguValidationService
{

    Task<ReneeOperationResult<UserVersionCguDto>> InitializeCGUVerificationAsync(Guid id);
    Task<ReneeOperationResult<bool>> MarkCGUAsAcceptedAsync(Guid id, string? cgu);

}
