using Renee.Application.Queries.User;
using Renee.UI.Components.Features.Admin.DeletedAccountManagement.ViewModel;

namespace Renee.UI.Components.Features.Admin.DeletedAccountManagement.Presenter;

public class DeletedAccountManagementPresenter
{
	public List<DeletedAccountManagementViewModel> ViewModel { get; set; } = [];

	public DeletedAccountManagementPresenter FromQuery(List<GetAllUntrackedAccountDeletionRequestsForAdminQueryObjectResult> queryObjectResults)
	{
		ViewModel = queryObjectResults
				.Select(x =>
					new DeletedAccountManagementViewModel
					{
						FirstName = x.FirstName,
						LastName = x.LastName,
						Role = x.Role,
						ReportingStructureName = x.ReportingStructureName,
						UserId = x.UserId
					})
				.ToList();
		return this;
	}

	public List<DeletedAccountManagementViewModel> Present() => ViewModel;
}