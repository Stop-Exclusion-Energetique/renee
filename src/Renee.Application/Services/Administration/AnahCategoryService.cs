using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.DTOs.Administration;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Queries.Administration;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services.Administration;

public class AnahCategoryService(IMediator mediator) : IAnahCategoryService
{
	public async Task<ReneeOperationResult<bool>> CreateAnahCategory(AnahCategoryDto dto, Guid connectedUserId) =>
		await mediator.Send(new CreateAnahCategoryCommand(dto, connectedUserId));

	public async Task<ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>> GetAllAnahCategories() =>
		await mediator.Send(new GetAllAnahCategoryQuery());

	public async Task<ReneeOperationResult<bool>> UpdateAnahCategory(AnahCategoryDto dto, Guid connectedUserId) =>
		await mediator.Send(new UpdateAnahCategoryCommand(dto, connectedUserId));

	public async Task<ReneeOperationResult<bool>> CreateAnahCategoryForSuplemntaryOccupant(AnahCategorySuplementaryOccupantIncome input) =>
		await mediator.Send(new CreateAnahCategoryForSuplemntaryOccupantCommand(input) );

	public async Task<ReneeOperationResult<bool>> UpdateAnahCategoryForSuplemntaryOccupant(AnahCategorySuplementaryOccupantIncome input) =>
		await mediator.Send(new UpdateAnahCategoryForSuplemntaryOccupantCommand(input));
}