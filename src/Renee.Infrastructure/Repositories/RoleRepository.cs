using Microsoft.EntityFrameworkCore;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class RoleRepository(IDbContextFactory<ReneeDbContext> contextFactory) : IRoleRepository
{
	public async Task<List<Role>> GetAllAsync()
	{
		var context = await contextFactory.CreateDbContextAsync();
		return await context.Roles.Where(r => r.IsVisibled && r.Name != Constants.TrustedTierRole).ToListAsync();
	}
}