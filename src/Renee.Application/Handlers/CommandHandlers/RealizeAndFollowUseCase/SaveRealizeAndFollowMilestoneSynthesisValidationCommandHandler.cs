using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.CommandsUseCasesInput.Results;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.RealizeAndFollowUseCase;

public class SaveRealizeAndFollowMilestoneSynthesisValidationCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<SaveRealizeAndFollowMilestoneSynthesisValidationCommandInput, ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>>
{
	public async Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> Handle(
		SaveRealizeAndFollowMilestoneSynthesisValidationCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			if (request.AccompanyingFileId == Guid.Empty)
				return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileSynthesis(request.AccompanyingFileId);

			if (accompanyingFile is null)
				return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			if ((AccompanyingType?)accompanyingFile.AccompanyingType == AccompanyingType.Targeted &&
				accompanyingFile.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation is null)
				return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Failure(Labels.Errors.NoTerritorialBuildersAffectedOnTargetedAccompanyingFileType);

			var user = await userRepository.GetUserById(request.UserId);

			if (user is null)
				return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Failure(Labels.Errors.UserNotFound);

			var currentStage = (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone;

			accompanyingFile.UpdateSynthesis(
				request.UserCanValidateSynthesis ? AccompanyingFileStage.Finished : AccompanyingFileStage.RealisationAndFollowing,
				request.UserCanValidateSynthesis ? AccompanyingFileStatus.Finished : AccompanyingFileStatus.WaitingForApproval,
				request.UserId,
				request.UserCanValidateSynthesis
			);

			var result = await accompanyingFileRepository.UpdateAccompanyingFileSynthesis(accompanyingFile);

			if (result == 0)
				return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileUpdateFailed);

			var requiredDataForEmail = new RequiredDataForStageSynthesisValidationEmail(
				currentStage,
				$"{accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName} {accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName}",
				(AccompanyingType?)accompanyingFile.AccompanyingType,
				$"{user.LastName} {user.FirstName}",
				GetUsersEmail(accompanyingFile.AccompanyingFileSupportTeamNavigation, (AccompanyingType?)accompanyingFile.AccompanyingType));

			return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Success(requiredDataForEmail, Labels.StageValidationSuccess);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static List<string> GetUsersEmail(Domain.Entity.SupportTeam supportTeam, AccompanyingType? accompanyingType)
	{
		var emails = new List<string>();

		if (accompanyingType == AccompanyingType.Diffuse && 
			!string.IsNullOrWhiteSpace(supportTeam.DiffuseCoordinatorNavigation?.Email))
		{
			emails.Add(supportTeam.DiffuseCoordinatorNavigation.Email);
		}

		if (accompanyingType == AccompanyingType.Targeted)
		{
			if (!string.IsNullOrWhiteSpace(supportTeam.TerritorialBuilderNavigation?.Email))
				emails.Add(supportTeam.TerritorialBuilderNavigation.Email);

			if (!string.IsNullOrWhiteSpace(supportTeam.SecondTerritorialBuilderNavigation?.Email))
				emails.Add(supportTeam.SecondTerritorialBuilderNavigation.Email);
		}

		return emails.Distinct().ToList();
	}
}