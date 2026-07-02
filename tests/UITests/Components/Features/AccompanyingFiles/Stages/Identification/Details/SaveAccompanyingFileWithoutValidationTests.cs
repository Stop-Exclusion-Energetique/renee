using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class SaveAccompanyingFileWithoutValidationTests
{
	private static BunitContext SetupContext()
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
		A.CallTo(
			() => mockAccompanyingFileService.GetAccompanyingFileById(
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95"), A<Guid>._, A<string>._)).Returns(
			ReneeOperationResult<AccompanyingFileDto?>.Success(
				new AccompanyingFileDto
				{
					Id = Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95"),
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
							GeographicalTypology = GeographicalHousingAreaTypology.Rural,
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
							Email = "duplicateaddress@sqli.com",
							PhoneNumber = "+33 7.77.77.77.77",
							Gender = "Mr",
							LastName = "test",
							FirstName = "test",
							SocioProfessionalCategoryId = SocioProfessionalCategory.Cadre,
							AccompanyingFileId = Guid.NewGuid(),
							Job = "test"
						},
					EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
					EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
					Stage = AccompanyingFileStage.Identify,
					RiskType = AccompanyingFileRiskType.Low,
					TaxIncome = 2000,
					HouseholdResourcesTypologies = [],
					DifficultiesFacedByFamily = [],
					SocialContext = "le context",
					HouseholdTypologyId = HouseholdTypology.SingleParentFamily
				}
			)
		);

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
	public void WhenModificationOnForm_SaveButtonIsActive_ShouldBeTrue()
	{
		var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));
		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var input = cut.Find("input");
		input.Change("NDU");
		input.Blur();

		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive.Should().BeTrue());
	}

	[Fact]
	public void WhenNoModification_ShouldNotShow_SaveButton()
	{
		var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		var buttons = cut.FindAll("button");

		buttons.Should().NotContain(x => x.TextContent == "Sauvegarder");
	}

	[Fact]
	public void WhenSaveButtonIsActiveIsTrue_OnClickOnSaveButton_SaveButtonIsActiveShouldBeFalse()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));
		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive = true);

		var saveBtn = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Sauvegarder");
		saveBtn?.Click();

		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive.Should().BeFalse());
	}

	[Fact]
	public void WhenSaveButtonIsActiveIsTrue_SaveButton_ShouldNotBeNull()
	{
		var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive = true);

		var buttons = cut.FindAll("button");

		buttons.FirstOrDefault(btn => btn.TextContent == "Sauvegarder").Should().NotBeNull();
	}
}