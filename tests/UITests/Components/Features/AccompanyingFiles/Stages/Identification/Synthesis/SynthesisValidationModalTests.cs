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
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal.ViewModel;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Synthesis;

public class SynthesisValidationModalTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IFileService>());
		ctx.Services.AddSingleton(A.Fake<IJSRuntime>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var mockAccompanyingService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingService.GetIdentifyValidationSynthesisModalData(A<Guid>.Ignored)).Returns(
			ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>.Success(
			new IdentifyAccompanyingFileValidationSynthesisModalDto
			{
				BlockingProofComment = "test", BlockingProofIds = new List<Guid>(), UserId = Guid.NewGuid(), IsIncludedInTzeeProgram = true
			}));

		A.CallTo(
			() => mockAccompanyingService.UpdateAccompanyingFileIdentificationSynthesis(
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
	public void SynthesisValidationModal_InputFieldsAreReadonly()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();

		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<IdentifySynthesisValidationModal>();

		//Act
		var inputFields = cut.FindAll("input[readonly]");

		//Assert
		Assert.NotEmpty(inputFields);
		foreach (var inputField in inputFields) Assert.True(inputField.HasAttribute("readonly"));
	}

	[Fact]
	public void
		SynthesisValidationModal_WhenCoordinatorIsFolderValidatedForCEEProgram_IsTrue_ShouldCheckTheBoxWithTrue()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var viewModel = new IdentifySynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeC,
			IsAccompanyingFileValidatedForCeeProgram = true,
			UserRole = Constants.TargetedCoordinatorRole
		};

		var cut = ctx.Render<IdentifySynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel));

		//Act
		var yesCheckBox = cut.Find("input[type=radio][Value=True]");

		//Assert
		viewModel.IsAccompanyingFileValidatedForCeeProgram.Should().BeTrue();
		Assert.True(yesCheckBox.HasAttribute("checked"));
	}

	[Fact]
	public void SynthesisValidationModal_WhenIsFolderValidatedForCEEProgram_IsFalse_ShouldCheckTheBoxWithFalse()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var viewModel = new IdentifySynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.Tenant,
			DpeLabel = Labels.DpeB
		};

		var cut = ctx.Render<IdentifySynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel));

		//Act
		var noCheckBox = cut.Find("input[type=radio][Value=False]");

		//Assert
		viewModel.IsAccompanyingFileValidatedForCeeProgram.Should().BeFalse();
		Assert.True(noCheckBox.HasAttribute("checked"));
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

		var viewModel = new IdentifySynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeE
		};

		var cut = ctx.Render<IdentifySynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).Add(p => p.MemoryStream, new MemoryStream()));

		//Act
		var buttons = cut.FindAll("button");
		var submitButton = buttons.FirstOrDefault(button => button.QuerySelector("span")?.TextContent == Labels.Submit);
		submitButton?.Click();

		//Assert
		Assert.Equal($"http://localhost{Endpoints.UserCreatedAccompanyingFiles}", navigationManager?.Uri);
	}

	[Fact]
	public void SynthesisValidationModal_WhenIsFolderValidatedForCEEProgram_IsTrue_ShouldCheckTheBoxWithTrue()
	{
		//Arrange
		var ctx = SetupContext();

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var viewModel = new IdentifySynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = Labels.FullOwnership,
			DpeLabel = Labels.DpeE
		};

		var cut = ctx.Render<IdentifySynthesisValidationModal>(
			parameters => parameters.Add(p => p.ViewModel, viewModel));

		//Act
		var yesCheckBox = cut.Find("input[type=radio][Value=True]");

		//Assert
		viewModel.IsAccompanyingFileValidatedForCeeProgram.Should().BeTrue();
		Assert.True(yesCheckBox.HasAttribute("checked"));
	}

	[Fact]
	public void
		SynthesisValidationModal_WhenOwnershipStatusIsCoOwner_And_OtherValueAreRight_IsAccompanyingFileValidatedForCeeProgram_ShouldBeTrue()
	{
		//Arrange
		var viewModel = new IdentifySynthesisValidationModalViewModel
		{
			AccompanyingFileId = Guid.NewGuid(),
			AnahCategory = Labels.LowIncomeHouseholdsAmount,
			OwnershipStatus = OwnershipStatusLabel.CoOwner,
			DpeLabel = Labels.DpeE
		};

		//Act
		var isAccompanyingFileValidatedForCeeProgram = viewModel.IsAccompanyingFileValidatedForCeeProgram;

		//Assert
		isAccompanyingFileValidatedForCeeProgram.Should().BeTrue();
	}
}