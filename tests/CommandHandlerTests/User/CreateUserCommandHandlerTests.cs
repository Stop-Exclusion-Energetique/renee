using Renee.Application.Commands.User;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.User;
using Renee.Application.Handlers.CommandHandlers.User;
using Renee.Application.Interfaces;

namespace CommandHandlerTests.NewUser;

public class CreateUserCommandHandlerTests
{
	private readonly IUnregisteredUserRepository _unregisteredUserRepository = A.Fake<IUnregisteredUserRepository>();
	private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

	[Fact]
	public async Task Handler_Should_CallAddUserOnRepository_WhenUserDtoIsValid()
	{
		//Arrange
		A.CallTo(() => _unregisteredUserRepository.AddUser(A<UnregisteredUser>._)).Returns(1);

		var command = new CreateUserCommand(
			new UserDto(Guid.NewGuid(), DateTime.UtcNow, "LastName", "FirstName", "test@test.com"));

		command.UserDto.SetDataForSignIn(
			Guid.NewGuid(),
			"Test",
			new ReportingStructureDto(Guid.NewGuid(), "Structure", null),
			null,
			Guid.NewGuid());

		var handler = new CreateUserCommandHandler(_unregisteredUserRepository, _telemetryService);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _unregisteredUserRepository.AddUser(A<UnregisteredUser>._)).MustHaveHappened();
	}

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenUserDtoIsInvalid()
	{
		//Arrange
		var command = new CreateUserCommand(
			new UserDto(Guid.NewGuid(), DateTime.UtcNow, "LastName", "FirstName", "test@test.com"));

		command.UserDto.SetDataForSignIn(
			Guid.NewGuid(),
			"Test",
			new ReportingStructureDto(Guid.Empty, "Structure", null),
			null,
			Guid.NewGuid());

		var handler = new CreateUserCommandHandler(_unregisteredUserRepository, _telemetryService);

		//Act
		var result = await handler.Handle(command, default);

		//Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public async Task Handler_ShouldNot_CallAddUserOnRepository_WhenUserDtoIsInvalid()
	{
		//Arrange
		var command = new CreateUserCommand(
			new UserDto(Guid.NewGuid(), DateTime.UtcNow, "LastName", "FirstName", "test@test.com"));

		command.UserDto.SetDataForSignIn(
			Guid.NewGuid(),
			"Test",
			new ReportingStructureDto(Guid.Empty, "Structure", null),
			null,
			Guid.NewGuid());

		var handler = new CreateUserCommandHandler(_unregisteredUserRepository, _telemetryService);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _unregisteredUserRepository.AddUser(A<UnregisteredUser>._)).MustNotHaveHappened();
	}
}