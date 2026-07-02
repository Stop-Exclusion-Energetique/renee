using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Domain.Repositories;

public interface IEmailRepository
{
	Task<Email?> GetEmailByType(MailType mailType);
}