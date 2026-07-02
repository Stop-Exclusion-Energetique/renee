using Azure;
using Azure.AI.OpenAI;
using Blazored.Modal;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.AI;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Radzen;
using Renee.Application;
using Renee.Domain;
using Renee.Infrastructure;
using Renee.Infrastructure.Providers;
using Renee.UI;
using Renee.UI.Components;
using Renee.UI.Components.CGUHandling;
using Renee.UI.Components.Layout.StageLayout;
using Renee.UI.Components.Layout.SynthesisLayoutManager;
using Renee.UI.Components.MiddleWare;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using Constants = Renee.Domain.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddEnvironmentVariables().AddUserSecrets(Assembly.GetExecutingAssembly(), true);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddMicrosoftIdentityConsentHandler();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddScoped<JwtGeneratorService>();
builder.Services.AddScoped<IStageNavigationStateService, StageNavigationStateService>();
// This is where you wire up to events to detect when a user Log in

var initialScopes = builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(options =>
{
	builder.Configuration.Bind("AzureADB2C", options);
	options.Events = new OpenIdConnectEvents
	{
		OnRedirectToIdentityProvider = async _ => { await Task.Yield(); },
		OnAuthenticationFailed = async _ => { await Task.Yield(); },
		OnSignedOutCallbackRedirect = async ctx =>
		{
			ctx.HttpContext.Response.Redirect(ctx.Options.SignedOutRedirectUri);
			ctx.HandleResponse();
			await Task.Yield();
		},
		OnTicketReceived = async ctx => await Task.Yield()
	};
}).EnableTokenAcquisitionToCallDownstreamApi(initialScopes).AddInMemoryTokenCaches();

builder.Services.AddAuthorization();
builder.Services.AddAuthorizationCore(options =>
{
	options.AddPolicy("CsvFileAccessPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireAssertion(context =>
			context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin") || 
			(context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "ES") && 
				context.User.HasClaim(c => c.Type == ClaimTypes.GroupSid && c.Value == "SOLIHA GRAND PARIS")
			)
		);
	});

	options.AddPolicy("AnahGrantCheckAccessPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim(ClaimTypes.Role, Constants.SolidarBuilderRole);
		policy.RequireClaim(CustomClaimTypes.ShouldCheckAnahFiles, "true");
	});
});

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<NavigationHistoryManager>();
builder.Services.AddScoped<SynthesysLayoutStateManager>();
builder.Services.AddScoped<CguInitializer>();
builder.Services.AddScoped<AccompanyingFileListFilterDataPersistance>();
builder.Services.AddScoped<UnsavedChangesGuard>();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazorContextMenu();

builder.Services.AddChatClient(services =>
    new AzureOpenAIClient(
        new Uri(builder.Configuration["AzureFoundryResources:ClientUri"]!),
        new AzureKeyCredential(builder.Configuration["AzureFoundryResources:AzureKeyCredential"]!))
    .GetChatClient(builder.Configuration["AzureFoundryResources:DeploymentName"]!)
    .AsIChatClient())
    .UseFunctionInvocation();

builder.Services.AddScoped<AuthenticationStateProvider, ImpersonationAuthenticationStateProvider>();
builder.Services.WithDatabaseInfrastructure(builder.Configuration);
builder.Services.WithApplication();
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

var app = builder.Build();

var cultureInfo = new CultureInfo("fr-FR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var supportedCultures = new[] { cultureInfo };

app.UseRewriter(
	new RewriteOptions().Add(context =>
	{
		if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
			context.HttpContext.Response.Redirect("/");
	}));
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/ErrorPage", true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseRequestLocalization(
	new RequestLocalizationOptions
	{
		DefaultRequestCulture = new RequestCulture(cultureInfo),
		SupportedCultures = supportedCultures,
		SupportedUICultures = supportedCultures
	});

app.UseErrorHandling();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

await app.RunAsync();