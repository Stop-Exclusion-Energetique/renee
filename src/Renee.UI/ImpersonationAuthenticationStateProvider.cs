using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;
using Renee.Application.Commands.Administration;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Queries.Administration;
using Renee.Application.Queries.UserCredential;
using Renee.Domain;
using Renee.Infrastructure.Providers;
using Constants = Renee.Domain.Constants;

namespace Renee.UI;

public class ImpersonationAuthenticationStateProvider(
	IHttpContextAccessor httpContextAccessor,
	IUserCredentialService userCredentialService,
	IImpersonateService impersonateService,
	GraphApiClientService graphApiClientService,
	IUserService userService) : AuthenticationStateProvider
{
	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var principal = httpContextAccessor.HttpContext?.User;
		if (principal?.GetObjectId() is null) return new AuthenticationState(new ClaimsPrincipal());

		var graphUser = await graphApiClientService.GetClient().Users[principal.GetObjectId()].Request().GetAsync();
		var registeredUserResult =
			await userCredentialService.GetRegisteredUser(new GetRegisteredUserOnCredentialsQuery(graphUser.Mail));
		var registeredUserDto = registeredUserResult.IsSuccess ? registeredUserResult.Value : null;
		var impersonateResult =
			await impersonateService.GetImpersonateForRegisteredUser(
				new GetImpersonateForConnectedUserQuery(registeredUserDto?.Id));
		var impersonatedUserDto = (impersonateResult.IsSuccess ? impersonateResult.Value : null) ??
			registeredUserDto;

		var user = await TransformAsync(principal, impersonatedUserDto, registeredUserDto);

		await UpdateLoginDateIfNeededAsync(registeredUserDto, graphUser.Mail);
		return new AuthenticationState(user);
	}

	public async Task ImpersonateUserAsync(Guid userId)
	{
		var principal = httpContextAccessor.HttpContext?.User;
		var basedEmail = principal?.FindFirst(c => c.Type == CustomClaimTypes.BasedEmail)?.Value;
		if (basedEmail != null)
		{
			var registeredUserResult =
				await userCredentialService.GetRegisteredUser(new GetRegisteredUserOnCredentialsQuery(basedEmail));
			var registeredUserDto = registeredUserResult.IsSuccess ? registeredUserResult.Value : null;

			if (registeredUserDto != null)
			{
				var impersonatedUserDto =
						await impersonateService.ImpersonateUser(new ImpersonateUserCommand(registeredUserDto.Id, userId));

					if (impersonatedUserDto.IsSuccess)
						await TransformAsync(principal!, impersonatedUserDto.Value, registeredUserDto);
			}
		}

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task StopImpersonation(Guid impersonatedUserId)
	{
		await impersonateService.StopImpersonation(new StopImpersonationCommand(impersonatedUserId));
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	private async Task UpdateLoginDateIfNeededAsync(
		RegisteredUserDto? registeredUserDto,
		string? email)
	{
		if (string.IsNullOrEmpty(email))
			return;

		var today = DateTime.UtcNow.Date;
		var lastLoginDate = registeredUserDto?.LastLoginDate?.Date;

		if (lastLoginDate == null || lastLoginDate < today)
			await userService.UpdateUserLastLoginDate(email);
	}

	private async Task<ClaimsPrincipal> TransformAsync(
		ClaimsPrincipal principal,
		RegisteredUserDto? impersonatedUserDto,
		RegisteredUserDto? registeredUserDto)
	{
		if (principal.HasClaim(claim => claim.Type == ClaimTypes.Role)) return principal;

		var clone = principal.Clone();
		var cloneIdentity = clone.Identity;

		var graphUser = await graphApiClientService.GetClient().Users[principal.GetObjectId()].Request().GetAsync();
		var email = graphUser.Mail;

		if (string.IsNullOrEmpty(email)) return await Task.FromResult(principal);

		if (impersonatedUserDto?.IsAccountDeleted == true) return new ClaimsPrincipal(new ClaimsIdentity());

		if (impersonatedUserDto?.RoleId is null ||
			string.IsNullOrWhiteSpace(impersonatedUserDto.UserName) ||
			string.IsNullOrWhiteSpace(impersonatedUserDto.RoleName) ||
			impersonatedUserDto.Id == Guid.Empty ||
			string.IsNullOrWhiteSpace(impersonatedUserDto.Email))
			return principal;

		if (cloneIdentity == null) return clone;
		var newIdentity = (ClaimsIdentity)cloneIdentity;
		newIdentity.TryRemoveClaim(newIdentity.FindFirst(ClaimTypes.Role));
		newIdentity.AddClaim(new Claim(ClaimTypes.Role, impersonatedUserDto.RoleName));

		newIdentity.TryRemoveClaim(newIdentity.FindFirst(ClaimTypes.Name));
		newIdentity.AddClaim(new Claim(ClaimTypes.Name, impersonatedUserDto.UserName));

		newIdentity.TryRemoveClaim(newIdentity.FindFirst(ClaimTypes.Email));
		newIdentity.AddClaim(new Claim(ClaimTypes.Email, impersonatedUserDto.Email));

		newIdentity.TryRemoveClaim(newIdentity.FindFirst(ClaimTypes.NameIdentifier));
		newIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, impersonatedUserDto.Id.ToString()));

		newIdentity.TryRemoveClaim(newIdentity.FindFirst(ClaimTypes.GroupSid));
		newIdentity.AddClaim(new Claim(ClaimTypes.GroupSid, impersonatedUserDto.GroupSid ?? string.Empty));

		newIdentity.TryRemoveClaim(newIdentity.FindFirst(ClaimTypes.Country));
		newIdentity.AddClaim(new Claim(ClaimTypes.Country, impersonatedUserDto.TerritoryId?.ToString() ?? string.Empty));

		if (registeredUserDto == null) return clone;

		newIdentity.TryRemoveClaim(new Claim(CustomClaimTypes.BasedRole, string.Empty));
		newIdentity.AddClaim(new Claim(CustomClaimTypes.BasedRole, registeredUserDto.RoleName!));

		newIdentity.TryRemoveClaim(new Claim(CustomClaimTypes.BasedName, string.Empty));
		newIdentity.AddClaim(new Claim(CustomClaimTypes.BasedName, registeredUserDto.UserName!));

		newIdentity.TryRemoveClaim(new Claim(CustomClaimTypes.BasedEmail, string.Empty));
		newIdentity.AddClaim(new Claim(CustomClaimTypes.BasedEmail, registeredUserDto.Email!));

		newIdentity.TryRemoveClaim(new Claim(CustomClaimTypes.BasedIdentity, string.Empty));
		newIdentity.AddClaim(new Claim(CustomClaimTypes.BasedIdentity, registeredUserDto.Id.ToString()));

		if (ShouldCheckAnahFiles(registeredUserDto))
			newIdentity.AddClaim(new Claim(CustomClaimTypes.ShouldCheckAnahFiles, "true"));
		else
			newIdentity.TryRemoveClaim(newIdentity.FindFirst(CustomClaimTypes.ShouldCheckAnahFiles));

		return clone;
	}

	private static bool ShouldCheckAnahFiles(RegisteredUserDto? registeredUserDto) =>
		registeredUserDto?.RoleName == Constants.SolidarBuilderRole && DateTime.UtcNow >= registeredUserDto?.NextDateForAnahGrantCheck;
}