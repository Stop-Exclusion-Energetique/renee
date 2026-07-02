using MediatR;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record ValidateAccompanyingFileStageChangeCommandInput(
	Guid AccompanyingFileId, 
	bool IsValidated,
	Guid UserId,
	string? CommentOnValidation) : IRequest<ReneeOperationResult<RequiredDataForStageValidationEmail>>;

public record RequiredDataForStageValidationEmail(
	string ValidatorEmail,
	List<string> SolidarBuildersEmail,
	string MainOccupantFullName,
	string SolidarBuilderFirstName,
	string AccompanyingFileReference,
	AccompanyingFileStage CurrentStage,
	AccompanyingFileStage UpdatedStage);