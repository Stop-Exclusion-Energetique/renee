using Microsoft.JSInterop;
using Renee.Domain;

namespace Renee.UI;

public class UnsavedChangesGuard(IJSRuntime js)
{
	private bool _hasUnsavedChanges;
	private string _currentUrl = string.Empty;
	private Func<Task<bool>>? _saveHandler;

	public void SetSaveHandler(Func<Task<bool>> saveHandler) =>
		_saveHandler = saveHandler ?? throw new ArgumentNullException(nameof(saveHandler));

	public async Task SetUnsavedChangesState(bool hasUnsavedChanges)
	{
		_hasUnsavedChanges = hasUnsavedChanges;

		if (js is null) return;

		if (hasUnsavedChanges)
			await js.InvokeVoidAsync("unsavedChangesGuard.enable");
		else
			await js.InvokeVoidAsync("unsavedChangesGuard.disable");
	}

	public void UpdateCurrentUrl(string absoluteUrl) => _currentUrl = absoluteUrl;

	public bool ShouldIntercept(string targetUrl) =>
		_hasUnsavedChanges && IsLeavingMilestone(targetUrl);

	public async Task<bool> SaveAsync()
	{
		if (_saveHandler is null)
			return false;

		return await _saveHandler();
	}

	private static bool IsMilestoneUrl(string url) =>
		!string.IsNullOrWhiteSpace(url) &&
		new List<string>
		{
			Endpoints.NewOccupant,
			Endpoints.OrganizeAndFinanceStage,
			Endpoints.RealiseAndFollowStage
		}.Any(e => url.Contains(e, StringComparison.OrdinalIgnoreCase));

	private bool IsLeavingMilestone(string targetUrl) =>
	   IsMilestoneUrl(_currentUrl) && _currentUrl != targetUrl;
}