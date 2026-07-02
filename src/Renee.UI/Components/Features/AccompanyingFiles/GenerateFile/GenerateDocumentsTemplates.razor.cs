using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.GenerateFile.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.GenerateFile.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.GenerateFile;

public partial class GenerateDocumentsTemplates
{
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

	public GenerateDocumentsTemplatesViewModel ViewModel { get; set; } = new();

	public List<ZeeSelectItem<Guid?>> Documents { get; set; } =
	[
		new (GenerateFilesLabels.GenerateAnahSynthesisButtonLabel, Guid.NewGuid()),
		new (GenerateFilesLabels.WorkCertificate, Guid.NewGuid())
	];

	private EditContext? _editContext;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
	}

	private void ShowModal()
	{
		var selectedFileName = Documents?.Find(d => d.Value == ViewModel.SelectedDocument)?.Label;
		if (selectedFileName is null)
			return;

		var (selectedFileType, modalTitle) = selectedFileName switch
		{
			GenerateFilesLabels.GenerateAnahSynthesisButtonLabel => (FileType.AnahSynthesis, GenerateFilesLabels.AnahSynthesisModalTitle),
			GenerateFilesLabels.WorkCertificate => (FileType.WorkCertificate, GenerateFilesLabels.WorkCertificateModalTitle),
			_ => throw new InvalidDataException(selectedFileName)
		};

		ModalService.Show<GenerateFileByAccompanyingFileReferenceModal>(
			new ModalParameters
			{
				{ nameof(GenerateFileByAccompanyingFileReferenceModal.FileType), selectedFileType },
				{ nameof(GenerateFileByAccompanyingFileReferenceModal.ModalTitle), modalTitle }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true });
	}
}