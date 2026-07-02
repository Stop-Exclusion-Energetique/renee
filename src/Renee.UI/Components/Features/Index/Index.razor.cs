using System.Collections.Immutable;
using System.Globalization;
using System.Security.Claims;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.Index.Modal;
using Renee.UI.Components.Features.Index.Presenter;
using Renee.UI.Components.Features.Index.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.Index;

public partial class Index
{
	public readonly ImmutableList<ZeeSelectItem<FilterValue>> SelectedFilter = ImmutableList.Create(
		new ZeeSelectItem<FilterValue>(IndexLabels.MyAccompanyingFiles, FilterValue.MyAccompanyingFile),
		new ZeeSelectItem<FilterValue>(IndexLabels.MyReportingStructure, FilterValue.MyReportingStructure));
	public readonly ImmutableList<ZeeSelectItem<FilterValue>> SelectedFilterCoordinatorAndTerritorialBuilder =
		ImmutableList.Create(
			new ZeeSelectItem<FilterValue>(IndexLabels.MyAccompanyingFiles, FilterValue.MyAccompanyingFile),
			new ZeeSelectItem<FilterValue>(IndexLabels.MyAllAccompanyingFiles, FilterValue.MyAllAccompanyingFile));
	public FilterViewModel FilterViewModel { get; } = new();
	public IndexViewModel? ViewModel { get; private set; }

	public List<DpeLabel?>? DpeLabel { get; set; }

	[CascadingParameter(Name = "ZeeSpinner")]
	public ZeeSpinner? ZeeSpinner { get; set; }

	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;
	[Inject] private IModalService ModalService { get; set; } = null!;
    [Inject] ICguVersionService CguVersionService { get; set; } = null!;
    [Inject] public IFileViewerService FileViewer { get; set; } = null!;
    [Inject] public IAdministrationConstantsManagementService AdministrationConstantsManagementService { get; set; } = null!;

    private Guid _userId;
	private string _userRole = string.Empty;
	private string? HeaderTitle { get; set; }

	private bool IsMyReportingStructureBaseDataLoaded { get; set; }
	private bool IsFirtsSolidarBuilderReportingStructureDataLoad { get; set; } = true;
	private IndexViewModel? MyAccompanyingFileBaseData { get; set; }
	private IndexViewModel? MyReportingStructureBaseData { get; set; }

	private List<ZeeSelectItem<Guid?>>? ReportingStructures { get; set; }

	private List<ZeeSelectItem<Guid?>>? SolidarBuilders { get; set; }

	private List<ZeeSelectItem<Guid?>>? Territories { get; set; }

	private IEnumerable<SolidarBuilderUserObjectResult>? Users { get; set; }
    private string? LabelCGU { get; set; }

