using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension;

public static class CopropertyProfileExtension
{
    public static CopropertyProfile InitializeCopropertyProfile(Guid userId)
    {
        return new CopropertyProfile
        {
            CopropertyHousingNavigation = new CopropertyHousing(),
            CopropertyGovernanceNavigation = new CopropertyGovernance(),
            CopropertyDiagnosticsNavigation = new CopropertyDiagnostics(),
            CopropertyWorkFinanceNavigation = new CopropertyWorkFinance(),
            CopropertyWorkTrackingNavigation = new CopropertyWorkTracking(),
            CopropertyMilestone = (int)AccompanyingFileStage.Identify,
            CopropertyStatus = (int)AccompanyingFileStatus.InProgress,
            CreatedBy = userId,
            CreationDate = DateTime.UtcNow
        };
    }

    public static CopropertyProfile QuickAddHousing(
        this CopropertyProfile copropertyProfile,
        Address address, 
        int typology)
    {
        copropertyProfile.CopropertyHousingNavigation.GeographicAreaTypology = typology;
        copropertyProfile.CopropertyHousingNavigation.HousingAddressNavigation = address;
        return copropertyProfile;
    }

    public static CopropertyProfile QuickAddSupportTeam(this CopropertyProfile copropertyProfile, SupportTeam supportTeam)
    {
        copropertyProfile.CopropertySupportTeamNavigation = supportTeam;
        return copropertyProfile;
    }

}
