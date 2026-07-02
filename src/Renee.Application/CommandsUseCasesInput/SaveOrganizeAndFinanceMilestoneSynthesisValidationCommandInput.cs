using MediatR;
using Renee.Application.CommandsUseCasesInput.Results;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveOrganizeAndFinanceMilestoneSynthesisValidationCommandInput(
	Guid AccompanyingFileId,
	AccompanyingFileStatus Status,
	AccompanyingFileStage Milestone,
	Guid UserId) : IRequest<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>>;