    protected override async Task OnInitializedAsync()
	{
        var cguVersionResult = await CguVersionService.GetLatestVersionAsync();
        if (cguVersionResult.IsSuccess)
        {
            LabelCGU = cguVersionResult.Value?.Label;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
		var claims = authState.User;
		var userIdClaims = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		var userRole = claims.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

		if (Guid.TryParse(userIdClaims, out var userId) && userRole != null)
		{
			_userId = userId;
			_userRole = userRole;

			await LoadFilterListData(_userRole);
			var indexPresenter = new IndexPresenter();
			MyAccompanyingFileBaseData = userRole switch
			{
				Constants.SolidarBuilderRole or Constants.StructuralReferentRole => (await SendEventQuery.Send(
						new GetStatisticsForSolidarBuilderAndStructuralReferentQuery { ConnectedUserId = _userId })) is { IsSuccess: true, Value: not null } sbResult
						? indexPresenter.FromQuery(sbResult.Value).Present()
						: null,
				Constants.AssociationMemberRole => (await SendEventQuery.Send(
						new GetAccompanyingFileStatisticsForAssociationMemberQuery { ConnectedUserId = _userId })) is { IsSuccess: true, Value: not null } amResult
						? indexPresenter.FromQuery(amResult.Value).Present()
						: null,
				Constants.DiffuseCoordinatorRole => (await SendEventQuery.Send(
						new GetStatisticsForCoordinatorsQuery { ConnectedUserId = _userId })) is { IsSuccess: true, Value: not null } dcResult
						? indexPresenter.FromQuery(dcResult.Value).Present()
						: null,
				Constants.TargetedCoordinatorRole => (await SendEventQuery.Send(
					new GetStatisticsForCoordinatorsQuery
					{
						ConnectedUserId = _userId, IsTargetedCoordinator = true
					})) is { IsSuccess: true, Value: not null } tcResult
					? indexPresenter.FromQuery(tcResult.Value).Present()
					: null,
				Constants.TerritorialBuilderRole => (await SendEventQuery.Send(
					new GetStatisticsForTerritorialBuilderQuery { ConnectedUserId = _userId })) is { IsSuccess: true, Value: not null } tbResult
					? indexPresenter.FromQuery(tbResult.Value).Present()
					: null,
				Constants.AdminRole => indexPresenter.FromQuery(
					await SendEventQuery.Send(new GetNumberOfUserForAdminQuery())).Present(),
				_ => MyAccompanyingFileBaseData
			};

			ViewModel = MyAccompanyingFileBaseData;
			HeaderTitle = GetHeaderTitle(userRole);
		}

		ZeeSpinner?.HideLoading();
	}

	public async Task OnFilterChange()
	{
		switch (_userRole)
		{
			case Constants.SolidarBuilderRole or Constants.StructuralReferentRole: 
				await LoadSolidarBuilderOrStructuralReferentStatistics(); 
				break;
			case Constants.AssociationMemberRole: 
				await LoadAssociationMemberStatistics(); 
				break;
			case Constants.DiffuseCoordinatorRole or Constants.TargetedCoordinatorRole:
				await LoadCoordinatorsStatistics();
				break;
			case Constants.TerritorialBuilderRole: 
				await LoadTerritorialBuilderStatistics(); 
				break;
		}

		StateHasChanged();
	}

	public async Task OnReportingStructuresChanged()
	{
		await LoadAssociationMemberStatistics();
	}

	public async Task OnReportingStructuresCoordinatorOrTerritorialBuilderChanged()
	{
		if (Users != null)
		{
			SolidarBuilders = FilterViewModel.SelectedReportingStructures?.Count > 0
				? Users.Where(e => FilterViewModel.SelectedReportingStructures.Contains(e?.ReportingStructureId))
					.Select(x => new ZeeSelectItem<Guid?>(x?.FullName, x?.Id)).ToList()
				: Users.Select(x => new ZeeSelectItem<Guid?>(x?.FullName, x?.Id)).ToList();

			if (FilterViewModel.SelectedSolidarBuilder is null)
				SolidarBuilders = Users.Select(x => new ZeeSelectItem<Guid?>(x?.FullName, x?.Id)).ToList();
		}

		if (_userRole == Constants.DiffuseCoordinatorRole || _userRole == Constants.TargetedCoordinatorRole)
			await LoadCoordinatorsStatistics();

		if (_userRole == Constants.TerritorialBuilderRole) await LoadTerritorialBuilderStatistics();
	}

	public async Task OnSolidarBuilderChanged()
	{
		if (_userRole == Constants.DiffuseCoordinatorRole || _userRole == Constants.TargetedCoordinatorRole)
			await LoadCoordinatorsStatistics();

		if (_userRole == Constants.TerritorialBuilderRole) await LoadTerritorialBuilderStatistics();
	}

	public async Task OnTerritoriesChanged()
	{
		await LoadCoordinatorsStatistics();
	}

    public async Task ResetFilter()
	{
		FilterViewModel.Reset();

		switch (_userRole)
		{
			case Constants.SolidarBuilderRole or Constants.StructuralReferentRole when FilterViewModel.FilterValue == FilterValue.MyAccompanyingFile:
				ViewModel = MyAccompanyingFileBaseData;
				break;
			case Constants.SolidarBuilderRole or Constants.StructuralReferentRole:
				if (!IsMyReportingStructureBaseDataLoaded)
				{
					var sbResetResult = await SendEventQuery.Send(
						new GetStatisticsForSolidarBuilderAndStructuralReferentQuery
						{
							ConnectedUserId = _userId,
							DateFrom = FilterViewModel.FromDate,
							DateTo = FilterViewModel.ToDate,
							ShouldFilterOnUserReportingStructure = true
						});
					if (sbResetResult.IsSuccess && sbResetResult.Value is not null)
						MyReportingStructureBaseData = new IndexPresenter().FromQuery(sbResetResult.Value).Present();
					IsMyReportingStructureBaseDataLoaded = true;
				}

				ViewModel = MyReportingStructureBaseData;
				break;
			case Constants.AssociationMemberRole: await LoadAssociationMemberStatistics(); break;
			case Constants.DiffuseCoordinatorRole or Constants.TargetedCoordinatorRole:
				await LoadCoordinatorsStatistics();
				break;
			case Constants.TerritorialBuilderRole: await LoadTerritorialBuilderStatistics(); break;
		}

		StateHasChanged();
	}

	public async Task UpdateAdminConstant()
	{
		if (ViewModel is null) return;

		var result = await AdministrationConstantsManagementService.UpdateAccompanyingFileMaximumValue(ViewModel.MaximalNumberOfAccompanyingFileCreated, 
			ViewModel.MaximalNumberOfAccompanyingFileToValidateFirstStage, 
			ViewModel.AccompanyingFileModificationDeadline,
			ViewModel.AccompanyingFileAlertBannerStartDate,
			ViewModel.AccompanyingFileAlertBannerEndDate,
			ViewModel.AccompanyingFileAlertBannerMessage);

		NotificationService.Notify(new NotificationMessage
			{
				Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
				Summary = result.IsSuccess ? IndexLabels.UpdateSuccess : result.Message,
				Duration = 4000
			});
	}

	private static string GetHeaderTitle(string userRole)
	{
		var baseTitle = userRole switch
		{
			Constants.SolidarBuilderRole => IndexLabels.IndexSolidarBuilderTitle,
			Constants.AssociationMemberRole => IndexLabels.IndexAssociationMemberTitle,
			Constants.DiffuseCoordinatorRole => IndexLabels.IndexDiffuseCoordinatorTitle,
			Constants.TargetedCoordinatorRole => IndexLabels.IndexTargetedCoordinatorTitle,
			Constants.TerritorialBuilderRole => IndexLabels.IndexTerritorialBuilderTitle,
			Constants.StructuralReferentRole => IndexLabels.IndexStructuralReferentTitle,
			_ => string.Empty
		};

		return $"{baseTitle}";
	}

	private async Task LoadAssociationMemberStatistics()
	{
		var result = await SendEventQuery.Send(
			new GetAccompanyingFileStatisticsForAssociationMemberQuery
			{
				ConnectedUserId = _userId,
				DateFrom = FilterViewModel.FromDate,
				DateTo = FilterViewModel.ToDate,
				SelectedReportingStructuresIds = FilterViewModel.SelectedReportingStructures
			});
		if (result.IsSuccess && result.Value is not null)
			ViewModel = new IndexPresenter().FromQuery(result.Value).Present();
	}

	private async Task LoadCoordinatorsStatistics()
	{
		var query = new GetStatisticsForCoordinatorsQuery
		{
			ConnectedUserId = _userId,
			DateFrom = FilterViewModel.FromDate,
			DateTo = FilterViewModel.ToDate,
			SelectedReportingStructuresIds = FilterViewModel.SelectedReportingStructures,
			SelectedSolidarBuilderIds = FilterViewModel.SelectedSolidarBuilder,
			ShouldFilterOnUserAllAccompanyingFile =
				FilterViewModel.FilterValue == FilterValue.MyAllAccompanyingFile,
			SelectedTerritoriesIds = FilterViewModel.SelectedTerritories,
			IsTargetedCoordinator = _userRole == Constants.TargetedCoordinatorRole
		};

		var coordinatorsResult = await SendEventQuery.Send(query);
		if (coordinatorsResult.IsSuccess && coordinatorsResult.Value is not null)
			ViewModel = new IndexPresenter().FromQuery(coordinatorsResult.Value).Present();
	}

	private async Task LoadFilterListData(string userRole)
	{
		var result = await SendEventQuery.Send(new LoadFilterDataBasedOnRoleQuery(userRole, _userId));

		if (!result.IsSuccess || result.Value is null) return;

		ReportingStructures = result.Value.ReportingStructures.Select(x => new ZeeSelectItem<Guid?>(x.Name, x.Id)).ToList();

		Users = result.Value.SolidarBuilderUsers;

		SolidarBuilders = Users.Select(x => new ZeeSelectItem<Guid?>(x.FullName, x.Id)).ToList();

		Territories = result.Value.Territories.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId)).ToList();
	}

	private async Task LoadSolidarBuilderOrStructuralReferentStatistics()
	{
		var query = new GetStatisticsForSolidarBuilderAndStructuralReferentQuery { ConnectedUserId = _userId, DateFrom = FilterViewModel.FromDate, DateTo = FilterViewModel.ToDate };

		if (FilterViewModel.FilterValue == FilterValue.MyReportingStructure)
		{
			query.ShouldFilterOnUserReportingStructure = true;

			if (IsFirtsSolidarBuilderReportingStructureDataLoad)
			{
				var rsResult = await SendEventQuery.Send(query);
				if (rsResult.IsSuccess && rsResult.Value is not null)
					MyReportingStructureBaseData = new IndexPresenter().FromQuery(rsResult.Value).Present();
				IsMyReportingStructureBaseDataLoaded = true;
				IsFirtsSolidarBuilderReportingStructureDataLoad = false;
			}
		}

		var sbResult = await SendEventQuery.Send(query);
		if (sbResult.IsSuccess && sbResult.Value is not null)
			ViewModel = new IndexPresenter().FromQuery(sbResult.Value).Present();
	}

	private async Task LoadTerritorialBuilderStatistics()
	{
		var query = new GetStatisticsForTerritorialBuilderQuery
		{
			ConnectedUserId = _userId,
			DateFrom = FilterViewModel.FromDate,
			DateTo = FilterViewModel.ToDate,
			SelectedReportingStructuresIds = FilterViewModel.SelectedReportingStructures,
			SelectedSolidarBuilderIds = FilterViewModel.SelectedSolidarBuilder,
			ShouldFilterOnUserAllAccompanyingFile =
				FilterViewModel.FilterValue == FilterValue.MyAllAccompanyingFile,
			DpeLabelFilter = DpeLabel
		};

		var tbResult = await SendEventQuery.Send(query);
		if (tbResult.IsSuccess && tbResult.Value is not null)
			ViewModel = new IndexPresenter().FromQuery(tbResult.Value).Present();
	}

	private void OnClickTooltip()
	{
		ModalService.Show<IndexModal>(
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });
	}

	private void Subscribe()
	{
		ZeeSpinner?.DisplayLoading();
		StateHasChanged();
		NavigationManager.NavigateTo(Endpoints.UserCreation);
	}

	private static string ToFormatAsFR(object value)
	{
		return ((double)value).ToString("C1", CultureInfo.CreateSpecificCulture("fr-FR"));
	}

	private string ToPercentageAnnahCategory(object value)
	{
		if (ViewModel is null) return string.Empty;

		var total = ViewModel.AnahCategoryRepartition.Sum(x => x.PieAxisValue);
		var percentageValue = (double)value / total;
		return $"{percentageValue.ToString("0.#%", CultureInfo.InvariantCulture)}";
	}

	private string ToPercentageOwnershipStatus(object value)
	{
		if (ViewModel is null) return string.Empty;

		var total = ViewModel.OwnershipStatusRepartition.Sum(x => x.PieAxisValue);
		var percentageValue = (double)value / total;
		return $"{percentageValue.ToString("0.#%", CultureInfo.InvariantCulture)}";
	}

	private string ToPercentageSocioProfessionalCategory(object value)
	{
		if (ViewModel is null) return string.Empty;

		var total = ViewModel.SocioProfessionalCategoryRepartition.Sum(x => x.PieAxisValue);
		var percentageValue = (double)value / total;
		return $"{percentageValue.ToString("0.#%", CultureInfo.InvariantCulture)}";
	}
}
