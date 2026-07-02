using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Identity.Web;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Synthesis.Modal.ViewModel;
using Renee.UI.Components.FormComponents;
using Constants = Renee.Domain.Constants;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.Modal;

public partial class OrganizeAndFinanceSynthesisValidationModal
{
	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;
	[Parameter] public OrganizeAndFinanceSynthesisValidationModalViewModel ViewModel { get; set; } = new();

	[Parameter] public string AnahGrantNotificationFileName { get; set; } = string.Empty;
	[Parameter] public MemoryStream? AnahGrantNotificationStream { get; set; }
	[Parameter] public string EnergyAuditFileName { get; set; } = string.Empty;
	[Parameter] public MemoryStream? EnergyAuditStream { get; set; }
	[Inject] private IFileService FileService { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		var result =
			await AccompanyingFileService.GetOrganizeAndFinanceValidationSynthesisModalData(
				ViewModel.AccompanyingFileId);

		if (result.IsSuccess && result.Value is not null)
		{
			var accompanyingFileValidationSynthesisModalDto = result.Value;
			ViewModel.IsInTzeeProgram = accompanyingFileValidationSynthesisModalDto.IsIncludedInTzeeProgram;
			ViewModel.AnahCategory = accompanyingFileValidationSynthesisModalDto.AnahCategory;
			ViewModel.OwnershipStatus = accompanyingFileValidationSynthesisModalDto.OwnershipStatus;
			ViewModel.DpeLabel = accompanyingFileValidationSynthesisModalDto.DpeLabel ?? string.Empty;
			ViewModel.GesLabel = accompanyingFileValidationSynthesisModalDto.GesLabel ?? string.Empty;
		}
	}

	private async Task Close() => await BlazoredModal.CloseAsync();

	private bool ShouldSubmitButtonBeDisabled() =>
	ViewModel is
	{
		IsAccompanyingFileValidatedForCeeProgram: true,
		IsUserAwareOfNoPossibilitiesToUpdateAccompanyingFile: false or null
	};

	private async Task Submit()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

		var userIdString = authState.User.GetNameIdentifierId();

		if (!Guid.TryParse(userIdString, out var userId) || AnahGrantNotificationStream == null || EnergyAuditStream == null) return;

		await FileService.UploadFileAsync(AnahGrantNotificationFileName, AnahGrantNotificationFileName, AnahGrantNotificationStream!, userId);
		await FileService.UploadFileAsync(EnergyAuditFileName, EnergyAuditFileName, EnergyAuditStream!, userId);

		var status = IsUserAbleToValidateSynthesisWithoutValidation() || (bool)!ViewModel.IsInTzeeProgram! ? AccompanyingFileStatus.InProgress : AccompanyingFileStatus.WaitingForApproval;
		var stage = IsUserAbleToValidateSynthesisWithoutValidation() || (bool)!ViewModel.IsInTzeeProgram! ? AccompanyingFileStage.RealisationAndFollowing : AccompanyingFileStage.OrganizingAndFinancing;

		if (!ViewModel.IsAccompanyingFileValidatedForCeeProgram)
		{
			status = AccompanyingFileStatus.Rejected;
			stage = AccompanyingFileStage.OrganizingAndFinancing;
		}

		var result = await AccompanyingFileService.UpdateAccompanyingFileOrganizeAndFinanceSynthesis(
			ViewModel.AccompanyingFileId,
			status,
			stage,
			userId,
			ShouldNotifyUsers());

		if (result.IsSuccess)
		{
			ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = result.Message, Duration = 4000 });

			NavigationManager.NavigateTo(
				IsUserAbleToValidateSynthesisWithoutValidation() || ((bool)!ViewModel.IsInTzeeProgram! && status == AccompanyingFileStatus.InProgress) ?
				$"{Endpoints.RealiseAndFollowStage}/{ViewModel.AccompanyingFileId}"
				: Endpoints.UserCreatedAccompanyingFiles,
				false,
				true);
		}
		else
		{
			ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
		}

		bool IsUserAbleToValidateSynthesisWithoutValidation()
				=> ViewModel.UserRole == Constants.DiffuseCoordinatorRole ||
					ViewModel.UserRole == Constants.TargetedCoordinatorRole ||
					ViewModel.UserRole == Constants.TerritorialBuilderRole ||
					ViewModel.UserRole == Constants.AdminRole;

		bool ShouldNotifyUsers() => ViewModel is { UserRole: Constants.SolidarBuilderRole, IsInTzeeProgram: true };
	}

	private void ShowNotification(NotificationMessage message)
	{
		NotificationService.Notify(message);
	}
}