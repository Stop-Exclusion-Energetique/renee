using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record AbortAccompanyingFileCommandInput(
	Guid UserId,
	Guid AccompanyingFileId, 
	Guid AbortReasonLabelId,
	string SolidarBuilderComment,
	bool IsBillingRequested,
	bool HasAttachment) : IRequest<ReneeStringOperationResult>;