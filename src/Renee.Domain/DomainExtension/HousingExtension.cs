using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension;

public static class HousingExtension
{
	public static Housing UpdateAddress(this Housing housing, Address updatedAddress)
	{
		housing.HousingAddressNavigation.Label = updatedAddress.Label;
		housing.HousingAddressNavigation.PostalCode = updatedAddress.PostalCode;
		housing.HousingAddressNavigation.City = updatedAddress.City;
		housing.HousingAddressNavigation.Department = updatedAddress.Department;
		housing.HousingAddressNavigation.Region = updatedAddress.Region;
		housing.HousingAddressNavigation.AdditionnalComment = updatedAddress.AdditionnalComment;
		return housing;
	}

	public static Housing UpdateHousing(this Housing housing, Housing updatedHousing)
	{
		housing.HousingType = updatedHousing.HousingType;
		housing.GeographicAreaTypology = updatedHousing.GeographicAreaTypology;
		housing.IsInAbfarea = updatedHousing.IsInAbfarea;
		housing.ArchitecturalOrTownPlanningStandards = updatedHousing.ArchitecturalOrTownPlanningStandards;
		housing.OwnershipStatus = updatedHousing.OwnershipStatus;
		housing.ConstructionYear = updatedHousing.ConstructionYear;
		housing.LivingSpace = updatedHousing.LivingSpace;
		housing.NumberOfRoom = updatedHousing.NumberOfRoom;
		housing.NumberOfFloor = updatedHousing.NumberOfFloor;
		housing.YearOfAcquisitionOrEntry = updatedHousing.YearOfAcquisitionOrEntry;
		housing.CadastralReference = updatedHousing.CadastralReference;
		housing.HasPreviousWork = updatedHousing.HasPreviousWork;
		housing.CommentOnPreviousWork = updatedHousing.CommentOnPreviousWork;

		return housing;
	}

	public static void UpdateInitialState(this Housing housing, HousingInitialState initialState)
	{
		housing.HousingInitialStateNavigation.DegradationIndex = initialState.DegradationIndex;
		housing.HousingInitialStateNavigation.UnsanitaryCoefficient = initialState.UnsanitaryCoefficient;
		housing.HousingInitialStateNavigation.SummerThermalComfortLevel = initialState.SummerThermalComfortLevel;
		housing.HousingInitialStateNavigation.WinterThermalComfortLevel = initialState.WinterThermalComfortLevel;
		housing.HousingInitialStateNavigation.NoiseComfortLevel = initialState.NoiseComfortLevel;
		housing.HousingInitialStateNavigation.AnnualEnergyConsumption = initialState.AnnualEnergyConsumption;
		housing.HousingInitialStateNavigation.AnnualGesemission = initialState.AnnualGesemission;
		housing.HousingInitialStateNavigation.EnergyDepravation = initialState.EnergyDepravation;
		housing.HousingInitialStateNavigation.Dpe = initialState.Dpe;
		housing.HousingInitialStateNavigation.Ges = initialState.Ges;
	}
}