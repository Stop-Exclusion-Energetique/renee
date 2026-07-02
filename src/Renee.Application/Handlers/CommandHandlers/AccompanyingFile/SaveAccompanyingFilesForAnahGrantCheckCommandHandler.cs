using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class SaveAccompanyingFilesForAnahGrantCheckCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<SaveAccompanyingFilesForAnahGrantCheckCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(SaveAccompanyingFilesForAnahGrantCheckCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			foreach (var item in request.AccompanyingFilesGrantDates)
			{
				var accompanyingFile = await accompanyingFileRepository.GetBaseAccompanyingFile(item.AccompanyingFileId);

				if (accompanyingFile is null)
					return ReneeOperationResult<bool>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

				accompanyingFile.AnahGrantDate = item.GrantDate;
				accompanyingFile.UpdatedBy = request.UserId;
				accompanyingFile.LastUpdateDate = DateTime.UtcNow;

				var updateResult = await accompanyingFileRepository.UpdateBaseAccompanyingFile(accompanyingFile);

				if (updateResult <= 0)
					return ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileUpdatingAccompanyingFile);
			}

			var user = await userRepository.GetUserById(request.UserId);

			if (user is null) return ReneeOperationResult<bool>.Failure(Labels.Errors.UserNotFound);

			user.NextDateForAnahGrantCheck = DateTime.UtcNow.AddDays(21);

			return ReneeOperationResult<bool>.Success(true, Labels.SaveAnahGrantCheckSuccess);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}