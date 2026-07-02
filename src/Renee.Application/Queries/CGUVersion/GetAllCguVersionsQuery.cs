using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.CGUVersion;

public sealed class GetAllCguVersionsQuery : IRequest<ReneeOperationResult<IEnumerable<CguVersionDto>>>;
