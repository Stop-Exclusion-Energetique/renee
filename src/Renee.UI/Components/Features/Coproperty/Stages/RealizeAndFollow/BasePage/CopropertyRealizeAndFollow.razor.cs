using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.Interfaces;
using Renee.Domain.Enums;
using Renee.Domain;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;
using Renee.UI.Components.Features.Coproperty.Stages.RealizeAndFollow.BasePage.ViewModel;
using Renee.Application.CommandsUseCasesInput;

namespace Renee.UI.Components.Features.Coproperty.Stages.RealizeAndFollow.BasePage;

public partial class CopropertyRealizeAndFollow
{
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;
    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;
    [Inject] public IJSRuntime? JsRuntime { get; set; }

    public CopropertyRealizeAndFollowViewModel? CopropertyRealizeAndFollowViewModel { get; set; }
    public EditContext? EditContext { get; set; }

    private bool _saveButtonIsActive;

    public string UserRole { get; set; } = string.Empty;

    private Guid UserId { get; set; }

    private CopropertyProfileRealizeAndFollowDto? CopropertyProfileRealizeAndFollowDto { get; set; }

    [Parameter] public Guid? CopropertyProfileId { get; set; }

    public bool SaveButtonIsActive
    {
        get => _saveButtonIsActive;
        set
        {
            _saveButtonIsActive = value;
            InvokeAsync(StateHasChanged);
        }
    }
    private bool IsUserNotAllowedToEdit =>
        UserRole == Constants.StructuralReferentRole ||
        (UserRole == Constants.SolidarBuilderRole &&
        CopropertyProfileRealizeAndFollowDto?.Status == AccompanyingFileStatus.WaitingForApproval &&
        CopropertyProfileRealizeAndFollowDto.IsInTzeeProgram) || CopropertyProfileRealizeAndFollowDto?.Status == AccompanyingFileStatus.Finished ||
        CopropertyProfileRealizeAndFollowDto?.Status == AccompanyingFileStatus.Aborted;

    protected override async Task OnInitializedAsync()
    {
        var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

        var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        UserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        if (value != null)
            UserId = Guid.Parse(value);
        else
            throw new InvalidDataException("User Id not found in claims");

        if (CopropertyProfileId is null)
        {
            NavigateToCopropertyProfileList();
            return;
        }

        var result = await CopropertyProfileService.GetCopropertyProfileForRealizeAndFollowMilestoneById(CopropertyProfileId!.Value, UserId, UserRole);

        if (!result.IsSuccess)
        {
            NavigateToCopropertyProfileList();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
            return;
        }
        CopropertyProfileRealizeAndFollowDto = result.Value;

        CopropertyRealizeAndFollowViewModel = CopropertyRealizeAndFollowViewModel.CreateViewModelFromDto(CopropertyProfileRealizeAndFollowDto!);

        EditContext = new EditContext(CopropertyRealizeAndFollowViewModel);
        EditContext.OnFieldChanged += OnFormFieldHasBeenChanged;

    }

    public async Task SubmitWithValidation()
    {
        try
        {
            var isValid = ValidateEditContext();

            if (isValid)
            {
                var saved = await SaveData();

                if (saved)
                    NavigationManager.NavigateTo(Endpoints.CopropertyRealizeAndFollowSynthesis + "/" + CopropertyProfileId);
            }
        }

        catch (Exception)
        {
            // ignored
        }
        finally
        {
            SaveButtonIsActive = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task SaveWithoutValidation()
    {
        try
        {
            await SaveData();
        }
        catch (Exception)
        {
            // ignored
        }
        finally { SaveButtonIsActive = false; }
    }

    private void OnFormFieldHasBeenChanged(object? sender, EventArgs e)
    {
        if (!SaveButtonIsActive) SaveButtonIsActive = true;
    }

    private void NavigateToCopropertyProfileList()
    {
        NavigationManager.NavigateTo(Endpoints.Coproperty);
    }

    public bool ValidateEditContext()
    {
        return EditContext!.Validate();
    }


    private async Task<bool> SaveData()
    {
        try
        {
            var updateWorkTracking = new UpdateCopropertyWorkTracking(
                CopropertyRealizeAndFollowViewModel?.CollectiveWorksStartDate,
                CopropertyRealizeAndFollowViewModel?.PlannedEndDate,
                CopropertyRealizeAndFollowViewModel?.ProgressPercentage,
                CopropertyRealizeAndFollowViewModel?.ActualCompletionDate,
                CopropertyRealizeAndFollowViewModel?.InvoiceTotalAmount,
                CopropertyRealizeAndFollowViewModel?.FollowUpComment
                );

            var result = await CopropertyProfileService.UpdateCopropertRealizeAndFollowMilestone(
                new SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
                    CopropertyProfileRealizeAndFollowDto!.Id,
                    updateWorkTracking,
                    UserId));

            if (result.IsSuccess && result.Value) StageNavigationStateService.NotifyStateChanged();

            NotificationService.Notify(new NotificationMessage
            {
                Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
                Summary = result.Message,
                Duration = 4000
            });

            return result.IsSuccess && result.Value;
        }
        catch (Exception) { return false; }
    }
}
