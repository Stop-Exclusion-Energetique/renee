using AirtableApiClient;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.Stages.SharedComponents.HorizontalTabsMenu;

public class HorizontalTabsMenuOrganizeAndFinanceMilestoneTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<IWorkTypesLabelsService>());
		ctx.Services.AddSingleton(A.Fake<IProjectTypeService>());
		ctx.Services.AddSingleton(A.Fake<IInsuranceTypeService>());
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IWorkTypeProjectTypeService>());
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();

        ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileForOrganizeAndFinanceMilestone(A<Guid>._, A<Guid>._, A<string>._))
			.Returns(
				ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>.Success(
					new GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult
					{
						Reference = "Michael SCOTT-NDU-75018-25/04/2024"
					}
				)
			);
		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var mockAirtableService = A.Fake<IAirtableService>();
		A.CallTo(() => mockAirtableService.AddRecordAsync(A<string?>._, A<string?>._, A<string?>._, A<string>._!, A<Guid>._, A<double>._))
			.Returns(ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>.Success(new AirtableCreateUpdateReplaceRecordResponse(new AirtableRecord())));
		ctx.Services.AddSingleton(mockAirtableService);

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
	public async Task ClickOnNextButton_OnFirstTab_SelectedTabShouldBeOne()
	{
		using var ctx = SetupContext();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		await cut.WaitForAssertionAsync(async () => await cut.Instance.ChangeTab(0));

		var nextButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Suivant");
		nextButton?.Click();

		cut.Instance.SelectedTab.Should().Be(1);
	}

	[Fact]
	public async Task ClickOnNextButton_OnSecondTab_SelectedTabShouldBeTwo()
	{
		using var ctx = SetupContext();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		await cut.WaitForAssertionAsync(async () => await cut.Instance.ChangeTab(1));

		var nextButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Suivant");
		nextButton?.Click();

		cut.Instance.SelectedTab.Should().Be(2);
	}

	[Fact]
	public async Task ClickOnNextButton_OnThirdTab_SelectedTabShouldBeThree()
	{
		using var ctx = SetupContext();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		await cut.WaitForAssertionAsync(async () => await cut.Instance.ChangeTab(2));

		var nextButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Suivant");
		nextButton?.Click();

		cut.Instance.SelectedTab.Should().Be(3);
	}

	[Fact]
	public async Task ClickOnPreviousButton_OnFirstTab_SelectedTabShouldBeZero()
	{
		using var ctx = SetupContext();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		await cut.WaitForAssertionAsync(async () => await cut.Instance.ChangeTab(0));

		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Précédent");

		previousButton?.HasAttribute("disabled").Should().BeTrue();
    }

	[Fact]
	public void ClickOnPreviousButton_OnFirstTab_ShouldRedirectToAccompanyingFileList()
	{
		using var ctx = SetupContext();
		var navigationManager = ctx.Services.GetService<NavigationManager>();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Précédent");

		previousButton?.HasAttribute("disabled").Should().BeTrue();
    }

	[Fact]
	public async Task ClickOnPreviousButton_OnSecondTab_SelectedTabShouldBeZero()
	{
		using var ctx = SetupContext();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		await cut.WaitForAssertionAsync(async () => await cut.Instance.ChangeTab(1));

		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Précédent");
		previousButton?.Click();

		cut.Instance.SelectedTab.Should().Be(0);
	}

	[Fact]
	public async Task ClickOnPreviousButton_OnThirdTab_SelectedTabShouldBeOne()
	{
		using var ctx = SetupContext();

		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();
		await cut.WaitForAssertionAsync(async () => await cut.Instance.ChangeTab(2));

		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Précédent");
		previousButton?.Click();

		cut.Instance.SelectedTab.Should().Be(1);
	}

	[Fact]
	public void OnTryToFindSaveButtonContainer_SaveButtonIsActiveIsFalse_ShouldNotReturnElementNotFoundException()
	{
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();

		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive = false);

		var findSaveButtonContainer = () => { cut.Find(".save-btn-container"); };

		findSaveButtonContainer.Should().Throw<ElementNotFoundException>();
	}

	[Fact]
	public void OnTryToFindSaveButtonContainer_SaveButtonIsActiveIsTRue_SaveButtonShouldExist()
	{
		//Arrange 
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.
				OrganizeAndFinance>();

		//Act
		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive = true);

		cut.Render();

		var findSaveButtonContainer = () => { cut.Find(".save-btn-container"); };

		//Assert
		findSaveButtonContainer.Should().NotThrow<ElementNotFoundException>();
	}
}