using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.DTOs.Territory;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record LoadListDataForCsvImportQuery : IRequest<ReneeOperationResult<LoadListDataForCsvImportQueryObjectResult>>;

public class LoadListDataForCsvImportQueryObjectResult
{
	public List<TerritoryQueryObjectResult> Territories { get; set; } = [];
	public List<ProjectTypeDto> ProjectTypes { get; set; } = [];
	public List<InsuranceTypeDto> InsuranceTypes { get; set; } = [];
	public List<DifficultyFacedFamilyDto> DifficultiesFacedFamily { get; set; } = [];
	public List<HouseholdResourcesTypologyDto> HouseholdResourcesTypology { get; set; } = [];
	public List<HouseholdHeatingEnergyLabel> HouseholdHeatingEnergyLabel { get; set; } = [];
}