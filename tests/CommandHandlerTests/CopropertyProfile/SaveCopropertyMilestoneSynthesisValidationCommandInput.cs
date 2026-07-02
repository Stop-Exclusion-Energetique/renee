using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.Coproperty;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.Entity;

namespace CommandHandlerTests.CopropertyTests;

public class SaveCopropertyMilestoneSynthesisValidationCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();
    private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

    private static SaveCopropertyMilestoneSynthesisValidationCommandInput GetFakeCommand(
        Guid? profileId = null,
        AccompanyingFileStatus? status = null,
        AccompanyingFileStage? milestone = null,
        Guid? userId = null)
        => new(
            profileId ?? Guid.NewGuid(),
            status ?? AccompanyingFileStatus.InProgress,
            milestone ?? AccompanyingFileStage.Identify,
            userId ?? Guid.NewGuid()
        );

    private static CopropertyProfile GetFakeProfile(
        AccompanyingType? type = null,
        bool hasTerritorialBuilder = true,
        int? milestone = null)
    {
        var profile = new CopropertyProfile
        {
            AccompanyingType = (int?)type ?? (int)AccompanyingType.Targeted,
            CopropertySupportTeamNavigation = new SupportTeam
            {
                TerritorialBuilder = hasTerritorialBuilder ? Guid.NewGuid() : null
            },
            CopropertyMilestone = milestone ?? (int)AccompanyingFileStage.Identify
        };
        return profile;
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var command = GetFakeCommand();
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.ErrorCopropertyProfileNotFound);
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenTargetedTypeAndNoTerritorialBuilder()
    {
        // Arrange
        var profile = GetFakeProfile(type: AccompanyingType.Targeted, hasTerritorialBuilder: false);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        var command = GetFakeCommand();
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.NoTerritorialBuildersAffectedOnTargetedCopropertyProfileType);
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenUserNotFound()
    {
        // Arrange
        var profile = GetFakeProfile();

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns((Renee.Domain.Entity.User?)null);

        var command = GetFakeCommand();
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.UserNotFound);
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenUpdateReturnsZeroOrLess()
    {
        // Arrange
        var profile = GetFakeProfile();

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(0);

        var command = GetFakeCommand();
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.StageValidationErrorCopropertyProfileUpdateFailed);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenAllIsValid()
    {
        // Arrange
        var profile = GetFakeProfile();

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand();
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Message.Should().Be(Labels.StageValidationCopropertySynthesisSuccess);
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_AndTrackException_WhenExceptionIsThrown()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Throws(new Exception("Database error"));

        var command = GetFakeCommand();
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.UnhandledErrorOccured);
        A.CallTo(() => _telemetryService.TrackExceptionAsync(A<Exception>._, A<CancellationToken>._)).MustHaveHappened();
    }

    [Theory]
    [InlineData(AccompanyingFileStage.Identify)]
    [InlineData(AccompanyingFileStage.OrganizingAndFinancing)]
    [InlineData(AccompanyingFileStage.RealisationAndFollowing)]
    public async Task Handler_Should_Update_ValidationFields_BasedOnMilestone(AccompanyingFileStage stage)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(milestone: (int)stage);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand(milestone: stage, userId: userId);
        var handler = new SaveCopropertyMilestoneSynthesisValidationCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        await handler.Handle(command, default);

        // Assert
        switch (stage)
        {
            case AccompanyingFileStage.Identify:
                profile.IdentifySynthesisValidationDate.Should().NotBeNull();
                profile.IdentifyMilestoneValidatedBy.Should().Be(userId);
                break;
            case AccompanyingFileStage.OrganizingAndFinancing:
                profile.OrganizeAndFinanceSynthesisValidationDate.Should().NotBeNull();
                profile.OrganizeAndFinanceMilestoneValidatedBy.Should().Be(userId);
                break;
            case AccompanyingFileStage.RealisationAndFollowing:
                profile.RealizeAndFollowSynthesisValidationDate.Should().NotBeNull();
                profile.RealizeAndFollowMilestoneValidatedBy.Should().Be(userId);
                break;
        }
    }
}