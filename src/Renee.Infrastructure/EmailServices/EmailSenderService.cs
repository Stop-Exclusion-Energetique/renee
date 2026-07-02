using brevo_csharp.Api;
using brevo_csharp.Client;
using brevo_csharp.Model;
using Microsoft.Extensions.Configuration;
using Renee.Application.Interfaces;

namespace Renee.Infrastructure.EmailServices;

public sealed class EmailSenderService(IConfiguration configuration) : IEmailSenderService
{
	public async Task<bool> SendEmail(string? mailTo, string subject, string htmlContent)
	{
		var (ApiKey, Sender, SenderName, FakeEmail) = GetBrevoConfiguration();
		Configuration.Default.ApiKey.TryAdd("api-key", ApiKey);
		var api = new TransactionalEmailsApi();

		await api.SendTransacEmailAsync(
			new SendSmtpEmail
			{
				To = [new SendSmtpEmailTo(!string.IsNullOrWhiteSpace(FakeEmail) ? FakeEmail : mailTo)],
				Subject = subject,
				HtmlContent = htmlContent,
				Sender = new SendSmtpEmailSender(SenderName, Sender)
			});
		return true;
	}

	public async Task<bool> SendEmail(List<string?>? mailsTo, string subject, string htmlContent)
	{
		var (ApiKey, Sender, SenderName, FakeEmail) = GetBrevoConfiguration();
		Configuration.Default.ApiKey.TryAdd("api-key", ApiKey);
		var api = new TransactionalEmailsApi();
		var mails = new List<SendSmtpEmailTo>();

		if (mailsTo is not null && mailsTo.Count > 0)
		{
			mails.AddRange(mailsTo.Select(mail => new SendSmtpEmailTo(mail)));
		}

		await api.SendTransacEmailAsync(
			new SendSmtpEmail
			{
				To = !string.IsNullOrWhiteSpace(FakeEmail) ? [new SendSmtpEmailTo(FakeEmail)] : mails,
				Subject = subject,
				HtmlContent = htmlContent,
				Sender = new SendSmtpEmailSender(SenderName, Sender)
			});
		return true;
	}

	private (string ApiKey, string Sender, string SenderName, string? FakeEmail) GetBrevoConfiguration()
	{
		return (
			configuration.GetValue<string>("Brevo:APIKey")!,
			configuration.GetValue<string>("Brevo:APISender")!,
			configuration.GetValue<string>("Brevo:APISenderName")!,
			configuration.GetValue<string>("Brevo:FakeUserEmail")
		);
	}
}