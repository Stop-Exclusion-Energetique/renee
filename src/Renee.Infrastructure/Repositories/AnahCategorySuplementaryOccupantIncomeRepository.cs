using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class AnahCategorySuplementaryOccupantIncomeRepository(ReneeDbContext dbContext) : IAnahCategorySuplementaryOccupantIncomeRepository
{
    public async Task<List<AnahCategorySuplementaryOccupantIncome>> GetAnahCategorySuplementaryOccupantIncomesAsync()
		=> await dbContext
					.AnahCategorySuplementaryOccupantIncome
					.AsNoTracking()
					.ToListAsync();

	public async Task<int> AddNewAnahCategorySuplementaryOccupantIncome(AnahCategorySuplementaryOccupantIncome entityToAdd)
    {
		try
		{
			await dbContext.AnahCategorySuplementaryOccupantIncome.AddAsync(entityToAdd);
			return await dbContext.SaveChangesAsync();
		}
		catch { return -1; }
		finally {dbContext.ChangeTracker.Clear();}
    }

    public async Task<int> UpdateNewAnahCategorySuplementaryOccupantIncome(AnahCategorySuplementaryOccupantIncome entityToUpdate)
    {
        try
		{
			dbContext.Entry(entityToUpdate).State = EntityState.Modified;

			var result = await dbContext.SaveChangesAsync();
			return result;
		}
		catch { return -1; }
		finally {dbContext.ChangeTracker.Clear();}
    }
}
