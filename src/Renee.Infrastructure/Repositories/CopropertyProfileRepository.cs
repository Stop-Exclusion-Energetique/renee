using System.Data;
using Microsoft.EntityFrameworkCore;
using Renee.Application.Interfaces;
using Renee.Domain.DomainExtension;
using Renee.Domain.DomainExtension.ToRepository;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public sealed class CopropertyProfileRepository(
    ReneeDbContext dbContext,
    IDbContextFactory<ReneeDbContext> contextFactory,
    ITelemetryService telemetryService)
    : ICopropertyProfileRepository
{
    public async Task<int> AddCopropertyProfile(CopropertyProfile copropertyProfile)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var alreadyExists = await context.CopropertyProfiles.FirstOrDefaultAsync(c =>
            c.CopropertyReference == copropertyProfile.CopropertyReference && c.IsDeleted == false);
            if (alreadyExists != null) throw new DuplicateNameException(
                $"Une fiche avec la référence {copropertyProfile.CopropertyReference} existe déjà.");
            await context.CopropertyProfiles.AddAsync(copropertyProfile);
            return await context.SaveChangesAsync();
        }
        catch (DuplicateNameException ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            throw;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<int> DeleteCopropertyProfile(Guid copropertyProfileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles.FindAsync(copropertyProfileId);
            if (copropertyProfile != null)
            {
                copropertyProfile.IsDeleted = true;
                context.Entry(copropertyProfile).State = EntityState.Modified;
            }
            return await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileAsync(Guid copropertyProfileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertyGovernanceNavigation)
                .Include(cp => cp.CopropertyDiagnosticsNavigation)
                .Include(cp => cp.CopropertyWorkFinanceNavigation)
                    .ThenInclude(cp => cp.WorkPackages)
                        .ThenInclude(pr => pr.WorkPackageWorkTypeCosts).ThenInclude(wp => wp.WorkTypeNavigation)
                .Include(cp => cp.CopropertyWorkTrackingNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

            if (copropertyProfile is not null) context.Entry(copropertyProfile).State = EntityState.Detached;

            return copropertyProfile;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return null;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileSynthesis(Guid copropertyProfileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

            if (copropertyProfile is not null) context.Entry(copropertyProfile).State = EntityState.Detached;

            return copropertyProfile;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return null;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileForIdentificationMilestoneAsync(Guid copropertyProfileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertyGovernanceNavigation)
                .Include(cp => cp.CopropertyDiagnosticsNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

            if (copropertyProfile is not null) context.Entry(copropertyProfile).State = EntityState.Detached;

            return copropertyProfile;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex); return null;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(Guid copropertyProfileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles
                .Include(cp => cp.CopropertyWorkFinanceNavigation)
                    .ThenInclude(cp => cp.WorkPackages)
                        .ThenInclude(pr => pr.WorkPackageWorkTypeCosts).ThenInclude(wp => wp.WorkTypeNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

            if (copropertyProfile is not null) context.Entry(copropertyProfile).State = EntityState.Detached;

            return copropertyProfile;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return null;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileForRealizeAndFollowMilestoneAsync(Guid copropertyProfileId)
    {
        var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles
                .Include(cp => cp.CopropertyWorkTrackingNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

            if (copropertyProfile is not null) context.Entry(copropertyProfile).State = EntityState.Detached;

            return copropertyProfile;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return null;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateCopropertyProfileSynthesis(CopropertyProfile copropertyProfile)
    {
        try
        {
            dbContext.Entry(copropertyProfile).State = EntityState.Modified;
            var result = await dbContext.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
        finally { dbContext.ChangeTracker.Clear(); }
    }

    public async Task<int> UpdateCopropertyProfileForIdentificationMilestoneAsync(CopropertyProfile copropertyProfile)
    {
        try
        {
            var context = await contextFactory.CreateDbContextAsync();
            context.Entry(copropertyProfile).State = EntityState.Modified;
            context.Entry(copropertyProfile.CopropertyHousingNavigation).State = EntityState.Modified;
            context.Entry(copropertyProfile.CopropertyGovernanceNavigation).State = EntityState.Modified;
            context.Entry(copropertyProfile.CopropertyDiagnosticsNavigation).State = EntityState.Modified;
            context.Entry(copropertyProfile.CopropertyHousingNavigation.HousingAddressNavigation).State = EntityState.Modified;

            var result = await context.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
    }

    public async Task<int> UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
        CopropertyProfile copropertyProfile,
        List<WorkPackage> workPackagesToAdd,
        List<WorkPackage> workPackagesToRemove,
        List<WorkPackageToUpdate> workPackagesToUpdate)
    {
        try
        {
            var context = await contextFactory.CreateDbContextAsync();
            context.Entry(copropertyProfile).State = EntityState.Modified;
            context.Entry(copropertyProfile.CopropertyWorkFinanceNavigation).State = EntityState.Modified;

            UpdateWorkPackages(context, workPackagesToUpdate);
            AddOrRemoveWorkPackagesToAddOrRemove(
                context,
                workPackagesToAdd,
                workPackagesToRemove);

            var result = await context.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
    }

    public async Task<int> UpdateCopropertyProfileForForRealizeAndFollowMilestoneAsync(
        CopropertyProfile copropertyProfile)
    {
        try
        {
            var context = await contextFactory.CreateDbContextAsync();
            context.Entry(copropertyProfile).State = EntityState.Modified;
            context.Entry(copropertyProfile.CopropertyWorkTrackingNavigation).State = EntityState.Modified;

            var result = await context.SaveChangesAsync();
            return result;

        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return -1;
        }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileForHeadBand(Guid copropertyProfileId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var copropertyProfile = await context.CopropertyProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

            if (copropertyProfile is not null) context.Entry(copropertyProfile).State = EntityState.Detached;

            return copropertyProfile;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return null;
        }
        finally { context.ChangeTracker.Clear(); }
    }

    public async Task<List<CopropertyProfile>?> GetAllCopropertyProfilesBySolidarBuilderReportingStructure(Guid reportingStructureId)
    {
        try
        {
            var copropertyProfiles = await dbContext.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                    .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                    .ThenInclude(su => su.SolidarBuilderNavigation.ReportingStructureNavigation)
                .AsNoTracking()
                .Where(cp => cp.CopropertySupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureId == reportingStructureId && cp.IsDeleted == false)
                .ToListAsync();

            return copropertyProfiles;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return [];
        }
    }

    public async Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForAssociationMemberDisplay()
    {
        try
        {
            var copropertyProfiles = await dbContext.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                    .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .Where(cp => cp.IsDeleted == false)
                .ToListAsync();

            return copropertyProfiles;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return [];
        }
    }

    public async Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForCoordinators()
    {
        try
        {
            var copropertyProfiles = await dbContext.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                    .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
                .Where(cp =>
                    cp.IsDeleted == false &&
                    cp.ZeroEnergyExclusionTerritoriesProgram == true)
                .ToListAsync();

            return copropertyProfiles;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return [];
        }
    }

    public async Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForTerritorialBuilder(Guid UserId)
    {
        try
        {
            var copropertyProfiles = await dbContext.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                    .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                .AsNoTracking()
        .Where(cp =>
            cp.IsDeleted == false &&
            cp.ZeroEnergyExclusionTerritoriesProgram == true &&
            (cp.CopropertySupportTeamNavigation.TerritorialBuilder == UserId || cp.CopropertySupportTeamNavigation.SecondTerritorialBuilder == UserId))
        .ToListAsync();

            return copropertyProfiles;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return [];
        }
    }

    public async Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForStructuralReferent(Guid reportingStructureId)
    {
        try
        {
            var nationalStructureId = await dbContext.ReportingStructures.AsNoTracking()
            .Where(rs => rs.Id == reportingStructureId)
            .Select(rs => rs.NationalStructureId)
            .FirstOrDefaultAsync();

            if (nationalStructureId == null)
                return [];

            var copropertyProfiles = await dbContext.CopropertyProfiles
                .Include(cp => cp.CopropertyHousingNavigation)
                    .ThenInclude(cp => cp.HousingAddressNavigation)
                .Include(cp => cp.CopropertySupportTeamNavigation)
                    .ThenInclude(su => su.SolidarBuilderNavigation.ReportingStructureNavigation)
                .AsNoTracking()
                .Where(cp => cp.CopropertySupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation != null &&
                cp.CopropertySupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation.NationalStructureId == nationalStructureId &&
                cp.IsDeleted == false)
                .ToListAsync();

            return copropertyProfiles;
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);
            return [];
        }
    }

    public async Task<CopropertyProfile?> GetCopropertyProfileSupportTeam(Guid copropertyProfileId)
    {
        var copropertyProfile = await dbContext.CopropertyProfiles
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.SolidarBuilderNavigation).ThenInclude(u => u.Role)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.SolidarBuilderNavigation).ThenInclude(u => u.ReportingStructureNavigation)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.SecondSolidarBuilderNavigation).ThenInclude(u => u!.Role)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.SecondSolidarBuilderNavigation).ThenInclude(u => u!.ReportingStructureNavigation)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.ThirdSolidarBuilderNavigation).ThenInclude(u => u!.Role)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.ThirdSolidarBuilderNavigation).ThenInclude(u => u!.ReportingStructureNavigation)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.DiffuseCoordinatorNavigation).ThenInclude(u => u!.Role)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.TargetCoordinatorNavigation).ThenInclude(u => u!.Role)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.TerritorialBuilderNavigation).ThenInclude(u => u!.Role)
            .Include(cp => cp.CopropertySupportTeamNavigation)
                .ThenInclude(spt => spt.SecondTerritorialBuilderNavigation).ThenInclude(u => u!.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(cp => cp.Id == copropertyProfileId);

        return copropertyProfile;
    }

    public async Task<int> UpdateCopropertyProfileSupportTeam(Guid supportTeamId, 
        User user,
        string supportTeamMemberRole)
    {

        using var context = await contextFactory.CreateDbContextAsync();
        try
        {
            var supportTeam = await context.SupportTeams.Include(st => st.SolidarBuilderNavigation)
                .Include(st => st.SecondSolidarBuilderNavigation).Include(st => st.ThirdSolidarBuilderNavigation)
                .Include(st => st.TerritorialBuilderNavigation).Include(st => st.SecondTerritorialBuilderNavigation)
                .Include(st => st.DiffuseCoordinatorNavigation).Include(st => st.TargetCoordinatorNavigation)
                .FirstOrDefaultAsync(st => st.Id == supportTeamId);

            if (supportTeam == null) return 0;
            SupportTeamExtension.UpdateSupportTeamMember(supportTeam, user, supportTeamMemberRole, context);

            context.Entry(supportTeam).State = EntityState.Modified;

            return await context.SaveChangesAsync();
        }
        catch (Exception ex) {

            await telemetryService.TrackExceptionAsync(ex); 
            return -1; 
        }
    }


    private static void UpdateWorkPackages(DbContext dbContext, IEnumerable<WorkPackageToUpdate> workPackagesToUpdate)
    {
        foreach (var workPackage in workPackagesToUpdate)
        {
            dbContext.Entry(workPackage.UpdatedWorkPackage).State = EntityState.Modified;
            if (workPackage.WorkTypeCostsToAdd.Count > 0)
                UpdateCopropertyEntities(dbContext, workPackage.WorkTypeCostsToAdd, EntityState.Added);

            if (workPackage.WorkTypeCostsToUpdate.Count > 0)
                UpdateCopropertyEntities(dbContext, workPackage.WorkTypeCostsToUpdate, EntityState.Modified);

            if (workPackage.WorkTypeCostsToRemove.Count > 0)
                UpdateCopropertyEntities(dbContext, workPackage.WorkTypeCostsToRemove, EntityState.Deleted);
        }
    }

    private static void AddOrRemoveWorkPackagesToAddOrRemove(
        DbContext dbContext,
        IEnumerable<WorkPackage> workPackageToAdd,
        IEnumerable<WorkPackage> workPackageToRemove)
    {
        foreach (var entity in workPackageToAdd)
        {
            dbContext.Entry(entity).State = EntityState.Added;
            if (entity.WorkPackageWorkTypeCosts.Count > 0)
                UpdateCopropertyEntities(dbContext, entity.WorkPackageWorkTypeCosts, EntityState.Added);
        }

        foreach (var entity in workPackageToRemove)
        {
            dbContext.Entry(entity).State = EntityState.Deleted;
            dbContext.Entry(entity).State = EntityState.Deleted;
            UpdateCopropertyEntities(dbContext, entity.WorkPackageWorkTypeCosts, EntityState.Deleted);
        }
    }

    private static void UpdateCopropertyEntities<T>(DbContext dbContext, IEnumerable<T> entitiesToUpdate, EntityState entityState)
        where T : class
    {
        foreach (var entity in entitiesToUpdate) dbContext.Entry(entity).State = entityState;
    }
}
