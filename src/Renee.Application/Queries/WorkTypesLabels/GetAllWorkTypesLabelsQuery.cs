using MediatR;
using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.WorkTypesLabels;

public class GetAllWorkTypesLabelsQuery : IRequest<ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>>;