using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace Renee.Infrastructure.Providers;

public sealed class GraphApiClientService(IConfiguration configuration)
{
	public GraphServiceClient GetClient()
	{
		var authProvider = new ClientSecretCredential(
			configuration.GetValue<string>("AzureADB2C:TenantId"),
			configuration.GetValue<string>("AzureADB2C:ClientId"),
			configuration.GetValue<string>("AzureADB2C:ClientSecret"));
		return new GraphServiceClient(authProvider);
	}
}