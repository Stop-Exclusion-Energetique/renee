using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.CGUVersion;

public class GetLatestCguVersionQuery : IRequest<ReneeOperationResult<CguVersionDto>>;