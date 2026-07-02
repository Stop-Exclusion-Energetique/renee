using Renee.Application.DTOs.PreWorkPlan;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IInsuranceTypeService
{
	Task<ReneeOperationResult<IEnumerable<InsuranceTypeDto>>> GetAllInsuranceTypes();
}