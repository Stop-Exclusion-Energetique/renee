
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Renee.Application.Interfaces;
using Renee.Infrastructure.Data;
using Renee.Infrastructure.FileServices;
using Renee.McpServer.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["JwtGenerationKey:SecretKey"]
    ?? throw new InvalidOperationException("JwtGenerationKey:SecretKey configuration is required.");
var jwtIssuer = builder.Configuration["JwtGenerationKey:issuer"]
    ?? throw new InvalidOperationException("JwtGenerationKey:Issuer configuration is required.");
var jwtAudience = builder.Configuration["JwtGenerationKey:audience"]
    ?? throw new InvalidOperationException("JwtGenerationKey:Audience configuration is required.");
var allowedOrigins = builder.Configuration["Cors:AllowedOrigins"]
	?? throw new InvalidOperationException("Cors:AllowedOrigins configuration is required.");

var keyBytes = Convert.FromBase64String(jwtKey);
var signingKey = new SymmetricSecurityKey(keyBytes);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Configuration.AddJsonFile("appsettings.json", true, true);

var connectionsStrings = builder.Configuration.GetValue<string>("ConnectionStrings:ReneeDbContext");

builder.Services.AddDbContextFactory<ReneeDbContext>(options =>
	{
		options.UseSqlServer(
			connectionsStrings,
			sqlServerOptions => sqlServerOptions.CommandTimeout(60));
	});

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = true; // en prod

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtIssuer,

			ValidateAudience = true,
			ValidAudience = jwtAudience,

			ValidateIssuerSigningKey = true,
			IssuerSigningKey = signingKey,

			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromSeconds(30),
			NameClaimType = JwtRegisteredClaimNames.Sub,
			RoleClaimType = ClaimTypes.Role
		};

		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = ctx =>
			{
				// Inspecter Authorization Bearer
				return Task.CompletedTask;
			},
			OnAuthenticationFailed = ctx =>
			{
				var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JWT");
				logger.LogWarning("JWT auth failed: {ExceptionType}", ctx.Exception.GetType().Name);
				return Task.CompletedTask;
			}
		};
	});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();

builder.Services
	.AddMcpServer()
	.WithHttpTransport(o =>
	{
		o.Stateless = false;
	})
	.AddAuthorizationFilters()
	.WithToolsFromAssembly()
	.WithPromptsFromAssembly()
	.WithResourcesFromAssembly();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigins", policy =>
	{
		policy.WithOrigins(allowedOrigins)
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

var renoHelpToken = builder.Configuration["RenoHelp:BearerToken"]
    ?? throw new InvalidOperationException("RenoHelp:BearerToken configuration is required.");

builder.Services.AddHttpClient<RenoHelpClient>(client =>
{
	client.BaseAddress = new Uri("https://mesaides.france-renov.gouv.fr/api/v1/");
	client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
	client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", $"Bearer {renoHelpToken}");
});

builder.Services.AddHttpClient<InseeService>();

builder.Services.AddHttpClient<GeoApiClient>(client =>
{
    client.BaseAddress = new Uri("https://geo.api.gouv.fr/");
});

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapMcp();

await app.RunAsync();
