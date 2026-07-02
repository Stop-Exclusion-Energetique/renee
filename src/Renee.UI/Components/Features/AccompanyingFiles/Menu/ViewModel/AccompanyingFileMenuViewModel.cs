using Renee.UI.Components.Features.AccompanyingFiles.Menu.Billing.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.TargetInformations.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.ViewModel;

public class AccompanyingFileMenuViewModel
{
	public string AccompanyingFileReference { get; set; } = string.Empty;
	public required List<TaskViewModel> Tasks { get; set; }

	public required List<TaskViewModel> DoneTasks { get; set; }
	public required List<TaskViewModel> ToBeCompletedTask { get; set; }
	public required List<TaskViewModel> UpcomingTasks { get; set; }

	public required List<SupportTeamMemberViewModel> SupportTeamMembers { get; set; }
	public required bool ShouldDisplayTask { get; set; }
	[ValidateComplexType]
	public AccompanyingFileTargetDetailsViewModel? TargetInformation { get; set; } 
	public BillingLogViewModel? BillingLog { get; set; }
	public List<DocumentViewModel> Documents { get; set; } = [];
}