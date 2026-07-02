using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IReportingStructureRepository
{
	Task<List<ReportingStructure>> GetAllReportingStructuresAsync();
	Task<Guid> AddReportingStructureAsync(ReportingStructure reportingStructure);
}