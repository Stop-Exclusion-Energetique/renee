using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.GenerateFile.Modal.ViewModel;

public class GenerateFileByAccompanyingFileReferenceViewModel
{
	[Required(ErrorMessage = GenerateFilesLabels.Errors.RequiredAccompanyingFileReference)]
	public string Reference { get; set; } = string.Empty;
}