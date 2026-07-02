using FakeItEasy;
using FluentAssertions;
using Renee.Application.Handlers.QueryHandlers.Coproperty;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace QueryHandlerTests.CopropertyProfileTests;


public class GetCopropertyProfileForOrganizeAndFinanceMilestoneQueryHandlerTests
{
    private readonly ICopropertyProfileRepository _repo = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

    private static CopropertyProfile GetFakeProfile(Guid userId)
    {
        var workTypeId = Guid.NewGuid();
        var workPackageId = Guid.NewGuid();
        return new CopropertyProfile
        {
            Id = Guid.NewGuid(),
            CopropertyReference = "REF-456",
            CopropertyMilestone = (int)AccompanyingFileStage.OrganizingAndFinancing,
            CopropertyStatus = (int)AccompanyingFileStatus.InProgress,
            CreationDate = DateTime.UtcNow.AddDays(-20),
            LastUpdateDate = DateTime.UtcNow,
            ZeroEnergyExclusionTerritoriesProgram = false,
            CopropertySupportTeamNavigation = new SupportTeam
            {
                TerritorialBuilder = userId,
                SolidarBuilder = Guid.NewGuid()
            },
            CopropertyWorkFinanceNavigation = new CopropertyWorkFinance
            {
                DateOfAgVote = DateTime.UtcNow.AddDays(-5),
                MprCoproAids = 5000.0,
                ComplementaryAids = 2000.0,
                WorkPackages = new List<WorkPackage>
                {
                    new WorkPackage
                    {
                        Id = workPackageId,
                        EnergeticsEffectAfterWorks = "Isolation",
                        WorkPackageWorkTypeCosts = new List<WorkPackageWorkTypeCost>
                        {
                            new WorkPackageWorkTypeCost
                            {
                                WorkType = workTypeId,
                                Cost = 1500.0,
                                Description = "Isolation murs"
                            }
                        }
                    }
                }
            }
        };
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _repo.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var handler = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery(Guid.NewGuid(), Guid.NewGuid(), "UserRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.ErrorWhileLoadingCopropertyProfile);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenUserIsInSupportTeam()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var profile = GetFakeProfile(userId);

        A.CallTo(() => _repo.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery(profile.Id, userId, "UserRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value?.Reference.Should().Be(profile.CopropertyReference);
        result.Value?.WorkPackages.Should().NotBeNull();
        result.Value?.WorkPackages?.First().TypeCostDtos?.First().Description.Should().Be("Isolation murs");
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenUserIsCoordinatorOrAdmin()
    {
        // Arrange
        var profile = GetFakeProfile(Guid.NewGuid());

        A.CallTo(() => _repo.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery(profile.Id, Guid.NewGuid(), Constants.AdminRole);

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

        A.CallTo(() => _repo.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery(profile.Id, Guid.NewGuid(), "OtherRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
    }
}
