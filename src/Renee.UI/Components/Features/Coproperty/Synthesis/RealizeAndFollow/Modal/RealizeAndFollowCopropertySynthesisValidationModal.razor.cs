using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Renee.Application.Interfaces;
using Microsoft.Identity.Web;
using Radzen;
using Renee.Domain.Enums;
using Renee.Domain;
using System.Security.Claims;
using Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.Modal.ViewModel;
using Constants = Renee.Domain.Constants;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.Modal;

public partial class RealizeAndFollowCopropertySynthesisValidationModal
{
    [CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;
    [Parameter] public string FileName { get; set; } = string.Empty;
    [Parameter] public MemoryStream? MemoryStream { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = null!;
    [Inject] private ICopropertyProfileService CopropertyProfileService { get; set; } = null!;

    [Parameter] public RealizeAndFollowCopropertySynthesisValidationModalViewModel ViewModel { get; set; } = new();
    private string UserRole { get; set; } = string.Empty;
    private Guid _userId;

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

    private async Task SubmitValidation()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var userIdString = authState.User.GetNameIdentifierId();

        if (!Guid.TryParse(userIdString, out var userId) || MemoryStream == null) return;

        await FileService.UploadFileAsync(FileName, FileName, MemoryStream, userId);

        var status = IsUserAbleToValidateSynthesisWithoutValidation() ? AccompanyingFileStatus.Finished : AccompanyingFileStatus.WaitingForApproval;
        var stage = IsUserAbleToValidateSynthesisWithoutValidation() ? AccompanyingFileStage.Finished : AccompanyingFileStage.RealisationAndFollowing;

        var result = await CopropertyProfileService.UpdateCopropertyProfileSynthesis(
            ViewModel.CopropertyProfileId,
            status,
            stage,
            userId);

        if (result.IsSuccess)
        {
            NavigationManager.NavigateTo(
                IsUserAbleToValidateSynthesisWithoutValidation() || status == AccompanyingFileStatus.InProgress ?
                $"{Endpoints.CopropertyRealizeAndFollow}/{ViewModel.CopropertyProfileId}"
                : Endpoints.Coproperty,
                true,
                true);

            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = result.Message, Duration = 4000 });
        }
        else
        {
            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
        }

    }

    private async Task Close() => await BlazoredModal.CloseAsync();

    bool IsUserAbleToValidateSynthesisWithoutValidation()
                => ViewModel.UserRole == Constants.DiffuseCoordinatorRole ||
                    ViewModel.UserRole == Constants.TargetedCoordinatorRole ||
                    ViewModel.UserRole == Constants.TerritorialBuilderRole ||
                    ViewModel.UserRole == Constants.AdminRole;

    private void ShowNotification(NotificationMessage message)
    {
        NotificationService.Notify(message);
    }
}
