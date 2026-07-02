using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IWorkTypesLabelsService
{
	Task<ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>> GetAllWorkTypesLabels();
}