using System.Drawing;
using Renee.Application.Helpers;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Features.Index.GraphicsObjects;
using Renee.UI.Components.Features.Index.ViewModel;

namespace Renee.UI.Components.Features.Index.Presenter;

public class IndexPresenter
{
	public double AverageDeliveryTime { get; set; }
	public double PassageRateFromFirstMilestoneToThirdMilestone { get; set; }
	private readonly PieChart[] _anahCategoryRepartitionPieChart =
	[
		new() { Name = Labels.LowIncomeHouseholdsAmount, PieAxisValue = 0 },
		new() { Name = Labels.VeryLowIncomeHouseholdsAmount, PieAxisValue = 0 }
	];

	private readonly HorizontalChart[] _statusChart =
	[
		new() { Name = IndexLabels.OnHold, HorizontalAxisValue = 0 },
		new() { Name = IndexLabels.Completed, HorizontalAxisValue = 0 }
	];

	private readonly HorizontalChart[] _householdTypology =
	[
		new() { Name = HouseholdTypology.CoupleWithAdultStaying.GetDescription(), HorizontalAxisValue = 0 },
		new() { Name = HouseholdTypology.CoupleWithChildren.GetDescription(), HorizontalAxisValue = 0 },
		new() { Name = HouseholdTypology.CoupleWithoutChildren.GetDescription(), HorizontalAxisValue = 0 },
		new() { Name = HouseholdTypology.SingleParentFamily.GetDescription(), HorizontalAxisValue = 0 },
		new() { Name = HouseholdTypology.SinglePerson.GetDescription(), HorizontalAxisValue = 0 },
		new() { Name = HouseholdTypology.SinglePersonWithAdultStaying.GetDescription(), HorizontalAxisValue = 0 }
	];

	private readonly HorizontalChart[] _averageFundingByType =
	[
		new() { Name = FundingType.Region.GetDescription(), HorizontalAxisValue = 0, Color = Color.Red },
		new() { Name = FundingType.Department.GetDescription(), HorizontalAxisValue = 0, Color = Color.RoyalBlue },
		new()
		{
			Name = FundingType.PublicEstablishmentsInterCooperation.GetDescription(),
			HorizontalAxisValue = 0,
			Color = Color.Salmon
		},
		new() { Name = FundingType.Municipality.GetDescription(), HorizontalAxisValue = 0, Color = Color.SeaGreen },
		new() { Name = FundingType.PrivateActors.GetDescription(), HorizontalAxisValue = 0, Color = Color.Silver },
		new()
		{
			Name = FundingType.PensionFunds.GetDescription(), HorizontalAxisValue = 0, Color = Color.Turquoise
		},
		new()
		{
			Name = FundingType.HouseholdMaximumSavingAmountForRenovationProject.GetDescription(),
			HorizontalAxisValue = 0,
			Color = Color.Brown
		},
		new()
		{
			Name = FundingType.MaximumAmountSupportFamilyMembersRenovationProject.GetDescription(),
			HorizontalAxisValue = 0,
			Color = Color.Cyan
		}
	];
	private readonly PieChart[] _socioProfessionalCategoryRepartitionPieChart =
	[
		new() { Name = SocioProfessionalCategoryLabel.Farmer, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.Artisan, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.Cadre, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.Employee, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.SearchingJob, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.Worker, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.IntermediateProfession, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.Retired, PieAxisValue = 0 },
		new() { Name = SocioProfessionalCategoryLabel.Unemployed, PieAxisValue = 0 }
	];


