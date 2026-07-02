using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class FinancialAidService(IMediator mediator) : IFinancialAidService
{
	public async Task<ReneeOperationResult<string?>> GetFinancialAidFromAnah(int? peopleNumber, bool? isInIleDeFrance, double? resources, DateTime? startOfAccompanyingDate) =>
		await mediator.Send(new GetFinancialAidFromAnahQuery(peopleNumber, isInIleDeFrance, resources, startOfAccompanyingDate));
}