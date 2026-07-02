using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;
using Renee.UI.Components.FormComponents;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class HouseholdIdentityTabTests
{
	private static BunitContext SetupContext(AccompanyingFileDto dto)
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<IMainOccupantService>());
		ctx.Services.AddSingleton(A.Fake<IFinancialAidService>());
		ctx.Services.AddSingleton(A.Fake<IDifficultyFacedByFamilyService>());
		ctx.Services.AddSingleton(A.Fake<IHouseholdResourcesTypologyService>());
		ctx.Services.AddSingleton(A.Fake<IAddressService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<ISendEventQuery>());
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();

        var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileById(A<Guid>._, A<Guid>._, A<string>._)).Returns(ReneeOperationResult<AccompanyingFileDto?>.Success(dto));

		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var authContext = ctx.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void OnDeleteSecondaryOccupant_SaveButtonShouldBeVisible()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Rural
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			SecondaryOccupants =
			[
				new SecondaryOccupantDto { Trigram = "TES", IsDependent = true },
				new SecondaryOccupantDto { Trigram = "AZE", IsDependent = false },
				new SecondaryOccupantDto { Trigram = "WCF", IsDependent = true }
			],
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.CoupleWithChildren
		};
		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		var deleteButton = cut.FindComponents<RadzenButton>().FirstOrDefault(b => b.Instance.Icon == "delete_forever");

		// Act
		deleteButton?.Find("button").Click();

		// Assert
		cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Sauvegarder").Should().NotBeNull();
	}

	[Fact]
	public void OnDeleteSecondaryOccupant_WhenThereAreThreeSecondaryOccupants_ShouldRemainTwoSecondaryOccupants()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Rural
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			SecondaryOccupants =
			[
				new SecondaryOccupantDto { Trigram = "TES", IsDependent = true },
				new SecondaryOccupantDto { Trigram = "AZE", IsDependent = false },
				new SecondaryOccupantDto { Trigram = "WCF", IsDependent = true }
			],
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.CoupleWithChildren
		};
		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		var deleteButton = cut.FindComponents<RadzenButton>().FirstOrDefault(b => b.Instance.Icon == "delete_forever");

		// Act
		deleteButton?.Find("button").Click();

		// Assert
		cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel.SecondaryOccupantViewModels.Count
			.Should().Be(2);
	}

	[Fact]
	public void OnInput_AgeCompletedGreaterThan18_ShouldShowRightPartOfHouseholdIdentityTab()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(),
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					Age = 42
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var rightPartIdentityTab = cut.FindAll(".flex-container > .grid-form:nth-child(2)");
		rightPartIdentityTab.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void OnInput_AgeCompletedSmallerThan18_ShouldNotShowRightPartOfHouseholdIdentityTab()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var ageComponent = cut.FindComponents<OutlinedInt>()
			.FirstOrDefault(c => c.Instance.Label == Labels.AgeOccupant);
		ageComponent?.Find("input").Change(12);

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				      .Age ==
			      12);

		var rightPartIdentityTab = cut.FindAll(".flex-container > .grid-form:nth-child(2)");
		rightPartIdentityTab.Should().BeNullOrEmpty();
	}

	[Fact]
	public void OnInput_AgeEmpty_ShouldShowRightPartOfHouseholdIdentityTab()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var rightPartIdentityTab = cut.FindAll(".flex-container > .grid-form:nth-child(2)");
		rightPartIdentityTab.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void OnRender_WithFourOccupants_LastOccupantShouldBeOccupantFour()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			SecondaryOccupants =
			[
				new SecondaryOccupantDto { Id = Guid.NewGuid() },
				new SecondaryOccupantDto { Id = Guid.NewGuid() },
				new SecondaryOccupantDto { Id = Guid.NewGuid() }
			],
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				      .SecondaryOccupantViewModels.Count >
			      0);

		var h4Elements = cut.FindAll(".occupant-title-and-image h4");
		h4Elements[^1].TextContent.Trim().Should().Be("Occupant 4");
	}

	[Fact]
	public void OnRender_WithOneOccupant_ShouldShowOccupantPrincipal()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Rural
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.CoupleWithChildren
		};
		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		var h4Elements = cut.Find(".occupant-title-and-image h4");
		h4Elements.TextContent.Trim().Should().Be("Occupant principal");
	}

	[Fact]
	public void WhenBothElectricityAndGasAreEmpty_EnergeticTotalShouldShowZero()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		ctx.Render<OccupantCreationForm>(parameters => parameters.Add(p => p.AccompanyingFileId, id));

		// Act
		var viewModel = new ExpensesViewModel();

		// Assert
		viewModel.EnergeticTotal.Should().Be(0);
	}

	[Fact]
	public void WhenEnergeticMonthlyExpenseAreCompleted_EnergeticTotalShouldShowTheRightAmount()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		ctx.Render<OccupantCreationForm>(parameters => parameters.Add(p => p.AccompanyingFileId, id));

		// Act
		var viewModel = new ExpensesViewModel
		{
			MonthlyEnergeticsExpense = new ExpenseModel { Id = Guid.NewGuid(), Value = 50 }
		};

		// Assert
		viewModel.EnergeticTotal.Should().Be(600);
	}

	[Fact]
	public void
		WhenEnergeticTotalIsNotNullAndIncomeTaxRevenueValueIsGreaterThanZero_EnergyEffortRateShouldDisplayTheRightAmount()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		ctx.Render<OccupantCreationForm>(parameters => parameters.Add(p => p.AccompanyingFileId, id));

		// Act
		var expensesViewModel = new ExpensesViewModel
		{
			MonthlyEnergeticsExpense = new ExpenseModel { Id = Guid.NewGuid(), Value = 50 }
		};
		var householdViewModel = new HouseholdViewModel(expensesViewModel)
		{
			IncomeTaxReference = 170,
			Expenses = expensesViewModel
		};

		// Assert
		householdViewModel.EnergyEffortRate.Should().Be(353);
	}

	[Fact]
	public void WhenEnergeticTotalIsNullAndResourcesTotalValueIsEqualsToZero_EnergyEffortRateShouldDisplayNothing()
	{
		// Arrange
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "ref",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing =
				new HousingDto
				{
					Id = Guid.NewGuid(),
					Address =
						new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
					GeographicalTypology = GeographicalHousingAreaTypology.Urban
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying
		};
		var ctx = SetupContext(dto);

		ctx.Render<OccupantCreationForm>(parameters => parameters.Add(p => p.AccompanyingFileId, id));

		// Act
		var expensesViewModel = new ExpensesViewModel();
		var householdViewModel = new HouseholdViewModel(expensesViewModel)
		{
			ResourceTypologieValues = [], Expenses = expensesViewModel
		};

		// Assert
		householdViewModel.EnergyEffortRate.Should().BeNull();
	}
}