	private readonly VerticalChart[] _houseHoldsByMarkerNatureCount =
	[
		new() { Name = MarkerNature.PublicActor.GetDescription(), VerticalAxisValue = 0, Color = Color.LightCoral },
		new() { Name = MarkerNature.Association.GetDescription(), VerticalAxisValue = 0, Color = Color.Green },
		new() { Name = MarkerNature.Volunteers.GetDescription(), VerticalAxisValue = 0, Color = Color.Yellow },
		new()
		{
			Name = MarkerNature.HealthFunds.GetDescription(), VerticalAxisValue = 0, Color = Color.GreenYellow
		},
		new() { Name = MarkerNature.CityHall.GetDescription(), VerticalAxisValue = 0, Color = Color.Blue },
		new() { Name = MarkerNature.Operator.GetDescription(), VerticalAxisValue = 0, Color = Color.Cyan },
		new() { Name = MarkerNature.Slime.GetDescription(), VerticalAxisValue = 0, Color = Color.Orange },
		new() { Name = MarkerNature.DirectCall.GetDescription(), VerticalAxisValue = 0, Color = Color.DarkKhaki },
		new() { Name = MarkerNature.Other.GetDescription(), VerticalAxisValue = 0, Color = Color.Gray }
	];
	private readonly VerticalChart[] _householdsByTypesOfANAHCount =
	[
		new()
		{
			Name = AnahType.GuidedPathwayBonus.GetDescription(),
			VerticalAxisValue = 0,
			Color = Color.GreenYellow
		},
		new() { Name = AnahType.CoOwnershipBonus.GetDescription(), VerticalAxisValue = 0, Color = Color.Blue },
		new() { Name = AnahType.DecentHousingBonus.GetDescription(), VerticalAxisValue = 0, Color = Color.Brown },
		new()
		{
			Name = AnahType.GuidedPathwayBonusAdaptationBonus.GetDescription(),
			VerticalAxisValue = 0,
			Color = Color.Coral
		}
	];

	private double[] HouseholdByDpeLabel { get; } = new double[Enum.GetNames<DpeLabel>().Length];
	private readonly VerticalChart[] _estimatedEnergyClassJump =
	[
		new()
		{
			Name = EstimatedJumpClass.JumpClass2.GetDescription(),
			VerticalAxisValue = 0,
			Color = Color.LightCoral
		},
		new() { Name = EstimatedJumpClass.JumpClass3.GetDescription(), VerticalAxisValue = 0, Color = Color.Green },
		new()
		{
			Name = EstimatedJumpClass.JumpClass4.GetDescription(), VerticalAxisValue = 0, Color = Color.Yellow
		},
		new()
		{
			Name = EstimatedJumpClass.JumpClass5.GetDescription(),
			VerticalAxisValue = 0,
			Color = Color.GreenYellow
		},
		new() { Name = EstimatedJumpClass.JumpClass6.GetDescription(), VerticalAxisValue = 0, Color = Color.Blue }
	];


	private readonly PieChart[] _ownershipStatusRepartitionPieChart =
	[
		new() { Name = OwnershipStatusLabel.FullOwnership, PieAxisValue = 0 },
		new() { Name = OwnershipStatusLabel.CoOwner, PieAxisValue = 0 },
		new() { Name = OwnershipStatusLabel.JointOwnership , PieAxisValue =0}
	];
	private readonly VerticalChart[] _averageWorkCostByEstimatedJump =
	[
		new() { Name = EstimatedJumpClass.JumpClass2.GetDescription(), VerticalAxisValue = 0 },
		new() { Name = EstimatedJumpClass.JumpClass3.GetDescription(), VerticalAxisValue = 0 },
		new() { Name = EstimatedJumpClass.JumpClass4.GetDescription(), VerticalAxisValue = 0 },
		new() { Name = EstimatedJumpClass.JumpClass5.GetDescription(), VerticalAxisValue = 0 },
		new() { Name = EstimatedJumpClass.JumpClass6.GetDescription(), VerticalAxisValue = 0 }
	];

	private VerticalChart[] _stageChart =
	[
		new() { Name = IndexLabels.Identification, VerticalAxisValue = 0 },
		new() { Name = IndexLabels.OrganizeAndFinance, VerticalAxisValue = 0 },
		new() { Name = IndexLabels.RealizeAndFollow, VerticalAxisValue = 0 }
	];

	private string _userFullName = string.Empty;

	private long EstimatedRemainingAmountAverage { get; set; }
	private long EstimatedWorkPackageCostAverage { get; set; }

	private double AverageAgeMainOccupant { get; set; }

	private double AverageEnergyEffortBeforeWork { get; set; }

	private double PassageRateFromFirstMilestoneToSecondMilestone { get; set; }

	private double EnergyDeprivationRate { get; set; }
	private double AverageTaxRevenue { get; set; }

	private double _accompanyingDurationAverage { get; set; }

	private int _numberOfUser { get; set; }
	private int? _maximalNumberOfAccompanyingFileCreated { get; set; }
	private int? _maximalNumberOfAccompanyingFileToValidateFirstStage { get; set; }
	private DateTime? _accompanyingFileModificationDeadline { get; set; }
	private DateTime? _accompanyingFileAlertBannerStartDate { get; set; }
	private DateTime? _accompanyingFileAlertBannerEndDate { get; set; }
	private string? _accompanyingFileAlertBannerMessage { get; set; }

