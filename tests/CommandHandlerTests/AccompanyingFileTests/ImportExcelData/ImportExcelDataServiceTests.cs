using System.Collections.Generic;
using System.Globalization;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Services.Administration.ImportExcelData;
using Renee.Domain;
using Renee.Domain.Enums;

namespace CommandHandlerTests.AccompanyingFileTests.ImportExcelData;

public class ImportExcelDataServiceTests : TestContext
{
	public ImportExcelDataServiceTests()
	{
		_projectTypeService = A.Fake<IProjectTypeService>();
		_importExcelDataService = new ImportExcelDataService(_projectTypeService);
	}

	private readonly ImportExcelDataService _importExcelDataService;
	private readonly IProjectTypeService _projectTypeService;
	
	private readonly List<ExcelData> _requiredFields =
	[
		new(ExcelDataLabel.IdentificationMilestone.Reference, "Pierre ES_PV-QSD-75002-24/10/2023"),
		new(ExcelDataLabel.IdentificationMilestone.Milestone, "Jalon 1 - Identifier"),
		new(ExcelDataLabel.IdentificationMilestone.StreetNumberName, "8 rue de la paix"),
		new(ExcelDataLabel.IdentificationMilestone.PostalCode, "75002"),
		new(ExcelDataLabel.IdentificationMilestone.Municipality, "Commune"),
		new(ExcelDataLabel.IdentificationMilestone.Department, "75 - Paris"),
		new(ExcelDataLabel.IdentificationMilestone.Region, "Ile-de-France"),
		new(ExcelDataLabel.IdentificationMilestone.MarkerNature, "Opérateur"),
		new(ExcelDataLabel.IdentificationMilestone.FirstContactDate, "24/10/2023"),
		new(ExcelDataLabel.IdentificationMilestone.MainOccupant, "ABC"),
		new(ExcelDataLabel.IdentificationMilestone.MainOccupantDateOfBirth, "10/07/1968"),
		new(ExcelDataLabel.IdentificationMilestone.MainOccupantSocialProfessionalCategory, "Profession intermédiaire")
	];
	
