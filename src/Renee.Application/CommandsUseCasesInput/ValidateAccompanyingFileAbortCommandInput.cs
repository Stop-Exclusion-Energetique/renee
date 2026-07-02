using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record ValidateAccompanyingFileAbortCommandInput (
	Guid AccompanyingFileId,
	string? CommentsOnAccompanyingFileAbort,
	bool ShouldAbort,
	Guid UserId) : IRequest<ReneeStringOperationResult>;
