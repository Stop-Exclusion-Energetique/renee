using MediatR;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveCopropertyMilestoneSynthesisValidationCommandInput(
    Guid CopropertyProfileId,
    AccompanyingFileStatus Status,
    AccompanyingFileStage Milestone,
    Guid UserId) : IRequest<ReneeStringOperationResult>;
