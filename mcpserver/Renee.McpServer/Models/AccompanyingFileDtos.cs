namespace Renee.McpServer.Models;

// DTOs de projection pour les outils MCP.
// Principe RGPD de minimisation (art. 5-1-c) : on ne sérialise jamais l'entité EF complète
// (qui exposerait les identifiants techniques, clés étrangères, métadonnées d'audit, indicateurs
// de suppression logique et l'ensemble du graphe de navigation lié). Chaque outil ne renvoie au
// LLM que les champs métier strictement nécessaires à son cas d'usage.

public record HouseholdDataDto
{
	public string Reference { get; init; } = string.Empty;
	public int? Typology { get; init; }
	public bool? IsFollowedByASocialWorker { get; init; }
	public bool? HasAnOccupantWithDisabilities { get; init; }
	public bool? HasAnOccupantWithLongTermIllness { get; init; }
	public bool? HasAnOccupantWithIndependenceLoss { get; init; }
	public bool? HasAnOccupantUnderCuratorship { get; init; }
	public bool? HasAnOccupantUnderGuardianship { get; init; }
	public double? ReferenceIncomeTax { get; init; }
	public string? AnahCategory { get; init; }
	public string? SocialContext { get; init; }
	public string? HouseholdProject { get; init; }
	public string? HouseholdAvailabilityForVisits { get; init; }
	public string? CommentsOnHouseholdDifficulties { get; init; }
	public bool? HasOverdueInvoice { get; init; }
	public double? EnergyEffortRate { get; init; }
	public OccupantDto? MainOccupant { get; init; }
	public List<string> Difficulties { get; init; } = [];
	public List<LabeledValueDto> Expenses { get; init; } = [];
	public List<LabeledValueDto> Resources { get; init; } = [];
	public List<LabeledValueDto> HeatingEnergies { get; init; } = [];
}

public record OccupantDto
{
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? Trigram { get; init; }
	public int? Age { get; init; }
	public int? SocioProfessionalCategory { get; init; }
	public string? Job { get; init; }
	public string? PhoneNumber { get; init; }
	public string? Email { get; init; }
	public int? SocialProtectionFund { get; init; }
	public int? PensionFund { get; init; }
	public int? AdditionnalFund { get; init; }
}

public record LabeledValueDto
{
	public string? Label { get; init; }
	public double? Value { get; init; }
}

public record HousingDataDto
{
	public string Reference { get; init; } = string.Empty;
	public AddressDto? Address { get; init; }
	public int? GeographicAreaTypology { get; init; }
	public bool? IsInAbfArea { get; init; }
	public string? ArchitecturalOrTownPlanningStandards { get; init; }
	public int? OwnershipStatus { get; init; }
	public int? HousingType { get; init; }
	public int? ConstructionYear { get; init; }
	public double? LivingSpace { get; init; }
	public int? NumberOfRoom { get; init; }
	public int? NumberOfFloor { get; init; }
	public int? YearOfAcquisitionOrEntry { get; init; }
	public int? SunExposure { get; init; }
	public double? CeilingHeight { get; init; }
	public bool? HasPreviousWork { get; init; }
	public string? CommentOnPreviousWork { get; init; }
	public HousingInitialStateDto? InitialState { get; init; }
	public HousingAfterWorkStateDto? AfterWorkState { get; init; }
}

public record AddressDto
{
	public string? Label { get; init; }
	public string? PostalCode { get; init; }
	public string? City { get; init; }
	public string? Department { get; init; }
	public string? Region { get; init; }
	public string? HouseNumber { get; init; }
	public string? Street { get; init; }
	public string? AdditionnalComment { get; init; }
}

public record HousingInitialStateDto
{
	public int? DegradationIndex { get; init; }
	public int? UnsanitaryCoefficient { get; init; }
	public int? EnergyDepravation { get; init; }
	public int? SummerThermalComfortLevel { get; init; }
	public int? WinterThermalComfortLevel { get; init; }
	public int? NoiseComfortLevel { get; init; }
	public string? InitialStateDiagnosticCommentary { get; init; }
	public string? DisordersObservedCommentary { get; init; }
	public int? Dpe { get; init; }
	public int? Ges { get; init; }
	public double? AnnualEnergyConsumption { get; init; }
	public double? AnnualGesEmission { get; init; }
	public string? HeatingEnergy { get; init; }
}

