namespace Renee.Domain.Entity;

public class AdminConstant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Value { get; set; } = 0;

    public DateTime? AccompanyingFileModificationDeadline { get; set; }
    public string? AccompanyingFileAlertBannerMessage { get; set; }
}