using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IMainOccupantService
{
	Task<ReneeOperationResult<IEnumerable<MainOccupantDto>>> GetAllMainOccupantAsync();
}