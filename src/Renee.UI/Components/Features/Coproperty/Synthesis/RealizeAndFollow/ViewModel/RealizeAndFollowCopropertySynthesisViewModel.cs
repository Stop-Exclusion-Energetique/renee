using Renee.Application.DTOs.CopropertyProfile;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.ViewModel;

public class RealizeAndFollowCopropertySynthesisViewModel
{
    public string? Reference { get; init; }
    public AccompanyingFileStage Stage { get; init; }
    public AccompanyingFileStatus Status { get; init; }

    public bool IsInTzeeProgram { get; init; }
    public DateTime? CollectiveWorksStartDate { get; set; }

    public DateTime? PlannedEndDate { get; set; }

    public double? ProgressPercentage { get; set; }

    public DateTime? ActualCompletionDate { get; set; }

    public double? InvoiceTotalAmount { get; set; }

    public Guid? SolidarBuilder { get; set; }

    public Guid? SecondSolidarBuilder { get; set; }

    public Guid? ThirdSolidarBuilder { get; set; }

    public Guid? DiffuseCoordinator { get; set; }

    public Guid? TargetedCoordinator { get; set; }

    public Guid? TerritorialBuilder { get; set; }

    public Guid? SecondTerritorialBuilder { get; set; }

    public static RealizeAndFollowCopropertySynthesisViewModel CreateViewModelFromDto(CopropertyProfileRealizeAndFollowDto dto)
        => new()
        {
            Reference = dto.Reference,
            Stage = dto.Stage,
            Status = dto.Status,
            IsInTzeeProgram = dto.IsInTzeeProgram,
            CollectiveWorksStartDate = dto.CollectiveWorksStartDate,
            PlannedEndDate = dto.PlannedEndDate,
            ProgressPercentage = dto.ProgressPercentage,
            ActualCompletionDate = dto.ActualCompletionDate,
            InvoiceTotalAmount = dto.InvoiceTotalAmount,
            SolidarBuilder = dto.SolidarBuilderId,
            SecondSolidarBuilder = dto.SecondSolidarBuilderId,
            ThirdSolidarBuilder = dto.ThirdSolidarBuilderId,
            DiffuseCoordinator = dto.DiffuseCoordinatorId,
            TargetedCoordinator = dto.TargetedCoordinatorId,
            TerritorialBuilder = dto.TerritorialBuilderId,
            SecondTerritorialBuilder = dto.SecondTerritorialBuilderId
        };
}
