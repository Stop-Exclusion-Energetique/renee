using Renee.Application.Abstraction.Query;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateDocumentGenerationLogCommandInput(
	Guid UserId, 
	string FileName) : IQuery<int>;