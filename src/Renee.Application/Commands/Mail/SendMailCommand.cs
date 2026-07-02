using MediatR;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Mail;

public class MailParameters
{
	public string? Param1 { get; set; }
	public string? Param2 { get; set; }
	public string? Param3 { get; set; }
	public string? Param4 { get; set; }
	public string? Param5 { get; set; }
	public string? Param6 { get; set; }
	public string? Param7 { get; set; }
}

public sealed class SendMailCommand : IRequest<ReneeOperationResult<bool>>
{
	public MailType MailTypeEnum { get; }
	public List<string?>? Mails { get; }
	public string? Mail { get; }
	public MailParameters? MailParameters { get; }

	public SendMailCommand(
		MailType mailType,
		string? mail,
		MailParameters? mailParameters = null)
	{
		MailTypeEnum = mailType;
		Mail = mail;
		MailParameters = mailParameters;
	}

	public SendMailCommand(
		MailType mailType,
		List<string?> mails,
		MailParameters? mailParameters = null)
	{
		MailTypeEnum = mailType;
		Mails = mails;
		MailParameters = mailParameters;
	}
}