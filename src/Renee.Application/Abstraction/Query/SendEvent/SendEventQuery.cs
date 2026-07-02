using MediatR;

namespace Renee.Application.Abstraction.Query.SendEvent;

public class SendEventQuery(IMediator mediator) : ISendEventQuery
{
	public Task<TResult> Send<TResult>(IQuery<TResult> query) => mediator.Send(query);
}