using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension.FilterOptions;

public record AccompanyingFilesListFilterOptions(
	List<AccompanyingFileStage> AccompanyingFileStages,
	List<AccompanyingFileStatus> AccompanyingFileStatuses,
	List<AccompanyingFileNeedingBilling> AccompanyingFileNeedingBillings,
	string? FilterValue,
	SortingState SortingState,
	FilterContext FilterContext,
	List<Guid?>? ReportingStructures = null,
	List<Guid?>? SolidarBuilders = null,
	List<Guid?>? Territories = null);