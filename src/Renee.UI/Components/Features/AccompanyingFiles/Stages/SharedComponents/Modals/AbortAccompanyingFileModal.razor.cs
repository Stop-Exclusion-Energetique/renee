using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AbortAccompanyingFile;
using Renee.Domain;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.Result;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals;

public partial class AbortAccompanyingFileModal
{
	[CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; } = default!;
	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;
	[Inject] public IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] public IModalService ModalService { get; set; } = null!;
	[Inject] private IFileService FileService { get; set; } = null!;

	[Parameter] public string AccompanyingFileReference { get; set; } = string.Empty;

	public AbortAccompanyingFileModalViewModel ViewModel { get; set; } = new();
	public List<ZeeSelectItem<Guid?>>? AbortReasonLabels { get; set; } = [];

	private EditContext? _editContext;
	private string _fileName = string.Empty;
	private byte[]? _fileData;
	private string _blobName = string.Empty;
	private string _fileExtension = string.Empty;
	public string ErrorMessage { get; set; } = string.Empty;

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(ViewModel);

		var abortResult = await SendEventQuery.Send(new GetAllAbortReasonLabelsQuery());
		if (abortResult.IsSuccess && abortResult.Value is not null)
			AbortReasonLabels = abortResult.Value
				.Select(x => new ZeeSelectItem<Guid?>(x.Label, x.Id)).ToList();
	}

	public async Task ViewFile()
	{
		if (_fileData != null)
		{
			var base64 = Convert.ToBase64String(_fileData);

			var mimeType = _fileExtension switch
			{
				"pdf" => "application/pdf",
				"jpeg" => "image/jpeg",
				"jpg" => "image/jpeg",
				"png" => "image/png",
				"webp" => "image/webp",
				_ => "application/octet-stream"
			};

			await JSRuntime.InvokeVoidAsync("viewFile", base64, mimeType, _fileName);
		}
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

		var allowedExtensions = new HashSet<string> { "pdf", "png", "jpeg", "jpg", "webp" };
		if (!allowedExtensions.Contains(_fileExtension))
		{
			ErrorMessage = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		var memoryStream = new MemoryStream();
		await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		memoryStream.Position = 0;

		ViewModel.AbortJustificationFileStream = memoryStream;
		_editContext?.NotifyFieldChanged(FieldIdentifierHelper.GetFieldIdentifierForProperty(() => ViewModel.AbortJustificationFileStream));
		
		_fileName = file.Name;
		_fileData = memoryStream.ToArray();
		ErrorMessage = string.Empty;

		StateHasChanged();
	}

	private void OnClickDeleteFile()
	{
		ViewModel.AbortJustificationFileStream = null;
		_fileData = null;
		_fileName = string.Empty;
		_fileExtension = string.Empty;

		ErrorMessage = string.Empty;

		StateHasChanged();
	}
	
	public static string GetFileExtension(string fileName)
	{
		if (string.IsNullOrEmpty(fileName) || !fileName.Contains('.'))
			return string.Empty;
		return fileName.Split('.')[^1];
	}

	private Task OnBillingRequestedChanged(bool? value)
	{
		ViewModel.IsBillingRequested = value;
		_editContext?.NotifyFieldChanged(FieldIdentifierHelper.GetFieldIdentifierForProperty(() => ViewModel.AbortJustificationFileStream));

		return Task.CompletedTask;
	}

	private async Task AbortAccompanyingFile()
	{
		if (_editContext is null || !_editContext.Validate())
			return;

		if (ViewModel.AbortJustificationFileStream != null) 
		{
			_blobName = $"{AccompanyingFileReference}_{Labels.AbortionJustification}.{_fileExtension}";
			await FileService.UploadFileAsync(_blobName, _blobName, ViewModel.AbortJustificationFileStream);
		}

        await ModalInstance.CloseAsync(ModalResult.Ok(new AbortAccompanyingFileModalResult(
			ViewModel.AbortReasonLabelId,
			ViewModel.AbortRequestDetails,
			ViewModel.IsBillingRequested,
			ViewModel.AbortJustificationFileStream is not null)));
    }
}