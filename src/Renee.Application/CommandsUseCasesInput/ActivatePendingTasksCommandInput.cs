using MediatR;

namespace Renee.Application.CommandsUseCasesInput;

public record ActivatePendingTasksCommandInput(Guid AccompanyingFileId) : IRequest;
