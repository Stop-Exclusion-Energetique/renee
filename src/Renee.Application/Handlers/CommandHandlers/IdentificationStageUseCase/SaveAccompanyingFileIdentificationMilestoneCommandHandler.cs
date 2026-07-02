using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.IdentificationStageUseCase;

public class SaveAccompanyingFileIdentificationMilestoneCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	IAdminConstantRepository adminConstantRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<SaveAccompanyingFileIdentificationMilestoneCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(
		SaveAccompanyingFileIdentificationMilestoneCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			if(request.IsSubmission)
			{
				var numberOfActiveAccompanyingFileInTzeeProgramForMilestone1 = await accompanyingFileRepository.CountAllActiveAndInTzeeProgramAccompanyingFileForMilestone1();
				var maxNumberOfAccompanyingFileInTzeeProgramForMilestone1 = (await adminConstantRepository.GetAll())
					.FirstOrDefault(c => c.Name == IndexLabels.MaximalNumberOfAccompanyingFileToValidateFirstStage)?.Value;

				if (numberOfActiveAccompanyingFileInTzeeProgramForMilestone1 >= maxNumberOfAccompanyingFileInTzeeProgramForMilestone1)
				{
					return ReneeOperationResult<bool>.Failure(Labels.Errors.TzeeProgramMilestone1DeadlineReached);
				}
			}

			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(
					request.AccompanyingFileId);

			if (accompanyingFile is null)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			accompanyingFile.UpdatedBy = request.ConnectedUserId;
			accompanyingFile.LastUpdateDate = DateTime.UtcNow;
			accompanyingFile.AccompanyingTimeDurationForIdentificationMilestone = (int?)request.AccompanyingTimeDuration;
			accompanyingFile.StartOfAccompanyingDate = request.StartAccompanyingDate;
			accompanyingFile.FirstEncounterDate = request.FirstEncounterDate;
			accompanyingFile.DeliveryTime = request.DeliveryTime;
			accompanyingFile.ShouldAccompanyingFileBeSubmittedToAnah = request.ShouldAccompanyingFileBeSubmittedToAnah;
			accompanyingFile.CopropertyProfileId = request.UpdatedHousing?.CopropertyProfileId;
			
			var dto = AccompanyingFileMilestoneUpdaterExtension.SaveIdentificationMilestoneUpdater(request, accompanyingFile);

			var numberItemsChanged = await accompanyingFileRepository.UpdateAccompanyingFileByIdForIdentificationMilestoneAsync(accompanyingFile, dto);

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