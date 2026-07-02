using MediatR;
using Renee.Application.CommandsUseCasesInput.Results;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveRealizeAndFollowMilestoneSynthesisValidationCommandInput(
	Guid AccompanyingFileId, 
	Guid UserId, 
	bool UserCanValidateSynthesis) : IRequest<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>>;