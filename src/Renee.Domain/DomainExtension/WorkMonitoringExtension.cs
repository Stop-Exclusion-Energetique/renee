using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension;

public static class WorkMonitoringExtension
{
	public static void UpdateWorkMonitoring(this WorkMonitoring workMonitoring, WorkMonitoring updatedWorkMonitoring)
	{
		workMonitoring.AccompanyingCost = updatedWorkMonitoring.AccompanyingCost;
		workMonitoring.HouseholdSelfFinancing = updatedWorkMonitoring.HouseholdSelfFinancing;
		workMonitoring.IntermediateAirtightnessTestResult = updatedWorkMonitoring.IntermediateAirtightnessTestResult;
		workMonitoring.JustificationAndActionsPutInPlaceIfNoTest =
			updatedWorkMonitoring.JustificationAndActionsPutInPlaceIfNoTest;
		workMonitoring.HasEffectiveComplianceWithWorkRecommendations =
			updatedWorkMonitoring.HasEffectiveComplianceWithWorkRecommendations;
		workMonitoring.HasWorksEnabledHouseholdToStayAtHome =
			updatedWorkMonitoring.HasWorksEnabledHouseholdToStayAtHome;
		workMonitoring.WellBeingRating = updatedWorkMonitoring.WellBeingRating;
		workMonitoring.EducationalFrameworkRating = updatedWorkMonitoring.EducationalFrameworkRating;
		workMonitoring.FamilySatisfaction = updatedWorkMonitoring.FamilySatisfaction;
		workMonitoring.ReturnToEmployment = updatedWorkMonitoring.ReturnToEmployment;
		workMonitoring.WorkTotalCost = updatedWorkMonitoring.WorkTotalCost;
		workMonitoring.HasHousingAdaptationWorks = updatedWorkMonitoring.HasHousingAdaptationWorks;
		workMonitoring.HasFinishingWorks = updatedWorkMonitoring.HasFinishingWorks;
		workMonitoring.HasSafetyWorks = updatedWorkMonitoring.HasSafetyWorks;
		workMonitoring.HasPreparationWorks = updatedWorkMonitoring.HasPreparationWorks;
		workMonitoring.HasEmergencyWorks = updatedWorkMonitoring.HasEmergencyWorks;
		workMonitoring.HasUnsanitaryExit = updatedWorkMonitoring.HasUnsanitaryExit;
		workMonitoring.TreatedAirTightness = updatedWorkMonitoring.TreatedAirTightness;
		workMonitoring.TreatedThermalBridges = updatedWorkMonitoring.TreatedThermalBridges;
		workMonitoring.HasHumidityManagement = updatedWorkMonitoring.HasHumidityManagement;
		workMonitoring.ShouldChangeFinalEstimatedDpe = updatedWorkMonitoring.ShouldChangeFinalEstimatedDpe;
	}
}