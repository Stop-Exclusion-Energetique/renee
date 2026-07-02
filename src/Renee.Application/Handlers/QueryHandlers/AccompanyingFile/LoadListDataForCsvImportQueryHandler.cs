using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.DTOs.Territory;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class LoadListDataForCsvImportQueryHandler(
	ITerritoryRepository territoryRepository,
	IProjectTypeRepository projectTypeRepository,
	IInsuranceTypeRepository insuranceTypeRepository,
	IDifficultyFacedByFamilyRepository difficultyFacedByFamilyRepository,
	IHouseholdResourcesTypologyRepository householdResourcesTypologyRepository,
	IHouseholdHeatingEnergyLabelRepository householdHeatingEnergyLabelRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<LoadListDataForCsvImportQuery, ReneeOperationResult<LoadListDataForCsvImportQueryObjectResult>>
{
	public async Task<ReneeOperationResult<LoadListDataForCsvImportQueryObjectResult>> Handle(LoadListDataForCsvImportQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var projectTypes = await projectTypeRepository.GetAllAsync();
			var territories = await territoryRepository.GetAllTerritories();
			var insuranceTypes = await insuranceTypeRepository.GetAllAsync();
			var difficultiesFacedByFamily = await difficultyFacedByFamilyRepository.GetAllAsync();
			var householdResourcesTypology = await householdResourcesTypologyRepository.GetAllAsync();
			var householdHeatingEnergyLabels = await householdHeatingEnergyLabelRepository.GetAllHouseHoldHeatingEnergyLabelAsync();

			return ReneeOperationResult<LoadListDataForCsvImportQueryObjectResult>.Success(new LoadListDataForCsvImportQueryObjectResult
			{
				Territories = territories.Select(t => new TerritoryQueryObjectResult(t.Label, t.Id)).ToList(),
				ProjectTypes = projectTypes.Select(pt => new ProjectTypeDto(pt.Id, pt.Label)).ToList(),
				InsuranceTypes = insuranceTypes.Select(it => new InsuranceTypeDto(it.Id, it.Label)).ToList(),
				DifficultiesFacedFamily = difficultiesFacedByFamily.Select(d => new DifficultyFacedFamilyDto { Id = d.Id, Name = d.Labels }).ToList(),
				HouseholdResourcesTypology = householdResourcesTypology.Select(hr => new HouseholdResourcesTypologyDto { Id = hr.Id, Name = hr.Labels }).ToList(),
				HouseholdHeatingEnergyLabel = householdHeatingEnergyLabels.Select(he => new HouseholdHeatingEnergyLabel { Id = he.Id, Name = he.Name }).ToList()
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<LoadListDataForCsvImportQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}