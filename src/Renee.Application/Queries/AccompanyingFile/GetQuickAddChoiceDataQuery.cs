using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.Territory;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetQuickAddChoiceDataQuery : IQuery<ReneeOperationResult<GetQuickAddChoiceDataQueryObjectResult>>
{
	public bool ShouldRetrieveFakeUser { get; set; }
}

public record GetQuickAddChoiceDataQueryObjectResult
{
	public required List<QuickAddUserResult> TerritorialBuilders { get; init; }
	public required List<QuickAddUserResult> SolidarBuilders { get; init; }
	public required List<QuickAddUserResult> DiffuseCoordinators { get; init; }
	public required List<QuickAddUserResult> TargetCoordinators { get; init; }
	public required List<TerritoryQueryObjectResult> Territories { get; init; }
}

public record QuickAddUserResult(string UserFullName, Guid UserId, Guid? TerritoryId = null);