using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.Territory;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

namespace Renee.Application.Queries.AccompanyingFile;
using Renee.Domain.ReneeError;

public class LoadFilterDataBasedOnRoleQuery(string role, Guid? userId = null)
	: IQuery<ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>>
{
	public string Role { get; set; } = role;
	public Guid? UserId { get; set; } = userId;
}

public record LoadAccompanyingFileListDataQueryObjectResult
{
	public List<ReportingStructureDto> ReportingStructures { get; set; } = new();
	public List<SolidarBuilderUserObjectResult> SolidarBuilderUsers { get; set; } = new();
	public List<TerritoryQueryObjectResult> Territories { get; set; } = new();
	public bool IsAccompanyingFileModificationDeadlineReached { get; set; } = false;
}
