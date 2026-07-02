using Blazored.Modal;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.Modal;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details;

public class AccompanyingFileSubmitTests
{
	private static BunitContext SetupContext(GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult queryObjectResult)
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());

		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<AuthenticationStateProvider>());
		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();
		ctx.Services.AddSingleton(A.Fake<IStageNavigationStateService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<ISendEventQuery>());
		ctx.Services.AddScoped<UnsavedChangesGuard>();
		ctx.Services.AddBlazorContextMenu();

        var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileForOrganizeAndFinanceMilestone(A<Guid>._, A<Guid>._, A<string>._)).Returns(ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>.Success(queryObjectResult));
		var userGuid = Guid.NewGuid();
		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var authContext = ctx.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, userGuid.ToString()),
		};

		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void OnSubmit_DifferentialBetweenWorkPackagesAndPreFinancingPlanTotalIsNotEqualsToZero_ShouldDisplayFinancingDifferentialModal()
	{
		// Arrange
		var queryObjectResult = new GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult
		{
			Stage = AccompanyingFileStage.OrganizingAndFinancing,
			RenovationType = RenovationType.MajorRenovation,
			EstimatedAnnualEnergyConsumptionAfterWork = 500,
			EstimatedEnergyDpeAfterWork = DpeLabel.E,
			EstimatedEnergyGesAfterWork = GesLabel.F,
			EstimatedEnergyClassJump = 3,
			TreatedAirTightness = PartlyStateTreatment.Yes,
			TreatedThermalBridge = PartlyStateTreatment.Partly,
			AreExistingHumidityAndVaporMigrationManagedAfterTreatment = PartlyStateTreatment.Yes,
			IsRgeLabelUpToDate = true,
			AccompanyingTimeDuration = AccompanyingTimeDuration.LessThanTwoHours,
			CoOwnershipBonus = 100,
			GuidedPathwayBonus = 0,
			DecentHousingBonus = 0,
			Region = 0,
			Department = 0,
			PublicEstablishmentsIntercommunalCooperation = 0,
			Municipality = 0,
			EnergySavingCertificates = 0,
			UnderprivilegedHousingFoundation = 10000,
			SocialProtectionGroup = 0,
			HouseholdMaximumSavingAmountForRenovationProject = 0,
			MaximumAmountSupportFamilyMembersRenovationProject = 0,
			AdaptationBonus = 0,
			BankLoanType = "Type prêt bancaire",
			AnahFolderFilingDate = DateTime.Now,
			AnahFolderNumber = "Numéro dossier ANAH",
			PreWorkPlanProjectTypes =
			[
				Guid.NewGuid()
			],
			WorkPackages =
			[
				new(
					Guid.NewGuid(),
					null,
					[
						new(Guid.NewGuid(), 1000, null),
						new(Guid.NewGuid(), 3000, null)
					]),
				new(
					Guid.NewGuid(),
					null,
					[
						new(Guid.NewGuid(), 1500, null),
						new(Guid.NewGuid(), 2000, null)
					])
			],
			InitialDpeLabel = DpeLabel.D,
			PensionFund = 0
		};

		var ctx = SetupContext(queryObjectResult);
		var cut = ctx.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.OrganizeAndFinance>();
		var modalService = ctx.Services.GetRequiredService<IModalService>();

		var fakeModalReference = A.Fake<IModalReference>();
		A.CallTo(() => fakeModalReference.Result)
			.Returns(Task.FromResult(ModalResult.Ok(true)));

		A.CallTo(() => modalService.Show<FinancingDifferentialModal>(
			A<ModalParameters>.Ignored,
			A<ModalOptions>.Ignored)).Returns(fakeModalReference);

		// Act
		cut.InvokeAsync(async () => await cut.Instance.ChangeTab(3));

		var submitButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Soumettre");
		submitButton?.Click();

		// Assert
		A.CallTo(
			() => modalService.Show<FinancingDifferentialModal>(
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappenedOnceExactly();
	}

	[Fact]
	public void OnSubmit_DifferentialBetweenWorkPackagesAndPreFinancingPlanTotalIsEqualsToZero_ShouldNotDisplayFinancingDifferentialModal()
	{
		// Arrange
		var queryObjectResult = new GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult
		{
			Stage = Renee.Domain.Enums.AccompanyingFileStage.OrganizingAndFinancing,
			RenovationType = Renee.Domain.Enums.RenovationType.MajorRenovation,
			EstimatedAnnualEnergyConsumptionAfterWork = 500,
			EstimatedEnergyDpeAfterWork = Renee.Domain.Enums.DpeLabel.E,
			EstimatedEnergyGesAfterWork = Renee.Domain.Enums.GesLabel.F,
			EstimatedEnergyClassJump = 3,
			TreatedAirTightness = Renee.Domain.Enums.PartlyStateTreatment.Yes,
			TreatedThermalBridge = Renee.Domain.Enums.PartlyStateTreatment.Partly,
			AreExistingHumidityAndVaporMigrationManagedAfterTreatment = Renee.Domain.Enums.PartlyStateTreatment.Yes,
			IsRgeLabelUpToDate = true,
			AccompanyingTimeDuration = Renee.Domain.Enums.AccompanyingTimeDuration.LessThanTwoHours,
			CoOwnershipBonus = 0,
			GuidedPathwayBonus = 0,
			DecentHousingBonus = 0,
			Region = 0,
			Department = 0,
			PublicEstablishmentsIntercommunalCooperation = 0,
			Municipality = 0,
			EnergySavingCertificates = 0,
			UnderprivilegedHousingFoundation = 0,
			SocialProtectionGroup = 0,
			HouseholdMaximumSavingAmountForRenovationProject = 0,
			MaximumAmountSupportFamilyMembersRenovationProject = 0,
			AdaptationBonus = 0,
			BankLoanType = "Type prêt bancaire",
			PreWorkPlanProjectTypes =
			[
				Guid.NewGuid()
			],
			WorkPackages =
			[
				new(
					Guid.NewGuid(),
					null,
					[
						new(Guid.NewGuid(), 1000, null),
						new(Guid.NewGuid(), 3000, null)
					]),
				new(
					Guid.NewGuid(),
					null,
					[
						new(Guid.NewGuid(), 1500, null),
						new(Guid.NewGuid(), 2000, null)
					])
			],
			PensionFund = 7500
		};

		var ctx = SetupContext(queryObjectResult);
		var cut = ctx.Render<Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.OrganizeAndFinance>();
		var modalService = ctx.Services.GetRequiredService<IModalService>();

		var fakeModalReference = A.Fake<IModalReference>();
		A.CallTo(() => fakeModalReference.Result)
			.Returns(Task.FromResult(ModalResult.Ok(true)));

		A.CallTo(() => modalService.Show<FinancingDifferentialModal>(
			A<ModalParameters>.Ignored,
			A<ModalOptions>.Ignored)).Returns(fakeModalReference);

		// Act
		cut.InvokeAsync(async () => await cut.Instance.ChangeTab(3));

		var submitButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Soumettre");
		submitButton?.Click();

		// Assert
		A.CallTo(() => modalService.Show<FinancingDifferentialModal>(
			A<ModalParameters>.That.Matches(p =>
				p.Get<decimal>("Differential") == 8500),
			A<ModalOptions>.Ignored)).MustNotHaveHappened();
	}
}