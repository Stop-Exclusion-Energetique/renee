using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class InsuranceTypeRepository(ReneeDbContext dbContext) : IInsuranceTypeRepository
{
	public async Task<IEnumerable<InsuranceType>> GetAllAsync() =>
		await dbContext.InsuranceTypes.AsNoTracking().ToListAsync();
}