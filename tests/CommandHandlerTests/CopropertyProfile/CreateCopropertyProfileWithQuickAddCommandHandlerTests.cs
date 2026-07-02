using FakeItEasy;
using FluentAssertions;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.QuickAddUseCases;
using Renee.Application.Interfaces;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace CommandHandlerTests.QuickAddUseCases;

public class CreateCopropertyProfileWithQuickAddCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();
    private readonly IAccompanyingFileRepository _accompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();

    private static QuickAddCreatedAddress GetFakeAddress() => new(
        Label: "Résidence Les Lilas",
        PostalCode: "75018",
        City: "Paris",
        Department: "75",
        Region: "Île-de-France",
        Street: "Rue des Lilas",
        HouseNumber: "12",
        AdditionalAddress: "Bâtiment B"
    );

    private static QuickAddGeographicalAreaTypology GetFakeTypology() =>
        new(GeographicalHousingAreaTypology.Urban);

    private static QuickAddSupportTeam GetFakeSupportTeam(Guid? solidarBuilder = null) =>
        new(
            Type: MarkerNature.Association,
            TrustedTierLastName: "Dupont",
            TrustedTierStructureName: "SyndicPro",
            SolidarBuilder: solidarBuilder ?? Guid.NewGuid(),
            SolidarBuilderFullName: "Jean Dupont",
            TerritorialBuilder: Guid.NewGuid(),
            SecondTerritorialBuilder: Guid.NewGuid(),
            CommentOnMarkerNature: "Bon syndic",
            TrustedTierFirstName: "Jean",
            TrustedTierPhoneNumber: "0601020303",
            TrustedTierEmail: "jean.dupont@syndicpro.fr",
            TrustedTierRole: TrustedTierRole.Identifier,
            CommentOnTrustedTierRole: "Expérimenté",
            SecondSolidarBuilder: Guid.NewGuid(),
            ReferentDiffuseCoordinator: Guid.NewGuid(),
            ReferentTargetCoordinator: Guid.NewGuid(),
            ThirdSolidarBuilder: Guid.NewGuid()
        );

    private static QuickAddCreatedCopropertyProfileEntities GetFakeProfileEntities(Guid? solidarBuilder = null) =>
        new(
            HousingAddress: GetFakeAddress(),
            GeographicalAreaTypology: GetFakeTypology(),
            SupportTeam: GetFakeSupportTeam(solidarBuilder),
            RelatedAccompanyingFilesIds: []
        );

    [Fact]
    public async Task Handler_Should_ReturnNull_WhenSolidarBuilderIsEmpty()
    {
        // Arrange
        var profileEntities = GetFakeProfileEntities(Guid.Empty);
        var command = new CreateCopropertyProfileWithQuickAddCommandInput(
            ConnectedUserId: Guid.NewGuid(),
            CopropertyProfileEntities: profileEntities,
            ZeroEnergyExclusionTerritoriesProgram: false,
            AccompanyingType: AccompanyingType.Targeted,
            Territory: Guid.NewGuid()
        );
        var handler = new CreateCopropertyProfileWithQuickAddCommandHandler(_copropertyProfileRepository, _telemetryService, _accompanyingFileRepository);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handler_Should_ReturnNull_WhenAddCopropertyProfileReturnsLessThanOrEqualToOne()
    {
        // Arrange
        var profileEntities = GetFakeProfileEntities();
        A.CallTo(() => _copropertyProfileRepository.AddCopropertyProfile(A<CopropertyProfile>._)).Returns(1);

        var command = new CreateCopropertyProfileWithQuickAddCommandInput(
            ConnectedUserId: Guid.NewGuid(),
            CopropertyProfileEntities: profileEntities,
            ZeroEnergyExclusionTerritoriesProgram: true,
            AccompanyingType: AccompanyingType.Targeted,
            Territory: Guid.NewGuid()
        );
        var handler = new CreateCopropertyProfileWithQuickAddCommandHandler(_copropertyProfileRepository, _telemetryService, _accompanyingFileRepository);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task Handler_Should_ReturnId_WhenAddCopropertyProfileReturnsGreaterThanOne()
    {
        // Arrange
        var profileEntities = GetFakeProfileEntities();
        var expectedId = Guid.NewGuid();
        A.CallTo(() => _copropertyProfileRepository.AddCopropertyProfile(A<CopropertyProfile>._))
            .Invokes((CopropertyProfile cp) => cp.Id = expectedId)
            .Returns(2);

        var command = new CreateCopropertyProfileWithQuickAddCommandInput(
            ConnectedUserId: Guid.NewGuid(),
            CopropertyProfileEntities: profileEntities,
            ZeroEnergyExclusionTerritoriesProgram: true,
            AccompanyingType: AccompanyingType.Targeted,
            Territory: Guid.NewGuid()
        );
        var handler = new CreateCopropertyProfileWithQuickAddCommandHandler(_copropertyProfileRepository, _telemetryService, _accompanyingFileRepository);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Value.Should().Be(expectedId);
    }

    [Fact]
    public async Task Handler_Should_ReturnNull_WhenExceptionIsThrown()
    {
        // Arrange
        var profileEntities = GetFakeProfileEntities();
        A.CallTo(() => _copropertyProfileRepository.AddCopropertyProfile(A<CopropertyProfile>._))
            .Throws(new Exception("Database error"));

        var command = new CreateCopropertyProfileWithQuickAddCommandInput(
            ConnectedUserId: Guid.NewGuid(),
            CopropertyProfileEntities: profileEntities,
            ZeroEnergyExclusionTerritoriesProgram: false,
            AccompanyingType: AccompanyingType.Targeted,
            Territory: Guid.NewGuid()
        );
        var handler = new CreateCopropertyProfileWithQuickAddCommandHandler(_copropertyProfileRepository, _telemetryService, _accompanyingFileRepository);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Value.Should().BeNull();
    }

    [Fact]
    public void CreateReference_Should_ReturnExpectedFormat()
    {
        // Arrange
        var fullName = "Jean Dupont";
        var postalCode = "75018";
        var date = new DateTime(2024, 6, 1);

        // Act
        var reference = CreateCopropertyProfileWithQuickAddCommandHandler.CreateReference(fullName, postalCode, date);

        // Assert
        reference.Should().StartWith($"{fullName}-COPRO-");
        reference.Should().Contain(postalCode);
        reference.Should().EndWith("01/06/2024");
    }

    [Fact]
    public void Extractcharacter_Should_ReturnThreeRandomCharacters_WhenInputIsLongEnough()
    {
        // Arrange
        var input = "Jean Dupont";

        // Act
        var result = CreateCopropertyProfileWithQuickAddCommandHandler.Extractcharacter(input);

        // Assert
        result.Length.Should().Be(3);
        input.Replace(" ", "").ToCharArray().Should().Contain(result);
    }

    [Fact]
    public void Extractcharacter_Should_ReturnAllCharacters_WhenInputIsShort()
    {
        // Arrange
        var input = "Jo";

        // Act
        var result = CreateCopropertyProfileWithQuickAddCommandHandler.Extractcharacter(input);

        // Assert
        result.Should().Be("Jo");
    }
}