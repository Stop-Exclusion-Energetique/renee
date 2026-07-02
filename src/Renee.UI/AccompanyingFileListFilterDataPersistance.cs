using Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

namespace Renee.UI;

public class AccompanyingFileListFilterDataPersistance
{
    public CreatedAccompanyingFileByUsersFilterViewModel? CreatedAccompanyingFileByUsersFilter { get; set; }
    public bool FilterOnReportingStructure { get; set; }
    public bool FilterOnSolidarBuilder { get; set; }
    public bool FilterOnTerritories { get; set; }
    public int CurrentPageIndex { get; set; }
    public int PageSize { get; set; }
}