using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity;

public partial class HouseholdIdentity
{
	[CascadingParameter] public EditContext EditContext { get; set; } = null!;
	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
	[CascadingParameter(Name = "IsSoliha")] public bool IsSoliha {  get; set; }
	[Parameter] public HouseholdIdentityViewModel HouseholdIdentityViewModel { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<SocioProfessionalCategory?>> CsPs { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<Guid?>> HouseholdResourcesTypologies { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<HouseholdTypology?>> HouseholdTypologies { get; set; } = null!;

	[Parameter][EditorRequired] public Func<string, Task> OnCheckDuplicatePhoneNumber { get; set; } = null!;

	[Parameter][EditorRequired] public Func<string, Task> OnCheckDuplicateEmail { get; set; } = null!;

	[Parameter] public bool IsDuplicatePhoneNumber { get; set; }

	[Parameter] public bool IsDuplicateEmail { get; set; }

	[Parameter] public EventCallback FormFieldHasChanged { get; set; }

	[Inject] public IHouseholdResourcesTypologyService HouseholdResourcesTypologyService { get; set; } = null!;

	[Inject] public ISendEventQuery SendEvent { get; set; } = null!;

	private List<ZeeSelectItem<Guid?>> _householdHeatingEnergyList = [];

	private List<HouseholdHeatingEnergyLabel> _householdHeatingEnergiesLabels = [];

	private static readonly List<ZeeSelectItem<SocialProtectionFund?>> SocialProtectionFundList =
    [
        new(SocialProtectionFund.Caf.GetDescription(), SocialProtectionFund.Caf),
        new(SocialProtectionFund.Carsat.GetDescription(), SocialProtectionFund.Carsat),
        new(SocialProtectionFund.Cgss.GetDescription(), SocialProtectionFund.Cgss),
        new(SocialProtectionFund.Cpam.GetDescription(), SocialProtectionFund.Cpam),
        new(SocialProtectionFund.Msa.GetDescription(), SocialProtectionFund.Msa),
        new(SocialProtectionFund.Rsi.GetDescription(), SocialProtectionFund.Rsi),
        new(SocialProtectionFund.Urssaf.GetDescription(), SocialProtectionFund.Urssaf),
        new(SocialProtectionFund.Other.GetDescription(), SocialProtectionFund.Other)
    ];

    private static readonly List<ZeeSelectItem<PensionFund?>> PensionFundList =
    [
        new(PensionFund.RetirementInsurance.GetDescription(), PensionFund.RetirementInsurance),
        new(PensionFund.Carsat.GetDescription(), PensionFund.Carsat),
        new(PensionFund.Cnav.GetDescription(), PensionFund.Cnav),
        new(PensionFund.Cram.GetDescription(), PensionFund.Cram),
        new(PensionFund.Crav.GetDescription(), PensionFund.Crav),
        new(PensionFund.AgriculturalSocialMutuality.GetDescription(), PensionFund.AgriculturalSocialMutuality),
        new(
            PensionFund.AgriculturalSocialMutualityEmployee.GetDescription(),
            PensionFund.AgriculturalSocialMutualityEmployee),
        new(PensionFund.Other.GetDescription(), PensionFund.Other)
    ];

	private static readonly List<ZeeSelectItem<AdditionalFund?>> AdditionnalFundList =
	[
		new(AdditionalFund.Agir.GetDescription(), AdditionalFund.Agir),
		new(AdditionalFund.Arrco.GetDescription(), AdditionalFund.Arrco),
		new(AdditionalFund.Ircantec.GetDescription(), AdditionalFund.Ircantec),
		new(AdditionalFund.Rafp.GetDescription(), AdditionalFund.Rafp),
		new(AdditionalFund.Cgss.GetDescription(), AdditionalFund.Cgss),
		new(AdditionalFund.Other.GetDescription(), AdditionalFund.Other)
	];

    private readonly List<EditContext> _secondaryOccupantEditContexts = [];

	private readonly HashSet<object> _validatedObjects = [];

	private EditContext? _editContext;
	private MainOccupantViewModel _mainOccupantViewModel = new();

	private List<SecondaryOccupantViewModel> _secondaryOccupantViewModels = [];

	private bool ShouldDisplayResources { get; set; } = true;
	private bool ShouldDisplayFixedExpense { get; set; } = true;
	private bool ShouldDisplayCurrentExpense { get; set; } = true;
	private bool ShouldDisplayOccasionalExpense { get; set; } = true;
	private bool ShouldDisplayHeatingEnergy { get; set; } = true;
	private List<HouseholdResourcesTypologyDto> _resourcesTypologyList = [];
	private Guid? _noneOptionId;

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(HouseholdIdentityViewModel);
		_mainOccupantViewModel = HouseholdIdentityViewModel.MainOccupantViewModel;
		_secondaryOccupantViewModels = HouseholdIdentityViewModel.SecondaryOccupantViewModels;
		_editContext = new EditContext(HouseholdIdentityViewModel);
		EditContext.OnValidationRequested += EditContext_OnValidationRequested;
		_editContext.OnFieldChanged += HandleFieldChanged;

		InitializedOccupantForms();

		HouseholdIdentityViewModel.HouseholdViewModel.AskedResourcesTypology = HouseholdIdentityViewModel
			.HouseholdViewModel.ResourceTypologieValues.Select(rt => rt.Id).ToList();

		_resourcesTypologyList = (await HouseholdResourcesTypologyService.GetAllAsync()).Value!.Select(rt =>
			new HouseholdResourcesTypologyDto
			{
				Id = rt.Id,
				Name = rt.Name
			}).ToList();

		_noneOptionId = _resourcesTypologyList.FirstOrDefault(rt => rt.Name == EnergyDeprivationLabel.None)?.Id;

		_householdHeatingEnergiesLabels = (await SendEvent.Send(new GetAllHouseholdHeatingEnergyQuery())).Value!.ToList();

		_householdHeatingEnergyList = _householdHeatingEnergiesLabels.Select(hhe => new ZeeSelectItem<Guid?>(hhe.Name, hhe.Id)).ToList();

		HouseholdIdentityViewModel.HouseholdViewModel.HeatingEnergiesValuesSelected = HouseholdIdentityViewModel.HouseholdViewModel.HeatingEnergiesValues.Select(hev => hev.Id).ToList();
	}

	private void CheckDuplicateEmail(string email)
	{
		OnCheckDuplicateEmail.Invoke(email);
	}

	private void CheckDuplicatePhoneNumber(string phoneNumber)
	{
		OnCheckDuplicatePhoneNumber.Invoke(phoneNumber);
	}

	private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
	{
		if (_validatedObjects.Contains(HouseholdIdentityViewModel)) return;
		_editContext?.Validate();
		_validatedObjects.Add(HouseholdIdentityViewModel);
	}

	private static string GetOccupantTitle(int index)
	{
		return (index + 2).ToString();
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (sender == _editContext) EditContext.NotifyFieldChanged(e.FieldIdentifier);
	}

	private void InitializedOccupantForms()
	{
		if (_mainOccupantViewModel != null)
		{
			EditContext editContext = new(_mainOccupantViewModel);
			editContext.OnValidationRequested += EditContext_OnValidationRequested;
			editContext.OnFieldChanged += HandleFieldChanged;
		}

		foreach (var newEditContext in _secondaryOccupantViewModels.Select(
					 occupantViewModel => new EditContext(occupantViewModel)))
		{
			newEditContext.OnValidationRequested += EditContext_OnValidationRequested;
			newEditContext.OnFieldChanged += HandleFieldChanged;
			_secondaryOccupantEditContexts.Add(newEditContext);
		}
	}

	private void OnClickAddOccupant()
	{
		var occupantViewModel = new SecondaryOccupantViewModel();
		var newEditContext = new EditContext(occupantViewModel);
		newEditContext.OnValidationRequested += EditContext_OnValidationRequested;
		newEditContext.OnFieldChanged += HandleFieldChanged;
		_secondaryOccupantEditContexts.Add(newEditContext);
		_secondaryOccupantViewModels.Add(occupantViewModel);
		FormFieldHasChanged.InvokeAsync();
	}

	private void OnClickRemoveOccupant(int index)
	{
		_secondaryOccupantViewModels.RemoveAt(index);
		_secondaryOccupantEditContexts.RemoveAt(index);
		FormFieldHasChanged.InvokeAsync();
	}

	private static void OnDateChanged(MainOccupantViewModel occupant)
	{
		// Check if the nullableDateTime has a value before converting
		if (occupant.Birthday.HasValue)
			occupant.Age = AccompanyingFileHelper.CalculateAge(occupant.Birthday.Value);
		else
			occupant.Age = null;
	}

	private static void OnDateChangeForSecondaryOccupant(SecondaryOccupantViewModel secondaryOccupant)
	{
		if (secondaryOccupant.Birthday.HasValue)
			secondaryOccupant.Age = AccompanyingFileHelper.CalculateAge(secondaryOccupant.Birthday.Value);
		else
			secondaryOccupant.Age = null;
	}

	private void OnNewAskedResourcesTypologyAdded()
	{
		var householdViewModel = HouseholdIdentityViewModel.HouseholdViewModel;

		if (householdViewModel.AskedResourcesTypology is null)
		{
			householdViewModel.ResourceTypologieValues.Clear();
			StateHasChanged();
			return;
		}

		AdjustResourcesTypology(householdViewModel);
	}

	private void AdjustResourcesTypology(HouseholdViewModel householdViewModel)
	{
		if (householdViewModel.AskedResourcesTypology.Contains(_noneOptionId))
		{
			HandleNoneOptionSelection(householdViewModel);
			return;
		}

		int askedCount = householdViewModel.AskedResourcesTypology.Count;
		int existingCount = householdViewModel.ResourceTypologieValues.Count;

		if (askedCount > existingCount)
		{
			AddResourcesTypologyAsync(householdViewModel);
		}
		else
		{
			RemoveUnselectedResources(householdViewModel);
		}

		StateHasChanged();
	}

	private void HandleNoneOptionSelection(HouseholdViewModel householdViewModel)
	{		
		householdViewModel.AskedResourcesTypology.RemoveAll(id => id != _noneOptionId);

		householdViewModel.ResourceTypologieValues.RemoveAll(rt => rt.Id != _noneOptionId);
		householdViewModel.ResourceTypologieValues.Add(new ResourceTypologyValueViewModel { Id = _noneOptionId, Name = EnergyDeprivationLabel.None, Value = 0 });

		StateHasChanged();
	}

	private void AddResourcesTypologyAsync(HouseholdViewModel householdViewModel)
	{
		var addedResourcesTypologyIdList = householdViewModel.AskedResourcesTypology.Where(
				id => householdViewModel.ResourceTypologieValues.TrueForAll(
					res => res.Id != id))
			.ToList();

		foreach (var resourceTypologieDto in
				 addedResourcesTypologyIdList.Select(id => _resourcesTypologyList.Find(rt => rt.Id == id)))
			householdViewModel.ResourceTypologieValues.Add(
				new ResourceTypologyValueViewModel
				{
					Id = resourceTypologieDto?.Id,
					Name = resourceTypologieDto?.Name
				});
	}

	private static void RemoveUnselectedResources(HouseholdViewModel householdViewModel)
	{
		var removedResources = householdViewModel.ResourceTypologieValues
			.Where(res => !householdViewModel.AskedResourcesTypology.Contains(res.Id))
			.ToList();

		householdViewModel.ResourceTypologieValues.RemoveAll(res => removedResources.Contains(res));
	}
	
	private void OnNewHeatingEnergyAdded()
	{
		var viewModel = HouseholdIdentityViewModel.HouseholdViewModel;

		if(viewModel.HeatingEnergiesValuesSelected == null)
		{
			viewModel.HeatingEnergiesValues.Clear();
			StateHasChanged();
			return;
		}

		int selectedHeatingValueCount = viewModel.HeatingEnergiesValuesSelected.Count;
		int heatingValueCount = viewModel.HeatingEnergiesValues.Count;

		if (selectedHeatingValueCount > heatingValueCount)
		{
			var addedHeatingEnergy = viewModel.HeatingEnergiesValuesSelected.Where(
				id => viewModel.HeatingEnergiesValues.TrueForAll(
					hev =>  hev.Id != id
				)
			).ToList();

			foreach(var heatingEnergy in addedHeatingEnergy.Select(id => _householdHeatingEnergiesLabels.Find(hel => hel.Id == id)))
			{
				viewModel.HeatingEnergiesValues.Add(new HouseholdHeatingEnergyValueViewModel { Id = heatingEnergy?.Id, Name = heatingEnergy?.Name });
			}
		}
		else
		{
			var removedHeatingEnergy = viewModel.HeatingEnergiesValues.Where(hev => !viewModel.HeatingEnergiesValuesSelected.Contains(hev.Id)).ToList();

			viewModel.HeatingEnergiesValues.RemoveAll(hev => removedHeatingEnergy.Contains(hev));
		}

		StateHasChanged();
	}
}