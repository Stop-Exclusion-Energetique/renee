using Renee.Domain.Enums;

namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetStatisticsForCoordinatorsQueryObjectResult
{
	public string UserFullName { get; set; } = string.Empty;
	public int AccompanyingFileInIdentifyMilestoneCount { get; set; }
	public int AccompanyingFileInOrganizingAndFinancingMilestoneCount { get; set; }
	public int AccompanyingFileInRealizingAndFollowingMilestoneCount { get; set; }
	public double HouseholdInCategoryAnahMCount { get; set; }
	public double HouseholdInCategoryAnahTmCount { get; set; }
	public double FinishedAccompanyingFile { get; set; }
	public long EstimatedRemainingAmountAverage { get; set; }
	public long WorkPackageCostAverage { get; set; }
	public double AverageAgeMainOccupant { get; set; }
	public Dictionary<MarkerNature, int>? HouseHoldsByMarkerNature { get; set; }
	public Dictionary<AnahType, int>? HouseholdsByTypesOfANAH { get; set; }
	public double AverageDeliveryTime { get; set; }
	public double TaxRevenueAverage { get; set; }
	public double AverageEnergyEffortBeforeWork { get; set; }
	public double EnergyPrivationRate { get; set; }
	public Dictionary<DpeLabel, int> HouseHoldsByInitialDpe { get; set; } = new();


	public required SocioProfessionalCategoryStatistics SocioProfessionalCategories { get; set; }
	public Dictionary<EstimatedJumpClass, int>? EstimatedEnergyClassJump { get; set; }

	public Dictionary<HouseholdTypology, int>? HouseholdTypologies { get; set; }
	public required OwnershipStatusStatistics OwnershipStatus { get; set; }

	public Dictionary<FundingType, double?>? AverageFundingByType { get; set; }
	public double AverageAccompanyingDuration { get; set; }

	public class SocioProfessionalCategoryStatistics
	{
		public long Farmer { get; set; }
		public long Artisan { get; set; }
		public long Cadre { get; set; }
		public long Employee { get; set; }
		public long SearchingJob { get; set; }
		public long Worker { get; set; }
		public long IntermediateProfession { get; set; }
		public long Retired { get; set; }
		public long Unemployed { get; set; }
	}

	public class OwnershipStatusStatistics
	{
		public long FullOwnership { get; set; }
		public long CoOwner { get; set; }
		public long JointOwnership { get; set; }
	}
}