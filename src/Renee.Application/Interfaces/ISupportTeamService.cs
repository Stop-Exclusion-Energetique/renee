using Renee.Application.CommandsUseCasesInput;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface ISupportTeamService
{
	Task<ReneeOperationResult<int>> UpdateSupportTeam(UpdateSupportTeamMemberCommandInput input);
}
