using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileForAnahSynthesisPdfQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAccompanyingFileForAnahSynthesisPdfQuery, ReneeOperationResult<GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>> HandleQuery(
		GetAccompanyingFileForAnahSynthesisPdfQuery request)
	{
		try
		{
			var accompanyingfile =
				await accompanyingFileRepository.GetAccompanyingFileDataForAnahSynthesisPdf(
					request.AccompanyingFileReference);

			if (accompanyingfile is null)
				return ReneeOperationResult<GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>.Failure(GenerateFilesLabels.Errors.AccompanyingFileNotFound);

			var projectTypes = accompanyingfile.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes.ToList();

			var accompanyingFileCreator = GetAccompanyingFileCreator(accompanyingfile, request.ConnectedUserId);

			if (accompanyingFileCreator is null)
				return ReneeOperationResult<GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>.Failure(GenerateFilesLabels.Errors.AccompanyingFileIsNotValid);

			return ReneeOperationResult< GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>.Success(new GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult
			{
				StreetNumber =
					accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.HouseNumber ??
					string.Empty,
				StreetName =
					accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.Street ?? string.Empty,
				PostalCode =
					accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode,
				City = accompanyingfile.AccompanyingFileHousingNavigation.HousingAddressNavigation.City,
				FirstVisitDate = accompanyingfile.FirstEncounterDate,
				HasHousingAdaptationWork =
					accompanyingfile.AccompanyingFileWorkMonitoringNavigation?.HasHousingAdaptationWorks ??
					false,
				HasUnsanitaryExit =
					accompanyingfile.AccompanyingFileWorkMonitoringNavigation?.HasUnsanitaryExit ?? false,
				SolidarBuilderFullName =
					accompanyingfile.CreatedByNavigation?.Role.Name != Constants.SolidarBuilderRole
						? $"{accompanyingFileCreator.FirstName} {accompanyingFileCreator.LastName}"
						: $"{accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.FirstName} {accompanyingfile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.LastName}",
				HasProjectTypeDegradation = HasProjectType(projectTypes, GenerateFilesLabels.AnahSynthesisLabel.Degradation),
				HasProjectTypeIsolation = HasProjectType(projectTypes, GenerateFilesLabels.AnahSynthesisLabel.Isolation),
				HasProjectTypeChangeOfHeatingSystem =
					HasProjectType(projectTypes, GenerateFilesLabels.AnahSynthesisLabel.HeatingSystem),
				HasProjectTypeReparation = HasProjectType(projectTypes, GenerateFilesLabels.AnahSynthesisLabel.Reparation),
				SiretNumber = accompanyingFileCreator.SiretNumber ?? string.Empty
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAccompanyingFileForAnahSynthesisPdfQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
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

	private static bool HasProjectType(IEnumerable<PreWorkPlanProjectType> projectTypes, string projectTypeLabel) =>
		projectTypes.Any(pt => pt.ProjectTypeNavigation.Label == projectTypeLabel);
}