	public List<CompletionSpeedDataItem> _completionSpeedDataIdentify { get; set; } = [];
	public List<CompletionSpeedDataItem> _completionSpeedDataOrganizeAndFinance { get; set; } = [];
	public List<CompletionSpeedDataItem> _completionSpeedDataRealizeAndFollow { get; set; } = [];

	public IndexPresenter FromQuery(GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult queryResult)
	{
		SetCommonProperties(queryResult);
		_anahCategoryRepartitionPieChart[0].PieAxisValue = queryResult.HouseholdInCategoryAnahMCount;
		_anahCategoryRepartitionPieChart[1].PieAxisValue = queryResult.HouseholdInCategoryAnahTmCount;
		_statusChart[0].HorizontalAxisValue = queryResult.OnHoldAccompanyingFile;
		_statusChart[1].HorizontalAxisValue = queryResult.FinishedAccompanyingFile;
		return this;
	}

	public IndexPresenter FromQuery(GetStatisticsForCoordinatorsQueryObjectResult queryResult)
	{
		SetCommonProperties(queryResult);
		_anahCategoryRepartitionPieChart[0].PieAxisValue = queryResult.HouseholdInCategoryAnahMCount;
		_anahCategoryRepartitionPieChart[1].PieAxisValue = queryResult.HouseholdInCategoryAnahTmCount;

		AverageAgeMainOccupant = queryResult.AverageAgeMainOccupant;
		AverageDeliveryTime = queryResult.AverageDeliveryTime;

		SetPropertiesChart(queryResult);

		foreach (var householdTypology in queryResult.HouseholdTypologies!)
		{
			var typology = _householdTypology.First(n => n.Name == householdTypology.Key.GetDescription());
			typology.HorizontalAxisValue = householdTypology.Value;
		}

		_socioProfessionalCategoryRepartitionPieChart[0].PieAxisValue = queryResult.SocioProfessionalCategories.Farmer;
		_socioProfessionalCategoryRepartitionPieChart[1].PieAxisValue = queryResult.SocioProfessionalCategories.Artisan;
		_socioProfessionalCategoryRepartitionPieChart[2].PieAxisValue = queryResult.SocioProfessionalCategories.Cadre;
		_socioProfessionalCategoryRepartitionPieChart[3].PieAxisValue =
			queryResult.SocioProfessionalCategories.Employee;
		_socioProfessionalCategoryRepartitionPieChart[4].PieAxisValue =
			queryResult.SocioProfessionalCategories.SearchingJob;
		_socioProfessionalCategoryRepartitionPieChart[5].PieAxisValue = queryResult.SocioProfessionalCategories.Worker;
		_socioProfessionalCategoryRepartitionPieChart[6].PieAxisValue =
			queryResult.SocioProfessionalCategories.IntermediateProfession;
		_socioProfessionalCategoryRepartitionPieChart[7].PieAxisValue = queryResult.SocioProfessionalCategories.Retired;
		_socioProfessionalCategoryRepartitionPieChart[8].PieAxisValue =
			queryResult.SocioProfessionalCategories.Unemployed;

		foreach (var houseHoldsByMarkerNature in queryResult.HouseHoldsByMarkerNature!)
		{
			var markerNature =
				_houseHoldsByMarkerNatureCount.First(n => n.Name == houseHoldsByMarkerNature.Key.GetDescription());
			markerNature.VerticalAxisValue = houseHoldsByMarkerNature.Value;
		}

		foreach (var householdsByTypesOfANAHCount in queryResult.HouseholdsByTypesOfANAH!)
		{
			var anahType =
				_householdsByTypesOfANAHCount.First(n => n.Name == householdsByTypesOfANAHCount.Key.GetDescription());
			anahType.VerticalAxisValue = householdsByTypesOfANAHCount.Value;
		}

		EnergyDeprivationRate = queryResult.EnergyPrivationRate;

		AverageTaxRevenue = queryResult.TaxRevenueAverage;

		AverageEnergyEffortBeforeWork = queryResult.AverageEnergyEffortBeforeWork / 100;

		for (var i = 0; i < Enum.GetNames<DpeLabel>().Length; i++)
			HouseholdByDpeLabel[i] = queryResult.HouseHoldsByInitialDpe.GetValueOrDefault((DpeLabel)i);

		foreach (var estimatedEnergyClassJump in queryResult.EstimatedEnergyClassJump!)
		{
			var estimatedEnergy =
				_estimatedEnergyClassJump.First(n => n.Name == estimatedEnergyClassJump.Key.GetDescription());
			estimatedEnergy.VerticalAxisValue = estimatedEnergyClassJump.Value;
		}

		_ownershipStatusRepartitionPieChart[0].PieAxisValue = queryResult.OwnershipStatus.FullOwnership;
		_ownershipStatusRepartitionPieChart[1].PieAxisValue = queryResult.OwnershipStatus.CoOwner;
		_ownershipStatusRepartitionPieChart[2].PieAxisValue = queryResult.OwnershipStatus.JointOwnership;

        foreach (var averageFundingByType in queryResult.AverageFundingByType!)
		{
			var fundingType = _averageFundingByType.First(n => n.Name == averageFundingByType.Key.GetDescription());
			fundingType.HorizontalAxisValue = averageFundingByType.Value ?? 0;
		}

		_accompanyingDurationAverage = queryResult.AverageAccompanyingDuration;
		return this;
	}

