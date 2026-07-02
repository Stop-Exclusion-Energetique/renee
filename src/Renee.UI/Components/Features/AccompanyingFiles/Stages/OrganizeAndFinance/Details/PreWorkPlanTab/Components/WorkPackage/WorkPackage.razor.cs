using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage;

public partial class WorkPackage
{
	[Parameter] public List<Guid> SelectedWorkTypesIds { get; set; } = [];

	[Parameter] public WorkPackageViewModel ViewModel { get; set; } = new();

	[Parameter] public List<ZeeSelectItem<Guid>> WorkTypes { get; set; } = [];

	[Parameter] public int Index { get; set; }

	[Parameter] public EditContext EditContext { get; set; } = null!;

	[Parameter] public bool IsUserAllowedToEdit { get; set; }

	[Parameter] public EventCallback SelectedWorkTypesIdsChanged { get; set; }

	protected override void OnParametersSet()
	{
		ViewModel.IsRequired = Index == 1;
	}

	private void OnSelectedWorkTypeChanged()
	{
		ViewModel = new WorkPackagePresenter(ViewModel, WorkTypes, SelectedWorkTypesIds).Present();

		if (SelectedWorkTypesIdsChanged.HasDelegate)
			SelectedWorkTypesIdsChanged.InvokeAsync();

		StateHasChanged();
	}
}