using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;
using Renee.Infrastructure.Providers;
using Constants = Renee.Domain.Constants;
using User = Renee.Domain.Entity.User;

namespace Renee.Infrastructure.Repositories;

public sealed class UserRepository(
	ReneeDbContext dbContext,
	GraphApiClientService graphApiClientService,
	IConfiguration configuration,
	IDbContextFactory<ReneeDbContext> contextFactory) : IUserRepository
{
	public async Task<int> CountAllUser() =>
		await dbContext.Users.Where(u => !u.IsDeleted && !u.IsAccountDeletionRequested).CountAsync();

	public async Task<bool> DeleteUserAccount(Guid userId)
	{
		try
		{
			var user = await dbContext.Users.FindAsync(userId);

			if (user == null) return false;

			user.IsDeleted = true;
			user.IsAccountDeletionRequested = false;

			var updateDbResult = await dbContext.SaveChangesAsync();

			if (updateDbResult > 0)
			{
				var graphClient = graphApiClientService.GetClient();
				var adUsers = await graphClient.Users.Request().GetAsync();
				var connectedUserAd = adUsers.FirstOrDefault(u => u.Mail == user.Email);

				if (connectedUserAd == null) return false;

				var updatedUser = new Microsoft.Graph.User { AccountEnabled = false };

				await graphClient.Users[connectedUserAd.Id].Request().UpdateAsync(updatedUser);

				var userAdAfterUpdate = await graphClient.Users[connectedUserAd.Id].Request()
					.Select(u => u.AccountEnabled).GetAsync();

				return userAdAfterUpdate.AccountEnabled == false;
			}

			return false;
		}
		catch (Exception) { return false; }
	}

	public Task<List<User>> GetAllAdminUsers() =>
		dbContext.Users.Where(o => o.Role.Name == Constants.AdminRole).Include(o => o.Role).AsNoTracking()
			.ToListAsync();

	public async Task<List<User>> GetAllRegisteredUser() =>
		await dbContext.Users.Include(u => u.Role).Include(u => u.ReportingStructureNavigation).AsNoTracking()
			.Where(u => !u.IsDeleted).ToListAsync();

	public async Task<List<User>> GetAllUntrackedAccountDeletionRequests() =>
		await dbContext.Users.Include(u => u.Role).Include(u => u.ReportingStructureNavigation)
			.Where(u => u.IsAccountDeletionRequested && !u.IsDeleted).AsNoTracking().ToListAsync();

	public async Task<List<User>> GetAllUsers() =>
		await dbContext.Users.Include(x => x.ReportingStructureNavigation).Where(e => !e.IsDeleted).AsNoTracking()
			.ToListAsync();

	public async Task<User?> GetRegisteredUserByMailAsync(string registeredUserMail)
	{
		using var context = await contextFactory.CreateDbContextAsync();
        return await context.Users.Include(x => x.Role).Include(x => x.ReportingStructureNavigation)
            .FirstOrDefaultAsync(u => u.Email == registeredUserMail);

    }

	public async Task<Guid?> GetSolidarBuilderUserId(string firstName, string lastName) =>
		(await dbContext.Users.FirstAsync(u => u.FirstName == firstName && u.LastName == lastName && !u.IsDeleted)).Id;

	public async Task<User?> GetUserById(Guid id) =>
		await dbContext.Users.Include(i => i.ReportingStructureNavigation).Include(i => i.Role)
			.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

	public async Task<List<User>> GetUsersByRole(string role, bool shouldDisplayFakeUser)
	{
		if (shouldDisplayFakeUser)
		{
			return await dbContext.Users.Include(o => o.Role).Where(o => o.Role.Name == role && !o.IsDeleted)
				.AsNoTracking().ToListAsync();
		}
		else
		{
			return await dbContext.Users.Include(o => o.Role)
				.Where(o => o.Role.Name == role && !o.IsDeleted && (o.IsFakeUser == false || o.IsFakeUser == null))
				.AsNoTracking().ToListAsync();
		}
	}

	public async Task<List<User>> GetUsersForQuickAdd() =>
		await dbContext.Users.Include(x => x.Role).AsNoTracking().Where(u => !u.IsDeleted).ToListAsync();

	public async Task<List<User>> GetAllSolidarBuildersFromSameReportingStructure(Guid userId)
	{
		try
		{
			var reportingStructureId =
				(await dbContext.Users.AsNoTracking().FirstAsync(u => u.Id == userId && !u.IsDeleted))
				.ReportingStructureId;
			return await dbContext.Users
				.Include(u => u.Role)
				.Where(u => u.ReportingStructureId == reportingStructureId
					&& u.Role.Name == Constants.SolidarBuilderRole
					&& !u.IsDeleted)
				.AsNoTracking().ToListAsync();
		}
		catch { return []; }
	}

    public async Task<User?> GetUserByEmail(string? userEmail)
	{
		try
		{
			return await dbContext.Users.Include(u => u.ReportingStructureNavigation)
				.FirstOrDefaultAsync(u => userEmail == u.Email);
		}
		catch { return null; }
	}

    public async Task<int> UpdateUserAccountDeletionRequestState(Guid userId, bool isAccountDeletionRequested)
	{
		var user = await dbContext.Users.FindAsync(userId);

		if (user == null) return -1;

		user.IsAccountDeletionRequested = isAccountDeletionRequested;

		dbContext.Users.Update(user);

		return await dbContext.SaveChangesAsync();
	}

	public async Task<int> UpdateUserAsync(User entity)
	{
		try
		{
			dbContext.Entry(entity).State = EntityState.Detached;
			var dbEntity = await dbContext.Users.FindAsync(entity.Id);
			if (dbEntity != null)
			{
				dbEntity.FirstName = entity.FirstName;
				dbEntity.LastName = entity.LastName;
				dbEntity.Email = entity.Email;
				dbEntity.PhoneNumber = entity.PhoneNumber;
				dbEntity.TerritoryId = entity.TerritoryId;
				dbEntity.SiretNumber = entity.SiretNumber;
				dbContext.Users.Update(dbEntity);
			}

			var result = await dbContext.SaveChangesAsync();
			return result;
		}
		catch (Exception) { return -1; }
	}

	public async Task<int> UpdateUserCGUAsync(Guid userId, string cgU)
	{
        try
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user == null) return -1;
            user.LastValidatedCGUVersion = cgU;
			user.LastValidatedCGUDate = DateTime.Now;
			dbContext.Users.Update(user);
            return await dbContext.SaveChangesAsync();
        }
		catch (Exception) { return -1; }
    }

	public async Task<int> UpdateUserByAdmin(User user)
	{
		try
		{
			using (var context = await contextFactory.CreateDbContextAsync())
			{
				var dbEntity = await context.Users.FindAsync(user.Id);

				if (dbEntity != null)
				{
					dbEntity.FirstName = user.FirstName;
					dbEntity.LastName = user.LastName;
					dbEntity.RoleId = user.RoleId;
					dbEntity.ReportingStructureId = user.ReportingStructureId;
					context.Users.Update(dbEntity);
				}

				var result = await context.SaveChangesAsync();
				return result;
			}
		}
		catch (Exception) { return -1; }
	}

	public async Task<bool> UpdateUserInAzureAd(User entity, string oldEmail)
	{
		try
		{
			var tenantDomain = configuration.GetValue<string>("AzureADB2C:Domain");
			var graphClient = graphApiClientService.GetClient();
			var userAd = (await graphClient.Users.Request().GetAsync()).FirstOrDefault(e => e.Mail == oldEmail);

			if (userAd == null) return false;

			var userToUpdate = new Microsoft.Graph.User
			{
				DisplayName = $"{entity.FirstName} {entity.LastName}",
				Mail = entity.Email,
				OtherMails = [entity.Email!],
				UserPrincipalName = $"{entity.Email!.Replace('@', '_')}#EXT#@{tenantDomain}",
				MailNickname = entity.Email.Replace('@', '_'),
				Identities =
				[
					new()
					{
						SignInType = "emailAddress", Issuer = tenantDomain, IssuerAssignedId = entity.Email
					}
				],
				MobilePhone = entity.PhoneNumber
			};

			await graphClient.Users[userAd.Id].Request().UpdateAsync(userToUpdate);

			var userAdAfterUpdate = await graphClient.Users[userAd.Id].Request().GetAsync();

			return userAdAfterUpdate.DisplayName == $"{entity.FirstName} {entity.LastName}" &&
			       userAdAfterUpdate.Mail == entity.Email &&
			       userAdAfterUpdate.MobilePhone == entity.PhoneNumber;
		}
		catch (Exception) { return false; }
	}

	public async Task<int> UpdateLastLoginDate(string email)
	{
		try
		{
			var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (user == null) return -1;

			user.LastLoginDate = DateTime.UtcNow;
			dbContext.Users.Update(user);

			var result = await dbContext.SaveChangesAsync();
			return result;
		}
		catch (Exception) { return -1; }
	}

	public async Task<bool?> VerifyDuplicatedEmailAsync(string? mail)
	{
		try { return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == mail && !x.IsDeleted) is not null; }
		catch (Exception) { return null; }
	}

	public async Task<int> UpdateNextDateForAnahGrantCheck(Guid userId)
	{
		try
		{
			var user = await dbContext.Users.FindAsync(userId);

			if (user == null) return -1;

			user.NextDateForAnahGrantCheck = DateTime.Now.AddDays(21);
			dbContext.Users.Update(user);

			var result = await dbContext.SaveChangesAsync();
			return result;
		}
		catch (Exception) { return -1; }
	}
}