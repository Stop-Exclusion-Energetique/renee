using Blazored.Modal;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.Modal;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Layout.SynthesisLayoutManager;
using Renee.UI;

namespace UITests.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Synthesis;

public class OrganizeAndFinanceSynthesisTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IFileService>());
		ctx.Services.AddSingleton(A.Fake<IConfiguration>());
		ctx.Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());

		var mockNavigationHistoryManager = A.Fake<NavigationHistoryManager>();
		mockNavigationHistoryManager.PreviousUri = Endpoints.OrganizeAndFinanceStage + $"{Guid.NewGuid()}";
		ctx.Services.AddSingleton(mockNavigationHistoryManager);

		ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var accompanyingFileService = A.Fake<IAccompanyingFileService>();

		A.CallTo(() => accompanyingFileService.GetOrganizeAndFinanceSynthesis(A<Guid>._))
			.Returns(ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Success(
				new GetOrganizeAndFinanceSynthesisQueryObjectResult { Stage = Renee.Domain.Enums.AccompanyingFileStage.OrganizingAndFinancing, Status = Renee.Domain.Enums.AccompanyingFileStatus.InProgress }));

		ctx.Services.AddSingleton(accompanyingFileService);
		var authContext = ctx.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);

		return ctx;
	}

	[Fact]
	public void SubmitButton_WhenNotUploadFile()
	{
		//Arrange
		var ctx = SetupContext();

		var cut = ctx.Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid()));

		//Act
		var toggles = cut.FindComponents<ZeeToggle>();
		toggles[0].Find("input").Change(true);

		//Assert
		var modalService = ctx.Services.GetRequiredService<IModalService>();
		A.CallTo(
			() => modalService.Show<OrganizeAndFinanceSynthesisValidationModal>(
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustNotHaveHappened();
	}

	[Fact]
	public void SubmitButton_WhenTogglesAreFalse_ShouldHaveDisabledAttribute()
	{
		//Arrange
		var ctx = SetupContext();

		//Act
		var cut = ctx.Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid()));

		var submitButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Soumettre");

		//Assert
		submitButton?.HasAttribute("disabled").Should().BeTrue();
	}

	[Fact]
	public void SubmitButton_WhenTogglesAreTrue_ShouldNotHaveDisabledAttribute()
	{
		// Arrange
		var ctx = SetupContext();

		var cut = ctx.Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid()));

		// Act
		var toggles = cut.FindComponents<ZeeToggle>();
		toggles[0].Find("input").Change(true);

		// Assert
		var submitButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Soumettre");
		submitButton?.HasAttribute("disabled").Should().BeFalse();
	}

	[Fact]
	public void SubmitButton_WhenUploadFileAreTrue()
	{
		//Arrange
		var ctx = SetupContext();

		var cut = ctx.Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid()));

		var toggles = cut.FindComponents<ZeeToggle>();
		toggles[0].Find("input").Change(true);

		var browserFile = A.Fake<IBrowserFile>();
		var filesToUpload = new InputFileChangeEventArgs(new[] { browserFile });

		//Act
		var submitButton = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Soumettre");
		submitButton?.Click();

		//Assert
		var modalService = ctx.Services.GetRequiredService<IModalService>();
		A.CallTo(
			() => modalService.Show<OrganizeAndFinanceSynthesisValidationModal>(
				A<ModalParameters>.Ignored,
				A<ModalOptions>.Ignored)).MustHaveHappened();
	}
}