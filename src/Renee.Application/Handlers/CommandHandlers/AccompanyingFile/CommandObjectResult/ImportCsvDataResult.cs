using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain.Enums;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;

public class ImportCsvDataCommandResult(
	ImportCsvResultStatus importCsvResultStatus,
	List<string>? successMessages, 
	List<LineErrorReport>? errorMessages)
{
	public ImportCsvResultStatus ImportCsvResultStatus { get; } = importCsvResultStatus;
	public List<string>? SuccessMessages { get; } = successMessages;
	public List<LineErrorReport>? ErrorMessages { get; } = errorMessages;

    public static ImportCsvDataCommandResult Success(List<string>? successMessages) => 
        new(ImportCsvResultStatus.Success, successMessages, null);

	public static ImportCsvDataCommandResult PartialSuccess(List<string>? successMessages, List<LineErrorReport>? errors) =>
		new(ImportCsvResultStatus.PartialSuccess, successMessages, errors);

	public static ImportCsvDataCommandResult Failure(List<LineErrorReport>? errors) => 
        new(ImportCsvResultStatus.Failure, null, errors);
}