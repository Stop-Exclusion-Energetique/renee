using System.ComponentModel.DataAnnotations;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.Occupant;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.EnergeticProfil.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.Housing.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.ReasonOfProject.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage.ViewModel;

public class AccompanyingFileCreationFormViewModel(
	HouseholdIdentityViewModel householdIdentityViewModel,
	HousingViewModel housingViewModel,
	EnergyProfileViewModel energyProfileViewModel,
	ReasonOfProjectViewModel reasonOfProjectViewModel)
{
	[ValidateComplexType] public HouseholdIdentityViewModel HouseholdIdentityViewModel { get; set; } = householdIdentityViewModel;

	[ValidateComplexType] public HousingViewModel HousingViewModel { get; set; } = housingViewModel;

	[ValidateComplexType] public EnergyProfileViewModel EnergyProfileViewModel { get; set; } = energyProfileViewModel;

	[ValidateComplexType] public ReasonOfProjectViewModel ReasonOfProjectViewModel { get; set; } = reasonOfProjectViewModel;

	public static AccompanyingFileCreationFormViewModel CreateViewModelFromAccompanyingFileDto(AccompanyingFileDto dto)
	{
		var householdIdentityViewModel = new HouseholdIdentityViewModel(
			new HouseholdViewModel(ExpensesViewModel.MapFromDtoList(dto.Expenses))
			{
				IncomeTaxReference = dto.TaxIncome,
				AnahCategory = dto.AnahCategory,
				LackOfAutonomy = dto.HasPersonWithLossOfIndependence,
				LongTermIllness = dto.HasPersonWithLongTermIllness,
				Curatorship = dto.HasPersonFollowedByCuratorship,
				Guardianship = dto.HasPersonFollowedByGuardianship,
				AskedHouseholdTypology = dto.HouseholdTypologyId,
				HasUnpaidEnergyBills = dto.HasUnpaidEnergyBills,
				Disability = dto.HasDisabilitySituation,
				FollowedBySocialWorker = dto.IsFollowedBySocialWorker,
				AskedResourcesTypology = []
			},
			MainOccupantViewModel.DtoToOccupantViewModel(dto.MainOccupant),
			ToSecondaryOccupantViewModelList(dto.SecondaryOccupants));

		if (dto.HouseholdResourcesTypologies.Count > 0)
			foreach (var item in dto.HouseholdResourcesTypologies)
				householdIdentityViewModel.HouseholdViewModel.ResourceTypologieValues.Add(
					new ResourceTypologyValueViewModel
					{
						Id = item.HouseholdResourcesTypologyId, Name = item.Name, Value = item.Value
					}
				);

		if (dto.HouseholdHeatingEnergies.Count > 0)
			foreach (var item in dto.HouseholdHeatingEnergies)
				householdIdentityViewModel.HouseholdViewModel.HeatingEnergiesValues.Add(
					new HouseholdHeatingEnergyValueViewModel
					{
						Id = item.HouseholdHeatingEnergyId, Name = item.Name, Value = item.Value
					}
				);

		var housingViewModel = new HousingViewModel
		{
			StreetNumberName = dto.Housing.Address,
			AdditionalAddress = dto.Housing.Address?.AdditionalAddress,
			Typology = dto.Housing.GeographicalTypology,
			NumberOfFloor = dto.Housing.NumberOfFloors,
			NumberOfRooms = dto.Housing.NumberOfRooms,
			Surface = dto.Housing.LivingSpaceInSquareMeter,
			AbfZoneYes = dto.Housing.IsAbfZone,
			ArchitecturalOrUrbanStandards = dto.Housing.ArchitecturalOrUrbanismStandard,
			AskedPropertyStatus = dto.Housing.OwnershipStatus,
			AskedPropertyType = dto.Housing.HousingType,
			CopropertyProfileId = dto.Housing.CopropertyProfileId,
			CadastralReference = dto.Housing.CadastralReference,
            AskedConstructionYear = dto.Housing.BuildingYear,
			YearOfacquisition = dto.Housing.AcquisitionYear,
			UnsanitaryCoefficient = dto.Housing.UnsanitaryCoefficient,
			DegradationIndex = dto.Housing.DegradationIndex,
			SoundComfort = dto.Housing.NoiseComfortLevel,
			SummerComfort = dto.Housing.SummerThermalComfortLevel,
			WinterComfort = dto.Housing.WinterThermalComfortLevel,
			RenovationWorkYes = dto.Housing.HasPreviousBuildingWork,
			RenovationExplanations = dto.Housing.ExplanationOnPreviousBuildingWork
		};

		var energyProfileViewModel = new EnergyProfileViewModel
		{
			EnergyConsumption = dto.Housing.AnnualEnergyConsumption,
			GesEmissions = dto.Housing.AnnualGesEmissions,
			EnergyDeprivation = dto.Housing.ElectricityDeprivation,
			AskedDpeLabel = dto.Housing.DpeLabel,
			AskedGesLabel = dto.Housing.GesLabel
		};

		var reasonOfProjectViewModel = new ReasonOfProjectViewModel
		{
			ReasonOfProjectSocialContext = dto.SocialContext,
			FamilyProject = dto.FamilyProject,
			VisitAvailability = dto.FamilyAvailabilityForVisits,
			DifficultiesDetails = dto.CommentOnDifficultiesFacedByFamily,
			AskedDifficulties = [],
			AccompanyingTimeDuration = dto.AccompanyingTimeDuration,
			SigningHouseholdSupportDate = dto.SigningHouseholdSupportDate,
			FirstEncounterDate = dto.FirstVisitDate,
			ShouldAccompanyingFileBeSubmittedToAnah = dto.ShouldAccompanyingFileBeSubmittedToAnah
		};

		foreach (var item in dto.DifficultiesFacedByFamily)
			reasonOfProjectViewModel.AskedDifficulties?.Add(item.DifficultyFacedByFamilyId);

		return new AccompanyingFileCreationFormViewModel(
			householdIdentityViewModel,
			housingViewModel,
			energyProfileViewModel,
			reasonOfProjectViewModel);
	}

	private static List<SecondaryOccupantViewModel> ToSecondaryOccupantViewModelList(List<SecondaryOccupantDto> dtos) =>
		dtos.Select(SecondaryOccupantViewModel.DtoToSecondaryOccupantViewModel).ToList();
}