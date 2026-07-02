using Renee.Domain;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.ViewModel;

public class SiteSupervisionViewModel
{
	public DateTime? OverallStartDate { get; set; }

	public DateTime? EstimatedOverallCompletionDate { get; set; }

	public DateTime? ActualOverallEndDate { get; set; }

	[Range(0, 100, ErrorMessage = RealiseAndFollowMilestone.Errors.ProgressInvalidRange)]	
	public double? OverallProgress { get; set; }

	public DateTime? NextCoordinationMeetingScheduledFor { get; set; }

	public string? OverallObservations { get; set; }

	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredAnahGrantDate)]
	public DateTime? AnahGrantDate { get; set; }

	public DateTime? PreSiteSupervisionMeetingDate { get; set; }


	[ValidateComplexType]
	public List<WorkParticipantViewModel> WorkParticipants { get; init; } = [];
}