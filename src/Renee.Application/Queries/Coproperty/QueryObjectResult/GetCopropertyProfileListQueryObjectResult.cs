using Renee.Domain.Enums;

namespace Renee.Application.Queries.Coproperty.QueryObjectResult;

public class GetCopropertyProfileListQueryObjectResult
{
    public List<CopropertyProfileResume> CopropertyProfiles { get; init; } = [];
}

public class CopropertyProfileResume
{
    public Guid Id { get; init; }
    public string Reference { get; init; } = null!;
    public string? Address { get; init; }
    public AccompanyingFileStage Stage { get; init; }
    public AccompanyingFileStatus Status { get; init; }
    public DateTime LastUpdatedDateUtc { get; init; }
    public DateTime? CreateDateUtc { get; init; }
    public Guid SolidarBuilder { get; set; }
    public Guid? SecondSolidarBuilder { get; set; }
    public Guid? ReportingStructureId { get; set; }
    public Guid? DiffuseCoordinatorId { get; set; }
    public Guid? TargetCoordinatorId { get; set; }
    public Guid? TerritorialBuilder { get; set; }
    public Guid? SecondTerritorialBuilder { get; set; }
    public Guid? TerritoryId { get; set; }
    public Guid? ThirdSolidarBuilder { get; set; }
}
