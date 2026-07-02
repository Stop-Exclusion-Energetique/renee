using MediatR;
using Renee.Application.Commands.CguVersion;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Application.Queries.CGUVersion;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class CguVersionService(IMediator mediator) : ICguVersionService
{
    public async Task<ReneeOperationResult<bool>> AddCGUVersion(CguVersionDto cguVersion)
        => await mediator.Send(new CreateCguVersionCommand(cguVersion));

    public async Task<ReneeOperationResult<bool>> DeleteCguAsync(Guid id)
        => await mediator.Send(new DeleteCguVersionCommand(id));

    public async Task<ReneeOperationResult<IEnumerable<CguVersionDto>>> GetAllVersionsAsync() 
        => await mediator.Send(new GetAllCguVersionsQuery());

    public async Task<ReneeOperationResult<CguVersionDto>> GetByVersionAsync(string version) 
        => await mediator.Send(new GetCguVersionByVersionQuery(version));

    public async Task<ReneeOperationResult<CguVersionDto>> GetLatestVersionAsync()
        => await mediator.Send(new GetLatestCguVersionQuery());

    public async Task<ReneeOperationResult<bool?>> VerifiyDuplicatedVersionAsync(string version)
        => await mediator.Send(new CheckDuplicatedCguVersionByVersionQuery(version));
}
