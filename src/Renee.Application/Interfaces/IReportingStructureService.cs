using Renee.Application.DTOs.ReportingStructure;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IReportingStructureService
{
	Task<ReneeOperationResult<IEnumerable<ReportingStructureDto>>> GetAllReportingStructures();
}