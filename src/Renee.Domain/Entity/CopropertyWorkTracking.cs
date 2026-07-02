namespace Renee.Domain.Entity;

public class CopropertyWorkTracking
{
    public Guid Id { get; set; }
    public DateTime? CollectiveWorksStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public double? ProgressPercentage { get; set; }
    public DateTime? ActualCompletionDate { get; set; }
    public double? InvoiceTotalAmount { get; set; }
    public string? FollowUpComment { get; set; }

    public virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();
}
