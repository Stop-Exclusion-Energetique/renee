using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Queries.User;
using Renee.UI.Components.Features.Admin.ReportingStructureManagement.Modal;
using Renee.UI.Components.Features.Admin.ReportingStructureManagement.Presenter;
using Renee.UI.Components.Features.Admin.ReportingStructureManagement.ViewModel;

namespace Renee.UI.Components.Features.Admin.ReportingStructureManagement;

public partial class ReportingStructureManagement
{
	[Inject] ISendEventQuery Sender { get; set; } = default!;
	[Inject] IModalService ModalService { get; set; } = default!;

	private List<ReportingStructureManagementViewModel> ReportingStructureList { get; set; } = new List<ReportingStructureManagementViewModel>();

	protected override async Task OnInitializedAsync()
	{
		await LoadReportingStructures();
	}

	private async Task AddReportingStructure(Guid userId, string reportingStructureName)
	{
		var parameters = new ModalParameters();
		parameters.Add(nameof(ReportingStructureManagementModal.UserId), userId);
		parameters.Add(nameof(ReportingStructureManagementModal.ReportingStructureName), reportingStructureName);

		var modal = ModalService.Show<ReportingStructureManagementModal>(parameters, new ModalOptions { Position = ModalPosition.Middle });

		var result = await modal.Result;

		if(result.Confirmed)
		{
			await LoadReportingStructures();
		}			
	}

	public async Task LoadReportingStructures()
	{
		ReportingStructureList = (new ReportingStructureManagementPresenter().FromQuery((await Sender.Send(new GetUntrackedReportingStructureForAdminQuery())).Value!)).Present();
		StateHasChanged();
	}
}