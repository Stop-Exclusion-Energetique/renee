using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetAllAccompanyingFilesAwaitingAnahResponseQuery(Guid UserId) : IRequest<ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>>;

public record AccompanyingFileAwaitingAnahResponseResume
{
	public  Guid AccompanyingFileId { get; set; }
	public string AccompanyingFileReference { get; set; } = string.Empty;
	public string OccupantFullName { get; set; } = string.Empty;
	public string HousingAddress { get; set; } = string.Empty;
}