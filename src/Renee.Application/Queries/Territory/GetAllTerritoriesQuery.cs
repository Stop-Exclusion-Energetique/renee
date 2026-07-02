using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.Territory;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Territory;

public class GetAllTerritoriesQuery : IQuery<ReneeOperationResult<IEnumerable<TerritoryQueryObjectResult>>>;