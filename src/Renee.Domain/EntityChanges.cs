namespace Renee.Domain;

public record EntityChanges<T>(
	IReadOnlyCollection<T> ToAdd,
	IReadOnlyCollection<T> ToUpdate,
	IReadOnlyCollection<T> ToRemove) where T : class
{
	public static EntityChanges<T> Empty =>
		new([], [], []);
}