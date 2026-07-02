using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class CguVersionRepository(IDbContextFactory<ReneeDbContext> contextFactory) : ICguVersionRepository
{
	public async Task<int> AddCGUVersion(CguVersion cguVersion)
	{
		try
		{
			using var context = await contextFactory.CreateDbContextAsync();
			await context.CGUVersions.AddAsync(cguVersion);
			return await context.SaveChangesAsync();
		}
		catch
		{
			return -1;
		}
	}

	public async Task<List<CguVersion>> GetAllVersionsAsync()
	{
		using var context = await contextFactory.CreateDbContextAsync();
		return await context.CGUVersions.AsNoTracking().ToListAsync();
	}

	public async Task<CguVersion?> GetByVersionAsync(string version)
	{
		using var context = await contextFactory.CreateDbContextAsync();
		return await context.CGUVersions.FirstOrDefaultAsync(x => x.Version == version.Trim());
	}

	public async Task<CguVersion?> GetLatestVersionAsync()
	{
		using var context = await contextFactory.CreateDbContextAsync();
		return await context.CGUVersions
			.OrderByDescending(c => c.CreatedAt)
			.FirstOrDefaultAsync();
	}

	public async Task<int> UpdateCguAsync(CguVersion cguVersion)
	{
		try
		{
			using var context = await contextFactory.CreateDbContextAsync();
			var cgu = await context.CGUVersions.FindAsync(cguVersion.Id);
			if (cgu != null)
			{
				cgu.Version = cguVersion.Version;
				cgu.Label = cguVersion.Label;
				cgu.ModifiedAt = DateTime.UtcNow;
				context.CGUVersions.Update(cgu);
			}
			return await context.SaveChangesAsync();
		}
		catch
		{
			return -1;
		}
	}

	public async Task<bool> DeleteCguAsync(Guid id)
	{
		using var context = await contextFactory.CreateDbContextAsync();
		var cgu = await context.CGUVersions.FindAsync(id);
		if (cgu == null)
			return false;

		context.CGUVersions.Remove(cgu);
		await context.SaveChangesAsync();

		return true;
	}

	public async Task<bool?> VerifiyDuplicatedVersionAsync(string version)
	{
		if (string.IsNullOrWhiteSpace(version))
			return null;

		using var context = await contextFactory.CreateDbContextAsync();
		bool exists = await context.CGUVersions
			.AnyAsync(c => c.Version == version.Trim());

		return exists;
	}
}