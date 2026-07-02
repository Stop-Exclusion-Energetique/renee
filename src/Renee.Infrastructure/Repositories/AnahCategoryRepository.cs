using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class AnahCategoryRepository(ReneeDbContext dbContext) : IAnahCategoryRepository
{
	public async Task<int> CreateAnahCategoryAsync(AnahCategory entity)
	{
		try
		{
			await dbContext.AnahCategories.AddAsync(entity);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception) { return -1; }
	}

	public async Task<List<AnahCategory>> GetAllAnahCategoriesAsync() =>
		await dbContext.AnahCategories.AsNoTracking().OrderBy(x => x.PeopleNumber).ToListAsync();

	public async Task<int> UpdateAnahCategoryAsync(AnahCategory entity)
	{
		try
		{
			dbContext.Entry(entity).State = EntityState.Detached;
			var dbEntity = await dbContext.AnahCategories.FindAsync(entity.Id);
			if (dbEntity != null)
			{
				dbEntity.PeopleNumber = entity.PeopleNumber;
				dbEntity.LowIncomeHouseholdsAmount = entity.LowIncomeHouseholdsAmount;
				dbEntity.IsInIleDeFrance = entity.IsInIleDeFrance;
				dbEntity.VeryLowIncomeHouseholdsAmount = entity.VeryLowIncomeHouseholdsAmount;
				dbEntity.AnahRuleDebutDate = entity.AnahRuleDebutDate;
				dbEntity.AnahRuleEndDate = entity.AnahRuleEndDate;
				dbEntity.LastUpdateById = entity.LastUpdateById;
				dbEntity.LastUpdateDatetimeUtc = entity.LastUpdateDatetimeUtc;
				
				dbContext.Entry(dbEntity).State = EntityState.Modified;
				dbContext.AnahCategories.Update(dbEntity);
			}

			var result = await dbContext.SaveChangesAsync();
			return result;
		}
		catch (Exception) { return -1; }
	}
}