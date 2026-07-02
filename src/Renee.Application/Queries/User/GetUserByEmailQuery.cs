using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;
namespace Renee.Application.Queries.User;

public record GetUserByEmailQuery(
    string? SolidarBuilderEmail,
    string? ExternalReference)
    : IRequest<ReneeOperationResult<UserDto?>>;