using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.WorkPackage;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetAccompanyingFileForWorkCertificatePdfQuery(
	string AccompanyingFileReference, 
	Guid ConnectedUserId) : IQuery<ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>>;

public class GetAccompanyingFileForWorkCertificatePdfQueryObjectResult
{
	public required string OccupantFullName { get; set; }
	public required string StreetNumber { get; set; }
	public required string StreetName { get; set; }
	public required string PostalCode { get; set; }
	public required string City { get; set; }
	public double WorkPackagesTotalCostIncludingAllTaxes { get; set; }
	public List<WorkPackageDto> WorkPackages { get; set; } = [];
	public double? EstimatedAnnualGesEmissionsBeforeWork { get; set; }
	public string? EstimatedEnergyDpeBeforeWork { get; set; }
	public double? AnnualEnergyConsumptionBeforeWork { get; set; }
	public double? Surface { get; set; }
	public double? EstimatedAnnualGesEmissionsAfterWork { get; set; }
	public string? EstimatedEnergyDpeAfterWork { get; set; }
	public double? AnnualEnergyConsumptionAfterWork { get; set; }
	public int? EstimatedDpeClassJump { get; set; }
	public required string SolidarBuilderFullName { get; set; }
	public string? SocialContext { get; set; }
	public required string SiretNumber { get; set; }
}