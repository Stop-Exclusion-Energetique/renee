using System.ComponentModel.DataAnnotations;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.Evaluations.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectCost.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectEnd.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.WorkSummary.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.ViewModel;

public class RealiseAndFollowViewModel
{
	public string? Reference { get; set; }
	public AccompanyingFileStatus? AccompanyingFileStatus { get; init; }
	public bool IsInTzeeProgram { get; set; }
	public bool? IsImported { get; set; }
	public string? ReportingStructureName { get; set; }
	[ValidateComplexType] public ProjectCostViewModel ProjectCostViewModel { get; set; } = new();

	[ValidateComplexType] public WorkSummaryViewModel WorkSummaryViewModel { get; set; } = new();

	[ValidateComplexType] public EvaluationsViewModel EvaluationsViewModel { get; set; } = new();

	[ValidateComplexType] public ProjectEndViewModel ProjectEndViewModel { get; set; } = new();

	[ValidateComplexType] public PreFinancingPlanViewModel FinalFinancingPlanViewModel { get; set; } = new();

	[ValidateComplexType] public SiteSupervisionViewModel SiteSupervisionViewModel { get; set; } = new();
}