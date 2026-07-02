namespace Renee.Domain.Entity;

public class Email
{
	public Guid Id { get; set; }

	public int Type { get; set; }

	public string MailName { get; set; } = null!;

	public string Subject { get; set; } = null!;

	public string HtmlContent { get; set; } = null!;
}