using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Handlers.CommandHandlers.Mail;
using Renee.Application.Handlers.CommandHandlers.Mail.MailStrategy;
using Renee.Application.Interfaces;
using Renee.Application.Interfaces.Administration;
using Renee.Application.Services;
using Renee.Application.Services.Administration;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Application.Services.Administration.ImportExcelData;
using Renee.Application.Services.TelemetryServices;

namespace Renee.Application;

public static class DependencyInjection
{
	public static void WithApplication(this IServiceCollection services)
	{
		services.AddScoped<IRoleService, RoleService>();
		services.AddScoped<ICguValidationService, CguValidationService>();
		services.AddScoped<IDepartmentService, DepartmentService>();
		services.AddScoped<IAnahCategoryService, AnahCategoryService>();
		services.AddScoped<IUserValidationService, UserValidationService>();
		services.AddScoped<IFinancialAidService, FinancialAidService>();
		services.AddScoped<IUserCredentialService, UserCredentialService>();
		services.AddScoped<IMainOccupantService, MainOccupantService>();
		services.AddScoped<IAddressService, AddressService>();
		services.AddScoped<IDifficultyFacedByFamilyService, DifficultyFacedByFamilyService>();
		services.AddScoped<IHouseholdResourcesTypologyService, HouseholdResourcesTypologyService>();
		services.AddScoped<IAccompanyingFileService, AccompanyingFileService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IWorkTypesLabelsService, WorkTypesLabelsService>();
		services.AddScoped<IInsuranceTypeService, InsuranceTypeService>();
		services.AddScoped<IProjectTypeService, ProjectTypeService>();
		services.AddScoped<IImportExcelDataService, ImportExcelDataService>();
        services.AddScoped<IImportCsvDataService, ImportCsvDataService>();
        services.AddScoped<ISendEventQuery, SendEventQuery>();
		services.AddScoped<IReportingStructureService, ReportingStructureService>();
		services.AddScoped<ITaskService, TaskService>();
		services.AddScoped<ICguVersionService, CguVersionService>();
		services.AddScoped<IGenerateFilesService, GenerateFilesService>();
		services.AddSingleton<ITelemetryService, TelemetryService>();
		services.AddScoped<ISupportTeamService, SupportTeamService>();
		services.AddScoped<IImpersonateService, ImpersonateService>();
		services.AddScoped<ICopropertyProfileService, CopropertyProfileService>();

		services.AddScoped<ISendMailStrategy, AbortAccompanyingFileRequestMailStrategy>();
		services.AddScoped<ISendMailStrategy, AbortCancellationMailStrategy>();
		services.AddScoped<ISendMailStrategy, AbortMailStrategy>();
		services.AddScoped<ISendMailStrategy, CreationCodeMailStrategy>();
		services.AddScoped<ISendMailStrategy, InscriptionAdminWaitingMailStrategy>();
		services.AddScoped<ISendMailStrategy, InscriptionConfirmationMailStrategy>();
		services.AddScoped<ISendMailStrategy, InscriptionRefusedMailStrategy>();
		services.AddScoped<ISendMailStrategy, InscriptionUserWaitingMailStrategy>();
		services.AddScoped<ISendMailStrategy, StageReadyMailStrategy>();
		services.AddScoped<ISendMailStrategy, StageRefusedMailStrategy>();
		services.AddScoped<ISendMailStrategy, StageValidatedMailStrategy>();
		services.AddScoped<ISendMailStrategy, TaskAssignedMailStrategy>();
		services.AddScoped<ISendMailStrategy, TaskCompletedMailStrategy>();
		services.AddScoped<ISendMailStrategy, TaskDeletedMailStrategy>();
		services.AddScoped<IWorkTypeProjectTypeService, WorkTypeProjectTypeService>();
		services.AddScoped<IAdministrationConstantsManagementService, AdministrationConstantsManagementService>();

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
	}
}