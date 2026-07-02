using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;

namespace Renee.Infrastructure.Data;

public class ReneeDbContext(DbContextOptions<ReneeDbContext> options) : DbContext(options)
{
	private const string NewId = "(newid())";

	public virtual DbSet<AbortReasonLabel> AbortReasonLabels { get; set; }

	public virtual DbSet<AccompanyingFile> AccompanyingFiles { get; set; }

	public virtual DbSet<AccompanyingFileResumeView> AccompanyingFileResumeView { get; set; }

	public virtual DbSet<AccompanyingFileTask> AccompanyingFileTasks { get; set; }

	public virtual DbSet<AccompanyingFileBillingLog> AccompanyingFileBillingLogs { get; set; }

	public virtual DbSet<Address> Addresses { get; set; }

	public virtual DbSet<AnahCategory> AnahCategories { get; set; }

	public virtual DbSet<AverageAccompanyingDuration> AverageAccompanyingDurations { get; set; }

	public virtual DbSet<Department> Departments { get; set; }

	public virtual DbSet<DocumentGenerationLog> DocumentGenerationLogs { get; set; }

	public virtual DbSet<Email> Emails { get; set; }

	public virtual DbSet<FundingMode> FundingModes { get; set; }

	public virtual DbSet<Household> Households { get; set; }

	public virtual DbSet<HouseholdDifficultiesLabel> HouseholdDifficultiesLabels { get; set; }

	public virtual DbSet<HouseholdDifficulty> HouseholdDifficulties { get; set; }

	public virtual DbSet<HouseholdExpense> HouseholdExpenses { get; set; }

	public virtual DbSet<HouseholdHeatingEnergy> HouseholdHeatingEnergies { get; set; }

	public virtual DbSet<HouseholdHeatingEnergyLabel> HouseholdHeatingEnergiesLabels { get; set; } 

	public virtual DbSet<HouseholdResource> HouseholdResources { get; set; }

	public virtual DbSet<HouseholdResourcesLabel> HouseholdResourcesLabels { get; set; }

	public virtual DbSet<Housing> Housings { get; set; }

	public virtual DbSet<HousingAfterWorkState> HousingAfterWorkStates { get; set; }

	public virtual DbSet<HousingInitialState> HousingInitialStates { get; set; }

	public virtual DbSet<ImportError> ImportErrors { get; set; }

	public virtual DbSet<ImportRun> ImportRuns { get; set; }

	public virtual DbSet<InsuranceType> InsuranceTypes { get; set; }

	public virtual DbSet<Invoice> Invoices { get; set; }

	public virtual DbSet<Impersonate> Impersonates { get; set; }

	public virtual DbSet<MainOccupant> MainOccupants { get; set; }

	public virtual DbSet<NationalStructure> NationalStructures { get; set; }

	public virtual DbSet<PreFinancingPlan> PreFinancingPlans { get; set; }

	public virtual DbSet<PreWorkPlan> PreWorkPlans { get; set; }

	public virtual DbSet<PreWorkPlanInsuranceType> PreWorkPlanInsuranceTypes { get; set; }

	public virtual DbSet<PreWorkPlanProjectType> PreWorkPlanProjectTypes { get; set; }

	public virtual DbSet<ProjectType> ProjectTypes { get; set; }

	public virtual DbSet<ReportingStructure> ReportingStructures { get; set; }

	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<SecondaryOccupant> SecondaryOccupants { get; set; }

	public virtual DbSet<SupportTeam> SupportTeams { get; set; }

	public virtual DbSet<Territory> Territories { get; set; }

	public virtual DbSet<UnregisteredUser> UnregisteredUsers { get; set; }

	public virtual DbSet<User> Users { get; set; }

	public virtual DbSet<WorkMonitoring> WorkMonitorings { get; set; }

	public virtual DbSet<WorkPackage> WorkPackages { get; set; }

	public virtual DbSet<WorkPackageWorkTypeCost> WorkPackageWorkTypeCosts { get; set; }

	public virtual DbSet<WorkTypesLabel> WorkTypesLabels { get; set; }

    public virtual DbSet<CguVersion> CGUVersions { get; set; }

    public virtual DbSet<AnahCategorySuplementaryOccupantIncome> AnahCategorySuplementaryOccupantIncome { get; set; }

	public virtual DbSet<CopropertyProfile> CopropertyProfiles { get; set; }

	public virtual DbSet<CopropertyHousing> CopropertyHousings { get; set; }

