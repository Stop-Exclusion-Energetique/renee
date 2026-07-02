using FakeItEasy;
using FluentAssertions;
using Renee.Application.Handlers.QueryHandlers.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;

namespace QueryHandlerTests;

public class GetFinancialAidFromAnahQueryHandlerTests
{
	private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

	[Fact]
	public async Task NoPeople_ReturnsNoAid()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(new List<AnahCategory>());
		var request = new GetFinancialAidFromAnahQuery(0, true, 0, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.NoAid);
	}

	[Fact]
	public async Task OnePersonAndOneMoreInIleDeFrance_WithLowResources_AndNoDataInScope_ReturnsLowResourceMessage()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					Year = 2021
				},
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = -1,
					LowIncomeHouseholdsAmount = 1000,
					VeryLowIncomeHouseholdsAmount = 500,
					Year = 2021
				}
			});
		var request = new GetFinancialAidFromAnahQuery(2, true, 10200, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be("Aucune aide");
	}

	[Fact]
	public async Task SixPersonInHouseholdInIleDeFrance_WithLowResources_ReturnsLowResourceMessage()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();

		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 5,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				}
			});

		A.CallTo(() => mockAnahSupplementaryOccupant.GetAnahCategorySuplementaryOccupantIncomesAsync()).Returns(
			new List<AnahCategorySuplementaryOccupantIncome>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					StartRuleDate = new DateTime(2026,01,01),
					SuplementaryOcupantLowIncome = 500,
					SuplementaryOccupantVerylowIncome = 250
				}
			}
		);
		var request = new GetFinancialAidFromAnahQuery(6, true, 10200, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.LowIncomeHouseholdsAmount);
	}

	[Fact]
	public async Task OnePersonAndOneMoreInIleDeFrance_WithResources_ReturnsNoAid()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					Year = 2024
				},
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = -1,
					LowIncomeHouseholdsAmount = 1000,
					VeryLowIncomeHouseholdsAmount = 500,
					Year = 2024
				}
			});
		var request = new GetFinancialAidFromAnahQuery(2, true, 12000, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.NoAid);
	}

	[Fact]
	public async Task OnePersonInIleDeFrance_WithLowResources_ReturnsLowResourceMessage()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				}
			});
		A.CallTo(() => mockAnahSupplementaryOccupant.GetAnahCategorySuplementaryOccupantIncomesAsync()).Returns(
			new List<AnahCategorySuplementaryOccupantIncome>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					StartRuleDate = new DateTime(2026,01,01),
					SuplementaryOcupantLowIncome = 500,
					SuplementaryOccupantVerylowIncome = 250
				}
			}
		);
		var request = new GetFinancialAidFromAnahQuery(1, true, 9000, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.LowIncomeHouseholdsAmount);
	}

	[Fact]
	public async Task OnePersonInIleDeFrance_WithResources_ReturnsNoAid()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				}
			});
		A.CallTo(() => mockAnahSupplementaryOccupant.GetAnahCategorySuplementaryOccupantIncomesAsync()).Returns(
			new List<AnahCategorySuplementaryOccupantIncome>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					StartRuleDate = new DateTime(2026,01,01),
					SuplementaryOcupantLowIncome = 500,
					SuplementaryOccupantVerylowIncome = 250
				}
			}
		);
		var request = new GetFinancialAidFromAnahQuery(1, true, 15000, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.NoAid);
	}

	[Fact]
	public async Task OnePersonInIleDeFrance_WithVeryLowResources_ReturnsVeryLowResourceMessage()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				},
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = false,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 9000,
					VeryLowIncomeHouseholdsAmount = 4000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				}
			});
		A.CallTo(() => mockAnahSupplementaryOccupant.GetAnahCategorySuplementaryOccupantIncomesAsync()).Returns(
			new List<AnahCategorySuplementaryOccupantIncome>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					StartRuleDate = new DateTime(2026,01,01),
					SuplementaryOcupantLowIncome = 500,
					SuplementaryOccupantVerylowIncome = 250
				}
			}
		);
		var request = new GetFinancialAidFromAnahQuery(1, true, 3000, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.VeryLowIncomeHouseholdsAmount);
	}

	[Fact]
	public async Task OnePersonNotInIleDeFrance_WithLowResources_ReturnsLowResourceMessage()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();
		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = false,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 5000,
					VeryLowIncomeHouseholdsAmount = 2500,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				},
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 1,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				}

			});
		A.CallTo(() => mockAnahSupplementaryOccupant.GetAnahCategorySuplementaryOccupantIncomesAsync()).Returns(
			new List<AnahCategorySuplementaryOccupantIncome>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					StartRuleDate = new DateTime(2026,01,01),
					SuplementaryOcupantLowIncome = 500,
					SuplementaryOccupantVerylowIncome = 250
				}
			}
		);
		var request = new GetFinancialAidFromAnahQuery(1, false, 4500, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.LowIncomeHouseholdsAmount);
	}

	[Fact]
	public async Task TwoPeopleNotInIleDeFrance_WithLowResources_ReturnsLowResourceMessage()
	{
		var mockAnahCategoryRepository = A.Fake<IAnahCategoryRepository>();
		var mockAnahSupplementaryOccupant = A.Fake<IAnahCategorySuplementaryOccupantIncomeRepository>();

		A.CallTo(() => mockAnahCategoryRepository.GetAllAnahCategoriesAsync()).Returns(
			new List<AnahCategory>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					PeopleNumber = 2,
					LowIncomeHouseholdsAmount = 10000,
					VeryLowIncomeHouseholdsAmount = 5000,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				},
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = false,
					PeopleNumber = 2,
					LowIncomeHouseholdsAmount = 7000,
					VeryLowIncomeHouseholdsAmount = 5500,
					AnahRuleDebutDate = new DateTime(2026,01,01)
				}
			});

		A.CallTo(() => mockAnahSupplementaryOccupant.GetAnahCategorySuplementaryOccupantIncomesAsync()).Returns(
			new List<AnahCategorySuplementaryOccupantIncome>
			{
				new()
				{
					Id = Guid.NewGuid(),
					IsInIleDeFrance = true,
					StartRuleDate = new DateTime(2026,01,01),
					SuplementaryOcupantLowIncome = 500,
					SuplementaryOccupantVerylowIncome = 250
				}
			}
		);
		var request = new GetFinancialAidFromAnahQuery(2, false, 6000, DateTime.Now);
		var handler = new GetFinancialAidFromAnahQueryHandler(mockAnahCategoryRepository, mockAnahSupplementaryOccupant, _telemetryService);
		var amount = await handler.Handle(request, new CancellationToken());
		amount.Value.Should().Be(Labels.LowIncomeHouseholdsAmount);
	}
}