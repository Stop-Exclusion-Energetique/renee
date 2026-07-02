using Renee.Application.Commands.Mail;
using Renee.Application.Interfaces;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Mail.MailStrategy;

public class TaskCompletedMailStrategy(IEmailSenderService emailSenderService, IEmailRepository emailRepository) : ISendMailStrategy
{
	public bool CanHandle(MailType type) => type == MailType.TaskCompleted;

	public async Task<bool> SendAsync(SendMailCommand request, CancellationToken cancellationToken)
	{
		var mail = await emailRepository.GetEmailByType(request.MailTypeEnum);

		if (mail is not null && !string.IsNullOrEmpty(mail.Subject.Trim()) && !string.IsNullOrEmpty(mail.HtmlContent.Trim()))
		{
			return await emailSenderService.SendEmail(
				request.Mail,
				string.Format(mail.Subject, request.MailParameters?.Param1),
				string.Format(
					mail.HtmlContent,
					request.MailParameters?.Param1,
					request.MailParameters?.Param2));
		}

		return false;
	}
}