public record HousingAfterWorkStateDto
{
	public int? EstimatedDpeClassJump { get; init; }
	public int? EstimatedDpeAfterWork { get; init; }
	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; init; }
	public double? EstimatedAnnualGesEmissionsAfterWork { get; init; }
	public int? EstimatedGesAfterWork { get; init; }
	public int? FinalDpe { get; init; }
	public int? FinalDpeClassJump { get; init; }
}

public record FinancingPlanDataDto
{
	public string Reference { get; init; } = string.Empty;
	public double? MaPrimeRenovGuidedPath { get; init; }
	public double? MaPrimeRenovCoOwnerShip { get; init; }
	public double? MaPrimeLogementDecent { get; init; }
	public double? MaPrimeAdapt { get; init; }
	public double? BonusForExitingEnergeticSieve { get; init; }
	public double? RegionalAids { get; init; }
	public double? DepartmentalAids { get; init; }
	public double? PublicEstablishmentsForInterCommunalCooperationAids { get; init; }
	public double? MunicipalityAids { get; init; }
	public string? SolicitedBankLoanType { get; init; }
	public double? ClassicBankLoan { get; init; }
	public string? RemainingAmountFinancingSource { get; init; }
	public bool? IsFinancingAskedToStopAssociation { get; init; }
	public double? EstimatedRemainingAmount { get; init; }
	public double? MdphFinancing { get; init; }
	public double? CeeFinancing { get; init; }
	public double? CafMsaFinancing { get; init; }
	public double? PensionFund { get; init; }
	public double? UnderprivilegedHousingFoundation { get; init; }
	public double? LeroyMerlinFoundation { get; init; }
	public double? WattForChangeFoundation { get; init; }
	public double? SocialProtectionGroup { get; init; }
	public double? StopEnergyExclusionFunds { get; init; }
	public double? HouseholdMaximumSavingAmountForRenovationProject { get; init; }
	public double? OtherFamilyMemberMaximumSupportAmountForRenovationProject { get; init; }
	public List<LabeledValueDto> FundingModes { get; init; } = [];
}

public record WorkPlanDataDto
{
	public string Reference { get; init; } = string.Empty;
	public int? RenovationType { get; init; }
	public string? NextStepAndVigilancePoint { get; init; }
	public bool? HasInterestInPossibleAraProcess { get; init; }
	public bool? HasNeedForTemporaryReHousing { get; init; }
	public bool? HasEmergencyWorks { get; init; }
	public bool? HasEnergeticsRenovationWorks { get; init; }
	public bool? HasInducedWorks { get; init; }
	public bool? HasSafetyAndHealthWorks { get; init; }
	public int? TreatedAirTightness { get; init; }
	public int? TreatedThermalBridge { get; init; }
	public int? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; init; }
	public string? WorksDetails { get; init; }
	public bool? IsRgeLabelUpToDate { get; init; }
	public string? OtherQualification { get; init; }
	public List<string> InsuranceTypes { get; init; } = [];
	public List<string> ProjectTypes { get; init; } = [];
	public List<WorkPackageDto> WorkPackages { get; init; } = [];
}

public record WorkPackageDto
{
	public string? EnergeticsEffectAfterWorks { get; init; }
	public List<WorkTypeCostDto> Costs { get; init; } = [];
}

public record WorkTypeCostDto
{
	public string? WorkType { get; init; }
	public double Cost { get; init; }
	public string? Description { get; init; }
}

public record BillingInfoDto
{
	public string Reference { get; init; } = string.Empty;
	public int Status { get; init; }
	public int Milestone { get; init; }
	public bool BilledJalon1 { get; init; }
	public double? AmountBilledFirstStage { get; init; }
	public DateTime? FundraisingLaunchDateForFirstStage { get; init; }
	public string? BillingCallNumberFirstStage { get; init; }
	public string? InvoiceNumberFirstStage { get; init; }
	public DateTime? BillingDateFirstStage { get; init; }
	public bool BilledJalon2 { get; init; }
	public double? AmountBilledSecondStage { get; init; }
	public DateTime? FundraisingLaunchDateForSecondStage { get; init; }
	public string? BillingCallNumberSecondStage { get; init; }
	public string? InvoiceNumberSecondStage { get; init; }
	public DateTime? BillingDateSecondStage { get; init; }
	public bool BilledJalon3 { get; init; }
	public double? AmountBilledThirdStage { get; init; }
	public DateTime? FundraisingLaunchDateForThirdStage { get; init; }
	public string? BillingCallNumberThirdStage { get; init; }
	public string? InvoiceNumberThirdStage { get; init; }
	public DateTime? BillingDateThirdStage { get; init; }
	public DateTime? LastUpdate { get; init; }
}
