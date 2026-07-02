using Renee.Application.DTOs.AI;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IAIDossierSynthesisService
{
    Task<ReneeOperationResult<AISynthesisResult>> AnalyzeAsync(Guid accompanyingFileId);
}
