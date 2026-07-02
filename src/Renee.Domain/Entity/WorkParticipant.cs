namespace Renee.Domain.Entity;

public class WorkParticipant
{
	public Guid Id { get; set; }

	public Guid SiteSupervisionId { get; set; }

	public int? ParticipantType { get; set; }

	public string? WorkTypesLabel { get; set; }

	public string? ParticipantName { get; set; } = string.Empty;

	public string? ContactAdvisor { get; set; }

	public DateTime? StartDateOfWork { get; set; }

	public DateTime? EstimatedCompletionDate { get; set; }

	public DateTime? ActualEndDate { get; set; }

	public double? Progress { get; set; }

	public int? WorkQuality { get; set; }

	public string? CommentOnWorkQuality { get; set; }

	public string? CommentOnWorkParticipantDifficulties { get; set; }

	public string? SpecificComments { get; set; }

	public SiteSupervision SiteSupervision { get; set; } = null!;

	public virtual ICollection<WorkParticipantDifficulty> WorkParticipantDifficulties { get; set; } = [];
}