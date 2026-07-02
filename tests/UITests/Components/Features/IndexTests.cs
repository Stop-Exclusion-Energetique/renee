using System.Security.Claims;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Features.Index.Presenter;
using static Renee.Application.Queries.AccompanyingFile.QueryObjectResult.GetStatisticsForCoordinatorsQueryObjectResult;
using Constants = Renee.Domain.Constants;
using Index = Renee.UI.Components.Features.Index.Index;

namespace UITests.Components.Features;

public class IndexTests : BunitContext
{
	public IndexTests()
	{
		Services.AddSingleton(A.Fake<IAdministrationConstantsManagementService>());
	}

	[Fact]
	public void OnCalculateCountOfEstimatedClassJump_WhenUserRoleIsDiffuseCoordinator_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.DiffuseCoordinatorRole)
		};
		authContext.SetClaims(claims);

		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForCoordinatorsQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 0,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature =
					new Dictionary<MarkerNature, int>
					{
						{ MarkerNature.PublicActor, 2 },
						{ MarkerNature.Association, 0 },
						{ MarkerNature.Volunteers, 0 },
						{ MarkerNature.HealthFunds, 0 },
						{ MarkerNature.CityHall, 0 },
						{ MarkerNature.Operator, 0 },
						{ MarkerNature.Slime, 0 },
						{ MarkerNature.DirectCall, 0 },
						{ MarkerNature.Other, 0 }
					},
				EstimatedEnergyClassJump =
					new Dictionary<EstimatedJumpClass, int>
					{
						{ EstimatedJumpClass.JumpClass2, 2 },
						{ EstimatedJumpClass.JumpClass3, 0 },
						{ EstimatedJumpClass.JumpClass4, 0 },
						{ EstimatedJumpClass.JumpClass5, 0 },
						{ EstimatedJumpClass.JumpClass6, 0 }
					},
				AverageAgeMainOccupant = 25.6,
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				AverageEnergyEffortBeforeWork = 30,
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 }
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var estimatedEnergyJumpCount = cut.Instance.ViewModel!.EstimatedEnergyJumpCount[0].VerticalAxisValue;

		//Assert
		estimatedEnergyJumpCount.Should().Be(2);
	}

	[Fact]
	public void OnCalculateCountOfHouseHoldMarkerNature_WhenUserRoleIsDiffuseCoordinator_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.DiffuseCoordinatorRole)
		};
		authContext.SetClaims(claims);

		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForCoordinatorsQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				//OnHoldAccompanyingFile = 6,
				FinishedAccompanyingFile = 0,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature =
					new Dictionary<MarkerNature, int>
					{
						{ MarkerNature.PublicActor, 2 },
						{ MarkerNature.Association, 0 },
						{ MarkerNature.Volunteers, 0 },
						{ MarkerNature.HealthFunds, 0 },
						{ MarkerNature.CityHall, 0 },
						{ MarkerNature.Operator, 0 },
						{ MarkerNature.Slime, 0 },
						{ MarkerNature.DirectCall, 0 },
						{ MarkerNature.Other, 0 }
					},
				EstimatedEnergyClassJump =
					new Dictionary<EstimatedJumpClass, int>
					{
						{ EstimatedJumpClass.JumpClass2, 2 },
						{ EstimatedJumpClass.JumpClass3, 0 },
						{ EstimatedJumpClass.JumpClass4, 0 },
						{ EstimatedJumpClass.JumpClass5, 0 },
						{ EstimatedJumpClass.JumpClass6, 0 }
					},
				AverageAgeMainOccupant = 25.6,
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 },
				AverageEnergyEffortBeforeWork = 30
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var holdsByMarkerNatureCount = cut.Instance.ViewModel!.HouseHoldsByMarkerNatureCount[0].VerticalAxisValue;

		//Assert
		holdsByMarkerNatureCount.Should().Be(2);
	}


	[Fact]
	public void
		OnCalculatePassageRateFromFirstStageToThirdStage_WhenUserRoleIsTerritorialBuilder_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.TerritorialBuilderRole)
		};
		authContext.SetClaims(claims);

		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForTerritorialBuilderQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForTerritorialBuilderQueryResult>.Success(
			new GetStatisticsForTerritorialBuilderQueryResult
			{
				UserFullName = "TestBuilderName",
				PassageRateFromFirstMilestoneToThirdMilestone = 50,
				CompletionSpeedResult = new Dictionary<AccompanyingFileStage, List<CompletionSpeedDataItem>>
				{
					{ AccompanyingFileStage.Identify, new List<CompletionSpeedDataItem>() },
					{ AccompanyingFileStage.OrganizingAndFinancing, new List<CompletionSpeedDataItem>() },
					{ AccompanyingFileStage.RealisationAndFollowing, new List<CompletionSpeedDataItem>() }
				}
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var passageRateFromFirstMilestoneToThirdMilestone =
			cut.Instance.ViewModel!.PassageRateFromFirstMilestoneToThirdMilestone;

		//Assert
		passageRateFromFirstMilestoneToThirdMilestone.Should().Be(50);
	}

	[Fact]
	public void OnFilterChange_WhenUserRoleIsEsAndFilterValueIsMyReportingStructure_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.SolidarBuilderRole)
		};
		authContext.SetClaims(claims);

		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForSolidarBuilderAndStructuralReferentQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>.Success(
			new GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				AccompanyingFileInIdentifyMilestoneCount = 1,
				AccompanyingFileInOrganizingAndFinancingMilestoneCount = 2,
				AccompanyingFileInRealizingAndFollowingMilestoneCount = 3,
				HouseholdInCategoryAnahMCount = 4,
				HouseholdInCategoryAnahTmCount = 2,
				OnHoldAccompanyingFile = 6,
				FinishedAccompanyingFile = 0,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500
			}));

		A.CallTo(
			() => mockSendEventQuery.Send(
				A<GetStatisticsForSolidarBuilderAndStructuralReferentQuery>.That.Matches(
					query => query.ShouldFilterOnUserReportingStructure))).Returns(
			ReneeOperationResult<GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult>.Success(
			new GetStatisticsForSolidarBuilderAndStructuralReferentQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				AccompanyingFileInIdentifyMilestoneCount = 10,
				AccompanyingFileInOrganizingAndFinancingMilestoneCount = 20,
				AccompanyingFileInRealizingAndFollowingMilestoneCount = 30,
				HouseholdInCategoryAnahMCount = 40,
				HouseholdInCategoryAnahTmCount = 20,
				OnHoldAccompanyingFile = 60,
				FinishedAccompanyingFile = 0,
				EstimatedRemainingAmountAverage = 85660,
				WorkPackageCostAverage = 1250
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var onHoldAccompanyingFileBeforeFilter = cut.Instance.ViewModel!.AccompanyingFileStatus[0].HorizontalAxisValue;

		cut.Instance.FilterViewModel.FilterValue = FilterValue.MyReportingStructure;

		cut.WaitForAssertion(() => cut.Instance?.OnFilterChange());

		var onHoldAccompanyingFileAfterFilter = cut.Instance.ViewModel!.AccompanyingFileStatus[0].HorizontalAxisValue;

		//Assert
		onHoldAccompanyingFileBeforeFilter.Should().Be(6);
		onHoldAccompanyingFileAfterFilter.Should().Be(60);
	}


	[Fact]
	public void OnSelectedESIds_WhenUserRoleIsDiffuseCoordinator_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.DiffuseCoordinatorRole)
		};
		authContext.SetClaims(claims);

		List<Guid?> selectedEsIds = [Guid.NewGuid(), Guid.NewGuid()];
		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForCoordinatorsQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 0,
				AccompanyingFileInIdentifyMilestoneCount = 6,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature = new Dictionary<MarkerNature, int>(),
				EstimatedEnergyClassJump = new Dictionary<EstimatedJumpClass, int>(),
				AverageAgeMainOccupant = 25.6,
				AverageDeliveryTime = 20,
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 },
				AverageEnergyEffortBeforeWork = 30
			}));

		A.CallTo(
			() => mockSendEventQuery.Send(
				A<GetStatisticsForCoordinatorsQuery>.That.Matches(
					query => query.SelectedSolidarBuilderIds == selectedEsIds))).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 0,
				AccompanyingFileInIdentifyMilestoneCount = 60,
				EstimatedRemainingAmountAverage = 85660,
				WorkPackageCostAverage = 1250,
				AverageAgeMainOccupant = 25.6,
				AverageDeliveryTime = 20,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature = new Dictionary<MarkerNature, int>(),
				EstimatedEnergyClassJump = new Dictionary<EstimatedJumpClass, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 },
				AverageEnergyEffortBeforeWork = 30
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var onIdentifierAccompanyingFileBeforeFilter =
			cut.Instance.ViewModel!.AccompanyingFileStage[0].VerticalAxisValue;

		cut.Instance.FilterViewModel.SelectedSolidarBuilder = selectedEsIds;

		cut.WaitForAssertion(() => cut.Instance?.OnSolidarBuilderChanged());

		var onIdentifierAccompanyingFileAfterFilter =
			cut.Instance.ViewModel!.AccompanyingFileStage[0].VerticalAxisValue;

		//Assert
		onIdentifierAccompanyingFileBeforeFilter.Should().Be(6);
		onIdentifierAccompanyingFileAfterFilter.Should().Be(60);
	}

	[Fact]
	public void OnSelectedReportingStructuresIds_WhenUserRoleIsAssociationMember_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.AssociationMemberRole)
		};
		authContext.SetClaims(claims);

		List<Guid?> selectedStructuresIds = [Guid.NewGuid(), Guid.NewGuid()];
		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetAccompanyingFileStatisticsForAssociationMemberQuery>._)).Returns(
			ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>.Success(
			new GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				OnHoldAccompanyingFile = 6,
				FinishedAccompanyingFile = 0,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500
			}));

		A.CallTo(
			() => mockSendEventQuery.Send(
				A<GetAccompanyingFileStatisticsForAssociationMemberQuery>.That.Matches(
					query => query.SelectedReportingStructuresIds == selectedStructuresIds))).Returns(
			ReneeOperationResult<GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult>.Success(
			new GetAccompanyingFileStatisticsForAssociationMemberQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				OnHoldAccompanyingFile = 60,
				FinishedAccompanyingFile = 0,
				EstimatedRemainingAmountAverage = 85660,
				WorkPackageCostAverage = 1250
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var onHoldAccompanyingFileBeforeFilter = cut.Instance.ViewModel!.AccompanyingFileStatus[0].HorizontalAxisValue;

		cut.Instance.FilterViewModel.SelectedReportingStructures = selectedStructuresIds;

		cut.WaitForAssertion(() => cut.Instance?.OnReportingStructuresChanged());

		var onHoldAccompanyingFileAfterFilter = cut.Instance.ViewModel!.AccompanyingFileStatus[0].HorizontalAxisValue;

		//Assert
		onHoldAccompanyingFileBeforeFilter.Should().Be(6);
		onHoldAccompanyingFileAfterFilter.Should().Be(60);
	}

	[Fact]
	public void OnSelectedReportingStructuresIds_WhenUserRoleIsDiffuseCoordinator_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.DiffuseCoordinatorRole)
		};
		authContext.SetClaims(claims);

		List<Guid?> selectedStructuresIds = [Guid.NewGuid(), Guid.NewGuid()];
		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForCoordinatorsQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 60,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500,
				AverageAgeMainOccupant = 25.6,
				AverageDeliveryTime = 20,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature = new Dictionary<MarkerNature, int>(),
				EstimatedEnergyClassJump = new Dictionary<EstimatedJumpClass, int>(),
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 },
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>()
			}));

		A.CallTo(
			() => mockSendEventQuery.Send(
				A<GetStatisticsForCoordinatorsQuery>.That.Matches(
					query => query.SelectedReportingStructuresIds == selectedStructuresIds))).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 6,
				EstimatedRemainingAmountAverage = 85660,
				WorkPackageCostAverage = 1250,
				AverageAgeMainOccupant = 25.6,
				AverageDeliveryTime = 20,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature = new Dictionary<MarkerNature, int>(),
				EstimatedEnergyClassJump = new Dictionary<EstimatedJumpClass, int>(),
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 }
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var onFinishedAccompanyingFileBeforeFilter = cut.Instance.ViewModel!.AccompanyingFileStage[3].VerticalAxisValue;

		cut.Instance.FilterViewModel.SelectedReportingStructures = selectedStructuresIds;

		cut.WaitForAssertion(() => cut.Instance?.OnReportingStructuresCoordinatorOrTerritorialBuilderChanged());

		var onFinishedAccompanyingFileAfterFilter = cut.Instance.ViewModel!.AccompanyingFileStage[3].VerticalAxisValue;

		//Assert
		onFinishedAccompanyingFileBeforeFilter.Should().Be(60);
		onFinishedAccompanyingFileAfterFilter.Should().Be(6);
	}

	[Fact]
	public void OnSelectedTerritoriesIds_WhenUserRoleIsTargetedCoordinator_ShouldChangeDataValue()
	{
		//Arrange
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<IReportingStructureService>());
		Services.AddSingleton(A.Fake<IUserService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<ICguVersionService>());
        Services.AddSingleton(A.Fake<IFileViewerService>());

        var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.TargetedCoordinatorRole)
		};
		authContext.SetClaims(claims);

		List<Guid?> selectedTerritoriesIds = [Guid.NewGuid(), Guid.NewGuid()];
		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<LoadFilterDataBasedOnRoleQuery>._))
			.Returns(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(new LoadAccompanyingFileListDataQueryObjectResult()));

		A.CallTo(() => mockSendEventQuery.Send(A<GetStatisticsForCoordinatorsQuery>._)).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 60,
				EstimatedRemainingAmountAverage = 1563,
				WorkPackageCostAverage = 1500,
				AverageAgeMainOccupant = 25.6,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature = new Dictionary<MarkerNature, int>(),
				EstimatedEnergyClassJump = new Dictionary<EstimatedJumpClass, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 },
				AverageEnergyEffortBeforeWork = 30
			}));

		A.CallTo(
			() => mockSendEventQuery.Send(
				A<GetStatisticsForCoordinatorsQuery>.That.Matches(
					query => query.SelectedTerritoriesIds == selectedTerritoriesIds))).Returns(
			ReneeOperationResult<GetStatisticsForCoordinatorsQueryObjectResult>.Success(
			new GetStatisticsForCoordinatorsQueryObjectResult
			{
				UserFullName = "TestBuilderName",
				FinishedAccompanyingFile = 6,
				EstimatedRemainingAmountAverage = 85660,
				WorkPackageCostAverage = 1250,
				AverageAgeMainOccupant = 25.6,
				AverageFundingByType = new Dictionary<FundingType, double?>(),
				HouseHoldsByMarkerNature = new Dictionary<MarkerNature, int>(),
				EstimatedEnergyClassJump = new Dictionary<EstimatedJumpClass, int>(),
				HouseholdsByTypesOfANAH = new Dictionary<AnahType, int>(),
				HouseholdTypologies = new Dictionary<HouseholdTypology, int>(),
				SocioProfessionalCategories = new SocioProfessionalCategoryStatistics
				{
					Farmer = 1,
					Artisan = 2,
					Cadre = 3,
					Employee = 4,
					SearchingJob = 5,
					Worker = 6,
					IntermediateProfession = 7,
					Retired = 8,
					Unemployed = 9
				},
				OwnershipStatus = new OwnershipStatusStatistics { FullOwnership = 3, CoOwner = 5, JointOwnership = 2 },
				AverageEnergyEffortBeforeWork = 30
			}));

		Services.AddSingleton(mockSendEventQuery);

		var cut = Render<Index>();

		cut.WaitForState(() => cut.Instance.ViewModel is not null);

		//Act
		var onFinishedAccompanyingFileBeforeFilter = cut.Instance.ViewModel!.AccompanyingFileStage[3].VerticalAxisValue;

		cut.Instance.FilterViewModel.SelectedTerritories = selectedTerritoriesIds;

		cut.WaitForAssertion(() => cut.Instance?.OnTerritoriesChanged());

		var onFinishedAccompanyingFileAfterFilter = cut.Instance.ViewModel!.AccompanyingFileStage[3].VerticalAxisValue;

		//Assert
		onFinishedAccompanyingFileBeforeFilter.Should().Be(60);
		onFinishedAccompanyingFileAfterFilter.Should().Be(6);
	}
}
