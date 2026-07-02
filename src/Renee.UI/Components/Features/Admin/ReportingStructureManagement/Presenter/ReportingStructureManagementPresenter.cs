using Renee.Application.Queries.User;
using Renee.UI.Components.Features.Admin.ReportingStructureManagement.ViewModel;
using System.Runtime.CompilerServices;

namespace Renee.UI.Components.Features.Admin.ReportingStructureManagement.Presenter;

public class ReportingStructureManagementPresenter
{
	public List<ReportingStructureManagementViewModel> ViewModel { get; set; } = new List<ReportingStructureManagementViewModel>();

	public ReportingStructureManagementPresenter FromQuery(List<GetUntrackedReportingStructureForAdminQueryObjectResult> queryObjectResults)
	{
		ViewModel = queryObjectResults
				.Select(x =>
					new ReportingStructureManagementViewModel
					{
						ReportingStructureName = x.ReportingStructureName,
						SubscriptionDate = x.SubscriptionDate,
						UserId = x.UserId
					})
				.ToList();
		return this;
	}

	public List<ReportingStructureManagementViewModel> Present() => ViewModel;
}
