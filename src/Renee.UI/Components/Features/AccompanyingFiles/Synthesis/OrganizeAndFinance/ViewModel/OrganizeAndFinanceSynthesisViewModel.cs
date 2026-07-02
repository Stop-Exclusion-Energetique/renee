using System.ComponentModel.DataAnnotations;
using Renee.Application.DTOs.WorkPackage;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.ViewModel;

public class OrganizeAndFinanceSynthesisViewModel
{
	public Guid Id { get; init; }
	public string AccompanyingFileReference { get; set; } = null!;
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }

	public string? PreWorkPlanProjectType { get; init; }
	public string? RenovationType { get; init; }
	public string? NextStepAndVigilancePoints { get; init; }
	public string? IsEmergencyWorks { get; init; }
	public string? TreatedAirTightness { get; init; }
	public string? TreatedThermalBridge { get; init; }
	public string? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; init; }

	public List<WorkPackageSummaryDto> WorkPackageSummary { get; init; } = [];
	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; init; }
	public string? EstimatedEnergyDpeAfterWork { get; init; }
	public string? EstimatedEnergyClassJump { get; init; }

	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? DiffuseCoordinator { get; set; }
	public Guid? TargetedCoordinator { get; set; }
	public Guid? TerritorialBuilder { get; set; }
	public Guid? SecondTerritorialBuilder { get; set; }

	public string InitialDpe { get; set; } = string.Empty;

	public string? AnahFolderNumber { get; set; }
	public DateTime? AnahFolderFilingDate { get; set; }

	[ValidateComplexType] public PreFinancingPlanViewModel PreFinancingPlanViewModel { get; init; } = new();

	public double WorkPackagesTotalPrice =>
		WorkPackageSummary?.Sum(wp => wp.WorkPackageTotalCost) ?? 0;
	
	public double AbsoluteValueGapBetweenWorkPackagesAndPreFinancingPlan =>
		Math.Round(Math.Abs(WorkPackagesTotalPrice - PreFinancingPlanViewModel.PreFinancingPlanTotal));
}