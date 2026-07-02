using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Renee.Application.Interfaces;
using Renee.Domain.Repositories;
using Renee.Infrastructure.AI;
using Renee.Infrastructure.Airtable;
using Renee.Infrastructure.Data;
using Renee.Infrastructure.EmailServices;
using Renee.Infrastructure.FileServices;
using Renee.Infrastructure.Providers;
using Renee.Infrastructure.Repositories;

namespace Renee.Infrastructure;

public static class DependencyInjection
{
	// ReSharper disable once UnusedParameter.Global
	public static void WithDatabaseInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionsStrings = configuration.GetValue<string>("ConnectionStrings:ReneeDbContext") ?? configuration.GetValue<string>("ReneeDbContext");
			
		services.AddDbContextFactory<ReneeDbContext>(options =>
		{
			options.UseSqlServer(
				connectionsStrings,
				sqlServerOptions => sqlServerOptions.CommandTimeout(60));
		});
		services.AddScoped<IUnregisteredUserRepository, UnregisteredUserRepository>();
		services.AddScoped<IDepartmentRepository, DepartmentRepository>();
		services.AddScoped<IAnahCategoryRepository, AnahCategoryRepository>();
		services.AddScoped<IEmailRepository, EmailRepository>();
		services.AddScoped<IRoleRepository, RoleRepository>();
		services.AddScoped<ICguVersionRepository, CguVersionRepository>();
		services.AddScoped<IEmailSenderService, EmailSenderService>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<GraphApiClientService>();
		services.AddScoped<AddressApiService>();
		services.AddScoped<IFileService, FileService>();
		services.AddScoped<IFileViewerService, FileViewerService>();
		services.AddScoped<IEncryptionService, EncryptionService>();
		services.AddScoped<IAirtableService, AirtableService>();
		services.AddScoped<IAIDossierSynthesisService, AIDossierSynthesisService>();
		services.AddScoped<JwtGeneratorService>();

		services.AddScoped<IAddressRepository, AddressRepository>();

		services.AddScoped<IDifficultyFacedByFamilyRepository, DifficultyFacedByFamilyRepository>();
		services.AddScoped<IHouseholdResourcesTypologyRepository, HouseholdResourcesTypologyRepository>();
		services.AddScoped<IAccompanyingFileRepository, AccompanyingFileRepository>();
		services.AddScoped<IMainOccupantRepository, MainOccupantRepository>();
		services.AddScoped<IWorkTypesLabelsRepository, WorkTypesLabelsRepository>();
		services.AddScoped<IInsuranceTypeRepository, InsuranceTypeRepository>();
		services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
		services.AddScoped<IReportingStructureRepository, ReportingStructureRepository>();
		services.AddScoped<ITaskRepository, TaskRepository>();
		services.AddScoped<ITerritoryRepository, TerritoryRepository>();
		services
			.AddScoped<IAnahCategorySuplementaryOccupantIncomeRepository,
				AnahCategorySuplementaryOccupantIncomeRepository>();
		services.AddScoped<IImpersonateRepository, ImpersonateRepository>();
		services.AddScoped<IHouseholdHeatingEnergyLabelRepository, HouseholdHeatingEnergyLabelRepository>();
		services.AddScoped<IImportRunRepository, ImportRunRepository>();
		services.AddScoped<IImportErrorRepository, ImportErrorRepository>();
		services.AddScoped<IAbortReasonLabelRepository, AbortReasonLabelRepository>();
		services.AddScoped<ICopropertyProfileRepository, CopropertyProfileRepository>();
		services.AddScoped<ISiteSupervisionDifficultyLabelRepository, SiteSupervisionDifficultyLabelRepository>();
		services.AddScoped<IWorkTypeProjectTypeRepository, WorkTypeProjectTypeRepository>();
		services.AddScoped<IDocumentGenerationLogRepository, DocumentGenerationLogRepository>();
		services.AddScoped<IAdminConstantRepository, AdminConstantRepository>();
	}
}