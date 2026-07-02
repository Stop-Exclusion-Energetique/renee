using FluentAssertions;
using Renee.Domain;
using Renee.UI;

namespace UITests;

public class UnsavedChangesGuardTests : BunitContext
{
	public UnsavedChangesGuardTests()
	{
		JSInterop.SetupVoid("unsavedChangesGuard.enable").SetVoidResult();
		JSInterop.SetupVoid("unsavedChangesGuard.disable").SetVoidResult();
	}

	[Fact]
	public async Task WhenNoUnsavedChangesOnMilestone_ShouldNotIntercept()
	{
		// Arrange
		var guard = new UnsavedChangesGuard(JSInterop.JSRuntime);
		guard.UpdateCurrentUrl(Endpoints.NewOccupant);
		await guard.SetUnsavedChangesState(false);

		// Act
		var intercept = guard.ShouldIntercept("https://app/other");

		// Assert
		intercept.Should().BeFalse();
	}

	[Fact]
	public async Task WhenNavigatingWithinSameUrl_ShouldNotIntercept()
	{
		// Arrange
		var guard = new UnsavedChangesGuard(JSInterop.JSRuntime);
		guard.UpdateCurrentUrl("testUrl");
		await guard.SetUnsavedChangesState(true);

		// Act
		var intercept = guard.ShouldIntercept("testUrl");

		// Assert
		intercept.Should().BeFalse();
	}

	[Fact]
	public async Task WhenUnsavedChangesAndLeavingMilestone_ShouldIntercept()
	{
		// Arrange
		var guard = new UnsavedChangesGuard(JSInterop.JSRuntime);
		guard.UpdateCurrentUrl(Endpoints.NewOccupant);
		await guard.SetUnsavedChangesState(true);

		// Act
		var intercept = guard.ShouldIntercept("https://app/other");

		// Assert
		intercept.Should().BeTrue();
	}

	[Fact]
	public async Task SaveAsync_NoHandler_ShouldReturnsFalse()
	{
		// Arrange
		var guard = new UnsavedChangesGuard(JSInterop.JSRuntime);
		await guard.SetUnsavedChangesState(true);

		// Act
		var result = await guard.SaveAsync();

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public async Task SaveAsync_HandlerSuccess_ShouldNotDisableInterceptionAfterwards()
	{
		// Arrange
		var guard = new UnsavedChangesGuard(JSInterop.JSRuntime);
		guard.UpdateCurrentUrl(Endpoints.NewOccupant);
		await guard.SetUnsavedChangesState(true);
		guard.SetSaveHandler(() => Task.FromResult(true));

		// Act
		var save = await guard.SaveAsync();
		var interceptAfterSave = guard.ShouldIntercept("https://app/other");

		// Assert
		save.Should().BeTrue();
		interceptAfterSave.Should().BeTrue();
	}

	[Fact]
	public async Task SaveAsync_HandlerFailure_ShouldIntercept()
	{
		// Arrange
		var guard = new UnsavedChangesGuard(JSInterop.JSRuntime);
		guard.UpdateCurrentUrl(Endpoints.NewOccupant);
		await guard.SetUnsavedChangesState(true);
		guard.SetSaveHandler(() => Task.FromResult(false));

		// Act
		var save = await guard.SaveAsync();
		var intercept = guard.ShouldIntercept("https://app/other");

		// Assert
		save.Should().BeFalse();
		intercept.Should().BeTrue();
	}
}