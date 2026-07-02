using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.ViewModel;

public class PreWorkPlanTabViewModel
{
	public double? KWhSavedPerYear
	{
		get
		{
			if (EstimatedAnnualEnergyConsumptionBeforeWork is null || EstimatedAnnualEnergyConsumptionAfterWork is null)
				return null;

			return EstimatedAnnualEnergyConsumptionBeforeWork - EstimatedAnnualEnergyConsumptionAfterWork;
		}
	}

	public double? EnergyGain
	{
		get
		{
			if (EstimatedAnnualEnergyConsumptionBeforeWork is null || EstimatedAnnualEnergyConsumptionAfterWork is null)
				return null;

			return Math.Round(
				(double)((EstimatedAnnualEnergyConsumptionBeforeWork - EstimatedAnnualEnergyConsumptionAfterWork) *
				         100 /
				         EstimatedAnnualEnergyConsumptionBeforeWork));
		}
	}

	public double? GhgEmissionsAvoidedPerYear
	{
		get
		{
			if (EstimatedAnnualGhgEmissionsAfterWork is null || EstimatedAnnualGhgEmissionsBeforeWork is null)
				return null;

			return EstimatedAnnualGhgEmissionsBeforeWork - EstimatedAnnualGhgEmissionsAfterWork;
		}
	}

	public double? RecommendedWorkTotalPrice => WorkPackages.Sum(wp => wp.TotalPrice);
	
	public List<Guid>? PreWorkPlanProjectTypes { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.RenovationType)]
	public RenovationType? RenovationType { get; set; }

	public string? NextStepAndVigilancePoints { get; set; }
	public bool? NeedTemporaryRehousingSolution { get; set; }
	public bool? InterestInPossibleSupportedSelfRehabilitationAra { get; set; }
	
	[ValidateComplexType]
	[WorkPackagesValidation]
	public List<WorkPackageViewModel> WorkPackages { get; init; } = [];

	public List<Guid>? PreWorkPlanInsuranceTypes { get; set; }
	public bool? AraOpeningStatementSent { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.EstimatedAnnualEnergyConsumptionAfterWork)]
	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; set; }
	public double? EstimatedAnnualEnergyConsumptionBeforeWork { get; init; }
	public double? EstimatedAnnualGhgEmissionsAfterWork { get; set; }
	public double? EstimatedAnnualGhgEmissionsBeforeWork { get; init; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.EstimatedDpeAfterWork)]
	public DpeLabel? EstimatedEnergyDpeAfterWork { get; set; }

	[Required(ErrorMessage = Labels.Errors.DpeMustBeSelected)]
	public DpeLabel? EstimatedEnergyDpeBeforeWork { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.EstimatedGesAfterWork)]
	public GesLabel? EstimatedEnergyGesAfterWork { get; set; }

	public int? EstimatedEnergyClassJump { get; set; }
	public bool? IsEmergencyWorks { get; set; }
	public bool? IsEnergeticsRenovationWorks { get; set; }
	public bool? IsInducedWorks { get; set; }
	public bool? IsSafetyAndHealthWorks { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.TreatedAirTightness)]
	public PartlyStateTreatment? TreatedAirTightness { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.TreatedThermalBridge)]
	public PartlyStateTreatment? TreatedThermalBridge { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.ExistingHumidityAndVaporMigrationManagedAfterTreatment)]
	public PartlyStateTreatment? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; set; }

	[Required(ErrorMessage = PreWorkPlanTabLabel.RequiredFields.IsRgeLabelUpToDate)]
	public bool? IsRgeLabelUpToDate { get; set; }

	public string? OtherQualification { get; set; }

	[AttributeUsage(AttributeTargets.Property)]
	public class WorkPackagesValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is not List<WorkPackageViewModel> list)
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

			var first = list[0];
			if (first.WorkTypes.Count == 0 || first.WorkTypes.Any(wt => wt.Price is null))
				return new ValidationResult(Labels.Errors.RequiredWorkPackage, [validationContext.MemberName!]);

			return ValidationResult.Success;
		}
	}
}