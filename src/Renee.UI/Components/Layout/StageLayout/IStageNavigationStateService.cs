using Renee.Domain.Enums;

namespace Renee.UI.Components.Layout.StageLayout;

public interface IStageNavigationStateService
{
	Guid AssociatedResourceId { get; }
	AccompanyingFileStage CurrentStage { get; }
	AccompanyingFileStage? SelectedStage { get; }
	string AssociatedResourceReference { get; }
	List<StageNavigationModel> StageNavigationModels { get; }
	event EventHandler? MilestoneChanged;

	string BuildUrl(AccompanyingFileStage stage);

	List<StageNavigationModel> GetAccompanyingFileStageNavigation();

	void SetSelectedStage(AccompanyingFileStage stage);

	void LoadStageNavigationService(
		Guid accompanyingFileId,
		AccompanyingFileStage accompanyingFileStage,
		string accompaningFileReference,
        List<StageNavigationModel> StageNavigationModels);

	void NotifyStateChanged();
}