	public IndexPresenter FromQuery(GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult queryResult)
	{
		_userFullName = queryResult.UserFullName;
		_statusChart[0].HorizontalAxisValue = queryResult.OnHoldAccompanyingFile;
		_statusChart[1].HorizontalAxisValue = queryResult.FinishedAccompanyingFile;
		EstimatedRemainingAmountAverage = queryResult.EstimatedRemainingAmountAverage;
		EstimatedWorkPackageCostAverage = queryResult.WorkPackageCostAverage;
		_statusChart[0].HorizontalAxisValue = queryResult.OnHoldAccompanyingFile;
		_statusChart[1].HorizontalAxisValue = queryResult.FinishedAccompanyingFile;
		return this;
	}

	public IndexPresenter FromQuery(GetStatisticsForTerritorialBuilderQueryResult queryResult)
	{
		_userFullName = queryResult.UserFullName;

		foreach (var averageTotalWorkCost in queryResult.EstimatedEnergyJumpCount)
		{
			var averageWorkCost =
				_averageWorkCostByEstimatedJump.First(n => n.Name == averageTotalWorkCost.Key.GetDescription());
			averageWorkCost.VerticalAxisValue = Convert.ToInt32(averageTotalWorkCost.Value);
		}

		_anahCategoryRepartitionPieChart[0].PieAxisValue = queryResult.HouseholdInCategoryAnahMCount;
		_anahCategoryRepartitionPieChart[1].PieAxisValue = queryResult.HouseholdInCategoryAnahTmCount;

		PassageRateFromFirstMilestoneToSecondMilestone = queryResult.PassageRateFromFirstMilestoneToSecondMilestone;
		PassageRateFromFirstMilestoneToThirdMilestone = queryResult.PassageRateFromFirstMilestoneToThirdMilestone;
		
		SetPropertiesChart(queryResult);
		SetCommonProperties(queryResult);

		_completionSpeedDataIdentify = queryResult.CompletionSpeedResult[AccompanyingFileStage.Identify];
		_completionSpeedDataOrganizeAndFinance = queryResult.CompletionSpeedResult[AccompanyingFileStage.OrganizingAndFinancing];
		_completionSpeedDataRealizeAndFollow = queryResult.CompletionSpeedResult[AccompanyingFileStage.RealisationAndFollowing];

		return this;
	}

	public IndexPresenter FromQuery(ReneeOperationResult<GetAdminDashboardDataQuery> queryResult)
	{
		if(queryResult.IsSuccess)
		{
			_numberOfUser = queryResult.Value!.NumberOfUser;
			_maximalNumberOfAccompanyingFileCreated = queryResult.Value.MaximalNumberOfAccompanyingFileCreated;
			_maximalNumberOfAccompanyingFileToValidateFirstStage = queryResult.Value.MaximalNumberOfAccompanyingFileToValidateFirstStage;
			_accompanyingFileModificationDeadline = queryResult.Value.AccompanyingFileModificationDeadline;
			_accompanyingFileAlertBannerStartDate = queryResult.Value.AccompanyingFileAlertBannerStartDate;
			_accompanyingFileAlertBannerEndDate = queryResult.Value.AccompanyingFileAlertBannerEndDate;
			_accompanyingFileAlertBannerMessage = queryResult.Value.AccompanyingFileAlertBannerMessage;
		}
		else
		{
			_numberOfUser = 0;
			_maximalNumberOfAccompanyingFileCreated = 0;
			_maximalNumberOfAccompanyingFileToValidateFirstStage = 0;
			_accompanyingFileModificationDeadline = null;
			_accompanyingFileAlertBannerStartDate = null;
			_accompanyingFileAlertBannerEndDate = null;
			_accompanyingFileAlertBannerMessage = null;
		}
		return this;
	}

