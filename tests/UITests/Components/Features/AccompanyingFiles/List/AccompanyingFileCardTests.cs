using System.Security.Claims;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.List.Components;
using Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

namespace UITests.Components.Features.AccompanyingFiles.List;

public class AccompanyingFileCardTests : BunitContext
{
	private void SetupDefaultServices()
	{
		Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IJSRuntime>());
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddBlazorContextMenu();
	}

	private void SetupAuthentication(Guid userId, string role = "ES")
	{
		var authContext = this.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		authContext.SetRoles(role);
		var claims = new Claim[] { new(ClaimTypes.NameIdentifier, userId.ToString()), new(ClaimTypes.Role, role) };
		authContext.SetClaims(claims);
	}

	private static CreatedAccompanyingFilesByUserViewModel CreateViewModel(
		Guid userId,
		AccompanyingFileStage stage,
		bool? shouldFillToAnah,
		AccompanyingFileStatus status = AccompanyingFileStatus.InProgress,
		bool isBillingRequested = false)
	{
		return new CreatedAccompanyingFilesByUserViewModel
		{
			Id = Guid.NewGuid(),
			Reference = "EM-58741-240416",
			Stage = stage,
			Status = status,
			OpeningDateUtc = DateTime.UtcNow,
			LastModificationDateUtc = DateTime.UtcNow,
			Address = "7 rue Michel, 58741 Michel",
			SolidarBuilder = userId,
			SecondSolidarBuilder = Guid.NewGuid(),
			ShouldAccompanyingFileBeSubmittedToAnah = shouldFillToAnah,
			IsBillingRequested = isBillingRequested
		};
	}

	[Fact]
	public void AccompanyingFileCard_ShouldDisplayCorrectName()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);
		var viewModel = CreateViewModel(userId, AccompanyingFileStage.OrganizingAndFinancing, false);

		var cut = Render<AccompanyingFileCard>(parameters => parameters.Add(p => p.ViewModel, viewModel)
			.Add(p => p.UserId, userId).Add(p => p.UserRole, Constants.SolidarBuilderRole));

		// Act
		var labelDivs = cut.FindAll("div.card-item-value");

		// Assert
		labelDivs.Any(label => label.TextContent == $"{viewModel.FirstName}").Should().BeTrue();
		labelDivs.Any(label => label.TextContent == $"{viewModel.LastName}").Should().BeTrue();
	}

	[Fact]
	public void AccompanyingFileCard_ShouldHaveCorrectBackgroundColor()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId, Constants.AdminRole);

		var viewModel = CreateViewModel(Guid.NewGuid(), AccompanyingFileStage.OrganizingAndFinancing, false);
		var cut = Render<AccompanyingFileCard>(parameters => parameters.Add(p => p.ViewModel, viewModel)
			.Add(p => p.UserId, userId).Add(p => p.UserRole, Constants.AdminRole).Add(
				p => p.SelectedFilterContext,
				FilterContext.WithoutSolidarBuilders));

		// Act
		var card = cut.Find(".accompanyingFile-card");

		// Assert
		card.ClassList.Contains("without-solidar-builders").Should().BeTrue();
	}

	[Fact]
	public void ClickOnEditButton_ShouldRedirectBasedOnStage()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);
		Services.AddScoped<NavigationManager, BunitNavigationManager>();

		var stagesAndUrls = new Dictionary<AccompanyingFileStage, string>
		{
			{ AccompanyingFileStage.Identify, "newoccupantdisplay" },
			{ AccompanyingFileStage.OrganizingAndFinancing, "organizeAndFinance" },
			{ AccompanyingFileStage.RealisationAndFollowing, "realiseAndFollow" }
		};

		foreach (var (stage, expectedUrl) in stagesAndUrls)
		{
			var viewModel = CreateViewModel(userId, stage, false);
			var navigationManager = Services.GetService<NavigationManager>();
			var cut = Render<AccompanyingFileCard>(parameters =>
				parameters.Add(p => p.ViewModel, viewModel).Add(p => p.UserId, userId).Add(
					p => p.UserRole,
					Constants.SolidarBuilderRole));

			// Act
			var editButton = cut.FindAll("button").FirstOrDefault(e =>
				e.FirstElementChild?.GetAttribute("alt") == Labels.EditAccompanyingFileButtonAltText);
			editButton?.Click();

			// Assert
			navigationManager?.Uri.Should().Be($"{navigationManager.BaseUri}{expectedUrl}/{viewModel.Id}");
		}
	}

	[Fact]
	public void ShowSynthesisButton_ShouldBehaveAccordingToStatusAndStage()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);

		var testCases = new[]
		{
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.InProgress, false),
			(AccompanyingFileStage.OrganizingAndFinancing, AccompanyingFileStatus.InProgress, true)
		};

		foreach (var (stage, status, isVisible) in testCases)
		{
			var viewModel = CreateViewModel(userId, stage, false, status);
			var cut = Render<AccompanyingFileCard>(parameters =>
				parameters.Add(p => p.ViewModel, viewModel).Add(p => p.UserRole, Constants.SolidarBuilderRole));

			// Act
			var visible = cut.FindAll("button span").Any(img =>
				img.GetAttribute("alt") == Labels.ShowSynthesisButtonAltText);

			// Assert
			visible.Should().Be(isVisible);
		}
	}

	[Fact]
	public void VisualIndicator_ShouldBeVisibleAccordingToStatusAndStageAndShouldAccompanyingFileBeSubmittedToAnahFieldValue()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);

		var testCases = new(AccompanyingFileStage, AccompanyingFileStatus, bool?, bool) []
		{
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.InProgress, true, true),
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.InProgress, false, false),
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.Aborted, true, false),
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.Aborted, true, false),
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.InProgress, null, false),
			(AccompanyingFileStage.Identify, AccompanyingFileStatus.Aborted, null, false),
			(AccompanyingFileStage.OrganizingAndFinancing, AccompanyingFileStatus.InProgress, true, true),
			(AccompanyingFileStage.OrganizingAndFinancing, AccompanyingFileStatus.InProgress, null, false),
			(AccompanyingFileStage.RealisationAndFollowing, AccompanyingFileStatus.InProgress, true, false),
			(AccompanyingFileStage.Finished, AccompanyingFileStatus.InProgress, true, false),
		};

		foreach (var (stage, status, shouldFillToAnah, isVisible) in testCases)
		{
			var viewModel = CreateViewModel(userId, stage, shouldFillToAnah, status);
			var cut = Render<AccompanyingFileCard>(parameters =>
				parameters.Add(p => p.ViewModel, viewModel).Add(p => p.UserRole, Constants.SolidarBuilderRole));

			// Act
			var visible = cut.FindAll("#anah").Any();

			// Assert
			visible.Should().Be(isVisible);
		}
	}

	[Fact]
	public void BillingBadge_WhenWaitingForAbortionWithBilling_ShouldDisplayWithBillingBadge()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);

		var viewModel = CreateViewModel(userId, AccompanyingFileStage.Identify, false,
			AccompanyingFileStatus.WaitingForAbortion, isBillingRequested: true);

		var cut = Render<AccompanyingFileCard>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel)
				.Add(p => p.UserId, userId)
				.Add(p => p.UserRole, Constants.SolidarBuilderRole));

		// Act
		var badge = cut.FindAll(".badge-with-billing");

		// Assert
		badge.Should().ContainSingle();
		badge.First().TextContent.Should().Contain(Labels.BillingBadgeWithBilling);
	}

	[Fact]
	public void BillingBadge_WhenWaitingForAbortionWithoutBilling_ShouldDisplayWithoutBillingBadge()
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);

		var viewModel = CreateViewModel(userId, AccompanyingFileStage.Identify, false,
			AccompanyingFileStatus.WaitingForAbortion, isBillingRequested: false);

		var cut = Render<AccompanyingFileCard>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel)
				.Add(p => p.UserId, userId)
				.Add(p => p.UserRole, Constants.SolidarBuilderRole));

		// Act
		var badge = cut.FindAll(".badge-without-billing");

		// Assert
		badge.Should().ContainSingle();
		badge.First().TextContent.Should().Contain(Labels.BillingBadgeWithoutBilling);
	}

	[Theory]
	[InlineData(AccompanyingFileStatus.InProgress)]
	[InlineData(AccompanyingFileStatus.WaitingForApproval)]
	[InlineData(AccompanyingFileStatus.Rejected)]
	[InlineData(AccompanyingFileStatus.Aborted)]
	[InlineData(AccompanyingFileStatus.Finished)]
	public void BillingBadge_WhenStatusIsNotWaitingForAbortion_ShouldNotDisplayBadge(AccompanyingFileStatus status)
	{
		// Arrange
		SetupDefaultServices();
		var userId = Guid.NewGuid();
		SetupAuthentication(userId);

		var viewModel = CreateViewModel(userId, AccompanyingFileStage.Identify, false, status);

		var cut = Render<AccompanyingFileCard>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel)
				.Add(p => p.UserId, userId)
				.Add(p => p.UserRole, Constants.SolidarBuilderRole));

		// Act
		var withBillingBadge = cut.FindAll(".badge-with-billing");
		var withoutBillingBadge = cut.FindAll(".badge-without-billing");

		// Assert
		withBillingBadge.Should().BeEmpty();
		withoutBillingBadge.Should().BeEmpty();
	}
}