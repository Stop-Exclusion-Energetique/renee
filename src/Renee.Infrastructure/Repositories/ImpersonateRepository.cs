using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class ImpersonateRepository(
    IDbContextFactory<ReneeDbContext> contextFactory) : IImpersonateRepository
{
	public async Task<Impersonate?> CreateImpersonateUserAsync(Impersonate impersonate)
	{
        using var context = await contextFactory.CreateDbContextAsync();
        await context.Impersonates.AddAsync(impersonate);
		await context.SaveChangesAsync();
		return impersonate;
	}

	public async Task<bool> DeleteImpersonateUserAsync(Guid? userId)
	{
        using var context = await contextFactory.CreateDbContextAsync();
        var impersonate =
			await context.Impersonates.FirstOrDefaultAsync(x => x.UserId == userId && x.EndedAt == null);
		if (impersonate is null) return false;

		impersonate.EndedAt = DateTime.UtcNow;
		context.Impersonates.Update(impersonate);

		await context.SaveChangesAsync();
		return true;
	}

	public async Task<User?> GetRegisteredUserByIdAsync(Guid? id)
	{
        using var context = await contextFactory.CreateDbContextAsync();
        var userId = (await context.Impersonates.FirstOrDefaultAsync(x => x.UserId == id && x.EndedAt == null))
			?.ImpersonateUserId;
		return await context.Users.Include(x => x.Role).Include(x => x.ReportingStructureNavigation)
			.FirstOrDefaultAsync(u => u.Id == userId);
	}
}