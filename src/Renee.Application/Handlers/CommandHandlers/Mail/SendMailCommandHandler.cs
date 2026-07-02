using MediatR;
using Renee.Application.Commands.Mail;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Mail;

public class SendMailCommandHandler(
	IEnumerable<ISendMailStrategy> _strategies,
	ITelemetryService telemetryService)
	: IRequestHandler<SendMailCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(SendMailCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request.MailTypeEnum));
			if (strategy == null)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.MailTypeNotFound);

			var result = await strategy.SendAsync(request, cancellationToken);
			return result
				? ReneeOperationResult<bool>.Success(true)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileSendingMail);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}