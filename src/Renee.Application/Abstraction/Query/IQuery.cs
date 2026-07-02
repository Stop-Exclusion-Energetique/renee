using MediatR;

namespace Renee.Application.Abstraction.Query;

public interface IQuery<out TResult> : IRequest<TResult>
{
}