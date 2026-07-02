using MediatR;
using Renee.Application.Commands.ReportingStructure;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.UserCredential;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class UserCredentialService(IMediator mediator) : IUserCredentialService
{
	public async Task<ReneeOperationResult<RegisteredUserDto?>> GetRegisteredUser(
		GetRegisteredUserOnCredentialsQuery getRegisteredUserOnCredentialsQuery) =>
		await mediator.Send(getRegisteredUserOnCredentialsQuery);

	public async Task<ReneeOperationResult<bool>> LinkReportingStructureWithUnRegisteredUser(Guid userId, string? reportingStructureName)
	{
		try
		{
			var reportingStructureId = await mediator.Send(new AddReportingStructureCommand(reportingStructureName));

			if (reportingStructureId.IsSuccess)
				return await mediator.Send(new ChangeUserReportingStructureCommand(userId, reportingStructureId.Value));

			return ReneeOperationResult<bool>.Failure(reportingStructureId.Message ?? "Failed to create reporting structure");

		}
		catch (Exception ex)
		{
			return ReneeOperationResult<bool>.Failure(ex.Message);
		}
	}

	public async Task<ReneeOperationResult<bool>> StickUserToNoReportingStructure(Guid userId) =>
		await mediator.Send(new ChangeUserReportingStructureCommand(userId, Guid.Empty));
}