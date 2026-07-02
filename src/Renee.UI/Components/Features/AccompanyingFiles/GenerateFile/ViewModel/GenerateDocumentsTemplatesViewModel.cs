using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.GenerateFile.ViewModel;

public class GenerateDocumentsTemplatesViewModel
{
	[Required(ErrorMessage = GenerateFilesLabels.Errors.FileNotSelected)]
	public Guid? SelectedDocument { get; set; }
}