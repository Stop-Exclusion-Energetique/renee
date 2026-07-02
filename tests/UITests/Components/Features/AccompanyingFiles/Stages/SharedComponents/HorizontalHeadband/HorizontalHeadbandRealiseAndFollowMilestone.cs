using System.Security.Claims;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Layout.StageLayout;

namespace UITests.Components.Features.AccompanyingFiles.Stages.SharedComponents.HorizontalHeadband;

public class HorizontalHeadbandRealiseAndFollowMilestone
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();

		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();

		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		var mockQuerySender = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockQuerySender.Send(A<GetAccompanyingFileForRealiseAndFollowMilestoneQuery>._)).Returns(
			ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult>.Success(
				new GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult
				{
					Reference = "Michael SCOTT-NDU-75018-25/04/2024"
				}
			)
		);
		A.CallTo(() => mockQuerySender.Send(A<GetAccompanyingFileForHeadBandQuery>._)).Returns(
			ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>.Success(
			new GetAccompanyingFileForHeadBandQueryObjectResult
			{
				AccompanyingFileReference = "Michael SCOTT-NDU-75018-25/04/2024",
				AccompanyingFileStage = AccompanyingFileStage.Identify
			}));
		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		ctx.Services.AddSingleton(mockAccompanyingFileService);
		ctx.Services.AddSingleton(mockQuerySender);

		var stageStateService = A.Fake<IStageNavigationStateService>();
		A.CallTo(() => stageStateService.GetAccompanyingFileStageNavigation()).Returns(
			new List<StageNavigationModel>
			{
				new()
				{
					Url = Endpoints.NewOccupant, Stage = AccompanyingFileStage.Identify, IsCurrentPage = false
				},
				new()
				{
					Url = Endpoints.NewOccupant,
					Stage = AccompanyingFileStage.OrganizingAndFinancing,
					IsCurrentPage = false
				},
				new()
				{
					Url = Endpoints.NewOccupant,
					Stage = AccompanyingFileStage.RealisationAndFollowing,
					IsCurrentPage = true
				}
			});
		A.CallTo(() => stageStateService.AssociatedResourceReference).Returns("Michael SCOTT-NDU-75018-25/04/2024");
		ctx.Services.AddSingleton(stageStateService);

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
	public void OnRender_Milestone_ShouldBeCorrect()
	{
		// Arrange
		var ctx = SetupContext();
		var navManager = ctx.Services.GetRequiredService<NavigationManager>();

		navManager.NavigateTo($"{navManager.BaseUri}realiseAndFollow/ac67eed2-eb14-4015-b483-8cefefc21f95");

		var cut = ctx.Render<StageLayout>();

		// Act
		var milestone = cut.Find("span.rz-dropdown-label.rz-inputtext");

		// Assert
		milestone.TextContent.Contains(AccompanyingFileStage.RealisationAndFollowing.GetDescription()).Should()
			.BeTrue();
	}

	[Fact]
	public void OnRender_Reference_ShouldBeCorrect()
	{
		// Arrange
		var ctx = SetupContext();
		var navManager = ctx.Services.GetRequiredService<NavigationManager>();

		navManager.NavigateTo($"{navManager.BaseUri}realiseAndFollow/ac67eed2-eb14-4015-b483-8cefefc21f95");

		var cut = ctx.Render<StageLayout>();

		// Act
		var reference = cut.Find("h3.font-weight-bold");

		// Assert
		reference.TextContent.Equals("Michael SCOTT-NDU-75018-25/04/2024").Should().BeTrue();
	}
}