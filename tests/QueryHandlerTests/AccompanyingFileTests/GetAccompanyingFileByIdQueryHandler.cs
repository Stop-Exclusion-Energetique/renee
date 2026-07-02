using FakeItEasy;
using FluentAssertions;
using Renee.Application.Handlers.QueryHandlers.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace QueryHandlerTests.AccompanyingFileTests;

public class GetAccompanyingFileByIdQueryHandler
{
	private readonly IAccompanyingFileRepository _accompanyingFileRepository = A.Fake<IAccompanyingFileRepository>();
	private readonly ITelemetryService _telemetryService = A.Fake<ITelemetryService>();

	[Fact]
	public async Task Handler_Should_ReturnAccompanyingFile_WhenAccompanyingFileExists()
	{
		var accompanyingFileGuid = Guid.NewGuid();
		var copropertyProfileId = Guid.NewGuid();
		var copropertyAddressId = Guid.NewGuid();

		A.CallTo(() => _accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(A<Guid>._))
			.Returns(
				new AccompanyingFile
				{
					CopropertyProfileId = copropertyProfileId,
					CopropertyProfileNavigation = new CopropertyProfile
					{
						Id = copropertyProfileId,
						CopropertyReference = "CP-REF-001",
						CopropertyHousingNavigation = new CopropertyHousing
						{
							HousingAddressNavigation = new Address
							{
								Id = copropertyAddressId,
								Label = "10 rue de la Copro",
								City = "Lyon",
								Department = "69",
								PostalCode = "69001",
								Region = "Auvergne-Rhone-Alpes",
								AdditionnalComment = "Batiment C",
								HouseNumber = "10",
								Street = "rue de la Copro"
							}
						},
						CopropertyDiagnosticsNavigation = new CopropertyDiagnostics
						{
							BuildingDpeLabel = (int)DpeLabel.D,
							BuildingDpeEnergy = 180
						}
					},
					AccompanyingFileHousingNavigation =
						new Housing
						{
							Id = Guid.NewGuid(),
							GeographicAreaTypology = (int)GeographicalHousingAreaTypology.Urban,
							HousingType = (int)HousingType.IndividualHouse,
							NumberOfFloor = 1,
							HousingAddressNavigation = new Address { Id = Guid.NewGuid(), Label = "1 rue du dossier", City = "Paris", Department = "75", PostalCode = "75001", Region = "Ile-de-France" },
							HousingInitialStateNavigation = new HousingInitialState { Id = Guid.NewGuid(), Dpe = (int)DpeLabel.G, AnnualEnergyConsumption = 450 }
						},
					AccompanyingFileReference = "EM-75001-240226",
					AccompanyingFileHouseholdNavigation = new Household
					{
						Id = Guid.NewGuid(),
						MainOccupantNavigation = new MainOccupant(),
						HouseholdExpenses = new List<HouseholdExpense>(),
						HouseholdResources = new List<HouseholdResource>()
					}
				});

		var query = new GetAccompanyingFileByIdQuery(accompanyingFileGuid, Guid.NewGuid(), Constants.AdminRole);
		var handler = new GetAccompanyingFileQueryHandler(_accompanyingFileRepository, _telemetryService);

		var result = await handler.Handle(query, default);

		result.Should().NotBeNull();
		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value!.Housing.CopropertyProfileId.Should().Be(copropertyProfileId);
		result.Value.Housing.CopropertyProfileReference.Should().Be("CP-REF-001");
		result.Value.Housing.Address!.Id.Should().Be(copropertyAddressId);
		result.Value.Housing.Address.City.Should().Be("Lyon");
		result.Value.Housing.GeographicalTypology.Should().Be(GeographicalHousingAreaTypology.Urban);
		result.Value.Housing.HousingType.Should().Be(HousingType.IndividualHouse);
		result.Value.Housing.NumberOfFloors.Should().Be(1);
		result.Value.Housing.DpeLabel.Should().Be(DpeLabel.D);
		result.Value.Housing.AnnualEnergyConsumption.Should().Be(180);
	}

	[Fact]
	public async Task Handler_Should_ReturnNull_WhenAccompanyingFileIsUnknown()
	{
		var accompanyingFileGuid = Guid.NewGuid();

		A.CallTo(
			() => _accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(
				accompanyingFileGuid)).Returns<AccompanyingFile?>(null);

		var query = new GetAccompanyingFileByIdQuery(accompanyingFileGuid, Guid.NewGuid(), "");
		var handler = new GetAccompanyingFileQueryHandler(_accompanyingFileRepository, _telemetryService);

		var result = await handler.Handle(query, default);

		result.IsSuccess.Should().BeFalse();
		result.Message.Should().Be(Labels.Errors.ErrorWhileLoadingAccompanyingFile);
	}
}