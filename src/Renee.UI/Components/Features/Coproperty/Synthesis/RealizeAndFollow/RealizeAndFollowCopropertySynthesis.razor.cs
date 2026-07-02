using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.ViewModel;
using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;
using Radzen;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;
using Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.Modal.ViewModel;
using Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.Modal;
using Renee.UI.Components.Features.Coproperty.SynthesisValidation;
using Renee.UI.Components.Layout.SynthesisLayoutManager;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow;

public partial class RealizeAndFollowCopropertySynthesis
{
    [Parameter] public Guid CopropertyProfileId { get; set; }

    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public NavigationHistoryManager NavigationHistoryManager { get; set; } = null!;
    [Inject] public SynthesysLayoutStateManager SynthesysLayoutStateManager { get; set; } = null!;
    [Inject] private IModalService ModalService { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
    [Inject] private IFileService FileService { get; set; } = null!;

    public RealizeAndFollowCopropertySynthesisViewModel ViewModel { get; set; } = new();
    private CopropertyProfileRealizeAndFollowDto? CopropertyDto { get; set; }

    private MemoryStream? _memoryStreamFile;
    private string _fileName = string.Empty;
    private byte[]? _fileData;
    private string _blobName = string.Empty;
    private string _fileExtension = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

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

        var result = await CopropertyProfileService.GetCopropertyProfileForRealizeAndFollowMilestoneById(CopropertyProfileId, UserId, UserRole);

        if (!result.IsSuccess)
        {
            NavigateToCopropertyProfileList();
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
            return;
        }
        CopropertyDto = result.Value;

        if (CopropertyDto != null)
        {
            ViewModel = RealizeAndFollowCopropertySynthesisViewModel.CreateViewModelFromDto(CopropertyDto);

            SynthesysLayoutStateManager.AssociatedResourceReference = ViewModel.Reference;
            SynthesysLayoutStateManager.AssociatedResourceId = CopropertyProfileId;
            SynthesysLayoutStateManager.AccompanyingFileStage = AccompanyingFileStage.RealisationAndFollowing;
            SynthesysLayoutStateManager.ShouldDisplayNavigationButton = !NavigationHistoryManager.PreviousUri!.Contains(Endpoints.CopropertyRealizeAndFollow);
            SynthesysLayoutStateManager.IsAccompanyingFile = false;

            SynthesysLayoutStateManager.NotifyStateChanged();

            _blobName = $"{ViewModel.Reference}_{Labels.HandoverReportDocument}";

            try
            {
                (_memoryStreamFile, _fileName) = await FileService.DownloadFileForSynthesisAsync(_blobName);
                if (_memoryStreamFile != null)
                {
                    _fileData = _memoryStreamFile.ToArray();
                    _fileExtension = GetFileExtension(_fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
            }
        }
    }

    private void NavigateToCopropertyProfileList()
    {
        NavigationManager.NavigateTo(Endpoints.Coproperty);
    }

    private bool ShouldDisplaySynthesis()
    {
        if (NavigationHistoryManager.PreviousUri!.Contains(Endpoints.CopropertyRealizeAndFollow))
            return true;

        return ViewModel.Stage > AccompanyingFileStage.RealisationAndFollowing 
            || (ViewModel.Stage == AccompanyingFileStage.RealisationAndFollowing && ViewModel.Status == AccompanyingFileStatus.WaitingForApproval);
    }

    private async Task OnSubmitSynthesis()
    {
        if (_memoryStreamFile == null)
        {
            ErrorMessage = Labels.Errors.UploadSynthesisFile;
            return;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var userIdString = authState.User;

        if (userIdString is null) return;

        var role = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        if (role is null) return;

        _fileName = $"{_blobName}.{_fileExtension}";

        var synthesisCopropertyValidationModalViewModel = new RealizeAndFollowCopropertySynthesisValidationModalViewModel
        {
            CopropertyProfileId = CopropertyProfileId,
            UserRole = role.Value,
            IsInTzeeProgram = ViewModel.IsInTzeeProgram
        };

        ModalService.Show<RealizeAndFollowCopropertySynthesisValidationModal>(
            new ModalParameters
            {
                { nameof(RealizeAndFollowCopropertySynthesisValidationModal.ViewModel), synthesisCopropertyValidationModalViewModel },
                { nameof(RealizeAndFollowCopropertySynthesisValidationModal.FileName), _fileName },
                { nameof(RealizeAndFollowCopropertySynthesisValidationModal.MemoryStream), _memoryStreamFile }
            },
            new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
        );
    }

    public async Task ViewFile()
    {
        if (_fileData != null)
        {
            var base64 = Convert.ToBase64String(_fileData);

            var mimeType = "application/octet-stream";
            if (_fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) mimeType = "application/pdf";
            if (_fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
            if (_fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
            if (_fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) mimeType = "image/png";
            if (_fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)) mimeType = "image/webp";

            await JSRuntime.InvokeVoidAsync("viewFile", base64, mimeType, _fileName);
        }
    }

    private async Task OnClickDeleteFile()
    {
        var modalResult =
            await ModalService.Show<FileDeletionModal>(
                new ModalParameters
                {
                    { nameof(FileDeletionModal.FileName), _fileName }
                },
                new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
            ).Result;

        if (modalResult.Cancelled)
            return;

        _memoryStreamFile = null;
        _fileData = null;
        _fileName = string.Empty;
        _fileExtension = string.Empty;

        StateHasChanged();
    }

    private bool ShouldDisplaySubmissionButton() => ViewModel.Stage == AccompanyingFileStage.RealisationAndFollowing && (ViewModel.Status == AccompanyingFileStatus.InProgress || ViewModel.Status == AccompanyingFileStatus.Rejected);

    private bool IsAccompanyingFileWaitingForValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForApproval && ViewModel.Stage == AccompanyingFileStage.RealisationAndFollowing;
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

    private void OnClickCancelButton() => NavigationManager.NavigateTo($"{Endpoints.CopropertyRealizeAndFollow}/{CopropertyProfileId}");

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

    public async Task OnChangeUpload(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file.Size > Constants.MaxFileSize)
        {
            ErrorMessage = Labels.Errors.MaximumFileSizeExceeded;
            return;
        }

        _fileExtension = GetFileExtension(file.Name).ToLowerInvariant();

        if (_fileExtension != "pdf" && _fileExtension != "png" && _fileExtension != "jpeg" && _fileExtension != "jpg" &&
            _fileExtension != "webp")
        {
            ErrorMessage = Labels.Errors.BadUploadedFileExtension;
            return;
        }

        ErrorMessage = string.Empty;
        var memoryStream = new MemoryStream();

        await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        _memoryStreamFile = memoryStream;
        _fileName = file.Name;
        _fileData = memoryStream.ToArray();

        if (IsAccompanyingFileWaitingForValidation())
            await FileService.UploadFileAsync($"{_blobName}.{_fileExtension}", $"{_blobName}.{_fileExtension}", _memoryStreamFile, UserId);
    }
    public static string GetFileExtension(string fileName) =>
         fileName.Split('.')[^1];
}