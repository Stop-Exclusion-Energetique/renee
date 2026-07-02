namespace Renee.UI;

public class NavigationHistoryManager
{
	public string PreviousUri { get; set; } = string.Empty;
	public string CurrentUri { get; set; } = string.Empty ;

	public void Update(string newUri)
	{
		PreviousUri = CurrentUri;
		CurrentUri = newUri;
	}
}
