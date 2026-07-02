using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Identity.Web;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Helpers;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.Admin.ImportAccompanyingFileData.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.Admin.ImportAccompanyingFileData;

public partial class ImportNewAccompanyingFileData
{
	[Inject] public ISendEventQuery? SendEventQuery { get; set; }

	public List<string> Errors { get; set; } = [];

	public string? Message { get; set; }
	public bool? FileVersion { get; set; } = true;

	private readonly ImportNewAccompanyingFileDataViewModel _importNewAccompanyingFileDataViewModel = new();
	private List<ZeeSelectItem<AccompanyingType?>> AccompanyingTypes { get; } =
	[
		new(AccompanyingType.Targeted.GetDescription(), AccompanyingType.Targeted),
		new(AccompanyingType.Diffuse.GetDescription(), AccompanyingType.Diffuse)
	];
	private EditContext? _importNewAccompanyingFileDataEditContext;
	private List<ZeeSelectItem<Guid?>>? SolidarBuilderUsers { get; set; }
	private List<ZeeSelectItem<Guid?>>? EtUsers { get; set; }
	private List<ZeeSelectItem<Guid?>>? DiffuseCoordinatorUsers { get; set; }
	private List<ZeeSelectItem<Guid?>>? TargetCoordinator { get; set; }
	private List<ZeeSelectItem<Guid?>>? Territory { get; set; }

	private MemoryStream? _memoryStreamFile;


	protected override async Task OnInitializedAsync()
	{
		_importNewAccompanyingFileDataEditContext = new EditContext(_importNewAccompanyingFileDataViewModel);
		await LoadData();
	}

	public async Task HandleExcelFile(InputFileChangeEventArgs e)
	{
		var file = e.File;
		if (file is not null)
		{
			try
			{
				Errors.Clear();
				Message = null;

				var stream = new MemoryStream();
				await file.OpenReadStream().CopyToAsync(stream);
				stream.Position = 0;
				_memoryStreamFile = stream;
			}
			catch (Exception ex) { Errors.Add(ex.Message); }
		}
	}

	public void OnAccompanyingTypeChanged()
	{
		if (_importNewAccompanyingFileDataViewModel.ZeroEnergyExclusionTerritoriesProgram == false)
		{
			_importNewAccompanyingFileDataViewModel.AccompanyingType = null;
			_importNewAccompanyingFileDataViewModel.ReferentEtId = null;
			_importNewAccompanyingFileDataViewModel.ReferentTargetCoordinator = null;
			_importNewAccompanyingFileDataViewModel.Territory = null;
			_importNewAccompanyingFileDataViewModel.ReferentDiffuseCoordinator = null;
		}

		if (_importNewAccompanyingFileDataViewModel.AccompanyingType == AccompanyingType.Diffuse)
		{
			_importNewAccompanyingFileDataViewModel.ReferentEtId = null;
			_importNewAccompanyingFileDataViewModel.ReferentTargetCoordinator = null;
			_importNewAccompanyingFileDataViewModel.Territory = null;
		}
		else { _importNewAccompanyingFileDataViewModel.ReferentDiffuseCoordinator = null; }
	}

	public async Task OnValidSubmitV2()
	{
		if (_importNewAccompanyingFileDataEditContext is not null &&
		    _importNewAccompanyingFileDataEditContext.Validate())
		{
			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var userIdString = authState.User.GetNameIdentifierId();

			if (userIdString is null || _memoryStreamFile is null) return;

			var requiredFields = new RequiredFieldsFromDataImportPage(
				_importNewAccompanyingFileDataViewModel.LastName,
				_importNewAccompanyingFileDataViewModel.FirstName,
				_importNewAccompanyingFileDataViewModel.ZeroEnergyExclusionTerritoriesProgram,
				_importNewAccompanyingFileDataViewModel.AccompanyingType,
				_importNewAccompanyingFileDataViewModel.ReferentTargetCoordinator,
				_importNewAccompanyingFileDataViewModel.ReferentEtId,
				_importNewAccompanyingFileDataViewModel.Territory,
				_importNewAccompanyingFileDataViewModel.ReferentDiffuseCoordinator,
				_importNewAccompanyingFileDataViewModel.ReferentSolidarBuilderId);

			var result = await AccompanyingFileService.ImportAccompanyingFileData(
				_memoryStreamFile,
				Guid.Parse(userIdString),
				requiredFields);

			if (result.IsSuccess) { Message = "Le dossier a été ajouté avec succès."; }
			else { Errors = result.Errors!; }
		}
	}

	public async Task OnValidSubmitV3()
	{
		if (_importNewAccompanyingFileDataEditContext is not null &&
		    _importNewAccompanyingFileDataEditContext.Validate())
		{
			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var userIdString = authState.User.GetNameIdentifierId();

			if (userIdString is null || _memoryStreamFile is null) return;

			var requiredFields = new RequiredFieldsFromDataImportPage(
				_importNewAccompanyingFileDataViewModel.LastName,
				_importNewAccompanyingFileDataViewModel.FirstName,
				_importNewAccompanyingFileDataViewModel.ZeroEnergyExclusionTerritoriesProgram,
				_importNewAccompanyingFileDataViewModel.AccompanyingType,
				_importNewAccompanyingFileDataViewModel.ReferentTargetCoordinator,
				_importNewAccompanyingFileDataViewModel.ReferentEtId,
				_importNewAccompanyingFileDataViewModel.Territory,
				_importNewAccompanyingFileDataViewModel.ReferentDiffuseCoordinator,
				_importNewAccompanyingFileDataViewModel.ReferentSolidarBuilderId);

			var result = await AccompanyingFileService.ImportAccompanyingFileV3Data(
				_memoryStreamFile,
				Guid.Parse(userIdString),
				requiredFields);

            if (result.IsSuccess) { Message = "Le dossier a été ajouté avec succès."; }
			else { Errors = result.Errors!; }
		}
	}


	private async Task LoadData()
	{
		if (SendEventQuery != null)
		{
			var userForQuickAdd =
				(await SendEventQuery.Send(new GetQuickAddChoiceDataQuery { ShouldRetrieveFakeUser = true })).Value!;
			SolidarBuilderUsers = userForQuickAdd.SolidarBuilders
				.Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();
			EtUsers = userForQuickAdd.TerritorialBuilders
				.Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();
			DiffuseCoordinatorUsers = userForQuickAdd.DiffuseCoordinators
				.Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();
			TargetCoordinator = userForQuickAdd.TargetCoordinators
				.Select(x => new ZeeSelectItem<Guid?>(x.UserFullName, x.UserId)).ToList();
			Territory = userForQuickAdd.Territories
				.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId)).ToList();
		}
	}
}