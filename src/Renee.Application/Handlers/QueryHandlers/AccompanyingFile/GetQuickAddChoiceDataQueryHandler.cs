using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.Territory;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetQuickAddChoiceDataQueryHandler(
	IUserRepository userRepository, 
	ITerritoryRepository territoryRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetQuickAddChoiceDataQuery, ReneeOperationResult<GetQuickAddChoiceDataQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetQuickAddChoiceDataQueryObjectResult>> HandleQuery(GetQuickAddChoiceDataQuery request)
	{
		try
		{
			var users = await userRepository.GetUsersForQuickAdd();
			var territories = await territoryRepository.GetAllTerritories();
			return ReneeOperationResult<GetQuickAddChoiceDataQueryObjectResult>.Success(new GetQuickAddChoiceDataQueryObjectResult
			{
				TerritorialBuilders = GetRoleUser(users, Constants.TerritorialBuilderRole, request.ShouldRetrieveFakeUser, includeTerritory: true),
				SolidarBuilders = GetRoleUser(users, Constants.SolidarBuilderRole, request.ShouldRetrieveFakeUser),
				DiffuseCoordinators = GetRoleUser(users, Constants.DiffuseCoordinatorRole, request.ShouldRetrieveFakeUser),
				TargetCoordinators = GetRoleUser(users, Constants.TargetedCoordinatorRole, request.ShouldRetrieveFakeUser, includeTerritory: true),
				Territories = territories.Select(x => new TerritoryQueryObjectResult(x.Label, x.Id)).ToList()
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetQuickAddChoiceDataQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static List<QuickAddUserResult> GetRoleUser(List<Domain.Entity.User> users, string role, bool shouldRetrieveFakeUser, bool includeTerritory = false)
	{
		if(shouldRetrieveFakeUser)
		{
			return users
				.Where(x => x.Role.Name == role && !x.IsDeleted)
				.Select(x => new QuickAddUserResult($"{x.LastName} {x.FirstName}", x.Id, TerritoryId: includeTerritory ? x.TerritoryId : null))
				.ToList();
		}
		else
		{
			return users
					.Where(x => x.Role.Name == role && (x.IsFakeUser == false || x.IsFakeUser is null) && !x.IsDeleted)
					.Select(x => new QuickAddUserResult($"{x.LastName} {x.FirstName}", x.Id, TerritoryId: includeTerritory ? x.TerritoryId : null))
					.ToList();
		}
	}
}