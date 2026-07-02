using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Synthesis.Modal.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;
using System.Security.Claims;
using Renee.UI.Components.Features.AccompanyingFiles.List.Modal;
using Renee.UI.Components.Layout.SynthesisLayoutManager;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance;

public partial class OrganizeAndFinanceSynthesis
{
	[Parameter] public Guid AccompanyingFileId { get; set; }

	public bool? IsFinancingAsked { get; set; } = false;
	public OrganizeAndFinanceSynthesisViewModel ViewModel { get; private set; } = new();

	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public IConfiguration Configuration { get; set; } = null!;
	[Inject]public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject]public IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] private IModalService ModalService { get; set; } = null!;
	[Inject] private IFileService FileService { get; set; } = null!;
	[Inject] public NavigationHistoryManager NavigationHistoryManager { get; set; } = null!;
	[Inject] public SynthesysLayoutStateManager SynthesysLayoutStateManager { get; set; } = null!;

	private string _anahGrantNotificationErrorMessage = string.Empty;
	private MemoryStream? _anahGrantNotificationStreamFile;
	private string _anahGrantNotificationFileName = string.Empty;
	private byte[]? _anahGrantNotificationFileData;
	private string _anahGrantNotificationBlobName = string.Empty;
	private string _anahGrantNotificationFileExtension = string.Empty;

	private string _energyAuditErrorMessage = string.Empty;
	private MemoryStream? _energyAuditStreamFile;
	private string _energyAuditFileName = string.Empty;
	private byte[]? _energyAuditFileData;
	private string _energyAuditBlobName = string.Empty;
	private string _energyAuditFileExtension = string.Empty;

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

		if (AccompanyingFileId != Guid.Empty)
		{
			ViewModel = new OrganizeAndFinanceSynthesisPresenter().FromQuery(
				(await AccompanyingFileService.GetOrganizeAndFinanceSynthesis(AccompanyingFileId)).Value!).Present();

			SynthesysLayoutStateManager.AssociatedResourceReference = ViewModel.AccompanyingFileReference;
			SynthesysLayoutStateManager.AssociatedResourceId = AccompanyingFileId;
			SynthesysLayoutStateManager.AccompanyingFileStage = AccompanyingFileStage.OrganizingAndFinancing;
			SynthesysLayoutStateManager.ShouldDisplayNavigationButton = !NavigationHistoryManager.PreviousUri!.Contains(Endpoints.OrganizeAndFinanceStage);
			SynthesysLayoutStateManager.NotifyStateChanged();

			_anahGrantNotificationBlobName = string.Format(
				"{0}_{1}",
				ViewModel.AccompanyingFileReference,
				Labels.AnahNotification);

			_energyAuditBlobName = string.Format(
				"{0}_{1}",
				ViewModel.AccompanyingFileReference,
				Labels.EnergyAudit);

			try
			{
				(_anahGrantNotificationStreamFile, _anahGrantNotificationFileName) = await FileService.DownloadFileForSynthesisAsync(_anahGrantNotificationBlobName);
				if (_anahGrantNotificationStreamFile != null)
				{
					_anahGrantNotificationFileData = _anahGrantNotificationStreamFile.ToArray();
					_anahGrantNotificationFileExtension = GetFileExtension(_anahGrantNotificationFileName);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
			}

			try
			{
				(_energyAuditStreamFile, _energyAuditFileName) = await FileService.DownloadFileForSynthesisAsync(_energyAuditBlobName);
				if (_energyAuditStreamFile != null)
				{
					_energyAuditFileData = _energyAuditStreamFile.ToArray();
					_energyAuditFileExtension = GetFileExtension(_energyAuditFileName);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
			}
		}
	}

	public static string GetFileExtension(string fileName) =>
	fileName.Split('.')[^1];

	private async Task UploadAnahGrantNotificationFile(InputFileChangeEventArgs e)
	{
		var file = e.File;
		if (file.Size > Constants.MaxFileSize)
		{
			_anahGrantNotificationErrorMessage = Labels.Errors.MaximumFileSizeExceeded;
			return;
		}

		_anahGrantNotificationFileExtension = GetFileExtension(file.Name).ToLowerInvariant();

		if (_anahGrantNotificationFileExtension != "pdf" && _anahGrantNotificationFileExtension != "png" && _anahGrantNotificationFileExtension != "jpeg" &&
			_anahGrantNotificationFileExtension != "jpg" && _anahGrantNotificationFileExtension != "webp")
		{
			_anahGrantNotificationErrorMessage = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		_anahGrantNotificationErrorMessage = string.Empty;
		var memoryStream = new MemoryStream();

		await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		_anahGrantNotificationStreamFile = memoryStream;
		_anahGrantNotificationFileName = file.Name;
		_anahGrantNotificationFileData = memoryStream.ToArray();

		if (IsAccompanyingFileWaitingForValidation())
			await FileService.UploadFileAsync($"{_anahGrantNotificationBlobName}.{_anahGrantNotificationFileExtension}", $"{_anahGrantNotificationBlobName}.{_anahGrantNotificationFileExtension}", _anahGrantNotificationStreamFile);
	}

	private async Task UploadEnergyAuditFile(InputFileChangeEventArgs e)
	{
		var file = e.File;
		if (file.Size > Constants.MaxFileSize)
		{
			_energyAuditErrorMessage = Labels.Errors.MaximumFileSizeExceeded;
			return;
		}

		_energyAuditFileExtension = GetFileExtension(file.Name).ToLowerInvariant();

		if (_energyAuditFileExtension != "pdf" && _energyAuditFileExtension != "png" && _energyAuditFileExtension != "jpeg" &&
			_energyAuditFileExtension != "jpg" && _energyAuditFileExtension != "webp")
		{
			_energyAuditErrorMessage = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		_energyAuditErrorMessage = string.Empty;
		var memoryStream = new MemoryStream();

		await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		_energyAuditStreamFile = memoryStream;
		_energyAuditFileName = file.Name;
		_energyAuditFileData = memoryStream.ToArray();

		if (IsAccompanyingFileWaitingForValidation())
			await FileService.UploadFileAsync($"{_energyAuditBlobName}.{_energyAuditFileExtension}", $"{_energyAuditBlobName}.{_energyAuditFileExtension}", _energyAuditStreamFile);
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

	private async Task OnClickDeleteAnahGrantNotificationFile()
	{
		await DeleteFile(_anahGrantNotificationFileName, () =>
		{
			_anahGrantNotificationStreamFile = null;
			_anahGrantNotificationFileData = null;
			_anahGrantNotificationFileName = string.Empty;
			_anahGrantNotificationFileExtension = string.Empty;
		});
	}

	private async Task OnClickDeleteEnergyAuditFile()
	{
		await DeleteFile(_energyAuditFileName, () =>
		{
			_energyAuditStreamFile = null;
			_energyAuditFileData = null;
			_energyAuditFileName = string.Empty;
			_energyAuditFileExtension = string.Empty;
		});
	}

	private async Task OnSubmitSynthesis()
	{
		if (IsFinancingAsked is not true) return;

		var isFileDeposited = true;

		if (_anahGrantNotificationStreamFile == null)
		{
			_anahGrantNotificationErrorMessage = GenerateFilesLabels.Errors.AnahUploadError;
			isFileDeposited = false;
		}

		if (_energyAuditStreamFile == null)
		{
			_energyAuditErrorMessage = GenerateFilesLabels.Errors.AnahUploadError;
			isFileDeposited = false;
		}

		if (!isFileDeposited)
		{
			return;
		}

		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var claims = authState.User;
		var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

		var synthesisValidationModalViewModel = new OrganizeAndFinanceSynthesisValidationModalViewModel
		{
			AccompanyingFileId = AccompanyingFileId,
			EnergyClassJump = ViewModel.EstimatedEnergyClassJump,
			UserRole = userRole
		};

		_anahGrantNotificationFileName = $"{_anahGrantNotificationBlobName}.{_anahGrantNotificationFileExtension}";
		_energyAuditFileName = $"{_energyAuditBlobName}.{_energyAuditFileExtension}";

		ModalService.Show<OrganizeAndFinanceSynthesisValidationModal>(
			new ModalParameters
			{
				{ nameof(OrganizeAndFinanceSynthesisValidationModal.ViewModel), synthesisValidationModalViewModel },
				{ nameof(OrganizeAndFinanceSynthesisValidationModal.AnahGrantNotificationFileName), _anahGrantNotificationFileName },
				{ nameof(OrganizeAndFinanceSynthesisValidationModal.AnahGrantNotificationStream), _anahGrantNotificationStreamFile },
				{ nameof(OrganizeAndFinanceSynthesisValidationModal.EnergyAuditFileName), _energyAuditFileName },
				{ nameof(OrganizeAndFinanceSynthesisValidationModal.EnergyAuditStream   ), _energyAuditStreamFile }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });
	}

	private void OnClickCancelButton() => NavigationManager.NavigateTo($"{Endpoints.OrganizeAndFinanceStage}/{AccompanyingFileId}");

    private bool ShouldDisplaySynthesis()
	{
		if (NavigationHistoryManager.PreviousUri!.Contains(Endpoints.OrganizeAndFinanceStage))
			return true;

		return ViewModel.Stage > AccompanyingFileStage.OrganizingAndFinancing || (ViewModel.Stage == AccompanyingFileStage.OrganizingAndFinancing && ViewModel.Status == AccompanyingFileStatus.WaitingForApproval);
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
		) || UserRole == Constants.AdminRole;

	private bool ShouldDisplaySubmissionButton() => ViewModel.Stage == AccompanyingFileStage.OrganizingAndFinancing && (ViewModel.Status == AccompanyingFileStatus.InProgress || ViewModel.Status == AccompanyingFileStatus.Rejected);

	private bool IsAccompanyingFileWaitingForValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForApproval && ViewModel.Stage == AccompanyingFileStage.OrganizingAndFinancing;
	private bool IsUserSolidarBuilder => UserRole!.Equals(Constants.SolidarBuilderRole);
	public async Task StageValidation(bool isValidationPopUp)
	{
		var modal = ModalService.Show<StageValidationModal>(
				new ModalParameters {
					{ nameof(StageValidationModal.AccompanyingFileId), AccompanyingFileId },
					{ nameof(StageValidationModal.IsValidationModal), isValidationPopUp },
					{ nameof(StageValidationModal.UserId), UserId }
				},
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true }
			);

        var result = await modal.Result;

		if(result.Confirmed) NavigationManager.NavigateTo($"{Endpoints.UserCreatedAccompanyingFiles}");
	}
}