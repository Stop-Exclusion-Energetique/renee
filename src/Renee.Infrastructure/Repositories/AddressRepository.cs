using System.Text.Json;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Providers;
using Renee.Infrastructure.Providers.JsonModels;

namespace Renee.Infrastructure.Repositories;

public sealed class AddressRepository(AddressApiService addressApiService) : IAddressRepository
{
	public async Task<List<Address>?> SearchAddressAsync(string search)
	{
		var queryHouseNumberString = $"?q={search}&type={Constants.HouseNumberApiType}&autocomplete=1";
		var queryLocalityString = $"?q={search}&type={Constants.LocalityApiType}&autocomplete=1";
		var responseHouseNumber = await addressApiService.GetHttpClient().GetAsync(queryHouseNumberString);
		var responseLocality = await addressApiService.GetHttpClient().GetAsync(queryLocalityString);
		if (!responseHouseNumber.IsSuccessStatusCode || !responseLocality.IsSuccessStatusCode) return null;

		try
		{
			var responseHouseNumberContent = await responseHouseNumber.Content.ReadAsStringAsync();
			var responseLocalityContent = await responseLocality.Content.ReadAsStringAsync();

			var jsonDataHouseNumber = JsonSerializer.Deserialize<Root>(responseHouseNumberContent);
			var jsonDataLocality = JsonSerializer.Deserialize<Root>(responseLocalityContent);

			if (jsonDataHouseNumber == null || jsonDataLocality == null) return null;

			jsonDataHouseNumber.features?.AddRange(jsonDataLocality.features!);
			var addresses = jsonDataHouseNumber.features!.Select(feature => new Address
			{
				Label = feature.properties?.label ?? string.Empty,
				PostalCode = feature.properties?.postcode ?? string.Empty,
				City = feature.properties?.city ?? string.Empty,
				Department = feature.properties?.context?.Split(",")[1] ?? string.Empty,
				Region = feature.properties?.context?.Split(",").ElementAtOrDefault(2) ?? string.Empty,
				HouseNumber = feature.properties?.housenumber ?? string.Empty,
				Street = feature.properties?.street ?? feature.properties?.name ?? string.Empty
			}).ToList();

			return addresses;
		}
		catch (Exception) { return null; }
	}
}