using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.IdentificationStageUseCase;
using Renee.Application.Interfaces;
using Renee.Domain.DomainExtension.ToRepository;

namespace CommandHandlerTests.AccompanyingFileTests.Identification;

public class UpdateAccompanyingFileCommandHandlerTests
{
	private readonly IAccompanyingFileRepository _accompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
	private readonly IAdminConstantRepository _adminConstantRepository = A.Fake<IAdminConstantRepository>();
	private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenGetAccompanyingFileByIdReturnNull()
	{
		//Arrange
		A.CallTo(() => _accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(A<Guid>._))
			.Returns<AccompanyingFile?>(null);

		var command = new SaveAccompanyingFileIdentificationMilestoneCommandInput(
			Guid.NewGuid(),
			null,
			new UpdatedHousehold(
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
				null),
			new UpdatedHouseholdMainOccupant(
				null,
				string.Empty,
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
				string.Empty,
				string.Empty),
			[],
			null,
			[],
			[],
			[],
			new UpdatedHousing(),
			new UpdatedAddress(null, null, null, null, null, null, null),
			new UpdatedHousingInitialStateForIdentificationMilestone(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null),
			Guid.NewGuid(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			null,
			null,
			false);
		var handler = new SaveAccompanyingFileIdentificationMilestoneCommandHandler(_accompanyingFileRepository, _adminConstantRepository, _telemetryService);

		//Act
		var result = await handler.Handle(command, default);

		//Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public async Task
		Handler_Should_ReturnTrue_WhenAccompanyingFileExistsAndUpdateAccompanyingFileOnRepositoryIsCalled()
	{
		var accompanyingFile = new AccompanyingFile
		{
			AccompanyingFileReference = "EM-75002-240222",
			AccompanyingFileHouseholdNavigation = new Household { MainOccupantNavigation = new MainOccupant() },
			AccompanyingFileHousingNavigation = new Housing
			{
				HousingAddressNavigation = new Address(),
				HousingInitialStateNavigation = new HousingInitialState()
			}
		};

		var accompanyingFileModified = new AccompanyingFile { AccompanyingFileReference = "EM-75002-240222" };

		A.CallTo(() => _accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(A<Guid>._))
			.Returns(accompanyingFile);
		A.CallTo(
			() => _accompanyingFileRepository.UpdateAccompanyingFileByIdForIdentificationMilestoneAsync(
				accompanyingFileModified,
				A<SaveAndSubmitIdentificationMilestoneData>._)).Returns(1);

		var command = new SaveAccompanyingFileIdentificationMilestoneCommandInput(
			Guid.NewGuid(),
			null,
			new UpdatedHousehold(
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
				null),
			new UpdatedHouseholdMainOccupant(
				null,
				string.Empty,
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
				string.Empty,
				string.Empty),
			[],
			null,
			[],
			[],
			[],
			new UpdatedHousing(),
			new UpdatedAddress(null, null, null, null, null, null, null),
			new UpdatedHousingInitialStateForIdentificationMilestone(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null),
			Guid.NewGuid(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			null,
			null,
			false);
		var handler = new SaveAccompanyingFileIdentificationMilestoneCommandHandler(_accompanyingFileRepository, _adminConstantRepository, _telemetryService);

		var result = await handler.Handle(command, default);

		result.IsSuccess.Should().BeTrue();
	}

	[Fact]
	public async Task Handler_ShouldNot_CallUpdateAccompanyingFileOnRepository_WhenGetAccompanyingFileIdReturnNull()
	{
		//Arrange
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(), AccompanyingFileReference = "EM-75002-240222"
		};

		A.CallTo(
			() => _accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(
				accompanyingFile.Id)).Returns(new AccompanyingFile());

		var command = new SaveAccompanyingFileIdentificationMilestoneCommandInput(
			Guid.NewGuid(),
			null,
			new UpdatedHousehold(
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
				null),
			new UpdatedHouseholdMainOccupant(
				null,
				string.Empty,
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
				string.Empty,
				string.Empty),
			[],
			null,
			[],
			[],
			[],
			new UpdatedHousing(),
			new UpdatedAddress(null, null, null, null, null, null, null),
			new UpdatedHousingInitialStateForIdentificationMilestone(
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null),
			Guid.NewGuid(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			null,
			null,
			false);
		var handler = new SaveAccompanyingFileIdentificationMilestoneCommandHandler(_accompanyingFileRepository, _adminConstantRepository, _telemetryService);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(
			() => _accompanyingFileRepository.UpdateAccompanyingFileByIdForIdentificationMilestoneAsync(
				accompanyingFile,
				A<SaveAndSubmitIdentificationMilestoneData>._)).MustNotHaveHappened();
	}
}
