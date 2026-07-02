using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.Housing.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.Housing;

public partial class Housing
{
	[CascadingParameter] public EditContext EditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }
	[CascadingParameter(Name = "AccompanyingFileId")] public Guid? AccompanyingFileId { get; set; }

	[Parameter] public Guid UserId { get; set; }

	[Parameter] public string UserRole { get; set; } = null!;

    [Parameter] public HousingViewModel ViewModel { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<OwnershipStatus?>>? PropertyStatuses { get; set; }

	[Parameter] public List<ZeeSelectItem<HousingType?>>? PropertyTypes { get; set; }

	[Parameter] public List<AddressDto>? AddressList { get; set; }

	[Parameter] public Func<LoadDataArgs?, Task>? LoadAddresses { get; set; }

	[Parameter] public List<ZeeSelectItem<HousingYearConstruction?>>? PropertyConstructionYears { get; set; }

	[Inject] public ICopropertyProfileService CopropertyProfileService { get; set; } = null!;

	[Inject] public NavigationManager NavigationManager { get; set; } = null!;

	public List<ZeeSelectItem<Guid?>> CopropertyProfiles { get; set; } = [];

	private static readonly List<ZeeSelectItem<DegradationIndex?>> DegradationIndexes =
	[
		new(Labels.LowDegradationIndex, DegradationIndex.Low),
		new(Labels.MediumDegradationIndex, DegradationIndex.Medium),
		new(Labels.HighDegradationIndex, DegradationIndex.High)
	];

	private static readonly List<ZeeSelectItem<UnsanitaryCoefficient?>> UnsanitaryCoefficients =
	[
		new(Labels.LowUnsanitaryCoefficient, UnsanitaryCoefficient.Low),
		new(Labels.MediumUnsanitaryCoefficient, UnsanitaryCoefficient.Medium),
		new(Labels.HighUnsanitaryCoefficient, UnsanitaryCoefficient.High)
	];

	private static readonly List<ZeeSelectItem<ComfortLevel?>> BadAverageGoodList =
	[
		new(ComfortLevel.Bad.GetDescription(), ComfortLevel.Bad),
		new(ComfortLevel.Medium.GetDescription(), ComfortLevel.Medium),
		new(ComfortLevel.Good.GetDescription(), ComfortLevel.Good)
	];


	private readonly HashSet<object> _validatedObjects = [];

	private EditContext? _editContext;

	private bool IsLinkedToCoproperty =>
		ViewModel.CopropertyProfileId.HasValue && ViewModel.CopropertyProfileId.Value != Guid.Empty;

	private bool AreAddressFieldsDisabled => IsUserAllowedToEdit || IsSoliha || IsLinkedToCoproperty;

	private bool IsCustomAddress => ViewModel.IsManualInput == true && !AreAddressFieldsDisabled;

	private bool ShouldDisplayCopropertyAttachmentField => ViewModel.AskedPropertyType == HousingType.ResidentialCollective;

    protected async override Task OnInitializedAsync()
	{
		base.OnInitialized();
		_editContext = new EditContext(ViewModel);
		EditContext.OnValidationRequested += EditContext_OnValidationRequested;
		_editContext.OnFieldChanged += HandleFieldChanged;

		await LoadCoproperty();
	}

	private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
	{
		if (_validatedObjects.Contains(ViewModel)) return;
		_editContext?.Validate();
		_validatedObjects.Add(ViewModel);
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (sender == _editContext) EditContext.NotifyFieldChanged(e.FieldIdentifier);
	}

	private async Task OnLoadAddresses(LoadDataArgs? args)
	{
		if (LoadAddresses is not null) await LoadAddresses.Invoke(args);
	}

	private void ValidateDegradationIndex()
	{
		_editContext?.NotifyFieldChanged(_editContext.Field(nameof(ViewModel.DegradationIndex)));
	}

	private void ValidateUnsanitaryCoefficient()
	{
		_editContext?.NotifyFieldChanged(_editContext.Field(nameof(ViewModel.UnsanitaryCoefficient)));
	}

	private async Task LoadCoproperty()
	{
        var result = await CopropertyProfileService.GetCopropertyProfileList(UserRole, UserId);

		if(result.IsSuccess)
		{
			CopropertyProfiles = result.Value?.CopropertyProfiles.Select(
				cp => new ZeeSelectItem<Guid?>(cp.Reference, cp.Id))?.ToList() ?? [];
		}
	}

	private void OnPropertyTypeChanged()
	{
		if(ViewModel.AskedPropertyType != HousingType.ResidentialCollective)
		{
			ViewModel.CopropertyProfileId = null;
			EditContext.NotifyFieldChanged(EditContext.Field(nameof(ViewModel.CopropertyProfileId)));
		}
		StateHasChanged();
	}

	private void CreateNewCopropertyProfile()
	{
		if (AccompanyingFileId.HasValue)		
		{
			NavigationManager.NavigateTo($"{Endpoints.QuickAddCoproperty}/{AccompanyingFileId}");
		}
	}
}