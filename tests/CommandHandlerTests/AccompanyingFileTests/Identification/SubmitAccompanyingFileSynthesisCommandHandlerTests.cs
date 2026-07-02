using Microsoft.Extensions.DependencyInjection;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.IdentificationStageUseCase;
using Renee.Application.Interfaces;
using Renee.Domain.Enums;

namespace CommandHandlerTests.AccompanyingFileTests.Identification;

public class SubmitAccompanyingFileSynthesisCommandHandlerTests : TestContext
{
	private readonly IAccompanyingFileRepository _accompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
	private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenAccompanyingFileIdIsEmpty()
	{
		// Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		var command = new SaveIdentificationMilestoneSynthesisValidationCommandInput(
				Guid.Empty,
				AccompanyingFileStatus.InProgress,
				AccompanyingFileStage.Identify,
				Guid.NewGuid()
			);

		var handler = new SaveIdentificationMilestoneSynthesisValidationCommandHandler(
			_accompanyingFileRepository,
			_userRepository,
			mockTelemetry);

		//Act
		var result = await handler.Handle(command, default);

		// Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenThereIsNoChangedItems()
	{
		//Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		A.CallTo(
			() => _accompanyingFileRepository.UpdateAccompanyingFileSynthesis(
				A<AccompanyingFile>.Ignored,
				A<List<Invoice>>.Ignored)).Returns(-1);

		var command = new SaveIdentificationMilestoneSynthesisValidationCommandInput(
			Guid.Empty,
			AccompanyingFileStatus.InProgress,
			AccompanyingFileStage.Identify,
			Guid.NewGuid());

		var handler = new SaveIdentificationMilestoneSynthesisValidationCommandHandler(
			_accompanyingFileRepository,
			_userRepository,
			mockTelemetry);

		//Act
		var result = await handler.Handle(command, default);

		// Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public async Task
		Handler_Should_ReturnTrue_WhenAccompanyingFileIdIsKnownAndUpdateAccompanyingFileSynthesisOnRepositoryIsCalled()
	{
		// Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		A.CallTo(
			() => _accompanyingFileRepository.UpdateAccompanyingFileSynthesis(
				A<AccompanyingFile>.Ignored,
				A<List<Invoice>>.Ignored)).Returns(1);

		var command = new SaveIdentificationMilestoneSynthesisValidationCommandInput(
			Guid.NewGuid(),
			AccompanyingFileStatus.InProgress,
			AccompanyingFileStage.Identify,
			Guid.NewGuid());
		var handler = new SaveIdentificationMilestoneSynthesisValidationCommandHandler(
			_accompanyingFileRepository,
			_userRepository,
			mockTelemetry);

		//Act
		var result = await handler.Handle(command, default);

		// Assert
		result.IsSuccess.Should().BeTrue();
	}

	[Fact]
	public async Task Handler_Should_ThrowExceptionAndReturnFalse_WhenThereIsAProblemWhileUpdating()
	{
		//Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		A.CallTo(
			() => _accompanyingFileRepository.UpdateAccompanyingFileSynthesis(
				A<AccompanyingFile>.Ignored,
				A<List<Invoice>>.Ignored)).Throws(new Exception());

		var command = new SaveIdentificationMilestoneSynthesisValidationCommandInput(
			Guid.Empty,
			AccompanyingFileStatus.InProgress,
			AccompanyingFileStage.Identify,
			Guid.NewGuid());

		var handler = new SaveIdentificationMilestoneSynthesisValidationCommandHandler(
			_accompanyingFileRepository,
			_userRepository,
			mockTelemetry);

		//Act
		var result = await handler.Handle(command, default);

		// Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public async Task Handler_ShouldNotCallUpdateAccompanyingFileSynthesisOnRepository_WhenAccompanyingFileIdIsEmpty()
	{
		// Arrange
		Services.AddApplicationInsightsTelemetry();
		Services.AddSingleton(A.Fake<ITelemetryService>());

		var mockTelemetry = Services.GetRequiredService<ITelemetryService>();

		var command = new SaveIdentificationMilestoneSynthesisValidationCommandInput(
			Guid.Empty,
			AccompanyingFileStatus.InProgress,
			AccompanyingFileStage.Identify,
			Guid.NewGuid());

		var handler = new SaveIdentificationMilestoneSynthesisValidationCommandHandler(
			_accompanyingFileRepository,
			_userRepository,
			mockTelemetry);

		//Act
		await handler.Handle(command, default);

		// Assert
		A.CallTo(
			() => _accompanyingFileRepository.UpdateAccompanyingFileSynthesis(
				A<AccompanyingFile>.Ignored,
				A<List<Invoice>>.Ignored)).MustNotHaveHappened();
	}
}