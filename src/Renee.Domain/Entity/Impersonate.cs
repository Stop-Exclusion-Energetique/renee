namespace Renee.Domain.Entity;

public class Impersonate
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public Guid ImpersonateUserId { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? EndedAt { get; set; }
}