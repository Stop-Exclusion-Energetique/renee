using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class EmailRepository(ReneeDbContext dbContext) : IEmailRepository
{
	public async Task<Email?> GetEmailByType(MailType mailType) =>
		await dbContext.Emails.FirstOrDefaultAsync(x => x.Type == (int)mailType);
}