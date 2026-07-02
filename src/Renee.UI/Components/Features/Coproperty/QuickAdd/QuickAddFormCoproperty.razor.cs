using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using Renee.UI.Components.Features.Coproperty.QuickAdd.ViewModels;
using Renee.Application.Queries.AccompanyingFile;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;
using Renee.Domain;
using Renee.Domain.Enums;
using Radzen;
using Renee.Application.CommandsUseCasesInput;

namespace Renee.UI.Components.Features.Coproperty.QuickAdd;

public partial class QuickAddFormCoproperty
{

    public QuickAddFormCopropertyViewModel QuickAddFormViewModel { get; } = new();
    public List<AddressDto> AddressList { get; set; } = null!;

    [Inject] public IAddressService AddressService { get; set; } = null!;

    [Inject] public IModalService? ModalService { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;

    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    [Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;

    [Parameter] public Guid? AccompanyingFileId { get; set; }

    private EditContext? _quickAddCopropertyEditContext;

    public List<ZeeSelectItem<Guid?>>? SolidarBuilderUsers { get; set; }
    private List<ZeeSelectItem<Guid?>>? TerritorialBuilderUsers { get; set; }
    private List<ZeeSelectItem<Guid?>>? DiffuseCoordinatorUsers { get; set; }
    private List<ZeeSelectItem<Guid?>>? TargetCoordinator { get; set; }
    private List<ZeeSelectItem<Guid?>>? Territory { get; set; }
    private Guid _userId;
    private Guid? _userTerritory;
    private bool? IsCustomAddress { get; set; } = false;
    private List<QuickAddUserResult> _territorialBuilderUsersResults = [];
    private List<Guid> _relatedAccompanyingFilesIds = [];


    protected override async Task OnInitializedAsync()
    {
        _quickAddCopropertyEditContext = new EditContext(QuickAddFormViewModel);

        var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

        var userIdClaims = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        _userId = Guid.TryParse(userIdClaims, out var userId) ? userId : Guid.Empty;

        _userTerritory = Guid.TryParse(claims.FirstOrDefault(c => c.Type == ClaimTypes.Country)?.Value, out var territoryId) ? territoryId : null;

        var UserRoleValue = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        if (UserRoleValue.Equals(Constants.SolidarBuilderRole)) QuickAddFormViewModel.ReferentSolidarBuilderId = _userId;

        await LoadQuickAddData(IsUserTeritorialBuilderOrCoordinator(UserRoleValue));
    }


    public void OnAccompanyingTypeChanged()
    {
        if (QuickAddFormViewModel.ZeroEnergyExclusionTerritoriesProgram == false)
        {
            QuickAddFormViewModel.AccompanyingType = null;
            QuickAddFormViewModel.ReferentEtId = null;
            QuickAddFormViewModel.SecondReferentEtId = null;
            QuickAddFormViewModel.ReferentTargetCoordinator = null;
            QuickAddFormViewModel.Territory = null;
            QuickAddFormViewModel.ReferentDiffuseCoordinator = null;
        }

        if (QuickAddFormViewModel.AccompanyingType == AccompanyingType.Diffuse)
        {
            QuickAddFormViewModel.ReferentEtId = null;
            QuickAddFormViewModel.SecondReferentEtId = null;
            QuickAddFormViewModel.ReferentTargetCoordinator = null;
            QuickAddFormViewModel.Territory = null;

            QuickAddFormViewModel.ReferentDiffuseCoordinator = DiffuseCoordinatorUsers?
                .Where(dc => dc.Label != null && dc.Label.Contains(Constants.DefaultDiffuseCoordinatorFirstName))
                .Select(dc => dc.Value)
                .FirstOrDefault();
        }
        else
        {
            QuickAddFormViewModel.ReferentDiffuseCoordinator = null;

            QuickAddFormViewModel.ReferentTargetCoordinator = TargetCoordinator?
                .Where(tc => tc.Label != null && tc.Label.Contains(Constants.DefaultTerritorialCoordinatorFirstName))
                .Select(tc => tc.Value)
                .FirstOrDefault();

            if (_userTerritory is not null)
            {
                QuickAddFormViewModel.Territory = Territory?
                .Where(t => t.Value == _userTerritory)
                .Select(t => t.Value)
                .FirstOrDefault();

                var matchingTerritorialBuilder = _territorialBuilderUsersResults.FirstOrDefault(tb => tb.TerritoryId == _userTerritory);
                var matchingSecondTerritorialBuilder = _territorialBuilderUsersResults.FirstOrDefault(tb => tb.TerritoryId == _userTerritory && tb.UserId != matchingTerritorialBuilder?.UserId);

                if (matchingTerritorialBuilder is not null)
                    QuickAddFormViewModel.ReferentEtId = matchingTerritorialBuilder.UserId;

                if (matchingSecondTerritorialBuilder is not null)
                    QuickAddFormViewModel.SecondReferentEtId = matchingSecondTerritorialBuilder.UserId;

            }
        }
    }


    private string GetSolidarBuilderFullNameFormated()
    {
        var matchingSolidarBuilder =
            SolidarBuilderUsers?.Find(es => es.Value.Equals(QuickAddFormViewModel.ReferentSolidarBuilderId));

        if (matchingSolidarBuilder == null) return "";

        var solidarBuilderFullName = matchingSolidarBuilder.Label?.Split(" ");
        var (solidarBuilderLastName, solidarBuilderFirstName) =
            (solidarBuilderFullName?[0], solidarBuilderFullName?[1]);

        return
            $"{char.ToUpper(solidarBuilderFirstName![0]) + solidarBuilderFirstName[1..]} {solidarBuilderLastName?.ToUpper()}";
    }

    private async Task LoadAddresses(LoadDataArgs? args)
    {
        AddressList = !string.IsNullOrEmpty(args?.Filter)
            ? [.. await OnValueChangedAddress(args.Filter) ?? []]
            : [];
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadQuickAddData(bool shouldRetrieveFakeUser)
    {
        var userForQuickAdd = (await SendEventQuery.Send(new GetQuickAddChoiceDataQuery { ShouldRetrieveFakeUser = shouldRetrieveFakeUser })).Value!;
        SolidarBuilderUsers = userForQuickAdd.SolidarBuilders
            .Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();

        _territorialBuilderUsersResults = userForQuickAdd.TerritorialBuilders;
        TerritorialBuilderUsers = userForQuickAdd.TerritorialBuilders.Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId))
            .ToList();

        DiffuseCoordinatorUsers = userForQuickAdd.DiffuseCoordinators
            .Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();
        TargetCoordinator = userForQuickAdd.TargetCoordinators
            .Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();
        Territory = userForQuickAdd.Territories.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId))
            .ToList();
    }

    private void OnMarkerNatureChanged()
    {
        if (QuickAddFormViewModel.AccompaniementViewModel.MarkerNature is not null &&
            QuickAddFormViewModel.AccompaniementViewModel.MarkerNature == Domain.Enums.MarkerNature.Other)
            return;

        QuickAddFormViewModel.AccompaniementViewModel.CommentOnMarkerNature = null;
    }

    private async Task OnValidSubmit()
    {
        if (_quickAddCopropertyEditContext is not null && _quickAddCopropertyEditContext.Validate())
        {
            var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

            var userIdString = claims.FirstOrDefault(
                c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (userIdString != null) 
            {
                var userId = Guid.Parse(userIdString);
                var solidarBuilderFullName = GetSolidarBuilderFullNameFormated();

                var housingAddress = new QuickAddCreatedAddress(
                    QuickAddFormViewModel.StreetNumberName!.Label!,
                    QuickAddFormViewModel.PostalCode!,
                    QuickAddFormViewModel.Municipality!,
                    QuickAddFormViewModel.Department!,
                    QuickAddFormViewModel.Region!,
                    QuickAddFormViewModel.Street!,
                    QuickAddFormViewModel.HouseNumber!,
                    QuickAddFormViewModel.AdditionalAddress!);

                var householdTypology = new QuickAddGeographicalAreaTypology(
                    (GeographicalHousingAreaTypology)QuickAddFormViewModel.Typology!);

                var supportTeam = new QuickAddSupportTeam(
                    QuickAddFormViewModel.AccompaniementViewModel.MarkerNature,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierLastName,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierStructureName,
                    (Guid)QuickAddFormViewModel.ReferentSolidarBuilderId!,
                    solidarBuilderFullName,
                    QuickAddFormViewModel.ReferentEtId,
                    QuickAddFormViewModel.SecondReferentEtId,
                    QuickAddFormViewModel.AccompaniementViewModel.CommentOnMarkerNature,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierFirstName,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierPhoneNumber,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierEmail,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierRole,
                    QuickAddFormViewModel.AccompaniementViewModel.TrustedTierRoleFreeInput,
                    QuickAddFormViewModel.SecondReferentSolidarBuilderId,
                    QuickAddFormViewModel.ReferentDiffuseCoordinator,
                    QuickAddFormViewModel.ReferentTargetCoordinator,
                    QuickAddFormViewModel.ThirdReferentSolidarBuilderId);
                
                if (AccompanyingFileId is Guid accompanyingFileId && accompanyingFileId != Guid.Empty)
                    _relatedAccompanyingFilesIds.Add(accompanyingFileId);

                var copropertyProfileEntities = new QuickAddCreatedCopropertyProfileEntities(
                    housingAddress,
                    householdTypology,
                    supportTeam,
                    _relatedAccompanyingFilesIds);

                var copropertyProfileId = await CopropertyProfileService.CreateCopropertyProfileWithQuickAdd(
                    copropertyProfileEntities,
                    userId,
                    QuickAddFormViewModel.ZeroEnergyExclusionTerritoriesProgram!.Value,
                    QuickAddFormViewModel.AccompanyingType,
                    QuickAddFormViewModel.Territory);

                if (copropertyProfileId != null)
                {
                    if (AccompanyingFileId is Guid _accompanyingFileId && _accompanyingFileId != Guid.Empty)
                    {
                        NavigationManager.NavigateTo($"{Endpoints.NewOccupant}/{_accompanyingFileId}");
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"{Endpoints.CopropertyIdentification}/{copropertyProfileId}");
                    }
                }
            }
        }
    }

    private void OnClickCancelButton()
    {
        NavigationManager.NavigateTo($"{Endpoints.Coproperty}");
    }

    private async Task<IEnumerable<AddressDto>?> OnValueChangedAddress(string search) =>
        (await AddressService.SearchAddressAsync(search)).Value ?? [];

    private static bool IsUserTeritorialBuilderOrCoordinator(string role)
        => role.Equals(Constants.TerritorialBuilderRole) ||
            role.Equals(Constants.DiffuseCoordinatorRole) ||
            role.Equals(Constants.TargetedCoordinatorRole);
}
