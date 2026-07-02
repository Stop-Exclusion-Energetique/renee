using Renee.Application.DTOs.PreWorkPlan;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IProjectTypeService
{
	Task<ReneeOperationResult<IEnumerable<ProjectTypeDto>>> GetAllProjectTypes();
}