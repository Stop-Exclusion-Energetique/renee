using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Application.Interfaces;
using Renee.Application.Services;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.ViewModel;
using Renee.UI.Components.FormComponents;

namespace UITests.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details;

public class PreWorkPlanTabTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		var mockWorkTypesLabelsRepository = A.Fake<IWorkTypesLabelsService>();
		A.CallTo(() => mockWorkTypesLabelsRepository.GetAllWorkTypesLabels()).Returns(
			ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>.Success(
			new List<WorkTypesLabelsDto>
			{
				new(Guid.NewGuid(), "travaux 1"),
				new(Guid.NewGuid(), "travaux 2"),
				new(Guid.NewGuid(), "travaux 3"),
				new(Guid.NewGuid(), "travaux 4"),
				new(Guid.NewGuid(), "travaux 5"),
				new(Guid.NewGuid(), "travaux 6")
			}));
		ctx.Services.AddSingleton(mockWorkTypesLabelsRepository);

		var mockProjectTypes = A.Fake<IProjectTypeService>();
		A.CallTo(() => mockProjectTypes.GetAllProjectTypes()).Returns(
			ReneeOperationResult<IEnumerable<ProjectTypeDto>>.Success(
			new List<ProjectTypeDto>
			{
				new(Guid.NewGuid(), "Travaux d'urgence"),
				new(Guid.NewGuid(), "Travaux de rénovation énergétique"),
				new(Guid.NewGuid(), "Travaux induits"),
				new(Guid.NewGuid(), "Travaux de sécurité et salubrité"),
				new(Guid.NewGuid(), "Étanchéité à l'air traitée"),
				new(Guid.NewGuid(), "Ponts thermiques traités")
			}));
		ctx.Services.AddSingleton(mockProjectTypes);

		var mockInsuranceTypes = A.Fake<IInsuranceTypeService>();
		A.CallTo(() => mockInsuranceTypes.GetAllInsuranceTypes()).Returns(
			ReneeOperationResult<IEnumerable<InsuranceTypeDto>>.Success(
			new List<InsuranceTypeDto>
			{
				new(Guid.NewGuid(), "Assurance Multirisques habitation de l'occupant"),
				new(Guid.Parse("03fc5ee5-df60-4c31-97eb-11c89935dcfa"), "Responsabilité civile décennale ARA"),
				new(Guid.NewGuid(), "Responsabilité civile décennale Artisans/Entreprises")
			}));
		ctx.Services.AddSingleton(mockInsuranceTypes);

		ctx.Services.AddSingleton(A.Fake<IProjectTypeService>());
		ctx.Services.AddSingleton(A.Fake<IInsuranceTypeService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<IWorkTypeProjectTypeService>());
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;

		return ctx;
	}

	[Fact]
	public void OnClickOnAddPreWorkPlanButton_WhenOnlyOnePreWorkPlan_ShouldFindTheSecondPreWorkPlanTitle()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel();
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var addPreWorkPlanButton =
			cut.FindComponents<ZeeButton>().FirstOrDefault(p => p.Instance.Text == "Ajouter un devis");
		addPreWorkPlanButton?.Find("button").Click();

		// Assert
		var preWorkPlans = cut.FindAll(".wrapper h2").Where(c => c.InnerHtml.Contains("Devis"));
		preWorkPlans.ElementAt(1).InnerHtml.Trim().Should().Be("Devis 2");
	}

	[Fact]
	public void OnClickOnAddPreWorkPlanButton_WhenOnlyOnePreWorkPlan_ShouldReturnTwoPreWorkPlans()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel();
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var addPreWorkPlanButton =
			cut.FindComponents<ZeeButton>().FirstOrDefault(p => p.Instance.Text == "Ajouter un devis");
		addPreWorkPlanButton?.Find("button").Click();

		// Assert
		cut.Instance.ViewModel.WorkPackages.Count.Should().Be(2);
	}

	[Fact]
	public void OnClickOnDeleteTheSecondWorkPackage_WhenFourWorkPackages_WorkPackagesCountShouldBeThree()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel
		{
			WorkPackages =
			[
				new WorkPackageViewModel(), new WorkPackageViewModel(), new WorkPackageViewModel(),
				new WorkPackageViewModel()
			]
		};
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var deletePreWorkPlanButton =
			cut.FindComponents<RadzenButton>().FirstOrDefault(p => p.Instance.Icon == Icons.DeleteButton);
		deletePreWorkPlanButton?.Find("button").Click();

		// Assert
		cut.Instance.ViewModel.WorkPackages.Count.Should().Be(3);
	}

	[Fact]
	public void OnPreWorkPlanTab_WithOneWorkPackage_DeleteButtonShouldBeNull()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel { WorkPackages = [new WorkPackageViewModel()] };
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var deleteWorkPackageButton =
			cut.FindComponents<RadzenButton>().FirstOrDefault(p => p.Instance.Icon == Icons.DeleteButton);

		// Assert
		deleteWorkPackageButton.Should().BeNull();
	}

	[Fact]
	public void
		WhenEstimatedAnnualEnergyConsumptionAfterWorkFormField_IsCompleted_EnergyGainAutomaticCalculationShouldBeValid()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel { EstimatedAnnualEnergyConsumptionBeforeWork = 100 };
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var estimatedAnnualEnergyConsumptionAfterWorkField = cut.FindComponents<OutlinedDouble>().FirstOrDefault(
			p => p.Instance.Label == "Consommation énergétique annuelle estimée après travaux (kWh/m²)");
		estimatedAnnualEnergyConsumptionAfterWorkField?.Find("input").Change(58);

		// Assert
		cut.Instance.ViewModel.EnergyGain.Should().Be(42);
	}

	[Fact]
	public void
		WhenEstimatedAnnualEnergyConsumptionAfterWorkFormField_IsCompleted_KWhSavedPerYearAutomaticCalculationShouldBeValid()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel { EstimatedAnnualEnergyConsumptionBeforeWork = 100 };
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var estimatedAnnualEnergyConsumptionAfterWorkField = cut.FindComponents<OutlinedDouble>().FirstOrDefault(
			p => p.Instance.Label == "Consommation énergétique annuelle estimée après travaux (kWh/m²)");
		estimatedAnnualEnergyConsumptionAfterWorkField?.Find("input").Change(58);

		// Assert
		cut.Instance.ViewModel.KWhSavedPerYear.Should().Be(42);
	}

	[Fact]
	public void WhenEstimatedAnnualEnergyConsumptionBeforeWork_IsNullOrEmpty_EnergyGainValueShouldBeNull()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel { EstimatedAnnualEnergyConsumptionBeforeWork = null };
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var estimatedAnnualEnergyConsumptionAfterWorkField = cut.FindComponents<OutlinedDouble>().FirstOrDefault(
			p => p.Instance.Label == "Consommation énergétique annuelle estimée après travaux (kWh/m²)");
		estimatedAnnualEnergyConsumptionAfterWorkField?.Find("input").Change(58);

		// Assert
		cut.Instance.ViewModel.EnergyGain.Should().BeNull();
	}

	[Fact]
	public void
		WhenEstimatedAnnualGHGEmissionsAfterWorkFormField_IsCompleted_GHGEmissionsAvoidedPerYearAutomaticCalculationShouldBeValid()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel { EstimatedAnnualGhgEmissionsBeforeWork = 100 };
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Act
		var estimatedAnnualGhgEmissionsAfterWorkField = cut.FindComponents<OutlinedDouble>().FirstOrDefault(
			p => p.Instance.Label == "Emissions GES annuelles estimées après travaux (TonnesEqCO²)");
		estimatedAnnualGhgEmissionsAfterWorkField?.Find("input").Change(58);

		// Assert
		cut.Instance.ViewModel.GhgEmissionsAvoidedPerYear.Should().Be(42);
	}

	[Fact]
	public void WhenRenovationTypeFormField_IsEfficientRenovationInStages_ShouldShowNextStepAndVigilancePointsField()
	{
		// Arrange
		var viewModel = new PreWorkPlanTabViewModel { RenovationType = RenovationType.EfficientRenovationInStages };
		var editContext = new EditContext(viewModel);

		var ctx = SetupContext();
		var cut = ctx.Render<PreWorkPlanTab>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Assert
		cut.FindComponents<OutlinedTextArea>()
			.FirstOrDefault(p => p.Instance.Label!.Equals("Prochaine(s) étape(s) et points de vigilance")).Should()
			.NotBeNull();
	}
}