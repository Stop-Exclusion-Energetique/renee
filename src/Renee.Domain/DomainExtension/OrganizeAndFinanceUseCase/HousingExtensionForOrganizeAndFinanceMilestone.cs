using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;

public static class HousingExtensionForOrganizeAndFinanceMilestone
{
	public static Housing UpdateHousing(this Housing housing, Housing updatedHousing)
	{
		housing.NumberOfRoom = updatedHousing.NumberOfRoom;
		housing.NumberOfDoor = updatedHousing.NumberOfDoor;
		housing.NumberOfWindow = updatedHousing.NumberOfWindow;
		housing.NumberOfPatioDoor = updatedHousing.NumberOfPatioDoor;
		housing.NumberOfRoofDoor = updatedHousing.NumberOfRoofDoor;
		housing.NumberOfBayWindow = updatedHousing.NumberOfBayWindow;
		housing.CeilingHeight = updatedHousing.CeilingHeight;
		housing.SunExposure = updatedHousing.SunExposure;

		return housing;
	}

	public static void UpdateHousingAfterWorkState(this Housing housing, HousingAfterWorkState afterWorkState)
	{
        housing.HousingAfterWorkStateNavigation.EstimatedAnnualEnergyConsumptionAfterWork = 
			afterWorkState.EstimatedAnnualEnergyConsumptionAfterWork;
		housing.HousingAfterWorkStateNavigation.EstimatedAnnualGesemissionsAfterWork =
			afterWorkState.EstimatedAnnualGesemissionsAfterWork;
		housing.HousingAfterWorkStateNavigation.EstimatedDpeafterWork = afterWorkState.EstimatedDpeafterWork;
		housing.HousingAfterWorkStateNavigation.EstimatedGesafterWork = afterWorkState.EstimatedGesafterWork;
		housing.HousingAfterWorkStateNavigation.EstimatedDpeclassJump = afterWorkState.EstimatedDpeclassJump;
		housing.HousingAfterWorkStateNavigation.FinalDpe = afterWorkState.FinalDpe;
		housing.HousingAfterWorkStateNavigation.FinalDpeClassJump = afterWorkState.FinalDpeClassJump;
	}

	public static Housing UpdateInitialState(this Housing housing, HousingInitialState initialState)
	{
        housing.HousingInitialStateNavigation.RoofingState = initialState.RoofingState;
		housing.HousingInitialStateNavigation.WallsState = initialState.WallsState;
		housing.HousingInitialStateNavigation.FloorState = initialState.FloorState;
		housing.HousingInitialStateNavigation.ElectricalSafetyState = initialState.ElectricalSafetyState;
		housing.HousingInitialStateNavigation.GasSafetyState = initialState.GasSafetyState;
		housing.HousingInitialStateNavigation.FireSafetyState = initialState.FireSafetyState;
		housing.HousingInitialStateNavigation.VentilationState = initialState.VentilationState;
		housing.HousingInitialStateNavigation.CarpentryState = initialState.CarpentryState;
		housing.HousingInitialStateNavigation.HeatingState = initialState.HeatingState;
		housing.HousingInitialStateNavigation.HotWaterProductionState = initialState.HotWaterProductionState;
		housing.HousingInitialStateNavigation.HumidityState = initialState.HumidityState;
		housing.HousingInitialStateNavigation.LeadAndAsbestosState = initialState.LeadAndAsbestosState;
		housing.HousingInitialStateNavigation.SanitaryPlumbingState = initialState.SanitaryPlumbingState;
		housing.HousingInitialStateNavigation.InteriorDesignState = initialState.InteriorDesignState;
		housing.HousingInitialStateNavigation.InitialStateDiagnosticCommentary =
			initialState.InitialStateDiagnosticCommentary;
		housing.HousingInitialStateNavigation.HasPestOrMold = initialState.HasPestOrMold;
		housing.HousingInitialStateNavigation.HasFaultyElectricalSystem = initialState.HasFaultyElectricalSystem;
		housing.HousingInitialStateNavigation.HasVentilationSystem = initialState.HasVentilationSystem;
		housing.HousingInitialStateNavigation.HasHeatingSystem = initialState.HasHeatingSystem;
		housing.HousingInitialStateNavigation.HasHotWaterProduction = initialState.HasHotWaterProduction;
		housing.HousingInitialStateNavigation.HasOpenings = initialState.HasOpenings;
		housing.HousingInitialStateNavigation.HasInsulation = initialState.HasInsulation;
		housing.HousingInitialStateNavigation.HasHousingCover = initialState.HasHousingCover;
		housing.HousingInitialStateNavigation.DisordersObservedCommentary = initialState.DisordersObservedCommentary;
		housing.HousingInitialStateNavigation.Dpe = initialState.Dpe;
		return housing;
	}
}