	public IndexViewModel Present()
	{
		return new IndexViewModel
		{
			UserFullName = _userFullName,
			AccompanyingFileStage = _stageChart,
			AnahCategoryRepartition = _anahCategoryRepartitionPieChart,
			AccompanyingFileStatus = _statusChart,
			EstimatedRemainingAmountAverage = EstimatedRemainingAmountAverage,
			EstimatedWorkPackageCostAverage = EstimatedWorkPackageCostAverage,
			HouseHoldsByMarkerNatureCount = _houseHoldsByMarkerNatureCount,
			AverageAgeMainOccupant = AverageAgeMainOccupant,
			AverageDeliveryTime = AverageDeliveryTime,
			HouseholdTypology = _householdTypology,
			SocioProfessionalCategoryRepartition = _socioProfessionalCategoryRepartitionPieChart,
			OwnershipStatusRepartition = _ownershipStatusRepartitionPieChart,
			AverageDeprivationRate = EnergyDeprivationRate,
			AverageTaxRevenue = AverageTaxRevenue,
			AverageEnergyEffortBeforeWork = AverageEnergyEffortBeforeWork,
			HouseHoldsByInitialDpe = HouseholdByDpeLabel,
			EstimatedEnergyJumpCount = _estimatedEnergyClassJump,
			HouseholdsByTypesOfANAHCount = _householdsByTypesOfANAHCount,
			PassageRateFromFirstMilestoneToSecondMilestone = PassageRateFromFirstMilestoneToSecondMilestone,
			PassageRateFromFirstMilestoneToThirdMilestone = PassageRateFromFirstMilestoneToThirdMilestone,
			AverageFundingByType = _averageFundingByType,
			AccompanyingDurationAverage = _accompanyingDurationAverage,
			AverageWorkCostByEstimatedJump = _averageWorkCostByEstimatedJump,
			CompletionSpeedDataIdentify = _completionSpeedDataIdentify,
			CompletionSpeedDataOrganizeAndFinance = _completionSpeedDataOrganizeAndFinance,
			CompletionSpeedDataRealizeAndFollow = _completionSpeedDataRealizeAndFollow,
			NumberOfUser = _numberOfUser,
			MaximalNumberOfAccompanyingFileCreated = _maximalNumberOfAccompanyingFileCreated,
			MaximalNumberOfAccompanyingFileToValidateFirstStage = _maximalNumberOfAccompanyingFileToValidateFirstStage,
			AccompanyingFileModificationDeadline = _accompanyingFileModificationDeadline,
			AccompanyingFileAlertBannerStartDate = _accompanyingFileAlertBannerStartDate,
			AccompanyingFileAlertBannerEndDate = _accompanyingFileAlertBannerEndDate,
			AccompanyingFileAlertBannerMessage = _accompanyingFileAlertBannerMessage
		};
	}

	private void SetCommonProperties(dynamic queryResult)
	{
		_userFullName = queryResult.UserFullName;
		_stageChart[0].VerticalAxisValue = queryResult.AccompanyingFileInIdentifyMilestoneCount;
		_stageChart[1].VerticalAxisValue = queryResult.AccompanyingFileInOrganizingAndFinancingMilestoneCount;
		_stageChart[2].VerticalAxisValue = queryResult.AccompanyingFileInRealizingAndFollowingMilestoneCount;
		EstimatedRemainingAmountAverage = queryResult.EstimatedRemainingAmountAverage;
		EstimatedWorkPackageCostAverage = queryResult.WorkPackageCostAverage;
	}

	private void SetPropertiesChart(dynamic queryResult)
	{
		_stageChart = _stageChart.Append(
			new VerticalChart
			{
				Name = IndexLabels.FinishedAccompanyingFiles,
				VerticalAxisValue = (int)queryResult.FinishedAccompanyingFile
			}).ToArray();
		_stageChart[0].Name = IndexLabels.MilestoneIdentification;
		_stageChart[1].Name = IndexLabels.MilestoneOrganizeAndFinance;
		_stageChart[2].Name = IndexLabels.MilestoneRealizeAndFollow;
	}
}