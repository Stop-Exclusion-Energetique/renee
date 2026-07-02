using Renee.Application.Commands.Mail;
using Renee.Application.Interfaces;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Mail.MailStrategy;

public class StageRefusedMailStrategy(IEmailSenderService emailSenderService, IEmailRepository emailRepository) : ISendMailStrategy
{
    public bool CanHandle(MailType type) => type == MailType.StageRefused;

    public async Task<bool> SendAsync(SendMailCommand request, CancellationToken cancellationToken)
    {
        var mail = await emailRepository.GetEmailByType(request.MailTypeEnum);

        if (mail is not null && !string.IsNullOrEmpty(mail.Subject.Trim()) && !string.IsNullOrEmpty(mail.HtmlContent.Trim()))
        {
            return await emailSenderService.SendEmail(
					request.Mails,
					string.Format(mail.Subject, request.MailParameters?.Param1),
					string.Format(
						mail.HtmlContent,
						request.MailParameters?.Param2,
						request.MailParameters?.Param3,
						request.MailParameters?.Param4,
						request.MailParameters?.Param5,
						request.MailParameters?.Param7,
						request.MailParameters?.Param6));
        }

        return false;
    }
}
