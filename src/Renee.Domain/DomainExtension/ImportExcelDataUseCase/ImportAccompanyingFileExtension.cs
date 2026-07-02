using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension.ImportExcelDataUseCase;

public static class ImportAccompanyingFileExtension
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
		ImportAccompanyingFileData importAccompanyingFileData)
	{
		return new AccompanyingFile
		{
			AccompanyingFileReference = importAccompanyingFileData.Reference,
			AccompanyingFileMilestone = (int)AccompanyingFileStage.Identify,
			AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress,
			FirstEncounterDate = importAccompanyingFileData.FirstContactDate,
			StartOfAccompanyingDate = importAccompanyingFileData.StartSupportDate,
			EndOfAccompanyingDate = importAccompanyingFileData.EndSupportDate,
			EndOfEncounterDate = importAccompanyingFileData.EndContactDate,
			NumberOfEncounterWithFamilyForIdentificationMilestone = 
				importAccompanyingFileData.ContactWithFamilyDuringIdentifyStage,
			NumberOfEncounterWithFamilyForOrganizeAndFinanceMilestone =
				importAccompanyingFileData.ContactWithFamilyDuringOrganizeAndFinanceStage,
			NumberOfEncounterWithFamilyForRealizeAndFollowMilestone =
				importAccompanyingFileData.ContactWithFamilyDuringRealizeAndFollowStage,
			CreatedBy = importAccompanyingFileData.UserId,
			OpeningDate = DateTime.UtcNow,
			LastUpdateDate = DateTime.UtcNow,
			UpdatedBy = importAccompanyingFileData.UserId,
			IsDeleted = false,
			ZeroEnergyExclusionTerritoriesProgram = importAccompanyingFileData.ZeroEnergyExclusionTerritoriesProgram,
			AccompanyingType = (int?)importAccompanyingFileData.AccompanyingType,
			AccompanyingFileTerritory = importAccompanyingFileData.TerritoryId,
		};
	}

    public static AccompanyingFile ImportWorkMonitoring(this AccompanyingFile accompanyingFile, WorkMonitoring workMonitoring)
	{
		accompanyingFile.AccompanyingFileWorkMonitoringNavigation = workMonitoring;
		return accompanyingFile;
	}

	public static AccompanyingFile ImportInvoice(this AccompanyingFile accompanyingFile, Invoice invoice)
	{
		accompanyingFile.Invoices.Add(invoice);
		return accompanyingFile;
	}
}