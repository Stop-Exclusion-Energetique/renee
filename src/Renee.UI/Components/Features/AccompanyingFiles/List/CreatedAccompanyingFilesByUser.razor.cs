using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.Enums;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.List.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;
using Renee.UI.Components.FormComponents;
using System.Collections.Immutable;
using System.Security.Claims;
using Constants = Renee.Domain.Constants;

namespace Renee.UI.Components.Features.AccompanyingFiles.List;

public partial class CreatedAccompanyingFilesByUser
{
    public CreatedAccompanyingFileByUsersFilterViewModel CreatedAccompanyingFileByUsersFilterViewModel { get; set; } = new();

    public readonly ImmutableList<ZeeSelectItem<FilterContext>> SelectedEsFilter =
    [
        new(Labels.MyAccompanyingFiles, FilterContext.MyAccompanyingFile),
        new(Labels.MyReportingStructure, FilterContext.MyReportingStructure)
    ];

    public readonly ImmutableList<ZeeSelectItem<FilterContext>> SelectedCoordinatorFilter =
    [
        new(Labels.MyAccompanyingFiles, FilterContext.MyAccompanyingFile),
        new(Labels.AllFiles, FilterContext.AllFiles)
    ];

    public readonly ImmutableList<ZeeSelectItem<FilterContext>> SelectedTerritorialBuilderFilter =
    [
        new(Labels.MyAccompanyingFiles, FilterContext.MyAccompanyingFile)
    ];

    public readonly ImmutableList<ZeeSelectItem<FilterContext>> SelectedAdminFilter =
    [
        new(Labels.MyAccompanyingFiles, FilterContext.MyAccompanyingFile),
        new(Labels.FoldersWithoutSolidarBuilder, FilterContext.WithoutSolidarBuilders)
    ];

    public readonly ImmutableList<ZeeSelectItem<AccompanyingFileNeedingBilling>> AccompanyingFileNeedingBillingOptions =
    [
        new(EnumHelper.GetDescription(AccompanyingFileNeedingBilling.ToBillStageOne), AccompanyingFileNeedingBilling.ToBillStageOne),
        new(EnumHelper.GetDescription(AccompanyingFileNeedingBilling.ToBillStageTwo), AccompanyingFileNeedingBilling.ToBillStageTwo),
        new(EnumHelper.GetDescription(AccompanyingFileNeedingBilling.ToBillStageThree), AccompanyingFileNeedingBilling.ToBillStageThree)
    ];

