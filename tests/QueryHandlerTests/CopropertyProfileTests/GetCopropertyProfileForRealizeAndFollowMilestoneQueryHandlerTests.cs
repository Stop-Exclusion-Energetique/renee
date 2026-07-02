using FakeItEasy;
using Renee.Application.Handlers.QueryHandlers.Coproperty;
using FluentAssertions;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;
using Renee.Application.Queries.Coproperty;

namespace QueryHandlerTests.CopropertyProfileTests;

public class GetCopropertyProfileForRealizeAndFollowMilestoneQueryHandlerTests
{
    private readonly ICopropertyProfileRepository _repo = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

    private static CopropertyProfile GetFakeProfile(Guid userId)
    {
        return new CopropertyProfile
        {
            Id = Guid.NewGuid(),
            CopropertyReference = "REF-789",
            CopropertyMilestone = (int)AccompanyingFileStage.RealisationAndFollowing,
            CopropertyStatus = (int)AccompanyingFileStatus.InProgress,
            CreationDate = DateTime.UtcNow.AddDays(-30),
            LastUpdateDate = DateTime.UtcNow,
            ZeroEnergyExclusionTerritoriesProgram = true,
            CopropertySupportTeamNavigation = new SupportTeam
            {
                TerritorialBuilder = userId,
                SolidarBuilder = Guid.NewGuid()
            },
            CopropertyWorkTrackingNavigation = new CopropertyWorkTracking
            {
                CollectiveWorksStartDate = DateTime.UtcNow.AddDays(-10),
                PlannedEndDate = DateTime.UtcNow.AddDays(20),
                ProgressPercentage = 80.0,
                ActualCompletionDate = DateTime.UtcNow.AddDays(5),
                InvoiceTotalAmount = 25000.0,
                FollowUpComment = "Travaux en cours"
            }
        };
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _repo.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var handler = new GetCopropertyProfileForRealizeAndFollowMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForRealizeAndFollowMilestoneQuery(Guid.NewGuid(), Guid.NewGuid(), "UserRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message?.Should().Be(Labels.Errors.ErrorWhileLoadingCopropertyProfile);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenUserIsInSupportTeam()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(userId);

        A.CallTo(() => _repo.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForRealizeAndFollowMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForRealizeAndFollowMilestoneQuery(profile.Id, userId, "UserRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value?.Reference?.Should().Be(profile.CopropertyReference);
        result.Value?.FollowUpComment?.Should().Be("Travaux en cours");
        result.Value?.InvoiceTotalAmount.Should().Be(25000.0);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenUserIsCoordinatorOrAdmin()
    {
        // Arrange
        var profile = GetFakeProfile(Guid.NewGuid());

        A.CallTo(() => _repo.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForRealizeAndFollowMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForRealizeAndFollowMilestoneQuery(profile.Id, Guid.NewGuid(), Constants.AdminRole);

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenUserIsNotAllowed()
    {
        // Arrange
        var profile = GetFakeProfile(Guid.NewGuid());

        A.CallTo(() => _repo.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForRealizeAndFollowMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForRealizeAndFollowMilestoneQuery(profile.Id, Guid.NewGuid(), "OtherRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message?.Should().Be(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
    }
}
