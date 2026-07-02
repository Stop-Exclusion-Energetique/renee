namespace Renee.Application.DTOs.CguVersion;

public sealed class UserVersionCguDto
{
    public string? LastValidatedCGUVersionByUser { get; set; }
    public DateTime? LastValidatedCGUDateByUser { get; set; }
    public string? LastCGUVersion { get; set; }
    public string? LastCGULabel { get; set; }
}
