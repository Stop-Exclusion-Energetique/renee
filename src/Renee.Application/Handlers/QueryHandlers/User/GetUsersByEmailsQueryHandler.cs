using MediatR;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetUsersByEmailsQueryHandler(
    IUserRepository userRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<GetUserByEmailQuery, ReneeOperationResult<UserDto?>>
{
    public async Task<ReneeOperationResult<UserDto?>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.SolidarBuilderEmail == null)
                return ReneeOperationResult<UserDto?>.Failure(string.Format(CsvDataLabel.Errors.NoSolidarBuilder, request.ExternalReference));

            var solidarBuilder = await userRepository.GetUserByEmail(request.SolidarBuilderEmail);

            if (solidarBuilder == null)
                return ReneeOperationResult<UserDto?>.Failure(CsvDataLabel.Errors.InvalidSolidarBuilder);

            var reportingStructure = solidarBuilder.ReportingStructureNavigation;

            return ReneeOperationResult<UserDto?>.Success(new UserDto(
                solidarBuilder.Id, 
                solidarBuilder.LastName,
                solidarBuilder.FirstName,
                new ReportingStructureDto(
					reportingStructure != null ? reportingStructure.Id : Guid.Empty,
					reportingStructure != null ? reportingStructure.Name : string.Empty,
                    null)));
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<UserDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
}