using Renee.Application.DTOs.User;
using Renee.Application.Queries.User;

namespace Renee.UI.Components.Features.Admin.NewSubscriptionDisplay.ViewModel;

public class AccountManagementViewModel
{
	public List<UserDto> SuscribedUser { get; set; } = [];
	public List<GetAllRegisteredQyeryObjectResult> RegisteredUser { get; set; } = [];
}
