using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Radzen.Blazor;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AccompanyingFiles.AnahGrantCheck;

public partial class AccompanyingFilesAwaitingAnahResponseByUser
{
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;

	private RadzenDataGrid<AccompanyingFileAwaitingAnahResponseResume> grid = default!;

	private readonly List<ZeeSelectItem<bool?>> _grantStatusesOptions =
	[
		new ZeeSelectItem<bool?>(Labels.PendingAnahGrant, false),
		new ZeeSelectItem<bool?>(Labels.GrantReceived, true)
	];
	public List<AccompanyingFileAwaitingAnahResponseResume> AccompanyingFilesAwaitingAnahResponse { get; set; } = [];

	private Dictionary<Guid, bool?> AccompanyingFilesGrantStatuses { get; set; } = [];
	private Dictionary<Guid, DateTime?> AccompanyingFilesGrantDates { get; set; } = [];
	private Guid _userId;

	private bool CanSubmit => AccompanyingFilesAwaitingAnahResponse.All(f => IsRowValid(f.AccompanyingFileId));

	protected override void OnParametersSet()
	{
		foreach (var accompanyingFileId in AccompanyingFilesAwaitingAnahResponse.Select(af => af.AccompanyingFileId))
		{
			if (!AccompanyingFilesGrantStatuses.ContainsKey(accompanyingFileId))
			{
				AccompanyingFilesGrantStatuses[accompanyingFileId] = null;
			}

			if (!AccompanyingFilesGrantDates.ContainsKey(accompanyingFileId))
			{
				AccompanyingFilesGrantDates[accompanyingFileId] = null;
			}
		}
	}

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;
		var userIdClaims = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (!Guid.TryParse(userIdClaims, out var userId))
			return;

		_userId = userId;

		var pendingFilesResult = await AccompanyingFileService.GetAccompanyingFilesAwaitingAnahResponse(_userId);

		if (!pendingFilesResult.IsSuccess || pendingFilesResult.Value is null || pendingFilesResult.Value.Count == 0)
		{
			NavigationManager.NavigateTo(Endpoints.ErrorPage, false);
			return;
		}

		AccompanyingFilesAwaitingAnahResponse = pendingFilesResult.Value;
	}
    
    private void OnGrantStatusChanged(Guid fileId)
    {
		if (AccompanyingFilesGrantStatuses[fileId] != true)
		{
			AccompanyingFilesGrantDates[fileId] = null;
		}

		StateHasChanged();
	}
	    
    private bool IsRowValid(Guid fileId)
    {
		if (!AccompanyingFilesGrantStatuses.TryGetValue(fileId, out var status))
			return false;

		if (status is false)
			return true;

		return AccompanyingFilesGrantDates[fileId] is not null;
	}
           
    private async Task ValidateAndClose()
    {
		var updates = AccompanyingFilesGrantDates.Select(af => new AccompanyingFileGrantDateResume(af.Key, af.Value)).ToList();
		var result = await AccompanyingFileService.UpdateAccompanyingFilesForAnahGrantCheck(_userId, updates);

		if (result.IsSuccess && result.Value)
			NavigationManager.NavigateTo(Endpoints.Base, false);

		NotificationService.Notify(new NotificationMessage
		{
			Severity = result.IsSuccess && result.Value ? NotificationSeverity.Success : NotificationSeverity.Error,
			Summary = result.Message,
			Duration = 4000
		});
	}
}