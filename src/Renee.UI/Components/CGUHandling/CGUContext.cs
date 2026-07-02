namespace Renee.UI.Components.CGUHandling;

public class CguContext
{
    public bool IsInitialized { get; set; } = false;
    public string? LatestVersion { get; set; }
    public string? UserVersion { get; set; }
    public string? LatestVersionLabelFile { get; set; } = null;
    public Guid UserId { get; set; }
    public bool ShouldShowModal => LatestVersion != null && UserVersion != LatestVersion;
}