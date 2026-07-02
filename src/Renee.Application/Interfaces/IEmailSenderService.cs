namespace Renee.Application.Interfaces;

public interface IEmailSenderService
{
	Task<bool> SendEmail(string? mailTo, string subject, string htmlContent);
	Task<bool> SendEmail(List<string?>? mailsTo, string subject, string htmlContent);
}