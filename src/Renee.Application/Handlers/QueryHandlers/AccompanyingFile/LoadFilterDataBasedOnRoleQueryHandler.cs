using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.Territory;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class LoadFilterDataBasedOnRoleQueryHandler(
	IUserRepository userRepository,
	IReportingStructureRepository reportingStructureRepository,
	ITerritoryRepository territoryRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	IAdminConstantRepository adminConstantRepository,
	ITelemetryService telemetryService)
	: QueryHandler<LoadFilterDataBasedOnRoleQuery, ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>> HandleQuery(
		LoadFilterDataBasedOnRoleQuery request)
	{
		try
		{
			var es = new List<Domain.Entity.User>();
			var reportingStructures = new List<Domain.Entity.ReportingStructure>();
			var territories = new List<Domain.Entity.Territory>();
			var reportingStructureIdsForTerritorialBuilder = new HashSet<Guid>();
			var solidarBuilderIdsForTerritorialBuilder = new HashSet<Guid>();
			var accompanyingFileModificationDeadline = (await adminConstantRepository.GetAll()).Where(x => x.Name == IndexLabels.TzeeAccompanyingFileModificationDeadline).FirstOrDefault()?.AccompanyingFileModificationDeadline;

			if (request.Role == Constants.AssociationMemberRole ||
			    request.Role == Constants.DiffuseCoordinatorRole ||
			    request.Role == Constants.TargetedCoordinatorRole ||
			    request.Role == Constants.TerritorialBuilderRole)
				reportingStructures = await reportingStructureRepository.GetAllReportingStructuresAsync();

			if (request.Role == Constants.DiffuseCoordinatorRole ||
			    request.Role == Constants.TargetedCoordinatorRole ||
			    request.Role == Constants.TerritorialBuilderRole)
				es = await userRepository.GetUsersByRole(Constants.SolidarBuilderRole, true);

			if (request.Role == Constants.TargetedCoordinatorRole)
				territories = await territoryRepository.GetAllTerritories();

			if (request.Role == Constants.TerritorialBuilderRole && request.UserId.HasValue)
			{
				var filterData = await accompanyingFileRepository
					.GetTerritorialBuilderFilterData(request.UserId.Value);
				reportingStructureIdsForTerritorialBuilder = filterData.ReportingStructureIds.ToHashSet();
				solidarBuilderIdsForTerritorialBuilder = filterData.SolidarBuilderIds.ToHashSet();
			}

			if (request.Role == Constants.TerritorialBuilderRole)
			{
				reportingStructures = reportingStructures
					.Where(rs => reportingStructureIdsForTerritorialBuilder.Contains(rs.Id))
					.ToList();
				es = es.Where(user => solidarBuilderIdsForTerritorialBuilder.Contains(user.Id)).ToList();
			}

			return ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult
			{
				SolidarBuilderUsers =
					es.Select(
						x => new SolidarBuilderUserObjectResult(
							$"{x.FirstName} {x.LastName}",
							x.Id,
							x.ReportingStructureId ?? Guid.Empty)).ToList(),
				ReportingStructures =
					reportingStructures.Select(x => new ReportingStructureDto(x.Id, x.Name, x.NationalStructureId)).ToList(),
				Territories = territories.Select(x => new TerritoryQueryObjectResult(x.Label, x.Id)).ToList(),
				IsAccompanyingFileModificationDeadlineReached = accompanyingFileModificationDeadline.HasValue && DateTime.UtcNow > accompanyingFileModificationDeadline.Value
			});
		}
		catch (Exception e)
		{
			await telemetryService.TrackExceptionAsync(e, new CancellationToken());
			return ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
