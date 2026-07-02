using MediatR;

namespace Renee.Application.Abstraction.Query;

public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
	public async Task<TResult> Handle(TQuery request, CancellationToken cancellationToken)
	{
		return await HandleQuery(request);
	}

	public abstract Task<TResult> HandleQuery(TQuery request);
}