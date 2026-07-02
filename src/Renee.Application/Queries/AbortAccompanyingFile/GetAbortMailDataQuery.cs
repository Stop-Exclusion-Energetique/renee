using MediatR;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AbortAccompanyingFile;

public record GetAbortMailDataQuery(Guid AccompanyingFileId, bool IsMailForSolidarBuilder) : IRequest<ReneeOperationResult<AbortMailData>>;

public class AbortMailData
{
	public string AccompanyingFileReference { get; init; } = string.Empty;
	public AccompanyingFileStage AccompanyingFileStage { get; init; }
	public bool IsBillingRequested { get; init; }
	public string SolidarBuilderFullName { get; init; } = string.Empty;
	public string? ValidatorFullName { get; set; } = string.Empty;
	public string RecipientEmail { get; set; } = string.Empty;
}