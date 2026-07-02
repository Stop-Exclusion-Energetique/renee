using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension.ImportCsvDataUseCase;

public static class ImportAccompanyingFileCsvDataExtension
{
    public static AccompanyingFile ImportHousehold(this AccompanyingFile accompanyingFile, Household household)
    {
        accompanyingFile.AccompanyingFileHouseholdNavigation = household;
        return accompanyingFile;
    }

    public static AccompanyingFile ImportHousing(this AccompanyingFile accompanyingFile, Housing housing)
    {
        accompanyingFile.AccompanyingFileHousingNavigation = housing;
        return accompanyingFile;
    }

    public static AccompanyingFile ImportPreFinancingPlan(
        this AccompanyingFile accompanyingFile,
        PreFinancingPlan preFinancingPlan)
    {
        accompanyingFile.AccompanyingFilePreFinancingPlanNavigation = preFinancingPlan;
        return accompanyingFile;
    }

    public static AccompanyingFile ImportPreWorkPlan(this AccompanyingFile accompanyingFile, PreWorkPlan preWorkPlan)
    {
        accompanyingFile.AccompanyingFilePreWorkPlanNavigation = preWorkPlan;
        return accompanyingFile;
    }

    public static AccompanyingFile ImportSupportTeam(this AccompanyingFile accompanyingFile, SupportTeam supportTeam)
    {
        accompanyingFile.AccompanyingFileSupportTeamNavigation = supportTeam;
        return accompanyingFile;
    }

    public static AccompanyingFile InitializeAccompanyingFile(
        ImportAccompanyingFileCsvData importAccompanyingFileData)
    {
        return new AccompanyingFile
        {
            AccompanyingFileReference = importAccompanyingFileData.Reference,
            ExternalReference = importAccompanyingFileData.ExternalReference,
            AccompanyingFileMilestone = (int)AccompanyingFileStage.Identify,
            AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress,
            FirstEncounterDate = importAccompanyingFileData.FirstContactDate,
            StartOfAccompanyingDate = importAccompanyingFileData.StartSupportDate,
            EndOfAccompanyingDate = importAccompanyingFileData.EndSupportDate,
            EndOfEncounterDate = importAccompanyingFileData.EndContactDate,
            AccompanyingTimeDurationForIdentificationMilestone =
                (int?)importAccompanyingFileData.AccompanyingTimeDurationForIdentificationMilestone,
            AccompanyingTimeDurationForOrganizeAndFinanceMilestone =
                (int?)importAccompanyingFileData.AccompanyingTimeDurationForOrganizeAndFinanceMilestone,
            AccompanyingTimeDurationForRealizeAndFollowMilestone =
                (int?)importAccompanyingFileData.AccompanyingTimeDurationForRealizeAndFollowMilestone,
            CreatedBy = importAccompanyingFileData.UserId,
            OpeningDate = importAccompanyingFileData.FileOpeningDate,
            CloseDate = importAccompanyingFileData.FileClosingDate,
            LastUpdateDate = DateTime.UtcNow,
            UpdatedBy = importAccompanyingFileData.UserId,
            IsDeleted = importAccompanyingFileData.IsDeleted,
            ZeroEnergyExclusionTerritoriesProgram = importAccompanyingFileData.ZeroEnergyExclusionTerritoriesProgram,
            AccompanyingType = (int?)importAccompanyingFileData.AccompanyingType,
            AccompanyingFileTerritory = importAccompanyingFileData.AccompanyingType is AccompanyingType.Targeted
                ? importAccompanyingFileData.TerritoryId
                : null,
            ImportRunId = importAccompanyingFileData.ImportRunId
        };
    }

    public static AccompanyingFile ImportWorkMonitoring(this AccompanyingFile accompanyingFile, WorkMonitoring workMonitoring)
    {
        accompanyingFile.AccompanyingFileWorkMonitoringNavigation = workMonitoring;
        return accompanyingFile;
    }

	public static AccompanyingFile ImportSiteSupervision(this AccompanyingFile accompanyingFile, SiteSupervision siteSupervision)
	{
		accompanyingFile.SiteSupervision = siteSupervision;
		return accompanyingFile;
	}
}