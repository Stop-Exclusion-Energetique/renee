using Renee.Application.Abstraction.Query;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetAccompanyingFileForFileListQuery(
	Guid UserId,
	string Role,
	AccompanyingFilesListFilterOptions FilterOptions,
	int NumberOfItempsToSkip,
	int PageSize) : IQuery<ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>>;