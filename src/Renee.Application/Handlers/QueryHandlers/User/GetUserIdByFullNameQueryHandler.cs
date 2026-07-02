using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetUserIdByFullNameQueryHandler(
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetUserIdByFullNameQuery, ReneeOperationResult<Guid?>>
{
	public async Task<ReneeOperationResult<Guid?>> Handle(GetUserIdByFullNameQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var splittedFullname = request.Fullname.Split(' ');
			return ReneeOperationResult<Guid?>.Success(await userRepository.GetSolidarBuilderUserId(splittedFullname[0], splittedFullname[1]));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<Guid?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}