namespace Renee.Domain.Entity;

public class AccompanyingFileResumeView
{
	public Guid Id { get; set; }

	public string AccompanyingFileReference { get; set; } = null!;

	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? Label { get; set; }

	public int AccompanyingFileMilestone { get; set; }

	public int AccompanyingFileStatus { get; set; }
	public int? AccompanyingType { get; set; }

	public DateTime? OpeningDate { get; set; }

	public DateTime? LastUpdateDate { get; set; }

	public DateTime? CloseDate { get; set; }

	public Guid SolidarBuilder { get; set; }

	public Guid? SecondSolidarBuilder { get; set; }

	public Guid? ReportingStructureId { get; set; }

	public Guid? DiffuseCoordinator { get; set; }

	public Guid? TargetCoordinator { get; set; }

	public Guid? AccompanyingFileTerritory { get; set; }

	public Guid? TerritorialBuilder { get; set; }
	
    public Guid? SecondTerritorialBuilder { get; set; }

	public Guid? ThirdSolidarBuilder { get; set; }

	public bool? SolidarBuilderDeleted { get; set; }

	public bool? SecondSolidarBuilderDeleted { get; set; }

	public bool? ThirdSolidarBuilderDeleted { get; set; }

	public bool? IsDeleted { get; set; }

	public bool? ZeroEnergyExclusionTerritoriesProgram { get; set; }

	public Guid? NationalStructureId { get; set; }

	public bool? ShouldAccompanyingFileBeSubmittedToAnah { get; set; }
	
	public Guid? AbortReasonLabelId { get; set; } 

	public string? OtherAbortReason { get; set; }

	public string? SolidarBuilderAbortRequestDetails { get; set; }

	public bool? IsAbortBillingRequested { get; set; }

	public bool? BilledJalon1 { get; set; }
	public double? AmountBilledFirstStage { get; set; }
	public string? InvoiceNumberFirstStage { get; set; }
	public DateTime? BillingDateFirstStage { get; set; }
	
	public bool? BilledJalon2 { get; set; }
	public double? AmountBilledSecondStage { get; set; }
	public string? InvoiceNumberSecondStage { get; set; }
	public DateTime? BillingDateSecondStage { get; set; }
	
	public bool? BilledJalon3 { get; set; }
	public double? AmountBilledThirdStage { get; set; }
	public string? InvoiceNumberThirdStage { get; set; }
	public DateTime? BillingDateThirdStage { get; set; }
}