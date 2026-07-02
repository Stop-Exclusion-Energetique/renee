using Blazored.Modal.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.ReasonOfProject;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.ReasonOfProject.ViewModel;
using Renee.UI.Components.FormComponents;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class ReasonOfProjectTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IJSRuntime>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		return ctx;
	}

	[Fact]
	public void OnDisplay_CalculateDeliveryTime_ShouldBeFalse()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new ReasonOfProjectViewModel
		{
			FirstEncounterDate = new DateTime(2024, 9, 25, 0, 0, 0, DateTimeKind.Utc),
			SigningHouseholdSupportDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc)
		};
		var editContext = new EditContext(viewModel);
		var difficultiesFacedFamily = new List<ZeeSelectItem<Guid?>>();

		var cut = ctx.Render<ReasonOfProject>(
			parameters => parameters.Add(p => p.EditContext, editContext).Add(p => p.ViewModel, viewModel).Add(
				p => p.DifficultiesFacedFamily,
				difficultiesFacedFamily));
		// Act
		var labelCalculateDelivery = cut.Find(".bold-text");

		//Assert
		labelCalculateDelivery.TextContent.Trim().Should().Be("");
	}

	[Fact]
	public void OnDisplay_CalculateDeliveryTime_ShouldBeTrue()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new ReasonOfProjectViewModel
		{
			FirstEncounterDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc),
			SigningHouseholdSupportDate = new DateTime(2024, 9, 25, 0, 0, 0, DateTimeKind.Utc)
		};
		var editContext = new EditContext(viewModel);
		var difficultiesFacedFamily = new List<ZeeSelectItem<Guid?>>();

		var cut = ctx.Render<ReasonOfProject>(
			parameters => parameters.Add(p => p.EditContext, editContext).Add(p => p.ViewModel, viewModel).Add(
				p => p.DifficultiesFacedFamily,
				difficultiesFacedFamily));
		// Act
		var labelCalculateDelivery = cut.Find(".bold-text");

		//Assert
		labelCalculateDelivery.TextContent.Trim().Should().Be("0");
	}

	[Fact]
	public void OnDisplay_CalculateDeliveryTimeWhenLessThan_15_Days_ShouldBeTrue()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new ReasonOfProjectViewModel
		{
			FirstEncounterDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc),
			SigningHouseholdSupportDate = new DateTime(2024, 10, 14, 0, 0, 0, DateTimeKind.Utc)
		};
		var editContext = new EditContext(viewModel);
		var difficultiesFacedFamily = new List<ZeeSelectItem<Guid?>>();

		var cut = ctx.Render<ReasonOfProject>(
			parameters => parameters.Add(p => p.EditContext, editContext).Add(p => p.ViewModel, viewModel).Add(
				p => p.DifficultiesFacedFamily,
				difficultiesFacedFamily));
		// Act
		var labelCalculateDelivery = cut.Find(".bold-text");

		//Assert
		labelCalculateDelivery.TextContent.Trim().Should().Be("1");
	}

	[Fact]
	public void OnDisplay_CalculateDeliveryTimeWhenMoreThan_15_Days_ShouldBeTrue()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new ReasonOfProjectViewModel
		{
			FirstEncounterDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc),
			SigningHouseholdSupportDate = new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc)
		};
		var editContext = new EditContext(viewModel);
		var difficultiesFacedFamily = new List<ZeeSelectItem<Guid?>>();

		var cut = ctx.Render<ReasonOfProject>(
			parameters => parameters.Add(p => p.EditContext, editContext).Add(p => p.ViewModel, viewModel).Add(
				p => p.DifficultiesFacedFamily,
				difficultiesFacedFamily));
		// Act
		var labelCalculateDelivery = cut.Find(".bold-text");

		//Assert
		labelCalculateDelivery.TextContent.Trim().Should().Be("2");
	}
}