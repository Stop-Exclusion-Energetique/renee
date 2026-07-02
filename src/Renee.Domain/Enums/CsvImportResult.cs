using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum ImportCsvResultStatus
{
	[Description(CsvImportResultLabels.Success)]
	Success,
	[Description(CsvImportResultLabels.PartialSuccess)]
	PartialSuccess,
	[Description(CsvImportResultLabels.Failure)]
	Failure
}