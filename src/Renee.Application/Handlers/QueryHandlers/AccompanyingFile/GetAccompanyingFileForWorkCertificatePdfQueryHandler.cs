using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.DTOs.WorkPackageWorkTypeCost;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Helpers;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileForWorkCertificatePdfQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService) : QueryHandler<GetAccompanyingFileForWorkCertificatePdfQuery, ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>> HandleQuery(
		GetAccompanyingFileForWorkCertificatePdfQuery request)
	{
		try
		{
			var accompanyingfile = await accompanyingFileRepository.GetAccompanyingFileDataForWorkCertificatePdf(
					request.AccompanyingFileReference);

			if (accompanyingfile is null)
				return ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>.Failure(GenerateFilesLabels.Errors.AccompanyingFileNotFound);

			var accompanyingFileCreator = GetAccompanyingFileCreator(accompanyingfile, request.ConnectedUserId);

			if (accompanyingFileCreator == null)
				return ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>.Failure(GenerateFilesLabels.Errors.AccompanyingFileIsNotValid);

			return ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>.Success(new GetAccompanyingFileForWorkCertificatePdfQueryObjectResult
			{
				OccupantFullName = $"{accompanyingfile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName?.ToUpperInvariant()} {accompanyingfile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName}",
				StreetNumber = accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.HouseNumber ?? string.Empty,
				StreetName = accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label,
				PostalCode = accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode,
				City = accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.City,
				WorkPackages = accompanyingfile.AccompanyingFilePreWorkPlanNavigation.WorkPackages.Select(wp =>
					new WorkPackageDto(wp.Id, wp.EnergeticsEffectAfterWorks, wp.WorkPackageWorkTypeCosts.Select(wt =>
					new WorkPackageWorkTypeCostDto(wt.WorkType, wt.Cost, wt.Description)).ToList())).ToList(),
				WorkPackagesTotalCostIncludingAllTaxes = StatisticsHelper.CalculateTotalWorkPackageCosts(accompanyingfile),
				EstimatedAnnualGesEmissionsBeforeWork = accompanyingfile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.AnnualGesemission,
				AnnualEnergyConsumptionBeforeWork = accompanyingfile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.AnnualEnergyConsumption,
				EstimatedEnergyDpeBeforeWork = ((DpeLabel?)accompanyingfile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe).GetDescription(),
				Surface = accompanyingfile.AccompanyingFileHousingNavigation.LivingSpace,
				EstimatedAnnualGesEmissionsAfterWork = accompanyingfile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedAnnualGesemissionsAfterWork,
				AnnualEnergyConsumptionAfterWork = accompanyingfile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedAnnualEnergyConsumptionAfterWork,
				EstimatedEnergyDpeAfterWork = ((DpeLabel?)accompanyingfile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeafterWork).GetDescription(),
				EstimatedDpeClassJump = accompanyingfile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeclassJump,
				SolidarBuilderFullName = accompanyingfile.CreatedByNavigation?.Role.Name != Constants.SolidarBuilderRole
						? $"{accompanyingFileCreator.LastName?.ToUpperInvariant()} {accompanyingFileCreator.FirstName}"
						: $"{accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.LastName?.ToUpperInvariant()} {accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.FirstName}",
				SocialContext = accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation?.Name,
				SiretNumber = accompanyingFileCreator.SiretNumber ?? string.Empty
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAccompanyingFileForWorkCertificatePdfQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static Domain.Entity.User? GetAccompanyingFileCreator(
		Domain.Entity.AccompanyingFile accompanyingfile,
		Guid connectedUserId)
	{
		if (accompanyingfile.CreatedByNavigation?.Role.Name == Constants.StructuralReferentRole)
			return accompanyingfile.CreatedByNavigation;

		if (accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilder == connectedUserId)
			return accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation;

		if (accompanyingfile.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder is not null &&
			accompanyingfile.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == connectedUserId)
			return accompanyingfile.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation;

		if (accompanyingfile.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder is not null &&
			accompanyingfile.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == connectedUserId)
			return accompanyingfile.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation;

		return null;
	}
}