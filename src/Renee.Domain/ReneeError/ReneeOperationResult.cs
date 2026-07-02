namespace Renee.Domain.ReneeError;

public class ReneeOperationResult<T>
{
	public bool IsSuccess { get; init; }
	public T? Value { get; init; }
	public string? Message { get; init; }
	public string? ReplyUrl { get; init; }

	protected ReneeOperationResult(bool isSuccess, T? value, string? message, string? replyUrl = null)
	{
		IsSuccess = isSuccess;
		Value = value;
		Message = message;
		ReplyUrl = replyUrl;
	}

	public static ReneeOperationResult<T> Failure(string? message) => new(false, default, message);

	public static ReneeOperationResult<T> Success(T value, string? message = null, string? replyUrl = null) => new(true, value, message, replyUrl);
}