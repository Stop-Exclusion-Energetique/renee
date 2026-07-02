using System.Security.Claims;
using Blazored.Modal.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.CommandsUseCasesInput.Results;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Synthesis.Modal.ViewModel;

namespace UITests.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Synthesis;

public class OrganizeAndFinanceSynthesisValidationModalTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IJSRuntime>());
		ctx.Services.AddSingleton(A.Fake<IFileService>());

		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		var mockAccompanyingService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingService.GetOrganizeAndFinanceValidationSynthesisModalData(A<Guid>.Ignored))
			.Returns(
				ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>.Success(
				new OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto
				{
					OwnershipStatus = Labels.FullOwnership,
					AnahCategory = Labels.LowIncomeHouseholdsAmount,
					DpeLabel = Labels.DpeB,
					GesLabel = Labels.GesF,
					BlockingProofComment = "test",
					BlockingProofIds = new List<Guid>(),
					IsIncludedInTzeeProgram = true
				}));

		A.CallTo(
			() => mockAccompanyingService.UpdateAccompanyingFileOrganizeAndFinanceSynthesis(
				A<Guid>._,
				A<AccompanyingFileStatus>._,
				A<AccompanyingFileStage>._,
				A<Guid>._,
				A<bool>._)).Returns(
					ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>.Success(
						new RequiredDataForStageSynthesisValidationEmail(
							AccompanyingFileStage.Identify,
							string.Empty,
							AccompanyingType.Diffuse,
							string.Empty,
							[])));

		ctx.Services.AddSingleton(mockAccompanyingService);

		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		authContext.SetClaims(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

		return ctx;
	}

	[Fact]
	public void OrganizeAndFinanceSynthesisValidationModal_InputTextFieldsAreReadonly()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var viewModel = new OrganizeAndFinanceSynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeE,
			GesLabel = Labels.GesA,
			EnergyClassJump = "3"
		};

		var cut = ctx.Render<OrganizeAndFinanceSynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel));

		//Act
		var inputFields = cut.FindAll("input[type='text']");

		//Assert
		inputFields.Should().NotBeNullOrEmpty();

		foreach (var inputField in inputFields) inputField.HasAttribute("readonly").Should().BeTrue();
	}

	[Fact]
	public void
		OrganizeAndFinanceSynthesisValidationModal_WhenIsFolderValidatedForCEEProgram_IsFalse_ShouldCheckTheBoxWithFalse()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var viewModel = new OrganizeAndFinanceSynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeB,
			GesLabel = Labels.GesA,
			EnergyClassJump = "1"
		};

		var cut = ctx.Render<OrganizeAndFinanceSynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel));

		//Act
		var noCheckBox = cut.Find("input[type=radio][Value=False]");

		//Assert
		viewModel.IsAccompanyingFileValidatedForCeeProgram.Should().BeFalse();
		noCheckBox.HasAttribute("checked").Should().BeTrue();
	}

	[Fact]
	public void
		OrganizeAndFinanceSynthesisValidationModal_WhenIsFolderValidatedForCEEProgram_IsTrue_AndIsReadyForTheNextStage_IsTrue_RealiseAndFollowPageShouldBeRender()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		ctx.JSInterop.SetupVoid("Radzen.preventArrows", _ => true);

		var navigationManager = ctx.Services.GetService<NavigationManager>();

		var viewModel = new OrganizeAndFinanceSynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeE,
			EnergyClassJump = "4"
		};

		var cut = ctx.Render<OrganizeAndFinanceSynthesisValidationModal>(
			parameters => parameters
				.Add(p => p.ViewModel, viewModel)
				.Add(p => p.AnahGrantNotificationStream, new MemoryStream())
				.Add(p => p.EnergyAuditStream, new MemoryStream()));

		//Act
		var buttons = cut.FindAll("button");
		var submitButton = buttons.FirstOrDefault(button => button.QuerySelector("span")?.TextContent == Labels.Submit);
		submitButton?.Click();

		//Assert
		navigationManager?.Uri.Should()
			.Be($"{navigationManager.BaseUri}myaccompanyingfiles");
	}

	[Fact]
	public void
		OrganizeAndFinanceSynthesisValidationModal_WhenIsFolderValidatedForCEEProgram_IsTrue_ShouldCheckTheBoxWithTrue()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var viewModel = new OrganizeAndFinanceSynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(), EnergyClassJump = "3"
		};

		var cut = ctx.Render<OrganizeAndFinanceSynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel));

		//Act
		var yesCheckBox = cut.Find("input[type=radio][Value=True]");

		//Assert
		viewModel.IsAccompanyingFileValidatedForCeeProgram.Should().BeTrue();
		yesCheckBox.HasAttribute("checked").Should().BeTrue();
	}

	[Fact]
	public void
		SynthesisValidationModal_WhenIsFolderValidatedForCEEProgram_IsTrue_AndIsReadyForTheNextStage_IsFalse_AccompanyingFileListShouldBeRender()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		ctx.JSInterop.SetupVoid("Radzen.preventArrows", _ => true);

		var navigationManager = ctx.Services.GetService<NavigationManager>();

		var viewModel = new OrganizeAndFinanceSynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeE,
			EnergyClassJump = "4"
		};

		var cut = ctx.Render<OrganizeAndFinanceSynthesisValidationModal>(
			parameters => parameters
			.Add(p => p.ViewModel, viewModel)
			.Add(p => p.AnahGrantNotificationStream, new MemoryStream())
			.Add(p => p.EnergyAuditStream, new MemoryStream()));

		//Act
		var buttons = cut.FindAll("button");
		var submitButton = buttons.FirstOrDefault(button => button.QuerySelector("span")?.TextContent == Labels.Submit);
		submitButton?.Click();

		//Assert
		navigationManager?.Uri.Should().Be($"{navigationManager.BaseUri}myaccompanyingfiles");
	}
}