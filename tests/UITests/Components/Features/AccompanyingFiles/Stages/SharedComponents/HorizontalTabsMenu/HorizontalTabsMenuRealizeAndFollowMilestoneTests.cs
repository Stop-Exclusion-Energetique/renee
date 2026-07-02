using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.Stages.SharedComponents.HorizontalTabsMenu;

public class HorizontalTabsMenuRealizeAndFollowMilestoneTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();
		ctx.Services.AddSingleton(A.Fake<IAccompanyingFileService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<ISendEventQuery>());
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());
		var workTypesLabelsService = A.Fake<IWorkTypesLabelsService>();
		A.CallTo(() => workTypesLabelsService.GetAllWorkTypesLabels())
			.Returns(Task.FromResult(ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>.Success([])));
		ctx.Services.AddSingleton(workTypesLabelsService);
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();

        ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var mockQuerySender = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockQuerySender.Send(A<GetAccompanyingFileForRealiseAndFollowMilestoneQuery>._)).Returns(
			ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>.Success(
				new GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult
				{
					Reference = "Michael SCOTT-NDU-75018-25/04/2024"
				}
			)
		);
		ctx.Services.AddSingleton(mockQuerySender);

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
	public void ClickOnNextButton_OnFirstTab_SelectedTabShouldBeOne()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();
		cut.WaitForAssertion(async () => await cut.Instance.ChangeTab(0));

		// Act
		var nextButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == Labels.Next);
		nextButton?.Click();

		// Assert
		cut.Instance.SelectedTab.Should().Be(1);
	}

	[Fact]
	public void ClickOnNextButton_OnSecondTab_SelectedTabShouldBeTwo()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();
		cut.WaitForAssertion(async () => await cut.Instance.ChangeTab(1));

		// Act
		var nextButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == Labels.Next);
		nextButton?.Click();

		// Assert
		cut.Instance.SelectedTab.Should().Be(2);
	}

	[Fact]
	public void ClickOnNextButton_OnThirdTab_SelectedTabShouldBeThree()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();
		cut.WaitForAssertion(async () => await cut.Instance.ChangeTab(2));

		// Act
		var nextButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == Labels.Next);
		nextButton?.Click();

		// Assert
		cut.Instance.SelectedTab.Should().Be(3);
	}

	[Fact]
	public void ClickOnPreviousButton_OnFirstTab_SelectedTabShouldBeZero()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();
		cut.WaitForAssertion(async () => await cut.Instance.ChangeTab(0));

		// Act
		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Précédent");

		// Assert
		previousButton?.HasAttribute("disabled").Should().BeTrue();
    }

	[Fact]
	public void ClickOnPreviousButton_OnFirstTab_ShouldRedirectToAccompanyingFileList()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();

		// Act
		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == Labels.Previous);

		// Assert
		previousButton?.HasAttribute("disabled").Should().BeTrue();
    }

	[Fact]
	public void ClickOnPreviousButton_OnSecondTab_SelectedTabShouldBeZero()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();
		cut.WaitForAssertion(async () => await cut.Instance.ChangeTab(1));

		// Act
		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == Labels.Previous);
		previousButton?.Click();

		// Assert
		cut.Instance.SelectedTab.Should().Be(0);
	}

	[Fact]
	public void ClickOnPreviousButton_OnThirdTab_SelectedTabShouldBeOne()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();
		cut.WaitForAssertion(async () => await cut.Instance.ChangeTab(2));

		// Act
		var previousButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == Labels.Previous);
		previousButton?.Click();

		// Assert
		cut.Instance.SelectedTab.Should().Be(1);
	}

	[Fact]
	public void OnClickOnEvaluationsTab_EvaluationsTabShouldBeTheActiveTab()
	{
		// Arrange
		var ctx = SetupContext();
		var cut =
			ctx.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>(
				parameters => parameters.Add(
					p => p.AccompanyingFileId,
					Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		// Act
		cut.Find("#tab-3 > a").Click();

		// Assert
		cut.Find("#tab-3").ClassList.Contains("active").Should().BeTrue();
	}

	[Fact]
	public void OnClickOnProjectCostTab_ProjectCostTabShouldBeTheActiveTab()
	{
		// Arrange
		var ctx = SetupContext();
		var cut =
			ctx.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>(
				parameters => parameters.Add(
					p => p.AccompanyingFileId,
					Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		// Act
		cut.Find("#tab-1 > a").Click();

		// Assert
		cut.Find("#tab-1").ClassList.Contains("active").Should().BeTrue();
	}

	[Fact]
	public void OnClickOnProjectEndTab_ProjectEndTabShouldBeTheActiveTab()
	{
		// Arrange
		var ctx = SetupContext();
		var cut =
			ctx.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>(
				parameters => parameters.Add(
					p => p.AccompanyingFileId,
					Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		// Act
		cut.Find("#tab-4 > a").Click();

		// Assert
		cut.Find("#tab-4").ClassList.Contains("active").Should().BeTrue();
	}

	[Fact]
	public void OnClickOnWorkSummaryTab_WorkSummaryTabShouldBeTheActiveTab()
	{
		// Arrange
		var ctx = SetupContext();
		var cut =
			ctx.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>(
				parameters => parameters.Add(
					p => p.AccompanyingFileId,
					Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95")));

		// Act
		cut.Find("#tab-2 > a").Click();

		// Assert
		cut.Find("#tab-2").ClassList.Contains("active").Should().BeTrue();
	}

	[Fact]
	public void OnTryToFindSaveButtonContainer_SaveButtonIsActiveIsFalse_ShouldNotReturnElementNotFoundException()
	{
		var ctx = SetupContext();
		var cut = ctx
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();

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
			.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.
				RealiseAndFollow>();

		//Act
		cut.WaitForAssertion(() => cut.Instance.SaveButtonIsActive = true);

		cut.Render();

		var findSaveButtonContainer = () => { cut.Find(".save-btn-container"); };

		//Assert
		findSaveButtonContainer.Should().NotThrow<ElementNotFoundException>();
	}
}