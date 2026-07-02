using MediatR;
using Renee.Application.DTOs.Task;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record UpdateTaskCommandInput(TaskDto TaskDto) : IRequest<ReneeOperationResult<int>>;