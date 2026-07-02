using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.WorkSummary.ViewModel;

public class WorkSummaryViewModel
{
	public string? IntermediateAirtightnessTestResult { get; set; }
	public string? WaterproofingTreatmentActions { get; set; }
	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredEffectiveComplianceWithWorkRecommendations)]
	public bool? HasEffectiveComplianceWithWorkRecommendations { get; set; }
	public bool? HasWorkEnablingHomeSupport { get; set; }
	public bool? HasHousingAdaptationWorks { get; set; }
	public bool? HasFinishingWorks { get; set; }
	public bool? HasSafetyWorks { get; set; }
	public bool? HasPreparationWorks { get; set; }
	public bool? HasEmergencyWorks { get; set; }
	public bool? HasUnsanitaryExit { get; set; }
	public PartlyStateTreatment? TreatedAirTightness { get; set; }
	public PartlyStateTreatment? TreatedThermalBridges { get; set; }
	public bool? HasHumidityManagement { get; set; }
	public bool? ShouldChangeFinalEstimatedDpe { get; set; }
	public DpeLabel? InitialDpe { get; set; }
	public DpeLabel? EstimatedDpe { get; set; }
	[FinalDpeValidation]
	public DpeLabel? FinalDpe { get; set; }
	public int? FinalDpeClassJump { get; set; }

	[AttributeUsage(AttributeTargets.Property)]
	public class FinalDpeValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var viewModel = (WorkSummaryViewModel)validationContext.ObjectInstance;

			if (viewModel.ShouldChangeFinalEstimatedDpe == false)
				return ValidationResult.Success;

			if (viewModel.ShouldChangeFinalEstimatedDpe == true &&
				viewModel.FinalDpe == viewModel.EstimatedDpe)
			{
				return new ValidationResult(
					RealiseAndFollowMilestone.Errors.FinalDpeValidationError,
					[validationContext.MemberName!]);
			}

			return ValidationResult.Success;
		}
	}
}