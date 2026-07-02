namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;

public class ImportExcelDataCommandResult(bool isSuccess, List<string>? errors)
{
	public bool IsSuccess { get; private set; } = isSuccess;
	public List<string>? Errors { get; private set; } = errors;
	public static ImportExcelDataCommandResult Failure(List<string> errors) => new(false, errors);

	public static ImportExcelDataCommandResult Success() => new(true, null);
}