using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.Coproperty;
using Renee.Application.Interfaces;

namespace CommandHandlerTests.CopropertyTests;

public class DeleteCopropertyProfileCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetry = A.Fake<ITelemetryService>();

    [Fact]
    public async Task Handler_Should_ReturnTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _copropertyProfileRepository.DeleteCopropertyProfile(id)).Returns(1);

        var command = new DeleteCopropertyProfileCommandInput(id);
        var handler = new DeleteCopropertyProfileCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenDeleteReturnsMinusOne()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _copropertyProfileRepository.DeleteCopropertyProfile(id)).Returns(-1);

        var command = new DeleteCopropertyProfileCommandInput(id);
        var handler = new DeleteCopropertyProfileCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        A.CallTo(() => _copropertyProfileRepository.DeleteCopropertyProfile(id)).Throws(new Exception("Database error"));

        var command = new DeleteCopropertyProfileCommandInput(id);
        var handler = new DeleteCopropertyProfileCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}