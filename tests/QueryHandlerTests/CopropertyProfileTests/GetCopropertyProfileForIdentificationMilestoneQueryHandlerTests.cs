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

public class GetCopropertyProfileForIdentificationMilestoneQueryHandlerTests
{
    private readonly ICopropertyProfileRepository _repo = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

    private static CopropertyProfile GetFakeProfile(Guid userId)
    {
        return new CopropertyProfile
        {
            Id = Guid.NewGuid(),
            CopropertyReference = "REF-123",
            CopropertyMilestone = (int)AccompanyingFileStage.Identify,
            CopropertyStatus = (int)AccompanyingFileStatus.InProgress,
            CreationDate = DateTime.UtcNow.AddDays(-10),
            LastUpdateDate = DateTime.UtcNow,
            ZeroEnergyExclusionTerritoriesProgram = true,
            CopropertySupportTeamNavigation = new SupportTeam
            {
                TerritorialBuilder = userId,
                SolidarBuilder = Guid.NewGuid()
            },
            CopropertyHousingNavigation = new CopropertyHousing
            {
                GeographicAreaTypology = (int)GeographicalHousingAreaTypology.Urban,
                HousingType = (int)HousingType.IndividualHouse,
                NumberOfFloor = 5,
                NumberOfLots = 20,
                HeatingType = (int)HeatingType.Collective,
                PerilType = (int)PerilType.BudgetControl,
                HousingAddressNavigation = new Address
                {
                    Label = "Résidence Les Lilas",
                    PostalCode = "75018",
                    City = "Paris",
                    Department = "75",
                    Region = "IDF",
                    Street = "Rue des Lilas",
                    HouseNumber = "12",
                    AdditionnalComment = "Bâtiment B"
                }
            },
            CopropertyGovernanceNavigation = new CopropertyGovernance
            {
                NatureOfSyndic = (int)NatureOfSyndicType.Professional,
                NameOfSyndic = "SyndicPro",
                PhoneOfSyndic = "0601020303",
                MailOfSyndic = "syndic@pro.fr",
                NameOfAmo = "AMO",
                ContactOfAmo = "ContactAMO",
                NumberOfContacts = 2
            },
            CopropertyDiagnosticsNavigation = new CopropertyDiagnostics
            {
                BuildingDpeLabel = (int)DpeLabel.B,
                BuildingDpeEnergy = 120.5,
                ApartmentDpeLabel = (int)DpeLabel.C,
                ApartmentDpeEnergy = 110.0
            }
        };
    }

    [Fact]
    public async Task Handler_Should_ReturnFailure_WhenProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _repo.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var handler = new GetCopropertyProfileForIdentificationMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForIdentificationMilestoneQuery(Guid.NewGuid(), Guid.NewGuid(), "UserRole");

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

        A.CallTo(() => _repo.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForIdentificationMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForIdentificationMilestoneQuery(profile.Id, userId, "UserRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Reference.Should().Be(profile.CopropertyReference);
        result.Value.Address?.Label.Should().Be(profile.CopropertyHousingNavigation.HousingAddressNavigation.Label);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenUserIsCoordinatorOrAdmin()
    {
        // Arrange
        var profile = GetFakeProfile(Guid.NewGuid());

        A.CallTo(() => _repo.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForIdentificationMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForIdentificationMilestoneQuery(profile.Id, Guid.NewGuid(), Constants.AdminRole);

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

        A.CallTo(() => _repo.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns(profile);

        var handler = new GetCopropertyProfileForIdentificationMilestoneQueryHandler(_repo, _telemetryService);
        var query = new GetCopropertyProfileForIdentificationMilestoneQuery(profile.Id, Guid.NewGuid(), "OtherRole");

        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
    }
}
