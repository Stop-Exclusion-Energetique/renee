using System.Security.Claims;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
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

namespace UITests.Components.Features.AccompanyingFiles.Stages.SharedComponents.HorizontalTabsMenu;

public class HorizontalTabsMenuIdentifyMilestoneTests
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
		ctx.Services.AddSingleton(A.Fake<ICopropertyProfileService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());
		ctx.Services.AddSingleton(A.Fake<ISendEventQuery>());
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();


        ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileById(A<Guid>._, A<Guid>._, A<string>._)).Returns(
			ReneeOperationResult<AccompanyingFileDto?>.Success(
				new AccompanyingFileDto
				{
					Id = Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95"),
					Reference = "EM-75001-240226",
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
							Gender = "Mr",
							LastName = "test"
						},
					EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
					EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
					AnahCategory = "",
					Stage = AccompanyingFileStage.Identify,
					RiskType = AccompanyingFileRiskType.Low
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
	public void ClickOnNextButton_OnFirstTab_SelectedIndexShouldBeOne()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		var buttons = cut.FindAll("button");

		buttons.FirstOrDefault(btn => btn.TextContent == "Suivant")?.Click();

		cut.Instance.SelectedTab.Should().Be(1);
	}

	[Fact]
	public void ClickOnPreviousButton_OnFirstTab_ShouldRedirectToAccompanyingFileList()
	{
		using var ctx = SetupContext();
        var navigationManager = ctx.Services.GetService<NavigationManager>();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		var buttons = cut.FindAll("button");

		buttons.FirstOrDefault(btn => btn.TextContent == "Précédent")?.HasAttribute("disabled").Should().BeTrue();
	}

	[Fact]
	public void ClickOnPreviousButton_OnFourthTab_SelectedIndexShouldBeTwo()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		cut.Find("#tab-4 > a").Click();

		cut.WaitForState(() => cut.Instance.SelectedTab == 3);

		var buttons = cut.FindAll("button");
		buttons.FirstOrDefault(btn => btn.TextContent == "Précédent")?.Click();

		cut.Instance.SelectedTab.Should().Be(2);
	}

	[Fact]
	public void OnClickOnEnergyProfileTab_ShouldRenderEnergyProfileTab()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		cut.Find("#tab-3 > a").Click();

		var tabContentH4 = cut.Find("h4");
		var tabContentH4Text = tabContentH4.TextContent;
		tabContentH4Text.MarkupMatches(Labels.EnergyCharacteristics);
	}

	[Fact]
	public void OnClickOnHouseholdIdentityTab_ShouldRenderHouseholdIdentityTab()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));
		cut.WaitForAssertion(() => cut.Instance.SelectedTab = 2);

		cut.Find("#tab-1 > a").Click();

		var tabContentH4 = cut.Find(".home-composition-header > h3");
		var tabContentH4Text = tabContentH4.TextContent;
		tabContentH4Text.MarkupMatches("Composition du foyer");
	}

	[Fact]
	public void OnClickOnHousingTab_ShouldRenderHousingTab()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		cut.Find("#tab-2 > a").Click();

		var tabContentH4 = cut.Find("h4");
		var tabContentH4Text = tabContentH4.TextContent;
		tabContentH4Text.MarkupMatches(Labels.Address);
	}

	[Fact]
	public void OnClickOnReasonOfSolicitationTab_ShouldRenderReasonOfSolicitationTab()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		cut.Find("#tab-4 > a").Click();

		var tabContentH4 = cut.Find("h4");
		var tabContentH4Text = tabContentH4.TextContent;
		tabContentH4Text.MarkupMatches(Labels.ReasonOfProject);
	}

	[Fact]
	public void SubmitButton_OnFourthTab_ShouldBeVisible()
	{
		using var ctx = SetupContext();
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(
				p => p.AccompanyingFileId,
				Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		cut.WaitForAssertion(() => cut.Instance.SelectedTab = 3);

		var buttons = cut.FindAll("button");

		buttons.FirstOrDefault(btn => btn.TextContent == "Soumettre")?.Should().NotBeNull();
	}
}