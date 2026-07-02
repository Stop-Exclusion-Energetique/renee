using MediatR;
using Renee.Application.Commands.User;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.User;

public class CreateUserCommandHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateUserCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (!request.UserDto.AskedRole.HasValue ||
				request.UserDto.ReportingStructureDto?.Id == Guid.Empty)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.InvalidUserData);
			var user = new UnregisteredUser
			{
				UserState = (int)UserState.IsNew,
				AskedRole = request.UserDto.AskedRole.Value,
				Email = request.UserDto.Email,
				FirstName = request.UserDto.FirstName,
				Function = request.UserDto.Function,
				Id = request.UserDto.Id,
				LastName = request.UserDto.LastName,
				PhoneNumber = request.UserDto.PhoneNumber,
				ReportingStructureId = request.UserDto.ReportingStructureDto?.Id ?? Guid.NewGuid(),
				ReportingStructure = request.UserDto.OtherReportingStructure,
				SubscriptionDateUtc = request.UserDto.SubscriptionDateUtc,
				TerritoryId = request.UserDto.TerritoryId,
				SiretNumber = request.UserDto.SiretNumber,
				CguVersion = request.UserDto.CguVersion,
			};
			var numberItemsChanged = await unregisteredUserRepository.AddUser(user);

			return numberItemsChanged > -1
				? ReneeOperationResult<bool>.Success(true, Labels.CreateUserSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileCreatingUser);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}