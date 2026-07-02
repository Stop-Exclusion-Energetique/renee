using MediatR;
using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.WorkTypeProjectType;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.WorkTypeProjectType;

public record GetAllWorkTypesProjectTypesQuery : IQuery<ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>>;