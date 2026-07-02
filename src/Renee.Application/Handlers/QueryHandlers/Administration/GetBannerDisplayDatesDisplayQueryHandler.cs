using MediatR;
using Renee.Application.Abstraction.Query;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Queries.Administration;

public class GetBannerDisplayDatesDisplayQueryHandler(IAdminConstantRepository _adminRepository) : QueryHandler<GetBannerDisplayDatesDisplayQuery, ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>>
{
    public override async Task<ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>> HandleQuery(GetBannerDisplayDatesDisplayQuery request)
    {
        var adminConstants = await _adminRepository.GetAll();

        var accompanyingFileAlertBannerStartDate = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileAlertBannerStartDate)?.AccompanyingFileModificationDeadline;
		var accompanyingFileAlertBannerEndDate = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileAlertBannerEndDate)?.AccompanyingFileModificationDeadline;
		var accompanyingFileAlertBannerMessage = adminConstants.FirstOrDefault(x => x.Name == IndexLabels.TzeeAccompanyingFileAlertBannerMessage)?.AccompanyingFileAlertBannerMessage;

        return ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>.Success(new GetBannerDisplayDatesDisplayQueryObjectResult
        {
            AccompanyingFileAlertBannerStartDate = accompanyingFileAlertBannerStartDate,
            AccompanyingFileAlertBannerEndDate = accompanyingFileAlertBannerEndDate,
            AccompanyingFileAlertBannerMessage = accompanyingFileAlertBannerMessage
        });
    }
}