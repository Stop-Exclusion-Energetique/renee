using System.Security.Claims;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Domain;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.ProfileManagement;

namespace UITests.Components.Features.ProfileManagement;

public class ProfileManagementFormTests
{
	private static BunitContext SetupContext(string role)
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IUserService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddSingleton(A.Fake<IImpersonateService>());

		var authContext = ctx.AddAuthorization();
		authContext.SetAuthorized("testuser@sqli.com");
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()), new(ClaimTypes.Role, role)
		};
		authContext.SetClaims(claims);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void OnRender_WhenUserIsAdmin_DeleteAccountButtonShouldNotBeDisplayed()
	{
		// Arrange
		var ctx = SetupContext(Constants.AdminRole);
		var cut = ctx.Render<ProfileManagementForm>();

		// Act
		var deleteButton = cut.FindComponents<ZeeButton>().FirstOrDefault(c => c.Instance.Text == Labels.DeleteAccount);

		// Assert
		deleteButton.Should().BeNull();
	}

	[Fact]
	public void OnRender_WhenUserIsNotAdmin_DeleteAccountButtonShouldBeDisplayed()
	{
		// Arrange
		var ctx = SetupContext(Constants.SolidarBuilderRole);

		var cut = ctx.Render<ProfileManagementForm>();

		// Act
		var deleteButton = cut.FindComponents<ZeeButton>().FirstOrDefault(c => c.Instance.Text == Labels.DeleteAccount);

		// Assert
		deleteButton.Should().NotBeNull();
	}
}