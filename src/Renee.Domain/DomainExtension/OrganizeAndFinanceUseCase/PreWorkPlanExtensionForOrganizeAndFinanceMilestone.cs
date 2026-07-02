using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;

public static class PreWorkPlanExtensionForOrganizeAndFinanceMilestone
{
	public static void UpdatePreWorkPlan(this PreWorkPlan preWorkPlan, PreWorkPlan updatedPreWorkPlan)
	{
        preWorkPlan.RenovationType = updatedPreWorkPlan.RenovationType;
		preWorkPlan.NextStepAndVigilancePoint = updatedPreWorkPlan.NextStepAndVigilancePoint;
		preWorkPlan.HasNeedForTemporaryReHousing = updatedPreWorkPlan.HasNeedForTemporaryReHousing;
		preWorkPlan.HasInterestInPossibleAraprocess = updatedPreWorkPlan.HasInterestInPossibleAraprocess;
		preWorkPlan.IsAraopeningStatementSent = updatedPreWorkPlan.IsAraopeningStatementSent;
		preWorkPlan.HasEmergencyWorks = updatedPreWorkPlan.HasEmergencyWorks;
		preWorkPlan.HasEnergeticsRenovationWorks = updatedPreWorkPlan.HasEnergeticsRenovationWorks;
		preWorkPlan.HasInducedWorks = updatedPreWorkPlan.HasInducedWorks;
		preWorkPlan.HasSafetyAndHealthWorks = updatedPreWorkPlan.HasSafetyAndHealthWorks;
		preWorkPlan.TreatedAirTightness = updatedPreWorkPlan.TreatedAirTightness;
		preWorkPlan.TreatedThermalBridge = updatedPreWorkPlan.TreatedThermalBridge;
		preWorkPlan.AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
			updatedPreWorkPlan.AreExistingHumidityAndVaporMigrationManagedAfterTreatment;
		preWorkPlan.IsHouseholdReadyToStartAraprocess = updatedPreWorkPlan.IsHouseholdReadyToStartAraprocess;
		preWorkPlan.AreHouseholdPhysicalCapacitiesTakenIntoAccount =
			updatedPreWorkPlan.AreHouseholdPhysicalCapacitiesTakenIntoAccount;
		preWorkPlan.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean =
			updatedPreWorkPlan.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean;
		preWorkPlan.WorksDetails = updatedPreWorkPlan.WorksDetails;
		preWorkPlan.HouseholdAvailabilitiyToOrganizeArasite =
			updatedPreWorkPlan.HouseholdAvailabilitiyToOrganizeArasite;
		preWorkPlan.IsRgeLabelUpToDate = updatedPreWorkPlan.IsRgeLabelUpToDate;
		preWorkPlan.OtherQualification = updatedPreWorkPlan.OtherQualification;
	}

	public static EntityChanges<PreWorkPlanInsuranceType> UpdatePreWorkPlanInsuranceTypes(
		this PreWorkPlan preWorkPlan,
		List<Guid> insuranceTypes)
	{
        var insuranceTypesToRemove = preWorkPlan.PreWorkPlanInsuranceTypes
			.Where(pt => !insuranceTypes.Exists(id => id == pt.InsuranceType)).ToList();

		foreach (var insuranceType in insuranceTypesToRemove) insuranceType.PreWorkPlan = preWorkPlan.Id;

		var insuranceTypesToAddId = insuranceTypes
			.Where(ud => preWorkPlan.PreWorkPlanInsuranceTypes.All(d => d.InsuranceType != ud)).ToList();

		var insuranceTypesToAdd = insuranceTypesToAddId.Select(
				insurance => new PreWorkPlanInsuranceType { PreWorkPlan = preWorkPlan.Id, InsuranceType = insurance })
			.ToList();

		return new EntityChanges<PreWorkPlanInsuranceType>(
			insuranceTypesToAdd,
			[],
			insuranceTypesToRemove);
	}

	public static EntityChanges<PreWorkPlanProjectType> UpdatePreWorkPlanProjectTypes(
		this PreWorkPlan preWorkPlan,
		List<Guid> projectTypes)
	{
		var projectTypesToRemove = preWorkPlan.PreWorkPlanProjectTypes
			.Where(pt => !projectTypes.Exists(id => id == pt.ProjectType)).ToList();

		foreach (var projectType in projectTypesToRemove) projectType.PreWorkPlan = preWorkPlan.Id;

		var projectTypesToAddId = projectTypes
			.Where(ud => preWorkPlan.PreWorkPlanProjectTypes.All(d => d.ProjectType != ud)).ToList();

		var projectTypesToAdd = projectTypesToAddId.Select(
				projectType => new PreWorkPlanProjectType { PreWorkPlan = preWorkPlan.Id, ProjectType = projectType })
			.ToList();

		return new EntityChanges<PreWorkPlanProjectType>(
			projectTypesToAdd,
			[],
			projectTypesToRemove);
	}

	public static EntityChanges<WorkPackage> UpdatePreWorkPlanWorkPackages(
		this PreWorkPlan preWorkPlan,
		List<WorkPackage> updatedWorkPackages)
	{
        var itemToRemove = preWorkPlan.WorkPackages.Where(wp => !updatedWorkPackages.Exists(uwp => uwp.Id == wp.Id))
			.ToList();

		foreach (var item in itemToRemove) item.PreWorkPlan = preWorkPlan.Id;

		var itemToAdd = updatedWorkPackages.Where(uwp => preWorkPlan.WorkPackages.All(wp => wp.Id != uwp.Id)).ToList();

		foreach (var item in itemToAdd) item.PreWorkPlan = preWorkPlan.Id;

		var itemToUpdate = updatedWorkPackages.Where(uwp => preWorkPlan.WorkPackages.Any(wp => wp.Id == uwp.Id))
			.ToList();

		foreach (var item in itemToUpdate) item.PreWorkPlan = preWorkPlan.Id;

		return new EntityChanges<WorkPackage>(
			itemToAdd,
			itemToUpdate,
			itemToRemove);
	}

	public static EntityChanges<WorkPackageWorkTypeCost> UpdateWorkTypeCosts(this WorkPackage updatedWorkPackage, WorkPackage oldWorkPackage)
	{
		var itemToRemove = oldWorkPackage.WorkPackageWorkTypeCosts
			.Where(wtc => !updatedWorkPackage.WorkPackageWorkTypeCosts.ToList().Exists(uwtc => uwtc.WorkType == wtc.WorkType)).ToList();

		foreach (var item in itemToRemove) item.WorkPackage = updatedWorkPackage.Id;

		var itemToAdd = updatedWorkPackage.WorkPackageWorkTypeCosts
			.Where(uwp => oldWorkPackage.WorkPackageWorkTypeCosts.All(wp => wp.WorkType != uwp.WorkType)).ToList();

		foreach (var item in itemToAdd) item.WorkPackage = updatedWorkPackage.Id;

		var itemToUpdate = updatedWorkPackage.WorkPackageWorkTypeCosts
			.Where(uwp => oldWorkPackage.WorkPackageWorkTypeCosts.Any(wp => wp.WorkType == uwp.WorkType)).ToList();

		foreach (var item in itemToUpdate) item.WorkPackage = updatedWorkPackage.Id;

		return new EntityChanges<WorkPackageWorkTypeCost>(
			itemToAdd,
			itemToUpdate,
			itemToRemove);
	}
}