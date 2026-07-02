using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.ToRepository;

public record SaveAndSubmitOrganizeAndFinanceMilestoneData(
	EntityChanges<PreWorkPlanProjectType> ProjectTypeChanges,
	EntityChanges<PreWorkPlanInsuranceType> InsuranceTypeChanges,
	EntityChanges<WorkPackage> WorkPackageChanges,
	EntityChanges<FundingMode> FundingModeChanges);

public record WorkPackageUpdate(List<WorkPackage> WorkPackagesToAdd, List<WorkPackage> WorkPackagesToRemove);

public record WorkPackageToUpdate(
	WorkPackage UpdatedWorkPackage,
	List<WorkPackageWorkTypeCost> WorkTypeCostsToAdd,
	List<WorkPackageWorkTypeCost> WorkTypeCostsToUpdate,
	List<WorkPackageWorkTypeCost> WorkTypeCostsToRemove);