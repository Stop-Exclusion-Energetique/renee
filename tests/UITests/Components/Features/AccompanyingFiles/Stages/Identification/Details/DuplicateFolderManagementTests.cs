using System.Globalization;
using System.Security.Claims;
using Blazored.Modal;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.BasePage;
using Renee.UI.Components.FormComponents;
using Renee.UI.Components.Layout.StageLayout;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class DuplicateFolderManagementTests
{
	private static BunitContext SetupContext(Guid id)
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<IFinancialAidService>());
		ctx.Services.AddSingleton(A.Fake<IDifficultyFacedByFamilyService>());
		ctx.Services.AddSingleton(A.Fake<IHouseholdResourcesTypologyService>());
		ctx.Services.AddSingleton(A.Fake<IAddressService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<ISendEventQuery>());
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();

        var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileById(A<Guid>._, A<Guid>._, A<string>._)).Returns(
			ReneeOperationResult<AccompanyingFileDto?>.Success(
				new AccompanyingFileDto
				{
					Id = id,
					Reference = "ref",
					CreationDatetimeUtc = DateTime.Now,
					LastUpdateDatetimeUtc = DateTime.Now,
					ClosedDatetimeUtc = null,
					Housing = new HousingDto
					{
						Id = Guid.NewGuid(),
						Address = new AddressDto
						{
							Id = Guid.NewGuid(),
							AdditionalAddress = "",
							City = "Rouen",
							Department = "Seine-Maritime",
							Label = "1 rue du gros horloge",
							Name = "test",
							PostalCode = "76000",
							Region = "Haute-Normandie",
							Type = "housenumber"
						},
						// ReSharper disable once StringLiteralTypo
						GeographicalTypology = GeographicalHousingAreaTypology.Rural,
						DegradationIndex = DegradationIndex.Low,
						UnsanitaryCoefficient = UnsanitaryCoefficient.High
					},
					MainOccupant =
						new MainOccupantDto
						{
							Id = Guid.NewGuid(),
							Email = "duplicateaddress@sqli.com",
							PhoneNumber = "+33 7.77.77.77.77",
							PensionFund = PensionFund.Cnav,
							SocialWelfareFund = SocialProtectionFund.Cgss,
							Gender = "Mr",
							LastName = "test",
							FirstName = "test",
							Age = 42,
							Birthdate = DateTime.ParseExact("10/07/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture),
							SocioProfessionalCategoryId = SocioProfessionalCategory.Artisan,
							AccompanyingFileId = Guid.NewGuid(),
							Job = "test"
						},
					EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
					EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
					AnahCategory = "",
					HasDisabilitySituation = false,
					HasPersonFollowedByCuratorship = false,
					HouseholdResourcesTypologies = [],
					DifficultiesFacedByFamily = [],
					HouseholdTypologyId = HouseholdTypology.SinglePerson
				}
			)
		);

		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var mockMainOccupantService = A.Fake<IMainOccupantService>();

		A.CallTo(() => mockMainOccupantService.GetAllMainOccupantAsync()).Returns(
			ReneeOperationResult<IEnumerable<MainOccupantDto>>.Success(
			new List<MainOccupantDto>
			{
				new() { Id = Guid.NewGuid(), Email = "testuser@sqli.com", PhoneNumber = "+33 9.99.99.99.99" }
			}));

		ctx.Services.AddSingleton(mockMainOccupantService);
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());

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
	public void OnDuplicateEmail_ShouldDisableSubmitButton()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var emailComponent = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.EmailOccupant);
		var emailField = emailComponent?.Find("input");
		emailField?.Change("testuser@sqli.com");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				      .Email ==
			      "testuser@sqli.com");

		cut.Find("#tab-4 > a").Click();
		cut.WaitForState(() => cut.Instance.SelectedTab == 3);

		var submitButtonComponent =
			cut.FindComponents<ZeeButton>().FirstOrDefault(c => c.Instance.Text == Labels.Submit);
		var submitButton = submitButtonComponent?.Find("button");
		submitButton?.HasAttribute("disabled").Should().BeTrue();
	}

	[Fact]
	public void OnDuplicateEmail_ShouldRenderPopup()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var emailComponent = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.EmailOccupant);
		var emailField = emailComponent?.Find("input");
		emailField?.Change("testuser@sqli.com");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				      .Email ==
			      "testuser@sqli.com");

		A.CallTo(
			() => modalService.Show<ZeeErrorModal>(
				A<string>.Ignored,
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappened();
	}

	[Fact]
	public void OnDuplicatePhoneNumber_ShouldDisableSubmitButton()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));
		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var phoneComponent = cut.FindComponents<ZeePhoneNumber>()[0];
		var phoneNumberField = phoneComponent?.FindComponent<RadzenMask>();
		var input = phoneNumberField?.Find("input");
		input?.Change("9.99.99.99.99");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				.PhoneNumber!.Contains("9.99.99.99.99"));

		cut.Find("#tab-4 > a").Click();
		cut.WaitForState(() => cut.Instance.SelectedTab == 3);

		var submitButtonComponent =
			cut.FindComponents<ZeeButton>().FirstOrDefault(c => c.Instance.Text == Labels.Submit);
		var submitButton = submitButtonComponent?.Find("button");
		submitButton?.HasAttribute("disabled").Should().BeTrue();
	}

	[Fact]
	public void OnDuplicatePhoneNumber_ShouldRenderPopup()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var phoneComponent = cut.FindComponents<ZeePhoneNumber>()[0];
		var phoneNumberField = phoneComponent?.FindComponent<RadzenMask>();
		var input = phoneNumberField?.Find("input");
		input?.Change("9.99.99.99.99");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				.PhoneNumber!.Contains("9.99.99.99.99"));

		A.CallTo(
			() => modalService.Show<ZeeErrorModal>(
				A<string>.Ignored,
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappened();
	}

	[Fact]
	public void OnNewEmail_ShouldNotRenderPopup()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var emailComponent = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.EmailOccupant);
		var emailField = emailComponent?.Find("input");
		emailField?.Change("testuser1@sqli.com");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				      .Email ==
			      "testuser1@sqli.com");

		A.CallTo(
			() => modalService.Show<ZeeErrorModal>(
				A<string>.Ignored,
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustNotHaveHappened();
	}

	[Fact]
	public void OnNewEmail_SubmitButtonShouldBeEnabled()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));
		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var emailComponent = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.EmailOccupant);
		var emailField = emailComponent!.Find("input");
		emailField.Change("testuser1@sqli.com");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				      .Email ==
			      "testuser1@sqli.com");

		cut.Find("#tab-4 > a").Click();
		cut.WaitForState(() => cut.Instance.SelectedTab == 3);

		var submitButton = cut.Find("button[type='submit']");
		submitButton.HasAttribute("disabled").Should().BeFalse();
	}

	[Fact]
	public void OnNewPhoneNumber_ShouldNotRenderPopup()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);

		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);

		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var phoneComponent = cut.FindComponents<ZeePhoneNumber>()[0];
		var phoneNumberField = phoneComponent?.FindComponent<RadzenMask>();
		phoneNumberField?.Find("input").Change("0.00.00.00.00");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				.PhoneNumber!.Contains("0.00.00.00.00"));

		A.CallTo(
			() => modalService.Show<ZeeErrorModal>(
				A<string>.Ignored,
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustNotHaveHappened();
	}

	[Fact]
	public void OnNewPhoneNumber_SubmitButtonShouldBeEnabled()
	{
		var id = Guid.NewGuid();
		using var ctx = SetupContext(id);
		var modalService = A.Fake<IModalService>();
		ctx.Services.AddSingleton(modalService);
		var cut = ctx.Render<OccupantCreationForm>(
			parameters => parameters.Add(p => p.AccompanyingFileId, id));

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel?.HouseholdIdentityViewModel
				.MainOccupantViewModel is not null);

		var phoneComponent = cut.FindComponents<ZeePhoneNumber>()[0];
		var phoneNumberField = phoneComponent?.FindComponent<RadzenMask>();
		var input = phoneNumberField?.Find("input");
		input?.Change("0.00.00.00.00");

		cut.WaitForState(
			() => cut.Instance.AccompanyingFileCreationFormViewModel!.HouseholdIdentityViewModel.MainOccupantViewModel
				.PhoneNumber!.Contains("0.00.00.00.00"));

		cut.Find("#tab-4 > a").Click();
		cut.WaitForState(() => cut.Instance.SelectedTab == 3);

		var submitButton = cut.Find("button[type='submit']");
		submitButton.HasAttribute("disabled").Should().BeFalse();
	}
}