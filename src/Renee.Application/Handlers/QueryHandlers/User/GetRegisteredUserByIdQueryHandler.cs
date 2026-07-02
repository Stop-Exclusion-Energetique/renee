using MediatR;
using Renee.Application.DTOs.User;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetRegisteredUserByIdQueryHandler(IUserRepository userRepository, ITelemetryService telemetryService)
	: IRequestHandler<GetRegisteredUserByIdQuery, ReneeOperationResult<UserDto?>>
{
	public async Task<ReneeOperationResult<UserDto?>> Handle(GetRegisteredUserByIdQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var userEntity = await userRepository.GetUserById(request.Id);

			if (userEntity?.ReportingStructureId is null) return ReneeOperationResult<UserDto?>.Success(null);

			return ReneeOperationResult<UserDto?>.Success(new UserDto(
				userEntity.Id,
				new UserPersonnalInformations(
					userEntity.Email,
					userEntity.FirstName,
					userEntity.LastName,
					userEntity.PhoneNumber,
					userEntity.SiretNumber),
				userEntity.TerritoryId,
				new ReportingStructureDto(
					(Guid)userEntity.ReportingStructureId, 
					userEntity.ReportingStructureNavigation?.Name ?? string.Empty, 
					userEntity.ReportingStructureNavigation?.NationalStructureId)));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<UserDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}