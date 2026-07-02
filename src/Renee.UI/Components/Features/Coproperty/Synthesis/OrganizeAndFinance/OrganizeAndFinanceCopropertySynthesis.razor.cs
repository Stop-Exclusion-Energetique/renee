using System.Security.Claims;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;
using Renee.UI.Components.Features.Coproperty.Synthesis.OrganizeAndFinance.Modal;
using Renee.UI.Components.Features.Coproperty.Synthesis.OrganizeAndFinance.Modal.ViewModel;
using Renee.UI.Components.Features.Coproperty.Synthesis.OrganizeAndFinance.ViewModel;
using Renee.UI.Components.Features.Coproperty.SynthesisValidation;
using Renee.UI.Components.Layout.SynthesisLayoutManager;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.OrganizeAndFinance;

public partial class OrganizeAndFinanceCopropertySynthesis : ComponentBase
{
    [Parameter] public Guid CopropertyProfileId { get; set; }
    public OrganizeAndFinanceCopropertySynthesisViewModel ViewModel { get; set; } = new();

    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;
    [Inject] public SynthesysLayoutStateManager SynthesysLayoutStateManager { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public NavigationHistoryManager NavigationHistoryManager { get; set; } = null!;
    [Inject] private IModalService ModalService { get; set; } = null!;
    [Inject] private IFileService FileService { get; set; } = null!;
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    private CopropertyProfileOrganizeAndFinanceDto? CopropertyOrganizeAndFinanceDto { get; set; }

    private string _anahAidRequestNotificationErrorMessage = string.Empty;
    private MemoryStream? _anahAidRequestNotificationStreamFile;
    private string _anahAidRequestNotificationFileName = string.Empty;
    private byte[]? _anahAidRequestNotificationFileData;
    private string _anahAidRequestNotificationBlobName = string.Empty;
    private string _anahAidRequestNotificationFileExtension = string.Empty;

    private string _generalMeetingErrorMessage = string.Empty;
    private MemoryStream? _generalMeetingStreamFile;
    private string _generalMeetingFileName = string.Empty;
    private byte[]? _generalMeetingFileData;
    private string _generalMeetingBlobName = string.Empty;
    private string _generalMeetingFileExtension = string.Empty;

    public string UserRole { get; set; } = string.Empty;
    private Guid UserId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

        var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        UserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        if (value != null)
            UserId = Guid.Parse(value);
        else
            throw new InvalidDataException("User Id not found in claims");

        var result = await CopropertyProfileService.GetCopropertyProfileForOrganizeAndFinanceMilestoneById(CopropertyProfileId, UserId, UserRole);

        if (!result.IsSuccess)
        {
            NavigateToCopropertyProfileList();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
            return;
        }
        CopropertyOrganizeAndFinanceDto = result.Value;

        if (CopropertyOrganizeAndFinanceDto != null)
        {

            ViewModel = OrganizeAndFinanceCopropertySynthesisViewModel.CreateViewModelFromDto(CopropertyOrganizeAndFinanceDto);

            SynthesysLayoutStateManager.AssociatedResourceReference = ViewModel.Reference;
            SynthesysLayoutStateManager.AssociatedResourceId = CopropertyProfileId;
            SynthesysLayoutStateManager.AccompanyingFileStage = AccompanyingFileStage.OrganizingAndFinancing;
            SynthesysLayoutStateManager.ShouldDisplayNavigationButton = !NavigationHistoryManager.PreviousUri!.Contains(Endpoints.CopropertyOrganizeAndFinance);
            SynthesysLayoutStateManager.IsAccompanyingFile = false;

            SynthesysLayoutStateManager.NotifyStateChanged();

            _anahAidRequestNotificationBlobName = string.Format(
                "{0}_{1}",
                ViewModel.Reference,
                Labels.AnahAidRequestNotificationDocument);

            _generalMeetingBlobName = string.Format(
                "{0}_{1}",
                ViewModel.Reference,
                Labels.GeneralMeetingMinutesDocument);

            try
            {
                (_anahAidRequestNotificationStreamFile, _anahAidRequestNotificationFileName) = await FileService.DownloadFileForSynthesisAsync(_anahAidRequestNotificationBlobName);
                if (_anahAidRequestNotificationStreamFile != null)
                {
                    _anahAidRequestNotificationFileData = _anahAidRequestNotificationStreamFile.ToArray();
                    _anahAidRequestNotificationFileExtension = GetFileExtension(_anahAidRequestNotificationFileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }

            try
            {
                (_generalMeetingStreamFile, _generalMeetingFileName) = await FileService.DownloadFileForSynthesisAsync(_generalMeetingBlobName);
                if (_generalMeetingStreamFile != null)
                {
                    _generalMeetingFileData = _generalMeetingStreamFile.ToArray();
                    _generalMeetingFileExtension = GetFileExtension(_anahAidRequestNotificationFileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }

        }
    }

    private async Task UploadanahAidRequestNotificationFile(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file.Size > Constants.MaxFileSize)
        {
            _anahAidRequestNotificationErrorMessage = Labels.Errors.MaximumFileSizeExceeded;
            return;
        }

        _anahAidRequestNotificationFileExtension = GetFileExtension(file.Name).ToLowerInvariant();

        if (_anahAidRequestNotificationFileExtension != "pdf" && _anahAidRequestNotificationFileExtension != "png" && _anahAidRequestNotificationFileExtension != "jpeg" &&
            _anahAidRequestNotificationFileExtension != "jpg" && _anahAidRequestNotificationFileExtension != "webp")
        {
            _anahAidRequestNotificationErrorMessage = Labels.Errors.BadUploadedFileExtension;
            return;
        }

        _anahAidRequestNotificationErrorMessage = string.Empty;
        var memoryStream = new MemoryStream();

        await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
        _anahAidRequestNotificationStreamFile = memoryStream;
        _anahAidRequestNotificationFileName = file.Name;
        _anahAidRequestNotificationFileData = memoryStream.ToArray();

        if (IsAccompanyingFileWaitingForValidation())
            await FileService.UploadFileAsync($"{_anahAidRequestNotificationBlobName}.{_anahAidRequestNotificationFileExtension}", $"{_anahAidRequestNotificationBlobName}.{_anahAidRequestNotificationFileExtension}", _anahAidRequestNotificationStreamFile, UserId);
    }

    private async Task UploadGeneralMeetingFile(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file.Size > Constants.MaxFileSize)
        {
            _generalMeetingErrorMessage = Labels.Errors.MaximumFileSizeExceeded;
            return;
        }

        _generalMeetingFileExtension = GetFileExtension(file.Name).ToLowerInvariant();

        if (_generalMeetingFileExtension != "pdf" && _generalMeetingFileExtension != "png" && _generalMeetingFileExtension != "jpeg" &&
            _generalMeetingFileExtension != "jpg" && _generalMeetingFileExtension != "webp")
        {
            _generalMeetingErrorMessage = Labels.Errors.BadUploadedFileExtension;
            return;
        }

        _generalMeetingErrorMessage = string.Empty;
        var memoryStream = new MemoryStream();

        await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
        _generalMeetingStreamFile = memoryStream;
        _generalMeetingFileName = file.Name;
        _generalMeetingFileData = memoryStream.ToArray();

        if (IsAccompanyingFileWaitingForValidation())
            await FileService.UploadFileAsync($"{_generalMeetingBlobName}.{_generalMeetingFileExtension}", $"{_generalMeetingBlobName}.{_generalMeetingFileExtension}", _generalMeetingStreamFile);
    }



    public async Task ViewFile(byte[]? fileData, string fileName)
    {
        if (fileData != null)
        {
            var base64 = Convert.ToBase64String(fileData);

            var mimeType = "application/octet-stream";
            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) mimeType = "application/pdf";
            if (fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
            if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
            if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) mimeType = "image/png";
            if (fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)) mimeType = "image/webp";

            await JSRuntime.InvokeVoidAsync("viewFile", base64, mimeType, fileName);
        }
    }

