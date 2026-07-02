using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.User;

public class GetNumberOfUserForAdminQueryHandler(IUserRepository userRepository,IAdminConstantRepository adminConstantRepository,  ITelemetryService telemetryService) : IRequestHandler<GetNumberOfUserForAdminQuery, ReneeOperationResult<GetAdminDashboardDataQuery>>
{
	public async Task<ReneeOperationResult<GetAdminDashboardDataQuery>> Handle(GetNumberOfUserForAdminQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var numberOfUser = await userRepository.CountAllUser();
			var adminConstants = await adminConstantRepository.GetAll();
			var maximalNumberOfAccompanyingFileCreated = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.MaximalNumberOfAccompanyingFileCreated)?.Value ?? 0;
			var maximalNumberOfAccompanyingFileToValidateFirstStage = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.MaximalNumberOfAccompanyingFileToValidateFirstStage)?.Value ?? 0;
			var accompanyingFileModificationDeadline = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileModificationDeadline)?.AccompanyingFileModificationDeadline;
			var accompanyingFileAlertBannerStartDate = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileAlertBannerStartDate)?.AccompanyingFileModificationDeadline;
			var accompanyingFileAlertBannerEndDate = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileAlertBannerEndDate)?.AccompanyingFileModificationDeadline;
			var accompanyingFileAlertBannerMessage = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileAlertBannerMessage)?.AccompanyingFileAlertBannerMessage;

			var result = new GetAdminDashboardDataQuery
			{
				MaximalNumberOfAccompanyingFileCreated = maximalNumberOfAccompanyingFileCreated,
				MaximalNumberOfAccompanyingFileToValidateFirstStage = maximalNumberOfAccompanyingFileToValidateFirstStage,
				NumberOfUser = numberOfUser,
				AccompanyingFileModificationDeadline = accompanyingFileModificationDeadline,
				AccompanyingFileAlertBannerStartDate = accompanyingFileAlertBannerStartDate,
				AccompanyingFileAlertBannerEndDate = accompanyingFileAlertBannerEndDate,
				AccompanyingFileAlertBannerMessage = accompanyingFileAlertBannerMessage
			};

			return ReneeOperationResult<GetAdminDashboardDataQuery>.Success(result);
		}
		catch (Exception ex) 
		{
			await telemetryService.TrackExceptionAsync(ex);
			return ReneeOperationResult<GetAdminDashboardDataQuery>.Failure(IndexLabels.Error.ErrorWhileLoadingIndexData);
		}
	}
}
