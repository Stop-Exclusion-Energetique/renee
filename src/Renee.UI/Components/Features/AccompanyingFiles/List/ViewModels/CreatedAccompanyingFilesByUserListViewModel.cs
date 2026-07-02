namespace Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

public class CreatedAccompanyingFilesByUserListViewModel
{
	public List<CreatedAccompanyingFilesByUserViewModel> AccompanyingFiles { get; init; } = [];
	public int TotalCount { get; init; }
}