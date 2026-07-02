using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Occupant;

public class GetAllDifficultiesFacedByFamilyQueryHandler(
	IDifficultyFacedByFamilyRepository difficultyFacedByFamilyRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllDifficultiesFacedByFamilyQuery, ReneeOperationResult<IEnumerable<DifficultyFacedFamilyDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<DifficultyFacedFamilyDto>>> Handle(
		GetAllDifficultiesFacedByFamilyQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var categories = await difficultyFacedByFamilyRepository.GetAllAsync();
			var result = categories.Select(c => new DifficultyFacedFamilyDto { Id = c.Id, Name = c.Labels }).OrderBy(x => x.Name);
			return ReneeOperationResult<IEnumerable<DifficultyFacedFamilyDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<DifficultyFacedFamilyDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}