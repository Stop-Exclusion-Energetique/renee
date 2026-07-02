using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;
using Renee.Domain.DomainExtension;
using Renee.Domain.Enums;
using Renee.Application.Interfaces;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class ValidateAccompanyingFileStageChangeCommandHandler (
	IAccompanyingFileRepository accompanyingFileRepository,
	IUserRepository userRepository,
	IAdminConstantRepository adminConstantsRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<ValidateAccompanyingFileStageChangeCommandInput, ReneeOperationResult<RequiredDataForStageValidationEmail>>
{
	public async Task<ReneeOperationResult<RequiredDataForStageValidationEmail>> Handle(ValidateAccompanyingFileStageChangeCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			var tzeeMaximumNumberOfAccompanyingFile = (await adminConstantsRepository.GetAll()).FirstOrDefault(c => c.Name == IndexLabels.MaximalNumberOfAccompanyingFileCreated)?.Value;
			var numberOfAccompanyingFileInTzeeProgram = await accompanyingFileRepository.CountAllActiveAndInTzeeProgramAccompanyingFile();

				if (numberOfAccompanyingFileInTzeeProgram >= tzeeMaximumNumberOfAccompanyingFile)
					return ReneeOperationResult<RequiredDataForStageValidationEmail>.Failure(Labels.Errors.TzeeProgramDeadlineReached);	

			var accompanyingFile = await accompanyingFileRepository.GetAccompanyingFileForStageValidation(request.AccompanyingFileId);

			if (accompanyingFile == null)
				return ReneeOperationResult<RequiredDataForStageValidationEmail>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);
			
			var currentAccompanyingFileStage = (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone;

			accompanyingFile.ChangeAccompanyingFileStageAndStatus(request.IsValidated, request.UserId, request.CommentOnValidation);

			var result = await accompanyingFileRepository.UpdateBaseAccompanyingFile(accompanyingFile);

			if (result == 0)
				return ReneeOperationResult<RequiredDataForStageValidationEmail>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileUpdateFailed);

			var stageValidator = await userRepository.GetUserById(request.UserId);

			if (stageValidator == null)
				return ReneeOperationResult<RequiredDataForStageValidationEmail>.Failure(Labels.Errors.UserNotFound);

			var requiredFields = new RequiredDataForStageValidationEmail(
				stageValidator.Email ?? string.Empty,
				GetSolidarBuildersEmailFromSupportTeam(accompanyingFile.AccompanyingFileSupportTeamNavigation),
				$"{accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName} {accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName}",
				accompanyingFile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.FirstName ?? string.Empty,
				accompanyingFile.AccompanyingFileReference,
				currentAccompanyingFileStage,
				(AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone);

			return ReneeOperationResult<RequiredDataForStageValidationEmail>.Success(requiredFields, Labels.StageValidationSuccess);
		}
		catch (Exception ex) 
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<RequiredDataForStageValidationEmail>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static List<string> GetSolidarBuildersEmailFromSupportTeam(Domain.Entity.SupportTeam supportTeam) 
	{
		return new[]
		{
			supportTeam.SolidarBuilderNavigation?.Email,
			supportTeam.SecondSolidarBuilderNavigation?.Email,
			supportTeam.ThirdSolidarBuilderNavigation?.Email
		}.Where(email => !string.IsNullOrWhiteSpace(email)).Distinct().ToList()!;
	}
}