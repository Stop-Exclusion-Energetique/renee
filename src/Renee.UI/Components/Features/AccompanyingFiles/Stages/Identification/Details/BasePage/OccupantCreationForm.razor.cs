using System.Security.Claims;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals;
using Renee.UI.Components.FormComponents;
using Renee.UI.Components.Layout.StageLayout;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.Result;
using Renee.Application.DTOs.AI;
using Renee.Domain.ReneeError;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage;

public partial class OccupantCreationForm
{
    [SupplyParameterFromQuery]
    public int SelectedTab { get; set; }

    [Inject] private IServiceProvider ServiceProvider { get; set; } = null!;
	[Inject] public IMainOccupantService MainOccupantService { get; set; } = null!;
	[Inject] public IFinancialAidService FinancialAidService { get; set; } = null!;
	[Inject] public IDifficultyFacedByFamilyService DifficultyFacedByFamilyService { get; set; } = null!;
	[Inject] public IHouseholdResourcesTypologyService HouseholdResourcesTypologyService { get; set; } = null!;
	[Inject] public IAddressService AddressService { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;
	[Inject] public IJSRuntime? JsRuntime { get; set; }
	[Inject] public UnsavedChangesGuard UnsavedChangesGuard { get; set; } = null!;

	public AccompanyingFileCreationFormViewModel? AccompanyingFileCreationFormViewModel { get; set; }

	[Parameter] public Guid? AccompanyingFileId { get; set; }

	private string RedirectTabUrl => $"{Endpoints.NewOccupant}/{AccompanyingFileId}";

	private bool IsLinkedToCoproperty =>
		AccompanyingFileCreationFormViewModel?.HousingViewModel.CopropertyProfileId is Guid copropertyProfileId &&
		copropertyProfileId != Guid.Empty;

	private bool _saveButtonIsActive;
	public bool SaveButtonIsActive
	{
		get => _saveButtonIsActive;
		set
		{
			_saveButtonIsActive = value;
			StateHasChanged();
		}
	}

	public EditContext? EditContext { get; set; }

	private List<ZeeSelectItem<HouseholdTypology?>>? HouseholdTypologies { get; } =
	[
		new ZeeSelectItem<HouseholdTypology?>(
			HouseholdTypology.CoupleWithAdultStaying.GetDescription(),
			HouseholdTypology.CoupleWithAdultStaying),
		new ZeeSelectItem<HouseholdTypology?>(
			HouseholdTypology.CoupleWithChildren.GetDescription(),
			HouseholdTypology.CoupleWithChildren),
		new ZeeSelectItem<HouseholdTypology?>(
			HouseholdTypology.CoupleWithoutChildren.GetDescription(),
			HouseholdTypology.CoupleWithoutChildren),
		new ZeeSelectItem<HouseholdTypology?>(
			HouseholdTypology.SingleParentFamily.GetDescription(),
			HouseholdTypology.SingleParentFamily),
		new ZeeSelectItem<HouseholdTypology?>(
			HouseholdTypology.SinglePerson.GetDescription(),
			HouseholdTypology.SinglePerson),
		new ZeeSelectItem<HouseholdTypology?>(
			HouseholdTypology.SinglePersonWithAdultStaying.GetDescription(),
			HouseholdTypology.SinglePersonWithAdultStaying)
	];

    private List<ZeeSelectItem<OwnershipStatus?>>? PropertyStatuses { get; } =
    [
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.CoOwner.GetDescription(), OwnershipStatus.CoOwner),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.DismemberedBarePropertyOnly.GetDescription(),
            OwnershipStatus.DismemberedBarePropertyOnly),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.DismemberedUsurfructOnly.GetDescription(),
            OwnershipStatus.DismemberedUsurfructOnly),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.JointOwnership.GetDescription(),
            OwnershipStatus.JointOwnership),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.PrivateParkTenant.GetDescription(),
            OwnershipStatus.PrivateParkTenant),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.PublicParkTenant.GetDescription(),
            OwnershipStatus.PublicParkTenant),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.OccupantFreeOfCharge.GetDescription(),
            OwnershipStatus.OccupantFreeOfCharge),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.OccupantWithoutRightAndTitle.GetDescription(),
            OwnershipStatus.OccupantWithoutRightAndTitle),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.FullOwnership.GetDescription(),
            OwnershipStatus.FullOwnership),
        new ZeeSelectItem<OwnershipStatus?>(
            OwnershipStatus.RealEstateCompany.GetDescription(),
            OwnershipStatus.RealEstateCompany)
    ];

    private List<ZeeSelectItem<HousingType?>>? PropertyTypes { get; } =
	[
		new ZeeSelectItem<HousingType?>(HousingType.IndividualHouse.GetDescription(), HousingType.IndividualHouse),
		new ZeeSelectItem<HousingType?>(
			HousingType.ResidentialCollective.GetDescription(),
			HousingType.ResidentialCollective)
	];

	private List<ZeeSelectItem<HousingYearConstruction?>>? PropertyConstructionYears { get; } =
	[
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod1.GetDescription(), HousingYearConstruction.HouseConstructionPeriod1),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod2.GetDescription(), HousingYearConstruction.HouseConstructionPeriod2),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod3.GetDescription(), HousingYearConstruction.HouseConstructionPeriod3),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod4.GetDescription(), HousingYearConstruction.HouseConstructionPeriod4),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod5.GetDescription(), HousingYearConstruction.HouseConstructionPeriod5),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod6.GetDescription(), HousingYearConstruction.HouseConstructionPeriod6),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod7.GetDescription(), HousingYearConstruction.HouseConstructionPeriod7),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod8.GetDescription(), HousingYearConstruction.HouseConstructionPeriod8),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod9.GetDescription(), HousingYearConstruction.HouseConstructionPeriod9),
		new ZeeSelectItem<HousingYearConstruction?>(HousingYearConstruction.HouseConstructionPeriod10.GetDescription(), HousingYearConstruction.HouseConstructionPeriod10)
	];

    private List<ZeeSelectItem<SocioProfessionalCategory?>> CsPs { get; } =
	[
		new(SocioProfessionalCategory.Farmer.GetDescription(), SocioProfessionalCategory.Farmer),
		new(SocioProfessionalCategory.Artisan.GetDescription(), SocioProfessionalCategory.Artisan),
		new(SocioProfessionalCategory.Cadre.GetDescription(), SocioProfessionalCategory.Cadre),
		new(SocioProfessionalCategory.Employee.GetDescription(), SocioProfessionalCategory.Employee),
		new(SocioProfessionalCategory.SearchingJob.GetDescription(), SocioProfessionalCategory.SearchingJob),
		new(SocioProfessionalCategory.Worker.GetDescription(), SocioProfessionalCategory.Worker),
		new(
			SocioProfessionalCategory.IntermediateProfession.GetDescription(),
			SocioProfessionalCategory.IntermediateProfession),
		new(SocioProfessionalCategory.Retired.GetDescription(), SocioProfessionalCategory.Retired),
		new(SocioProfessionalCategory.Unemployed.GetDescription(), SocioProfessionalCategory.Unemployed)
	];

	private List<ZeeSelectItem<AccompanyingTimeDuration?>> AccompanyingTimeDurations { get; } =
	[
		new(AccompanyingTimeDuration.LessThanTwoHours.GetDescription(), AccompanyingTimeDuration.LessThanTwoHours),
		new(AccompanyingTimeDuration.BetweenTwoAndFiveHours.GetDescription(), AccompanyingTimeDuration.BetweenTwoAndFiveHours),
		new(AccompanyingTimeDuration.BetweenFiveAndFourteenHours.GetDescription(), AccompanyingTimeDuration.BetweenFiveAndFourteenHours),
		new(AccompanyingTimeDuration.BetweenFourteenAndTwentyHours.GetDescription(), AccompanyingTimeDuration.BetweenFourteenAndTwentyHours),
		new(AccompanyingTimeDuration.BetweenTwentyAndTwentyEightHours.GetDescription(), AccompanyingTimeDuration.BetweenTwentyAndTwentyEightHours),
		new(AccompanyingTimeDuration.BetweenTwentyEightAndFourtyHours.GetDescription(), AccompanyingTimeDuration.BetweenTwentyEightAndFourtyHours),
		new(AccompanyingTimeDuration.BetweenFourtyAndSixtyHours.GetDescription(), AccompanyingTimeDuration.BetweenFourtyAndSixtyHours),
		new(AccompanyingTimeDuration.MoreThanSixtyHours.GetDescription(), AccompanyingTimeDuration.MoreThanSixtyHours)
	];

	private List<AddressDto>? AddressList { get; set; }
	private bool IsDuplicatePhoneNumber { get; set; }
	private bool IsDuplicateEmail { get; set; }
	[Inject] private IModalService? ModalService { get; set; }
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;

	private Guid UserId { get; set; }
	private string UserRole { get; set; } = string.Empty;

	private List<ZeeSelectItem<Guid?>>? HouseholdResourcesTypologies { get; set; }

	private List<ZeeSelectItem<Guid?>>? DifficultiesFacedFamily { get; set; }

	private AccompanyingFileDto? AccompanyingFile { get; set; }

	private bool IsUserNotAllowedToEdit =>
		(
			UserRole == Constants.StructuralReferentRole || (UserRole == Constants.SolidarBuilderRole &&
			(AccompanyingFile?.Stage > AccompanyingFileStage.Identify || AccompanyingFile?.Status >= AccompanyingFileStatus.WaitingForApproval) &&
			AccompanyingFile.IsInTzeeProgram)
		);

	private bool IsSoliha { get; set; } = false;

	private bool IsLastTab => SelectedTab == 3;

	private bool _isSubmission = false;

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

		var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		UserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

		if (value != null)
			UserId = Guid.Parse(value);
		else
			throw new InvalidDataException("User Id not found in claims");

		if(AccompanyingFileId is null)
		{
			NavigateToAccompanyingFileList();
			return;
		}

		UnsavedChangesGuard.UpdateCurrentUrl(NavigationManager.Uri);
		UnsavedChangesGuard.SetSaveHandler(SaveData);

		await LoadAccompanyingFileAsync();

		EditContext = new EditContext(AccompanyingFileCreationFormViewModel!);
		EditContext.OnFieldChanged += async (sender, args) => await OnFormFieldHasBeenChanged();

		IsSoliha = AccompanyingFile?.ReportingStructureName?.ToLowerInvariant().Contains(Labels.IsSolihaStructure, StringComparison.InvariantCultureIgnoreCase) == true && AccompanyingFile?.IsImported == true;

		HouseholdResourcesTypologies = (await HouseholdResourcesTypologyService.GetAllAsync()).Value!
			.Select(x => new ZeeSelectItem<Guid?>(x.Name, x.Id)).ToList();

		DifficultiesFacedFamily = (await DifficultyFacedByFamilyService.GetAllAsync()).Value!.OrderBy(x => x.Name)
			.Select(x => new ZeeSelectItem<Guid?>(x.Name, x.Id)).ToList();
	}

	public async Task SubmitWithValidation()
	{
		try
		{
			var isValid = ValidateEditContext();

			if (isValid)
			{
				_isSubmission = true;
				var saved = await SaveData();

				if (saved)
					NavigationManager.NavigateTo(
						$"{Endpoints.IdentificationSynthesis}/{AccompanyingFileId}");

				_isSubmission = false;
			}
		}

		catch (Exception)
		{
			// ignored
		}
	}

	public bool ValidateEditContext()
	{
		return EditContext!.Validate();
	}

	private async Task ChangeTab(int index)
	{
		await SaveWithoutValidation();
        SelectedTab = index;
		StateHasChanged();
		if (JsRuntime != null)
		{
			await JsRuntime.InvokeVoidAsync("scrollToTop");
		}
    }

	private string GetContextMenuRedirectUrl(int? index)
	{
		return index switch
		{
			0 or 1 or 2 or 3 => $"{RedirectTabUrl}?SelectedTab={index}",
			_ => $"{RedirectTabUrl}"
		};
	}

	private async Task CheckDuplicateEmail(string email)
	{
		if (string.IsNullOrEmpty(email.Trim())) { IsDuplicateEmail = false; }
		else
		{
			var occupants = await MainOccupantService.GetAllMainOccupantAsync();

			if (occupants.Value!.Any(o => o.Email == email))
			{
				ShowZeeErrorModal(Labels.Errors.DuplicateEmail);
				IsDuplicateEmail = true;
			}
			else { IsDuplicateEmail = false; }
		}

		await InvokeAsync(StateHasChanged);
	}

	private async Task CheckDuplicatePhoneNumber(string phoneNumber)
	{
		if (string.IsNullOrEmpty(phoneNumber.Trim())) { IsDuplicatePhoneNumber = false; }
		else
		{
			var occupants = await MainOccupantService.GetAllMainOccupantAsync();

			if (occupants.Value!.Any(o => o.PhoneNumber == phoneNumber))
			{
				ShowZeeErrorModal(Labels.Errors.DuplicatePhoneNumber);
				IsDuplicatePhoneNumber = true;
			}
			else { IsDuplicatePhoneNumber = false; }
		}

		await InvokeAsync(StateHasChanged);
	}

	private static bool IsPostalCodeInIleDeFrance(string? postalCode)
	{
		if (string.IsNullOrEmpty(postalCode)) return false;
		string[] ileDeFranceDepartments = ["75", "77", "78", "91", "92", "93", "94", "95"];
		var firstTwoDigits = postalCode[..2];
		return ileDeFranceDepartments.Contains(firstTwoDigits);
	}

	private async Task LoadAddresses(LoadDataArgs? args)
	{
		if (!string.IsNullOrEmpty(args?.Filter))
		{
			AddressList = [.. (await AddressService.SearchAddressAsync(args.Filter)).Value ?? []];
        }
        else
			AddressList = [];
		StateHasChanged();
	}

	private void NavigateToAccompanyingFileList()
	{
		NavigationManager.NavigateTo(Endpoints.UserCreatedAccompanyingFiles);
	}

	// TO DO : find another solution
	private async Task OccupantsHaveChanged()
	{
		if (!SaveButtonIsActive)
		{
			await UnsavedChangesGuard.SetUnsavedChangesState(true);
			SaveButtonIsActive = true;
		}
	}

	private async Task OnFormFieldHasBeenChanged()
	{
        LoadEnergyEffortRate();

		if (SaveButtonIsActive) return;

		await UnsavedChangesGuard.SetUnsavedChangesState(true);
		SaveButtonIsActive = true;
	}

    private async Task<bool> SaveData()
	{
		try
		{
			var formViewModel = EditContext!.Model as AccompanyingFileCreationFormViewModel;

			if(formViewModel!.ReasonOfProjectViewModel.SigningHouseholdSupportDate is not null)
			{
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.AnahCategory =
					(await FinancialAidService.GetFinancialAidFromAnah(
						1 + formViewModel.HouseholdIdentityViewModel.SecondaryOccupantViewModels.Count,
						IsPostalCodeInIleDeFrance(AccompanyingFileCreationFormViewModel!.HousingViewModel.PostalCode),
						double.Parse(AccompanyingFileCreationFormViewModel.HouseholdIdentityViewModel.HouseholdViewModel.IncomeTaxReference?.ToString() ??"0"),
						formViewModel!.ReasonOfProjectViewModel.SigningHouseholdSupportDate
					)).Value;
			}

			var updatedHousehold = new UpdatedHousehold(
				formViewModel!.HouseholdIdentityViewModel.HouseholdViewModel.AskedHouseholdTypology,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.FollowedBySocialWorker,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.Disability,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.LongTermIllness,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.LackOfAutonomy,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.Curatorship,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.Guardianship,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.IncomeTaxReference,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.AnahCategory,
				formViewModel.ReasonOfProjectViewModel.ReasonOfProjectSocialContext,
				formViewModel.ReasonOfProjectViewModel.FamilyProject,
				formViewModel.ReasonOfProjectViewModel.VisitAvailability,
				formViewModel.ReasonOfProjectViewModel.DifficultiesDetails,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.HasUnpaidEnergyBills,
				formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.EnergyEffortRate);

			var updatedMainOccupant = new UpdatedHouseholdMainOccupant(
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.Id,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.Trigram ?? string.Empty,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.Birthday,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.Age,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.AskedCsp,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.PhoneNumber,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.Email,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.Profession,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.AskedSocialProtectionFund,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.SocialProtectionFundFreeInput,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.AskedPensionFund,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.PensionFundFreeInput,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.AskedAdditionalFund,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.AdditionalFundFreeInput,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.FirstName,
				formViewModel.HouseholdIdentityViewModel.MainOccupantViewModel.LastName);

			var updatedHouseholdOccupants = formViewModel.HouseholdIdentityViewModel.SecondaryOccupantViewModels.Select(
				o => new UpdatedHouseholdSecondaryOccupant(
					o.Id,
					o.Trigram ?? string.Empty,
					o.Birthday,
					o.Age,
					o.IsDependent)).ToList();

			var updatedHouseholdDifficulties = formViewModel.ReasonOfProjectViewModel.AskedDifficulties;

			var updatedHouseholdExpenses = formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.Expenses
				.ToUpdatedExpenseInput();

			var updatedHouseholdResources = formViewModel.HouseholdIdentityViewModel.HouseholdViewModel
				.ResourceTypologieValues.Select(rtv => new UpdatedHouseholdResource(rtv.Id, rtv.Value ?? 0)).ToList();

			var updatedHouseholdHeatingEnergy = formViewModel.HouseholdIdentityViewModel.HouseholdViewModel.HeatingEnergiesValues.Select(hev => new UpdatedHouseholdHeatingEnergy(hev.Id, hev.Value ?? 0)).ToList();

			var updatedHousing = new UpdatedHousing(
				formViewModel.HousingViewModel.Typology,
				formViewModel.HousingViewModel.AbfZoneYes,
				formViewModel.HousingViewModel.ArchitecturalOrUrbanStandards,
				formViewModel.HousingViewModel.AskedPropertyStatus,
				formViewModel.HousingViewModel.AskedPropertyType,
				formViewModel.HousingViewModel.AskedConstructionYear,
				formViewModel.HousingViewModel.Surface,
				formViewModel.HousingViewModel.NumberOfRooms,
				formViewModel.HousingViewModel.NumberOfFloor,
				formViewModel.HousingViewModel.YearOfacquisition,
				formViewModel.HousingViewModel.CadastralReference,
				formViewModel.HousingViewModel.RenovationWorkYes,
				formViewModel.HousingViewModel.RenovationExplanations,
				formViewModel.HousingViewModel.CopropertyProfileId);

			var updatedAddress = new UpdatedAddress(
				AccompanyingFile!.Housing.Address!.Id,
				formViewModel.HousingViewModel.Name,
				formViewModel.HousingViewModel.PostalCode,
				formViewModel.HousingViewModel.Municipality,
				formViewModel.HousingViewModel.Departement,
				formViewModel.HousingViewModel.Region,
				formViewModel.HousingViewModel.AdditionalAddress);

			var updatedHousingInitialState = new UpdatedHousingInitialStateForIdentificationMilestone(
				formViewModel.HousingViewModel.DegradationIndex,
				formViewModel.HousingViewModel.UnsanitaryCoefficient,
				formViewModel.HousingViewModel.SummerComfort,
				formViewModel.HousingViewModel.WinterComfort,
				formViewModel.HousingViewModel.SoundComfort,
				formViewModel.EnergyProfileViewModel.EnergyConsumption,
				formViewModel.EnergyProfileViewModel.GesEmissions,
				formViewModel.EnergyProfileViewModel.EnergyDeprivation,
				formViewModel.EnergyProfileViewModel.AskedDpeLabel,
				formViewModel.EnergyProfileViewModel.AskedGesLabel);

			var result = await AccompanyingFileService.UpdateAccompanyingFileForIdentificationMilestone(
				new SaveAccompanyingFileIdentificationMilestoneCommandInput(
					AccompanyingFile!.Id,
					formViewModel.ReasonOfProjectViewModel.AccompanyingTimeDuration,
					updatedHousehold,
					updatedMainOccupant,
					updatedHouseholdOccupants,
					updatedHouseholdDifficulties,
					updatedHouseholdExpenses,
					updatedHouseholdResources,
					updatedHouseholdHeatingEnergy,
					updatedHousing,
					updatedAddress,
					updatedHousingInitialState,
					UserId,
					formViewModel.ReasonOfProjectViewModel.SigningHouseholdSupportDate,
					formViewModel.ReasonOfProjectViewModel.FirstEncounterDate,
					formViewModel.ReasonOfProjectViewModel.DeliveryTime != null
						? int.Parse(formViewModel.ReasonOfProjectViewModel.DeliveryTime)
						: null,
					formViewModel.ReasonOfProjectViewModel.ShouldAccompanyingFileBeSubmittedToAnah,
					_isSubmission));

			if (result.IsSuccess && result.Value)
			{
				StageNavigationStateService.NotifyStateChanged();
			}

			NotificationService.Notify(new NotificationMessage
			{
				Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
				Summary = result.Message,
				Duration = 4000
			});

			return result.IsSuccess && result.Value;
		}
		catch (Exception) { return false; }
		finally
		{
			await UnsavedChangesGuard.SetUnsavedChangesState(false);
			SaveButtonIsActive = false;
		}
	}

	private async Task SaveWithoutValidation()
	{
		try { await SaveData(); }
		catch (Exception)
		{
			// ignored
		}
	}

	private void ShowZeeErrorModal(string message)
	{
		var parameters = new ModalParameters { { "Message", message } };
		var modalOptions = new ModalOptions { Position = ModalPosition.Middle };
		ModalService?.Show<ZeeErrorModal>("", parameters, modalOptions);
	}

	private void LoadEnergyEffortRate()
	{
        if (AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel.HouseholdViewModel.EnergyEffortRate is double rate)
        {
            AccompanyingFileCreationFormViewModel.EnergyProfileViewModel.IsEnergyEffortRateRequired = rate >= 8;
        }

        StateHasChanged();
    }

	private async Task ShowAbortAccompanyingFileModal()
	{
		if (ModalService == null)
			return;

		var modalResult = await ModalService.Show<AbortAccompanyingFileModal>(
			new ModalParameters().Add(nameof(AbortAccompanyingFileModal.AccompanyingFileReference), AccompanyingFile!.Reference),
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true, Size = ModalSize.Large }).Result;

		if (modalResult.Cancelled)
			return;

		var saveResult = await SaveData();

		if (modalResult.Data is not AbortAccompanyingFileModalResult modalData || !saveResult)
			return;

		var abortCommandResult = await AccompanyingFileService.AbortAccompanyingFile(new AbortAccompanyingFileCommandInput(
			UserId,
			AccompanyingFileId.GetValueOrDefault(),
			modalData.AbortReasonLabelId.GetValueOrDefault(),
			modalData.SolidarBuilderComment ?? string.Empty,
			modalData.IsBillingRequested ?? false,
			modalData.HasAttachment));

		if (abortCommandResult.IsSuccess)
			await AccompanyingFileService.SendAccompanyingFileAbortMails(AccompanyingFileId.GetValueOrDefault());

		NotificationService.Notify(new NotificationMessage
		{
			Severity = abortCommandResult.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
			Summary = abortCommandResult.Message,
			Duration = 4000
		});

		if (abortCommandResult.IsSuccess)
			await LoadAccompanyingFileAsync();

		StateHasChanged();
	}

	private async Task LoadAccompanyingFileAsync()
	{
		var result = await AccompanyingFileService.GetAccompanyingFileById(AccompanyingFileId!.Value, UserId, UserRole);

		if (!result.IsSuccess)
		{
			NavigateToAccompanyingFileList();
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
			return;
		}

		AccompanyingFile = result.Value;

		AccompanyingFileCreationFormViewModel =
			AccompanyingFileCreationFormViewModel.CreateViewModelFromAccompanyingFileDto(AccompanyingFile!);
	}

	public Task<ReneeOperationResult<AISynthesisResult>> AnalyzeDossierWithAIAsync()
	{
		if (!AccompanyingFileId.HasValue)
			throw new InvalidOperationException("AccompanyingFileId cannot be null when analyzing the dossier with AI.");

		var aiDossierSynthesisService = ServiceProvider.GetService(typeof(IAIDossierSynthesisService)) as IAIDossierSynthesisService;

      if (aiDossierSynthesisService is null)
			return Task.FromResult(ReneeOperationResult<AISynthesisResult>.Failure("Le service d'analyse IA n'est pas disponible."));

        return aiDossierSynthesisService.AnalyzeAsync(AccompanyingFileId.Value);
	}
}
