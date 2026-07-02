using FakeItEasy;
using FluentAssertions;
using Renee.Application.Handlers.QueryHandlers.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace QueryHandlerTests.Index;

public class GetStatisticsForTerritorialBuilderQueryHandlerTests
{
	private readonly IAccompanyingFileRepository _mockAccompanyingFileRepository;
	private readonly IUserRepository _mockUserRepository;
	private readonly ITelemetryService _mockTelemetryService;

	public GetStatisticsForTerritorialBuilderQueryHandlerTests()
	{
		_mockAccompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
		_mockUserRepository = A.Fake<IUserRepository>();
		_mockTelemetryService = A.Fake<ITelemetryService>();
	}

	private static GetStatisticsForTerritorialBuilderQuery CreateDefaultRequest()
	{
		return new GetStatisticsForTerritorialBuilderQuery
		{
			ConnectedUserId = Guid.NewGuid(),
			DateFrom = null,
			DateTo = null,
			ShouldFilterOnUserAllAccompanyingFile = false,
			SelectedReportingStructuresIds = null,
			SelectedSolidarBuilderIds = null,
			DpeLabelFilter = null
		};
	}

	private static List<AccompanyingFile> CreateAccompanyingFiles(params AccompanyingFile[] files)
	{
		return [.. files];
	}

