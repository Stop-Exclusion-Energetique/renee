using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IDifficultyFacedByFamilyService
{
	Task<ReneeOperationResult<IEnumerable<DifficultyFacedFamilyDto>>> GetAllAsync();
}