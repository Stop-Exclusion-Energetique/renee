using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectEnd;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectEnd.ViewModel;

namespace UITests.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details;

public class ProjectEndTests
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();

		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");

		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());

		return ctx;
	}

	[Fact]
	public void ProjectEnd_When_EndOfAccompanyingDateIsBeforeStartOfAccompanyingDate_AccompanyingTimeShouldBeNull()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new ProjectEndViewModel
		{
			StartOfAccompanyingDate = new DateTime(1998, 01, 01, 0, 0, 0, DateTimeKind.Utc),
			EndOfAccompanyingDate = new DateTime(1995, 01, 01, 0, 0, 0, DateTimeKind.Utc)
		};
		var editContext = new EditContext(viewModel);

		ctx.Render<ProjectEnd>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Assert
		viewModel.AccompanyingTime.Should().Be(null);
	}

	[Fact]
	public void
		ProjectEnd_When_EndOfAccompanyingDateIsLaterThanStartOfAccompanyingDate_AccompanyingTimeShouldHaveTheRightValue()
	{
		// Arrange
		var ctx = SetupContext();
		var viewModel = new ProjectEndViewModel
		{
			StartOfAccompanyingDate = new DateTime(1990, 01, 01, 0, 0, 0, DateTimeKind.Utc),
			EndOfAccompanyingDate = new DateTime(1991, 01, 01, 0, 0, 0, DateTimeKind.Utc)
		};
		var editContext = new EditContext(viewModel);

		ctx.Render<ProjectEnd>(
			parameters => parameters.Add(p => p.ViewModel, viewModel).AddCascadingValue(editContext));

		// Assert
		viewModel.AccompanyingTime.Should().Be("12");
	}
}