using System.Security.Claims;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Queries.Administration;
using Renee.Domain;
using Renee.UI.Components.Features.ProfileManagement.Modal;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.ProfileManagement;

public partial class ProfileManagementForm
{
	[Inject] public IModalService ModalService { get; set; } = null!;
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public IUserService UserService { get; set; } = null!;
	[Inject] public IImpersonateService ImpersonateService { get; set; } = null!;
	public Guid ImpersonateUserId { get; set; }

	private Guid _userId;
	private Guid _userBaseId;
	private RegisteredUserDto? _impersonateUser;
	private string? _userRole;
	private string? _userBaseRole;

	private List<ZeeSelectItem<Guid>>? _users;

	protected override async Task OnInitializedAsync()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var claims = authState.User;
		var userIdClaims = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
		_userBaseRole = claims.FindFirst(c => c.Type == CustomClaimTypes.BasedRole)?.Value;
		var baseIdentity = claims.FindFirst(c => c.Type == CustomClaimTypes.BasedIdentity)?.Value;
		if (baseIdentity != null) _userBaseId = Guid.Parse(baseIdentity);

		if (Guid.TryParse(userIdClaims, out var userId) && userRole != null)
		{
			_userId = userId;
			_userRole = userRole;
		}

		if (_userBaseRole == Constants.AdminRole)
		{
			var impersonateResult =
				await ImpersonateService.GetImpersonateForRegisteredUser(
					new GetImpersonateForConnectedUserQuery(_userBaseId));
			_impersonateUser = impersonateResult.IsSuccess ? impersonateResult.Value : null;

			var usersResult = await UserService.GetAllUsers();
			var users = usersResult.Value;
			if (users != null)
				_users = users.Select(x => new ZeeSelectItem<Guid>(x?.FirstName + " " + x?.LastName, x!.Id)).ToList();
		}
	}

	private async Task ImpersonateUser()
	{
		if (_userBaseRole == Constants.AdminRole && ImpersonateUserId != Guid.Empty)
		{
			await (AuthenticationStateProvider as ImpersonationAuthenticationStateProvider)!.ImpersonateUserAsync(
				ImpersonateUserId);
			NavigationManager.NavigateTo("/", true);
		}
	}

	private async Task OnShowDeleteAccountModal()
	{
		var modal = ModalService.Show<DeleteAccountModal>(
			new ModalParameters { { nameof(DeleteAccountModal.UserId), _userId } },
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });

		var result = await modal.Result;
		if (result.Confirmed) NavigationManager.NavigateTo("MicrosoftIdentity/Account/SignOut", true);
	}


	private void OnShowPersonalAccount()
	{
		ModalService.Show<UpdateProfileManagementModal>(
			new ModalParameters { { nameof(UpdateProfileManagementModal.UserId), _userId } },
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });
	}

	private async Task StopImpersonateUser()
	{
		if (_userBaseRole == Constants.AdminRole && _userBaseId != Guid.Empty)
		{
			await (AuthenticationStateProvider as ImpersonationAuthenticationStateProvider)!.StopImpersonation(
				_userBaseId);
			NavigationManager.NavigateTo("/", true);
		}
	}
}