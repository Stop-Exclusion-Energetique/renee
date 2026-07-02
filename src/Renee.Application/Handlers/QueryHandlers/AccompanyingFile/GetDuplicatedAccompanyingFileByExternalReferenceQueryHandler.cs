using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetDuplicatedAccompanyingFileByExternalReferenceQueryHandler(
    IAccompanyingFileRepository accompanyingFileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<GetDuplicatedAccompanyingFileByExternalReferenceQuery, ReneeOperationResult<List<ExistingAccompanyingFileDto>>>
{
    public async Task<ReneeOperationResult<List<ExistingAccompanyingFileDto>>> Handle(
        GetDuplicatedAccompanyingFileByExternalReferenceQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.References.Count() == 0)
                return ReneeOperationResult<List<ExistingAccompanyingFileDto>>.Success([]);

            var duplicates = await accompanyingFileRepository
                .GetAccompanyingFilesByExternalReferences(request.References);

            return ReneeOperationResult<List<ExistingAccompanyingFileDto>>.Success(duplicates.Select(af => new ExistingAccompanyingFileDto
            {
                Id = af.Id,
                ExternalReference = af.ExternalReference,
                MainOccupantId = af.AccompanyingFileHouseholdNavigation?.MainOccupantNavigation?.Id,
                AddressId = af.AccompanyingFileHousingNavigation?.HousingAddressNavigation?.Id
            }).ToList());
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<List<ExistingAccompanyingFileDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}
