using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetAllRegisteredUserQueryHandler(
    IUserRepository userRepository,
    ITelemetryService telemetryService
) : QueryHandler<GetAllRegisteredUserQuery, ReneeOperationResult<List<GetAllRegisteredQyeryObjectResult>>>
{
    public override async Task<ReneeOperationResult<List<GetAllRegisteredQyeryObjectResult>>> HandleQuery(GetAllRegisteredUserQuery request)
    {
        try
        {
            var userList = await userRepository.GetAllRegisteredUser();

            return ReneeOperationResult<List<GetAllRegisteredQyeryObjectResult>>.Success(userList.Select(
                    u => new GetAllRegisteredQyeryObjectResult(
                        u.Id,
                        u.FirstName!, 
                        u.LastName!, 
                        u.RoleId, 
                        u.Role.LongName, 
                        u.ReportingStructureId, 
                        u.ReportingStructureId is null ? u.ReportingStructure : u.ReportingStructureNavigation!.Name 
                    )
                )
                .ToList());
        }
        catch(Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            return ReneeOperationResult<List<GetAllRegisteredQyeryObjectResult>>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}
