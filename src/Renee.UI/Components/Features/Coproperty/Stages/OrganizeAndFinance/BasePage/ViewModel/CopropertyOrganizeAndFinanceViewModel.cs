using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.DTOs.WorkPackageWorkTypeCost;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.WorkPackage.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.Coproperty.Stages.OrganizeAndFinance.BasePage.ViewModel;

public class CopropertyOrganizeAndFinanceViewModel
{
    public DateTime? DateOfAgVote { get; set; }

    public double? MprCoproAids { get; set; }

    public double? ComplementaryAids { get; set; }

    [ValidateComplexType] public List<WorkPackageViewModel> WorkPackages { get; init; } = [];

    public double? RecommendedWorkTotalPrice => WorkPackages.Sum(wp => wp.TotalPrice);

    public static CopropertyOrganizeAndFinanceViewModel CreateViewModelFromDto(CopropertyProfileOrganizeAndFinanceDto dto)
        => new()
        {
            DateOfAgVote = dto.DateOfAgVote,
            MprCoproAids = dto.MprCoproAids,
            ComplementaryAids = dto.ComplementaryAids,
            WorkPackages = dto.WorkPackages?.Select(wp => CreateWorkPackageViewModel(wp)).ToList() ?? []
        };
    private static WorkPackageViewModel CreateWorkPackageViewModel(WorkPackageDto dto) =>
        new()
        {
            Id = dto.Id,
            EnergeticsEffectOfWorks = dto.EnergeticsEffectOfWorks,
            WorkTypes = dto.TypeCostDtos?.Select(CreateWorkType).ToList() ?? []
        };

    private static WorkPackageViewModel.WorkType CreateWorkType(WorkPackageWorkTypeCostDto dto) =>
        new(dto.Id, string.Empty, false) { Description = dto.Description, Price = dto.Cost };
}
