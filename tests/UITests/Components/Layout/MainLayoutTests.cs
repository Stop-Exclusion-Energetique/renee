using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.Abstraction.Query;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Administration;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.UI.Components.CGUHandling;
using Renee.UI.Components.CGUHandling.CGUModal;
using Renee.UI.Components.Layout;
using System.Security.Claims;

namespace UITests.Components.Layout;

public class MainLayoutTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		var sendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => sendEventQuery.Send(A<IQuery<ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>>>._))
			.Returns(ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>.Success(new GetBannerDisplayDatesDisplayQueryObjectResult
			{
				AccompanyingFileAlertBannerStartDate = null,
				AccompanyingFileAlertBannerEndDate = null,
				AccompanyingFileAlertBannerMessage = null
			}));

		ctx.Services.AddScoped<BunitNavigationManager>();
		ctx.Services.AddScoped<NavigationManager>(sp => sp.GetRequiredService<BunitNavigationManager>());
		ctx.Services.AddScoped<NotificationService>();
		ctx.Services.AddSingleton(A.Fake<ITelemetryService>());
		ctx.Services.AddSingleton(A.Fake<IJSRuntime>());
		ctx.Services.AddSingleton(A.Fake<IFileViewerService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IAccompanyingFileService>());
		ctx.Services.AddSingleton(sendEventQuery);
		ctx.Services.AddBlazorContextMenu();

		return ctx;
	}

	private static BunitContext SetupContextForMenuAndCguTests(
		string? role = null,
		bool setupCgu = false,
		string? cguUserVersion = "v2",
		string? cguLatestVersion = "v2")
	{
		var ctx = SetupContext();

		var cguValidationService = A.Fake<ICguValidationService>();
		var authProvider = A.Fake<AuthenticationStateProvider>();
		var cguInitializer = new CguInitializer(authProvider, cguValidationService);
		
		if (setupCgu)
		{
			cguInitializer.Context.LatestVersion = cguLatestVersion ?? "v2";
			cguInitializer.Context.UserVersion = cguUserVersion ?? "v2";
			cguInitializer.Context.LatestVersionLabelFile = $"CGU_{cguLatestVersion}.pdf";
			cguInitializer.Context.UserId = Guid.NewGuid();
		}
		else
		{
			cguInitializer.Context.LatestVersion = "v2";
			cguInitializer.Context.UserVersion = "v2";
		}
		
		ctx.Services.AddSingleton(cguInitializer);

		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		
		if (!string.IsNullOrEmpty(role))
			authContext.SetRoles(role);

		return ctx;
	}

	private static BunitContext SetupContextForAnahTests(
		string role = Constants.SolidarBuilderRole,
		bool shouldCheckAnahClaim = false,
		List<AccompanyingFileAwaitingAnahResponseResume>? pendingFiles = null)
	{
		var ctx = SetupContext();

		var cguValidationService = A.Fake<ICguValidationService>();
		var authProvider = A.Fake<AuthenticationStateProvider>();
		var cguInitializer = new CguInitializer(authProvider, cguValidationService);
		cguInitializer.Context.LatestVersion = "v2";
		cguInitializer.Context.UserVersion = "v2";
		ctx.Services.AddSingleton(cguInitializer);

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.HasAccompanyingFilesAwaitingAnahResponse(A<Guid>._))
			.Returns(ReneeOperationResult<bool>.Success((pendingFiles?.Count ?? 0) > 0));
		ctx.Services.AddSingleton(mockAccompanyingFileService);

		ctx.Services.AddSingleton(A.Fake<IModalService>());

		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, role),
			new(CustomClaimTypes.BasedRole, role)
		};

		if (shouldCheckAnahClaim)
			claims.Add(new Claim(CustomClaimTypes.ShouldCheckAnahFiles, "true"));

		authContext.SetClaims([.. claims]);

		return ctx;
	}

	private static List<AccompanyingFileAwaitingAnahResponseResume> GetFakePendingFiles(int count = 2)
	{
		var files = new List<AccompanyingFileAwaitingAnahResponseResume>();
		for (int i = 1; i <= count; i++)
		{
			files.Add(new AccompanyingFileAwaitingAnahResponseResume
			{
				AccompanyingFileId = Guid.NewGuid(),
				AccompanyingFileReference = $"REF-{i:D3}",
				OccupantFullName = $"Dupont Jean {i}",
				HousingAddress = $"{i} rue de la Paix, 75001 Paris"
			});
		}
		return files;
	}

	[Fact]
	public void MainLayout_WithESRole_HaveToDisplayHomeAndESMenu()
	{
		using var ctx = SetupContextForMenuAndCguTests(role: "ES");
		var cut = ctx.Render<MainLayout>();
		var menuItemsIcons = cut.FindAll(".rz-navigation-item-link");

		menuItemsIcons.Count.Should().Be(10);
	}

	[Fact]
	public void MainLayout_WithoutRoles_HaveToDisplayHome()
	{
		using var ctx = SetupContextForMenuAndCguTests();
		var cut = ctx.Render<MainLayout>();
		var menuItemsIcons = cut.FindAll(".rz-navigation-item-link");

		menuItemsIcons.Count.Should().Be(1);
	}

	[Fact]
	public void MainLayout_ShouldDisplayCGUModal_WhenShouldShowModalIsTrue()
	{
		using var ctx = SetupContextForMenuAndCguTests(
			setupCgu: true,
			cguUserVersion: "v1",
			cguLatestVersion: "v2");

		var cut = ctx.Render<MainLayout>();

		cut.FindComponents<CGUModal>().Should().ContainSingle();
		var modal = cut.FindComponent<CGUModal>();
		modal.Instance.Show.Should().BeTrue();
		modal.Instance.LabelCGU.Should().Be("CGU_v2.pdf");
	}

	[Fact]
	public void MainLayout_ShouldNotDisplayCGUModal_WhenShouldShowModalIsFalse()
	{
		using var ctx = SetupContextForMenuAndCguTests(
			setupCgu: true,
			cguUserVersion: "v2",
			cguLatestVersion: "v2");

		var cut = ctx.Render<MainLayout>();

		var modal = cut.FindComponent<CGUModal>();
		modal.Instance.Show.Should().BeFalse();
	}

	[Fact]
	public async Task MainLayout_ShouldNotShowAnahModal_WhenUserNotAuthenticated()
	{
		using var ctx = SetupContextForAnahTests();
		var authContext = ctx.AddAuthorization();
		authContext.SetNotAuthorized();

		var navMan = ctx.Services.GetRequiredService<NavigationManager>();

		ctx.Render<MainLayout>();
		await Task.Delay(100);

		navMan.Uri.Should().NotContain(Endpoints.AnahGrantCheck);
	}

	[Fact]
	public async Task MainLayout_ShouldNotShowAnahModal_WhenShouldCheckClaimIsFalse()
	{
		using var ctx = SetupContextForAnahTests(
			role: Constants.SolidarBuilderRole,
			shouldCheckAnahClaim: false);
		
		var navMan = ctx.Services.GetRequiredService<NavigationManager>();

		ctx.Render<MainLayout>();
		await Task.Delay(100);

		navMan.Uri.Should().NotContain(Endpoints.AnahGrantCheck);
	}

	[Fact]
	public async Task MainLayout_ShouldNotShowAnahModal_WhenNoPendingFiles()
	{
		using var ctx = SetupContextForAnahTests(
			role: Constants.SolidarBuilderRole,
			shouldCheckAnahClaim: true,
			pendingFiles: []);

		var navMan = ctx.Services.GetRequiredService<NavigationManager>();

		ctx.Render<MainLayout>();
		await Task.Delay(100);

		navMan.Uri.Should().NotContain(Endpoints.AnahGrantCheck);
	}

	[Fact]
	public async Task MainLayout_ShouldNavigateToAnahPage_WhenAllConditionsAreMet()
	{
		var pendingFiles = GetFakePendingFiles(3);
		using var ctx = SetupContextForAnahTests(
			role: Constants.SolidarBuilderRole,
			shouldCheckAnahClaim: true,
			pendingFiles: pendingFiles);

		var navMan = ctx.Services.GetRequiredService<NavigationManager>();

		ctx.Render<MainLayout>();
		await Task.Delay(100);

		navMan.Uri.Should().Contain(Endpoints.AnahGrantCheck);
	}

	[Fact]
	public async Task MainLayout_ShouldCallServiceWithCorrectUserId()
	{
		var userId = Guid.NewGuid();
		var ctx = SetupContextForAnahTests(
			role: Constants.SolidarBuilderRole,
			shouldCheckAnahClaim: true,
			pendingFiles: GetFakePendingFiles());

		var authContext = ctx.AddAuthorization();
		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
			new Claim(ClaimTypes.Role, Constants.SolidarBuilderRole),
			new Claim(CustomClaimTypes.BasedRole, Constants.SolidarBuilderRole),
			new Claim(CustomClaimTypes.ShouldCheckAnahFiles, "true")
		};
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);

		var mockAccompanyingFileService = ctx.Services.GetRequiredService<IAccompanyingFileService>();

		ctx.Render<MainLayout>();
		await Task.Delay(100);

		A.CallTo(() => mockAccompanyingFileService.HasAccompanyingFilesAwaitingAnahResponse(userId))
			.MustHaveHappenedOnceExactly();
	}
}