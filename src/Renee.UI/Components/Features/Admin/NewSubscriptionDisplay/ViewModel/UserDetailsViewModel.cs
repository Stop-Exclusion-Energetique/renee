using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.Admin.NewSubscriptionDisplay.ViewModel;

public class UserDetailsViewModel
{
	[Required]
	public Guid UserId {get; set;}
	[Required]
	public string? FirstName{get; set;}
	[Required]
	public string? LastName{get; set;}
	[Required]
	public Guid? UserRoleId{get; set;}
	[Required]
	public Guid? ReportingStructureId{get; set;}
}
