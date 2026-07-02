using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension;

public static class SupportTeamExtension
{
    public static void UpdateSupportTeamMember(SupportTeam supportTeam, User entity, string role, DbContext dbContext)
    {
        switch (role)
        {
            case Labels.ReferentSolidarBuilder:
                AssignUserToRole(
                    entity,
                    u => supportTeam.SolidarBuilder = u.Id,
                    () => supportTeam.SolidarBuilderNavigation,
                    u => supportTeam.SolidarBuilderNavigation = u,
                    dbContext);
                break;

            case $"Second {Labels.ReferentSolidarBuilder}":
                AssignUserToRole(
                    entity,
                    u => supportTeam.SecondSolidarBuilder = u.Id,
                    () => supportTeam.SecondSolidarBuilderNavigation,
                    u => supportTeam.SecondSolidarBuilderNavigation = u,
                    dbContext);
                break;

            case $"Troisième {Labels.ReferentSolidarBuilder}":
                AssignUserToRole(
                    entity,
                    u => supportTeam.ThirdSolidarBuilder = u.Id,
                    () => supportTeam.ThirdSolidarBuilderNavigation,
                    u => supportTeam.ThirdSolidarBuilderNavigation = u,
                    dbContext);
                break;

            case Labels.TrustedTier:
                supportTeam.TrustedTierFirstName = entity.FirstName;
                supportTeam.TrustedTierLastName = entity.LastName;
                supportTeam.TrustedTierEmail = entity.Email;
                supportTeam.TrustedTierPhoneNumber = entity.PhoneNumber;
                break;

            case Labels.ReferentEt:
                AssignUserToRole(
                    entity,
                    u => supportTeam.TerritorialBuilder = u.Id,
                    () => supportTeam.TerritorialBuilderNavigation,
                    u => supportTeam.TerritorialBuilderNavigation = u,
                    dbContext);
                break;

            case Labels.SecondReferentEt:
                AssignUserToRole(
                    entity,
                    u => supportTeam.SecondTerritorialBuilder = u.Id,
                    () => supportTeam.SecondTerritorialBuilderNavigation,
                    u => supportTeam.SecondTerritorialBuilderNavigation = u,
                    dbContext);
                break;

            case Labels.ReferentDiffuseCoordinator:
                AssignUserToRole(
                    entity,
                    u => supportTeam.DiffuseCoordinator = u.Id,
                    () => supportTeam.DiffuseCoordinatorNavigation,
                    u => supportTeam.DiffuseCoordinatorNavigation = u,
                    dbContext);
                break;

            case Labels.ReferentTargetedCoordinator:
                AssignUserToRole(
                    entity,
                    u => supportTeam.TargetCoordinator = u.Id,
                    () => supportTeam.TargetCoordinatorNavigation,
                    u => supportTeam.TargetCoordinatorNavigation = u,
                    dbContext);
                break;

            default: throw new ArgumentException($"Role not found: {role}");
        }
    }

    private static void AssignUserToRole(
        User entity,
        Action<User> assignUserId,
        Func<User?> getNavigationProperty,
        Action<User> setNavigationProperty,
        DbContext dbContext)
    {
        var existingUser = getNavigationProperty();

        if (existingUser != null)
        {
            dbContext.Entry(existingUser).State = EntityState.Detached;
            assignUserId(entity);
            MapUserProperties(existingUser, entity);
        }
        else
        {
            var newUser = new User
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                RoleId = entity.RoleId,
                TerritoryId = entity.TerritoryId,
                ReportingStructureId = entity.ReportingStructureId
            };

            assignUserId(newUser);
            setNavigationProperty(newUser);
        }
    }
    private static void MapUserProperties(User target, User source)
    {
        target.FirstName = source.FirstName;
        target.LastName = source.LastName;
        target.Email = source.Email;
        target.PhoneNumber = source.PhoneNumber;
    }
}
