using Blazored.Modal.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.EnergeticProfil;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.EnergeticProfil.ViewModel;
using Renee.UI.Components.FormComponents;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class EnergeticProfileTests : BunitContext
{
	[Fact]
	public void SelectDpe_OnClickOnE_AskedGesLabelShouldBeE()
	{
		// Arrange
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<IJSRuntime>());
		Services.AddSingleton(A.Fake<NotificationService>());

		var viewModel = new EnergyProfileViewModel();
		var editContext = new EditContext(viewModel);

		var cut = Render<EnergyProfile>(
			parameters => parameters.Add(p => p.EditContext, editContext).Add(p => p.ViewModel, viewModel));

		var dpeSelect = cut.FindComponent<ZeeSelectDpe>();
		var dpeInputs = dpeSelect.FindAll("input");

		// Act
		dpeInputs[4].Click();

		// Assert
		cut.Instance.ViewModel.AskedDpeLabel.Should().Be(DpeLabel.E);
	}

	[Fact]
	public void SelectGes_OnClickOnLast_AskedGesLabelShouldBeG()
	{
		// Arrange
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<IJSRuntime>());
		Services.AddSingleton(A.Fake<NotificationService>());

		var viewModel = new EnergyProfileViewModel();
		var editContext = new EditContext(viewModel);

		var cut = Render<EnergyProfile>(
			parameters => parameters.Add(p => p.EditContext, editContext).Add(p => p.ViewModel, viewModel));

		var gesSelect = cut.FindComponent<ZeeSelectGes>();
		var gesInputs = gesSelect.FindAll("input");

		// Act
		gesInputs[gesInputs.Count - 1].Click();

		// Assert
		cut.Instance.ViewModel.AskedGesLabel.Should().Be(GesLabel.G);
	}

	[Fact]
	public void EnergyFields_ShouldBeDisabled_WhenAccompanyingFileIsLinkedToCoproperty()
	{
		// Arrange
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<IJSRuntime>());
		Services.AddSingleton(A.Fake<NotificationService>());

		var viewModel = new EnergyProfileViewModel();
		var editContext = new EditContext(viewModel);

		//Act
		var cut = Render<EnergyProfile>(parameters => parameters
			.Add(p => p.EditContext, editContext)
			.Add(p => p.ViewModel, viewModel)
			.Add(p => p.IsLinkedToCoproperty, true));

		var energyConsumptionInput = cut.FindAll("input[type='text']")[0];

		// Assert
		energyConsumptionInput.HasAttribute("disabled").Should().BeTrue();
	}
}