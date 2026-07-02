using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.OrganizeAndFinanceStateUseCase;

public class SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(
		SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(
					request.AccompanyingFileId);

			if (accompanyingFile is null) return ReneeOperationResult<bool>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			accompanyingFile.LastUpdateDate = DateTime.UtcNow;
			accompanyingFile.UpdatedBy = request.ConnectedUserId;
			accompanyingFile.AccompanyingTimeDurationForOrganizeAndFinanceMilestone = (int?)request.AccompanyingTimeDuration;
			accompanyingFile.AnahFolderNumber = request.AnahFolderNumber;
			accompanyingFile.AnahFolderFilingDate = request.AnahFolderFilingDate;

			var dto = AccompanyingFileMilestoneUpdaterExtension.SaveOrganizeAndFinanceMilestoneUpdater(request, accompanyingFile);

			var numberItemsChanged = await accompanyingFileRepository
				.UpdateAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(accompanyingFile, dto);

			return numberItemsChanged > -1
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