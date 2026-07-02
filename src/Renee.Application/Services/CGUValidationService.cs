using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class CguValidationService(IMediator mediator) : ICguValidationService
{

    public async Task<ReneeOperationResult<UserVersionCguDto>> InitializeCGUVerificationAsync(Guid id)
    {
        return await mediator.Send(new GetRegisteredUserAndLastCguQuery(id));
    }

    public async Task<ReneeOperationResult<bool>> MarkCGUAsAcceptedAsync(Guid id, string? cgu)
    {
        var userCGUDto = new UserCguDto{ IdUser = id, ValidatedCGUVersion = cgu};
        return await mediator.Send(new UpdateVersionCguForUserCommand(userCGUDto));
    }
}



    
