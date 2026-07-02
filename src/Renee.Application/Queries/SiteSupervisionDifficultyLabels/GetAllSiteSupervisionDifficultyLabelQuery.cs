using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.SiteSupervision;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.SiteSupervisionDifficultyLabels;

public record GetAllSiteSupervisionDifficultyLabelQuery : IQuery<ReneeOperationResult<IEnumerable<SiteSupervisionDifficultyDto>>>;
