using Renee.Application.DTOs.WorkPackageWorkTypeCost;

namespace Renee.Application.DTOs.WorkPackage;

public record WorkPackageDto(
	Guid Id, 
	string? EnergeticsEffectOfWorks, 
	List<WorkPackageWorkTypeCostDto>? TypeCostDtos);