using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.Coproperty.IdentificationStageUseCase;
using Renee.Application.Interfaces;

namespace CommandHandlerTests.CopropertyTests.Identification;

public class SaveCopropertyProfileIdentificationMilestoneCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetry = A.Fake<ITelemetryService>();

    private static UpdatedCopropertyHousing GetFakeHousing() => new(
        GeographicAreaTypology: 1,
        HousingType: 2,
        NumberOfLots: 10,
        NumberOfFloor: 3,
        HeatingType: 1,
        PerilType: 0
    );

    private static UpdatedGovernanceContats GetFakeGovernance() => new(
        NatureOfSyndic: 1,
        NameOfSyndic: "SyndicTest",
        PhoneOfSyndic: "0601020304",
        MailOfSyndic: "syndic@test.fr",
        NameOfAmo: "AmoTest",
        ContactOfAmo: "ContactAmo",
        NumberOfContacts: 2
    );

    private static UpdatedDiagnostics GetFakeDiagnostics() => new(
        BuildingDpeLabel: 2,
        BuildingDpeEnergy: 150.5,
        ApartmentDpeLabel: 3,
        ApartmentDpeEnergy: 120.0
    );

    private static UpdatedCopropertyAddress GetFakeAddress() => new(
        Label: "12 rue des Lilas",
        PostalCode: "75018",
        City: "Paris",
        Department: "75",
        Region: "Île-de-France",
        AdditionalComment: "Entrée B"
    );

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenCopropertyProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var command = new SaveCopropertyProfileIdentificationMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAddress(),
            GetFakeHousing(),
            GetFakeGovernance(),
            GetFakeDiagnostics(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileIdentificationMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

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
            CopropertyHousingNavigation = new CopropertyHousing { HousingAddressNavigation = new Address() },
            CopropertyGovernanceNavigation = new CopropertyGovernance(),
            CopropertyDiagnosticsNavigation = new CopropertyDiagnostics()
        };

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForIdentificationMilestoneAsync(A<CopropertyProfile>._))
            .Returns(1);

        var command = new SaveCopropertyProfileIdentificationMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAddress(),
            GetFakeHousing(),
            GetFakeGovernance(),
            GetFakeDiagnostics(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileIdentificationMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

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
            CopropertyHousingNavigation = new CopropertyHousing { HousingAddressNavigation = new Address() },
            CopropertyGovernanceNavigation = new CopropertyGovernance(),
            CopropertyDiagnosticsNavigation = new CopropertyDiagnostics()
        };

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForIdentificationMilestoneAsync(A<CopropertyProfile>._))
            .Returns(-1);

        var command = new SaveCopropertyProfileIdentificationMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAddress(),
            GetFakeHousing(),
            GetFakeGovernance(),
            GetFakeDiagnostics(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileIdentificationMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForIdentificationMilestoneAsync(A<Guid>._))
            .Throws(new Exception("Database error"));

        var command = new SaveCopropertyProfileIdentificationMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAddress(),
            GetFakeHousing(),
            GetFakeGovernance(),
            GetFakeDiagnostics(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileIdentificationMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}