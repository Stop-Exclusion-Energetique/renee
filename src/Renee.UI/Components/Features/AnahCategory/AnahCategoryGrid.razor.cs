using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Renee.Application.DTOs.Administration;
using Renee.Application.Interfaces.Administration;
using Renee.Domain.Entity;
using Renee.UI.Components.DisplayComponents;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AnahCategory;

public partial class AnahCategoryGrid
{
	[Inject] public IAnahCategoryService AnahCategoryService { get; set; } = default!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

	private List<AnahCategoryDto> _anahCategoriesInIleDeFrance = new List<AnahCategoryDto>();
	private List<AnahCategoryDto> _anahCategoriesNotInIleDeFrance = new List<AnahCategoryDto>();

	private ZeeDataGrid<AnahCategoryDto>? _anahCategoriesInIleDeFranceGrid;
	private ZeeDataGrid<AnahCategoryDto>? _anahCategoriesNotInIleDeFranceGrid;

	private AnahCategoryDto? _anahCategoryToInsert;
	private AnahCategoryDto? _anahCategoryToUpdate;

	private List<AnahCategorySuplementaryOccupantIncome> _anahCategoriesSupplementaryIncomeInIleDeFrance = new();
	private List<AnahCategorySuplementaryOccupantIncome> _anahCategoriesSupplementaryIncomeNotInIleDeFrance = new();

	private ZeeDataGrid<AnahCategorySuplementaryOccupantIncome>? _anahCategoriesSupplementaryIncomeInIleDeFranceGrid;
	private ZeeDataGrid<AnahCategorySuplementaryOccupantIncome>? _anahCategoriesSupplementaryIncomeNotInIleDeFranceGrid;

	private AnahCategorySuplementaryOccupantIncome? _anahCategorySupplementaryIncomeToInsert;
	private AnahCategorySuplementaryOccupantIncome? _anahCategorySupplementaryIncomeToUpdate;

	private Guid _userId;

	protected override async Task OnInitializedAsync()
	{
		await LoadAnahCategory();

		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;
		var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (Guid.TryParse(value, out Guid id))
			_userId = id;
	}

	private void CancelEdit(ZeeDataGrid<AnahCategoryDto>? grid, AnahCategoryDto anahCategoryDto)
	{
		Reset();

		grid?.CancelEditRow(anahCategoryDto);
	}

	private void CancelOccupantIncomeEdit(ZeeDataGrid<AnahCategorySuplementaryOccupantIncome>? grid, AnahCategorySuplementaryOccupantIncome anahCategoryDto)
	{
		ResetSupplementaryIncome();

		grid?.CancelEditRow(anahCategoryDto);
	}

	private async Task EditRow(ZeeDataGrid<AnahCategoryDto>? grid, AnahCategoryDto anahCategoryDto)
	{
		_anahCategoryToUpdate = anahCategoryDto;

		if (grid != null)
			await grid.EditRow(anahCategoryDto);
	}

	private async Task EditOccupantIncomeRow(ZeeDataGrid<AnahCategorySuplementaryOccupantIncome>? grid, AnahCategorySuplementaryOccupantIncome supplementaryOccupantIncome)
	{
		_anahCategorySupplementaryIncomeToUpdate = supplementaryOccupantIncome;

		if (grid != null)
			await grid.EditRow(supplementaryOccupantIncome);
	}

	private async Task InsertRow(ZeeDataGrid<AnahCategoryDto>? grid)
	{
		if (grid != null && grid == _anahCategoriesInIleDeFranceGrid)
		{
			_anahCategoryToInsert = new AnahCategoryDto { Id = Guid.NewGuid(), IsInIleDeFrance = true };
			await grid.InsertRow(_anahCategoryToInsert);
		}

		if (grid != null && grid == _anahCategoriesNotInIleDeFranceGrid)
		{
			_anahCategoryToInsert = new AnahCategoryDto { Id = Guid.NewGuid(), IsInIleDeFrance = false };
			await grid.InsertRow(_anahCategoryToInsert);
		}
	}

