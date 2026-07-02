using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.OrganizeAndFinanceStateUseCase;
using Renee.Application.Interfaces;

namespace CommandHandlerTests.AccompanyingFileTests.OrganizeAndFinance;

public class SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandTests
{
	private readonly IAccompanyingFileRepository _accompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
	private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

	[Fact]
	public async Task Handler_Should_ReturnFalse_WhenAccompanyingFileIdIsEmpty()
	{
		// Arrange

		var updateHousing = new UpdateHousing(null, null, null, null, null, null, null, null);

		var updateHousingInitialState = new UpdatedHousingInitialStateForOrganizeAndFinanceMilestone(
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null);

		var updateHousingAfterWorkState = new UpdateHousingAfterWorkState(null, null, null, null, null);

		var updatePreWorkPlan = new UpdatePreWorkPlan(
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
			[],
			[]);

		var updatePrefinancingPlan = new UpdatePreFinancingPlan(
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
			[],
			null);

		var command = new SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput(
			Guid.Empty,
			null,
			updateHousing,
			updateHousingInitialState,
			updateHousingAfterWorkState,
			updatePreWorkPlan,
			updatePrefinancingPlan,
			[],
			[],
			Guid.NewGuid(),
			null,
			null);

		var handler = new SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandHandler(_accompanyingFileRepository, _telemetryService);

		//Act
		var result = await handler.Handle(command, default);

		// Assert
		result.IsSuccess.Should().BeFalse();
	}
}