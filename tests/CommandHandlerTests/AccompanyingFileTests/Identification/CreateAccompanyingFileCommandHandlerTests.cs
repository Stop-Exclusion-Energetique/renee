using System.Globalization;
using FakeItEasy;
using FluentAssertions;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.QuickAddUseCases;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace CommandHandlerTests.AccompanyingFileTests.Identification;

public class CreateAccompanyingFileCommandHandlerTests
{
	private readonly IAccompanyingFileRepository _accompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
	private readonly IAdminConstantRepository _adminConstantRepository = A.Fake<IAdminConstantRepository>();
	private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

	[Fact]
	public void GenerateReference_ShouldReturnTheCorrectReference()
	{
		var reference = AccompanyingFileHelper.CreateReference(
			"Michael SCOTT",
			"NDU",
			"75018");

		reference.Should().Be($"Michael SCOTT-NDU-75018-{DateTime.Now:dd/MM/yyyy}");
	}

	[Fact]
	public async Task Handler_Should_CallAddAccompanyingFileOnRepository_WhenEntityIsValid()
	{
		//Arrange
		A.CallTo(() => _accompanyingFileRepository.AddAccompanyingFile(A<AccompanyingFile>._)).Returns(1);
		A.CallTo(() => _adminConstantRepository.GetAll()).Returns([
			new AdminConstant
			{
				Name = "Nombre maximal de dossiers pouvant être créés",
				Value = 10
			}
		]);

		var command = new CreateAccompanyingFileWithQuickAddCommandInput(
			new QuickAddCreatedAccompanyingFileEntities(
				new QuickAddCreatedOccupant("TZU", "John", "Doe"),
				new QuickAddCreatedAddress(
					"Label",
					"Postal code",
					"City",
					"department",
					"region",
					"street",
					"housenumber"),
				new QuickAddGeographicalAreaTypology(GeographicalHousingAreaTypology.Urban),
				new QuickAddSupportTeam(
					MarkerNature.Association,
					"LastName",
					"Structure",
					Guid.NewGuid(),
					"FullName",
					Guid.NewGuid(),
                    Guid.NewGuid(),
                    null,
					null,
					null,
					null,
					null,
					null,
					Guid.NewGuid(),
					Guid.NewGuid(),
					null,
					Guid.NewGuid()),
				new QuickAddHousingPropertyType(HousingType.IndividualHouse, null)),
			Guid.NewGuid(),
			true,
			AccompanyingType.Diffuse,
			Guid.NewGuid());
		var handler = new CreateAccompanyingFileWithQuickAddCommandHandler(_accompanyingFileRepository, _adminConstantRepository, _telemetryService);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _accompanyingFileRepository.AddAccompanyingFile(A<AccompanyingFile>._)).MustHaveHappened();
	}

	[Fact]
	public async Task Handler_Should_ReturnNull_WhenAccompanyingFileHasNoAffectedUserId()
	{
		//Arrange
		A.CallTo(() => _adminConstantRepository.GetAll()).Returns([
			new AdminConstant
			{
				Name = "Nombre maximal de dossiers pouvant être créés",
				Value = 10
			}
		]);

		var command = new CreateAccompanyingFileWithQuickAddCommandInput(
			new QuickAddCreatedAccompanyingFileEntities(
				new QuickAddCreatedOccupant("TZU", "John", "Doe"),
				new QuickAddCreatedAddress(
					"Label",
					"Postal code",
					"City",
					"department",
					"region",
					"street",
					"housenumber"),
				new QuickAddGeographicalAreaTypology(GeographicalHousingAreaTypology.Urban),
				new QuickAddSupportTeam(
					MarkerNature.Association,
					"LastName",
					"Structure",
					Guid.NewGuid(),
					"FullName",
					Guid.NewGuid(),
					Guid.NewGuid(),
					null,
					null,
					null,
					null,
					null,
					null,
					Guid.NewGuid(),
					Guid.NewGuid(),
					null,
					Guid.NewGuid()),
				new QuickAddHousingPropertyType(HousingType.IndividualHouse, null)),
			Guid.NewGuid(),
			true,
			AccompanyingType.Diffuse,
			Guid.NewGuid());
		var handler = new CreateAccompanyingFileWithQuickAddCommandHandler(_accompanyingFileRepository, _adminConstantRepository, _telemetryService);

		//Act
		var result = await handler.Handle(command, default);

		//Assert
		result.Value.Should().BeNull();
	}

	[Fact]
	public async Task Handler_ShouldNot_CallAddAccompanyingFileOnRepository_WhenAccompanyingFileHasNoAffectedUserId()
	{
		//Arrange
		var accompanyingFile = new AccompanyingFile();
		A.CallTo(() => _adminConstantRepository.GetAll()).Returns([
			new AdminConstant
			{
				Name = "Nombre maximal de dossiers pouvant être créés",
				Value = 10
			}
		]);
		var command = new CreateAccompanyingFileWithQuickAddCommandInput(
			new QuickAddCreatedAccompanyingFileEntities(
				new QuickAddCreatedOccupant("TZU", "John", "Doe"),
				new QuickAddCreatedAddress(
					"Label",
					"Postal code",
					"City",
					"department",
					"region",
					"street",
					"housenumber"),
				new QuickAddGeographicalAreaTypology(GeographicalHousingAreaTypology.Urban),
				new QuickAddSupportTeam(
					MarkerNature.Association,
					"LastName",
					"Structure",
					Guid.NewGuid(),
					"FullName",
					Guid.NewGuid(),
					Guid.NewGuid(),
					null,
					null,
					null,
					null,
					null,
					null,
					Guid.NewGuid(),
					Guid.NewGuid(),
					null,
					Guid.NewGuid()),
				new QuickAddHousingPropertyType(HousingType.IndividualHouse, null)),
			Guid.NewGuid(),
			true,
			AccompanyingType.Diffuse,
			Guid.NewGuid());
		var handler = new CreateAccompanyingFileWithQuickAddCommandHandler(_accompanyingFileRepository, _adminConstantRepository, _telemetryService);

		//Act
		await handler.Handle(command, default);

		//Assert
		A.CallTo(() => _accompanyingFileRepository.AddAccompanyingFile(accompanyingFile)).MustNotHaveHappened();
	}
}
