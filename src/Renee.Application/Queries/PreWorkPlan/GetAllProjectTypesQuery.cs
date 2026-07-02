using MediatR;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.PreWorkPlan;

public class GetAllProjectTypesQuery : IRequest<ReneeOperationResult<IEnumerable<ProjectTypeDto>>>;