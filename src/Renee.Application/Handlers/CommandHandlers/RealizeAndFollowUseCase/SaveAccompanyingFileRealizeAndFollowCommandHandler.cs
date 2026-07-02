using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.RealizeAndFollowUseCase;

public class SaveAccompanyingFileRealizeAndFollowCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<SaveAccompanyingFileRealizeAndFollowCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(
		SaveAccompanyingFileRealizeAndFollowCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(
					request.AccompanyingFileId);

			if (accompanyingFile == null) return ReneeOperationResult<bool>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			accompanyingFile.AccompanyingFileWorkMonitoringNavigation ??= new WorkMonitoring();
			accompanyingFile.SiteSupervision ??= new SiteSupervision();

			var dto = AccompanyingFileMilestoneUpdaterExtension.SaveRealizeAndFollowMilestoneUpdater(request, accompanyingFile);

			var success = await accompanyingFileRepository.UpdateAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(
				accompanyingFile,
				dto);

			return success > -1
				? ReneeOperationResult<bool>.Success(true, Labels.SaveMilestoneSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileSavingMilestone);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}