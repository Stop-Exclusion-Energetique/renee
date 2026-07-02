using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Domain.ReneeError;
using Renee.UI.Components.CGUHandling;

namespace UITests.Components.CGUHandling;

public class CGUInitializerTests
{
    [Fact]
    public async Task InitializeAsync_ShouldSetContextProperties()
    {
        var authProvider = A.Fake<AuthenticationStateProvider>();
        var cguValidationService = A.Fake<ICguValidationService>();

        var userId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));
        A.CallTo(() => authProvider.GetAuthenticationStateAsync()).Returns(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        A.CallTo(() => cguValidationService.InitializeCGUVerificationAsync(userId))
            .Returns(Task.FromResult(
                ReneeOperationResult<UserVersionCguDto>.Success(new UserVersionCguDto
                {
                    LastValidatedCGUVersionByUser = "v1",
                    LastCGUVersion = "v2",
                    LastCGULabel = "CGU_v2.pdf"
                })));


        var initializer = new CguInitializer(authProvider, cguValidationService);
        await initializer.InitializeAsync(userId);

        initializer.Context.IsInitialized.Should().BeTrue();
        initializer.Context.UserVersion.Should().Be("v1");
        initializer.Context.LatestVersion.Should().Be("v2");
        initializer.Context.LatestVersionLabelFile.Should().Be("CGU_v2.pdf");
        initializer.Context.UserId.Should().Be(userId);

    }

    [Fact]
    public async Task MarkCGUAsAcceptedAsync_WithValidContext_CallsServiceAndUpdatesContext()
    {
        // Arrange
        var cguValidationService = A.Fake<ICguValidationService>();
        var mockAuthProvider = A.Fake<AuthenticationStateProvider>();

        var initializer = new CguInitializer(mockAuthProvider, cguValidationService);
        initializer.Context.UserId = Guid.NewGuid();
        initializer.Context.LatestVersion = "v2";
        initializer.Context.UserVersion = "v1";
        initializer.Context.IsInitialized = false;

        // Act
        await initializer.MarkCGUAsAcceptedAsync();

        // Assert
        A.CallTo(() => cguValidationService.MarkCGUAsAcceptedAsync(initializer.Context.UserId, "v2"))
            .MustHaveHappenedOnceExactly();
        initializer.Context.UserVersion.Should().Be("v2");
        initializer.Context.IsInitialized.Should().BeTrue();
    }

    [Fact]
    public async Task MarkCGUAsAcceptedAsync_WithEmptyUserId_DoesNotCallService()
    {
        // Arrange
        var cguValidationService = A.Fake<ICguValidationService>();
        var mockAuthProvider = A.Fake<AuthenticationStateProvider>();
        var initializer = new CguInitializer(mockAuthProvider, cguValidationService);
        initializer.Context.UserId = Guid.Empty;
        initializer.Context.LatestVersion = "v2";

        // Act
        await initializer.MarkCGUAsAcceptedAsync();

        // Assert
        A.CallTo(() => cguValidationService.MarkCGUAsAcceptedAsync(A<Guid>._, A<string>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task MarkCGUAsAcceptedAsync_WithNullLatestVersion_DoesNotCallService()
    {
        // Arrange
        var cguValidationService = A.Fake<ICguValidationService>();
        var mockAuthProvider = A.Fake<AuthenticationStateProvider>();
        var initializer = new CguInitializer(mockAuthProvider, cguValidationService);
        initializer.Context.UserId = Guid.NewGuid();
        initializer.Context.LatestVersion = null;

        // Act
        await initializer.MarkCGUAsAcceptedAsync();

        // Assert
        A.CallTo(() => cguValidationService.MarkCGUAsAcceptedAsync(A<Guid>._, A<string>._)).MustNotHaveHappened();
    }
}