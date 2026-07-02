namespace Renee.Domain.Entity;

public class SiteSupervisionDifficultyLabel
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public virtual ICollection<WorkParticipantDifficulty> WorkParticipantDifficulties { get; set; } = [];
}