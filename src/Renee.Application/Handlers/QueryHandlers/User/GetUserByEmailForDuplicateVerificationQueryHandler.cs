using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetUserByEmailForDuplicateVerificationQueryHandler(
	IUnregisteredUserRepository unregisteredUserRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetUserByEmailForDuplicateVerificationQuery, ReneeOperationResult<bool?>>
{
	public async Task<ReneeOperationResult<bool?>> Handle(
		GetUserByEmailForDuplicateVerificationQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			return ReneeOperationResult<bool?>.Success(await unregisteredUserRepository.VerifyDuplicateEmailAsync(request.Email));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}