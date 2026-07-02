using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveAccompanyingFilesForAnahGrantCheckCommandInput(
	Guid UserId,
	List<AccompanyingFileGrantDateResume> AccompanyingFilesGrantDates)
	: IRequest<ReneeOperationResult<bool>>;

public record AccompanyingFileGrantDateResume(Guid AccompanyingFileId, DateTime? GrantDate);