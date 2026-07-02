using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetRegisteredUserAndLastCguQueryHandler(IUserRepository userRepository,
    ICguVersionRepository cguVersionRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<GetRegisteredUserAndLastCguQuery, ReneeOperationResult<UserVersionCguDto>>
{
    public async Task<ReneeOperationResult<UserVersionCguDto>> Handle(GetRegisteredUserAndLastCguQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var lastVersionCGU = await cguVersionRepository.GetLatestVersionAsync();
            if (lastVersionCGU is null) return ReneeOperationResult<UserVersionCguDto>.Failure(Labels.Errors.CguNotFound);

            var userEntity = await userRepository.GetUserById(request.Id);

            if (userEntity is null)
            {
                var versionCgu = new UserVersionCguDto
                {
                    LastCGULabel = lastVersionCGU?.Label
                };
                return ReneeOperationResult<UserVersionCguDto>.Success(versionCgu);
            }

            var userVersionCgu = new UserVersionCguDto
            {
                LastCGUVersion = lastVersionCGU?.Version,
                LastCGULabel = lastVersionCGU?.Label,
                LastValidatedCGUVersionByUser = userEntity.LastValidatedCGUVersion,
                LastValidatedCGUDateByUser = userEntity.LastValidatedCGUDate,
            };
            return ReneeOperationResult<UserVersionCguDto>.Success(userVersionCgu);

            
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            throw;
        }
    }
}