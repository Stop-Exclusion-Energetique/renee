using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetAccompanyingFileForAnahSynthesisPdfQuery(
	string AccompanyingFileReference,
	Guid ConnectedUserId) : IQuery<ReneeOperationResult<GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>>;

public record GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult
{
	public required string StreetNumber { get; init; }
	public required string StreetName { get; init; }
	public required string PostalCode { get; init; }
	public required string City { get; init; }
	public DateTime? FirstVisitDate { get; init; }
	public bool HasHousingAdaptationWork { get; init; }
	public bool HasUnsanitaryExit { get; init; }
	public required string SolidarBuilderFullName { get; init; }
	public bool HasProjectTypeDegradation { get; init; }
	public bool HasProjectTypeIsolation { get; init; }
	public bool HasProjectTypeChangeOfHeatingSystem { get; init; }
	public bool HasProjectTypeReparation { get; init; }
	public required string SiretNumber { get; init; }
}