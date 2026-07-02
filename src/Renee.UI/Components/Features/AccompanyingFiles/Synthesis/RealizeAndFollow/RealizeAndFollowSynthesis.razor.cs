using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.List.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;
using Renee.UI.Components.Layout.SynthesisLayoutManager;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow;

public partial class RealizeAndFollowSynthesis
{
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;

	[Parameter] public Guid AccompanyingFileId { get; set; }

	[Inject] public SynthesysLayoutStateManager SynthesysLayoutStateManager { get; set; } = null!;
	[Inject] public NavigationHistoryManager NavigationHistoryManager { get; set; } = null!;
	[Inject] public IFileService FileService { get; set; } = null!;
	[Inject] public IJSRuntime JsRuntime { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] private IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] private IModalService ModalService { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;


	private IBrowserFile? _workReceiptPv;
	private string _workReceiptError = string.Empty;
	private byte[]? _workReceiptPvData;
	private string _workReceiptPvFileName = string.Empty;
	private string _workReceiptPvBlob = string.Empty;
	private string _workReceiptPvExtension = string.Empty;
	private MemoryStream? _workReceiptStream;

	private IBrowserFile? _accompanyingFileReport;
	private string _accompanyingFileReportError = string.Empty;
	private byte[]? _accompanyingFileReportData;
	private string _accompanyingFileReportFileName = string.Empty;
	private string _accompanyingFileReportBlob = string.Empty;
	private string _accompanyingFileReportExtension = string.Empty;
	private MemoryStream? _accompanyingFileReportStream;

	private IBrowserFile? _anahHelpObtentionFile;
	private string _anahHelpObtentionFileError = string.Empty;
	private byte[]? _anahHelpObtentionFileData;
	private string _anahHelpObtentionFileName = string.Empty;
	private string _anahHelpObtentionFileBlob = string.Empty;
	private string _anahHelpObtentionFileExtension = string.Empty;
	private MemoryStream? _anahHelpObtentionFileStream;

	public CreateRealiseAndFollowSynthesisViewModel ViewModel { get; set; } = new();

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
			var synthesisResult = await AccompanyingFileService.GetRealiseAndFollowSynthesis(AccompanyingFileId);
			if (!synthesisResult.IsSuccess || synthesisResult.Value is null)
				return;

			ViewModel = new CreateRealiseAndFollowSynthesisPresenter().FromQuery(synthesisResult.Value)
				.Present();

			SynthesysLayoutStateManager.AssociatedResourceReference = ViewModel.AccompaniyingFileReference;
			SynthesysLayoutStateManager.AssociatedResourceId = AccompanyingFileId;
			SynthesysLayoutStateManager.AccompanyingFileStage = AccompanyingFileStage.RealisationAndFollowing;
			SynthesysLayoutStateManager.ShouldDisplayNavigationButton = !NavigationHistoryManager.PreviousUri!.Contains(Endpoints.RealiseAndFollowStage);
			SynthesysLayoutStateManager.NotifyStateChanged();

			_workReceiptPvBlob = $"{ViewModel.AccompaniyingFileReference}_{RealiseAndFollowMilestone.Labels.WorksReceiptPv}";
			_accompanyingFileReportBlob = $"{ViewModel.AccompaniyingFileReference}_{RealiseAndFollowMilestone.Labels.AccompanyingFileReport}";
			_anahHelpObtentionFileBlob = $"{ViewModel.AccompaniyingFileReference}_{RealiseAndFollowMilestone.Labels.AnahHelpObtention}";

			try
			{
				(_workReceiptStream, _workReceiptPvFileName) = await FileService.DownloadFileForSynthesisAsync(_workReceiptPvBlob);
				if (_workReceiptStream != null)
				{
					_workReceiptPvExtension = GetFileExtension(_workReceiptPvFileName);
					_workReceiptPvData = _workReceiptStream.ToArray();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
			}

			try
			{
				(_accompanyingFileReportStream, _accompanyingFileReportFileName) = await FileService.DownloadFileForSynthesisAsync(_accompanyingFileReportBlob);
				if (_accompanyingFileReportStream != null)
				{
					_accompanyingFileReportExtension = GetFileExtension(_accompanyingFileReportFileName);
					_accompanyingFileReportData = _accompanyingFileReportStream.ToArray();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
			}

			try
			{
				(_anahHelpObtentionFileStream, _anahHelpObtentionFileName) = await FileService.DownloadFileForSynthesisAsync(_anahHelpObtentionFileBlob);
				if (_anahHelpObtentionFileStream != null)
				{
					_anahHelpObtentionFileExtension = GetFileExtension(_anahHelpObtentionFileName);
					_anahHelpObtentionFileData = _anahHelpObtentionFileStream.ToArray();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
			}
		}
	}

	public bool AreSupportingDocumentsAsked =>
		_workReceiptPvData is not null &&
		_accompanyingFileReportData is not null &&
		_anahHelpObtentionFileData is not null &&
		string.IsNullOrEmpty(_accompanyingFileReportError) &&
		string.IsNullOrEmpty(_workReceiptError) &&
		string.IsNullOrEmpty(_anahHelpObtentionFileError);

	public static string GetFileExtension(string fileName) =>
		fileName.Split('.')[^1];

	private bool ShouldDisplaySynthesis()
	{
		if (NavigationHistoryManager.PreviousUri!.Contains(Endpoints.RealiseAndFollowStage))
			return true;

		return ViewModel.Stage > AccompanyingFileStage.RealisationAndFollowing || (ViewModel.Stage == AccompanyingFileStage.RealisationAndFollowing && ViewModel.Status == AccompanyingFileStatus.WaitingForApproval);
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

	private void OnClickCancelButton() => NavigationManager.NavigateTo($"{Endpoints.RealiseAndFollowStage}/{ViewModel.Id}");

	private async Task UploadAccompanyingFileReport(InputFileChangeEventArgs e)
	{
		_accompanyingFileReport = e.File;
		_accompanyingFileReportError = string.Empty;

		if (_accompanyingFileReport is null) return;

		if (_accompanyingFileReport.Size > Constants.MaxFileSize)
		{
			_accompanyingFileReportError = Labels.Errors.MaximumFileSizeExceeded;
			return;
		}

		_accompanyingFileReportExtension = GetFileExtension(_accompanyingFileReport.Name).ToLowerInvariant();
		if (_accompanyingFileReportExtension != "pdf" && _accompanyingFileReportExtension != "png" && _accompanyingFileReportExtension != "jpeg" &&
			_accompanyingFileReportExtension != "jpg" && _accompanyingFileReportExtension != "webp")
		{
			_accompanyingFileReportError = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		var memoryStream = new MemoryStream();
		await _accompanyingFileReport.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		memoryStream.Position = 0;

		_accompanyingFileReportStream = memoryStream;
		_accompanyingFileReportData = memoryStream.ToArray();
		_accompanyingFileReportFileName = _accompanyingFileReport.Name;

		if (IsAccompanyingFileWaitingForValidation())
			await FileService.UploadFileAsync($"{_accompanyingFileReportBlob}.{_accompanyingFileReportExtension}", $"{_accompanyingFileReportBlob}.{_accompanyingFileReportExtension}", _accompanyingFileReportStream, UserId);
	}

	private async Task UploadWorkReceiptPv(InputFileChangeEventArgs e)
	{
		_workReceiptPv = e.File;
		_workReceiptError = string.Empty;

		if (_workReceiptPv is null) return;

		if (_workReceiptPv.Size > Constants.MaxFileSize)
		{
			_workReceiptError = Labels.Errors.MaximumFileSizeExceeded;
			return;
		}

		_workReceiptPvExtension = GetFileExtension(_workReceiptPv.Name).ToLowerInvariant();
		if (_workReceiptPvExtension != "pdf" && _workReceiptPvExtension != "png" && _workReceiptPvExtension != "jpeg" &&
			_workReceiptPvExtension != "jpg" && _workReceiptPvExtension != "webp")
		{
			_workReceiptError = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		var memoryStream = new MemoryStream();
		await _workReceiptPv.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		memoryStream.Position = 0;

		_workReceiptStream = memoryStream;
		_workReceiptPvData = memoryStream.ToArray();
		_workReceiptPvFileName = _workReceiptPv.Name;

		if (IsAccompanyingFileWaitingForValidation())
			await FileService.UploadFileAsync($"{_workReceiptPvBlob}.{_workReceiptPvExtension}", $"{_workReceiptPvBlob}.{_workReceiptPvExtension}", _workReceiptStream);
	}

	private async Task UploadAnahHelpObtentionFile(InputFileChangeEventArgs e)
	{
		_anahHelpObtentionFile = e.File;
		_anahHelpObtentionFileError = string.Empty;

		if (_anahHelpObtentionFile is null) return;

		if (_anahHelpObtentionFile.Size > Constants.MaxFileSize)
		{
			_anahHelpObtentionFileError = Labels.Errors.MaximumFileSizeExceeded;
			return;
		}

		_anahHelpObtentionFileExtension = GetFileExtension(_anahHelpObtentionFile.Name).ToLowerInvariant();

		if (_anahHelpObtentionFileExtension != "pdf" && _anahHelpObtentionFileExtension != "png" && _anahHelpObtentionFileExtension != "jpeg" &&
			_anahHelpObtentionFileExtension != "jpg" && _anahHelpObtentionFileExtension != "webp")
		{
			_anahHelpObtentionFileError = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		var memoryStream = new MemoryStream();
		await _anahHelpObtentionFile.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		memoryStream.Position = 0;
		_anahHelpObtentionFileStream = memoryStream;
		_anahHelpObtentionFileData = memoryStream.ToArray();
		_anahHelpObtentionFileName = _anahHelpObtentionFile.Name;
		if (IsAccompanyingFileWaitingForValidation())
			await FileService.UploadFileAsync($"{_anahHelpObtentionFileBlob}.{_anahHelpObtentionFileExtension}", $"{_anahHelpObtentionFileBlob}.{_anahHelpObtentionFileExtension}", _anahHelpObtentionFileStream);
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

	private async Task OnClickDeleteAccompanyingFileReport()
	{
		await DeleteFile(_accompanyingFileReportFileName, () =>
		{
			_accompanyingFileReportStream = null;
			_accompanyingFileReportData = null;
			_accompanyingFileReportFileName = string.Empty;
			_accompanyingFileReportExtension = string.Empty;
		});
	}

	private async Task OnClickDeleteWorkReceiptPv()
	{
		await DeleteFile(_workReceiptPvFileName, () =>
		{
			_workReceiptStream = null;
			_workReceiptPvData = null;
			_workReceiptPvFileName = string.Empty;
			_workReceiptPvExtension = string.Empty;
		});

	}

	private async Task OnClickDeleteAnahHelpObtentionFile()
	{
		await DeleteFile(_anahHelpObtentionFileName, () =>
		{
			_anahHelpObtentionFileStream = null;
			_anahHelpObtentionFileData = null;
			_anahHelpObtentionFileName = string.Empty;
			_anahHelpObtentionFileExtension = string.Empty;
		});
	}

	private void OnSubmit()
	{
		if (_workReceiptStream == null || _accompanyingFileReportStream == null) return;

		_workReceiptPvFileName = $"{_workReceiptPvBlob}.{_workReceiptPvExtension}";
		_accompanyingFileReportFileName = $"{_accompanyingFileReportBlob}.{_accompanyingFileReportExtension}";
		_anahHelpObtentionFileName = $"{_anahHelpObtentionFileBlob}.{_anahHelpObtentionFileExtension}";

		ModalService.Show<RealizeAndFollowSynthesisValidationModal>(
			new ModalParameters
			{
				{ nameof(RealizeAndFollowSynthesisValidationModal.AccompanyingFileId), ViewModel.Id },
				{ nameof(RealizeAndFollowSynthesisValidationModal.WorkReceiptPvFileName), _workReceiptPvFileName },
				{ nameof(RealizeAndFollowSynthesisValidationModal.WorkReceiptMemoryStream), _workReceiptStream },
				{ nameof(RealizeAndFollowSynthesisValidationModal.AccompanyingFileReportFileName), _accompanyingFileReportFileName },
				{ nameof(RealizeAndFollowSynthesisValidationModal.AccompanyingFileReportMemoryStream), _accompanyingFileReportStream },
				{ nameof(RealizeAndFollowSynthesisValidationModal.IsInTzeeProgram), ViewModel.IsInTzeeProgram },
				{ nameof(RealizeAndFollowSynthesisValidationModal.AnahHelpObtentionFileName), _anahHelpObtentionFileName },
				{ nameof(RealizeAndFollowSynthesisValidationModal.AnahHelpObtentionMemoryStream), _anahHelpObtentionFileStream }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true, Size = ModalSize.Large });
	}

	private bool ShouldDisplaySubmissionButton() => ViewModel.Stage == AccompanyingFileStage.RealisationAndFollowing && (ViewModel.Status == AccompanyingFileStatus.InProgress || ViewModel.Status == AccompanyingFileStatus.Rejected);

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

	private bool IsAccompanyingFileWaitingForValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForApproval && ViewModel.Stage == AccompanyingFileStage.RealisationAndFollowing;
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