	[Fact]
	public async Task MapExcelData_WhenDataAreGiven_MappingShouldBeDoneCorrectly()
	{
		// Arrange
		var diffuseCoordinatorId = Guid.NewGuid();
		var targetCoordinatorId = Guid.NewGuid();
		var territoryId = Guid.NewGuid();
		var territorialBuilderId = Guid.NewGuid();
		var solidarBuilderId = Guid.NewGuid();

		var expectedData = new ImportAccompanyingFileFromFileInputCommandInput(
			"Pierre ES_PV-QSD-75002-24/10/2023",
			DateTime.SpecifyKind(
				DateTime.ParseExact("24/10/2023", Labels.DateFormatUtc, CultureInfo.InvariantCulture),
				DateTimeKind.Utc).Date,
			DateTime.SpecifyKind(
				DateTime.ParseExact("12/11/2023", Labels.DateFormatUtc, CultureInfo.InvariantCulture),
				DateTimeKind.Utc).Date,
			DateTime.SpecifyKind(
				DateTime.ParseExact("25/11/2024", Labels.DateFormatUtc, CultureInfo.InvariantCulture),
				DateTimeKind.Utc).Date,
			DateTime.SpecifyKind(
				DateTime.ParseExact("12/07/2025", Labels.DateFormatUtc, CultureInfo.InvariantCulture),
				DateTimeKind.Utc).Date,
			MarkerNature.Association,
			null,
			5,
			null,
			null,
			true,
			AccompanyingType.Targeted,
			territoryId,
			new ImportSupportTeam(
				diffuseCoordinatorId,
				targetCoordinatorId,
				territorialBuilderId, 
				solidarBuilderId, 
				MarkerNature.Association, 
				null),
			new ImportHousehold(
				15000,
				HouseholdTypology.SingleParentFamily,
				"le contexte",
				false,
				"MO - modeste",
				new MainOccupant
				{
					Trigram = "ABC",
					Birthdate =
						DateTime.ParseExact("10/10/1968", Labels.DateFormatUtc, CultureInfo.InvariantCulture),
					SocioProfessionalCategory = 6
				},
				[]),
			new ImportHousing(
				1987,
				94,
				HousingType.IndividualHouse,
				GeographicalHousingAreaTypology.Urban,
				OwnershipStatus.FullOwnership,
				new Address(),
				new HousingInitialState(),
				new HousingAfterWorkState()),
			new ImportPreWorkPlan([], null, null, null, null, null, null, []),
			new ImportPreFinancingPlan(200, 300, null, null, null, null, null, null, null),
			new ImportWorkMonitoring(400, null, null, null, null, 6, null, null, null),
			new ImportInvoice(500, 600));

		var inputData = new List<ExcelData>
		{
			new(ExcelDataLabel.IdentificationMilestone.ContactWithFamily, "5"),
			new(ExcelDataLabel.IdentificationMilestone.Reference, "Pierre ES_PV-QSD-75002-24/10/2023"),
			new(ExcelDataLabel.IdentificationMilestone.FirstContactDate, "24/10/2023"),
			new(ExcelDataLabel.IdentificationMilestone.StartSupportDate, "12/11/2023"),
			new(ExcelDataLabel.IdentificationMilestone.StreetNumberName, "8 rue de la paix"),
			new(ExcelDataLabel.IdentificationMilestone.PostalCode, "75002"),
			new(ExcelDataLabel.IdentificationMilestone.Municipality, "Paris"),
			new(ExcelDataLabel.IdentificationMilestone.Department, "75 - Paris"),
			new(ExcelDataLabel.IdentificationMilestone.MainOccupant, "ABC"),
			new(ExcelDataLabel.IdentificationMilestone.Region, "Ile-de-France"),
			new(ExcelDataLabel.IdentificationMilestone.MainOccupantDateOfBirth, "10/07/1968"),
			new(ExcelDataLabel.IdentificationMilestone.MainOccupantSocialProfessionalCategory, "Profession intermédiaire"),
			new(ExcelDataLabel.IdentificationMilestone.HouseholdTypology, "Famille monoparentale"),
			new(ExcelDataLabel.OrganizeAndFinanceMilestone.RegionAids, "200"),
			new(ExcelDataLabel.OrganizeAndFinanceMilestone.DepartmentAids, "300"),
			new(ExcelDataLabel.RealizeAndFollowMilestone.HouseholdAutoFinancing, "400"),
			new(ExcelDataLabel.RealizeAndFollowMilestone.TotalCost, "500"),
			new(ExcelDataLabel.RealizeAndFollowMilestone.BilledWorkForce, "600"),
			new(ExcelDataLabel.RealizeAndFollowMilestone.WellBeingRating, "6")
		};

		var requiredFieldsFromPage = new RequiredFieldsFromDataImportPage(
			"Nom",
			"Prénom",
			true,
			AccompanyingType.Targeted,
			targetCoordinatorId,
			territorialBuilderId,
			territoryId,
			diffuseCoordinatorId,
			solidarBuilderId);

		// Act
		var result = await _importExcelDataService.MapExcelData(inputData, requiredFieldsFromPage);

		// Assert
		result?.Reference.Should().Be(expectedData.Reference);
		result?.FirstContactDate.Should().Be(expectedData.FirstContactDate);
		result?.StartSupportDate.Should().Be(expectedData.StartSupportDate);
		result?.Household.HouseholdTypology.Should().Be(expectedData.Household.HouseholdTypology);
		result?.PreFinancingPlan.RegionAids.Should().Be(expectedData.PreFinancingPlan.RegionAids);
		result?.PreFinancingPlan.DepartmentAids.Should().Be(expectedData.PreFinancingPlan.DepartmentAids);
		result?.Invoice.InvoiceCost.Should().Be(expectedData.Invoice.InvoiceCost);
		result?.Invoice.LaborCost.Should().Be(expectedData.Invoice.LaborCost);
		result?.WorkMonitoring.WellBeingRating.Should().Be(expectedData.WorkMonitoring.WellBeingRating);
	}

	[Fact]
	public void ValidateData_WhenDataAreNotValid_ShouldReturnFailure()
	{
		// Arrange
		var dataToValidate = new List<ExcelData>();

		// Act
		var result = _importExcelDataService.ValidateData(dataToValidate,false);

		// Assert
		result.IsSuccess.Should().BeFalse();
	}

	[Fact]
	public void ValidateData_WhenDataAreValid_ShouldReturnSuccess()
	{
		// Arrange
		var dataToValidate = _requiredFields;

		// Act
		var result = _importExcelDataService.ValidateData(dataToValidate,false);

		// Assert
		result.IsSuccess.Should().BeTrue();
	}
}