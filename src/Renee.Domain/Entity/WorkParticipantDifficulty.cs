namespace Renee.Domain.Entity;

public class WorkParticipantDifficulty
{
	public Guid WorkParticipantId { get; set; }
	public Guid DifficultyId { get; set; }

	public virtual WorkParticipant WorkParticipant { get; set; } = null!;
	public virtual SiteSupervisionDifficultyLabel Difficulty { get; set; } = null!;
}