using Microsoft.EntityFrameworkCore;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;
using Renee.Domain.DomainExtension.ToRepository;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;
using System.Data;
using User = Renee.Domain.Entity.User;

namespace Renee.Infrastructure.Repositories;

public sealed class AccompanyingFileRepository(
    ReneeDbContext dbContext,
    IDbContextFactory<ReneeDbContext> contextFactory,
    ITelemetryService telemetryService) : IAccompanyingFileRepository
{
    public async Task<int> CountAllActiveAndInTzeeProgramAccompanyingFile()
    {
        try
        {
            return await dbContext.AccompanyingFiles.CountAsync(af => af.IsDeleted == false && af.ZeroEnergyExclusionTerritoriesProgram == true);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
    }

    public async Task<int> CountAllActiveAndInTzeeProgramAccompanyingFileForMilestone1()
    {
        try
        {
            return await dbContext.AccompanyingFiles.CountAsync(af => af.IsDeleted == false && af.ZeroEnergyExclusionTerritoriesProgram == true &&  af.AccompanyingFileMilestone >= 1);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
    }

    public async Task<int> AddAccompanyingFile(AccompanyingFile accompanyingFile)
    {
        try
        {
            var alreadyExists = await dbContext.AccompanyingFiles.FirstOrDefaultAsync(a =>
                a.AccompanyingFileReference == accompanyingFile.AccompanyingFileReference && a.IsDeleted == false);
            if (alreadyExists != null)
                throw new DuplicateNameException(
                    $"Un dossier avec la référence {accompanyingFile.AccompanyingFileReference} existe déjà.");
            await dbContext.AccompanyingFiles.AddAsync(accompanyingFile);
            return await dbContext.SaveChangesAsync();
        }
        catch (DuplicateNameException) { throw; }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> AddAccompanyingFiles(List<AccompanyingFile> accompanyingFiles)
    {
        try
        {
            dbContext.AccompanyingFiles.AddRange(accompanyingFiles);
            return await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> DeleteAccompanyingFile(Guid accompanyingFileId)
    {
        try
        {
            var accompanyingFile = await dbContext.AccompanyingFiles.FindAsync(accompanyingFileId);
            if (accompanyingFile != null)
            {
                // Soft delete
                accompanyingFile.IsDeleted = true;
                dbContext.Entry(accompanyingFile).State = EntityState.Modified;
            }

            return await dbContext.SaveChangesAsync();
        }
        catch (Exception) { return -1; }
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileByIdForIdentificationMilestoneAsync(Guid accompanyingFileId)
    {
        try
        {
			using var context = await contextFactory.CreateDbContextAsync();
			var accompanyingFile = await context.AccompanyingFiles
				.Include(af => af.AccompanyingFileHouseholdNavigation)
				.ThenInclude(hshld => hshld.MainOccupantNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
				.ThenInclude(hshld => hshld.SecondaryOccupants)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
				.ThenInclude(hshld => hshld.HouseholdResources).ThenInclude(hr => hr.HouseholdResourcesNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
				.ThenInclude(hshld => hshld.HouseholdHeatingEnergies).ThenInclude(he => he.HouseholdHeatingEnergyNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation).ThenInclude(hshld => hshld.HouseholdExpenses)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
				.ThenInclude(hshld => hshld.HouseholdDifficulties).ThenInclude(hd => hd.DifficultyNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation)
				.ThenInclude(hsg => hsg.HousingAddressNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation)
				.ThenInclude(hsg => hsg.HousingInitialStateNavigation)
				.Include(af => af.CopropertyProfileNavigation)
				.ThenInclude(cp => cp!.CopropertyHousingNavigation)
				.ThenInclude(ch => ch.HousingAddressNavigation)
				.Include(af => af.CopropertyProfileNavigation)
				.ThenInclude(cp => cp!.CopropertyDiagnosticsNavigation)
				.Include(af => af.AccompanyingFileSupportTeamNavigation)
				.ThenInclude(sb => sb.SolidarBuilderNavigation).ThenInclude(rs => rs.ReportingStructureNavigation)
				.AsNoTracking()
				.FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

			if (accompanyingFile is not null) context.Entry(accompanyingFile).State = EntityState.Detached;

			return accompanyingFile;
		}
        catch (Exception) { return null; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(
        Guid accompanyingFileId)
    {
        try
        {
			using var context = await contextFactory.CreateDbContextAsync();
			var accompanyingFile = await context.AccompanyingFiles
				.Include(af => af.AccompanyingFileHousingNavigation).ThenInclude(ha => ha.HousingAddressNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation)
				.ThenInclude(hsg => hsg.HousingInitialStateNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
				.ThenInclude(hshld => hshld.MainOccupantNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation)
				.ThenInclude(hsg => hsg.HousingAfterWorkStateNavigation)
				.Include(af => af.CopropertyProfileNavigation)
				.ThenInclude(cp => cp!.CopropertyWorkFinanceNavigation)
				.ThenInclude(cwf => cwf.WorkPackages)
				.ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
				.Include(af => af.AccompanyingFilePreWorkPlanNavigation)
				.ThenInclude(pr => pr.PreWorkPlanInsuranceTypes)
				.Include(af => af.AccompanyingFilePreWorkPlanNavigation)
				.ThenInclude(pr => pr.PreWorkPlanProjectTypes).ThenInclude(wp => wp.ProjectTypeNavigation)
				.Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pr => pr.WorkPackages)
				.ThenInclude(pr => pr.WorkPackageWorkTypeCosts).ThenInclude(wp => wp.WorkTypeNavigation)
				.Include(af => af.AccompanyingFilePreFinancingPlanNavigation).ThenInclude(pf => pf.FundingModes)
				.Include(af => af.AccompanyingFileSupportTeamNavigation)
				.ThenInclude(sb => sb.SolidarBuilderNavigation).ThenInclude(rs => rs.ReportingStructureNavigation)
				.AsNoTracking().FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

			if (accompanyingFile is not null) context.Entry(accompanyingFile).State = EntityState.Detached;
			return accompanyingFile;
		}
        catch (Exception) { return null; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(
        Guid accompanyingFileId)
    {
        try
        {
            var context = await contextFactory.CreateDbContextAsync();
            var accompanyingFile = await context.AccompanyingFiles
					.Include(af => af.AccompanyingFileWorkMonitoringNavigation)
                    .Include(af => af.Invoices)
                    .Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
                    .Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
					.Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
					.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
                    .Include(af => af.AccompanyingFilePreFinancingPlanNavigation.FundingModes)
                    .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
					.Include(af => af.SiteSupervision)
	                    .ThenInclude(ss => ss!.WorkParticipants)
		                    .ThenInclude(wp => wp.WorkParticipantDifficulties)
					.AsNoTracking()
                    .FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

                if (accompanyingFile is not null) context.Entry(accompanyingFile).State = EntityState.Detached;

                return accompanyingFile;
        }
        catch (Exception) { return null; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileForCsvImport(Guid accompanyingFileId)
    {
        try
        {
			using var context = await contextFactory.CreateDbContextAsync();
			var accompanyingFile = await context.AccompanyingFiles
				.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation).ThenInclude(h => h.SecondaryOccupants)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
					.ThenInclude(h => h.HouseholdResources)
						.ThenInclude(hr => hr.HouseholdResourcesNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
					.ThenInclude(h => h.HouseholdHeatingEnergies)
						.ThenInclude(he => he.HouseholdHeatingEnergyNavigation)
				.Include(af => af.AccompanyingFileHouseholdNavigation).ThenInclude(h => h.HouseholdExpenses)
				.Include(af => af.AccompanyingFileHouseholdNavigation)
					.ThenInclude(h => h.HouseholdDifficulties)
						.ThenInclude(hd => hd.DifficultyNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
				.Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
				.Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pw => pw.PreWorkPlanInsuranceTypes)
				.Include(af => af.AccompanyingFilePreWorkPlanNavigation)
					.ThenInclude(pw => pw.PreWorkPlanProjectTypes)
						.ThenInclude(pt => pt.ProjectTypeNavigation)
				.Include(af => af.AccompanyingFilePreWorkPlanNavigation)
					.ThenInclude(pw => pw.WorkPackages)
						.ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
							.ThenInclude(wtc => wtc.WorkTypeNavigation)
				.Include(af => af.AccompanyingFilePreFinancingPlanNavigation).ThenInclude(pf => pf.FundingModes)

				.Include(af => af.AccompanyingFileSupportTeamNavigation)
				.Include(af => af.AccompanyingFileWorkMonitoringNavigation)
				.Include(af => af.SiteSupervision)
					.ThenInclude(ss => ss!.WorkParticipants)
						.ThenInclude(wp => wp.WorkParticipantDifficulties)
				.Include(af => af.Invoices)
				.AsNoTracking()
				.AsSplitQuery()
				.FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

			if (accompanyingFile is not null) context.Entry(accompanyingFile).State = EntityState.Detached;
			return accompanyingFile;
		}
        catch (Exception) { return null; }
        finally { dbContext.ChangeTracker.Clear(); }

    }

    public async Task<AccompanyingFile?> GetAccompanyingFileDataForAnahSynthesisPdf(string accompanyingFileReference) =>
        await dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHousingNavigation)
            .ThenInclude(hsg => hsg.HousingAddressNavigation).Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.SolidarBuilderNavigation).Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.SecondSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.ThirdSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileWorkMonitoringNavigation)
            .Include(af => af.CreatedByNavigation).ThenInclude(u => u != null ? u.Role : null)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pwp => pwp.PreWorkPlanProjectTypes)
            .ThenInclude(pwpt => pwpt.ProjectTypeNavigation).AsNoTracking()
            .FirstOrDefaultAsync(af => af.AccompanyingFileReference == accompanyingFileReference && af.IsDeleted == false);

    public async Task<AccompanyingFile?> GetAccompanyingFileDataForWorkCertificatePdf(string accompanyingFileReference) =>
        await dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
                .ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
                    .ThenInclude(wt => wt.WorkTypeNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
            .Include(af => af.CreatedByNavigation).ThenInclude(u => u != null ? u.Role : null)
            .AsNoTracking().FirstOrDefaultAsync(af => af.AccompanyingFileReference == accompanyingFileReference && af.IsDeleted == false);

    public async Task<AccompanyingFile?> GetAccompanyingFileForAirtable(string accompanyingFileReference) =>
        await dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation)
            .ThenInclude(hshld => hshld.MainOccupantNavigation).Include(af => af.AccompanyingFileHouseholdNavigation)
            .ThenInclude(hsld => hsld.SecondaryOccupants).Include(af => af.AccompanyingFileHousingNavigation)
            .ThenInclude(hsg => hsg.HousingAddressNavigation).Include(af => af.AccompanyingFileHouseholdNavigation)
            .ThenInclude(hshld => hshld.HouseholdResources).ThenInclude(hr => hr.HouseholdResourcesNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation).ThenInclude(hsg => hsg.HousingInitialStateNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation).ThenInclude(hsg => hsg.HousingAfterWorkStateNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pwp => pwp.WorkPackages)
            .ThenInclude(wp => wp.WorkPackageWorkTypeCosts).Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(sp => sp.SolidarBuilderNavigation)
            .ThenInclude(sb => sb.ReportingStructureNavigation).AsNoTracking()
            .FirstOrDefaultAsync(af => af.AccompanyingFileReference == accompanyingFileReference && af.IsDeleted == false);

    public async Task<AccompanyingFile> GetBaseAccompanyingFile(Guid accompanyingFileId)
    {
		using var context = await contextFactory.CreateDbContextAsync();
		return await context.AccompanyingFiles.FirstAsync(af => af.Id == accompanyingFileId);
	}

    public async Task<AccompanyingFile?> GetAccompanyingFileForStageValidation(Guid accompanyingFileId)
    {
        try
        {
            var accompanyingFile =
                await dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
                    .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation)
                    .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
                    .Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation).AsNoTracking()
                    .FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

            if (accompanyingFile is not null) dbContext.Entry(accompanyingFile).State = EntityState.Detached;

            return accompanyingFile;
        }
        catch (Exception) { return null; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<List<AccompanyingFile>> GetAccompanyingFileStatisticsForAssociationMember(
        DateTime? fromDate,
        DateTime? toDate,
        List<Guid?>? reportingStructures = null)
    {
        var query = dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation)
            .Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pr => pr.WorkPackages)
            .ThenInclude(wp => wp.WorkPackageWorkTypeCosts).AsNoTracking().Where(af =>
                af.IsDeleted == false &&
                (
                    (fromDate == null && toDate == null) ||
                    (fromDate != null && af.FirstEncounterDate >= fromDate && (toDate == null || af.FirstEncounterDate <= toDate))
                )
            );

        if (reportingStructures is { Count: > 0 })
            query = query.Where(af => reportingStructures.Contains(
                af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureId));

        return await query.ToListAsync();
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileSupportTeam(Guid accompanyingFileId)
    {
        var accompanyingFile = await dbContext.AccompanyingFiles
        .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.Role)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.SecondSolidarBuilderNavigation).ThenInclude(u => u!.Role)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.SecondSolidarBuilderNavigation).ThenInclude(u => u!.ReportingStructureNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.ThirdSolidarBuilderNavigation).ThenInclude(u => u!.Role)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.ThirdSolidarBuilderNavigation).ThenInclude(u => u!.ReportingStructureNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.DiffuseCoordinatorNavigation).ThenInclude(u => u!.Role)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.TargetCoordinatorNavigation).ThenInclude(u => u!.Role)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.TerritorialBuilderNavigation).ThenInclude(u => u!.Role)
        .Include(af => af.AccompanyingFileSupportTeamNavigation)
            .ThenInclude(spt => spt.SecondTerritorialBuilderNavigation).ThenInclude(u => u!.Role)
        .Include(af => af.AbortRequestedBy)
        .Include(af => af.AbortDecidedBy)
        .Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
        .Include(af => af.AccompanyingFileBillingLog)
        .AsNoTracking().FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

        return accompanyingFile;
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileSynthesis(Guid accompanyingFileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        return await context.AccompanyingFiles
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(sp => sp.DiffuseCoordinatorNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(sp => sp.TerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(sp => sp.SecondTerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation).
            Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
                .ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
            .AsNoTracking()
            .FirstOrDefaultAsync(af => af.Id == accompanyingFileId);
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileToRetrieveSupportTeam(Guid accompanyingFileId) =>
        await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.SolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.SecondSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.ThirdSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.TerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.SecondTerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.DiffuseCoordinatorNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).ThenInclude(spt => spt.TargetCoordinatorNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

    public async Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFileForAssociationMembersAndAdmins(
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize)
    {
        var query = dbContext.AccompanyingFileResumeView.AsNoTracking().
            Where(af => af.IsDeleted == false);

        if (filterOptions.FilterContext == FilterContext.WithoutSolidarBuilders)
            query = query.Where(af =>
			    af.SolidarBuilderDeleted == true && af.SecondSolidarBuilderDeleted == true && 
                af.ThirdSolidarBuilderDeleted == true);

        if (filterOptions.ReportingStructures is { Count: > 0 })
            query = query.Where(af => filterOptions.ReportingStructures.Contains(af.ReportingStructureId));

        query = ApplyFiltersForAccompanyingFilesList(
            query,
			filterOptions.AccompanyingFileStages,
			filterOptions.AccompanyingFileStatuses,
			filterOptions.FilterValue,
			filterOptions.SortingState,
            filterOptions.AccompanyingFileNeedingBillings);

		return await ApplyPagination(query, numberOfItemsToSkip, pageSize);
	}

    public async Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForSolidarBuilders(
		Guid userId,
		Guid reportingStructureId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize)
    {
        var query = dbContext.AccompanyingFileResumeView.AsNoTracking().Where(af => af.IsDeleted == false);

        if (filterOptions.FilterContext is FilterContext.MyAccompanyingFile)
			query = query.Where(af =>
				(af.SolidarBuilder == userId) ||
				(af.SecondSolidarBuilder == userId) ||
				(af.ThirdSolidarBuilder == userId));

        if (filterOptions.FilterContext is FilterContext.MyReportingStructure)
		    query = query.Where(af => af.ReportingStructureId == reportingStructureId);

		query = ApplyFiltersForAccompanyingFilesList(
			query,
			filterOptions.AccompanyingFileStages,
			filterOptions.AccompanyingFileStatuses,
			filterOptions.FilterValue,
			filterOptions.SortingState,
            filterOptions.AccompanyingFileNeedingBillings);

		return await ApplyPagination(query, numberOfItemsToSkip, pageSize);
	}

    public async Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForCoordinators(
		Guid userId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize)
    {
        var query = dbContext.AccompanyingFileResumeView.AsNoTracking().Where(af => af.IsDeleted == false);

		if (filterOptions.FilterContext == FilterContext.MyAccompanyingFile)
			query = query.Where(af => af.DiffuseCoordinator == userId || af.TargetCoordinator == userId);

		if (filterOptions.FilterContext == FilterContext.AllFiles)
			query = query.Where(af => af.ZeroEnergyExclusionTerritoriesProgram == true);

        if (filterOptions.Territories is { Count: > 0})
            query = query.Where(af => filterOptions.Territories.Contains(af.AccompanyingFileTerritory));

        if (filterOptions.ReportingStructures is { Count: > 0 })
            query = query.Where(af => filterOptions.ReportingStructures.Contains(af.ReportingStructureId));

		if (filterOptions.SolidarBuilders is { Count: > 0 })
			query = query.Where(af =>
				filterOptions.SolidarBuilders.Contains(af.SolidarBuilder) ||
				filterOptions.SolidarBuilders.Contains(af.SecondSolidarBuilder) ||
				filterOptions.SolidarBuilders.Contains(af.ThirdSolidarBuilder));

		query = ApplyFiltersForAccompanyingFilesList(
			query,
			filterOptions.AccompanyingFileStages,
			filterOptions.AccompanyingFileStatuses,
			filterOptions.FilterValue,
			filterOptions.SortingState,
            filterOptions.AccompanyingFileNeedingBillings);

		return await ApplyPagination(query, numberOfItemsToSkip, pageSize);
	}

	public async Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForTerritorialBuilders(
		Guid userId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize)
    {
        var query = dbContext.AccompanyingFileResumeView.AsNoTracking().Where(af => 
            af.IsDeleted == false &&
            af.ZeroEnergyExclusionTerritoriesProgram == true &&
            (af.TerritorialBuilder == userId || af.SecondTerritorialBuilder == userId));

		if (filterOptions.ReportingStructures is { Count: > 0 })
			query = query.Where(af => filterOptions.ReportingStructures.Contains(af.ReportingStructureId));

		if (filterOptions.SolidarBuilders is { Count: > 0 })
			query = query.Where(af =>
				filterOptions.SolidarBuilders.Contains(af.SolidarBuilder) ||
				filterOptions.SolidarBuilders.Contains(af.SecondSolidarBuilder) ||
				filterOptions.SolidarBuilders.Contains(af.ThirdSolidarBuilder));

		query = ApplyFiltersForAccompanyingFilesList(
			query,
			filterOptions.AccompanyingFileStages,
			filterOptions.AccompanyingFileStatuses,
			filterOptions.FilterValue,
			filterOptions.SortingState,
            filterOptions.AccompanyingFileNeedingBillings);

		return await ApplyPagination(query, numberOfItemsToSkip, pageSize);
	}

	public async Task<(List<Guid> ReportingStructureIds, List<Guid> SolidarBuilderIds)> GetTerritorialBuilderFilterData(
		Guid userId)
	{
		var baseQuery = dbContext.AccompanyingFileResumeView.AsNoTracking().Where(af =>
			af.IsDeleted == false &&
			af.ZeroEnergyExclusionTerritoriesProgram == true &&
			(af.TerritorialBuilder == userId || af.SecondTerritorialBuilder == userId));

		var reportingStructures = await baseQuery
			.Select(af => af.ReportingStructureId)
			.Where(id => id.HasValue)
			.Select(id => id!.Value)
			.Distinct()
			.ToListAsync();

		var primarySolidarBuilders = await baseQuery
			.Select(af => af.SolidarBuilder)
			.Distinct()
			.ToListAsync();

		var secondSolidarBuilders = await baseQuery
			.Select(af => af.SecondSolidarBuilder)
			.Where(id => id.HasValue)
			.Select(id => id!.Value)
			.Distinct()
			.ToListAsync();

		var thirdSolidarBuilders = await baseQuery
			.Select(af => af.ThirdSolidarBuilder)
			.Where(id => id.HasValue)
			.Select(id => id!.Value)
			.Distinct()
			.ToListAsync();

		var solidarBuilders = primarySolidarBuilders
			.Concat(secondSolidarBuilders)
			.Concat(thirdSolidarBuilders)
			.Distinct()
			.ToList();

		return (reportingStructures, solidarBuilders);
	}

	public async Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForStructuralReferents(
		Guid reportingStructureId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize)
    {
        var nationalStructureId = await dbContext.ReportingStructures.AsNoTracking()
            .Where(rs => rs.Id == reportingStructureId)
            .Select(rs => rs.NationalStructureId)
            .FirstOrDefaultAsync();

        if (nationalStructureId == null)
            return ([], 0);

        var query = dbContext.AccompanyingFileResumeView.AsNoTracking().Where(af =>
            af.IsDeleted == false && af.NationalStructureId == nationalStructureId);

		query = ApplyFiltersForAccompanyingFilesList(
			query,
			filterOptions.AccompanyingFileStages,
			filterOptions.AccompanyingFileStatuses,
			filterOptions.FilterValue,
			filterOptions.SortingState,
            filterOptions.AccompanyingFileNeedingBillings);

		return await ApplyPagination(query, numberOfItemsToSkip, pageSize);
	}

	public async Task<bool> HasAwaitingAnahResponseAsync(Guid userId)
	{
		try
		{
			using var context = await contextFactory.CreateDbContextAsync();

			return await BuildAwaitingAnahResponseQuery(context, userId).AnyAsync();
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return false;
		}
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<List<AccompanyingFile>> GetAllAwaitingAnahResponseAsync(Guid userId)
	{
		try
		{
			using var context = await contextFactory.CreateDbContextAsync();

			return await BuildAwaitingAnahResponseQuery(context, userId).ToListAsync();
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return [];
		}
		finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<(List<AccompanyingFile>, AverageAccompanyingDuration)> GetStatisticsForCoordinatorsIndex(
        Guid userId,
        DateTime? fromDate,
        DateTime? toDate,
        bool shouldFilterOnUserAllAccompanyingFile,
		AccompanyingFilesStatisticsFilterOptions filterOptions,
        bool IsTargetedCoordinator)
    {
        var accompanyingDurationAverage = await dbContext.AverageAccompanyingDurations.FirstAsync();

        var query = dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation)
            .ThenInclude(hshld => hshld.MainOccupantNavigation).Include(af => af.AccompanyingFileHousingNavigation)
            .ThenInclude(hsg => hsg.HousingInitialStateNavigation)
            .Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation).Include(af => af.AccompanyingFileHousingNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pr => pr.WorkPackages)
            .ThenInclude(wp => wp.WorkPackageWorkTypeCosts).Include(af => af.AccompanyingFileHousingNavigation)
            .ThenInclude(af => af.HousingAfterWorkStateNavigation).AsNoTracking().Where(af =>
                af.IsDeleted == false &&
                ((fromDate == null && toDate == null) ||
                 (fromDate != null &&
                  af.FirstEncounterDate >= fromDate &&
                  (toDate == null || af.FirstEncounterDate <= toDate))) &&
                af.ZeroEnergyExclusionTerritoriesProgram.HasValue &&
                af.ZeroEnergyExclusionTerritoriesProgram.Value);

        if (!shouldFilterOnUserAllAccompanyingFile)
            query = query.Where(af => IsTargetedCoordinator
                    ? af.AccompanyingFileSupportTeamNavigation.TargetCoordinator == userId
                    : af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator == userId);

        query = ApplyFilterOptions(query, filterOptions);

        return (await query.ToListAsync(), accompanyingDurationAverage);
    }

    public async Task<List<AccompanyingFile>>
        GetStatisticsForSolidarBuilderIndex(Guid userId, DateTime? fromDate, DateTime? toDate) =>
        await dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation)
            .Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pr => pr.WorkPackages)
            .ThenInclude(wp => wp.WorkPackageWorkTypeCosts).AsNoTracking().Where(af =>
                af.IsDeleted == false &&
                (af.AccompanyingFileSupportTeamNavigation.SolidarBuilder == userId ||
                 af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == userId ||
                 af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == userId) &&
                ((fromDate == null && toDate == null) ||
                 (fromDate != null && toDate == null && af.FirstEncounterDate >= fromDate) ||
                 (fromDate == null && toDate != null && af.FirstEncounterDate <= toDate) ||
                 (fromDate != null &&
                  toDate != null &&
                  af.FirstEncounterDate >= fromDate &&
                  af.FirstEncounterDate <= toDate))).ToListAsync();

    public async Task<List<AccompanyingFile>>
        GetStatisticsForSolidarBuilderReportingStructure(
            Guid reportingStructureId,
            DateTime? fromDate,
            DateTime? toDate) =>
        await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileHouseholdNavigation)
            .Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
                .ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
            .AsNoTracking()
            .Where(
                af =>
                af.IsDeleted == false &&
                af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureId ==
                reportingStructureId &&
                (
                    (fromDate == null && toDate == null) ||
                    (fromDate != null && toDate == null && af.FirstEncounterDate >= fromDate) ||
                    (fromDate == null && toDate != null && af.FirstEncounterDate <= toDate) ||
                    (fromDate != null &&
                        toDate != null &&
                        af.FirstEncounterDate >= fromDate &&
                        af.FirstEncounterDate <= toDate
                    )
                )
            ).ToListAsync();

    public async Task<List<AccompanyingFile>> GetStatisticsForTerritorialBuilderIndex(
        Guid userId,
        DateTime? fromDate,
        DateTime? toDate,
        bool shouldFilterOnUserAllAccompanyingFile,
		AccompanyingFilesStatisticsFilterOptions filterOptions)
    {
        var query = dbContext.AccompanyingFiles.Include(af => af.AccompanyingFileHouseholdNavigation)
            .Include(af => af.AccompanyingFilePreWorkPlanNavigation).ThenInclude(pr => pr.WorkPackages)
            .ThenInclude(wp => wp.WorkPackageWorkTypeCosts).Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
            .Include(af => af.AccompanyingFileWorkMonitoringNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation).ThenInclude(hsg => hsg.HousingInitialStateNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation).ThenInclude(hsg => hsg.HousingAfterWorkStateNavigation)
            .AsNoTracking().Where(af =>
                af.IsDeleted == false &&
                ((fromDate == null && toDate == null) ||
                 (fromDate != null &&
                  af.FirstEncounterDate >= fromDate &&
                  (toDate == null || af.FirstEncounterDate <= toDate))) &&
                af.AccompanyingType == (int)AccompanyingType.Targeted);

        if (!shouldFilterOnUserAllAccompanyingFile)
            query = query.Where(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilder == userId ||
                                    af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder == userId);

        query = ApplyFilterOptions(query, filterOptions);

        return await query.ToListAsync();
    }

    public async Task<List<AccompanyingFile>> GetStatisticsForStructuralReferentIndex(
            Guid reportingStructureId,
            DateTime? fromDate,
            DateTime? toDate
        )
    {
        var nationalStructureId = await dbContext.ReportingStructures.AsNoTracking()
            .Where(rs => rs.Id == reportingStructureId)
            .Select(rs => rs.NationalStructureId)
            .FirstOrDefaultAsync();

        if (nationalStructureId != null)
        {
            return [.. dbContext.AccompanyingFiles
                .Include(af => af.CreatedByNavigation).ThenInclude(u => u != null ? u.ReportingStructureNavigation : null)
                .Include(af => af.AccompanyingFileHouseholdNavigation)
                .Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
                    .ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
                .Include(af => af.AccompanyingFilePreFinancingPlanNavigation)
                .Include(af => af.AccompanyingFileWorkMonitoringNavigation)
                .Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
                .Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
                .AsNoTracking()
                .Where(
                    af =>
                    af.IsDeleted == false &&
                    af.CreatedByNavigation != null &&
                    af.CreatedByNavigation.ReportingStructureNavigation != null &&
                    af.CreatedByNavigation.ReportingStructureNavigation.NationalStructureId == nationalStructureId &&
                    ((fromDate == null && toDate == null) ||
                    (fromDate != null &&
                    af.FirstEncounterDate >= fromDate &&
                    (toDate == null || af.FirstEncounterDate <= toDate)))
                )
            ];
        }

        return [];
    }

    public async Task<List<AccompanyingFile>> GetAccompanyingFilesByExternalReferences(List<string?> references)
    {
        return await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
            .Where(af => af.ExternalReference != null && references.Contains(af.ExternalReference)).ToListAsync();
    }

    public async Task<int> UpdateAccompanyingFileByIdForIdentificationMilestoneAsync(
        AccompanyingFile accompanyingFile,
        SaveAndSubmitIdentificationMilestoneData data)
    {
        try
        {
            dbContext.Entry(accompanyingFile).State = EntityState.Modified;

            ApplyIdentificationMilestoneUpdates(accompanyingFile, data);

            var result = await dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateBaseAccompanyingFile(AccompanyingFile accompanyingFile)
    {
        try
        {
            dbContext.Entry(accompanyingFile).State = EntityState.Modified;
            return await dbContext.SaveChangesAsync();
        }
        catch (Exception ex) 
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            return -1; 
        }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int?> UpdateAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(
        AccompanyingFile accompanyingFile,
        SaveAndSubmitOrganizeAndFinanceMilestoneData data)
    {
        try
        {
            dbContext.Entry(accompanyingFile).State = EntityState.Modified;

            ApplyOrganizeAndFinanceMilestoneUpdates(accompanyingFile, data);

            return await dbContext.SaveChangesAsync();
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(
        AccompanyingFile accompanyingFile,
		SaveAndSubmitRealizeAndFollowMilestoneData data)
    {
        try
        {
            dbContext.Entry(accompanyingFile).State = EntityState.Modified;
            dbContext.Entry(accompanyingFile.AccompanyingFilePreFinancingPlanNavigation).State = EntityState.Modified;
            dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation).State = EntityState.Modified;

			ApplyRealizeAndFollowMilestoneUpdates(accompanyingFile,data);

            return await dbContext.SaveChangesAsync();
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateAccompanyingFileImportedWithCsvAsync(
        AccompanyingFile accompanyingFile,
        SaveAndSubmitIdentificationMilestoneData identificationMilestoneData,
        SaveAndSubmitOrganizeAndFinanceMilestoneData organizeAndFinanceMilestoneData,
        SaveAndSubmitRealizeAndFollowMilestoneData realizeAndFollowMilestoneData)
    {
        try
        {
            dbContext.Entry(accompanyingFile).State = EntityState.Modified;

            ApplyIdentificationMilestoneUpdates(accompanyingFile, identificationMilestoneData);

            ApplyOrganizeAndFinanceMilestoneUpdates(accompanyingFile, organizeAndFinanceMilestoneData);

            ApplyRealizeAndFollowMilestoneUpdates(accompanyingFile, realizeAndFollowMilestoneData);

            return await dbContext.SaveChangesAsync();
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateAccompanyingFileSynthesis(
        AccompanyingFile accompanyingFile,
        List<Invoice>? createdInvoices = null)
    {
        try
        {
            dbContext.Entry(accompanyingFile).State = EntityState.Modified;

            if (createdInvoices != null)
                foreach (var invoice in createdInvoices)
                    dbContext.Entry(invoice).State = EntityState.Added;

            var result = await dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateAccompanyingSupportTeam(
        AccompanyingFile accompanyingFile,
        User entity,
        string supportTeamMemberRole)
    {
        try
        {
            var supportTeam = await dbContext.SupportTeams.Include(st => st.SolidarBuilderNavigation)
                .Include(st => st.SecondSolidarBuilderNavigation).Include(st => st.ThirdSolidarBuilderNavigation)
                .Include(st => st.TerritorialBuilderNavigation).Include(st => st.SecondTerritorialBuilderNavigation)
                .Include(st => st.DiffuseCoordinatorNavigation).Include(st => st.TargetCoordinatorNavigation)
                .FirstOrDefaultAsync(st => st.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.Id);

            if (supportTeam == null) return 0;

            SupportTeamExtension.UpdateSupportTeamMember(supportTeam, entity, supportTeamMemberRole, dbContext);

            dbContext.Entry(supportTeam).State = EntityState.Modified;

            return await dbContext.SaveChangesAsync();
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
    }

	public async Task<int> UpdateAccompanyingFilesForAnahGrantCheck(List<AccompanyingFile> accompanyingFiles)
    {
        try
        {
            foreach (var accompanyingFile in accompanyingFiles)
            {
                dbContext.Entry(accompanyingFile).State = EntityState.Modified;
            }
            return await dbContext.SaveChangesAsync();
        }
        catch (Exception) { return -1; }
        finally { dbContext.ChangeTracker.Clear(); }
	}

	public async Task<List<AccompanyingFile>> GetAllAccompanyingFileForExcelExport()
		=> await dbContext.AccompanyingFiles
		.Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation)
		.Include(af => af.AbortReasonLabel)
		.Include(af => af.AccompanyingFileTerritoryNavigation)
		.Include(af => af.AccompanyingFilePreFinancingPlanNavigation.FundingModes)
		.Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
			.ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
				.ThenInclude(tpc => tpc.WorkTypeNavigation)
		.Include(af => af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes)
			.ThenInclude(pt => pt.ProjectTypeNavigation)
		.Include(af => af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanInsuranceTypes)
			.ThenInclude(pt => pt.InsuranceTypeNavigation)
		.Include(af => af.AccompanyingFileWorkMonitoringNavigation)
		.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdDifficulties)
			.ThenInclude(hsld => hsld.DifficultyNavigation)
		.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
		.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdExpenses)
		.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdResources)
			.ThenInclude(r => r.HouseholdResourcesNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
		.Include(af => af.AbortDecidedBy != null ? af.AbortDecidedBy.Role : null)
		.Include(af => af.SiteSupervision)
		.AsNoTracking()
		.AsSplitQuery()
		.Where(af => af.ZeroEnergyExclusionTerritoriesProgram == true && af.IsDeleted == false)
		.ToListAsync();

	public async Task<List<AccompanyingFile>> GetSolidarOrTerritorialBuilderAccompanyingFileForExcelExport(Guid userId)
	    => await dbContext.AccompanyingFiles
		.Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation)
		.Include(af => af.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation)
        .Include(af => af.AbortReasonLabel)
		.Include(af => af.AccompanyingFileTerritoryNavigation)
		.Include(af => af.AccompanyingFilePreFinancingPlanNavigation.FundingModes)
		.Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
			.ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
				.ThenInclude(tpc => tpc.WorkTypeNavigation)
		.Include(af => af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes)
			.ThenInclude(pt => pt.ProjectTypeNavigation)
		.Include(af => af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanInsuranceTypes)
			.ThenInclude(pt => pt.InsuranceTypeNavigation)
		.Include(af => af.AccompanyingFileWorkMonitoringNavigation)
		.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdDifficulties)
			.ThenInclude(hsld => hsld.DifficultyNavigation)
		.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
		.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdExpenses)
		.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdResources)
			.ThenInclude(r => r.HouseholdResourcesNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
		.Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
        .Include(af => af.SiteSupervision)
		.Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
        .Include(af => af.AbortDecidedBy != null ? af.AbortDecidedBy.Role : null)
		.AsNoTracking()
		.AsSplitQuery()
		.Where(af => (af.AccompanyingFileSupportTeamNavigation.SolidarBuilder == userId ||
						af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == userId ||
						af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == userId ||
						af.AccompanyingFileSupportTeamNavigation.TerritorialBuilder == userId ||
						af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder == userId) && 
					af.IsDeleted == false)
		.ToListAsync();

    public async Task<List<AccompanyingFile>> GetStructuralReferentAccompanyingFileForExcelExport(Guid nationalStructureId)
    {
        return await dbContext.AccompanyingFiles
        .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation)
        .Include(af => af.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation)
        .Include(af => af.AbortReasonLabel)
        .Include(af => af.AccompanyingFileTerritoryNavigation)
        .Include(af => af.AccompanyingFilePreFinancingPlanNavigation.FundingModes)
        .Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
            .ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
                .ThenInclude(tpc => tpc.WorkTypeNavigation)
        .Include(af => af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes)
            .ThenInclude(pt => pt.ProjectTypeNavigation)
        .Include(af => af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanInsuranceTypes)
            .ThenInclude(pt => pt.InsuranceTypeNavigation)
        .Include(af => af.AccompanyingFileWorkMonitoringNavigation)
        .Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdDifficulties)
            .ThenInclude(hsld => hsld.DifficultyNavigation)
        .Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
        .Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdExpenses)
        .Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdResources)
            .ThenInclude(r => r.HouseholdResourcesNavigation)
        .Include(af => af.AccompanyingFileHousingNavigation)
        .Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
        .Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
        .Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
		.Include(af => af.AbortDecidedBy != null ? af.AbortDecidedBy.Role : null)
		.Include(af => af.SiteSupervision)
		.AsNoTracking()
        .AsSplitQuery()
        .Where(af => (af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation != null &&
            af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation.NationalStructureId == nationalStructureId))
        .ToListAsync();
    }

    public async Task<bool> UpdateAccompanyingFileTargetInformations(AccompanyingFile accompanyingFile)
    {
        dbContext.Entry(accompanyingFile).State = EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileSupportTeamNavigation).State = EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation).State = EntityState.Modified;

        var rows = await dbContext.SaveChangesAsync();
        dbContext.Entry(accompanyingFile).State = EntityState.Detached;
        dbContext.Entry(accompanyingFile.AccompanyingFileSupportTeamNavigation).State = EntityState.Detached;
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation).State = EntityState.Detached;

        return rows > 0;
    }

    public async Task<AccompanyingFile?> GetAccompanyingFileWithBillingLog(Guid accompanyingFileId) => await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileBillingLog)
            .AsNoTracking()
            .FirstOrDefaultAsync(af => af.Id == accompanyingFileId);

    public async Task<bool> UpdateAccompanyingFileBillingLog(AccompanyingFile accompanyingFile)
    {
        dbContext.Entry(accompanyingFile).State = EntityState.Modified;
        if (accompanyingFile.AccompanyingFileBillingLog != null && accompanyingFile.AccompanyingFileBillingLog.Id != Guid.Empty)
        {
            dbContext.Entry(accompanyingFile.AccompanyingFileBillingLog).State = EntityState.Modified;
        }

        if( accompanyingFile.AccompanyingFileBillingLog != null && accompanyingFile.AccompanyingFileBillingLog.Id == Guid.Empty)
        {
            dbContext.Entry(accompanyingFile.AccompanyingFileBillingLog).State = EntityState.Added;
        }

        var rows = await dbContext.SaveChangesAsync();
        dbContext.Entry(accompanyingFile).State = EntityState.Detached;
        if (accompanyingFile.AccompanyingFileBillingLog != null)
        {
            dbContext.Entry(accompanyingFile.AccompanyingFileBillingLog).State = EntityState.Detached;
        }

        return rows > 0;
    }

    public async Task<List<AccompanyingFile>> GetAllAccompanyingFileForBillingLogAndAdministrationExcelExport()
    {
        return await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileBillingLog)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation)
            .Include(af => af.AccompanyingFileTerritoryNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(af => af.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<List<AccompanyingFile>> GetSolidarOrTerritorialBuilderAccompanyingFileForBillingLogAndAdministrationExcelExport(Guid userId)
    {
        return await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileBillingLog)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation)
            .Include(af => af.AccompanyingFileTerritoryNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(af => (af.AccompanyingFileSupportTeamNavigation.SolidarBuilder == userId ||
                            af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == userId ||
                            af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == userId ||
                            af.AccompanyingFileSupportTeamNavigation.TerritorialBuilder == userId ||
                            af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder == userId) &&
                        af.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<List<AccompanyingFile>> GetStructuralReferentAccompanyingFileForBillingLogAndAdministrationExcelExport(Guid nationalStructureId)
    {
        return await dbContext.AccompanyingFiles
            .Include(af => af.AccompanyingFileBillingLog)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation)
            .Include(af => af.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation)
            .Include(af => af.AccompanyingFileTerritoryNavigation)
            .Include(af => af.AccompanyingFileHousingNavigation)
            .AsNoTracking()
            .AsSplitQuery()
            .Where(af => (af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation != null &&
                af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation.NationalStructureId == nationalStructureId) &&
                af.IsDeleted == false)
        .ToListAsync();
    }

    public async Task<int> UpdateAccompanyingFilesCopropertyProfileId(
          List<Guid> accompanyingFileIds,
          Guid copropertyProfileId)
    {
        try
        {
            var accompanyingFiles = await dbContext.AccompanyingFiles
                .Where(accompanyingFile => accompanyingFileIds.Contains(accompanyingFile.Id))
                .ToListAsync();

            foreach (var accompanyingFile in accompanyingFiles)
            {
                accompanyingFile.CopropertyProfileId = copropertyProfileId;
                dbContext.Entry(accompanyingFile).State = EntityState.Modified;
            }

            return accompanyingFiles.Count == 0 ? 0 : await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    private void ApplyIdentificationMilestoneUpdates(
        AccompanyingFile accompanyingFile,
        SaveAndSubmitIdentificationMilestoneData dto)
    {
        dbContext.Entry(accompanyingFile.AccompanyingFileHouseholdNavigation).State = EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation).State =
            EntityState.Modified;

        UpdateEntitiesWithStates(dbContext, dto.SecondaryOccupantChanges);
		UpdateEntitiesWithStates(dbContext, dto.HouseholdExpenseChanges);
		UpdateEntitiesWithStates(dbContext, dto.HouseholdResourceChanges);
		UpdateEntitiesWithStates(dbContext, dto.HouseholdDifficultyChanges);
		UpdateEntitiesWithStates(dbContext, dto.HouseholdHeatingEnergyChanges);

        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation).State = EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation).State =
            EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation).State =
            EntityState.Modified;
    }

    private void ApplyOrganizeAndFinanceMilestoneUpdates(
        AccompanyingFile accompanyingFile, SaveAndSubmitOrganizeAndFinanceMilestoneData data)
    {
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation).State = EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation).State =
            EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation).State =
            EntityState.Modified;

        dbContext.Entry(accompanyingFile.AccompanyingFilePreWorkPlanNavigation).State = EntityState.Modified;
        dbContext.Entry(accompanyingFile.AccompanyingFilePreFinancingPlanNavigation).State = EntityState.Modified;

		UpdateEntitiesWithStates(dbContext, data.ProjectTypeChanges);
        UpdateEntitiesWithStates(dbContext, data.InsuranceTypeChanges);
        UpdateEntitiesWithStates(dbContext, data.FundingModeChanges);
		UpdateEntityGroup(
	        dbContext,
	        accompanyingFile,
	        data.WorkPackageChanges,
	        file => file.AccompanyingFilePreWorkPlanNavigation.WorkPackages,
	        (wp, existing) => wp.UpdateWorkTypeCosts(existing));
	}

	private void ApplyRealizeAndFollowMilestoneUpdates(
        AccompanyingFile accompanyingFile,
        SaveAndSubmitRealizeAndFollowMilestoneData data)
    {
        if (accompanyingFile.AccompanyingFileWorkMonitoring == Guid.Empty ||
            accompanyingFile.AccompanyingFileWorkMonitoring == null)
            dbContext.Entry(accompanyingFile.AccompanyingFileWorkMonitoringNavigation!).State = EntityState.Added;
        else
            dbContext.Entry(accompanyingFile.AccompanyingFileWorkMonitoringNavigation!).State =
                EntityState.Modified;

        if (accompanyingFile.SiteSupervisionId == Guid.Empty ||
            accompanyingFile.SiteSupervisionId == null)
            dbContext.Entry(accompanyingFile.SiteSupervision!).State = EntityState.Added;
        else
            dbContext.Entry(accompanyingFile.SiteSupervision!).State =
                EntityState.Modified;

        UpdateEntitiesWithStates(dbContext, data.InvoiceChanges);
        UpdateEntitiesWithStates(dbContext, data.FundingModeChanges);
        UpdateEntityGroup(
            dbContext,
            accompanyingFile,
            data.WorkParticipantChanges,
            file => file.SiteSupervision?.WorkParticipants ?? [],
            (wp, existing) => wp.UpdateWorkParticipantDifficulties(existing));
	}

    private static IQueryable<AccompanyingFile> ApplyFilterOptions(
        IQueryable<AccompanyingFile> query,
		AccompanyingFilesStatisticsFilterOptions filterOptions)
    {
        if (filterOptions.ReportingStructures is { Count: > 0 })
            query = query.Where(af => filterOptions.ReportingStructures.Contains(
                af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureId));

        if (filterOptions.SolidarBuilders is { Count: > 0 })
            query = query.Where(af =>
                filterOptions.SolidarBuilders.Contains(
                    af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.Id) ||
                filterOptions.SolidarBuilders.Contains(af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder));

        if (filterOptions.Territories is { Count: > 0 })
            query = query.Where(af =>
                af.AccompanyingFileTerritoryNavigation != null &&
                filterOptions.Territories.Contains(af.AccompanyingFileTerritoryNavigation.Id));

        return query;
    }

	private static void UpdateEntitiesWithStates<T>(
	    DbContext dbContext,
        EntityChanges<T> entityChanges) where T : class
	{
		foreach (var entity in entityChanges.ToAdd) dbContext.Entry(entity).State = EntityState.Added;
		foreach (var entity in entityChanges.ToUpdate) dbContext.Entry(entity).State = EntityState.Modified;
		foreach (var entity in entityChanges.ToRemove) dbContext.Entry(entity).State = EntityState.Deleted;
	}

    private static void UpdateEntityGroup<TBaseEntity, TEntity, TChildEntity>(
	    DbContext dbContext,
	    TBaseEntity parent,
	    EntityChanges<TEntity> changes,
	    Func<TBaseEntity, ICollection<TEntity>> existingCollectionSelector,
	    Func<TEntity, TEntity, EntityChanges<TChildEntity>> updateChildFunc)
	    where TEntity : class
	    where TChildEntity : class
	{
		var collection = existingCollectionSelector(parent);
		UpdateEntitiesWithStates(dbContext, changes);

		foreach (var entityToDelete in changes.ToRemove)
		{
			var existingId = (Guid)dbContext.Entry(entityToDelete).Property("Id").CurrentValue!;
			var existing = collection.FirstOrDefault(e =>
			{
				var id = (Guid)dbContext.Entry(e).Property("Id").CurrentValue!;
				return id == existingId;
			});

			if (existing is not null)
			{
				collection.Remove(existing);
				dbContext.Remove(existing);
			}
		}

		foreach (var entityToAdd in changes.ToAdd)
		{
			collection.Add(entityToAdd);
		}

		foreach (var entity in changes.ToUpdate)
		{
			var targetId = (Guid)dbContext.Entry(entity).Property("Id").CurrentValue!;

			var existing = collection.FirstOrDefault(e =>
			{
				var existingId = (Guid)dbContext.Entry(e).Property("Id").CurrentValue!;
				return existingId == targetId;
			});

			if (existing == null)
				continue;

			var entityChanges = updateChildFunc(entity, existing);
			UpdateEntitiesWithStates(dbContext, entityChanges);
		}
	}

    private static IQueryable<AccompanyingFileResumeView> ApplyFiltersForAccompanyingFilesList(
        IQueryable<AccompanyingFileResumeView> query,
        List<AccompanyingFileStage> stages,
        List<AccompanyingFileStatus> statuses,
        string? filterValue,
        SortingState sortingState,
        List<AccompanyingFileNeedingBilling> needingBilling)
    {
        if (stages is { Count: > 0 })
            query = query.Where(af => stages.Contains((AccompanyingFileStage)af.AccompanyingFileMilestone));

        if (statuses is { Count: > 0 })
            query = query.Where(af => statuses.Contains((AccompanyingFileStatus)af.AccompanyingFileStatus));

        if (!string.IsNullOrWhiteSpace(filterValue))
        {
            var value = filterValue.Trim();
            query = query.Where(af =>
                EF.Functions.Like(af.AccompanyingFileReference, $"%{value}%") ||
                EF.Functions.Like(af.Label, $"%{value}%") ||
                EF.Functions.Like(af.FirstName, $"%{value}%") ||
                EF.Functions.Like(af.LastName, $"%{value}%"));
        }

        query = ApplyBillingFilters(query, needingBilling);

        query = sortingState switch
        {
            SortingState.Ascending => query.OrderBy(af => af.LastUpdateDate),
            SortingState.Descending => query.OrderByDescending(af => af.LastUpdateDate),
            SortingState.None => query.OrderBy(af => af.OpeningDate),
            _ => query
        };

        return query;
    }

    private static IQueryable<AccompanyingFileResumeView> ApplyBillingFilters(
        IQueryable<AccompanyingFileResumeView> query,
        List<AccompanyingFileNeedingBilling> needingBilling)
    {
        if (needingBilling is { Count: > 0 })
        {
            var filterStageOne = needingBilling.Contains(AccompanyingFileNeedingBilling.ToBillStageOne);
            var filterStageTwo = needingBilling.Contains(AccompanyingFileNeedingBilling.ToBillStageTwo);
            var filterStageThree = needingBilling.Contains(AccompanyingFileNeedingBilling.ToBillStageThree);

            if (filterStageOne || filterStageTwo || filterStageThree)
            {
                query = query.Where(af =>
                    (filterStageOne && (af.BilledJalon1 == false || af.BilledJalon1 == null) && af.AccompanyingFileMilestone >= (int)AccompanyingFileStage.OrganizingAndFinancing) ||
                    (filterStageTwo && (af.BilledJalon2 == false || af.BilledJalon2 == null) && af.AccompanyingFileMilestone >= (int)AccompanyingFileStage.RealisationAndFollowing) ||
                    (filterStageThree && (af.BilledJalon3 == false || af.BilledJalon3 == null) && af.AccompanyingFileMilestone == (int)AccompanyingFileStage.Finished)
                );
            }
        }
        return query;
    }

    private static async Task<(List<AccompanyingFileResumeView>, int)> ApplyPagination(
        IQueryable<AccompanyingFileResumeView> query,
        int skip, 
        int take) => (await query.Skip(skip).Take(take).ToListAsync(), await query.CountAsync());

	private static IQueryable<AccompanyingFile> BuildAwaitingAnahResponseQuery(ReneeDbContext context, Guid userId)
	{
		var sixMonthsAgoDate = DateTime.UtcNow.AddMonths(-6);

		return context.AccompanyingFiles
			.Include(af => af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation)
			.Include(af => af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation)
			.Include(af => af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation)
			.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
			.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
            .AsNoTracking()
			.Where(af =>
				af.IsDeleted == false &&
				(af.AccompanyingFileSupportTeamNavigation.SolidarBuilder == userId ||
				 af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == userId ||
				 af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == userId) &&
				af.AccompanyingFileMilestone == (int)AccompanyingFileStage.RealisationAndFollowing &&
				af.AnahFolderFilingDate.HasValue &&
				af.AnahFolderFilingDate.Value < sixMonthsAgoDate &&
				af.AnahGrantDate == null);
	}
}