	public virtual DbSet<CopropertyGovernance> CopropertyGovernances { get; set; }

	public virtual DbSet<CopropertyDiagnostics> CopropertyDiagnostics { get; set; }	

	public virtual DbSet<CopropertyWorkFinance> CopropertyWorkFinances { get; set; }

	public virtual DbSet<CopropertyWorkTracking> CopropertyWorkTrackings { get; set; }

	public virtual DbSet<SiteSupervision> SiteSupervisions { get; set; }

	public virtual DbSet<WorkParticipant> WorkParticipants { get; set; }

	public virtual DbSet<SiteSupervisionDifficultyLabel> SiteSupervisionDifficultyLabels { get; set; }

	public virtual DbSet<WorkParticipantDifficulty> WorkParticipantDifficulties { get; set; }

	public virtual DbSet<WorkTypeProjectType> WorkTypeProjectTypes { get; set; } = null!;

	public virtual DbSet<AdminConstant> AdminConstants { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AbortReasonLabel>(entity =>
		{
			entity.ToTable("AbortReasonLabel");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<AccompanyingFile>(entity =>
		{
			entity.ToTable("AccompanyingFile");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.AccompanyingFileReference).HasMaxLength(50);

			entity.HasOne(d => d.AccompanyingFileHouseholdNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFileHousehold).HasConstraintName("FK_AccompanyingFile_Household");

			entity.HasOne(d => d.AccompanyingFileHousingNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFileHousing).HasConstraintName("FK_AccompanyingFile_Housing");

			entity.HasOne(d => d.AccompanyingFilePreFinancingPlanNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFilePreFinancingPlan)
				.HasConstraintName("FK_AccompanyingFile_PreFinancementPlan");

			entity.HasOne(d => d.AccompanyingFilePreWorkPlanNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFilePreWorkPlan).HasConstraintName("FK_AccompanyingFile_PreWorkPlan");

			entity.HasOne(d => d.AccompanyingFileSupportTeamNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFileSupportTeam).HasConstraintName("FK_AccompanyingFile_SupportTeam");

			entity.HasOne(d => d.AccompanyingFileTerritoryNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFileTerritory).HasConstraintName("FK_AccompanyingFile_Territory");

			entity.HasOne(d => d.AccompanyingFileWorkMonitoringNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.AccompanyingFileWorkMonitoring)
				.HasConstraintName("FK_AccompanyingFile_WorkMonitoring");

			entity.HasOne(d => d.ClosedByNavigation).WithMany(p => p.AccompanyingFileClosedByNavigations)
				.HasForeignKey(d => d.ClosedBy).HasConstraintName("FK_AccompanyingFile_User_Closed");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccompanyingFileCreatedByNavigations)
				.HasForeignKey(d => d.CreatedBy).HasConstraintName("FK_AccompanyingFile_User_Creation");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AccompanyingFileUpdatedByNavigations)
				.HasForeignKey(d => d.UpdatedBy).HasConstraintName("FK_AccompanyingFile_User_LastUpdate");

			entity.HasOne(d => d.IdentifyMilestoneValidatedByNavigation).WithMany(p => p.AccompanyingFileIdentifyMilestoneValidatedByNavigations)
				.HasForeignKey(d => d.IdentifyMilestoneValidatedBy).HasConstraintName("FK_AccompanyingFile_User_Identify_Milestone_Validated");

			entity.HasOne(d => d.OrganizeAndFinanceMilestoneValidatedByNavigation).WithMany(p => p.AccompanyingFileOrganizeAndFinanceMilestoneValidatedByNavigations)
				.HasForeignKey(d => d.OrganizeAndFinanceMilestoneValidatedBy).HasConstraintName("FK_AccompanyingFile_User_OrganizeAndFinance_Milestone_Validated");

			entity.HasOne(d => d.RealizeAndFollowMilestoneValidatedByNavigation).WithMany(p => p.AccompanyingFileRealizeAndFollowMilestoneValidatedByNavigations)
				.HasForeignKey(d => d.RealizeAndFollowMilestoneValidatedBy).HasConstraintName("FK_AccompanyingFile_User_RealizeAndFollow_Milestone_Validated");

			entity.HasOne(d => d.AbortReasonLabel).WithMany(e => e.AccompanyingFiles)
				.HasForeignKey(d => d.AbortReasonLabelId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("FK_AccompanyingFile_AbortReasonLabel");

			entity.HasOne(d => d.AbortRequestedBy).WithMany(d => d.AccompanyingFileAbortRequestedByNavigations)
				.HasForeignKey(d => d.AbortRequestedById).HasConstraintName("FK_AccompanyingFile_User_AbortRequestedById");

			entity.HasOne(d => d.AbortDecidedBy).WithMany(d => d.AccompanyingFileAbortDecidedByNavigations)
				.HasForeignKey(d => d.AbortDecidedById).HasConstraintName("FK_AccompanyingFile_User_AbortDecidedById");

			entity.HasOne(d => d.CopropertyProfileNavigation).WithMany(p => p.AccompanyingFiles)
				.HasForeignKey(d => d.CopropertyProfileId).HasConstraintName("FK_AccompanyingFile_CopropertyProfile");
		});

		modelBuilder.Entity<AdminConstant>(entity =>
		{
			entity.ToTable("AdminConstant");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Name).HasMaxLength(100);
		});

