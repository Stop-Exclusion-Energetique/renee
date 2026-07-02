using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Application.Queries.CGUVersion;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.CGUVersion;

public class GetLatestCguVersionQueryHandler(
	ICguVersionRepository cguVersionRepository,
	ITelemetryService telemetryService)
    : IRequestHandler<GetLatestCguVersionQuery, ReneeOperationResult<CguVersionDto>>
{
    public async Task<ReneeOperationResult<CguVersionDto>> Handle(GetLatestCguVersionQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var cgu = await cguVersionRepository.GetLatestVersionAsync();
			if (cgu == null)
				return ReneeOperationResult<CguVersionDto>.Failure(Labels.Errors.CguNotFound); 
			var cguVersionDto = new CguVersionDto { Id = cgu.Id, Label = cgu.Label, Version = cgu.Version, CreatedDate = cgu.CreatedAt };
			return ReneeOperationResult<CguVersionDto>.Success(cguVersionDto);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<CguVersionDto>.Failure(Labels.Errors.CguNotFound);
		}
    }
}
