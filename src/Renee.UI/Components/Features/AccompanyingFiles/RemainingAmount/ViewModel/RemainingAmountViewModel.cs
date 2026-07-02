using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.RemainingAmount.ViewModel;

public class RemainingAmountViewModel
{
	[Required(ErrorMessage = GenerateFilesLabels.Errors.RequiredAccompanyingFileReference)]
	public string? Reference { get; set; }
	[Required(ErrorMessage = Labels.Errors.RequiredRemainingAmount)]
	public double? EstimatedRemainingAmount { get; set; }

	public void CleanViewModel()
	{
		Reference = string.Empty;
		EstimatedRemainingAmount = null;
	}
}
