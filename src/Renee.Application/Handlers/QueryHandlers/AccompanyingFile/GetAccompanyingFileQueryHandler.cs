using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.AccompanyingFileDifficultyFacedByFamily;
using Renee.Application.DTOs.AccompanyingFileHouseholdHeatingEnergy;
using Renee.Application.DTOs.AccompanyingFileHouseholdResourceTypology;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;
using ExpenseDto = Renee.Application.DTOs.Expense.ExpenseDto;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAccompanyingFileByIdQuery, ReneeOperationResult<AccompanyingFileDto?>>
{
	public async Task<ReneeOperationResult<AccompanyingFileDto?>> Handle(
		GetAccompanyingFileByIdQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync((Guid)request.AccompanyingFileId!);

			if (accompanyingFile is null)
				return ReneeOperationResult<AccompanyingFileDto?>.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFile);

			if (IsUserInSupportTeam(accompanyingFile, request.UserId) || IsUserACoordinatorOrAdmin(request.UserRole))
				return ReneeOperationResult<AccompanyingFileDto?>.Success(ToAccompanyingFileDto(accompanyingFile));

			return ReneeOperationResult<AccompanyingFileDto?>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<AccompanyingFileDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static bool IsUserInSupportTeam(Domain.Entity.AccompanyingFile accompanyingFile, Guid userId)
		=> userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.TerritorialBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SecondTerritorialBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SolidarBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SecondSolidarBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.ThirdSolidarBuilder ;

	private static bool IsUserACoordinatorOrAdmin(string userRole)
		=> userRole.Equals(Constants.DiffuseCoordinatorRole) ||
			userRole.Equals(Constants.TargetedCoordinatorRole) ||
			userRole.Equals(Constants.AdminRole);

	private static List<AccompanyingFileDifficultyFacedByFamilyDto> MapAccompanyingFileDifficultyFacedByFamiliesDto(
		Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdDifficulties.Select(
				df => new AccompanyingFileDifficultyFacedByFamilyDto { DifficultyFacedByFamilyId = df.Difficulty })
			.ToList();
	}

	private static List<AccompanyingFileHouseholdResourcesTypologyDto>
		MapAccompanyingFileHouseholdResourcesTypologiesDto(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdResources.Select(
			hrt => new AccompanyingFileHouseholdResourcesTypologyDto
			{
				HouseholdResourcesTypologyId = hrt.HouseholdResources,
				Value = hrt.Value,
				Name = hrt.HouseholdResourcesNavigation.Labels
			}).ToList();
	}

	private static List<AccompanyingFileHouseholdHeatingEnergyDto> MapAccompanyingFileHouseholdHeatingEnergyDtos(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdHeatingEnergies.Select(
			hhe => new AccompanyingFileHouseholdHeatingEnergyDto
			{
				HouseholdHeatingEnergyId = hhe.HouseholdHeatingEnergyLabel,
				Value = hhe.Value,
				Name = hhe.HouseholdHeatingEnergyNavigation.Name
			}).ToList();
	}

	private static List<ExpenseDto> MapExpensesDto(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdExpenses.Select(
			e => new ExpenseDto { Id = e.Id, Type = (ExpenseType)e.Type, Value = e.Value }).ToList();
	}

	private static AddressDto MapAddressDto(Domain.Entity.Address address)
	{
		return new AddressDto
		{
			Id = address.Id,
			City = address.City,
			Department = address.Department,
			Region = address.Region,
			PostalCode = address.PostalCode,
			Name = address.Label,
			Label = address.Label,
			AdditionalAddress = address.AdditionnalComment,
			HouseNumber = address.HouseNumber,
			Street = address.Street
		};
	}

	private static HousingDto MapHousingDto(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		var housing = accompanyingFile.AccompanyingFileHousingNavigation;
		var housingInitialState = housing.HousingInitialStateNavigation;
		var copropertyProfile = accompanyingFile.CopropertyProfileNavigation;
		var copropertyHousing = copropertyProfile?.CopropertyHousingNavigation;
		var copropertyDiagnostics = copropertyProfile?.CopropertyDiagnosticsNavigation;
		var address = copropertyHousing?.HousingAddressNavigation ?? housing.HousingAddressNavigation;

		return new HousingDto
		{
			Id = housing.Id,
			CopropertyProfileId = accompanyingFile.CopropertyProfileId,
			CopropertyProfileReference = copropertyProfile?.CopropertyReference,
			Address = MapAddressDto(address),
			GeographicalTypology =
				housing.GeographicAreaTypology is null
					? null
					: (GeographicalHousingAreaTypology)housing.GeographicAreaTypology,
			AcquisitionYear = housing.YearOfAcquisitionOrEntry,
			AnnualEnergyConsumption = copropertyDiagnostics?.BuildingDpeEnergy is not null
				? copropertyDiagnostics.BuildingDpeEnergy
				: housingInitialState.AnnualEnergyConsumption,
			AnnualGesEmissions = housingInitialState.AnnualGesemission,
			ArchitecturalOrUrbanismStandard =
				housing.ArchitecturalOrTownPlanningStandards,
			BuildingYear = housing.ConstructionYear is null
            ? null
			: (HousingYearConstruction)housing.ConstructionYear,
			CadastralReference = housing.CadastralReference,
			DegradationIndex =
				(DegradationIndex?)housingInitialState.DegradationIndex,
			UnsanitaryCoefficient =
				(UnsanitaryCoefficient?)housingInitialState.UnsanitaryCoefficient,
			DpeLabel = copropertyDiagnostics?.BuildingDpeLabel is not null
				? (DpeLabel)copropertyDiagnostics.BuildingDpeLabel
				: housingInitialState.Dpe is not null
					? (DpeLabel)housingInitialState.Dpe
					: null,
			GesLabel = (GesLabel?)housingInitialState.Ges,
			ElectricityDeprivation =
				housingInitialState.EnergyDepravation is null
					? null
					: (EnergyDeprivation)housingInitialState.EnergyDepravation,
			ExplanationOnPreviousBuildingWork =
				housing.CommentOnPreviousWork,
			HasPreviousBuildingWork = housing.HasPreviousWork,
			HousingType =
				housing.HousingType is null
					? null
					: (HousingType)housing.HousingType,
			IsAbfZone = housing.IsInAbfarea,
			LivingSpaceInSquareMeter = housing.LivingSpace,
			NoiseComfortLevel =
				housingInitialState.NoiseComfortLevel is null
					? null
					: (ComfortLevel)housingInitialState.NoiseComfortLevel,
			NumberOfFloors = housing.NumberOfFloor,
			NumberOfRooms = housing.NumberOfRoom,
			OwnershipStatus =
				housing.OwnershipStatus is null
					? null
					: (OwnershipStatus)housing.OwnershipStatus,
			SummerThermalComfortLevel =
				housingInitialState.SummerThermalComfortLevel is null
					? null
					: (ComfortLevel)housingInitialState.SummerThermalComfortLevel,
			WinterThermalComfortLevel =
				housingInitialState.WinterThermalComfortLevel is null
					? null
					: (ComfortLevel)housingInitialState.WinterThermalComfortLevel
		};
	}

	private static MainOccupantDto MapOccupantsDto(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		var mainOccupant = accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation;
		if (mainOccupant is not null)
			return new MainOccupantDto
			{
				Id = mainOccupant.Id,
				Trigram = mainOccupant.Trigram,
				Email = mainOccupant.Email,
				PhoneNumber = mainOccupant.PhoneNumber,
				Age = mainOccupant.Age,
				Birthdate = mainOccupant.Birthdate,
				ComplementaryFund =
					mainOccupant.AdditionnalFund is null ? null : (AdditionalFund)mainOccupant.AdditionnalFund,
				Job = mainOccupant.Job,
				OtherComplementaryFund = mainOccupant.CommentOnAdditionnalFund,
				OtherPensionFund = mainOccupant.CommentOnPensionFund,
				OtherSocialWelfareFund = mainOccupant.CommentOnSocialProtectionFund,
				PensionFund = mainOccupant.PensionFund is null ? null : (PensionFund)mainOccupant.PensionFund,
				SocialWelfareFund =
					mainOccupant.SocialProtectionFund is null
						? null
						: (SocialProtectionFund)mainOccupant.SocialProtectionFund,
				SocioProfessionalCategoryId = mainOccupant.SocioProfessionalCategory is null
					? null
					: (SocioProfessionalCategory)mainOccupant.SocioProfessionalCategory,
				FirstName = mainOccupant.FirstName,
				LastName = mainOccupant.LastName
			};

		return new MainOccupantDto();
	}

	private static List<SecondaryOccupantDto> MapSecondaryOccupantsDto(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return accompanyingFile.AccompanyingFileHouseholdNavigation.SecondaryOccupants.Select(
			so => new SecondaryOccupantDto
			{
				Id = so.Id,
				Trigram = so.Trigram,
				Birthdate = so.Birthdate,
				Age = so.Age,
				IsDependent = so.IsDependent
			}).ToList();
	}

	private static AccompanyingFileDto ToAccompanyingFileDto(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return new AccompanyingFileDto
		{
			Id = accompanyingFile.Id,
			Reference = accompanyingFile.AccompanyingFileReference,
			AnahCategory = accompanyingFile.AccompanyingFileHouseholdNavigation.AnahCategory,
			CommentOnDifficultiesFacedByFamily =
				accompanyingFile.AccompanyingFileHouseholdNavigation.CommentsOnHouseholdDifficulties,
			FamilyAvailabilityForVisits =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdAvailabilityForVisits,
			FamilyProject = accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdProject,
			HasDisabilitySituation =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HasAnOccupantWithDisabilities,
			HasPersonFollowedByCuratorship =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HasAnOccupantUnderCuratorship,
			HasPersonFollowedByGuardianship =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HasAnOccupantUnderGuardianship,
			HasPersonWithLongTermIllness =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HasAnOccupantWithLongTermIllness,
			HasPersonWithLossOfIndependence =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HasAnOccupantWithIndependenceLoss,
			IsFollowedBySocialWorker =
				accompanyingFile.AccompanyingFileHouseholdNavigation.IsFollowedByAnSocialWorker,
			SocialContext = accompanyingFile.AccompanyingFileHouseholdNavigation.SocialContext,
			TaxIncome =
				(int?)accompanyingFile.AccompanyingFileHouseholdNavigation.ReferenceIncomeTax,
			Housing = MapHousingDto(accompanyingFile),
			HouseholdTypologyId =
				accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdTypology is null
					? null
					: (HouseholdTypology)accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdTypology,
			Stage = (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone,
			MainOccupant = MapOccupantsDto(accompanyingFile),
			SecondaryOccupants = MapSecondaryOccupantsDto(accompanyingFile),
			Expenses = MapExpensesDto(accompanyingFile),
			DifficultiesFacedByFamily = MapAccompanyingFileDifficultyFacedByFamiliesDto(accompanyingFile),
			HouseholdResourcesTypologies = MapAccompanyingFileHouseholdResourcesTypologiesDto(accompanyingFile),
			HouseholdHeatingEnergies = MapAccompanyingFileHouseholdHeatingEnergyDtos(accompanyingFile),
			Status = (AccompanyingFileStatus)accompanyingFile.AccompanyingFileStatus,
			BlockingProofComment = accompanyingFile.CommentOnBlockingProof,
			AccompanyingTimeDuration = (AccompanyingTimeDuration?)accompanyingFile.AccompanyingTimeDurationForIdentificationMilestone,
			HasUnpaidEnergyBills = accompanyingFile.AccompanyingFileHouseholdNavigation.HasOverdueInvoice,
			FirstVisitDate = accompanyingFile.FirstEncounterDate,
			SigningHouseholdSupportDate = accompanyingFile.StartOfAccompanyingDate,
			IsInTzeeProgram = accompanyingFile.ZeroEnergyExclusionTerritoriesProgram ?? false,
			ReportingStructureName = accompanyingFile.AccompanyingFileSupportTeamNavigation?.SolidarBuilderNavigation?.ReportingStructureNavigation?.Name,
			IsImported = accompanyingFile.ImportRunId != null,
			ShouldAccompanyingFileBeSubmittedToAnah = accompanyingFile.ShouldAccompanyingFileBeSubmittedToAnah
		};
	}
}