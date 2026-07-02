using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.Identification;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow;
using Renee.UI.Components.Layout;
using Renee.UI.Components.Layout.SynthesisLayoutManager;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.Synthesis;

public class SynthesisTests : BunitContext
{
	[Fact]
	public void OnClickOrganizeAndFinanceButton_WhenStageIsOrganizeAndFinance_SouldDisplayNotAvailableText()
	{
		// Arrange
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());
		Services.AddSingleton(A.Fake<IFileService>()); 
		Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		Services.AddSingleton(A.Fake<IConfiguration>());

		var authContext = this.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		
		A.CallTo(() => mockAccompanyingFileService.GetOrganizeAndFinanceSynthesis(A<Guid>._)).Returns(
			ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Success(
			new GetOrganizeAndFinanceSynthesisQueryObjectResult
			{
				AccompanyingFileReference = "AZERTY", 
				Stage = AccompanyingFileStage.OrganizingAndFinancing
			}
		));
		Services.AddSingleton(mockAccompanyingFileService);

		var cut = Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid())
		);

		// Act
		var button = cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Organiser et financer");
		button?.Click();
		// Assert
		cut.Find("p").TextContent.Should().Be(Labels.SynthesisNotAvailable);
	}

	[Fact]
	public void OnClickRealizeAndFollowButton_WhenStageIsOrganizeAndFinance_SouldDisplayNotAvailableText()
	{
		// Arrange
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IFileService>());
		Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		Services.AddSingleton(A.Fake<IConfiguration>());
		Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());

		var authContext = this.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetOrganizeAndFinanceSynthesis(A<Guid>._)).Returns(
			ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Success(
			new GetOrganizeAndFinanceSynthesisQueryObjectResult
			{
				AccompanyingFileReference = "AZERTY", Stage = AccompanyingFileStage.OrganizingAndFinancing
			}));
		Services.AddSingleton(mockAccompanyingFileService);

		var cut = Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid())
				.AddCascadingValue(new SynthesisLayout()));

		// Act
		cut.FindAll("button").FirstOrDefault(btn => btn.TextContent == "Réaliser et suivre")?.Click();

		// Assert
		cut.Find("p").TextContent.Should().Be(Labels.SynthesisNotAvailable);
	}

	[Fact]
	public void OnRender_WhenStageIsOrganizeAndFinance_SouldDisplayIdentifySynthesis()
	{
		// Arrange 
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IFileService>());
		Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());


		var authContext = this.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(A<Guid>._)).Returns(
			ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(
			new AccompanyingFileSynthesisDto
			{
				AccompanyingFileReference = "AZERTY", Stage = AccompanyingFileStage.OrganizingAndFinancing
			}));
		Services.AddSingleton(mockAccompanyingFileService);

		var cut = Render<IdentificationSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid())
				.AddCascadingValue(new SynthesisLayout()));

		// Assert
		cut.Find("h1").TextContent.Should().Be(Labels.ValidationHouseholdSynthesis);
	}

	[Fact]
	public void OnRenderIdentificationSynthesis_WhenStageIsOrganizeAndFinance_ShouldDisplaySynthesis()
	{
		// Arrange 
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IFileService>());
		Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());


		var authContext = this.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileSynthesis(A<Guid>._)).Returns(
			ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(
			new AccompanyingFileSynthesisDto
			{
				AccompanyingFileReference = "AZERTY", Stage = AccompanyingFileStage.OrganizingAndFinancing
			}));
		Services.AddSingleton(mockAccompanyingFileService);

		var cut = Render<IdentificationSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid())
				.AddCascadingValue(new SynthesisLayout()));

		// Assert
		cut.Find("h1").TextContent.Should().Be(Labels.ValidationHouseholdSynthesis);
	}

	[Fact]
	public void OnRenderOrganizeAndFinanceSynthesis_WhenStageIsRealiseAndFollow_ShouldDisplaySynthesis()
	{
		// Arrange 
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IFileService>());
		Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());
		Services.AddSingleton(A.Fake<IConfiguration>());

		var authContext = this.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetOrganizeAndFinanceSynthesis(A<Guid>._)).Returns(
			ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Success(
			new GetOrganizeAndFinanceSynthesisQueryObjectResult
			{
				AccompanyingFileReference = "AZERTY", Stage = AccompanyingFileStage.RealisationAndFollowing
			}));
		Services.AddSingleton(mockAccompanyingFileService);

		var cut = Render<OrganizeAndFinanceSynthesis>(
			parameters => parameters.Add(p => p.AccompanyingFileId, Guid.NewGuid())
				.AddCascadingValue(new SynthesisLayout()));

		// Assert
		cut.Find("h1").TextContent.Should().Be(Labels.OrganizeAndFinanceSynthesisTitle);
	}

	[Fact]
	public void OnRenderRealizeAndFollowSynthesis_WhenStageIsRealiseAndFollow_ShouldDisplayNotAvailable()
	{
		// Arrange 
		Services.AddSingleton(A.Fake<IModalService>());
		Services.AddSingleton(A.Fake<NotificationService>());
		Services.AddSingleton(A.Fake<IFileService>());
		Services.AddSingleton(A.Fake<NavigationHistoryManager>());
		Services.AddSingleton(A.Fake<SynthesysLayoutStateManager>());
		Services.AddSingleton(A.Fake<IConfiguration>());

		var authContext = this.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();

		Services.AddSingleton(mockAccompanyingFileService);

		var cut = Render<RealizeAndFollowSynthesis>(
			parameters => parameters.AddCascadingValue(new SynthesisLayout()));

		// Assert
		cut.Find("p").TextContent.Should().Be(Labels.SynthesisNotAvailable);
	}
}