using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain;
using Renee.Domain.Enums;

namespace CommandHandlerTests.AccompanyingFileTests.ImportCsvData;

public class CreateAccompanyingFileFromCsvImportCommandHandlerTests
{
	private static ImportAccompanyingFileFromCsvCommandInput CreateDefaultInput(
	string reference,
	string? externalReference,
	int line) =>
		new(
			line,
			reference,
			externalReference,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			new ImportSupportTeamFromCsv(
				Guid.NewGuid(),
				MarkerNature.Association,
				null),
			new ImportHouseholdFromCsv(
				null,
				null,
				null,
				null,
				null,
				[],
				[],
				[],
				[],
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				new MainOccupant()),
			new ImportHousingFromCsv(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				new Address(),
				new HousingInitialState(),
				new HousingAfterWorkState()),
			new ImportPreWorkPlanFromCsv(
				[],
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				[]),
			new ImportPreFinancingPlanFromCsv(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				[]),
			new ImportWorkMonitoringFromCsv(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null));

	[Fact]
	public async Task Handler_WhenUserIdIsEmpty_ShouldReturnTheExpectedErrorMessage()
	{
		// Arrange
		var inputs = new List<ImportAccompanyingFileFromCsvCommandInput>
		{
			CreateDefaultInput("Référence-1", null, 2)
		};

		var mockAccompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
		var mockTelemetryService = A.Fake<ITelemetryService>();

		var request = new CreateAccompanyingFileFromCsvFileCommandInput(
			inputs,
			Guid.Empty,
			Guid.Empty);

		var handler = new CreateAccompanyingFileFromCsvImportCommandHandler(
			mockAccompanyingFileRepository,
			mockTelemetryService);

		// Act
		var result = await handler.Handle(request, new CancellationToken());

		// Assert
		result.ImportCsvResultStatus.Should().Be(ImportCsvResultStatus.Failure);
		result.ErrorMessages.Should().BeEquivalentTo([LineErrorReport.Create(null, Labels.Errors.UserNotFound)]);
	}

	[Fact]
	public async Task Handler_WhenContainDuplicatedExternalReferences_ShouldReturnTheExpectedErrorMessage()
	{
		// Arrange
		var inputs = new List<ImportAccompanyingFileFromCsvCommandInput>
		{
			CreateDefaultInput("Référence-1", "External-1", 2),
			CreateDefaultInput("Référence-2", "External-1", 3),
			CreateDefaultInput("Référence-1", "External-2", 4)
		};

		var mockAccompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();

		A.CallTo(() => mockAccompanyingFileRepository.AddAccompanyingFiles(A<List<AccompanyingFile>>._)).Returns(1);

		var mockTelemetryService = A.Fake<ITelemetryService>();

		var request = new CreateAccompanyingFileFromCsvFileCommandInput(
			inputs,
			Guid.NewGuid(),
			Guid.Empty);

		var handler = new CreateAccompanyingFileFromCsvImportCommandHandler(
			mockAccompanyingFileRepository,
			mockTelemetryService);

		// Act
		var result = await handler.Handle(request, new CancellationToken());

		// Assert
		result.ImportCsvResultStatus.Should().Be(ImportCsvResultStatus.PartialSuccess);
		result.ErrorMessages.Should().BeEquivalentTo([
			LineErrorReport.Create(inputs[0].LineNumber, string.Format(CsvDataLabel.Errors.DuplicatedExternalReference, inputs[0].ExternalReference))]);
	}

	[Fact]
	public async Task Handler_WhenThereIsAnErrorWithAddAccompanyingFilesMethod_ShouldReturnTheExpectedErrorMessage()
	{
		// Arrange
		var mockAccompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();

		A.CallTo(() => mockAccompanyingFileRepository.AddAccompanyingFiles(A<List<AccompanyingFile>>._)).Returns(-1);

		var inputs = new List<ImportAccompanyingFileFromCsvCommandInput>
		{
			CreateDefaultInput("Référence-1", "External-1", 2)
		};

		var mockTelemetryService = A.Fake<ITelemetryService>();
		var request = new CreateAccompanyingFileFromCsvFileCommandInput(
			inputs,
			Guid.NewGuid(),
			Guid.Empty);

		var handler = new CreateAccompanyingFileFromCsvImportCommandHandler(
			mockAccompanyingFileRepository,
			mockTelemetryService);

		// Act
		var result = await handler.Handle(request, new CancellationToken());

		// Assert
		result.ImportCsvResultStatus.Should().Be(ImportCsvResultStatus.Failure);
		result.ErrorMessages.Should().BeEquivalentTo(
		[
			LineErrorReport.Create(null, CsvDataLabel.Errors.ErrorWhileCreatingAccompanyingFiles)
		]);
	}

	[Fact]
	public async Task Handler_WhenNoExternalReferencesAndNoReferencesAreDuplicatedAndAddAccompanyingFilesMethodSucceeded_ShouldReturnNoErrorMessage()
	{
		// Arrange
		var mockAccompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();

		A.CallTo(() => mockAccompanyingFileRepository.AddAccompanyingFiles(A<List<AccompanyingFile>>._)).Returns(10);

		var inputs = new List<ImportAccompanyingFileFromCsvCommandInput>
		{
			CreateDefaultInput("Référence-1", "External-1", 2),
			CreateDefaultInput("Référence-2", "External-2", 3),
			CreateDefaultInput("Référence-3", "External-3", 4),
			CreateDefaultInput("Référence-4", "External-4", 5)
		};

		var mockTelemetryService = A.Fake<ITelemetryService>();
		var request = new CreateAccompanyingFileFromCsvFileCommandInput(
			inputs,
			Guid.NewGuid(),
			Guid.Empty);

		var handler = new CreateAccompanyingFileFromCsvImportCommandHandler(
			mockAccompanyingFileRepository,
			mockTelemetryService);

		// Act
		var result = await handler.Handle(request, new CancellationToken());

		// Assert
		result.ImportCsvResultStatus.Should().Be(ImportCsvResultStatus.Success);
		result.ErrorMessages.Should().BeNull();
	}
}