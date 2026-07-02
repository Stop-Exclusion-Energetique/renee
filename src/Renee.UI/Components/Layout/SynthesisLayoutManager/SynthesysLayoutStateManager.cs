using Renee.Domain.Enums;

namespace Renee.UI.Components.Layout.SynthesisLayoutManager;

public class SynthesysLayoutStateManager
{
	public bool ShouldDisplayNavigationButton { get; set; } = true;
	public Guid AssociatedResourceId { get; set; }
	public string? AssociatedResourceReference { get; set; } = string.Empty;
	public AccompanyingFileStage AccompanyingFileStage { get; set; }

	public bool IsAccompanyingFile { get; set; } = true;

	public event EventHandler? OnSynthesysChanged;

	public void NotifyStateChanged() => OnSynthesysChanged?.Invoke(this, EventArgs.Empty);
}
