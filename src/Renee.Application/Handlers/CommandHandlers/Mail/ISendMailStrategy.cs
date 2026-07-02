using Renee.Application.Commands.Mail;
using Renee.Domain.Enums;

namespace Renee.Application.Handlers.CommandHandlers.Mail;

public interface ISendMailStrategy
{
    bool CanHandle(MailType type);
    Task<bool> SendAsync(SendMailCommand request, CancellationToken cancellationToken);
}
