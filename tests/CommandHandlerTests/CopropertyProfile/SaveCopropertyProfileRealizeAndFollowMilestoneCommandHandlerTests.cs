using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.Coproperty.RealizeAndFollowStageUseCase;
using Renee.Application.Interfaces;

namespace CommandHandlerTests.CopropertyTests.RealizeAndFollow;

public class SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetry = A.Fake<ITelemetryService>();

    private static UpdateCopropertyWorkTracking GetFakeWorkTracking() => new(
        CollectiveWorksStartDate: DateTime.UtcNow.AddDays(-30),
        PlannedEndDate: DateTime.UtcNow.AddDays(60),
        ProgressPercentage: 75.5,
        ActualCompletionDate: DateTime.UtcNow.AddDays(10),
        InvoiceTotalAmount: 123456.78,
        FollowUpComment: "Travaux avancent bien"
    );

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenCopropertyProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var command = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeWorkTracking(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_ReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var copropertyProfile = new CopropertyProfile
        {
            CopropertyWorkTrackingNavigation = new CopropertyWorkTracking()
        };

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForForRealizeAndFollowMilestoneAsync(A<CopropertyProfile>._))
            .Returns(1);

        var command = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeWorkTracking(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenUpdateReturnsMinusOne()
    {
        // Arrange
        var copropertyProfile = new CopropertyProfile
        {
            CopropertyWorkTrackingNavigation = new CopropertyWorkTracking()
        };

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForForRealizeAndFollowMilestoneAsync(A<CopropertyProfile>._))
            .Returns(-1);

        var command = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeWorkTracking(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Throws(new Exception("Database error"));

        var command = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeWorkTracking(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_Update_All_WorkTrackingFields()
    {
        // Arrange
        var workTracking = new CopropertyWorkTracking();
        var copropertyProfile = new CopropertyProfile
        {
            CopropertyWorkTrackingNavigation = workTracking
        };

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForForRealizeAndFollowMilestoneAsync(A<CopropertyProfile>._))
            .Returns(1);

        var fakeTracking = GetFakeWorkTracking();

        var command = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
            Guid.NewGuid(),
            fakeTracking,
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        await handler.Handle(command, default);

        // Assert
        workTracking.CollectiveWorksStartDate.Should().Be(fakeTracking.CollectiveWorksStartDate);
        workTracking.PlannedEndDate.Should().Be(fakeTracking.PlannedEndDate);
        workTracking.ProgressPercentage.Should().Be(fakeTracking.ProgressPercentage);
        workTracking.ActualCompletionDate.Should().Be(fakeTracking.ActualCompletionDate);
        workTracking.InvoiceTotalAmount.Should().Be(fakeTracking.InvoiceTotalAmount);
        workTracking.FollowUpComment.Should().Be(fakeTracking.FollowUpComment);
    }
}