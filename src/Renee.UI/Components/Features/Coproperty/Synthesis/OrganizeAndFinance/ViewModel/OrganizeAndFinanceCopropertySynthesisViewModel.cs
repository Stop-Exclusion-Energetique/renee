using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.DTOs.WorkPackage;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.OrganizeAndFinance.ViewModel;

public class OrganizeAndFinanceCopropertySynthesisViewModel
{
    public string? Reference { get; init; }
    public AccompanyingFileStage Stage { get; init; }
    public AccompanyingFileStatus Status { get; init; }
    public bool IsInTzeeProgram { get; init; }
    public DateTime? DateOfAgVote { get; set; }

    public double? MprCoproAids { get; set; }

    public double? ComplementaryAids { get; set; }

    public List<WorkPackageSummaryDto> WorkPackageSummary { get; init; } = [];
    public double WorkPackagesTotalPrice =>
    WorkPackageSummary?.Sum(wp => wp.WorkPackageTotalCost) ?? 0;

    public Guid? SolidarBuilder { get; set; }

    public Guid? SecondSolidarBuilder { get; set; }

    public Guid? ThirdSolidarBuilder { get; set; }

    public Guid? DiffuseCoordinator { get; set; }

    public Guid? TargetedCoordinator { get; set; }

    public Guid? TerritorialBuilder { get; set; }

    public Guid? SecondTerritorialBuilder { get; set; }

    public static OrganizeAndFinanceCopropertySynthesisViewModel CreateViewModelFromDto(CopropertyProfileOrganizeAndFinanceDto dto)
    => new()
    {
        Reference = dto.Reference,
        Stage = dto.Stage,
        Status = dto.Status,
        IsInTzeeProgram = dto.IsInTzeeProgram,
        DateOfAgVote = dto.DateOfAgVote,
        MprCoproAids = dto.MprCoproAids,
        ComplementaryAids = dto.ComplementaryAids,
        WorkPackageSummary = dto.WorkPackages?.Select(wp =>
            new WorkPackageSummaryDto(
                wp.Id,
                wp.TypeCostDtos is not null
                    ? string.Join(", ", wp.TypeCostDtos.Select(tc => tc.Description))
                    : string.Empty,
                wp.TypeCostDtos?.Sum(tc => tc.Cost ?? 0) ?? 0
            )
        ).ToList() ?? [],
        SolidarBuilder = dto.SolidarBuilderId,
        SecondSolidarBuilder = dto.SecondSolidarBuilderId,
        ThirdSolidarBuilder = dto.ThirdSolidarBuilderId,
        DiffuseCoordinator = dto.DiffuseCoordinatorId,
        TargetedCoordinator = dto.TargetedCoordinatorId,
        TerritorialBuilder = dto.TerritorialBuilderId,
        SecondTerritorialBuilder = dto.SecondTerritorialBuilderId
    };
}
