using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;
using Microsoft.AspNetCore.Components.Authorization;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal;
using System.Security.Claims;
using Renee.UI.Components.Features.AccompanyingFiles.List.Modal;
using Renee.UI.Components.Layout.SynthesisLayoutManager;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification;

public partial class IdentificationSynthesis
{
	[Parameter] public Guid AccompanyingFileId { get; set; }
	public OccupantSynthesisViewModel OccupantSynthesisViewModel { get; } = new();

	[Inject] public SynthesysLayoutStateManager SynthesysLayoutStateManager { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] private IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] private IModalService ModalService { get; set; } = null!;
	[Inject] private IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] private IFileService FileService { get; set; } = null!;
	[Inject] public NavigationHistoryManager NavigationHistoryManager { get; set; } = null!;

	private bool? IsFinancingAsked { get; set; } = false;
	private MemoryStream? _memoryStreamFile;
	private string _fileName = string.Empty;
	private byte[]? _fileData;
	private string _blobName = string.Empty;
	private string _fileExtension = string.Empty;
	public string ErrorMessage { get; set; } = string.Empty;

	public string UserRole { get; set; } = string.Empty;
	private Guid UserId { get; set; }

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

		var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		UserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

		if(Guid.TryParse(value, out Guid id))
		{
			UserId = id;
		}

		var synthesisResult =
			await AccompanyingFileService.GetAccompanyingFileSynthesis(AccompanyingFileId);

		if (synthesisResult.IsSuccess && synthesisResult.Value is not null)
		{
			var accompanyingFileForSynthesisDto = synthesisResult.Value;
			SynthesysLayoutStateManager.AssociatedResourceReference = accompanyingFileForSynthesisDto.AccompanyingFileReference;
			SynthesysLayoutStateManager.AssociatedResourceId = AccompanyingFileId;
			SynthesysLayoutStateManager.AccompanyingFileStage = AccompanyingFileStage.Identify;
			SynthesysLayoutStateManager.ShouldDisplayNavigationButton = !NavigationHistoryManager.PreviousUri!.Contains(Endpoints.NewOccupant);

			SynthesysLayoutStateManager.NotifyStateChanged();

			OccupantSynthesisViewModel.Id = accompanyingFileForSynthesisDto.Id;
			OccupantSynthesisViewModel.Trigram = accompanyingFileForSynthesisDto.Trigram;
			OccupantSynthesisViewModel.Name = accompanyingFileForSynthesisDto.Name;
			OccupantSynthesisViewModel.FirstName = accompanyingFileForSynthesisDto.FirstName;
			OccupantSynthesisViewModel.PhoneNumber = accompanyingFileForSynthesisDto.PhoneNumber;
			OccupantSynthesisViewModel.Age = accompanyingFileForSynthesisDto.Age;
			OccupantSynthesisViewModel.SocioProfessionalCategory =
				accompanyingFileForSynthesisDto.SocioProfessionalCategory!.GetDescription();
			OccupantSynthesisViewModel.NumberOfOccupants = accompanyingFileForSynthesisDto.NumberOfOccupants;
			OccupantSynthesisViewModel.HouseholdTypology =
				accompanyingFileForSynthesisDto.HouseholdTypology!.GetDescription();
			OccupantSynthesisViewModel.HouseholdDifficulties =
				GetHouseholdDifficulties(accompanyingFileForSynthesisDto);
			OccupantSynthesisViewModel.TaxIncome = accompanyingFileForSynthesisDto.TaxIncome;
			OccupantSynthesisViewModel.AnahCategory = accompanyingFileForSynthesisDto.AnahCategory;
			OccupantSynthesisViewModel.AvailableBudget = accompanyingFileForSynthesisDto.AvailableBudget;
			OccupantSynthesisViewModel.EnergeticTotal = accompanyingFileForSynthesisDto.EnergeticTotal;
			OccupantSynthesisViewModel.EnergyEffortRate = accompanyingFileForSynthesisDto.EnergyEffortRate;
			OccupantSynthesisViewModel.Address = accompanyingFileForSynthesisDto.Address;
			OccupantSynthesisViewModel.PostalCode = accompanyingFileForSynthesisDto.PostalCode;
			OccupantSynthesisViewModel.City = accompanyingFileForSynthesisDto.City;
			OccupantSynthesisViewModel.HousingType = accompanyingFileForSynthesisDto.HousingType!.GetDescription();
			OccupantSynthesisViewModel.OwnershipStatus =
				accompanyingFileForSynthesisDto.OwnershipStatus!.GetDescription();
			OccupantSynthesisViewModel.LivingSpaceInSquareMeter =
				accompanyingFileForSynthesisDto.LivingSpaceInSquareMeter;
			OccupantSynthesisViewModel.BuildingYear = accompanyingFileForSynthesisDto.BuildingYear;
			OccupantSynthesisViewModel.DegradationIndex =
				accompanyingFileForSynthesisDto.DegradationIndex!.GetDescription();
			OccupantSynthesisViewModel.UnsanitaryCoefficient =
				accompanyingFileForSynthesisDto.UnsanitaryCoefficient!.GetDescription();
			OccupantSynthesisViewModel.MarClassification = accompanyingFileForSynthesisDto.Mar;
			OccupantSynthesisViewModel.DpeLabel = accompanyingFileForSynthesisDto.DpeLabel;
			OccupantSynthesisViewModel.EnergyConsumption = accompanyingFileForSynthesisDto.EnergyConsumption;
			OccupantSynthesisViewModel.GesLabel = accompanyingFileForSynthesisDto.GesLabel;
			OccupantSynthesisViewModel.GesEmissions = accompanyingFileForSynthesisDto.GesEmissions;
			OccupantSynthesisViewModel.ElectricityDeprivation =
				accompanyingFileForSynthesisDto.ElectricityDeprivation!.GetDescription();
			OccupantSynthesisViewModel.DifficultiesFacedByFamily =
				GetDifficultiesFacedByFamily(accompanyingFileForSynthesisDto.DifficultiesFacedByFamily);
			OccupantSynthesisViewModel.AccompanyingFileReference =
				accompanyingFileForSynthesisDto.AccompanyingFileReference;
			OccupantSynthesisViewModel.Stage = accompanyingFileForSynthesisDto.Stage;
			OccupantSynthesisViewModel.Status = accompanyingFileForSynthesisDto.Status;
            OccupantSynthesisViewModel.MarkerNature = accompanyingFileForSynthesisDto.MarkerNature.GetDescription();
			OccupantSynthesisViewModel.SolidarBuilder = accompanyingFileForSynthesisDto.SolidarBuilder;
			OccupantSynthesisViewModel.SecondSolidarBuilder = accompanyingFileForSynthesisDto.SecondSolidarBuilder;
			OccupantSynthesisViewModel.ThirdSolidarBuilder = accompanyingFileForSynthesisDto.ThirdSolidarBuilder;
			OccupantSynthesisViewModel.DiffuseCoordinator = accompanyingFileForSynthesisDto.DiffuseCoordinator;
			OccupantSynthesisViewModel.TargetedCoordinator = accompanyingFileForSynthesisDto.TargetedCoordinator;
			OccupantSynthesisViewModel.TerritorialBuilder = accompanyingFileForSynthesisDto.TerritorialBuilder;
			OccupantSynthesisViewModel.SecondTerritorialBuilder = accompanyingFileForSynthesisDto.SecondTerritorialBuilder;
			OccupantSynthesisViewModel.GeographicAreaTypology = accompanyingFileForSynthesisDto.GeographicAreaTypology;

			_blobName = $"{OccupantSynthesisViewModel.AccompanyingFileReference}_{Labels.SignedHouseholdSupportContractLabel}";

			try
			{
				(_memoryStreamFile, _fileName) = await FileService.DownloadFileForSynthesisAsync(_blobName);
				if (_memoryStreamFile != null)
				{
					_fileData = _memoryStreamFile.ToArray();
					_fileExtension = GetFileExtension(_fileName);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred while downloading the file: {ex.Message}");
			}
		}
	}

	public static string GetFileExtension(string fileName) =>
		fileName.Split('.')[^1];

	public async Task OnChangeUpload(InputFileChangeEventArgs e)
	{
		var file = e.File;
		if (file.Size > Constants.MaxFileSize)
		{
			ErrorMessage = Labels.Errors.MaximumFileSizeExceeded;
			return;
		}

		_fileExtension = GetFileExtension(file.Name).ToLowerInvariant();

		if (_fileExtension != "pdf" && _fileExtension != "png" && _fileExtension != "jpeg" && _fileExtension != "jpg" &&
			_fileExtension != "webp")
		{
			ErrorMessage = Labels.Errors.BadUploadedFileExtension;
			return;
		}

		ErrorMessage = string.Empty;
		var memoryStream = new MemoryStream();

		await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStream);
		memoryStream.Position = 0;
		_memoryStreamFile = memoryStream;
		_fileName = file.Name;
		_fileData = memoryStream.ToArray();

		if(IsAccompanyingFileWaitingForValidation())
			await FileService.UploadFileAsync($"{_blobName}.{_fileExtension}", $"{_blobName}.{_fileExtension}", _memoryStreamFile);
	}

	public async Task ViewFile()
	{
		if (_fileData != null)
		{
			var base64 = Convert.ToBase64String(_fileData);

			var mimeType = "application/octet-stream";
			if (_fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) mimeType = "application/pdf";
			if (_fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
			if (_fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
			if (_fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) mimeType = "image/png";
			if (_fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)) mimeType = "image/webp";


			await JSRuntime.InvokeVoidAsync("viewFile", base64, mimeType, _fileName);
		}
	}

	private async Task OnClickDeleteFile()
	{
		var modalResult =
			await ModalService.Show<FileDeletionModal>(
				new ModalParameters
				{
					{ nameof(FileDeletionModal.FileName), _fileName }
				},
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
			).Result;

		if (modalResult.Cancelled)
			return;

		_memoryStreamFile = null;
		_fileData = null;
		_fileName = string.Empty;
		_fileExtension = string.Empty;

		StateHasChanged();
	}

	private static string GetDifficultiesFacedByFamily(List<string>? difficulties) =>
		difficulties?.Count > 0 ? string.Join(", ", difficulties) : string.Empty;

	private static string GetFieldWithProperUnit(object? fieldValue, string unit)
	{
		return fieldValue is not null ? $"{fieldValue} {unit}" : string.Empty;
	}


	private static string GetHouseholdDifficulties(AccompanyingFileSynthesisDto accompanyingFileForSynthesisDto)
	{
		List<string> householdDifficulties = [];

		if (accompanyingFileForSynthesisDto.HasDisabilitySituation is true)
			householdDifficulties.Add(Labels.DifficultyFacedByFamilySynthesisDisability);
		if (accompanyingFileForSynthesisDto.HasPersonWithLossOfIndependence is true)
			householdDifficulties.Add(Labels.DifficultyFacedByFamilySynthesisLossOfIndependence);
		if (accompanyingFileForSynthesisDto.HasPersonWithLongTermIllness is true)
			householdDifficulties.Add(Labels.DifficultyFacedByFamilySynthesisLongTermIllness);
		if (accompanyingFileForSynthesisDto.IsOverIndebted)
			householdDifficulties.Add(Labels.DifficultyFacedByFamilySynthesisOverIndebted);
		if (accompanyingFileForSynthesisDto.HasPersonFollowedByCuratorship is true)
			householdDifficulties.Add(Labels.DifficultyFacedByFamilySynthesisCuratorship);
		if (accompanyingFileForSynthesisDto.HasPersonFollowedByGuardianship is true)
			householdDifficulties.Add(Labels.DifficultyFacedByFamilySynthesisGuardianship);

		return string.Join(", ", householdDifficulties);
	}

	private void OnClickCancelButton() => NavigationManager.NavigateTo($"{Endpoints.NewOccupant}/{AccompanyingFileId}");

	private async Task OnSubmitSynthesis()
	{
		if (IsFinancingAsked is not true) return;

		if (_memoryStreamFile == null)
		{
			ErrorMessage = Labels.Errors.RequiredSignedHouseholdSupportContract;
			return;
		}

		var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

		var userIdString = authState.User;

		if (userIdString is null) return;

		var role = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

		if (role is null) return;

		var synthesisValidationModalViewModel = new IdentifySynthesisValidationModalViewModel
		{
			AccompanyingFileId = AccompanyingFileId,
			AnahCategory = OccupantSynthesisViewModel.AnahCategory ?? string.Empty,
			OwnershipStatus = OccupantSynthesisViewModel.OwnershipStatus ?? string.Empty,
			DpeLabel = OccupantSynthesisViewModel.DpeLabel ?? string.Empty,
			UserRole = role.Value,
			GesLabel = OccupantSynthesisViewModel.GesLabel ?? string.Empty,
		};

		_fileName = $"{_blobName}.{_fileExtension}";

		ModalService.Show<IdentifySynthesisValidationModal>(
			new ModalParameters
			{
				{ nameof(IdentifySynthesisValidationModal.ViewModel), synthesisValidationModalViewModel },
				{ nameof(IdentifySynthesisValidationModal.FileName), _fileName },
				{ nameof(IdentifySynthesisValidationModal.MemoryStream), _memoryStreamFile }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
		);
	}

	private bool IsUserAllowedToModifyDataFolder() =>
	(
			UserId == OccupantSynthesisViewModel.SolidarBuilder ||
			UserId == OccupantSynthesisViewModel.SecondSolidarBuilder ||
			UserId == OccupantSynthesisViewModel.DiffuseCoordinator ||
			UserId == OccupantSynthesisViewModel.TargetedCoordinator ||
			UserId == OccupantSynthesisViewModel.TerritorialBuilder ||
			UserId == OccupantSynthesisViewModel.SecondTerritorialBuilder ||
			UserId == OccupantSynthesisViewModel.ThirdSolidarBuilder
	) || UserRole == Constants.AdminRole;

	private bool ShouldDisplaySynthesis()
	{
		if (NavigationHistoryManager.PreviousUri!.Contains(Endpoints.NewOccupant))
			return true;

		return OccupantSynthesisViewModel.Stage >= AccompanyingFileStage.Identify ;
	}
	
	private bool ShouldDisplaySubmissionButton() => OccupantSynthesisViewModel.Stage == AccompanyingFileStage.Identify && (OccupantSynthesisViewModel.Status == AccompanyingFileStatus.InProgress || OccupantSynthesisViewModel.Status == AccompanyingFileStatus.Rejected);

	private bool IsAccompanyingFileWaitingForValidation() => OccupantSynthesisViewModel.Status == AccompanyingFileStatus.WaitingForApproval && OccupantSynthesisViewModel.Stage == AccompanyingFileStage.Identify;
	private bool IsUserSolidarBuilder => UserRole!.Equals(Constants.SolidarBuilderRole);
	public async Task StageValidation(bool isValidationPopUp)
	{
		var modal = ModalService.Show<StageValidationModal>(
				new ModalParameters {
					{ nameof(StageValidationModal.AccompanyingFileId), AccompanyingFileId },
					{ nameof(StageValidationModal.IsValidationModal), isValidationPopUp },
					{ nameof(StageValidationModal.UserId), UserId }
				},
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true }
		);

        var result = await modal.Result;

		if(result.Confirmed) NavigationManager.NavigateTo($"{Endpoints.UserCreatedAccompanyingFiles}");
	}
}