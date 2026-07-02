using Microsoft.Extensions.Configuration;

namespace Renee.Infrastructure.Providers;

public sealed class AddressApiService(IConfiguration configuration)
{
	public HttpClient GetHttpClient()
	{
		var httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(
			configuration.GetValue<string>("AddressApiGouv:Uri") ??
			throw new InvalidDataException("AddressApiGouv__Uri is not set"));
		return httpClient;
	}
}