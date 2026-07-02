using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Services.Administration.ImportCsvData;

namespace Renee.Application.Interfaces;

public interface IImportCsvDataService
{
    List<ParsedCsvData> ProcessCsvFile(Stream fileStream, out List<LineErrorReport> lineErrorsReporting);
	Task<CsvMappingResult> MapCsvData(List<ParsedCsvData> dataList);
}