using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Occupant;

public sealed class GetAllMainOccupantsQuery : IRequest<ReneeOperationResult<IEnumerable<MainOccupantDto>>>;