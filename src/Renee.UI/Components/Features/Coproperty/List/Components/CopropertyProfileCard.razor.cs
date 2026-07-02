using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.Coproperty.List.Modal;
using Renee.UI.Components.Features.Coproperty.List.ViewModel;
using Renee.UI.Components.Features.Coproperty.SynthesisValidation;

namespace Renee.UI.Components.Features.Coproperty.List.Components;

public partial class CopropertyProfileCard
{
    [Parameter] public CreatedCopropertyProfileByUserViewModel ViewModel { get; set; } = null!;
    [Parameter] public Guid UserId { get; set; }
    [Parameter] public string? UserRole { get; set; }
    [Parameter] public EventCallback OnCopropertyProfileDeleted { get; set; }
    [Parameter] public EventCallback OnStateChanged { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IModalService ModalService { get; set; } = null!;

    private bool IsUserSolidarBuilder => UserRole!.Equals(Constants.SolidarBuilderRole);

    public async Task StageValidation(bool isValidationPopUp)
    {
        var modal = ModalService.Show<CopropertyStageValidationModal>(
            new ModalParameters
            {
                { nameof(CopropertyStageValidationModal.CopropertyProfileId), ViewModel.Id },
                { nameof(CopropertyStageValidationModal.IsValidationModal), isValidationPopUp },
                { nameof(CopropertyStageValidationModal.UserId), UserId }
            },
            new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true });

        await modal.Result;

        if (OnStateChanged.HasDelegate) await OnStateChanged.InvokeAsync();
    }

    private bool IsUserAllowedToModifyDataFolder() =>
        (
            UserId == ViewModel.SolidarBuilder ||
            UserId == ViewModel.SecondSolidarBuilder ||
            UserId == ViewModel.DiffuseCoordinator ||
            UserId == ViewModel.TargetedCoordinator ||
            UserId == ViewModel.TerritorialBuilder ||
            UserId == ViewModel.SecondTerritorialBuilder ||
            UserId == ViewModel.ThirdSolidarBuilder
        ) || UserRole == Constants.AdminRole || UserRole == Constants.StructuralReferentRole;

    private bool IsCopropertyProfileWaitingForValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForApproval;

    private void OnEdit()
    {
        NavigationManager.NavigateTo(ViewModel.EditUrl);
    }

    private async Task OnDeleteCopropertyProfile()
    {
        var modal = ModalService.Show<DeleteCopropertyProfileModal>(
            new ModalParameters { { nameof(DeleteCopropertyProfileModal.CopropertyProfileId), ViewModel.Id } },
            new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });

        var result = await modal.Result;
        if (result.Confirmed) await OnCopropertyProfileDeleted.InvokeAsync();
    }

    private bool ShownSynthesisButton()
    {
        switch (ViewModel.Stage)
        {
            case > 0:
            case AccompanyingFileStage.Identify when ViewModel.Status != AccompanyingFileStatus.InProgress && ViewModel.Status != AccompanyingFileStatus.Rejected:
                return true;
            default: return false;
        }
    }

    private void AnalyzeToValidate()
    {
        NavigationManager.NavigateTo(ViewModel.SynthesisUrl);
    }
}
