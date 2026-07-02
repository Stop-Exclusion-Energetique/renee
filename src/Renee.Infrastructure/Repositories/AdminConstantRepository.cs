using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class AdminConstantRepository(ReneeDbContext dbContext) : IAdminConstantRepository
{
    public async Task<List<AdminConstant>> GetAll()
    {
        return await dbContext.AdminConstants.AsNoTracking().ToListAsync();
    }

    public async Task<int> UpdateValue(List<AdminConstant> adminConstants)
    {
        dbContext.AdminConstants.UpdateRange(adminConstants);
        var result = await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();
        return result;
    }
}