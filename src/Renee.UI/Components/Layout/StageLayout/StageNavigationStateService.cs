using Renee.Domain.Enums;

namespace Renee.UI.Components.Layout.StageLayout;

public class StageNavigationStateService : IStageNavigationStateService
{
	public Guid AssociatedResourceId { get; private set; }
	public AccompanyingFileStage CurrentStage { get; private set; }
	public AccompanyingFileStage? SelectedStage { get; private set; }

    public string AssociatedResourceReference { get; private set; } = string.Empty;

	public List<StageNavigationModel> StageNavigationModels { get; private set; } = [];
    public event EventHandler? MilestoneChanged;

	public string BuildUrl(AccompanyingFileStage stage)
	{
		var endpoint = StageNavigationModels.FirstOrDefault(x => x.Stage == stage)?.Url ?? string.Empty;
		return string.IsNullOrWhiteSpace(endpoint)
			? string.Empty
			: $"{endpoint}/{AssociatedResourceId}";
	}

	public List<StageNavigationModel> GetAccompanyingFileStageNavigation() =>
		StageNavigationModels.Where(x => x.Stage <= CurrentStage).ToList();

	public void LoadStageNavigationService(
		Guid accompanyingFileId,
		AccompanyingFileStage accompanyingFileStage,
		string accompaningFileReference,
        List<StageNavigationModel> stageNavigationModels)
	{
		AssociatedResourceId = accompanyingFileId;
		CurrentStage = accompanyingFileStage;
		AssociatedResourceReference = accompaningFileReference;
		SelectedStage ??= CurrentStage;
		StageNavigationModels = stageNavigationModels;

        SetCurrentPage();
	}

	public void SetSelectedStage(AccompanyingFileStage stage)
	{
		SelectedStage = stage > CurrentStage ? CurrentStage : stage;
		SetCurrentPage();
	}

	private void SetCurrentPage() => StageNavigationModels.ForEach(x => x.IsCurrentPage = x.Stage == SelectedStage);
	
	public void NotifyStateChanged() => MilestoneChanged?.Invoke(this, EventArgs.Empty);
}