using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Occupant;

public class GetFinancialAidFromAnahQuery(int? peopleNumber, bool? isInIleDeFrance, double? resources, DateTime? startOfAccompanyingDate)
	: IRequest<ReneeOperationResult<string?>>
{
	public int? PeopleNumber { get; } = peopleNumber;
	public bool? IsInIleDeFrance { get; } = isInIleDeFrance;
	public double? Resources { get; } = resources;
	public DateTime? StartOfAccompanyingDate { get; } = startOfAccompanyingDate;
}