using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.Presenter;

public class WorkPackagePresenter(
	WorkPackageViewModel viewModel,
	List<ZeeSelectItem<Guid>> workTypes,
	List<Guid> selectedWorkTypesIds)
{
	public WorkPackageViewModel Present()
	{
		if (selectedWorkTypesIds == null)
		{
			viewModel.WorkTypes.Clear();
			return viewModel;
		}

		var selectedWorkTypes = workTypes.Where(wt => selectedWorkTypesIds.Contains(wt.Value)).ToList();
		AddWorktypeToViewModelWorkTypeList(selectedWorkTypes);
		DeleteWorktypeFromViewModelWorkTypeList();

		return viewModel;
	}

	private void AddWorktypeToViewModelWorkTypeList(List<ZeeSelectItem<Guid>> selectedWorkType)
	{
		var workTypesToAdd = selectedWorkType.Where(swt => viewModel.WorkTypes.TrueForAll(wt => wt.Id != swt.Value))
			.Select(swt => new WorkPackageViewModel.WorkType(swt.Value, swt.Label!, viewModel.IsRequired)).ToList();
		foreach (var workType in workTypesToAdd) viewModel.WorkTypes.Add(workType);
	}

	private void DeleteWorktypeFromViewModelWorkTypeList()
	{
		var workTypesToDelete = viewModel.WorkTypes.Where(wt => !selectedWorkTypesIds.Contains(wt.Id)).ToList();
		foreach (var workType in workTypesToDelete) viewModel.WorkTypes.Remove(workType);
	}
}