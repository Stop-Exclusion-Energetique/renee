using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal.ViewModel;

public class IdentifySynthesisValidationModalViewModel
{
	public Guid AccompanyingFileId { get; init; }

	public bool IsAccompanyingFileValidatedForCeeProgram
	{
		get
		{
			return 	((string.Equals(AnahCategory, Labels.LowIncomeHouseholdsAmount) || string.Equals(AnahCategory, Labels.VeryLowIncomeHouseholdsAmount)) && IsDpeLabelOrClimateLabelValid()) ||
					((UserRole == Constants.DiffuseCoordinatorRole || UserRole == Constants.TargetedCoordinatorRole || UserRole == Constants.TerritorialBuilderRole || UserRole == Constants.AdminRole) && isAccompanyingFileValidatedForCeeProgram);
		}
		set { isAccompanyingFileValidatedForCeeProgram = value; }
	}

	public string AnahCategory { get; set; } = string.Empty;
	public string OwnershipStatus { get; set; } = string.Empty;
	public string DpeLabel { get; set; } = string.Empty;
	public string GesLabel { get; set; } = string.Empty;
	public string UserRole { get; set; } = string.Empty;
	public bool? IsInTzeeProgram { get; set; }

	public bool? IsUserAwareOfNoPossibilitiesToUpdateAccompanyingFile { get; set; }

	private bool isAccompanyingFileValidatedForCeeProgram;

	public int IsDpeLowerThanGes() => string.Compare(DpeLabel, GesLabel, StringComparison.Ordinal);

	private bool IsDpeLabelOrClimateLabelValid() =>
		DpeLabel switch
		{
			Labels.DpeE or Labels.DpeF or Labels.DpeG => true,
			_ => false
		} ||
		GesLabel switch
		{
			Labels.GesE or Labels.GesF or Labels.GesG => true,
			_ => false
		};
}