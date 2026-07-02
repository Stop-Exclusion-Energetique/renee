using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;
using Renee.Domain.DomainExtension;
using Renee.Domain.Enums;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class ValidateAccompanyingFileAbortCommandHandler(
    IAccompanyingFileRepository accompanyingFileRepository,
    ITelemetryService telemetryService) : IRequestHandler<ValidateAccompanyingFileAbortCommandInput, ReneeStringOperationResult>
{
    public async Task<ReneeStringOperationResult> Handle(ValidateAccompanyingFileAbortCommandInput request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AccompanyingFileId == Guid.Empty)
                return ReneeStringOperationResult.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

            var accompanyingFile = await accompanyingFileRepository.GetBaseAccompanyingFile(request.AccompanyingFileId);

            if (accompanyingFile == null)
                return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFile);

			if (request.ShouldAbort)
				accompanyingFile.UpdateAccompanyingFileForAbortValidation(
					request.UserId,
					AccompanyingFileStatus.Aborted,
					request.CommentsOnAccompanyingFileAbort);
			else
				accompanyingFile.UpdateAccompanyingFileForAbortValidation(
					request.UserId,
					AccompanyingFileStatus.InProgress,
					request.CommentsOnAccompanyingFileAbort
				);

			var result = await accompanyingFileRepository.UpdateBaseAccompanyingFile(accompanyingFile);

            if (result == -1)
                return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingAccompanyingFile);

            if (result == 0)
                return ReneeStringOperationResult.Failure(Labels.Errors.StageValidationErrorAccompanyingFileUpdateFailed);

            return request.ShouldAbort ? ReneeStringOperationResult.Success(Labels.AccompanyingFileAbortedSuccessfully) : ReneeStringOperationResult.Success(Labels.AccompanyingFileRevertedSuccessfully);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeStringOperationResult.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}