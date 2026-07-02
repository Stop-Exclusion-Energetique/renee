using Renee.Application.DTOs.CopropertyProfile;

namespace Renee.UI.Components.Features.Coproperty.Stages.RealizeAndFollow.BasePage.ViewModel;

public class CopropertyRealizeAndFollowViewModel
{
    public DateTime? CollectiveWorksStartDate { get; set; }

    public DateTime? PlannedEndDate { get; set; }

    public double? ProgressPercentage { get; set; }

    public DateTime? ActualCompletionDate { get; set; }

    public double? InvoiceTotalAmount { get; set; }

    public string? FollowUpComment { get; set; }

    public static CopropertyRealizeAndFollowViewModel CreateViewModelFromDto(CopropertyProfileRealizeAndFollowDto dto)
        => new()
        {
            CollectiveWorksStartDate = dto.CollectiveWorksStartDate,
            PlannedEndDate = dto.PlannedEndDate,
            ProgressPercentage = dto.ProgressPercentage,
            ActualCompletionDate = dto.ActualCompletionDate,
            InvoiceTotalAmount = dto.InvoiceTotalAmount,
            FollowUpComment = dto.FollowUpComment
        };
}
