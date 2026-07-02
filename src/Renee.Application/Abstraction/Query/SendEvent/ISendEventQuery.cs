namespace Renee.Application.Abstraction.Query.SendEvent;

public interface ISendEventQuery
{
	Task<TResult> Send<TResult>(IQuery<TResult> query);
}