using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Synthesis.Modal.ViewModel;

public class OrganizeAndFinanceSynthesisValidationModalViewModel
{

	public bool IsAccompanyingFileValidatedForCeeProgram 
	{ 
		get
		{
			return ((string.Equals(AnahCategory, Labels.LowIncomeHouseholdsAmount) || string.Equals(AnahCategory, Labels.VeryLowIncomeHouseholdsAmount)) && IsDpeLabelOrClimateLabelValid() && int.Parse(EnergyClassJump!) >= 2) ||
					((UserRole == Constants.DiffuseCoordinatorRole ||
						UserRole == Constants.TargetedCoordinatorRole ||
						UserRole == Constants.TerritorialBuilderRole ||
						UserRole == Constants.AdminRole) &&
                    isAccompanyingFileValidatedForCeeProgram);
		}

		set { isAccompanyingFileValidatedForCeeProgram = value; } 
	}

	private bool isAccompanyingFileValidatedForCeeProgram;

	public Guid AccompanyingFileId { get; init; }

	public string? AnahCategory { get; set; }
	public string? OwnershipStatus { get; set; }
	public string DpeLabel { get; set; } = null!;
	public string GesLabel { get; set; } = null!;
	public string? EnergyClassJump { get; init; }
	public string? UserRole { get; set; }
	public bool? IsUserAwareOfNoPossibilitiesToUpdateAccompanyingFile { get; set; }
	public bool? IsInTzeeProgram { get; set; }

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