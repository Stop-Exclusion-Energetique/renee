using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class AbortAccompanyingFileCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService) 
	: IRequestHandler<AbortAccompanyingFileCommandInput, ReneeStringOperationResult>
{
	public async Task<ReneeStringOperationResult> Handle(AbortAccompanyingFileCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.UserId == Guid.Empty)
				return ReneeStringOperationResult.Failure(Labels.Errors.UserNotFound);

			var accompanyingFile = await accompanyingFileRepository.GetBaseAccompanyingFile(request.AccompanyingFileId);

			if (accompanyingFile == null)
				return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFile);

			accompanyingFile.UpdateAccompanyingFileForAbortRequest(
				request.AbortReasonLabelId,
				request.UserId,
				request.SolidarBuilderComment,
				request.IsBillingRequested,
				request.HasAttachment);

			var result = await accompanyingFileRepository.UpdateBaseAccompanyingFile(accompanyingFile);

			if (result == -1)
				return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingAccompanyingFile);

			if (result == 0)
				return ReneeStringOperationResult.Failure(Labels.Errors.StageValidationErrorAccompanyingFileUpdateFailed);

			return ReneeStringOperationResult.Success(Labels.AccompanyingFileAbortConfirmationMessage);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeStringOperationResult.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}