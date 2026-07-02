using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.Territory;
using Renee.Application.DTOs.User;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetInitialisationDataForUserCreationFormQuery : IQuery<ReneeOperationResult<GetInitialisationDataForUserCreationFormQueryResult>>
{}

public record GetInitialisationDataForUserCreationFormQueryResult
{
	public List<RoleDto> Roles { get; set; } = [];
	public List<ReportingStructureDto> ReportingStructures { get; set; } = [];
	public List<TerritoryQueryObjectResult> Territories { get; set; } = [];
}