using Blazored.Modal;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification.Modal;
using Renee.UI.Components.FormComponents;
using Renee.UI.Components.Layout;
using Renee.UI.Components.Layout.SynthesisLayoutManager;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Synthesis;

public class OccupantSynthesisTests
{
	private Guid AccompanyingFileId { get; } = Guid.Parse("330e4303-5c11-43ef-ac77-756009dcaa94");

	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IFileService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());
		ctx.Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");

		authContext.SetRoles("CD");
		return ctx;
	}

	[Fact]
	public void OccupantSynthesis_EmptyFieldsOnPreviousTabs_ShouldDisplayNoValueOnTheseSameFields()
	{
		var ctx = SetupContext();

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		var expectedAccompanyingFileDto = new AccompanyingFileSynthesisDto
		{
			Id = AccompanyingFileId,
			SocioProfessionalCategory = SocioProfessionalCategory.Worker,
			HouseholdTypology = HouseholdTypology.CoupleWithChildren,
			HousingType = HousingType.ResidentialCollective,
			OwnershipStatus = OwnershipStatus.PrivateParkTenant,
			ElectricityDeprivation = EnergyDeprivation.None
		};

		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(AccompanyingFileId))
			.Returns(ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(expectedAccompanyingFileDto));

		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var cut = ctx.Render<IdentificationSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, AccompanyingFileId));

		var synthesisFields = cut.FindAll("input[readonly]");
		synthesisFields.Should().NotBeNull();

		foreach (var field in synthesisFields) field.TextContent.Should().Be(string.Empty);
	}

	[Fact]
	public void OccupantSynthesis_HouseholdDifficultiesCheckedYesOnPreviousTab_ShouldBeDisplayed()
	{
		// Arrange
		var ctx = SetupContext();
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		var expectedAccompanyingFileDto = new AccompanyingFileSynthesisDto
		{
			Id = AccompanyingFileId,
			HasDisabilitySituation = true,
			HasPersonWithLongTermIllness = true,
			HasPersonFollowedByCuratorship = true,
			SocioProfessionalCategory = SocioProfessionalCategory.Worker,
			HouseholdTypology = HouseholdTypology.CoupleWithChildren,
			HousingType = HousingType.ResidentialCollective,
			OwnershipStatus = OwnershipStatus.PrivateParkTenant,
			ElectricityDeprivation = EnergyDeprivation.None
		};

		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(AccompanyingFileId))
			.Returns(ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(expectedAccompanyingFileDto));

		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var cut = ctx.Render<IdentificationSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, AccompanyingFileId));

		// Act
		var householdDifficulties = cut.Instance.OccupantSynthesisViewModel.HouseholdDifficulties;
		List<string> expectedHouseholdDifficulties = ["handicap", "maladie", "curatelle"];

		// Assert
		householdDifficulties.Should().Be(string.Join(", ", expectedHouseholdDifficulties));
	}

	[Fact]
	public void OccupantSynthesis_ThreeOccupantsAddedOnPreviousTab_ShouldDisplayTheRightNumberOfOccupants()
	{
		var ctx = SetupContext();

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		var expectedAccompanyingFileDto = new AccompanyingFileSynthesisDto
		{
			Id = AccompanyingFileId,
			NumberOfOccupants = 3,
			SocioProfessionalCategory = SocioProfessionalCategory.Worker,
			HouseholdTypology = HouseholdTypology.CoupleWithChildren,
			HousingType = HousingType.ResidentialCollective,
			OwnershipStatus = OwnershipStatus.PrivateParkTenant,
			ElectricityDeprivation = EnergyDeprivation.None
		};

		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(AccompanyingFileId))
			.Returns(ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(expectedAccompanyingFileDto));

		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var cut = ctx.Render<IdentificationSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, AccompanyingFileId));

		var numberOfOccupantsField = cut.FindAll("input[readonly]")[7];
		numberOfOccupantsField.Should().NotBeNull();

		numberOfOccupantsField.GetAttribute("value").Should()
			.Be(expectedAccompanyingFileDto.NumberOfOccupants.ToString());
	}

	[Fact]
	public async Task
		OccupantSynthesis_ToggleActive_ShouldDisplaySubmitButtonNotDisabled_And_OneClickOnSubmitButtonShowPopUp()
	{
		//Arrange
		var ctx = SetupContext();
		ctx.JSInterop.SetupVoid("window.appendStar", _ => true);
		
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		var mockFile = A.Fake<IBrowserFile>();
		var expectedAccompanyingFileDto = new AccompanyingFileSynthesisDto
		{
			Id = AccompanyingFileId,
			HasDisabilitySituation = true,
			HasPersonWithLongTermIllness = true,
			HasPersonFollowedByCuratorship = true,
			SocioProfessionalCategory = SocioProfessionalCategory.Worker,
			HouseholdTypology = HouseholdTypology.CoupleWithChildren,
			HousingType = HousingType.ResidentialCollective,
			OwnershipStatus = OwnershipStatus.PrivateParkTenant,
			ElectricityDeprivation = EnergyDeprivation.None
		};

		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(A<Guid>._))
			.Returns(ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(expectedAccompanyingFileDto));

		ctx.Services.AddSingleton(mockAccompanyingFileService);
		var modalService = ctx.Services.GetRequiredService<IModalService>();

		var cut = ctx.Render<IdentificationSynthesis>();

		//Act
		await cut.InvokeAsync(() => cut.Instance.OnChangeUpload(new InputFileChangeEventArgs([mockFile])));

		var toggle = cut.FindComponent<ZeeToggle>();
		toggle.Find("input").Change(true);

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Should().NotBeNull();

		submitButton.Click();

		//Assert
		A.CallTo(
			() => modalService.Show<IdentifySynthesisValidationModal>(
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void OccupantSynthesis_ToggleNotActive_ShouldDisplaySubmitButtonDisabled()
	{
		var ctx = SetupContext();

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();

		var expectedAccompanyingFileDto = new AccompanyingFileSynthesisDto
		{
			Id = AccompanyingFileId,
			HasDisabilitySituation = true,
			HasPersonWithLongTermIllness = true,
			HasPersonFollowedByCuratorship = true,
			SocioProfessionalCategory = SocioProfessionalCategory.Worker,
			HouseholdTypology = HouseholdTypology.CoupleWithChildren,
			HousingType = HousingType.ResidentialCollective,
			OwnershipStatus = OwnershipStatus.PrivateParkTenant,
			ElectricityDeprivation = EnergyDeprivation.None
		};

		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(A<Guid>._))
			.Returns(ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(expectedAccompanyingFileDto));

		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var cut = ctx.Render<IdentificationSynthesis>();

		var toggle = cut.Find(".switch-input");
		toggle.Should().NotBeNull();

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Should().NotBeNull();

		submitButton.HasAttribute("disabled").Should().BeTrue();
	}
}