    [Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;
    [Inject] public IUserService? UserService { get; set; }
    [Inject] public AccompanyingFileListFilterDataPersistance AccompanyingFileListFilterDataPersistance { get; set; } = null!;

    public CreatedAccompanyingFilesByUserListViewModel CreatedAccompanyingFilesListViewModel { get; private set; } =
        new();

    private List<ZeeSelectItem<Guid?>>? ReportingStructures { get; set; }
    private List<ZeeSelectItem<Guid?>>? SolidarBuilderDisplayList { get; set; }
    private List<SolidarBuilderUserObjectResult>? SolidarBuilderList { get; set; }
    private List<ZeeSelectItem<Guid?>>? TerritoryDisplayList { get; set; }
    private List<ZeeSelectItem<AccompanyingFileStage>>? AccompanyingFileStages { get; set; }
    private List<ZeeSelectItem<AccompanyingFileStatus>>? AccompanyingFileStatus { get; set; }

    private string _userRole = string.Empty;
    private Guid _userId;
    private int _pageIndex = 1;
    private int _pageSize = 25;
    private int _numberOfElementsToSkip;
    private bool _isDeadlineDateReached = false;

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

            await InitializeData();
        }
    }

    private async Task LoadAccompanyingFilesAsync()
    {
        var queryResult = await SendEventQuery.Send( 
                new GetAccompanyingFileForFileListQuery(
                    _userId,
                    _userRole,
                    new AccompanyingFilesListFilterOptions(
                        CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStages,
                        CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStatuses,
                        CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileNeedingBillings,
                        CreatedAccompanyingFileByUsersFilterViewModel.FilterValue,
                        CreatedAccompanyingFileByUsersFilterViewModel.SortingState,
                        CreatedAccompanyingFileByUsersFilterViewModel.FilterContext,
                        CreatedAccompanyingFileByUsersFilterViewModel.ReportingStructureIdList,
                        CreatedAccompanyingFileByUsersFilterViewModel.SolidarBuildersList,
                        CreatedAccompanyingFileByUsersFilterViewModel.Territories
                    ),
                    _numberOfElementsToSkip,
                    _pageSize
                )
            );

        if (queryResult.IsSuccess && queryResult.Value is not null)
            CreatedAccompanyingFilesListViewModel = new CreatedAccompanyingFilesByUserPresenter().FromQuery(queryResult.Value).Present();

        StateHasChanged();
    }

    private async Task InitializeData()
    {
        AccompanyingFileStages = GetEnumSelectList<AccompanyingFileStage>();
        AccompanyingFileStatus = GetEnumSelectList<AccompanyingFileStatus>();

        var result = await SendEventQuery.Send(new LoadFilterDataBasedOnRoleQuery(_userRole, _userId));

        if (result.IsSuccess && result.Value is not null)
        {
            ReportingStructures = [.. result.Value.ReportingStructures.Select(x => new ZeeSelectItem<Guid?>(x.Name, x.Id))];
            SolidarBuilderList = result.Value.SolidarBuilderUsers;
            SolidarBuilderDisplayList = SolidarBuilderList.Select(x => new ZeeSelectItem<Guid?>(x.FullName, x.Id)).ToList();
            TerritoryDisplayList = [.. result.Value.Territories.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId))];
            _isDeadlineDateReached = result.Value.IsAccompanyingFileModificationDeadlineReached;
        }

        RestoreFilterAndPagingFromPersistence();

        await UpdateAccompanyingFilesList(shouldResetPaging: false);
    }

    private void RestoreFilterAndPagingFromPersistence()
    {
        if (AccompanyingFileListFilterDataPersistance.CreatedAccompanyingFileByUsersFilter is not null)
            CreatedAccompanyingFileByUsersFilterViewModel = AccompanyingFileListFilterDataPersistance.CreatedAccompanyingFileByUsersFilter;

        var hasPageIndexSaved = AccompanyingFileListFilterDataPersistance.CurrentPageIndex > 0;
        var hasPageSizeSaved = AccompanyingFileListFilterDataPersistance.PageSize > 0;

        if (hasPageIndexSaved)
            _pageIndex = AccompanyingFileListFilterDataPersistance.CurrentPageIndex;

        if (hasPageSizeSaved)
            _pageSize = AccompanyingFileListFilterDataPersistance.PageSize;

        if (hasPageIndexSaved || hasPageSizeSaved)
            _numberOfElementsToSkip = (_pageIndex - 1) * _pageSize;
    }

    private static List<ZeeSelectItem<TEnum>> GetEnumSelectList<TEnum>() where TEnum : struct, Enum =>
        [.. Enum.GetValues<TEnum>().Select(x => new ZeeSelectItem<TEnum>(EnumHelper.GetDescription(x), x))];

    public async Task UpdateAccompanyingFilesList(
        string? filterValue = null,
        SortingState? sortingState = null,
        bool shouldResetPaging = true)
    {
        if (filterValue is not null)
			CreatedAccompanyingFileByUsersFilterViewModel.FilterValue = filterValue;

        if (sortingState is not null)
			CreatedAccompanyingFileByUsersFilterViewModel.SortingState = sortingState.Value;

        var (filterOnReportingStructure, filterOnSolidarBuilders, filterOnTerritories) = _userRole switch
        {
            Constants.AssociationMemberRole => (true, false, false),
			Constants.DiffuseCoordinatorRole => (true, true, false),
            Constants.TerritorialBuilderRole => (true, true, false),
            Constants.TargetedCoordinatorRole => (true, true, true),
            _ => (false, false, false)
        };

        if (shouldResetPaging)
            ResetPaging();

        await LoadAccompanyingFilesAsync();

        AccompanyingFileListFilterDataPersistance.CreatedAccompanyingFileByUsersFilter = CreatedAccompanyingFileByUsersFilterViewModel;
        AccompanyingFileListFilterDataPersistance.FilterOnReportingStructure = filterOnReportingStructure;
        AccompanyingFileListFilterDataPersistance.FilterOnSolidarBuilder = filterOnSolidarBuilders;
        AccompanyingFileListFilterDataPersistance.FilterOnTerritories = filterOnTerritories;
        AccompanyingFileListFilterDataPersistance.CurrentPageIndex = _pageIndex;
    }

    private async Task OnPagingChanged(PagingChangedArgs args)
    {
        _pageIndex = args.PageIndex;
        _numberOfElementsToSkip = args.ElementsToSkip;
        AccompanyingFileListFilterDataPersistance.CurrentPageIndex = _pageIndex;
        AccompanyingFileListFilterDataPersistance.PageSize = _pageSize;

        await LoadAccompanyingFilesAsync();
    }

    private void ResetPaging()
    {
        _pageIndex = 1;
        _numberOfElementsToSkip = 0;
    }
}
