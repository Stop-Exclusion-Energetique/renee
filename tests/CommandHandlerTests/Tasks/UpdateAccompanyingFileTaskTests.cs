using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.Task;
using Renee.Application.Handlers.CommandHandlers.Task;
using Renee.Application.Interfaces;
using Renee.Domain.Enums;

namespace CommandHandlerTests.Tasks;

public class UpdateAccompanyingFileTaskTests : TestContext
{
	private readonly ITaskRepository _taskRepository = A.Fake<ITaskRepository>();

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenTaskDtoIsInvalid()
	{
		//Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var hostEnvironment = Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.SetEnvironmentToDevelopment();

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		var command = new UpdateTaskCommandInput(
			new TaskDto(
				Guid.Empty,
				string.Empty,
				DateTime.UtcNow,
				DateTime.UtcNow,
				Guid.Empty,
				(ProgressTask?)ProgressTask.CurrentTask,
				TaskPriority.Low,
				Guid.NewGuid(),
				Guid.NewGuid()));

		var handler = new UpdateTaskCommandHandler(_taskRepository, mockTelemetry);

		//Act
		var result = await handler.Handle(command, default);

		//Assert
		result.Value.Should().Be(0);
	}

	[Fact]
	public async Task Handler_ShouldNot_CallUpdateTaskOnRepository_WhenTaskDtoIsInvalid()
	{
		//Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var hostEnvironment = Services.GetRequiredService<FakeWebAssemblyHostEnvironment>();
		hostEnvironment.SetEnvironmentToDevelopment();

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		var command = new UpdateTaskCommandInput(
			new TaskDto(
				Guid.Empty,
				string.Empty,
				DateTime.UtcNow,
				DateTime.UtcNow,
				Guid.Empty,
				(ProgressTask?)ProgressTask.CurrentTask,
				TaskPriority.Low,
				Guid.NewGuid(),
				Guid.NewGuid()));

		var handler = new UpdateTaskCommandHandler(_taskRepository, mockTelemetry);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _taskRepository.UpdateTaskStatus(A<AccompanyingFileTask>._)).MustNotHaveHappened();
	}
}