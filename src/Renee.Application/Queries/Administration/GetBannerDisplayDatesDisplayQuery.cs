using MediatR;
using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Administration;

public record GetBannerDisplayDatesDisplayQuery() : IQuery<ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>>;

public record GetBannerDisplayDatesDisplayQueryObjectResult
{
    public DateTime? AccompanyingFileAlertBannerStartDate { get; init; }
    public DateTime? AccompanyingFileAlertBannerEndDate { get; init; }
    public string? AccompanyingFileAlertBannerMessage { get; init; }
}