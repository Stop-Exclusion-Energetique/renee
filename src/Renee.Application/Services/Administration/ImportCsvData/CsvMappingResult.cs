using Renee.Application.CommandsUseCasesInput;

namespace Renee.Application.Services.Administration.ImportCsvData;

public record CsvMappingResult(
	string? OperatorName,
	List<ImportAccompanyingFileFromCsvCommandInput> AccompanyingFilesToCreate,
	List<UpdateAccompanyingFileFromCsvFileInput> AccompanyingFilesToUpdate);