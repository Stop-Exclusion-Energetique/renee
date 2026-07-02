namespace Renee.Domain.Entity;

public class SiteSupervision
{
	public Guid Id { get; set; }

	public DateTime? OverallStartDate { get; set; }

	public DateTime? EstimatedOverallCompletionDate { get; set; }

	public DateTime? ActualOverallEndDate { get; set; }

	public double? OverallProgress { get; set; }

	public DateTime? NextCoordinationMeetingScheduledFor { get; set; }

	public string? OverallObservations { get; set; }

	public DateTime? PreSiteSupervisionMeetingDate { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = [];

	public virtual ICollection<WorkParticipant> WorkParticipants { get; set; } = [];
}