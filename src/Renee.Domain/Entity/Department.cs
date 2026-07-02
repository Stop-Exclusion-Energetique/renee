namespace Renee.Domain.Entity;

public class Department
{
	public Guid Id { get; set; }

	public string Number { get; set; } = null!;

	public string Name { get; set; } = null!;
}