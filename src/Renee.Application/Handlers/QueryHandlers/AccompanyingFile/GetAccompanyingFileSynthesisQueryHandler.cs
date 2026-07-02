using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.AccompanyingFileHouseholdResourceTypology;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileSynthesisQueryHandler(
	IAccompanyingFileRepository accompanyingFileForSynthesisRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAccompanyingFileSynthesisQuery, ReneeOperationResult<AccompanyingFileSynthesisDto>>
{
	public async Task<ReneeOperationResult<AccompanyingFileSynthesisDto>> Handle(
		GetAccompanyingFileSynthesisQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			if (request.Id is null) return ReneeOperationResult<AccompanyingFileSynthesisDto>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			var accompanyingFileEntity =
				await accompanyingFileForSynthesisRepository.GetAccompanyingFileByIdForIdentificationMilestoneAsync(
					(Guid)request.Id);

			if (accompanyingFileEntity is null) return ReneeOperationResult<AccompanyingFileSynthesisDto>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			return ReneeOperationResult<AccompanyingFileSynthesisDto>.Success(MapToSynthesisDto(accompanyingFileEntity));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<AccompanyingFileSynthesisDto>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private AccompanyingFileSynthesisDto MapToSynthesisDto(Domain.Entity.AccompanyingFile accompanyingFileEntity)
	{
		var mainOccupantAccompanyingFile =
			accompanyingFileEntity.AccompanyingFileHouseholdNavigation.MainOccupantNavigation;

		var resourcesTotalValue = GetResourcesTotalValue(accompanyingFileEntity);
		var expensesTotalValue = GetExpensesTotalValue(accompanyingFileEntity);
		var energeticExpensesTotalValue = GetEnergeticExpensesTotalValue(accompanyingFileEntity);
		var casualExpensesTotalValue = GetCasualExpensesTotalValue(accompanyingFileEntity);
		var mar = AccompanyingFileHelper.GetMar(
			accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DegradationIndex is null
				? null
				: (DegradationIndex?)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DegradationIndex,
			accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.UnsanitaryCoefficient is null
				? null
				: (UnsanitaryCoefficient?)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.UnsanitaryCoefficient
		);

		return new AccompanyingFileSynthesisDto
		{
			Id = accompanyingFileEntity.Id,
			Trigram = mainOccupantAccompanyingFile.Trigram,
			Name = mainOccupantAccompanyingFile.LastName,
			FirstName = mainOccupantAccompanyingFile.FirstName,
			PhoneNumber = mainOccupantAccompanyingFile.PhoneNumber,
			Age = mainOccupantAccompanyingFile.Age,
			SocioProfessionalCategory =
				mainOccupantAccompanyingFile.SocioProfessionalCategory is null
					? null
					: (SocioProfessionalCategory)mainOccupantAccompanyingFile.SocioProfessionalCategory,
			NumberOfOccupants =
				1 + accompanyingFileEntity.AccompanyingFileHouseholdNavigation.SecondaryOccupants.Count,
			HouseholdTypology =
				accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdTypology is null
					? null
					: (HouseholdTypology)accompanyingFileEntity.AccompanyingFileHouseholdNavigation
						.HouseholdTypology,
			IsOverIndebted =
				DecimalHelper.AlmostEquals(resourcesTotalValue, 0) &&
				casualExpensesTotalValue / resourcesTotalValue > Constants.OverIndebtednessRate,
			HasDisabilitySituation =
				accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HasAnOccupantWithDisabilities,
			HasPersonWithLongTermIllness =
				accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HasAnOccupantWithLongTermIllness,
			HasPersonWithLossOfIndependence =
				accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HasAnOccupantWithIndependenceLoss,
			HasPersonFollowedByCuratorship =
				accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HasAnOccupantUnderCuratorship,
			HasPersonFollowedByGuardianship =
				accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HasAnOccupantUnderGuardianship,
			TaxIncome = (int)(accompanyingFileEntity.AccompanyingFileHouseholdNavigation.ReferenceIncomeTax ?? 0),
			AnahCategory = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.AnahCategory,
			AvailableBudget = resourcesTotalValue - expensesTotalValue,
			EnergeticTotal = energeticExpensesTotalValue,
			EnergyEffortRate = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.EnergyEffortRate,
			Address = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label,
			PostalCode = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode,
			City = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingAddressNavigation.City,
			HousingType =
				accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingType is null
					? null
					: (HousingType)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingType,
			OwnershipStatus =
				accompanyingFileEntity.AccompanyingFileHousingNavigation.OwnershipStatus is null
					? null
					: (OwnershipStatus)accompanyingFileEntity.AccompanyingFileHousingNavigation.OwnershipStatus,
			LivingSpaceInSquareMeter = accompanyingFileEntity.AccompanyingFileHousingNavigation.LivingSpace,
			BuildingYear = accompanyingFileEntity.AccompanyingFileHousingNavigation.ConstructionYear is null ?
					string.Empty :
					EnumHelper.GetDescription((HousingYearConstruction)accompanyingFileEntity.AccompanyingFileHousingNavigation.ConstructionYear),
			DegradationIndex = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DegradationIndex is null ?
				null :
				(DegradationIndex)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DegradationIndex,
			UnsanitaryCoefficient = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.UnsanitaryCoefficient is null ?
				null :
				(UnsanitaryCoefficient)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.UnsanitaryCoefficient,
			Mar = mar,
			DpeLabel =
				accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe is null ?
				null
				: ((DpeLabel)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe).GetDescription(),
			EnergyConsumption = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.AnnualEnergyConsumption,
			GesLabel = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Ges is null ?
				null
				: ((GesLabel)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Ges).GetDescription(),
			GesEmissions = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.AnnualGesemission,
			ElectricityDeprivation = accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.EnergyDepravation is null ?
				null
				: (EnergyDeprivation)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.EnergyDepravation,
			DifficultiesFacedByFamily = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdDifficulties.Select(d => d.DifficultyNavigation.Labels).ToList(),
			SocialContext = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.SocialContext,
			FamilyProject = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdProject,
			CommentsOnHouseholdDifficulties = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.CommentsOnHouseholdDifficulties,
			IsFollowedBySocialWorker = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.IsFollowedByAnSocialWorker,
			HouseholdResourcesTypologies = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdResources
				.Select(r => new AccompanyingFileHouseholdResourcesTypologyDto
				{
					HouseholdResourcesTypologyId = r.HouseholdResources,
					Name = r.HouseholdResourcesNavigation.Labels,
					Value = r.Value
				}).ToList(),
			HouseholdHeatingEnergies = accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdHeatingEnergies
				.Select(h => h.HouseholdHeatingEnergyNavigation.Name).ToList(),
			AccompanyingFileReference = accompanyingFileEntity.AccompanyingFileReference,
			Stage = (AccompanyingFileStage)accompanyingFileEntity.AccompanyingFileMilestone,
			Status = (AccompanyingFileStatus)accompanyingFileEntity.AccompanyingFileStatus,
			MarkerNature = (MarkerNature)accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.MarkerNature,
			SolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SolidarBuilder,
			SecondSolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder,
			ThirdSolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder,
			TargetedCoordinator = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.TargetCoordinator,
			DiffuseCoordinator = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator,
			TerritorialBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.TerritorialBuilder,
			SecondTerritorialBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder,
			GeographicAreaTypology = accompanyingFileEntity.AccompanyingFileHousingNavigation.GeographicAreaTypology is null ?
				null
				: (GeographicalHousingAreaTypology)accompanyingFileEntity.AccompanyingFileHousingNavigation.GeographicAreaTypology
		};
	}

	private static double GetCasualExpensesTotalValue(Domain.Entity.AccompanyingFile accompanyingFileEntity) =>
		accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdExpenses.Where(
				o => o.Type is (int)ExpenseType.CreditRepayment
					or (int)ExpenseType.Children
					or (int)ExpenseType.Housing)
			.Sum(o => o.Value ?? 0);

	private static double GetEnergeticExpensesTotalValue(Domain.Entity.AccompanyingFile accompanyingFileEntity) =>
		accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdExpenses
			.Where(o => o.Type is (int)ExpenseType.MonthlyEnergecticsExpenses).Sum(o => o.Value ?? 0);

	private static double GetExpensesTotalValue(Domain.Entity.AccompanyingFile accompanyingFileEntity) =>
		accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdExpenses.Sum(e => e.Value ?? 0);

	private static double GetResourcesTotalValue(Domain.Entity.AccompanyingFile accompanyingFileEntity) =>
		accompanyingFileEntity.AccompanyingFileHouseholdNavigation.HouseholdResources.Sum(r => r.Value);
}
