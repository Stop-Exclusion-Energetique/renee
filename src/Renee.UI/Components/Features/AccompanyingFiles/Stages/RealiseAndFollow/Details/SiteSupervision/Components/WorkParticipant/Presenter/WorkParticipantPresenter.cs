using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.Presenter;

public class WorkParticipantPresenter(
	WorkParticipantViewModel viewModel,
	List<ZeeSelectItem<Guid?>> workParticipantsDifficulties,
	List<Guid?> selectedWorkParticipantsDifficultiesIds)
{
	public WorkParticipantViewModel Present()
	{
		if (selectedWorkParticipantsDifficultiesIds == null)
		{
			viewModel.WorkParticipantDifficulties.Clear();
			return viewModel;
		}

		var selectedWorkParticipants = workParticipantsDifficulties.Where(wt => wt.Value is Guid id && selectedWorkParticipantsDifficultiesIds.Contains(id)).ToList();
		AddWorkParticipantDifficulties(selectedWorkParticipants);
		DeleteWorkParticipantDifficulties();

		return viewModel;
	}

	private void AddWorkParticipantDifficulties(List<ZeeSelectItem<Guid?>> selectedWorkParticipants)
	{
		var workParticipantsDifficultiesToAdd = selectedWorkParticipants.Where(swpd => viewModel.WorkParticipantDifficulties.TrueForAll(difficulty => difficulty != swpd.Value))
			.Select(swpd => swpd.Value)
			.ToList();
		foreach (var difficulty in workParticipantsDifficultiesToAdd) viewModel.WorkParticipantDifficulties.Add(difficulty);
	}

	private void DeleteWorkParticipantDifficulties()
	{
		var workParticipantDifficultiesToDelete = viewModel.WorkParticipantDifficulties.Where(d => d is Guid id && !selectedWorkParticipantsDifficultiesIds.Contains(id)).ToList();
		foreach (var difficulty in workParticipantDifficultiesToDelete) viewModel.WorkParticipantDifficulties.Remove(difficulty);
	}
}