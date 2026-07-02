using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.Index.GraphicsObjects;

namespace Renee.UI.Components.Features.Index.ViewModel;

public class IndexViewModel
{
	public string UserFullName { get; init; } = string.Empty;

	public VerticalChart[] AccompanyingFileStage { get; init; } = new VerticalChart[3];

	public HorizontalChart[] AccompanyingFileStatus { get; set; } = new HorizontalChart[2];

	public PieChart[] AnahCategoryRepartition { get; set; } = new PieChart[2];

	public long EstimatedRemainingAmountAverage { get; set; }
	public long EstimatedWorkPackageCostAverage { get; set; }
	public double AverageAgeMainOccupant { get; set; }
	public double AverageDeliveryTime { get; set; }
	public double AverageDeprivationRate { get; set; }
	public double AverageEnergyEffortBeforeWork { get; set; }

	public HorizontalChart[] HouseholdTypology { get; set; } =
		new HorizontalChart[Enum.GetNames<HouseholdTypology>().Length];

	public PieChart[] SocioProfessionalCategoryRepartition { get; set; } = new PieChart[9];
	public VerticalChart[] HouseHoldsByMarkerNatureCount { get; init; } =
		new VerticalChart[Enum.GetNames<MarkerNature>().Length];
	public VerticalChart[] EstimatedEnergyJumpCount { get; init; } = new VerticalChart[5];

	public double AverageTaxRevenue { get; set; }
	public double[] HouseHoldsByInitialDpe { get; set; } = new double[Enum.GetNames<DpeLabel>().Length];
	public PieChart[] OwnershipStatusRepartition { get; set; } = new PieChart[2];

	public double PassageRateFromFirstMilestoneToSecondMilestone { get; set; }
	public double PassageRateFromFirstMilestoneToThirdMilestone { get; set; }
	public VerticalChart[] HouseholdsByTypesOfANAHCount { get; set; } = new VerticalChart[4];

	public HorizontalChart[] AverageFundingByType { get; set; } = new HorizontalChart[8];
	public double AccompanyingDurationAverage { get; set; }
	public VerticalChart[] AverageWorkCostByEstimatedJump { get; set; } =
		new VerticalChart[Enum.GetNames<EstimatedJumpClass>().Length];

	public List<CompletionSpeedDataItem> CompletionSpeedDataIdentify { get; set; } = [];
	public List<CompletionSpeedDataItem> CompletionSpeedDataOrganizeAndFinance { get; set; } = [];
	public List<CompletionSpeedDataItem> CompletionSpeedDataRealizeAndFollow { get; set; } = [];

	public int NumberOfUser { get; set; }
	public int? MaximalNumberOfAccompanyingFileCreated { get; set; } = 0;
	public int? MaximalNumberOfAccompanyingFileToValidateFirstStage { get; set; } = 0;
	public DateTime? AccompanyingFileModificationDeadline { get; set; }

	public DateTime? AccompanyingFileAlertBannerStartDate { get; set; }
	public DateTime? AccompanyingFileAlertBannerEndDate { get; set; }
	public string? AccompanyingFileAlertBannerMessage { get; set; }
}