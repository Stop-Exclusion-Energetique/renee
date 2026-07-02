namespace Renee.Application.Services.Administration.ImportCsvData;

public class LineErrorReport
{
	public int? LineNumber { get; }
	public int? LineId { get; set; }
	public readonly List<string> _errors = [];

	private LineErrorReport(int? lineNumber) => LineNumber = lineNumber;

	public static LineErrorReport Create(int? lineNumber, string errorMessage)
	{
		var report = new LineErrorReport(lineNumber);
		report.AddErrors(errorMessage);
		return report;
	}

	public static LineErrorReport Create(int? lineNumber, List<string> errors)
	{
		var report = new LineErrorReport(lineNumber);
		report.AddErrors([..errors]);
		return report;
	}

	public void AddErrors(params string[] messages) => _errors.AddRange(messages);
	public List<string> GetErrors() => _errors;
}