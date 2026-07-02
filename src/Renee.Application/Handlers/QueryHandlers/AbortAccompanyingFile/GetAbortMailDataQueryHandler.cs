using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AbortAccompanyingFile;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AbortAccompanyingFile;

public class GetAbortMailDataQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService) : IRequestHandler<GetAbortMailDataQuery, ReneeOperationResult<AbortMailData>>
{
	public async Task<ReneeOperationResult<AbortMailData>> Handle(GetAbortMailDataQuery request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.AccompanyingFileId == Guid.Empty)
				return ReneeOperationResult<AbortMailData>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			var accompanyingFile = await accompanyingFileRepository.GetAccompanyingFileSupportTeam(request.AccompanyingFileId);

			if (accompanyingFile == null)
				return ReneeOperationResult<AbortMailData>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			var solidarBuilder = accompanyingFile.AbortRequestedBy;

			if (solidarBuilder == null)
				return ReneeOperationResult<AbortMailData>.Failure(Labels.Errors.UserNotFound);

			var supportTeam = accompanyingFile.AccompanyingFileSupportTeamNavigation;
			var result = new AbortMailData
			{
				AccompanyingFileReference = accompanyingFile.AccompanyingFileReference,
				AccompanyingFileStage = (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone,
				IsBillingRequested = accompanyingFile.IsAbortBillingRequested ?? false,
				SolidarBuilderFullName = $"{solidarBuilder.FirstName} {solidarBuilder.LastName}",
			};

			if (request.IsMailForSolidarBuilder && accompanyingFile.AbortDecidedBy != null)
				return BuildResult(result, accompanyingFile.AbortDecidedBy);

			var accompanyingType = (AccompanyingType?)accompanyingFile.AccompanyingType;

			if (accompanyingType is not null
				&& (AccompanyingType)accompanyingType == AccompanyingType.Diffuse
				&& supportTeam.DiffuseCoordinatorNavigation != null)
				return BuildResult(result, supportTeam.DiffuseCoordinatorNavigation);

			if (accompanyingType is not null
				&& accompanyingType == AccompanyingType.Targeted
				&& supportTeam.TerritorialBuilderNavigation != null)
				return BuildResult(result, supportTeam.TerritorialBuilderNavigation);
				
			return ReneeOperationResult<AbortMailData>.Failure(Labels.Errors.ErrorWhileRetrievingAccompanyingTeamInformation);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<AbortMailData>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static ReneeOperationResult<AbortMailData> BuildResult(AbortMailData result, Domain.Entity.User user)
	{
		result.ValidatorFullName = $"{user.FirstName} {user.LastName}";
		result.RecipientEmail = user.Email ?? string.Empty;
		return ReneeOperationResult<AbortMailData>.Success(result);
	}
}
