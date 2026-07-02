using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetIdentifyAccompanyingFileValidationSynthesisModalByIdQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetIdentifyAccompanyingFileValidationSynthesisModalByIdQuery,
		ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>>
{
	public async Task<ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>> Handle(
		GetIdentifyAccompanyingFileValidationSynthesisModalByIdQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFileEntity =
				await accompanyingFileRepository.GetAccompanyingFileSynthesis(request.AccompanyingFileId);

			if (accompanyingFileEntity is not null)
				return ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>.Success(new IdentifyAccompanyingFileValidationSynthesisModalDto
				{
					AccompanyingFileId = request.AccompanyingFileId,
					BlockingProofComment = accompanyingFileEntity.CommentOnBlockingProof ?? string.Empty,
					Status = (AccompanyingFileStatus)accompanyingFileEntity.AccompanyingFileStatus,
					IsIncludedInTzeeProgram = (bool) accompanyingFileEntity.ZeroEnergyExclusionTerritoriesProgram!
				});

			return ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>.Success(null);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}