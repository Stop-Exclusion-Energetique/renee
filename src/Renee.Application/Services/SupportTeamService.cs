using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services
{
	public class SupportTeamService(IMediator mediator) : ISupportTeamService
	{
		public Task<ReneeOperationResult<int>> UpdateSupportTeam(UpdateSupportTeamMemberCommandInput input)
			=> mediator.Send(input);
	}
}