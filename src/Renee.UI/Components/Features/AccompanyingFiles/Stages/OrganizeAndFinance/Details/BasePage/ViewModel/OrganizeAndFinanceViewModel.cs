using System.ComponentModel.DataAnnotations;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.HousingInitialState.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.SupportedSelfRehabilitation.
	ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.ViewModel;

public class OrganizeAndFinanceViewModel
{
	public string? Reference { get; init; }

	public bool IsInTzeeProgram { get; init; }

	public AccompanyingFileStage? AccompanyingFileStage { get; init; }
	public AccompanyingFileStatus? AccompanyingFileStatus { get; init; }

	public bool? IsImported { get; init; }
	public string? ReportingStructureName { get; init; }
	public DateTime? StartOfAccompanyingDate { get; init; }

	[ValidateComplexType] public InitialHousingStateViewModel InitialHousingStateViewModel { get; init; } = new();

	[ValidateComplexType] public PreFinancingPlanViewModel PreFinancingPlanViewModel { get; init; } = new();

	[ValidateComplexType] public PreWorkPlanTabViewModel PreWorkPlanTabViewModel { get; init; } = new();

	[ValidateComplexType]
	public SupportedSelfRehabilitationViewModel SupportedSelfRehabilitationViewModel { get; init; } = new();
}