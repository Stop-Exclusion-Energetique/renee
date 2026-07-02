using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IAnahCategorySuplementaryOccupantIncomeRepository
{
	Task<List<AnahCategorySuplementaryOccupantIncome>> GetAnahCategorySuplementaryOccupantIncomesAsync();
	Task<int> AddNewAnahCategorySuplementaryOccupantIncome(AnahCategorySuplementaryOccupantIncome entityToAdd);
	Task<int> UpdateNewAnahCategorySuplementaryOccupantIncome(AnahCategorySuplementaryOccupantIncome entityToUpdate);
}
