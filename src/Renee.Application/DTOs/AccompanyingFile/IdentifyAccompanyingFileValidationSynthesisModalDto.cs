using Renee.Domain.Enums;

namespace Renee.Application.DTOs.AccompanyingFile;

public sealed class IdentifyAccompanyingFileValidationSynthesisModalDto
{
	public Guid AccompanyingFileId { get; set; }
	public string BlockingProofComment { get; init; } = null!;
	public ICollection<Guid> BlockingProofIds { get; init; } = null!;
	public AccompanyingFileStatus Status { get; init; }
	public Guid UserId { get; set; }
	public bool? IsIncludedInTzeeProgram { get; set; }
}