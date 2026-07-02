using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.Territory;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetInitialisationDataForUserCreationFormQueryHandler (
		IRoleRepository roleRepository,
		IReportingStructureRepository reportingStructureRepository,
		ITerritoryRepository territoryRepository,
		ITelemetryService telemetryService)
	: QueryHandler<GetInitialisationDataForUserCreationFormQuery, ReneeOperationResult<GetInitialisationDataForUserCreationFormQueryResult>>
{
	public override async Task<ReneeOperationResult<GetInitialisationDataForUserCreationFormQueryResult>> HandleQuery(GetInitialisationDataForUserCreationFormQuery request)
	{
		try
		{
			var roles = await roleRepository.GetAllAsync();
			var reportingStructures = await reportingStructureRepository.GetAllReportingStructuresAsync();
			var territories = await territoryRepository.GetAllTerritories();

			return ReneeOperationResult<GetInitialisationDataForUserCreationFormQueryResult>.Success(new GetInitialisationDataForUserCreationFormQueryResult
			{
				Roles = roles.Select(r => new RoleDto { Id = r.Id, LongName = r.LongName }).ToList(),
				ReportingStructures = reportingStructures.Select(r => new ReportingStructureDto(r.Id, r.Name, r.NationalStructureId)).ToList(),
				Territories = territories.Select(t => new TerritoryQueryObjectResult(t.Label, t.Id)).ToList()
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetInitialisationDataForUserCreationFormQueryResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
