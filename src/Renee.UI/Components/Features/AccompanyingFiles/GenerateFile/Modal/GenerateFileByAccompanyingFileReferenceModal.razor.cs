using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Features.AccompanyingFiles.GenerateFile.Modal.ViewModel;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AccompanyingFiles.GenerateFile.Modal;

public partial class GenerateFileByAccompanyingFileReferenceModal
{
	[CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; } = default!;
	[Parameter] public FileType FileType { get; set; }
	[Parameter] public string ModalTitle { get; set; } = string.Empty;

	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] public IWebHostEnvironment WebHostEnvironment { get; set; } = null!;
	[Inject] public IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] public IGenerateFilesService GenerateFileService { get; set; } = null!;
	[Inject] public ITelemetryService TelemetryService { get; set; } = null!;
	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;

	public GenerateFileByAccompanyingFileReferenceViewModel ViewModel { get; } = new();

	private static readonly Dictionary<FileType, (
		string FileName,
		Func<IGenerateFilesService, string, string, Guid, Task<ReneeOperationResult<string>>> Generator)> FileMap = 
		new()
		{
			[FileType.AnahSynthesis] = (
			GenerateFilesLabels.AnahSynthesisDocumentName,
			(svc, path, reference, userId) => svc.GenerateAnahSynthesis(path, reference, userId)),
			[FileType.WorkCertificate] = (
			GenerateFilesLabels.WorkCertificateDocumentName,
			(svc, path, reference, userId) => svc.GenerateWorkCertificate(path, reference, userId)),
		};

	private EditContext? _editContext;
	private string? _errorMessage;
	private Guid _userId;

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(ViewModel);

		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

		var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (Guid.TryParse(value, out var userId))
			_userId = userId;
		else
			_userId = Guid.Empty;
	}

	public async Task GenerateDocument()
	{
		try
		{
			if (!FileMap.TryGetValue(FileType, out var config))
			{
				_errorMessage = GenerateFilesLabels.Errors.ErrorWhileGeneratingDocument;
				return;
			}

			var absolutePath = Path.Combine(WebHostEnvironment.WebRootPath, "PdfsFiles", config.FileName);
			if (!File.Exists(absolutePath))
			{
				_errorMessage = string.Format(GenerateFilesLabels.Errors.FileNotFound, config.FileName);
				return;
			}

			var result = await config.Generator(GenerateFileService, absolutePath, ViewModel.Reference, _userId);
			if (!result.IsSuccess)
			{
				_errorMessage = result.Message;
				return;
			}

			_errorMessage = string.Empty;

			await JSRuntime.InvokeVoidAsync("downloadFile", config.FileName, result.Value);
			await SendEventQuery.Send(new CreateDocumentGenerationLogCommandInput(_userId, config.FileName));

			await ModalInstance.CloseAsync();
		}
		catch (Exception ex)
		{
			await TelemetryService.TrackExceptionAsync(ex);
			_errorMessage = GenerateFilesLabels.Errors.ErrorWhileGeneratingDocument;
		}
	}
}