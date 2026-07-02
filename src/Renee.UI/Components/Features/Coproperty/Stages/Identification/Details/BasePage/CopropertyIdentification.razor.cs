using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.BasePage.ViewModel;
using Renee.UI.Components.Layout.StageLayout;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.BasePage;

public partial class CopropertyIdentification
{
    public int SelectedTab { get; set; }

    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public IJSRuntime? JsRuntime { get; set; }

    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;

    [Inject] public IAddressService AddressService { get; set; } = null!;
    [Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;

    public CopropertyIdentificationViewModel? CopropertyIdentificationViewModel { get; set; }

    [Parameter] public Guid? CopropertyProfileId { get; set; }

    private List<AddressDto>? AddressList { get; set; }

    private bool _saveButtonIsActive;

    public string UserRole { get; set; } = string.Empty;

    private Guid UserId { get; set; }

    public EditContext? EditContext { get; set; }

    private CopropertyProfileIdentificationDto? CopropertyIdentificationDto { get; set; }

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
        (
            UserRole == Constants.StructuralReferentRole || (UserRole == Constants.SolidarBuilderRole &&
            (CopropertyIdentificationDto?.Stage > AccompanyingFileStage.Identify || CopropertyIdentificationDto?.Status == AccompanyingFileStatus.WaitingForApproval) &&
            CopropertyIdentificationDto.IsInTzeeProgram)
        );

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

        var result = await CopropertyProfileService.GetCopropertyProfileForIdentificationMilestoneById(CopropertyProfileId!.Value, UserId, UserRole);

        if (!result.IsSuccess)
        {
            NavigateToCopropertyProfileList();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
            return;
        }
        CopropertyIdentificationDto = result.Value;

        CopropertyIdentificationViewModel = CopropertyIdentificationViewModel.CreateViewModelFromDto(CopropertyIdentificationDto!);

        EditContext = new EditContext(CopropertyIdentificationViewModel);
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
                    NavigateToCopropertyProfileList();
                NavigationManager.NavigateTo(Endpoints.CopropertyIdentificationSynthesis + "/" + CopropertyProfileId);
                
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

    private async Task ChangeTab(int index)
    {
        await SaveWithoutValidation();
        SelectedTab = index;
        StateHasChanged();
        if (JsRuntime != null)
        {
            await JsRuntime.InvokeVoidAsync("scrollToTop");
        }
    }

    private async Task LoadAddresses(LoadDataArgs? args)
    {
        if (!string.IsNullOrEmpty(args?.Filter))
        {
            var addressResult = await AddressService.SearchAddressAsync(args.Filter);
            if (addressResult.IsSuccess && addressResult.Value is not null)
                AddressList = [.. addressResult.Value];
            else
                AddressList = [];
        }
        else
            AddressList = [];
        StateHasChanged();
    }

    private async Task<bool> SaveData()
    {
        try
        {
            var formViewModel = EditContext!.Model as CopropertyIdentificationViewModel;

            var updatedAddress = new UpdatedCopropertyAddress(
                formViewModel!.GeneralInfoViewModel.Name,
                formViewModel.GeneralInfoViewModel.PostalCode,
                formViewModel.GeneralInfoViewModel.Municipality,
                formViewModel.GeneralInfoViewModel.Departement,
                formViewModel.GeneralInfoViewModel.Region,
                formViewModel.GeneralInfoViewModel.AdditionalAddress);

            var updatedCopropertyHousing = new UpdatedCopropertyHousing(
                (int?)formViewModel.GeneralInfoViewModel.Typology,
                (int?)formViewModel.GeneralInfoViewModel.HousingTypology,
                formViewModel.GeneralInfoViewModel.NumberOfLots,
                formViewModel.GeneralInfoViewModel.NumberOfFloor,
                (int?)formViewModel.GeneralInfoViewModel.HeatingType,
                (int?)formViewModel.GeneralInfoViewModel.PerilTypeLabel
                );

            var updatedGovernanceContats = new UpdatedGovernanceContats(
                (int?)formViewModel.GovernanceContactsViewModel.NatureOfSyndic,
                formViewModel.GovernanceContactsViewModel.NameOfSyndic,
                formViewModel.GovernanceContactsViewModel.PhoneOfSyndic,
                formViewModel.GovernanceContactsViewModel.MailOfSyndic,
                formViewModel.GovernanceContactsViewModel.NameOfAmo,
                formViewModel.GovernanceContactsViewModel.ContactOfAmo,
                formViewModel.GovernanceContactsViewModel.NumberOfContacts
                );

            var updatedDiagnostics = new UpdatedDiagnostics(
                (int?)formViewModel.DiagnosticsPerformanceViewModel.BuildingDpeLabel,
                formViewModel.DiagnosticsPerformanceViewModel.BuildingDpeEnergy,
                (int?)formViewModel.DiagnosticsPerformanceViewModel.ApartmentDpeLabel,
                formViewModel.DiagnosticsPerformanceViewModel.ApartmentDpeEnergy
                );

            var result = await CopropertyProfileService.UpdateCopropertyIdentificationMilestone(
                new SaveCopropertyProfileIdentificationMilestoneCommandInput(
                    CopropertyIdentificationDto!.Id, updatedAddress, updatedCopropertyHousing, updatedGovernanceContats, updatedDiagnostics, UserId));
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
        finally
        {
            SaveButtonIsActive = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}