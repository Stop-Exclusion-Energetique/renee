using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.SupportTeam;

public class UpdateSupportTeamMemberCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ICopropertyProfileRepository copropertyProfileRepository,
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<UpdateSupportTeamMemberCommandInput, ReneeOperationResult<int>>
{
	public async Task<ReneeOperationResult<int>> Handle(
		UpdateSupportTeamMemberCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			if (request.AssignedTo == null)
				return ReneeOperationResult<int>.Failure(Labels.Errors.AssignedUserRequired);

			var assignedUser = await userRepository.GetUserById((Guid)request.AssignedTo);
			if (assignedUser == null)
				return ReneeOperationResult<int>.Failure(Labels.Errors.AssignedUserNotFound);

			if (request.IsCoproperty)
			{
				var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileSupportTeam(request.AssociatedResourceId);
				if (copropertyProfile == null)
					return ReneeOperationResult<int>.Failure(Labels.Errors.ErrorCopropertyProfileNotFound);

				var result = await copropertyProfileRepository.UpdateCopropertyProfileSupportTeam(
					copropertyProfile!.CopropertySupportTeamNavigation.Id, 
					assignedUser, 
					request.SupportTeamMemberRole);
				return ReneeOperationResult<int>.Success(result, Labels.UpdateSupportTeamSuccess);
			}

			var accompanyingFile = await accompanyingFileRepository.GetAccompanyingFileSupportTeam(request.AssociatedResourceId);
			if (accompanyingFile == null)
				return ReneeOperationResult<int>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			var updateResult = await accompanyingFileRepository.UpdateAccompanyingSupportTeam(
				accompanyingFile,
				assignedUser,
				request.SupportTeamMemberRole);
			return ReneeOperationResult<int>.Success(updateResult, Labels.UpdateSupportTeamSuccess);
		}
		catch (Exception ex) 
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<int>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}