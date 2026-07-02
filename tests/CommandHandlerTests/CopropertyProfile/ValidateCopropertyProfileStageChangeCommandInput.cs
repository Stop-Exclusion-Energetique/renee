using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.Coproperty;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;

namespace CommandHandlerTests.CopropertyTests;

public class ValidateCopropertyProfileStageChangeCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();
    private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

    private static ValidateCopropertyProfileStageChangeCommandInput GetFakeCommand(
        Guid? profileId = null,
        bool isValidated = true,
        Guid? userId = null,
        string? comment = null)
        => new(
            profileId ?? Guid.NewGuid(),
            isValidated,
            userId ?? Guid.NewGuid(),
            comment
        );

    private static CopropertyProfile GetFakeProfile(
        int? milestone = null,
        int? status = null)
    {
        return new CopropertyProfile
        {
            CopropertyMilestone = milestone ?? (int)AccompanyingFileStage.Identify,
            CopropertyStatus = status ?? (int)AccompanyingFileStatus.InProgress
        };
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var command = GetFakeCommand();
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.ErrorCopropertyProfileNotFound);
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
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.UserNotFound);
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenUpdateReturnsZero()
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
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.StageValidationErrorCopropertyProfileUpdateFailed);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenValidated_IdentifyStage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(milestone: (int)AccompanyingFileStage.Identify);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand(userId: userId, isValidated: true);
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        profile.CopropertyMilestone.Should().Be((int)AccompanyingFileStage.OrganizingAndFinancing);
        profile.CopropertyStatus.Should().Be((int)AccompanyingFileStatus.InProgress);
        profile.IdentifySynthesisValidationDate.Should().NotBeNull();
        profile.IdentifyMilestoneValidatedBy.Should().Be(userId);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenValidated_OrganizingAndFinancingStage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(milestone: (int)AccompanyingFileStage.OrganizingAndFinancing);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand(userId: userId, isValidated: true);
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        profile.CopropertyMilestone.Should().Be((int)AccompanyingFileStage.RealisationAndFollowing);
        profile.CopropertyStatus.Should().Be((int)AccompanyingFileStatus.InProgress);
        profile.OrganizeAndFinanceSynthesisValidationDate.Should().NotBeNull();
        profile.OrganizeAndFinanceMilestoneValidatedBy.Should().Be(userId);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenValidated_RealisationAndFollowingStage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(milestone: (int)AccompanyingFileStage.RealisationAndFollowing);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand(userId: userId, isValidated: true);
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        profile.CopropertyMilestone.Should().Be((int)AccompanyingFileStage.Finished);
        profile.CopropertyStatus.Should().Be((int)AccompanyingFileStatus.Finished);
        profile.RealizeAndFollowSynthesisValidationDate.Should().NotBeNull();
        profile.RealizeAndFollowMilestoneValidatedBy.Should().Be(userId);
    }

    [Fact]
    public async Task Handler_Should_SetRejectedStatus_AndComment_WhenNotValidated_WithComment()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var comment = "Rejeté pour motif X";
        var profile = GetFakeProfile(milestone: (int)AccompanyingFileStage.Identify);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand(userId: userId, isValidated: false, comment: comment);
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        profile.CopropertyStatus.Should().Be((int)AccompanyingFileStatus.Rejected);
        profile.IdentifyMilestoneValidatedBy.Should().Be(userId);
        profile.RejectionCommentOnSynthesisValidation.Should().Be(comment);
    }

    [Fact]
    public async Task Handler_Should_SetRejectedStatus_WithoutComment_WhenNotValidated_AndNoComment()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(milestone: (int)AccompanyingFileStage.OrganizingAndFinancing);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Returns(profile);

        A.CallTo(() => _userRepository.GetUserById(A<Guid>._))
            .Returns(new Renee.Domain.Entity.User());

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileSynthesis(A<CopropertyProfile>._))
            .Returns(1);

        var command = GetFakeCommand(userId: userId, isValidated: false, comment: null);
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        profile.CopropertyStatus.Should().Be((int)AccompanyingFileStatus.Rejected);
        profile.OrganizeAndFinanceMilestoneValidatedBy.Should().Be(userId);
        profile.RejectionCommentOnSynthesisValidation.Should().BeNull();
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_AndTrackException_WhenExceptionIsThrown()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileSynthesis(A<Guid>._))
            .Throws(new Exception("Database error"));

        var command = GetFakeCommand();
        var handler = new ValidateCopropertyProfileStageChangeCommandHandler(
            _copropertyProfileRepository, _userRepository, _telemetryService);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.UnhandledErrorOccured);
        A.CallTo(() => _telemetryService.TrackExceptionAsync(A<Exception>._, A<CancellationToken>._)).MustHaveHappened();
    }
}