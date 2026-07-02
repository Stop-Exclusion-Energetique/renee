using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Services.Administration.ImportExcelData;

namespace Renee.Application.Interfaces;

public interface IImportExcelDataService
{
	Task<ImportAccompanyingFileFromFileInputCommandInput?> MapExcelData(
		List<ExcelData> data,
		RequiredFieldsFromDataImportPage requiredFieldsFromDataImportPage);

	List<ExcelData> ProcessExcelFile(Stream fileStream);
	ImportExcelDataCommandResult ValidateData(List<ExcelData> data, bool isV3);
}