using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;
using Renee.Infrastructure.Providers;
using Constants = Renee.Domain.Constants;
using StringHelper = Renee.Application.Helpers.StringHelper;
using User = Renee.Domain.Entity.User;

namespace Renee.Infrastructure.Repositories;

public sealed class UnregisteredUserRepository(
	ReneeDbContext dbContext,
	GraphApiClientService graphApiClientService,
	IConfiguration configuration) : IUnregisteredUserRepository
{
	public async Task<int> AddRegisteredUser(User user)
	{
		try
		{
			await dbContext.Users.AddAsync(user);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception) { return -1; }
	}

	public async Task<int> AddUser(UnregisteredUser user)
	{
		try
		{
			await dbContext.UnregisteredUsers.AddAsync(user);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception) { return -1; }
	}

	public async Task<string?> AddUserInAzureAd(UnregisteredUser user)
	{
		var password = StringHelper.GenerateRandomString(true);

		try
		{
			if (string.IsNullOrEmpty($"{user.FirstName}{user.LastName}") || string.IsNullOrEmpty(user.Email))
				return null;

			var tenantDomain = configuration.GetValue<string>("AzureADB2C:Domain");

			var userAd = new Microsoft.Graph.User
			{
				AccountEnabled = true,
				GivenName = user.LastName,
				Surname = user.FirstName,
				DisplayName = $"{user.FirstName} {user.LastName}",
				Mail = user.Email,
				OtherMails = new List<string> { user.Email },
				MailNickname = user.Email.Replace('@', '_'),
				UserPrincipalName = $"{user.Email.Replace('@', '_')}#EXT#@{tenantDomain}",
				CreationType = "LocalAccount",
				PasswordProfile = new PasswordProfile { ForceChangePasswordNextSignIn = false, Password = password },
				Identities =
				[
					new() { SignInType = "emailAddress", Issuer = tenantDomain, IssuerAssignedId = user.Email }
				],
				PasswordPolicies = "DisablePasswordExpiration"
			};

			var userAdded = await graphApiClientService.GetClient().Users.Request().AddAsync(userAd);

			var phoneAuthenticationMethod = new PhoneAuthenticationMethod
			{
				PhoneNumber = user.PhoneNumber,
				PhoneType = AuthenticationPhoneType.Mobile,
				SmsSignInState = AuthenticationMethodSignInState.Ready
			};

			await graphApiClientService.GetClient().Users[userAdded.Id].Authentication.PhoneMethods.Request()
				.AddAsync(phoneAuthenticationMethod);

			return password;
		}
		catch (Exception)
		{
			// Log the exception
			Console.WriteLine("Error when inserting");
		}

		return null;
	}

	public async Task<int> DeleteUser(Guid id)
	{
		try
		{
			var user = await dbContext.UnregisteredUsers.FindAsync(id);
			if (user != null) dbContext.UnregisteredUsers.Remove(user);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception) { return -1; }
	}

	public async Task<IEnumerable<UnregisteredUser>> GetAllSubscribedUsers() =>
		await dbContext.UnregisteredUsers.ToListAsync();

	public async Task<List<UnregisteredUser>> GetUntrackedReportingStructure()
	{
		var otherReportingStructureId = await dbContext.ReportingStructures.AsNoTracking()
			.Where(rs => rs.Name == Labels.Other).FirstAsync();

		var solidarBuilderRoleId = await dbContext.Roles.AsNoTracking()
			.Where(r => r.Name == Constants.SolidarBuilderRole).FirstAsync();

		return await dbContext.UnregisteredUsers.AsNoTracking().Where(u =>
			u.ReportingStructureId == otherReportingStructureId.Id &&
			u.AskedRole == solidarBuilderRoleId.Id &&
			u.ReportingStructure != null).ToListAsync();
	}

	public async Task<UnregisteredUser?> GetUserById(Guid id) =>
		await dbContext.UnregisteredUsers.Include(u => u.ReportingStructureNavigation)
			.FirstOrDefaultAsync(u => u.Id == id);

	public async Task<int> UpdateUserReportingStructure(Guid id, Guid reportingStructureId)
	{
		var user = await dbContext.UnregisteredUsers.FindAsync(id);

		if (user == null) return -1;

		if (reportingStructureId == Guid.Empty)
			user.ReportingStructure = null;
		else
			user.ReportingStructureId = reportingStructureId;

		try
		{
			dbContext.UnregisteredUsers.Update(user);
			return await dbContext.SaveChangesAsync();
		}
		catch (Exception) { return -1; }
	}

	public async Task<bool?> VerifyDuplicateEmailAsync(string? email)
	{
		try { return await dbContext.UnregisteredUsers.FirstOrDefaultAsync(x => x.Email == email) is not null; }
		catch (Exception) { return null; }
	}
}