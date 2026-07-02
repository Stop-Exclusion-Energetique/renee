using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Blazored.Modal;
using Radzen;
using Renee.Application.Interfaces;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.Components.Modal;

namespace Renee.UI.Components.Features.Coproperty.SupportTeamMenu;

public partial class SupportTeamMenu
{

    public required List<SupportTeamMemberViewModel> SupportTeamMembers { get; set; }
    [Parameter] public Guid CopropertyProfileId { get; set; } = Guid.Empty;

    [Inject] public IModalService ModalService { get; set; } = null!;

    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;

    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    private Guid _userId;
    private string _userRole = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;
        var userIdClaims = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (Guid.TryParse(userIdClaims, out var userId) && role != null)
        {
            _userId = userId;
            _userRole = role;
        }

        await LoadData();
    }

    public async Task ShowUpdateSupportTeamMemberModal(string supportTeamMemberRole)
    {
        var modal = ModalService.Show<UpdateSupportTeamMemberModal>(
            new ModalParameters
            {
            { nameof(UpdateSupportTeamMemberModal.ConnectedUserId), _userId },
            { nameof(UpdateSupportTeamMemberModal.AssociatedResourceId), CopropertyProfileId },
            { nameof(UpdateSupportTeamMemberModal.SupportTeamMemberRole), supportTeamMemberRole },
            {nameof(UpdateSupportTeamMemberModal.IsCoproperty), true }
            },
            new ModalOptions
            {
                Position = ModalPosition.Middle,
                HideCloseButton = true,
                HideHeader = true,
                Size = ModalSize.Large
            });

        var result = await modal.Result;
        if (result.Confirmed) await LoadData();
    }

    private async Task LoadData()
    {
        var result = await CopropertyProfileService.GetAllCopropertyProfileSupportTeam(CopropertyProfileId, _userId);

        if (!result.IsSuccess || result.Value is null)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
            return;
        }

        SupportTeamMembers = result.Value.SupportTeamMembers.Select(
            qr => new SupportTeamMemberViewModel
            {
                FullName = qr.FullName,
                Email = qr.Email,
                PhoneNumber = qr.PhoneNumber,
                Role = qr.Role,
                IsEditable = qr.IsEditable
            }).ToList();

        StateHasChanged();
    }
}