	private async Task InsertSupplementaryOccupantIncomeRow(ZeeDataGrid<AnahCategorySuplementaryOccupantIncome>? grid)
	{
		if (grid != null && grid == _anahCategoriesSupplementaryIncomeInIleDeFranceGrid)
		{
			_anahCategorySupplementaryIncomeToInsert = new AnahCategorySuplementaryOccupantIncome { Id = Guid.NewGuid(), IsInIleDeFrance = true };
			await grid.InsertRow(_anahCategorySupplementaryIncomeToInsert);
		}

		if (grid != null && grid == _anahCategoriesSupplementaryIncomeNotInIleDeFranceGrid)
		{
			_anahCategorySupplementaryIncomeToInsert = new AnahCategorySuplementaryOccupantIncome { Id = Guid.NewGuid(), IsInIleDeFrance = false };
			await grid.InsertRow(_anahCategorySupplementaryIncomeToInsert);
		}
	}

	private async Task OnCreateRow(AnahCategoryDto anahCategoryDto)
	{
		if (AnahCategoryService != null)
		{
			var result = await AnahCategoryService.CreateAnahCategory(anahCategoryDto, _userId);

			if (!result.IsSuccess)
				ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
		}

		await LoadAnahCategory();

		_anahCategoryToInsert = null;

		StateHasChanged();
	}

	private async Task OnCreateSupplementaryOccupantIncomeRow(AnahCategorySuplementaryOccupantIncome incomeRule)
	{
		if (AnahCategoryService != null)
		{
			var result = await AnahCategoryService.CreateAnahCategoryForSuplemntaryOccupant(incomeRule);

			if (!result.IsSuccess)
				ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
		}

		await LoadAnahCategory();

		_anahCategorySupplementaryIncomeToInsert = null;

		StateHasChanged();
	}

	private async Task OnUpdateRow(AnahCategoryDto anahCategoryDto)
	{
		Reset();

		if (AnahCategoryService != null)
		{
			var result = await AnahCategoryService.UpdateAnahCategory(anahCategoryDto, _userId);

			if (!result.IsSuccess)
				ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
		}

		await LoadAnahCategory();
		StateHasChanged();
	}

	private async Task OnUpdateSupplementaryOccupantIncomeRow(AnahCategorySuplementaryOccupantIncome input)
	{
		ResetSupplementaryIncome();

		if (AnahCategoryService != null)
		{
			var result = await AnahCategoryService.UpdateAnahCategoryForSuplemntaryOccupant(input);

			if (!result.IsSuccess)
				ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
		}

		await LoadAnahCategory();
		StateHasChanged();
	}

	private void Reset()
	{
		_anahCategoryToInsert = null;
		_anahCategoryToUpdate = null;
	}

	private void ResetSupplementaryIncome()
	{
		_anahCategorySupplementaryIncomeToInsert = null;
		_anahCategorySupplementaryIncomeToUpdate = null;
	}

	private static async Task SaveRow(ZeeDataGrid<AnahCategoryDto>? grid, AnahCategoryDto anahCategoryDto)
	{
		if (grid != null)
			await grid.UpdateRow(anahCategoryDto);
	}

	private static async Task SaveSupplementaryOccupantIncomeRow(ZeeDataGrid<AnahCategorySuplementaryOccupantIncome>? grid, AnahCategorySuplementaryOccupantIncome anahCategoryDto)
	{
		if (grid != null)
			await grid.UpdateRow(anahCategoryDto);
	}

	private async Task LoadAnahCategory()
	{
		var result = await AnahCategoryService.GetAllAnahCategories();

		if (result.IsSuccess)
		{
			var anahCategoryDtos = result!.Value!.AnahCategories;

			_anahCategoriesInIleDeFrance = anahCategoryDtos!.Where(x => x.IsInIleDeFrance).ToList();
			_anahCategoriesNotInIleDeFrance = anahCategoryDtos!.Where(x => !x.IsInIleDeFrance).ToList();

			var supplementaryIncome = result!.Value!.SupplementaryOccupantIncomes;

			_anahCategoriesSupplementaryIncomeInIleDeFrance = supplementaryIncome!.Where(x => (bool)x.IsInIleDeFrance!).ToList();
			_anahCategoriesSupplementaryIncomeNotInIleDeFrance = supplementaryIncome!.Where(x => !(bool)x.IsInIleDeFrance!).ToList();
		}
	}

	private static string ConvertDateTimeToString(DateTime? dateTime)
	{
		if (dateTime == null)
			return string.Empty;

		return dateTime.Value.ToShortDateString();
	}

	private void ShowNotification(NotificationMessage message)
	{
		NotificationService.Notify(message);
	}
}