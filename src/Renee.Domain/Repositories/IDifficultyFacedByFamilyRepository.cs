using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IDifficultyFacedByFamilyRepository
{
	Task<List<HouseholdDifficultiesLabel>> GetAllAsync();
}