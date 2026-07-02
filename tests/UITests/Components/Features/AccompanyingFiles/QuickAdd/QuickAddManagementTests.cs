using Blazored.Modal;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.Occupant;
using Renee.Application.DTOs.Territory;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.QuickAdd;
using Renee.UI.Components.Features.AccompanyingFiles.QuickAdd.ViewModels;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.QuickAdd;

public class QuickAddManagementTests
{
	private static void FillAllFields(IRenderedComponent<QuickAddForm> cut)
	{
		var vm = cut.Instance.QuickAddFormViewModel;

		vm.Should().NotBeNull();

		vm.Trigram = "NIC";
		vm.FirstName = "Nicolas";
		vm.LastName = "Dupont";
		vm.Email = "testuser@sqli.com";
		vm.PhoneNumber = "+33 9.99.99.99.99";
		vm.ReferentSolidarBuilderId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc");
		vm.ReferentEtId = Guid.Parse("a9088d7f-b021-45bc-b991-ea35d9cb2c67");
		vm.StreetNumberName = new AddressDto
		{
			AdditionalAddress = "",
			City = "Rouen",
			Department = "Seine-Maritime",
			Label = "1 rue du gros horloge",
			Name = "test",
			PostalCode = "76000",
			Region = "Haute-Normandie",
			Type = "housenumber"
		};
		vm.Typology = GeographicalHousingAreaTypology.Rural;
		vm.AccompaniementViewModel = new QuickAddAccompaniementViewModel
		{
			TrustedTierStructureName = "structure",
			TrustedTierLastName = "toto",
			TrustedTierFirstName = "titi",
			TrustedTierPhoneNumber = "+33 9.99.99.99.98",
			MarkerNature = MarkerNature.Association
		};
		vm.ZeroEnergyExclusionTerritoriesProgram = true;
		vm.AccompanyingType = AccompanyingType.Diffuse;
		vm.ReferentDiffuseCoordinator = Guid.NewGuid();
	}

	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<IFinancialAidService>());
		ctx.Services.AddSingleton(A.Fake<IDifficultyFacedByFamilyService>());
		ctx.Services.AddSingleton(A.Fake<IHouseholdResourcesTypologyService>());
		ctx.Services.AddSingleton(A.Fake<IAddressService>());
		ctx.Services.AddSingleton(A.Fake<IAccompanyingFileService>());
		ctx.Services.AddSingleton(A.Fake<ICopropertyProfileService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<GetQuickAddChoiceDataQuery>._)).Returns(
			ReneeOperationResult<GetQuickAddChoiceDataQueryObjectResult>.Success(
			new GetQuickAddChoiceDataQueryObjectResult
			{
				TerritorialBuilders = new List<QuickAddUserResult> { new("test ET", Guid.NewGuid()) },
				SolidarBuilders = new List<QuickAddUserResult> { new("testuser", Guid.NewGuid()) },
				DiffuseCoordinators = new List<QuickAddUserResult> { new("test Diffuse", Guid.NewGuid()) },
				TargetCoordinators = new List<QuickAddUserResult> { new("test Solidar", Guid.NewGuid()) },
				Territories = new List<TerritoryQueryObjectResult> { new("Territory 1", Guid.NewGuid()) }
			}));
		ctx.Services.AddSingleton(mockSendEventQuery);

		var userGuid = Guid.NewGuid();
		var mockMainOccupantService = A.Fake<IMainOccupantService>();

		A.CallTo(() => mockMainOccupantService.GetAllMainOccupantAsync()).Returns(
			ReneeOperationResult<IEnumerable<MainOccupantDto>>.Success(
			new List<MainOccupantDto>
			{
				new()
				{
					Id = Guid.NewGuid(), Email = "duplicateaddress@sqli.com", PhoneNumber = "+33 7.77.77.77.77"
				}
			}));

		ctx.Services.AddSingleton(mockMainOccupantService);

		var authContext = ctx.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void IfEmailAlreadyExisting_SubmitButtonShouldOpenErrorPopup()
	{
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		var vm = cut.Instance.QuickAddFormViewModel;
		vm.Should().NotBeNull();

		vm.Email = "duplicateaddress@sqli.com";

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Should().NotBeNull();

		submitButton.Click();
		A.CallTo(
			() => modalService.Show<ZeeErrorModal>(
				A<string>.Ignored,
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappened();
	}

	[Fact]
	public void IfMandatoryFieldsAreNotFilled_SubmitButtonShouldNotRedirect()
	{
		using var ctx = SetupContext();
		ctx.Services.AddSingleton(A.Fake<IModalService>());

		var cut = ctx.Render<QuickAddForm>();

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Should().NotBeNull();

		submitButton.Click();

		var validationMessages = cut.FindAll(".validation-message");

		validationMessages.Should().NotBeNull();
		validationMessages.Should().NotBeEmpty();

		var expectedValidationMessages = new List<string>
		{
			Labels.Errors.RequiredStreetNumberNameAddressInput,
			Labels.Errors.RequiredPostalCodeAddressInput,
			Labels.Errors.RequiredMunicipalityAddressInput,
			Labels.Errors.RequiredDepartmentAddressInput,
			Labels.Errors.RequiredRegionAddressInput,
			Labels.Errors.RequiredHousingTypologyInput,
			Labels.Errors.RequiredMarkerNature,
			Labels.Errors.RequiredZeroEnergyExclusionTerritoriesProgram
		};

		var exists = expectedValidationMessages.TrueForAll(
			msg => validationMessages.Any(message => message.TextContent.Trim() == msg));
		exists.Should().BeTrue();
	}

	[Fact]
	public void IfPhoneNumberAlreadyExisting_SubmitButtonShouldOpenErrorPopup()
	{
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		var vm = cut.Instance.QuickAddFormViewModel;
		vm.Should().NotBeNull();

		vm.PhoneNumber = "+33 7.77.77.77.77";

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Should().NotBeNull();

		submitButton.Click();
		A.CallTo(
			() => modalService.Show<ZeeErrorModal>(
				A<string>.Ignored,
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappened();
	}

	[Fact]
	public void InQuickAddForm_WhenMarkerNatureSelectedIsOther_NewFieldShouldAppear()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Instance.QuickAddFormViewModel.AccompaniementViewModel.MarkerNature = MarkerNature.Other;
		cut.Render();

		var commentOnMarkerNature = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == MarkerNatureLabels.Other);

		//Assert
		commentOnMarkerNature.Should().NotBeNull();
	}

	[Fact]
	public void InQuickAddForm_WhenZeroEnergyExclusionTerritoriesProgramIsFalse_AccompanyingTypeFieldShouldNotAppear()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Instance.QuickAddFormViewModel.ZeroEnergyExclusionTerritoriesProgram = false;
		cut.Render();
		var accompanyingTypeField = cut.FindComponents<ZeeSelect<AccompanyingType?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("AccompanyingType"));

		//Assert
		accompanyingTypeField.Should().BeNull();
	}

	[Fact]
	public void
		InQuickAddForm_WhenZeroEnergyExclusionTerritoriesProgramIsFalseAndAccompanyingTypeIsNull_ErrorMessageShouldNotBeDisplayed()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Instance.QuickAddFormViewModel.ZeroEnergyExclusionTerritoriesProgram = false;
		cut.Instance.QuickAddFormViewModel.AccompanyingType = null;
		cut.Render();

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Click();
		var validationMessage = cut.FindAll(".validation-message")
			.FirstOrDefault(e => e.TextContent == Labels.Errors.RequiredAccompanyingType);

		//Assert
		validationMessage.Should().BeNull();
	}

	[Fact]
	public void InQuickAddForm_WhenZeroEnergyExclusionTerritoriesProgramIsTrue_AccompanyingTypeFieldShouldAppear()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Render();
		var accompanyingTypeField = cut.FindComponents<ZeeSelect<AccompanyingType?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("AccompanyingType"));

		//Assert
		accompanyingTypeField.Should().NotBeNull();
	}

	[Fact]
	public void
		InQuickAddForm_WhenZeroEnergyExclusionTerritoriesProgramIsTrueAndAccompanyingTypeIsNull_ErrorMessageShouldBeDisplayed()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Instance.QuickAddFormViewModel.AccompanyingType = null;
		cut.Render();

		var submitButton = cut.Find("button[type='submit']");
		submitButton.Click();
		var validationMessage = cut.FindAll(".validation-message")
			.FirstOrDefault(e => e.TextContent == Labels.Errors.RequiredAccompanyingType);

		//Assert
		validationMessage.Should().NotBeNull();
	}


	[Fact]
	public void QuickAdd_WhenAccompanyingTypeIsDiffuse_DiffuseFieldShouldBeDisplayed()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Render();
		var referentCoordinatorField = cut.FindComponents<ZeeSelect<Guid?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("DiffuseCoordiator"));

		//Assert
		referentCoordinatorField.Should().NotBeNull();
	}

	[Fact]
	public void QuickAdd_WhenAccompanyinTypeIsTargeted_TargetedFieldShouldBeDisplayed()
	{
		//Arrange
		using var ctx = SetupContext();
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<QuickAddForm>();

		FillAllFields(cut);

		//Act
		cut.Instance.QuickAddFormViewModel.AccompanyingType = AccompanyingType.Targeted;
		cut.Render();
		var referentCoordinatorField = cut.FindComponents<ZeeSelect<Guid?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("TargetCoordinator"));
		var territoryField = cut.FindComponents<ZeeSelect<Guid?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("Territory"));
		var territorialBuilderField = cut.FindComponents<ZeeSelect<Guid?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("referent-et-"));

		//Assert
		referentCoordinatorField.Should().NotBeNull();
		territoryField.Should().NotBeNull();
		territorialBuilderField.Should().NotBeNull();
	}
}