using Blazored.Modal;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Domain;
using Renee.UI;
using Renee.UI.Components;
using Renee.UI.Components.CGUHandling;
using System.Security.Claims;

namespace UITests.Components;

public class RoutesTests : BunitContext
{
	private NavigationManager _nav = default!;
	private UnsavedChangesGuard _guard = default!;

	private void Setup()
	{
		Services.AddBlazoredModal();
		Services.AddBlazorContextMenu();
		Services.AddScoped<NavigationHistoryManager>();
		Services.AddScoped<UnsavedChangesGuard>();
		Services.AddScoped<CguInitializer>();
		Services.AddSingleton(A.Fake<ICguVersionService>());
		Services.AddSingleton(A.Fake<ICguValidationService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<ISendEventQuery>());
		Services.AddSingleton(A.Fake<ITelemetryService>());
		Services.AddSingleton(A.Fake<IFileViewerService>());
		Services.AddSingleton(A.Fake<IAdministrationConstantsManagementService>());
		Services.AddSingleton(A.Fake<IAccompanyingFileService>());

		var auth = this.AddAuthorization();
		auth.SetAuthorized("tester");
		auth.SetRoles(Constants.SolidarBuilderRole);
		auth.SetClaims(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

		_nav = Services.GetRequiredService<NavigationManager>();
		_guard = Services.GetRequiredService<UnsavedChangesGuard>();

		JSInterop.Mode = JSRuntimeMode.Loose;
	}

	private void GoToMilestone(string milestoneUrl)
	{
		_nav.NavigateTo(milestoneUrl);
		_guard.UpdateCurrentUrl(_nav.Uri);
	}

	[Theory]
	[InlineData(Endpoints.NewOccupant)]
	[InlineData(Endpoints.OrganizeAndFinanceStage)]
	[InlineData(Endpoints.RealiseAndFollowStage)]
	public async Task GivenUnsavedChanges_WhenSaveAndLeaveSucceeds_ShouldNavigateAndStopIntercepting(string milestoneUrl)
	{
		// Arrange
		Setup();
		var cut = Render<Routes>();
		GoToMilestone(milestoneUrl);

		bool saveCalled = false;
		await _guard.SetUnsavedChangesState(true);
		_guard.SetSaveHandler(() =>
		{
			saveCalled = true;
			return Task.FromResult(true);
		});
		var target = $"{_nav.BaseUri}after-save";

		// Act
		_nav.NavigateTo(target);
		var saveAndLeaveButton = await cut.WaitForElementAsync("#unsaved-save-leave");
		await saveAndLeaveButton.ClickAsync();

		// Assert
		await cut.WaitForAssertionAsync(() =>
		{
			_nav.Uri.Should().Be(target);
			_guard.ShouldIntercept($"{_nav.BaseUri}next").Should().BeFalse();
			saveCalled.Should().BeTrue();
		});
	}

	[Theory]
	[InlineData(Endpoints.NewOccupant)]
	[InlineData(Endpoints.OrganizeAndFinanceStage)]
	[InlineData(Endpoints.RealiseAndFollowStage)]
	public async Task GivenUnsavedChanges_WhenLeaveWithoutSaving_ShouldNavigateWithoutSavingAndStopIntercepting(string milestoneUrl)
	{
		// Arrange
		Setup();
		var cut = Render<Routes>();
		GoToMilestone(milestoneUrl);

		await _guard.SetUnsavedChangesState(true);
		var saveCalled = false;
		_guard.SetSaveHandler(() =>
		{
			saveCalled = true;
			return Task.FromResult(true);
		});
		var target = $"{_nav.BaseUri}leave";

		// Act
		_nav.NavigateTo(target);
		var leaveWithoutSaveButton = await cut.WaitForElementAsync("#unsaved-leave-without-save");
		await leaveWithoutSaveButton.ClickAsync();

		// Assert
		await cut.WaitForAssertionAsync(() =>
		{
			_nav.Uri.Should().Be(target);
			saveCalled.Should().BeFalse();
			_guard.ShouldIntercept($"{_nav.BaseUri}x").Should().BeFalse();
		});
	}

	[Theory]
	[InlineData(Endpoints.NewOccupant)]
	[InlineData(Endpoints.OrganizeAndFinanceStage)]
	[InlineData(Endpoints.RealiseAndFollowStage)]
	public async Task GivenUnsavedChanges_WhenCancelNavigation_ShouldStayAndContinueIntercepting(string milestoneUrl)
	{
		// Arrange
		Setup();
		var cut = Render<Routes>();
		GoToMilestone(milestoneUrl);

		var origin = _nav.Uri;
		var target = $"{_nav.BaseUri}cancel";

		await _guard.SetUnsavedChangesState(true);
		_guard.SetSaveHandler(() => Task.FromResult(true));

		// Act
		_nav.NavigateTo(target);
		var cancelButton = await cut.WaitForElementAsync("#unsaved-cancel");
		await cancelButton.ClickAsync();

		// Assert
		await cut.WaitForAssertionAsync(() =>
		{
			_nav.Uri.Should().Be(origin);
			_guard.ShouldIntercept($"{_nav.BaseUri}again").Should().BeTrue();
		});
	}

	[Theory]
	[InlineData(Endpoints.NewOccupant)]
	[InlineData(Endpoints.OrganizeAndFinanceStage)]
	[InlineData(Endpoints.RealiseAndFollowStage)]
	public async Task GivenUnsavedChanges_WhenSaveAndLeaveFails_ShouldStayAndContinueIntercepting(string milestoneUrl)
	{
		// Arrange
		Setup();
		var cut = Render<Routes>();
		GoToMilestone(milestoneUrl);

		var origin = _nav.Uri;
		await _guard.SetUnsavedChangesState(true);
		_guard.SetSaveHandler(() => Task.FromResult(false));
		var target = $"{_nav.BaseUri}fail";

		// Act
		_nav.NavigateTo(target);
		var saveAndLeaveButton = await cut.WaitForElementAsync("#unsaved-save-leave");
		await saveAndLeaveButton.ClickAsync();

		// Assert
		await cut.WaitForAssertionAsync(() =>
		{
			_nav.Uri.Should().Be(origin);
			_guard.ShouldIntercept($"{_nav.BaseUri}other").Should().BeTrue();
		});
	}
}
