using Renee.Application.DTOs.WorkPackage;
using Renee.Domain.Enums;

namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetOrganizeAndFinanceSynthesisQueryObjectResult
{
	public Guid Id { get; init; }
	public string AccompanyingFileReference { get; init; } = null!;
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }

	public string? PreWorkPlanProjectType { get; init; }
	public string? RenovationType { get; init; }
	public string? NextStepAndVigilancePoints { get; init; }
	public string? IsEmergencyWorks { get; init; }
	public string? TreatedAirTightness { get; init; }
	public string? TreatedThermalBridge { get; init; }
	public string? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; init; }

	public List<WorkPackageSummaryDto> WorkPackageSummary { get; init; } = [];
	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; init; }
	public string? EstimatedEnergyDpeAfterWork { get; init; }
	public string? EstimatedEnergyClassJump { get; init; }

	public double? GuidedPathwayBonus { get; init; }
	public double? CoOwnershipBonus { get; init; }
	public double? DecentHousingBonus { get; init; }
	public double? AdaptationBonus { get; init; }
	public double? ExitEnergySieveBonus { get; init; }
	public double? Region { get; init; }
	public double? Department { get; init; }
	public double? PublicEstablishmentsIntercommunalCooperation { get; init; }
	public double? Municipality { get; init; }
	public string? BankLoanType { get; init; }
	public double? ClassicBankLoan { get; init; }
	public string? IsFinancingAsked { get; init; }
	public double? EstimatedRemainingAmount { get; init; }
	public double? DepartmentalHouseForDisabledPersons { get; init; }
	public double? EnergySavingCertificates { get; init; }
	public double? FamilyAllowanceFund { get; init; }
	public double? PensionFund { get; init; }
	public double? UnderprivilegedHousingFoundation { get; init; }
	public double? LeroyMerlinFoundation { get; init; }
	public double? WattForChangeFoundation { get; init; }
	public double? SocialProtectionGroup { get; init; }
	public double? StopEnergyExclusionFunds { get; init; }

	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? DiffuseCoordinator { get; set; }
	public Guid? TargetedCoordinator { get; set; }
	public Guid? TerritorialBuilder { get; set; }
	public Guid? SecondTerritorialBuilder { get; set; }

	public double? HouseholdMaximumSavingAmountForRenovationProject { get; init; }
	public double? MaximumAmountSupportFamilyMembersRenovationProject { get; init; }

	public List<FundingModeSummary> FundingModes { get; init; } = [];

	public string InitialDpeLabel { get; set; } = string.Empty;

	public string? AnahFolderNumber { get; set; }
	public DateTime? AnahFolderFilingDate { get; set; }
}

public class FundingModeSummary
{
	public string Label { get; set; } = null!;
	public double Value { get; set; }
}