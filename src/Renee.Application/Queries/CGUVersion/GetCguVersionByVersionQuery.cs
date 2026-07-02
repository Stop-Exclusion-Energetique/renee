using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.CGUVersion;

public class GetCguVersionByVersionQuery(string version) : IRequest<ReneeOperationResult<CguVersionDto>>
{
    public string Version { get; set; } = version;
}
