using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Administration;
using Renee.Domain;
using Renee.UI.Components.CGUHandling;
using Renee.UI.Components.ErrorHandling;
using System.Security.Claims;

namespace Renee.UI.Components.Layout;

public partial class MainLayout
{
	bool _sidebar1Expanded = true;
	bool _showBanner = true;
	string? _bannerMessage ;
	[Inject] public CguInitializer CGUInitializer { get; set; } = null!;
	[Inject] public IFileViewerService FileViewer { get; set; } = null!;
	[Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] public ISendEventQuery Sender { get; set; } = null!;

	public CguContext CGUContext => CGUInitializer.Context;
	private CustomErrorBoundary? errorBoundary;

	protected override void OnParametersSet()
	{
		errorBoundary?.Recover();
	}

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;
		var userIdClaims = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (!Guid.TryParse(userIdClaims, out var userId))
			return;

		await CGUInitializer.InitializeAsync(userId);
		await CheckAnahGrant(claims, userId);
		_showBanner = await ShouldDisplayBannerAsync();
	}

	private void Logout() => NavigationManager.NavigateTo("MicrosoftIdentity/Account/SignOut", true);
	private void ShowProfileManagementModal() => NavigationManager.NavigateTo(Endpoints.ProfileManagementPage);

	private async Task HandleCGUAcceptedAsync()
	{
		await CGUInitializer.MarkCGUAsAcceptedAsync();
		StateHasChanged();
	}

	private async Task CheckAnahGrant(IEnumerable<Claim> claims, Guid userId)
	{
		if (!claims.Any(c => c.Type == CustomClaimTypes.ShouldCheckAnahFiles))
			return;

		var hasPendingFiles = (await AccompanyingFileService.HasAccompanyingFilesAwaitingAnahResponse(userId)).Value;

		if (!hasPendingFiles)
			return;

		NavigationManager.NavigateTo(Endpoints.AnahGrantCheck, false);
	}

	private async Task<bool> ShouldDisplayBannerAsync()
	{
		var result = await Sender.Send(new GetBannerDisplayDatesDisplayQuery());

		if (!result.IsSuccess || result.Value == null)
			return false;

		var bannerStartDate = result.Value.AccompanyingFileAlertBannerStartDate;
		var bannerEndDate = result.Value.AccompanyingFileAlertBannerEndDate;

		if (bannerStartDate == null || bannerEndDate == null)
			return false;

		_bannerMessage = result.Value.AccompanyingFileAlertBannerMessage;
		var currentDate = DateTime.UtcNow;
		return currentDate >= bannerStartDate && currentDate <= bannerEndDate;
	} 
}