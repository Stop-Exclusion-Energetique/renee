using Renee.Domain.Enums;

namespace Renee.Application.CommandsUseCasesInput.Results;

public record RequiredDataForStageSynthesisValidationEmail(
	AccompanyingFileStage AccompanyingFileStage,
	string MainOccupantFullName,
	AccompanyingType? AccompanyingType,
	string SolidarBuilderFullName,
	List<string> UsersEmail);