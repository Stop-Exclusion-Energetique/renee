using Renee.Application.DTOs.WorkPackage;
using Renee.Domain.Enums;

namespace Renee.Application.DTOs.CopropertyProfile;

public class CopropertyProfileOrganizeAndFinanceDto
{
    public Guid Id { get; init; }

    public string? Reference { get; init; }

    public AccompanyingFileStage Stage { get; init; }

    public AccompanyingFileStatus Status { get; init; }

    public DateTime? CreationDatetimeUtc { get; set; }

    public DateTime? LastUpdateDatetimeUtc { get; set; }

    public bool IsInTzeeProgram { get; init; }

    public DateTime? DateOfAgVote { get; set; }

    public double? MprCoproAids { get; set; }

    public double? ComplementaryAids { get; set; }

    public List<WorkPackageDto>? WorkPackages { get; set; }

    public Guid? SolidarBuilderId { get; set; }

    public Guid? TerritorialBuilderId { get; set; }

    public Guid? SecondSolidarBuilderId { get; set; }

    public Guid? ThirdSolidarBuilderId { get; set; }

    public Guid? DiffuseCoordinatorId { get; set; }

    public Guid? TargetedCoordinatorId { get; set; }

    public Guid? SecondTerritorialBuilderId { get; set; }
}
