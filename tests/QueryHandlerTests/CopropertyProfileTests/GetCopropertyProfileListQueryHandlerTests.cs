using Renee.Application.Handlers.QueryHandlers.Coproperty;
using Renee.Application.Queries.Coproperty;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;
using Renee.Application.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Renee.Domain;

namespace QueryHandlerTests.CopropertyProfileTests;

public class GetCopropertyProfileListQueryHandlerTests
{
    private readonly ICopropertyProfileRepository _repo = A.Fake<ICopropertyProfileRepository>();
    private readonly IUserRepository _userRepo = A.Fake<IUserRepository>();
    private readonly ITelemetryService _telemetry = A.Fake<ITelemetryService>();

    private static User GetFakeUser(Guid? structureId = null) =>
        new() { ReportingStructureId = structureId ?? Guid.NewGuid() };

    private static CopropertyProfile GetFakeProfile(Guid id)
    {
        return new CopropertyProfile
        {
            Id = id,
            CopropertyReference = $"REF-{id}",
            CopropertyMilestone = (int)AccompanyingFileStage.Identify,
            CopropertyStatus = (int)AccompanyingFileStatus.InProgress,
            CreationDate = DateTime.UtcNow.AddDays(-10),
            LastUpdateDate = DateTime.UtcNow,
            CopropertyHousingNavigation = new CopropertyHousing
            {
                HousingAddressNavigation = new Address { Label = "Résidence Les Lilas" }
            },
            CopropertySupportTeamNavigation = new SupportTeam
            {
                SolidarBuilder = Guid.NewGuid(),
                SecondSolidarBuilder = Guid.NewGuid(),
                ThirdSolidarBuilder = Guid.NewGuid(),
                TerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilder = Guid.NewGuid(),
                DiffuseCoordinator = Guid.NewGuid(),
                TargetCoordinator = Guid.NewGuid(),
                SolidarBuilderNavigation = new User { ReportingStructureId = Guid.NewGuid() }
            },
            CopropertyTerritory = Guid.NewGuid()
        };
    }

    [Fact]
    public async Task Handler_Should_ReturnEmpty_WhenUserIdIsNull()
    {
        // Arrange
        var handler = new GetCopropertyProfileListQueryHandler(_repo, _userRepo, _telemetry);
        var query = new GetCopropertyProfileListQuery(Constants.SolidarBuilderRole, Guid.Empty);

        // Act
        var result = await handler.HandleQuery(query);

        // Assert
        result.Value?.CopropertyProfiles.Should().BeEmpty();
    }

    [Fact]
    public async Task Handler_Should_ReturnEmpty_WhenUserNotFound()
    {
        // Arrange
        A.CallTo(() => _userRepo.GetUserById(A<Guid>._)).Returns((User?)null);

        var handler = new GetCopropertyProfileListQueryHandler(_repo, _userRepo, _telemetry);
        var query = new GetCopropertyProfileListQuery(Constants.SolidarBuilderRole);

        // Act
        var result = await handler.HandleQuery(query);

        // Assert
        result.Value?.CopropertyProfiles.Should().BeEmpty();
    }

    [Fact]
    public async Task Handler_Should_ReturnEmpty_WhenUserHasNoReportingStructure()
    {
        // Arrange
        A.CallTo(() => _userRepo.GetUserById(A<Guid>._)).Returns(new User { ReportingStructureId = null });

        var handler = new GetCopropertyProfileListQueryHandler(_repo, _userRepo, _telemetry);
        var query = new GetCopropertyProfileListQuery(Constants.SolidarBuilderRole, Guid.NewGuid());

        // Act
        var result = await handler.HandleQuery(query);

        // Assert
        result.Value?.CopropertyProfiles.Should().BeEmpty();
    }

    [Fact]
    public async Task Handler_Should_ReturnEmpty_WhenRoleIsUnknown()
    {
        // Arrange
        var user = GetFakeUser();
        A.CallTo(() => _userRepo.GetUserById(A<Guid>._)).Returns(user);

        var handler = new GetCopropertyProfileListQueryHandler(_repo, _userRepo, _telemetry);
        var query = new GetCopropertyProfileListQuery("UnknownRole");

        // Act
        var result = await handler.HandleQuery(query);

        // Assert
        result.Value?.CopropertyProfiles.Should().BeEmpty();
    }

    [Theory]
    [InlineData(Constants.SolidarBuilderRole)]
    [InlineData(Constants.AssociationMemberRole)]
    [InlineData(Constants.AdminRole)]
    [InlineData(Constants.DiffuseCoordinatorRole)]
    [InlineData(Constants.TargetedCoordinatorRole)]
    [InlineData(Constants.TerritorialBuilderRole)]
    [InlineData(Constants.StructuralReferentRole)]
    public async Task Handler_Should_ReturnProfiles_ForKnownRoles(string role)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var structureId = Guid.NewGuid();
        var user = GetFakeUser(structureId);

        var profiles = new List<CopropertyProfile>
        {
            GetFakeProfile(Guid.NewGuid()),
            GetFakeProfile(Guid.NewGuid())
        };

        A.CallTo(() => _userRepo.GetUserById(userId)).Returns(user);

        switch (role)
        {
            case Constants.SolidarBuilderRole:
                A.CallTo(() => _repo.GetAllCopropertyProfilesBySolidarBuilderReportingStructure(structureId)).Returns(profiles);
                break;
            case Constants.AssociationMemberRole:
            case Constants.AdminRole:
                A.CallTo(() => _repo.GetAllCopropertyProfilesForAssociationMemberDisplay()).Returns(profiles);
                break;
            case Constants.DiffuseCoordinatorRole:
            case Constants.TargetedCoordinatorRole:
                A.CallTo(() => _repo.GetAllCopropertyProfilesForCoordinators()).Returns(profiles);
                break;
            case Constants.TerritorialBuilderRole:
                A.CallTo(() => _repo.GetAllCopropertyProfilesForTerritorialBuilder(userId)).Returns(profiles);
                break;
            case Constants.StructuralReferentRole:
                A.CallTo(() => _repo.GetAllCopropertyProfilesForStructuralReferent(structureId)).Returns(profiles);
                break;
        }

        var handler = new GetCopropertyProfileListQueryHandler(_repo, _userRepo, _telemetry);
        var query = new GetCopropertyProfileListQuery(role, userId);

        // Act
        var result = await handler.HandleQuery(query);

        // Assert
        result.Value?.CopropertyProfiles.Should().HaveCount(2);
        result.Value?.CopropertyProfiles.All(cp => cp.Reference.StartsWith("REF-")).Should().BeTrue();
        result.Value?.CopropertyProfiles.All(cp => cp.Address == "Résidence Les Lilas").Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_ReturnEmpty_WhenExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = GetFakeUser(Guid.NewGuid());

        A.CallTo(() => _userRepo.GetUserById(userId)).Returns(user);
        A.CallTo(() => _repo.GetAllCopropertyProfilesBySolidarBuilderReportingStructure(A<Guid>._))
            .Throws(new Exception("Database error"));

        var handler = new GetCopropertyProfileListQueryHandler(_repo, _userRepo, _telemetry);
        var query = new GetCopropertyProfileListQuery(Constants.SolidarBuilderRole, userId);

        // Act
        var result = await handler.HandleQuery(query);

        // Assert
        result.Value?.CopropertyProfiles.Should().BeEmpty();
        A.CallTo(() => _telemetry.TrackExceptionAsync(A<Exception>._, A<CancellationToken?>._)).MustHaveHappened();
    }
}