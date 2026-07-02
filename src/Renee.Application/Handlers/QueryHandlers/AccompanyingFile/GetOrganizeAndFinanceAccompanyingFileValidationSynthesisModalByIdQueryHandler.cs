using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetOrganizeAndFinanceAccompanyingFileValidationSynthesisModalByIdQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetOrganizeAndFinanceAccompanyingFileValidationSynthesisModalByIdQuery,
		ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>>
{
	public async Task<ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>> Handle(
		GetOrganizeAndFinanceAccompanyingFileValidationSynthesisModalByIdQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFileEntity =
				await accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(
					request.AccompanyingFileId);

			if (accompanyingFileEntity is null) return ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>.Success(null);

			return ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>.Success(new OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto
			{
				AccompanyingFileId = request.AccompanyingFileId,
				AnahCategory = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.AnahCategory,
				OwnershipStatus =
					((OwnershipStatus?)accompanyingFileEntity.AccompanyingFileHousingNavigation.OwnershipStatus)
					.GetDescription(),
				DpeLabel =
					((DpeLabel?)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
						.Dpe).GetDescription(),
				GesLabel =
					((GesLabel?)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
						.Ges).GetDescription(),
				BlockingProofComment = accompanyingFileEntity.CommentOnBlockingProof ?? string.Empty,
				Status = (AccompanyingFileStatus)accompanyingFileEntity.AccompanyingFileStatus,
					IsIncludedInTzeeProgram = accompanyingFileEntity.ZeroEnergyExclusionTerritoriesProgram
					});
				}
				catch (Exception ex)
				{
					await telemetryService.TrackExceptionAsync(ex, cancellationToken);
					return ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
				}
	}
}