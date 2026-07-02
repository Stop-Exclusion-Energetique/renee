using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.CGUVersion;

public class CheckDuplicatedCguVersionByVersionQuery(string version): IRequest<ReneeOperationResult<bool?>>
{
    public string Version { get; set; } = version;
}
