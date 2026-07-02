using MediatR;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.Interfaces;
using Renee.Application.Queries.ReportingStructure;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class ReportingStructureService(IMediator mediator) : IReportingStructureService
{
	public async Task<ReneeOperationResult<IEnumerable<ReportingStructureDto>>> GetAllReportingStructures() =>
		await mediator.Send(new GetAllReportingStructuresQuery());
}