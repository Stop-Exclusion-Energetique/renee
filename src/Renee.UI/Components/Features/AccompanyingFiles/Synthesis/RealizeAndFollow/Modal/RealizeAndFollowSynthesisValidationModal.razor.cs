using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Synthesis.Modal.ViewModel;
using System.Security.Claims;
using Constants = Renee.Domain.Constants;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.Modal;

public partial class RealizeAndFollowSynthesisValidationModal
{
	[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	[Inject] public IFileService FileService { get; set; } = null!;
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;
	[Parameter] public string WorkReceiptPvFileName { get; set; } = string.Empty;
	[Parameter] public MemoryStream? WorkReceiptMemoryStream { get; set; }
	[Parameter] public string AccompanyingFileReportFileName { get; set; } = string.Empty;
	[Parameter] public MemoryStream? AccompanyingFileReportMemoryStream { get; set; }
	[Parameter] public string AnahHelpObtentionFileName { get; set; } = string.Empty;
	[Parameter] public MemoryStream? AnahHelpObtentionMemoryStream { get; set; }
	[Parameter] public Guid AccompanyingFileId { get; set; }
	[Parameter] public bool IsInTzeeProgram { get; set; }
	private string UserRole { get; set; } = string.Empty;
	private Guid _userId;

	public RealizeAndFollowSynthesisValidationModalViewModel ViewModel { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var claims = authState.User;
		var userIdClaims = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

		if (Guid.TryParse(userIdClaims, out var userId) && userRole != null)
		{
			_userId = userId;
			UserRole = userRole;
		}
	}

	private async Task Close() => await BlazoredModal.CloseAsync();

	private async Task Submit()
	{
		if (WorkReceiptMemoryStream == null || AccompanyingFileReportMemoryStream == null) return;

		await UploadSingleFileAsync(WorkReceiptMemoryStream, WorkReceiptPvFileName);
		await UploadSingleFileAsync(AccompanyingFileReportMemoryStream, AccompanyingFileReportFileName);
		await UploadSingleFileAsync(AnahHelpObtentionMemoryStream, AnahHelpObtentionFileName);

		var result = await AccompanyingFileService.UpdateAccompanyingFileRealizeAndFollowSynthesis(
			AccompanyingFileId,
			IsUserAbleToValidateSynthesisWithoutValidation(),
			_userId,
			ShouldNotifyUsers());

		if (result.IsSuccess)
		{
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = result.Message, Duration = 4000 });
			NavigationManager.NavigateTo(Endpoints.UserCreatedAccompanyingFiles, true, true);
		}
		else
		{
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
		}

		bool IsUserAbleToValidateSynthesisWithoutValidation()
				=> UserRole == Constants.DiffuseCoordinatorRole ||
					UserRole == Constants.TargetedCoordinatorRole ||
					UserRole == Constants.TerritorialBuilderRole ||
					UserRole == Constants.AdminRole ||
					!IsInTzeeProgram;

		bool ShouldNotifyUsers() => UserRole == Constants.SolidarBuilderRole && IsInTzeeProgram;
	}

	private async Task UploadSingleFileAsync(MemoryStream? stream, string fileName)
	{
		await FileService.UploadFileAsync(fileName, fileName, stream!, _userId);
	}
}
