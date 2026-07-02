namespace Renee.Domain.ReneeError;

public class ReneeStringOperationResult : ReneeOperationResult<string?>
{
	private ReneeStringOperationResult(bool isSuccess, string? message) : base(
		isSuccess,
		default,
		message)
	{}

	public new static ReneeStringOperationResult Failure(string? message) => new(false, message);

	public static ReneeStringOperationResult Success(string? message) => new(true, message);
}