		modelBuilder.Entity<CopropertyProfile>(entity =>
		{
			entity.ToTable("CopropertyProfile");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.CopropertyReference).HasMaxLength(50);

			entity.HasOne(d => d.CopropertyGovernanceNavigation).WithMany(c => c.CopropertyProfiles)
				.HasForeignKey(p => p.CopropertyGovernance).HasConstraintName("FK_CopropertyProfile_Gouvernance");

            entity.HasOne(d => d.CopropertySupportTeamNavigation).WithMany(c => c.CopropertyProfiles)
				.HasForeignKey(p => p.CopropertySupportTeam).HasConstraintName("FK_CopropertyProfile_supportTeam");

            entity.HasOne(d => d.CopropertyTerritoryNavigation).WithMany(p => p.CopropertyProfiles)
                .HasForeignKey(d => d.CopropertyTerritory).HasConstraintName("FK_Coproperty_Territory");

            entity.HasOne(d => d.CopropertyWorkFinanceNavigation).WithMany(c => c.CopropertyProfiles)
				.HasForeignKey(p => p.CopropertyWorkFinance).HasConstraintName("FK_CopropertyProfile_WorkFinance");

            entity.HasOne(d => d.CopropertyDiagnosticsNavigation).WithMany(c => c.CopropertyProfiles)
				.HasForeignKey(p => p.CopropertyDiagnostics).HasConstraintName("FK_CopropertyProfile_Diagnostics");

            entity.HasOne(d => d.CopropertyHousingNavigation).WithMany(c => c.CopropertyProfiles)
				.HasForeignKey(p => p.CopropertyHousing).HasConstraintName("FK_CopropertyProfile_Housing");

            entity.HasOne(d => d.CopropertyWorkTrackingNavigation).WithMany(c => c.CopropertyProfiles)
				.HasForeignKey(p => p.CopropertyWorkTraking).HasConstraintName("FK_CopropertyProfile_WorkTracking");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CopropertyProfileCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy).HasConstraintName("FK_CopropertyProfile_User_Creation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CopropertyProfileUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy).HasConstraintName("FK_CopropertyProfile_User_LastUpdate");

			entity.HasOne(d => d.ClosedByNavigation).WithMany(p => p.CopropertyProfileClosedByNavigations)
                .HasForeignKey(d => d.ClosedBy).HasConstraintName("FK_CopropertyProfile_User_Closed");

			entity.HasOne(d => d.IdentifyMilestoneValidatedByNavigation).WithMany(p => p.CopropertyIdentifyMilestoneValidatedByNavigations)
                .HasForeignKey(d => d.IdentifyMilestoneValidatedBy).HasConstraintName("FK_CopropertyProfile_User_Identify_Milestone_Validated");

            entity.HasOne(d => d.OrganizeAndFinanceMilestoneValidatedByNavigation).WithMany(p => p.CopropertyOrganizeAndFinanceMilestoneValidatedByNavigations)
                .HasForeignKey(d => d.OrganizeAndFinanceMilestoneValidatedBy).HasConstraintName("FK_CopropertyProfile_User_OrganizeAndFinance_Milestone_Validated");

            entity.HasOne(d => d.RealizeAndFollowMilestoneValidatedByNavigation).WithMany(p => p.CopropertyRealizeAndFollowMilestoneValidatedByNavigations)
                .HasForeignKey(d => d.RealizeAndFollowMilestoneValidatedBy).HasConstraintName("FK_CopropertyProfile_User_RealizeAndFollow_Milestone_Validated");
        });

		modelBuilder.Entity<CopropertyHousing>(entity =>
		{
			entity.ToTable("CopropertyHousing");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.HasOne(d => d.HousingAddressNavigation).WithMany(p => p.CopropertyHousings)
				.HasForeignKey(d => d.HousingAddress).HasConstraintName("FK_CopropertyHousing_Address");
		});

		modelBuilder.Entity<CopropertyGovernance>(entity =>
		{
			entity.ToTable("CopropertyGovernance");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.PhoneOfSyndic).HasMaxLength(50);
			entity.Property(e => e.MailOfSyndic).HasMaxLength(50);
			entity.Property(e => e.ContactOfAmo).HasMaxLength(50);
        });

		modelBuilder.Entity<CopropertyDiagnostics>(entity =>
		{
			entity.ToTable("CopropertyDiagnostics");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
            entity.Property(e => e.BuildingDpeLabel).HasMaxLength(50);
			entity.Property(e => e.ApartmentDpeLabel).HasMaxLength(50);
		});

		modelBuilder.Entity<CopropertyWorkFinance>(entity =>
		{
			entity.ToTable("CopropertyWorkFinance");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<CopropertyWorkTracking>(entity =>
		{
			entity.ToTable("CopropertyWorkTracking");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<AccompanyingFileBillingLog>(entity =>
		{
			entity.ToTable("AccompanyingFileBillingLog");

			entity.HasKey(entity => entity.Id).HasName("Pk_AccompanyingFileBillingLog");

			entity.Property(entity => entity.BilledJalon1).HasDefaultValue(false);
			entity.Property(entity => entity.BilledJalon2).HasDefaultValue(false);
			entity.Property(entity => entity.BilledJalon3).HasDefaultValue(false);
			entity.Property(entity => entity.LastUpdate).HasDefaultValueSql("SYSDATETIME()");

			entity.HasOne(entity => entity.AccompanyingFileNavigation).WithOne(af => af.AccompanyingFileBillingLog)
				.HasForeignKey<AccompanyingFileBillingLog>(e => e.AccompanyingFileId)
				.HasConstraintName("FK_BillingLog_File");
		});

		modelBuilder.Entity<AccompanyingFileResumeView>(entity =>
		{
			entity.HasNoKey().ToView("AccompanyingFileResumeView");
		});

		modelBuilder.Entity<AccompanyingFileTask>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_Task");

			entity.ToTable("AccompanyingFileTask");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.AccompanyingFile).WithMany(p => p.AccompanyingFileTasks)
				.HasForeignKey(d => d.AccompanyingFileId).OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Task_AccompanyingFile");

			entity.HasOne(d => d.AssignedUser).WithMany(p => p.AccompanyingFileTasks)
				.HasForeignKey(d => d.AssignedUserId).OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Task_User");

			entity.HasOne(d => d.CreatedByUser).WithMany(p => p.CreatedAccompanyingFileTasks)
				.HasForeignKey(d => d.CreatedByUserId).OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Task_User_CreatedBy");
		});

		modelBuilder.Entity<Address>(entity =>
		{
			entity.ToTable("Address");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.AdditionnalComment).HasMaxLength(50).IsUnicode(false);
			entity.Property(e => e.City).HasMaxLength(50);
			entity.Property(e => e.Department).HasMaxLength(50);
			entity.Property(e => e.HouseNumber).HasMaxLength(50);
			entity.Property(e => e.Label).HasMaxLength(50).IsUnicode(false);
			entity.Property(e => e.PostalCode).HasMaxLength(50);
			entity.Property(e => e.Region).HasMaxLength(50);
		});

		modelBuilder.Entity<AnahCategory>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_NewTableAnahCategory");

			entity.ToTable("AnahCategory");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.CreatedBy).WithMany(p => p.AnahCategoryCreatedBies).HasForeignKey(d => d.CreatedById)
				.OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NewTableAnahCategory_User_CreatingUser");

			entity.HasOne(d => d.LastUpdateBy).WithMany(p => p.AnahCategoryLastUpdateBies)
				.HasForeignKey(d => d.LastUpdateById).OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_NewTableAnahCategory_User_UpdatingUser");
		});

		modelBuilder.Entity<AnahCategorySuplementaryOccupantIncome>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_NewTableAnahCategorySuplementaryOccupantIncome");

			entity.ToTable("AnahCategorySuplementaryOccupantIncome");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<AverageAccompanyingDuration>(entity =>
		{
			entity.HasNoKey().ToView("AverageAccompanyingDuration");
		});

		modelBuilder.Entity<Department>(entity =>
		{
			entity.ToTable("Department");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.Number).HasMaxLength(50);
		});

		modelBuilder.Entity<DocumentGenerationLog>(entity =>
		{
			entity.ToTable("DocumentGenerationLog");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<Email>(entity =>
		{
			entity.ToTable("Email");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<FundingMode>(entity =>
		{
			entity.ToTable("FundingMode");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Label).HasMaxLength(50);

			entity.HasOne(d => d.PreFinancingPlanNavigation).WithMany(p => p.FundingModes)
				.HasForeignKey(d => d.PreFinancingPlan).HasConstraintName("FK_FundingMode_PreFinancementPlan");
		});

		modelBuilder.Entity<Household>(entity =>
		{
			entity.ToTable("Household");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.AnahCategory).HasMaxLength(50);

			entity.HasOne(d => d.MainOccupantNavigation).WithMany(p => p.Households).HasForeignKey(d => d.MainOccupant)
				.HasConstraintName("FK_Household_MainOccupant");
		});

		modelBuilder.Entity<HouseholdDifficultiesLabel>(entity =>
		{
			entity.ToTable("HouseholdDifficultiesLabel");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Labels).HasMaxLength(50);
		});

		modelBuilder.Entity<HouseholdDifficulty>(entity =>
		{
			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.DifficultyNavigation).WithMany(p => p.HouseholdDifficulties)
				.HasForeignKey(d => d.Difficulty)
				.HasConstraintName("FK_HouseholdDifficulties_HouseholdDifficultiesLabel");

			entity.HasOne(d => d.HouseholdNavigation).WithMany(p => p.HouseholdDifficulties)
				.HasForeignKey(d => d.Household).HasConstraintName("FK_HouseholdDifficulties_Household");
		});

		modelBuilder.Entity<HouseholdExpense>(entity =>
		{
			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.HouseholdNavigation).WithMany(p => p.HouseholdExpenses).HasForeignKey(d => d.Household)
				.HasConstraintName("FK_HouseholdExpenses_Household");
		});

		modelBuilder.Entity<HouseholdHeatingEnergy>(entity =>
		{
			entity.HasKey(e => new { e.Household, e.HouseholdHeatingEnergyLabel }).HasName("PK_HouseholdHeatingEnergy");

			entity.HasOne(d => d.HouseholdNavigation).WithMany(p => p.HouseholdHeatingEnergies)
				.HasForeignKey(d => d.Household).HasConstraintName("FK_HouseholdHeatingEnergy_Household");

			entity.HasOne(d => d.HouseholdHeatingEnergyNavigation).WithMany(p => p.HouseholdHeatingEnergies)
				.HasForeignKey(d => d.HouseholdHeatingEnergyLabel)
				.HasConstraintName("FK_HouseholdHeatingEnergy_HouseholdHeatingEnergyLabel");
		});

		modelBuilder.Entity<HouseholdHeatingEnergyLabel>(entity =>
		{
			entity.ToTable("HouseholdHeatingEnergyLabel");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<HouseholdResource>(entity =>
		{
			entity.HasKey(e => new { e.Household, e.HouseholdResources }).HasName("PK_HouseholdRessources");

			entity.HasOne(d => d.HouseholdNavigation).WithMany(p => p.HouseholdResources)
				.HasForeignKey(d => d.Household).HasConstraintName("FK_HouseholdRessources_Household");

			entity.HasOne(d => d.HouseholdResourcesNavigation).WithMany(p => p.HouseholdResources)
				.HasForeignKey(d => d.HouseholdResources)
				.HasConstraintName("FK_HouseholdRessources_HouseholdResourcesLabel");
		});

		modelBuilder.Entity<HouseholdResourcesLabel>(entity =>
		{
			entity.ToTable("HouseholdResourcesLabel");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<Housing>(entity =>
		{
			entity.ToTable("Housing");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.CadastralReference).HasMaxLength(50);
			entity.Property(e => e.IsInAbfarea).HasColumnName("IsInABFArea");

			entity.HasOne(d => d.HousingAddressNavigation).WithMany(p => p.Housings)
				.HasForeignKey(d => d.HousingAddress).HasConstraintName("FK_Housing_Address");

			entity.HasOne(d => d.HousingAfterWorkStateNavigation).WithMany(p => p.Housings)
				.HasForeignKey(d => d.HousingAfterWorkState).HasConstraintName("FK_Housing_HousingAfterWorkState");

			entity.HasOne(d => d.HousingInitialStateNavigation).WithMany(p => p.Housings)
				.HasForeignKey(d => d.HousingInitialState).HasConstraintName("FK_Housing_HousingInitialState");
		});

		modelBuilder.Entity<HousingAfterWorkState>(entity =>
		{
			entity.ToTable("HousingAfterWorkState");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.EstimatedAnnualGesemissionsAfterWork)
				.HasColumnName("EstimatedAnnualGESEmissionsAfterWork");
			entity.Property(e => e.EstimatedDpeafterWork).HasColumnName("EstimatedDPEAfterWork");
			entity.Property(e => e.EstimatedDpeclassJump).HasColumnName("EstimatedDPEClassJump");
			entity.Property(e => e.EstimatedGesafterWork).HasColumnName("EstimatedGESAfterWork");
		});

		modelBuilder.Entity<HousingInitialState>(entity =>
		{
			entity.ToTable("HousingInitialState");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.AnnualGesemission).HasColumnName("AnnualGESEmission");
			entity.Property(e => e.Dpe).HasColumnName("DPE");
			entity.Property(e => e.Ges).HasColumnName("GES");
			entity.Property(e => e.HeatingEnergy).HasMaxLength(50);
		});

		modelBuilder.Entity<ImportError>(entity =>
		{
			entity.ToTable("ImportError");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.LineNumber).HasColumnName("LineNumber");
			entity.Property(e => e.ErrorMessage).HasColumnName("ErrorMessage");

			entity.HasOne(d => d.ImportRunNavigation).WithMany(p => p.ImportErrors)
				.HasForeignKey(d => d.ImportRunId).HasConstraintName("FK_ImportError_ImportRun_ImportRunId");
		});

		modelBuilder.Entity<ImportRun>(entity =>
		{
			entity.ToTable("ImportRun");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.StartedAt).HasColumnName("StartedAt");
			entity.Property(e => e.EndedAt).HasColumnName("EndedAt");
			entity.Property(e => e.ImportStatus).HasColumnName("ImportStatus");
			entity.Property(e => e.Operator).HasColumnName("Operator");
		});

		modelBuilder.Entity<InsuranceType>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_InsurranceType");

			entity.ToTable("InsuranceType");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Label).HasMaxLength(100);
		});

		modelBuilder.Entity<Invoice>(entity =>
		{
			entity.ToTable("Invoice");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.AccompanyingFile).WithMany(p => p.Invoices).HasForeignKey(d => d.AccompanyingFileId)
				.OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Invoice_AccompanyingFile");
		});

		modelBuilder.Entity<Impersonate>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_Impersonate");

			entity.ToTable("Impersonate");
			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<MainOccupant>(entity =>
		{
			entity.ToTable("MainOccupant");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Trigram).HasMaxLength(50);
		});

		modelBuilder.Entity<NationalStructure>(entity =>
		{
			entity.ToTable("NationalStructure");

			entity.HasKey(entity => entity.Id).HasName("PK_NationalStructure");
		});

		modelBuilder.Entity<PreFinancingPlan>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_PreFinancementPlan");

			entity.ToTable("PreFinancingPlan");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.RemainingAmountFinancingSource).HasMaxLength(50);
			entity.Property(e => e.SolicitedBankLoanType).HasMaxLength(50);
		});

		modelBuilder.Entity<PreWorkPlan>(entity =>
		{
			entity.ToTable("PreWorkPlan");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean)
				.HasColumnName("DoHouseholdCanMobilizeSocialCircleOnConstructionSite : boolean ");
			entity.Property(e => e.HasInterestInPossibleAraprocess).HasColumnName("HasInterestInPossibleARAProcess");
			entity.Property(e => e.HouseholdAvailabilitiyToOrganizeArasite)
				.HasColumnName("HouseholdAvailabilitiyToOrganizeARASite");
			entity.Property(e => e.IsAraopeningStatementSent).HasColumnName("IsARAOpeningStatementSent");
			entity.Property(e => e.IsHouseholdReadyToStartAraprocess)
				.HasColumnName("IsHouseholdReadyToStartARAProcess");
		});

		modelBuilder.Entity<PreWorkPlanInsuranceType>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_PreWorkPlanInssuranceType");

			entity.ToTable("PreWorkPlanInsuranceType");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.InsuranceTypeNavigation).WithMany(p => p.PreWorkPlanInsuranceTypes)
				.HasForeignKey(d => d.InsuranceType).HasConstraintName("FK_PreWorkPlanInssuranceType_InssuranceType");

			entity.HasOne(d => d.PreWorkPlanNavigation).WithMany(p => p.PreWorkPlanInsuranceTypes)
				.HasForeignKey(d => d.PreWorkPlan).HasConstraintName("FK_PreWorkPlanInssuranceType_PreWorkPlan");
		});

		modelBuilder.Entity<PreWorkPlanProjectType>(entity =>
		{
			entity.ToTable("PreWorkPlanProjectType");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.PreWorkPlanNavigation).WithMany(p => p.PreWorkPlanProjectTypes)
				.HasForeignKey(d => d.PreWorkPlan).HasConstraintName("FK_PreWorkPlanProjectType_PreWorkPlan");

			entity.HasOne(d => d.ProjectTypeNavigation).WithMany(p => p.PreWorkPlanProjectTypes)
				.HasForeignKey(d => d.ProjectType).HasConstraintName("FK_PreWorkPlanProjectType_ProjectType");
		});

		modelBuilder.Entity<ProjectType>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_ProjectTime");

			entity.ToTable("ProjectType");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Label).HasMaxLength(50);
		});

		modelBuilder.Entity<ReportingStructure>(entity =>
		{
			entity.ToTable("ReportingStructure");

			entity.HasOne(rs => rs.NationalStructureNavigation).WithMany(ns => ns.ReportingStructures)
				.HasForeignKey(rs => rs.NationalStructureId).OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_ReportingStructure_NationalStructure");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Name).HasMaxLength(50).IsUnicode(false);
		});

		modelBuilder.Entity<Role>(entity =>
		{
			entity.ToTable("Role");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.LongName).HasMaxLength(50);
			entity.Property(e => e.Name).HasMaxLength(50);
		});

		modelBuilder.Entity<SecondaryOccupant>(entity =>
		{
			entity.ToTable("SecondaryOccupant");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.Trigram).HasMaxLength(50);

			entity.HasOne(d => d.HouseholdNavigation).WithMany(p => p.SecondaryOccupants)
				.HasForeignKey(d => d.Household).OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_SecondaryOccupant_Household");
		});

		modelBuilder.Entity<SupportTeam>(entity =>
		{
			entity.ToTable("SupportTeam");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.DiffuseCoordinatorNavigation).WithMany(p => p.SupportTeamDiffuseCoordinatorNavigations)
				.HasForeignKey(d => d.DiffuseCoordinator);

			entity.HasOne(d => d.SecondSolidarBuilderNavigation)
				.WithMany(p => p.SupportTeamSecondSolidarBuilderNavigations).HasForeignKey(d => d.SecondSolidarBuilder)
				.HasConstraintName("FK_SupportTeam_User");

			entity.HasOne(d => d.SolidarBuilderNavigation).WithMany(p => p.SupportTeamSolidarBuilderNavigations)
				.HasForeignKey(d => d.SolidarBuilder).OnDelete(DeleteBehavior.ClientSetNull);

			entity.HasOne(d => d.TargetCoordinatorNavigation).WithMany(p => p.SupportTeamTargetCoordinatorNavigations)
				.HasForeignKey(d => d.TargetCoordinator).HasConstraintName("FK_SupportTeam_TargetCoordinator");

			entity.HasOne(d => d.TerritorialBuilderNavigation).WithMany(p => p.SupportTeamTerritorialBuilderNavigations)
				.HasForeignKey(d => d.TerritorialBuilder).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.SecondTerritorialBuilderNavigation)
                .WithMany(p => p.SupportTeamSecondTerritorialBuilderNavigations).HasForeignKey(d => d.SecondTerritorialBuilder);

            entity.HasOne(d => d.ThirdSolidarBuilderNavigation)
				.WithMany(p => p.SupportTeamThirdSolidarBuilderNavigations).HasForeignKey(d => d.ThirdSolidarBuilder);
		});

		modelBuilder.Entity<Territory>(entity =>
		{
			entity.ToTable("Territory");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<UnregisteredUser>(entity =>
		{
			entity.ToTable("UnregisteredUser");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.PhoneNumber).HasMaxLength(50);
			entity.Property(e => e.SiretNumber).HasMaxLength(14).IsUnicode(false);

			entity.HasOne(d => d.AskedRoleNavigation).WithMany(p => p.UnregisteredUsers).HasForeignKey(d => d.AskedRole)
				.HasConstraintName("FK_UnregisteredUser_Role");

			entity.HasOne(d => d.ReportingStructureNavigation).WithMany(p => p.UnregisteredUsers)
				.HasForeignKey(d => d.ReportingStructureId).HasConstraintName("FK_UnregisteredUser_ReportingStructure");

			entity.HasOne(u => u.Territory).WithMany(t => t.UnregisteredUsers).HasForeignKey(u => u.TerritoryId).HasConstraintName("FK_UnregisteredUser_TerritoryId");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.ToTable("User");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.PhoneNumber).HasMaxLength(50);
			entity.Property(e => e.SiretNumber).HasMaxLength(14).IsUnicode(false);

			entity.HasOne(u => u.Territory).WithMany(t => t.Users).HasForeignKey(u => u.TerritoryId).HasConstraintName("FK_User_Territory");

			entity.HasOne(d => d.ReportingStructureNavigation).WithMany(p => p.Users)
				.HasForeignKey(d => d.ReportingStructureId).OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK_User_ReportingStructure");

			entity.HasOne(d => d.Role).WithMany(p => p.Users).HasForeignKey(d => d.RoleId)
				.HasConstraintName("FK_User_Role");
		});

		modelBuilder.Entity<WorkMonitoring>(entity =>
		{
			entity.ToTable("WorkMonitoring");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
			entity.Property(e => e.HasEffectiveComplianceWithWorkRecommendations)
				.HasColumnName("HasEffectiveComplianceWithWorkRecommendations");
		});

		modelBuilder.Entity<WorkPackage>(entity =>
		{
			entity.ToTable("WorkPackage");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);

			entity.HasOne(d => d.PreWorkPlanNavigation).WithMany(p => p.WorkPackages).HasForeignKey(d => d.PreWorkPlan)
                .OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_WorkPackage_PreWorkPlan");

			entity.HasOne(d => d.CopropertyWorkFinanceNavigation).WithMany(p => p.WorkPackages).HasForeignKey(d => d.CopropertyWorkFinance)
                .OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_WorkPackage_CopropertyWorkFinance");
		});

		modelBuilder.Entity<WorkPackageWorkTypeCost>(entity =>
		{
			entity.HasKey(e => new { e.WorkPackage, e.WorkType });

			entity.ToTable("WorkPackageWorkTypeCost");

			entity.HasOne(d => d.WorkPackageNavigation).WithMany(p => p.WorkPackageWorkTypeCosts)
				.HasForeignKey(d => d.WorkPackage).HasConstraintName("FK_WorkPackageWorkTypeCost_WorkPackage");

			entity.HasOne(d => d.WorkTypeNavigation).WithMany(p => p.WorkPackageWorkTypeCosts)
				.HasForeignKey(d => d.WorkType).HasConstraintName("FK_WorkPackageWorkTypeCost_WorkTypesLabel");
		});

		modelBuilder.Entity<WorkTypesLabel>(entity =>
		{
			entity.ToTable("WorkTypesLabel");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<CguVersion>(entity =>
		{
			entity.ToTable("CguVersion");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<SiteSupervision>(entity =>
		{
			entity.ToTable("SiteSupervision");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<WorkParticipant>(entity =>
		{
			entity.ToTable("WorkParticipant");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<SiteSupervisionDifficultyLabel>(entity =>
		{
			entity.ToTable("SiteSupervisionDifficultyLabel");

			entity.Property(e => e.Id).HasDefaultValueSql(NewId);
		});

		modelBuilder.Entity<WorkParticipantDifficulty>(entity =>
		{
			entity.ToTable("WorkParticipantDifficulty");

			entity.HasKey(e => new { e.WorkParticipantId, e.DifficultyId });
		});

		modelBuilder.Entity<WorkTypeProjectType>(entity =>
		{
			entity.ToTable("WorkTypeProjectType");

			entity.HasKey(e => new { e.WorkTypeId, e.ProjectTypeId });

			entity.HasOne(x => x.WorkType).WithMany(w => w.WorkTypeProjectTypes)
			  .HasForeignKey(x => x.WorkTypeId).HasConstraintName("FK_WorkTypeProjectType_WorkType");

			entity.HasOne(x => x.ProjectType).WithMany(p => p.WorkTypeProjects)
				  .HasForeignKey(x => x.ProjectTypeId).HasConstraintName("WorkTypeProjectType_ProjectType");
		});
	}
}