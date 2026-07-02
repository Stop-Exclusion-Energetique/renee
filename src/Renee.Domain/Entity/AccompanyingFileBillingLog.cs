namespace Renee.Domain.Entity;

public class AccompanyingFileBillingLog
{
	public Guid Id { get; set; }
	public Guid AccompanyingFileId { get; set; }
	
	public bool BilledJalon1 { get; set; }
	public double? AmountBilledFirstStage { get; set; }
	public DateTime? FundraisingLauchDateForFirstStage { get; set; }
	public string? BillingCallNumberFirstStage { get; set; }
	public string? InvoiceNumberFirstStage { get; set; }
	public DateTime? BillingDateFirstStage { get; set; }
	
	public bool BilledJalon2 { get; set; }
	public double? AmountBilledSecondStage { get; set; }
	public DateTime? FundraisingLauchDateForSecondStage { get; set; }
	public string? BillingCallNumberSecondStage { get; set; }
	public string? InvoiceNumberSecondStage { get; set; }
	public DateTime? BillingDateSecondStage { get; set; }
	
	public bool BilledJalon3 { get; set; }
	public double? AmountBilledThirdStage { get; set; }
	public DateTime? FundraisingLauchDateForThirdStage { get; set; }
	public string? BillingCallNumberThirdStage { get; set; }
	public string? InvoiceNumberThirdStage { get; set; }
	public DateTime? BillingDateThirdStage { get; set; }

	public DateTime LastUpdate { get; set; }
	public virtual AccompanyingFile AccompanyingFileNavigation { get; set; } = null!;
}
