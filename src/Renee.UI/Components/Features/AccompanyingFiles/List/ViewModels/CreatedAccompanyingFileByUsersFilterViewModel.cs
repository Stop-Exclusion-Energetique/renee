using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

public class CreatedAccompanyingFileByUsersFilterViewModel
{
	public SortingState SortingState { get; set; } = SortingState.None;
	public string? FilterValue { get; set; } = string.Empty;
	public FilterContext FilterContext { get; set; } = FilterContext.MyAccompanyingFile;
	public List<Guid?> ReportingStructureIdList { get; set; } = [];
	public List<Guid?> SolidarBuildersList { get; set; } = [];
	public List<Guid?> Territories { get; set; } = [];
	public List<AccompanyingFileStage> AccompanyingFileStages { get; set; } = [];
	public List<AccompanyingFileStatus> AccompanyingFileStatuses { get; set; } = [];
	public List<AccompanyingFileNeedingBilling> AccompanyingFileNeedingBillings { get; set; } = [];
}