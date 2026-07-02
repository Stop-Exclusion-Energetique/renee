using Renee.Domain.Enums;

namespace Renee.Application.DTOs.CopropertyProfile;

public class CopropertyProfileRealizeAndFollowDto
{
    public Guid Id { get; init; }
    public string? Reference { get; init; }

    public AccompanyingFileStage Stage { get; init; }

    public AccompanyingFileStatus Status { get; init; }

    public DateTime? CreationDatetimeUtc { get; set; }

    public DateTime? LastUpdateDatetimeUtc { get; set; }

    public bool IsInTzeeProgram { get; init; }

    public DateTime? CollectiveWorksStartDate { get; set; }

    public DateTime? PlannedEndDate { get; set; }

    public double? ProgressPercentage { get; set; }

    public DateTime? ActualCompletionDate { get; set; }

    public double? InvoiceTotalAmount { get; set; }

    public string? FollowUpComment { get; set; }

    public Guid? SolidarBuilderId { get; set; }

    public Guid? TerritorialBuilderId { get; set; }

    public Guid? SecondSolidarBuilderId { get; set; }

    public Guid? ThirdSolidarBuilderId { get; set; }

    public Guid? DiffuseCoordinatorId { get; set; }

    public Guid? TargetedCoordinatorId { get; set; }

    public Guid? SecondTerritorialBuilderId { get; set; }
}
