using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Identity.Web;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal.ViewModel;
using Constants = Renee.Domain.Constants;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal;

public partial class IdentifySynthesisValidationModal
{
	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;

	[Parameter] public IdentifySynthesisValidationModalViewModel ViewModel { get; set; } = new();
	[Parameter] public string FileName { get; set; } = string.Empty;
	[Parameter] public MemoryStream? MemoryStream { get; set; }
	[Inject] private NavigationManager NavigationManager { get; set; } = default!;
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] private IFileService FileService { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		var accompanyingFileValidationSynthesisModalResult =
			await AccompanyingFileService.GetIdentifyValidationSynthesisModalData(ViewModel.AccompanyingFileId);

		if (accompanyingFileValidationSynthesisModalResult.IsSuccess && accompanyingFileValidationSynthesisModalResult.Value != null)
			ViewModel.IsInTzeeProgram = accompanyingFileValidationSynthesisModalResult.Value.IsIncludedInTzeeProgram;
	}

	private async Task Close() => await BlazoredModal.CloseAsync();

	private bool ShouldSubmitButtonBeDisabled() =>
		ViewModel is {
			IsAccompanyingFileValidatedForCeeProgram: true,
			IsUserAwareOfNoPossibilitiesToUpdateAccompanyingFile: false or null
		};

	private async Task Submit()
	{
		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

		var userIdString = authState.User.GetNameIdentifierId();

		if (!Guid.TryParse(userIdString, out var userId) || MemoryStream == null) return;

		await FileService.UploadFileAsync(FileName, FileName, MemoryStream, userId);

		var status = IsUserAbleToValidateSynthesisWithoutValidation() || (bool)!ViewModel.IsInTzeeProgram! ? AccompanyingFileStatus.InProgress : AccompanyingFileStatus.WaitingForApproval;
		var stage = IsUserAbleToValidateSynthesisWithoutValidation() || (bool)!ViewModel.IsInTzeeProgram! ? AccompanyingFileStage.OrganizingAndFinancing : AccompanyingFileStage.Identify;

		if (!ViewModel.IsAccompanyingFileValidatedForCeeProgram)
		{
			status = AccompanyingFileStatus.Rejected;
			stage = AccompanyingFileStage.Identify;
		}

		var result = await AccompanyingFileService.UpdateAccompanyingFileIdentificationSynthesis(
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
				$"{Endpoints.OrganizeAndFinanceStage}/{ViewModel.AccompanyingFileId}"
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