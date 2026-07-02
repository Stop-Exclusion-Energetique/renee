using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetRegisteredUserByEmailForDuplicateVerificationQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetRegisteredUserByEmailForDuplicateVerificationQuery, ReneeOperationResult<bool?>>
{
	public async Task<ReneeOperationResult<bool?>> Handle(
		GetRegisteredUserByEmailForDuplicateVerificationQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			return ReneeOperationResult<bool?>.Success(await userRepository.VerifyDuplicatedEmailAsync(request.Email));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}