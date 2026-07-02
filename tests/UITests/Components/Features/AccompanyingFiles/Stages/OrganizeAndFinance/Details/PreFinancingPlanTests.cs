using AirtableApiClient;
using Blazored.Modal.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.FundingMode;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.FundingMode.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;
using Renee.UI.Components.FormComponents;

namespace UITests.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details;

public class PreFinancingPlanTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());

		var mockWorkTypesLabelsService = A.Fake<IWorkTypesLabelsService>();
		A.CallTo(() => mockWorkTypesLabelsService.GetAllWorkTypesLabels())
			.Returns(Task.FromResult(ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>.Success([])));
		ctx.Services.AddSingleton(mockWorkTypesLabelsService);

		var mockAirtableService = A.Fake<IAirtableService>();
		A.CallTo(() => mockAirtableService.AddRecordAsync(
			A<string?>._,
			A<string?>._,
			A<string?>._,
			A<string>._!,
			A<Guid>._,
			A<double>._)).Returns(
			ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>.Success(
				new AirtableCreateUpdateReplaceRecordResponse(new AirtableRecord())));
		ctx.Services.AddSingleton(mockAirtableService);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void OnClickOnAddFundingMode_WhenFundingModeListCountIsThree_FundingModeListShouldBeEqualToFour()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new PreFinancingPlanViewModel();
		viewModel.FundingModes.Add(new FundingModeViewModel());
		viewModel.FundingModes.Add(new FundingModeViewModel());
		viewModel.FundingModes.Add(new FundingModeViewModel());
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var addFundingModeButton = cut.FindComponents<ZeeButton>()
			.FirstOrDefault(p => p.Instance.Text == "Ajouter un autre mode de financement");
		addFundingModeButton?.Find("button").Click();

		// Assert
		cut.Instance.ViewModel.FundingModes.Count.Should().Be(4);
	}

	[Fact]
	public void OnClickOnAddFundingMode_WhenFundingModeListIsEmpty_FundingModeListShouldBeEqualToOne()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new PreFinancingPlanViewModel();
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var addFundingModeButton = cut.FindComponents<ZeeButton>()
			.FirstOrDefault(p => p.Instance.Text == "Ajouter un autre mode de financement");
		addFundingModeButton?.Find("button").Click();

		// Assert
		cut.Instance.ViewModel.FundingModes.Count.Should().Be(1);
	}

	[Fact]
	public void OnClickOnRemoveFundingMode_WhenFundingModeListCountIsOne_FundingModeListShouldBeEqualToZero()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new PreFinancingPlanViewModel();
		viewModel.FundingModes.Add(new FundingModeViewModel());
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var fundingMode = cut.FindComponent<FundingMode>().FindComponent<RadzenButton>();
		fundingMode.Find("button").Click();

		// Assert
		cut.Instance.ViewModel.FundingModes.Count.Should().Be(0);
	}

	[Fact]
	public void OnInputNewFundingModeNameAlreadyExistingWithDifferentCaseCharacters_ErrorMessageShouldBeRendered()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new PreFinancingPlanViewModel
		{
			FundingModes = [new FundingModeViewModel { Name = "Test" }, new FundingModeViewModel()]
		};
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var fundingModeComponents = cut.FindComponents<FundingMode>();
		var inputFundingModeName =
			fundingModeComponents[fundingModeComponents.Count - 1].FindComponents<OutlinedText>()[0].Find("input");

		inputFundingModeName.Change("test");

		var validationMessage = cut.FindAll(".validation-message")
			.FirstOrDefault(m => m.TextContent == Labels.Errors.ExistingFundingModeName);

		// Assert
		validationMessage.Should().NotBeNull();
	}

	[Fact]
	public void
		OnInputNewFundingModeNameAlreadyExistingWithDifferentCaseCharacters_IsDuplicateFundingModeNameShouldBeTrue()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new PreFinancingPlanViewModel
		{
			FundingModes = [new FundingModeViewModel { Name = "Test" }, new FundingModeViewModel()]
		};
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var fundingModeComponents = cut.FindComponents<FundingMode>();
		var inputFundingModeName =
			fundingModeComponents[fundingModeComponents.Count - 1].FindComponents<OutlinedText>()[0].Find("input");

		inputFundingModeName.Change("test");

		// Assert
		cut.Instance.IsDuplicateFundingModeName.Should().BeTrue();
	}

	[Fact]
	public void OnInputNewFundingModeNameNotAlreadyExisting_ErrorMessageShouldNotBeRendered()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new PreFinancingPlanViewModel
		{
			FundingModes = [new FundingModeViewModel { Name = "Test" }, new FundingModeViewModel()]
		};
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act

		var fundingModeComponents = cut.FindComponents<FundingMode>();
		var inputFundingModeName =
			fundingModeComponents[fundingModeComponents.Count - 1].FindComponents<OutlinedText>()[0].Find("input");

		inputFundingModeName.Change("test2");
		var validationMessage = cut.FindAll(".validation-message")
			.FirstOrDefault(m => m.TextContent == Labels.Errors.ExistingFundingModeName);

		// Assert
		validationMessage.Should().BeNull();
	}

	[Fact]
	public void OnInputNewFundingModeNameNotAlreadyExisting_IsDuplicateFundingModeNameShouldBeFalse()
	{
		// Arrange
		var ctx = SetupContext();

		var viewModel = new PreFinancingPlanViewModel
		{
			FundingModes = [new FundingModeViewModel { Name = "Test" }, new FundingModeViewModel()]
		};
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var fundingModeComponents = cut.FindComponents<FundingMode>();
		var inputFundingModeName =
			fundingModeComponents[fundingModeComponents.Count - 1].FindComponents<OutlinedText>()[0].Find("input");

		inputFundingModeName.Change("test2");

		// Assert
		cut.Instance.IsDuplicateFundingModeName.Should().BeFalse();
	}

	[Fact]
	public void PreFinancingPlan_DisplayTextEstablishmentOfIntermunicipalCooperation()
	{
		// Arrange
		using var ctx = SetupContext();
		var viewModel = new PreFinancingPlanViewModel
		{
			FundingModes = [new FundingModeViewModel { Name = "Test" }, new FundingModeViewModel()]
		};
		var editContext = new EditContext(viewModel);

		var cut = ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Act
		var OutlinedDoubleComponents = cut.FindComponents<OutlinedDouble>();
		var OutlinedDoubleName = OutlinedDoubleComponents.FirstOrDefault(m =>
			m.Instance.Label == "Etablissements publics de coopération intercommunale/Agglomération (€)");

		// Assert
		OutlinedDoubleName.Should().NotBeNull();
	}

	[Fact]
	public void
		WhenUnderprivilegedHousingFoundationAndLeroyMerlinFoundationAndWattForChangeFoundationAndSocialProtectionGroupFieldsAreNotCompleted_PrivateActorsTotalPropertyShouldBeNull()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new PreFinancingPlanViewModel();

		var editContext = new EditContext(viewModel);

		ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Assert
		viewModel.PrivateAidTotalAmount.Should().Be(0);
	}

	[Fact]
	public void
		WhenUnderprivilegedHousingFoundationAndLeroyMerlinFoundationFieldsAreCompleted_PrivateActorsTotalPropertyShouldHaveTheRightAmount()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new PreFinancingPlanViewModel { UnderprivilegedHousingFoundation = 20, LeroyMerlinFoundation = 20 };

		var editContext = new EditContext(viewModel);

		ctx.Render<PreFinancingPlan>(parameters =>
			parameters.Add(p => p.ViewModel, viewModel).Add(p => p.FormEditContext, editContext));

		// Assert
		viewModel.PrivateAidTotalAmount.Should().Be(40);
	}
}