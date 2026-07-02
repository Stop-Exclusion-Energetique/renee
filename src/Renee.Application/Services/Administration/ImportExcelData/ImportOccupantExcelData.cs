namespace Renee.Application.Services.Administration.ImportExcelData;

public record ImportOccupantExcelData(string Trigram, string DateOfBirth, string? SocioprefessionalCategory)
{
	public string Trigram { get; } = Trigram;
	public string DateOfBirth { get; } = DateOfBirth;
	public string? SocioprofessionalCategory { get; } = SocioprefessionalCategory;
}