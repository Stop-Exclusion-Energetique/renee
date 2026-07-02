using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Renee.UI.Components.Layout.StageLayout;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using Renee.UI.Components.Features.Coproperty.Stages.OrganizeAndFinance.BasePage.ViewModel;
using Renee.Domain.Enums;
using Renee.Domain;
using Renee.Application.DTOs.CopropertyProfile;
using Radzen;
using System.Security.Claims;
using Renee.Application.Interfaces;
using Renee.Application.CommandsUseCasesInput;

namespace Renee.UI.Components.Features.Coproperty.Stages.OrganizeAndFinance.BasePage;

public partial class CopropertyOrganizeAndFinance
{
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;
    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;
    [Inject] public IJSRuntime? JsRuntime { get; set; }

    public CopropertyOrganizeAndFinanceViewModel? CopropertyOrganizeAndFinanceViewModel { get; set; }
    public EditContext? EditContext { get; set; }

    private bool _saveButtonIsActive;

    public string UserRole { get; set; } = string.Empty;

    private Guid UserId { get; set; }

    private CopropertyProfileOrganizeAndFinanceDto? CopropertyProfileOrganizeAndFinanceDto { get; set; }

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
        UserRole == Constants.StructuralReferentRole || (UserRole == Constants.SolidarBuilderRole &&
        (CopropertyProfileOrganizeAndFinanceDto!.Stage > AccompanyingFileStage.OrganizingAndFinancing
        || CopropertyProfileOrganizeAndFinanceDto.Status == AccompanyingFileStatus.WaitingForApproval) &&
        CopropertyProfileOrganizeAndFinanceDto.IsInTzeeProgram);

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

        var result = await CopropertyProfileService.GetCopropertyProfileForOrganizeAndFinanceMilestoneById(CopropertyProfileId!.Value, UserId, UserRole);

        if (!result.IsSuccess)
        {
            NavigateToCopropertyProfileList();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
            return;
        }
        CopropertyProfileOrganizeAndFinanceDto = result.Value;

        CopropertyOrganizeAndFinanceViewModel = CopropertyOrganizeAndFinanceViewModel.CreateViewModelFromDto(CopropertyProfileOrganizeAndFinanceDto!);

        EditContext = new EditContext(CopropertyOrganizeAndFinanceViewModel);
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
                    NavigationManager.NavigateTo(Endpoints.CopropertyOrganizeAndFinanceSynthesis + "/" + CopropertyProfileId);
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

    private void WorkPackagesHaveChanged()
    {
        if (!SaveButtonIsActive)
        {
            SaveButtonIsActive = true;
            StateHasChanged();
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
            var updateAids = new UpdateAids(
                CopropertyOrganizeAndFinanceViewModel?.DateOfAgVote,
                CopropertyOrganizeAndFinanceViewModel?.MprCoproAids,
                CopropertyOrganizeAndFinanceViewModel?.ComplementaryAids
                );

            var updatedWorkPackage = CopropertyOrganizeAndFinanceViewModel?.WorkPackages.Select(
                    wp => new UpdateWorkPackage(
                        wp.Id ?? Guid.Empty,
                        wp.EnergeticsEffectOfWorks,
                        wp.WorkTypes.Select(wt => new UpdateWorkTypeCost(wt.Id, wt.Price, wt.Description)).ToList()))
                .ToList();

            var result = await CopropertyProfileService.UpdateCopropertyOrganizeAndFinanceMilestone(
                new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
                    CopropertyProfileOrganizeAndFinanceDto!.Id,
                    updateAids,
                    updatedWorkPackage ?? [],
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
