using Microsoft.Identity.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Renee.Application.Interfaces;
using Renee.UI.Components.Features.Coproperty.List.ViewModel;
using Radzen;

namespace Renee.UI.Components.Features.Coproperty.List;

public partial class CreatedCopropertyProfilesByUser
{

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;

    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    [Inject] public IUserService? UserService { get; set; }

    public List<CreatedCopropertyProfileByUserViewModel> CreatedCopropertyProfileByUserViewModels { get; set; } = [];

    private string? _userRole;
    private Guid _userId;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        var userIdString = authState.User.GetNameIdentifierId();

        if (userIdString is null) return;

        if (Guid.TryParse(userIdString, out var userId))
        {
            var role = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (role is null) return;

            _userRole = role.Value;
            _userId = userId;

            await LoadCopropertyProfileDataAsync(_userRole, _userId);
        }
    }

    public async Task LoadCopropertyProfileDataAsync( string userRole, Guid? userId)
    {
        var result = await CopropertyProfileService.GetCopropertyProfileList(userRole, userId);
        if (!result.IsSuccess)
        {
            ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
        }
        else
        {
            var copropertyProfiles = result.Value?.CopropertyProfiles ?? [];
            CreatedCopropertyProfileByUserViewModels = copropertyProfiles.Select(
                cp => CreatedCopropertyProfileByUserViewModel.CreateViewModelFromResume(cp)).ToList();
        }
    }

    private void ShowNotification(NotificationMessage message)
    {
        NotificationService.Notify(message);
    }
}