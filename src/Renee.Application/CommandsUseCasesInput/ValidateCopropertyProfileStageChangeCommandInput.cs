using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record ValidateCopropertyProfileStageChangeCommandInput(
   Guid CopropertyProfileId,
   bool IsValidated,
   Guid UserId,
   string? CommentOnValidation) : IRequest<ReneeStringOperationResult>;
