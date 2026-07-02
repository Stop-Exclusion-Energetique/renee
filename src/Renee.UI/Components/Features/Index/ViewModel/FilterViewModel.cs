using Renee.UI.Components.Features.Index.Presenter;

namespace Renee.UI.Components.Features.Index.ViewModel;

public class FilterViewModel
{
	public FilterValue FilterValue { get; set; } = FilterValue.MyAccompanyingFile;
	public DateTime? FromDate { get; set; }
	public DateTime? ToDate { get; set; }
	public List<Guid?> SelectedReportingStructures { get; set; } = [];

	public List<Guid?> SelectedSolidarBuilder { get; set; } = [];
	public List<Guid?> SelectedTerritories { get; set; } = [];

	public void Reset()
	{
		FromDate = null;
		ToDate = null;
	}
}