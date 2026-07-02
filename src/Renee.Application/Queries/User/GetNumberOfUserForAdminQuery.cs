using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetNumberOfUserForAdminQuery : IQuery<ReneeOperationResult<GetAdminDashboardDataQuery>>;

public record GetAdminDashboardDataQuery
{
    public int MaximalNumberOfAccompanyingFileCreated { get; init; }
    public int MaximalNumberOfAccompanyingFileToValidateFirstStage { get; init; }
    public int NumberOfUser { get; init; }
    public DateTime? AccompanyingFileModificationDeadline { get; init; }
    public DateTime? AccompanyingFileAlertBannerStartDate { get; init; }
    public DateTime? AccompanyingFileAlertBannerEndDate { get; init; }
    public string? AccompanyingFileAlertBannerMessage { get; init; }
}