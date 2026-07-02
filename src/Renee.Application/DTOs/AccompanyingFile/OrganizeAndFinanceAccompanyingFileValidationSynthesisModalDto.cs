using Renee.Domain.Enums;

namespace Renee.Application.DTOs.AccompanyingFile;

public class OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto
{
	public Guid AccompanyingFileId { get; set; }
	public string? AnahCategory { get; init; }
	public string? OwnershipStatus { get; init; }
	public string? DpeLabel { get; init; }
	public string? GesLabel { get; init; }
	public string BlockingProofComment { get; init; } = null!;
	public ICollection<Guid> BlockingProofIds { get; init; } = null!;
	public AccompanyingFileStatus Status { get; set; }
	public bool? IsIncludedInTzeeProgram { get; set; }
}