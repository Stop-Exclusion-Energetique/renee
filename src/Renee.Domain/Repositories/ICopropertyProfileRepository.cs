using Renee.Domain.DomainExtension.ToRepository;
using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface ICopropertyProfileRepository
{
    Task<int> AddCopropertyProfile(CopropertyProfile copropertyProfile);

    Task<int> DeleteCopropertyProfile(Guid copropertyProfileId);

    Task<CopropertyProfile?> GetCopropertyProfileAsync(Guid copropertyProfileId);

    Task<CopropertyProfile?> GetCopropertyProfileSynthesis(Guid copropertyProfileId);

    Task<CopropertyProfile?> GetCopropertyProfileForIdentificationMilestoneAsync(Guid copropertyProfileId);

    Task<CopropertyProfile?> GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(Guid copropertyProfileId);

    Task<CopropertyProfile?> GetCopropertyProfileForRealizeAndFollowMilestoneAsync(Guid copropertyProfileId);

    Task<int> UpdateCopropertyProfileSynthesis(CopropertyProfile copropertyProfile);

    Task<int> UpdateCopropertyProfileForIdentificationMilestoneAsync(CopropertyProfile copropertyProfile);

    Task<int> UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
        CopropertyProfile copropertyProfile,
        List<WorkPackage> workPackagesToAdd,
        List<WorkPackage> workPackagesToRemove,
        List<WorkPackageToUpdate> workPackagesToUpdate);

    Task<int> UpdateCopropertyProfileForForRealizeAndFollowMilestoneAsync(CopropertyProfile copropertyProfile);

    Task<CopropertyProfile?> GetCopropertyProfileForHeadBand(Guid copropertyProfileId);

    Task<List<CopropertyProfile>?> GetAllCopropertyProfilesBySolidarBuilderReportingStructure(Guid reportingStructureId);

    Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForAssociationMemberDisplay();

    Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForCoordinators();

    Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForTerritorialBuilder(Guid UserId);

    Task<List<CopropertyProfile>?> GetAllCopropertyProfilesForStructuralReferent(Guid reportingStructureId);

    Task<CopropertyProfile?> GetCopropertyProfileSupportTeam(Guid copropertyProfileId);

    Task<int> UpdateCopropertyProfileSupportTeam(Guid supportTeamId, User user, string supportTeamMemberRole);
}