	[Fact]
	public async Task AllFilesInIdentifyPhase_ShouldCountAllInIdentify()
	{
		// Arrange
		var accompanyingFiles = CreateAccompanyingFiles(
			new AccompanyingFile
			{
				OpeningDate = DateTime.Now,
				AccompanyingFileHousingNavigation = new Housing()
				{
					HousingAfterWorkStateNavigation = new HousingAfterWorkState()
					{
						EstimatedDpeclassJump = (int)EstimatedJumpClass.JumpClass4
					}
				},
				AccompanyingFileHouseholdNavigation = new Household()
				{
					AnahCategory = Labels.LowIncomeHouseholdsAmount
				},
				AccompanyingFilePreWorkPlan = Guid.NewGuid(),
				AccompanyingFilePreWorkPlanNavigation = new PreWorkPlan(),
				AccompanyingFilePreFinancingPlan = Guid.NewGuid(),
				AccompanyingFilePreFinancingPlanNavigation = new PreFinancingPlan(),
				IdentifySynthesisValidationDate = null,
				OrganizeAndFinanceSynthesisValidationDate = null,
				RealizeAndFollowSynthesisValidationDate = null
			},
			new AccompanyingFile
			{
				OpeningDate = DateTime.Now,
				AccompanyingFileHousingNavigation = new Housing()
				{
					HousingAfterWorkStateNavigation = new HousingAfterWorkState()
					{
						EstimatedDpeclassJump = (int)EstimatedJumpClass.JumpClass4
					}
				},
				AccompanyingFileHouseholdNavigation = new Household()
				{
					AnahCategory = Labels.LowIncomeHouseholdsAmount
				},
				AccompanyingFilePreWorkPlan = Guid.NewGuid(),
				AccompanyingFilePreWorkPlanNavigation = new PreWorkPlan(),
				AccompanyingFilePreFinancingPlan = Guid.NewGuid(),
				AccompanyingFilePreFinancingPlanNavigation = new PreFinancingPlan(),
				IdentifySynthesisValidationDate = null,
				OrganizeAndFinanceSynthesisValidationDate = null,
				RealizeAndFollowSynthesisValidationDate = null
			},
			new AccompanyingFile
			{
				OpeningDate = DateTime.Now.AddMonths(-1),
				AccompanyingFileHousingNavigation = new Housing()
				{
					HousingAfterWorkStateNavigation = new HousingAfterWorkState()
					{
						EstimatedDpeclassJump = (int)EstimatedJumpClass.JumpClass4
					}
				},
				AccompanyingFileHouseholdNavigation = new Household()
				{
					AnahCategory = Labels.LowIncomeHouseholdsAmount
				},
				AccompanyingFilePreWorkPlan = Guid.NewGuid(),
				AccompanyingFilePreWorkPlanNavigation = new PreWorkPlan(),
				AccompanyingFilePreFinancingPlan = Guid.NewGuid(),
				AccompanyingFilePreFinancingPlanNavigation = new PreFinancingPlan(),
				IdentifySynthesisValidationDate = null,
				OrganizeAndFinanceSynthesisValidationDate = null,
				RealizeAndFollowSynthesisValidationDate = null
			}
		);

		A.CallTo(() => _mockAccompanyingFileRepository.GetStatisticsForTerritorialBuilderIndex(
			A<Guid>._,
			A<DateTime?>._,
			A<DateTime?>._,
			false,
			A<AccompanyingFilesStatisticsFilterOptions>._
		)).Returns(accompanyingFiles);

		var expectedIdentify = new List<CompletionSpeedDataItem>
		{
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-5).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-4).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-3).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-2).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-1).ToString("MMM"),  AccompanyingFileCount = 1 },
			new() { AbbreviatedMonth = DateTime.Now.ToString("MMM"), AccompanyingFileCount = 3 }
        };

		var expectedOrganizeAndFinance = new List<CompletionSpeedDataItem>
		{
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-5).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-4).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-3).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-2).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-1).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.ToString("MMM"), AccompanyingFileCount = 0 }
		};

		var expectedRealizeAndFollow = new List<CompletionSpeedDataItem>
		{
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-5).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-4).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-3).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-2).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-1).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.ToString("MMM"), AccompanyingFileCount = 0 }
		};

		// Act
		var request = CreateDefaultRequest();
		var handler = new GetStatisticsForTerritorialBuilderQueryHandler(
			_mockUserRepository,
			_mockAccompanyingFileRepository,
			_mockTelemetryService);
		var result = await handler.Handle(request, CancellationToken.None);

		// Assert
		result.Value!.CompletionSpeedResult[AccompanyingFileStage.Identify]
			  .Should().BeEquivalentTo(expectedIdentify);
		result.Value!.CompletionSpeedResult[AccompanyingFileStage.OrganizingAndFinancing]
			  .Should().BeEquivalentTo(expectedOrganizeAndFinance);
		result.Value!.CompletionSpeedResult[AccompanyingFileStage.RealisationAndFollowing]
			  .Should().BeEquivalentTo(expectedRealizeAndFollow);
	}

	[Fact]
	public async Task NoAccompanyingFiles_ShouldReturnZeroCounts()
	{
		// Arrange
		var accompanyingFiles = CreateAccompanyingFiles();

		A.CallTo(() => _mockAccompanyingFileRepository.GetStatisticsForTerritorialBuilderIndex(
			A<Guid>._,
			A<DateTime?>._,
			A<DateTime?>._,
			false,
			A<AccompanyingFilesStatisticsFilterOptions>._
		)).Returns(accompanyingFiles);

		var expectedIdentify = new List<CompletionSpeedDataItem>
		{
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-5).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-4).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-3).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-2).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-1).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.ToString("MMM"), AccompanyingFileCount = 0 }
		};

		var expectedOrganizeAndFinance = new List<CompletionSpeedDataItem>
		{
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-5).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-4).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-3).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-2).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-1).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.ToString("MMM"), AccompanyingFileCount = 0 }
		};

		var expectedRealizeAndFollow = new List<CompletionSpeedDataItem>
		{
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-5).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-4).ToString("MMM"), AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-3).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-2).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.AddMonths(-1).ToString("MMM"),  AccompanyingFileCount = 0 },
			new() { AbbreviatedMonth = DateTime.Now.ToString("MMM"), AccompanyingFileCount = 0 }
		};

		// Act
		var request = CreateDefaultRequest();
		var handler = new GetStatisticsForTerritorialBuilderQueryHandler(
			_mockUserRepository,
			_mockAccompanyingFileRepository,
			_mockTelemetryService);
		var result = await handler.Handle(request, CancellationToken.None);

		// Assert
		result.Value!.CompletionSpeedResult[AccompanyingFileStage.Identify]
			  .Should().BeEquivalentTo(expectedIdentify);
		result.Value!.CompletionSpeedResult[AccompanyingFileStage.OrganizingAndFinancing]
			  .Should().BeEquivalentTo(expectedOrganizeAndFinance);
		result.Value!.CompletionSpeedResult[AccompanyingFileStage.RealisationAndFollowing]
			  .Should().BeEquivalentTo(expectedRealizeAndFollow);
	}
}