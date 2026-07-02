using MediatR;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetUserByIdQueryHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetUserByIdQuery, ReneeOperationResult<UserDto?>>
{
	public async Task<ReneeOperationResult<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var userEntity = await unregisteredUserRepository.GetUserById(request.Id);
			if (userEntity is null) return ReneeOperationResult<UserDto?>.Success(null);
			var userDto = new UserDto(
				userEntity.Id,
				userEntity.SubscriptionDateUtc,
				userEntity.LastName,
				userEntity.FirstName,
				userEntity.Email);
			userDto = userDto.SetDataForSignIn(
				userEntity.AskedRole,
				userEntity.Function,
				new ReportingStructureDto(userEntity.ReportingStructureId, userEntity.ReportingStructureNavigation.Name, userEntity.ReportingStructureNavigation.NationalStructureId),
				userEntity.ReportingStructure,
				userEntity.TerritoryId);
			userDto = userDto.SetPersonnalInformations(
				userEntity.PhoneNumber,
				userEntity.SiretNumber);
			userDto = userDto.SetCguVersion(userEntity.CguVersion);

			return ReneeOperationResult<UserDto?>.Success(userDto);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<UserDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}