    private async Task DeleteFile(string fileName, Action clearFileState)
    {
        var modalResult =
            await ModalService.Show<FileDeletionModal>(
                new ModalParameters
                {
                    { nameof(FileDeletionModal.FileName), fileName }
                },
                new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
            ).Result;

        if (modalResult.Cancelled)
            return;

        clearFileState();

        StateHasChanged();
    }

    private async Task OnClickDeleteanahAidRequestNotificationFile()
    {
        await DeleteFile(_anahAidRequestNotificationFileName, () =>
        {
            _anahAidRequestNotificationStreamFile = null;
            _anahAidRequestNotificationFileData = null;
            _anahAidRequestNotificationFileName = String.Empty;
            _anahAidRequestNotificationFileExtension = String.Empty;
        });
    }

    private async Task OnClickDeleteGeneralMeetingFile()
    {
        await DeleteFile(_generalMeetingFileName, () =>
        {
            _generalMeetingFileData = null;
            _generalMeetingStreamFile = null;
            _generalMeetingFileName = String.Empty;
            _generalMeetingFileExtension = String.Empty;
        });
    }

    public static string GetFileExtension(string fileName) =>
        fileName.Split('.')[^1];

    private async Task OnSubmitSynthesis()
    {
        if (_anahAidRequestNotificationStreamFile == null)
        {
            _anahAidRequestNotificationErrorMessage = Labels.Errors.UploadSynthesisFile;
            return;
        }

        if (_generalMeetingStreamFile == null)
        {
            _generalMeetingErrorMessage = Labels.Errors.UploadSynthesisFile;
            return;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var userIdString = authState.User;

        if (userIdString is null) return;

        var role = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (role is null) return;

        _anahAidRequestNotificationFileName = $"{_anahAidRequestNotificationBlobName}.{_anahAidRequestNotificationFileExtension}";
        _generalMeetingFileName = $"{_generalMeetingBlobName}.{_generalMeetingFileExtension}";

        var synthesisCopropertyValidationModalViewModel = new OrganizeAndFinanceCopropertySynthesisValidationModalViewModel
        {
            CopropertyProfileId = CopropertyProfileId,
            UserRole = role.Value,
            IsInTzeeProgram = ViewModel.IsInTzeeProgram
        };

        ModalService.Show<OrganizeAndFinanceCopropertySynthesisValidationModal>(
            new ModalParameters
            {
                { nameof(OrganizeAndFinanceCopropertySynthesisValidationModal.ViewModel), synthesisCopropertyValidationModalViewModel },
                { nameof(OrganizeAndFinanceCopropertySynthesisValidationModal.AnahAidRequestNotificationFileName), _anahAidRequestNotificationFileName },
                { nameof(OrganizeAndFinanceCopropertySynthesisValidationModal.AnahAidRequestNotificationMemoryStream), _anahAidRequestNotificationStreamFile },
                { nameof(OrganizeAndFinanceCopropertySynthesisValidationModal.GeneralMeetingFileName), _generalMeetingFileName },
                { nameof(OrganizeAndFinanceCopropertySynthesisValidationModal.GeneralMeetingMemoryStream), _generalMeetingStreamFile }
            },
            new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
        );

    }

    private void NavigateToCopropertyProfileList()
    {
        NavigationManager.NavigateTo(Endpoints.Coproperty);
    }

    private bool ShouldDisplaySynthesis()
    {
        if (NavigationHistoryManager.PreviousUri!.Contains(Endpoints.CopropertyOrganizeAndFinance))
            return true;

        return ViewModel.Stage > AccompanyingFileStage.OrganizingAndFinancing || (ViewModel.Stage == AccompanyingFileStage.OrganizingAndFinancing && ViewModel.Status == AccompanyingFileStatus.WaitingForApproval);
    }

    private bool ShouldDisplaySubmissionButton() => ViewModel.Stage == AccompanyingFileStage.OrganizingAndFinancing && (ViewModel.Status == AccompanyingFileStatus.InProgress || ViewModel.Status == AccompanyingFileStatus.Rejected);

    private bool IsAccompanyingFileWaitingForValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForApproval && ViewModel.Stage == AccompanyingFileStage.OrganizingAndFinancing;
    private bool IsUserSolidarBuilder => UserRole!.Equals(Constants.SolidarBuilderRole);

    private bool IsUserAllowedToModifyDataFolder() =>
(
        UserId == ViewModel.SolidarBuilder ||
        UserId == ViewModel.SecondSolidarBuilder ||
        UserId == ViewModel.DiffuseCoordinator ||
        UserId == ViewModel.TargetedCoordinator ||
        UserId == ViewModel.TerritorialBuilder ||
        UserId == ViewModel.SecondTerritorialBuilder ||
        UserId == ViewModel.ThirdSolidarBuilder
) || UserRole == Constants.AdminRole;

    private void OnClickCancelButton() => NavigationManager.NavigateTo($"{Endpoints.CopropertyIdentification}/{CopropertyProfileId}");

    public async Task StageValidation(bool isValidationPopUp)
    {
        var modal = ModalService.Show<CopropertyStageValidationModal>(
                new ModalParameters {
                { nameof(CopropertyStageValidationModal.CopropertyProfileId), CopropertyProfileId },
                { nameof(CopropertyStageValidationModal.IsValidationModal), isValidationPopUp },
                { nameof(CopropertyStageValidationModal.UserId), UserId }
                },
                new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true }
            );

        var result = await modal.Result;

        if (result.Confirmed) NavigationManager.NavigateTo($"{Endpoints.Coproperty}");
    }

}
