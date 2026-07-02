using Renee.Domain;
using Renee.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.ViewModel;

public class WorkParticipantViewModel
{
	public Guid Id { get; init; }

	public ParticipantType? ParticipantType { get; set; }

	public string? WorkTypesLabel { get; set; }

	public string? ParticipantName { get; set; } = string.Empty;

	public string? ContactAdvisor { get; set; }

	public DateTime? StartDateOfWork { get; set; }

	public DateTime? EstimatedCompletionDate { get; set; }

	public DateTime? ActualEndDate { get; set; }

	[Range(0, 100, ErrorMessage = RealiseAndFollowMilestone.Errors.ProgressInvalidRange)]
	public double? Progress { get; set; }

	public WorkQuality? WorkQuality { get; set; }

	public string? CommentOnWorkQuality { get; set; }

	public List<Guid?> WorkParticipantDifficulties { get; set; } = [];

	public string? CommentOnWorkParticipantDifficulties { get; set; }

	public string? SpecificComments { get; set; }
}