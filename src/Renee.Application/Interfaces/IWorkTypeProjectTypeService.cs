using Renee.Application.DTOs.WorkTypeProjectType;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IWorkTypeProjectTypeService
{
	public Task<ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>> GetAllWorkTypeProjectTypes();
}