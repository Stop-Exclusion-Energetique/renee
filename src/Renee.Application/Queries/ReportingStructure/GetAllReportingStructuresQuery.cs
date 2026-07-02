using MediatR;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.ReportingStructure;

public class GetAllReportingStructuresQuery : IRequest<ReneeOperationResult<IEnumerable<ReportingStructureDto>>>;