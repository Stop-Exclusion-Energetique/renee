using MediatR;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public sealed class GetAccompanyingFileByIdQuery(Guid id, Guid userId, string userRole) : IRequest<ReneeOperationResult<AccompanyingFileDto?>>
{
	public Guid? AccompanyingFileId { get; } = id;
	public Guid UserId { get; } = userId;
	public string UserRole { get; } = userRole;
}