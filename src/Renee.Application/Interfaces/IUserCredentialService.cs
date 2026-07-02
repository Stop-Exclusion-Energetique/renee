using Renee.Application.DTOs.User;
using Renee.Application.Queries.UserCredential;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IUserCredentialService
{
	Task<ReneeOperationResult<RegisteredUserDto?>> GetRegisteredUser(GetRegisteredUserOnCredentialsQuery getRegisteredUserOnCredentialsQuery);
	Task<ReneeOperationResult<bool>> LinkReportingStructureWithUnRegisteredUser(Guid userId, string? reportingStructureName);
	Task<ReneeOperationResult<bool>> StickUserToNoReportingStructure(Guid userId);
}