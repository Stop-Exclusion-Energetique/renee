namespace Renee.Application.DTOs.CguVersion;

public sealed class CguVersionDto
{
    public Guid Id { get; set; }
    public string? Label { get; set; }
    public string? Version { get; set; }
    public DateTime CreatedDate { get; set; }
}
