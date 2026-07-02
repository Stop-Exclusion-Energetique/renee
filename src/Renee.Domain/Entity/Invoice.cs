namespace Renee.Domain.Entity;

public class Invoice
{
	public Guid Id { get; set; }

	public Guid AccompanyingFileId { get; set; }

	public double? InvoiceCost { get; set; }

	public double? LaborCost { get; set; }

	public virtual AccompanyingFile AccompanyingFile { get; set; } = null!;
}