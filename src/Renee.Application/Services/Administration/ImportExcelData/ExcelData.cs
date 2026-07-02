namespace Renee.Application.Services.Administration.ImportExcelData;

public record ExcelData(string Header, string? Value)
{
	public string Header { get; } = Header;
	public string? Value { get; } = Value;
}