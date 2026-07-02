using System.ComponentModel.DataAnnotations;
using Renee.Domain;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.Evaluations.ViewModel;

public class EvaluationsViewModel
{
	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredWellBeingRating)]
	public int? WellBeingRating { get; set; }
	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredEducationalFrameworkRating)]
	public int? EducationalFrameworkRating { get; set; }
	[Required(ErrorMessage = RealiseAndFollowMilestone.Errors.RequiredFamilySatisfactionWithSupport)]
	public int? FamilySatisfactionWithSupport { get; set; }
	public bool? IsBackToEmployment { get; set; }
}