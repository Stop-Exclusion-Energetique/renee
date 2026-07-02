using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IFinancialAidService
{
	Task<ReneeOperationResult<string?>> GetFinancialAidFromAnah(int? peopleNumber, bool? isInIleDeFrance, double? resources, DateTime? startOfAccompanyingDate);
}