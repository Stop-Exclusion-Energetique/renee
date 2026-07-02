using System.Security.Claims;
using Renee.Domain;
using Renee.Domain.Entity;

namespace Renee.McpServer.Extensions;

public static class AccompanyingFileAccessFilter
{
	public static IQueryable<AccompanyingFile> ApplyUserAccessFilter(
		this IQueryable<AccompanyingFile> query,
		ClaimsPrincipal user)
	{
		var role = user.FindFirst(ClaimTypes.Role)?.Value;
		var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (role is Constants.AdminRole or Constants.AssociationMemberRole)
			return query;

		if (!Guid.TryParse(userIdString, out var userId))
			return query.Where(af => false);

		return role switch
		{
			Constants.SolidarBuilderRole => query.Where(af =>
				af.AccompanyingFileSupportTeamNavigation.SolidarBuilder == userId ||
				af.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == userId ||
				af.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == userId),

			Constants.TerritorialBuilderRole => query.Where(af =>
				af.ZeroEnergyExclusionTerritoriesProgram == true &&
				(af.AccompanyingFileSupportTeamNavigation.TerritorialBuilder == userId ||
				 af.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder == userId)),

			Constants.DiffuseCoordinatorRole => query.Where(af =>
				af.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator == userId),

			Constants.TargetedCoordinatorRole => query.Where(af =>
				af.AccompanyingFileSupportTeamNavigation.TargetCoordinator == userId),

			_ => query.Where(af => false)
		};
	}
}
