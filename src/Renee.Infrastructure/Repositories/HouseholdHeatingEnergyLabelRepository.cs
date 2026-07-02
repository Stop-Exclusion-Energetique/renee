using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class HouseholdHeatingEnergyLabelRepository(ReneeDbContext dbContext) : IHouseholdHeatingEnergyLabelRepository
{
	public async Task<List<HouseholdHeatingEnergyLabel>> GetAllHouseHoldHeatingEnergyLabelAsync()
		=> await dbContext.HouseholdHeatingEnergiesLabels.ToListAsync();
}
