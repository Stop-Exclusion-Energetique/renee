using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class DifficultyFacedByFamilyService(IMediator mediator) : IDifficultyFacedByFamilyService
{
	public async Task<ReneeOperationResult<IEnumerable<DifficultyFacedFamilyDto>>> GetAllAsync() =>
		await mediator.Send(new GetAllDifficultiesFacedByFamilyQuery());
}