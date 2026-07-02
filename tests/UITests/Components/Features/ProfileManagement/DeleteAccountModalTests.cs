using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.UI.Components.Features.ProfileManagement.Modal;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;

namespace UITests.Components.Features.ProfileManagement;

public class DeleteAccountModalTests
{
	[Fact]
	public void Modal_WhenOneAnswerIsSelectedForTheFirstQuestion_AnswersOfTheSecondQuestionShouldNotBeDisabled()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx.Render<DeleteAccountModal>();

		// Act
		cut.Instance.ViewModel.AreAccompanyingFileAttribuated = true;
		cut.Render();

		var component = cut.FindComponents<ZeeSelect<bool?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("account-deletion-confirmed"));
		
        // Assert
        component?.Instance.IsDisabled.Should().BeFalse();
	}
	
	[Fact]
	public void Modal_WhenNoAnswerIsSelectedForTheFirstQuestion_AnswersOfTheSecondQuestionShouldBeDisabled()
	{
		// Arrange
		var ctx = SetupContext();
		var cut = ctx.Render<DeleteAccountModal>();

		// Act
		var component = cut.FindComponents<ZeeSelect<bool?>>().FirstOrDefault(
			c => c.Instance.ElementName != null && c.Instance.ElementName.StartsWith("account-deletion-confirmed"));
		

        // Assert
        component?.Instance.IsDisabled.Should().BeTrue();
	}

	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IUserService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
			new(ClaimTypes.Role, Constants.SolidarBuilderRole)
		};
		authContext.SetClaims(claims);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}
}