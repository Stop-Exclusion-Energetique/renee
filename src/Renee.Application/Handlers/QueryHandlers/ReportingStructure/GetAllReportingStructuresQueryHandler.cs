using MediatR;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.Interfaces;
using Renee.Application.Queries.ReportingStructure;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.ReportingStructure;

public class GetAllReportingStructuresQueryHandler(
	IReportingStructureRepository reportingStructureRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllReportingStructuresQuery, ReneeOperationResult<IEnumerable<ReportingStructureDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<ReportingStructureDto>>> Handle(
		GetAllReportingStructuresQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = (await reportingStructureRepository.GetAllReportingStructuresAsync()).Select(
				rs => new ReportingStructureDto(rs.Id, rs.Name, rs.NationalStructureId));
			return ReneeOperationResult<IEnumerable<ReportingStructureDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<ReportingStructureDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}