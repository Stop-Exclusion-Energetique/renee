using System.Globalization;
using System.Security.Claims;
using Blazored.Modal.Services;
using Bunit.Extensions;
using Bunit.TestDoubles;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.AccompanyingFileHouseholdResourceTypology;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage;
using Renee.UI.Components.Layout.StageLayout;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class AccompanyingFileSubmitTests
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
		ctx.Services.AddSingleton(A.Fake<ICopropertyProfileService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<ISendEventQuery>());
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		var userGuid = Guid.NewGuid();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileById(A<Guid>._, A<Guid>._, A<string>._)).Returns(ReneeOperationResult<AccompanyingFileDto?>.Success(dto));
		A.CallTo(
			() => mockAccompanyingFileService.UpdateAccompanyingFileForIdentificationMilestone(
				A<SaveAccompanyingFileIdentificationMilestoneCommandInput>._)).Returns(ReneeOperationResult<bool>.Success(true));
		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var authContext = ctx.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, userGuid.ToString()),
		};

		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void
		OnSubmit_NeitherTheUnsanitaryCoefficientNorTheDegradationIndexIsCompleted_ErrorMessagesShouldBeDisplayed()
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
					GeographicalTypology = GeographicalHousingAreaTypology.Urban,
					BuildingYear = HousingYearConstruction.HouseConstructionPeriod1,
					AcquisitionYear = 1985,
					DpeLabel = DpeLabel.E,
					GesLabel = GesLabel.E,
					OwnershipStatus = OwnershipStatus.FullOwnership,
					HousingType = HousingType.ResidentialCollective
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(),
					Trigram = "NDU",
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					PensionFund = PensionFund.RetirementInsurance,
					SocialWelfareFund = SocialProtectionFund.Cgss,
					Age = 20,
					Birthdate = DateTime.ParseExact("20/10/1987", "dd/MM/yyyy", CultureInfo.InvariantCulture),
					SocioProfessionalCategoryId = SocioProfessionalCategory.Cadre,
					AccompanyingFileId = Guid.NewGuid(),
					ComplementaryFund = AdditionalFund.Rafp
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			AnahCategory = "",
			HasDisabilitySituation = false,
			HasPersonFollowedByCuratorship = false,
			HasPersonFollowedByGuardianship = false,
			HasPersonWithLongTermIllness = false,
			HasPersonWithLossOfIndependence = false,
			HasUnpaidEnergyBills = false,
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			TaxIncome = 2000,
			HouseholdResourcesTypologies = [],
			DifficultiesFacedByFamily = [],
			FamilyProject = "le projet",
			SocialContext = "le context",
			IsFollowedBySocialWorker = false,
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying,
			AccompanyingTimeDuration = AccompanyingTimeDuration.LessThanTwoHours,
			SigningHouseholdSupportDate = DateTime.Now
		};

		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.SelectedTab = 3);

		// Act
		var form = cut.Find("form");
		cut.WaitForAssertion(() => form.Submit());

		var expectedValidationMessage = Labels.Errors.RequiredUnsanitaryCoefficientOrDegradationIndex;
		var validationMessages = cut.FindAll(".validation-message").Where(m => !string.IsNullOrEmpty(m.TextContent));

		// Assert
		validationMessages.Any(m => m.TextContent == expectedValidationMessage).Should().BeTrue();
	}

	[Fact]
	public void
		OnSubmit_OtherComplementaryFundIsSelected_WithNoValueOnTextInput_FoundValidationMessageForOtherComplementaryFundShouldBeTrue()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing = new HousingDto
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
					ComplementaryFund = AdditionalFund.Other
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low
		};
		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.ValidateEditContext());

		var expectedValidationMessage = "Veuillez saisir la caisse complémentaire.";

		var validationMessages = cut.FindAll(".validation-message");
		var validationMessageFound =
			validationMessages.Any(message => message.TextContent.Trim() == expectedValidationMessage);
		validationMessageFound.Should().BeTrue();
	}

	[Fact]
	public void
		OnSubmit_OtherPensionFundIsSelected_WithNoValueOnTextInput_FoundValidationMessageForOtherPensionFundShouldBeTrue()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing = new HousingDto
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
					Id = Guid.NewGuid(),
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					PensionFund = PensionFund.Other
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low
		};
		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.ValidateEditContext());

		var expectedValidationMessage = "Veuillez saisir la caisse de retraite.";

		var validationMessages = cut.FindAll(".validation-message");
		var validationMessageFound =
			validationMessages.Any(message => message.TextContent.Trim() == expectedValidationMessage);
		validationMessageFound.Should().BeTrue();
	}

	[Fact]
	public void
		OnSubmit_OtherSocialWelfareFundIsSelected_WithNoValueOnTextInput_FoundValidationMessageForOtherSocialWelfareFundShouldBeTrue()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "",
			CreationDatetimeUtc = DateTime.Now,
			LastUpdateDatetimeUtc = DateTime.Now,
			ClosedDatetimeUtc = null,
			Housing = new HousingDto
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
					Id = Guid.NewGuid(),
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					SocialWelfareFund = SocialProtectionFund.Other
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low
		};
		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.ValidateEditContext());

		var expectedValidationMessage = "Veuillez saisir la caisse de protection sociale.";

		var validationMessages = cut.FindAll(".validation-message");
		var validationMessageFound =
			validationMessages.Any(message => message.TextContent.Trim() == expectedValidationMessage);
		validationMessageFound.Should().BeTrue();
	}

	[Fact]
	public void OnSubmit_RequiredFieldAreCompleted_NoValidationMessagesShouldBeTrue()
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
					GeographicalTypology = GeographicalHousingAreaTypology.Urban,
					BuildingYear = HousingYearConstruction.HouseConstructionPeriod1,
					AcquisitionYear = 1985,
					DpeLabel = DpeLabel.E,
					DegradationIndex = DegradationIndex.Low,
					UnsanitaryCoefficient = UnsanitaryCoefficient.High
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(),
					Trigram = "NDU",
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					PensionFund = PensionFund.Cnav,
					SocialWelfareFund = SocialProtectionFund.Caf,
					Age = 20,
					Birthdate = DateTime.ParseExact("20/10/1987", "dd/MM/yyyy", CultureInfo.InvariantCulture),
					SocioProfessionalCategoryId = SocioProfessionalCategory.IntermediateProfession,
					AccompanyingFileId = Guid.NewGuid(),
					Job = "test",
					FirstName = "John",
					LastName = "Doe"
				},
			SecondaryOccupants =
			[
				new SecondaryOccupantDto
				{
					Id = Guid.NewGuid(),
					Trigram = "NDU",
					Age = 10,
					Birthdate =
						DateTime.ParseExact("20/10/1987", "dd/MM/yyyy", CultureInfo.InvariantCulture),
					IsDependent = true
				}
			],
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			AnahCategory = "",
			HasUnpaidEnergyBills = false,
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			TaxIncome = 2000,
			HouseholdResourcesTypologies =
			[
				new AccompanyingFileHouseholdResourcesTypologyDto{ HouseholdResourcesTypologyId = Guid.NewGuid(), Value = 100 }
			],
			DifficultiesFacedByFamily = [],
			FamilyProject = "le projet",
			SocialContext = "le context",
			IsFollowedBySocialWorker = false,
			HouseholdTypologyId = HouseholdTypology.SinglePerson,
			Expenses =
			[
				new(){ Id = Guid.NewGuid(), Type = ExpenseType.MonthlyEnergecticsExpenses, Value = 200}
			]
		};

		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.ValidateEditContext());

		var validationMessages = cut.FindAll(".validation-message").All(m => string.IsNullOrEmpty(m.TextContent));
		validationMessages.Should().BeTrue();
	}

	[Fact]
	public void OnSubmit_RequiredFieldAreCompleted_ShouldNavigateToSynthesisPage()
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
					GeographicalTypology = GeographicalHousingAreaTypology.Urban,
					BuildingYear = HousingYearConstruction.HouseConstructionPeriod1,
					AcquisitionYear = 1985,
					DpeLabel = DpeLabel.E,
					GesLabel = GesLabel.E,
					DegradationIndex = DegradationIndex.Low,
					UnsanitaryCoefficient = UnsanitaryCoefficient.High,
					OwnershipStatus = OwnershipStatus.FullOwnership,
					HousingType = HousingType.ResidentialCollective,
					CopropertyProfileId = Guid.NewGuid(),
					LivingSpaceInSquareMeter = 100,
					ElectricityDeprivation = EnergyDeprivation.Partial,
					AnnualEnergyConsumption = 500
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(),
					Trigram = "NDU",
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					PensionFund = PensionFund.RetirementInsurance,
					SocialWelfareFund = SocialProtectionFund.Cgss,
					Age = 20,
					Birthdate = DateTime.ParseExact("20/10/1987", "dd/MM/yyyy", CultureInfo.InvariantCulture),
					SocioProfessionalCategoryId = SocioProfessionalCategory.Cadre,
					AccompanyingFileId = Guid.NewGuid(),
					ComplementaryFund = AdditionalFund.Rafp,
					FirstName = "John",
					LastName = "Doe"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			AnahCategory = "",
			HasUnpaidEnergyBills = false,
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			TaxIncome = 2000,
			HouseholdResourcesTypologies =
			[
				new AccompanyingFileHouseholdResourcesTypologyDto{ HouseholdResourcesTypologyId = Guid.NewGuid(), Value = 100 }
			],
			DifficultiesFacedByFamily = [],
			FamilyProject = "le projet",
			SocialContext = "le context",
			IsFollowedBySocialWorker = false,
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying,
			AccompanyingTimeDuration = AccompanyingTimeDuration.LessThanTwoHours,
			SigningHouseholdSupportDate = DateTime.Now,
			FirstVisitDate = DateTime.Now,
			Expenses =
			[
				new(){ Id = Guid.NewGuid(), Type = ExpenseType.MonthlyEnergecticsExpenses, Value = 200},
			]
		};

		var ctx = SetupContext(dto);
		var navigationManager = ctx.Services.GetRequiredService<NavigationManager>();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		var form = cut.Find("form");

		cut.WaitForAssertion(() => form.Submit());

		navigationManager.Uri.Should().Be($"{navigationManager.BaseUri}synthesis/identification/{id}");
	}

	[Fact]
	public void OnSubmit_RequiredFieldAreNotCompleted_NoValidationMessagesShouldBeFalse()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "",
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
					PensionFund = PensionFund.Cnav,
					SocialWelfareFund = SocialProtectionFund.Caf,
					Gender = "Mr",
					LastName = "test",
					FirstName = "test",
					Age = 42,
					Birthdate = DateTime.ParseExact("20/10/1987", "dd/MM/yyyy", CultureInfo.InvariantCulture),
					SocioProfessionalCategoryId = SocioProfessionalCategory.Cadre,
					AccompanyingFileId = Guid.NewGuid(),
					Job = "test"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			AnahCategory = "",
			HasDisabilitySituation = false,
			HasPersonFollowedByCuratorship = false,
			HasPersonFollowedByGuardianship = false,
			HasPersonWithLongTermIllness = false,
			HasPersonWithLossOfIndependence = false,
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low
		};

		var ctx = SetupContext(dto);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.ValidateEditContext());

		var validationMessages = cut.FindAll(".validation-message").All(m => !string.IsNullOrEmpty(m.TextContent));
		validationMessages.Should().BeFalse();
	}

	[Fact]
	public void OnSubmit_TheThreeOtherFundsAreSelected_WithValueOnTextInput_FoundValidationMessagesShouldBeFalse()
	{
		var id = Guid.NewGuid();
		var dto = new AccompanyingFileDto
		{
			Id = id,
			Reference = "",
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
					ComplementaryFund = AdditionalFund.Other,
					OtherPensionFund = "Other pension fund",
					OtherSocialWelfareFund = "Other social welfare",
					OtherComplementaryFund = "Other complementary found"
                },
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			HouseholdResourcesTypologies =
			[
				new AccompanyingFileHouseholdResourcesTypologyDto { HouseholdResourcesTypologyId = Guid.NewGuid(), Value = 100 }
			]
		};
		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.ValidateEditContext());

		var expectedValidationMessage = "Veuillez saisir la caisse complémentaire.";

		var validationMessages = cut.FindAll(".validation-message");
		var validationMessageFound =
			validationMessages.Any(message => message.TextContent.Trim() == expectedValidationMessage);
		validationMessageFound.Should().BeFalse();
	}

	[Fact]
	public void
		OnSubmit_UnsanitaryCoefficientIsCompletedAndDegradationIndexIsNotCompleted_ErrorMessagesShouldBeDisplayed()
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
					GeographicalTypology = GeographicalHousingAreaTypology.Urban,
					BuildingYear = HousingYearConstruction.HouseConstructionPeriod1,
					AcquisitionYear = 1985,
					DpeLabel = DpeLabel.E,
					GesLabel = GesLabel.E,
					OwnershipStatus = OwnershipStatus.FullOwnership,
					HousingType = HousingType.ResidentialCollective,
					CopropertyProfileId = Guid.NewGuid(),
					UnsanitaryCoefficient = UnsanitaryCoefficient.Medium,
					LivingSpaceInSquareMeter = 100,
					ElectricityDeprivation = EnergyDeprivation.Partial,
					AnnualEnergyConsumption = 500
				},
			MainOccupant =
				new MainOccupantDto
				{
					Id = Guid.NewGuid(),
					Trigram = "NDU",
					Email = "duplicateaddress@sqli.com",
					PhoneNumber = "+33 7.77.77.77.77",
					PensionFund = PensionFund.RetirementInsurance,
					SocialWelfareFund = SocialProtectionFund.Cgss,
					Age = 20,
					Birthdate = DateTime.ParseExact("20/10/1987", "dd/MM/yyyy", CultureInfo.InvariantCulture),
					SocioProfessionalCategoryId = SocioProfessionalCategory.Cadre,
					AccompanyingFileId = Guid.NewGuid(),
					ComplementaryFund = AdditionalFund.Rafp,
					FirstName = "John",
					LastName = "Doe"
				},
			EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
			EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
			AnahCategory = "",
			HasUnpaidEnergyBills = false,
			Stage = AccompanyingFileStage.Identify,
			RiskType = AccompanyingFileRiskType.Low,
			TaxIncome = 2000,
			HouseholdResourcesTypologies =
			[
				new AccompanyingFileHouseholdResourcesTypologyDto{ HouseholdResourcesTypologyId = Guid.NewGuid(), Value = 100 }
			],
			DifficultiesFacedByFamily = [],
			FamilyProject = "le projet",
			SocialContext = "le context",
			IsFollowedBySocialWorker = false,
			HouseholdTypologyId = HouseholdTypology.SinglePersonWithAdultStaying,
			AccompanyingTimeDuration = AccompanyingTimeDuration.LessThanTwoHours,
			SigningHouseholdSupportDate = DateTime.Now,
			FirstVisitDate = DateTime.Now,
			Expenses =
			[
				new(){ Id = Guid.NewGuid(), Type = ExpenseType.MonthlyEnergecticsExpenses, Value = 200}
			]
		};

		var ctx = SetupContext(dto);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);
		cut.WaitForAssertion(() => cut.Instance.SelectedTab = 3);

		// Act
		var form = cut.Find("form");
		cut.WaitForAssertion(() => form.Submit());

		var areValidationMessagesEmpty = cut.FindAll(".validation-message").All(m => string.IsNullOrEmpty(m.TextContent));

		// Assert
		areValidationMessagesEmpty.Should().BeTrue();
	}
}