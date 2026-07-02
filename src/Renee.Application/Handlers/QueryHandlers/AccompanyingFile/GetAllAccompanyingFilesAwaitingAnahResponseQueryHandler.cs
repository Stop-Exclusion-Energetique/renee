using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAllAccompanyingFilesAwaitingAnahResponseQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService) : IRequestHandler<GetAllAccompanyingFilesAwaitingAnahResponseQuery, ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>>
{
	public async Task<ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>> Handle(GetAllAccompanyingFilesAwaitingAnahResponseQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFiles = await accompanyingFileRepository.GetAllAwaitingAnahResponseAsync(request.UserId);

			return ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>.Success([.. accompanyingFiles.Select(af => new AccompanyingFileAwaitingAnahResponseResume
			{
				AccompanyingFileId = af.Id,
				AccompanyingFileReference = af.AccompanyingFileReference,
				OccupantFullName = $"{af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName} {af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName}",
				HousingAddress = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label,
			})]);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}