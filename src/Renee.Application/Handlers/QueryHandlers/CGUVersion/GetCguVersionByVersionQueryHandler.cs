using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Application.Queries.CGUVersion;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.CGUVersion;

public class GetCguVersionByVersionQueryHandler(
	ICguVersionRepository cguVersionRepository,
	ITelemetryService telemetryService)
    : IRequestHandler<GetCguVersionByVersionQuery, ReneeOperationResult<CguVersionDto>>
{
    public async Task<ReneeOperationResult<CguVersionDto>> Handle(
        GetCguVersionByVersionQuery request, CancellationToken cancellationToken)
    {
		try
		{
			var cgu = await cguVersionRepository.GetByVersionAsync(request.Version);
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
