using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Documents;

public partial class AccompanyingFileDocuments
{
	[Parameter] public IReadOnlyList<DocumentViewModel> Documents { get; set; } = [];

	[Parameter] public EventCallback<(string FileName, byte[]? Data)> OnViewFile { get; set; }

	[Parameter] public EventCallback<InputFileChangeEventArgs> OnUpload { get; set; }

	[Parameter] public EventCallback<string> OnClickDeleteFile { get; set; }

	[Parameter] public IReadOnlyList<string>? ErrorMessages { get; set; }

	private static string BuildFileLoadedMessage(DocumentViewModel document)
	{
		var date = document.UploadedAt?.ToString("dd/MM/yyyy");
		var author = string.IsNullOrWhiteSpace(document.Author)
			? ""
			: $" par <strong>{document.Author}</strong>";

		return string.Format(Labels.FileLoadedMessageAccompanyingFileMenu, document.Name, date, author);
	}
}