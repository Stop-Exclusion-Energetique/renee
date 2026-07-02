using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.User;
using Renee.Application.Handlers.CommandHandlers.User;
using Renee.Application.Interfaces;

namespace CommandHandlerTests.User;

public class UpdateUserCommandHandlerTests : TestContext
{
	private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenUserDtoIsInvalid()
	{
        //Arrange
        Services.AddApplicationInsightsTelemetry();
        Services.AddSingleton(A.Fake<ITelemetryService>());

        var command = new UpdateUserCommandInput(
			new UserDto(Guid.NewGuid(), DateTime.UtcNow, "LastName", "FirstName", string.Empty));

        var mockTelemetry = Services.GetRequiredService<ITelemetryService>();
		var handler = new UpdateUserCommandHandler(_userRepository, mockTelemetry);

		//Act
		var result = await handler.Handle(command, default);

		//Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public async Task Handler_ShouldNot_CallUpdateUserADOnRepository_WhenUserDtoIsInvalid()
	{
		//Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var hostEnvironment = Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.SetEnvironmentToDevelopment();

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		var command = new UpdateAzureUserCommand(
			new UserDto(Guid.NewGuid(), DateTime.UtcNow, "LastName", "FirstName", string.Empty),
			string.Empty);

		var handler = new UpdateAzureUserCommandHandler(_userRepository, mockTelemetry);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _userRepository.UpdateUserInAzureAd(A<Renee.Domain.Entity.User>._, string.Empty))
			.MustNotHaveHappened();
	}

	[Fact]
	public async Task Handler_ShouldNot_CallUpdateUserOnRepository_WhenUserDtoIsInvalid()
	{
        //Arrange
        Services.AddApplicationInsightsTelemetry();
        Services.AddSingleton(A.Fake<ITelemetryService>());

        var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

        var command = new UpdateUserCommandInput(
			new UserDto(
				Guid.NewGuid(),
				new UserPersonnalInformations(
					"test@test.com",
					"FirstName",
					"LastName",
					"07.89.16.33.10", 
					string.Empty),
					Guid.NewGuid(), null));

		var handler = new UpdateUserCommandHandler(_userRepository, mockTelemetry);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _userRepository.UpdateUserAsync(A<Renee.Domain.Entity.User>._)).MustNotHaveHappened();
	}
}