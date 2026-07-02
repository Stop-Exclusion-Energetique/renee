using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Occupant;

public class SearchAddressQueryHandler(
	IAddressRepository adressRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<SearchAddressQuery, ReneeOperationResult<List<AddressDto>>>
{
	public async Task<ReneeOperationResult<List<AddressDto>>> Handle(SearchAddressQuery request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.Search.Trim().Length < 3) return ReneeOperationResult<List<AddressDto>>.Success([]);
			var response = await adressRepository.SearchAddressAsync(request.Search);
			var result = response?.Select(
				addressEntity => new AddressDto
				{
					Label = addressEntity.Label,
					PostalCode = addressEntity.PostalCode,
					City = addressEntity.City,
					Department = addressEntity.Department,
					Region = addressEntity.Region,
					Street = addressEntity.Street,
					HouseNumber = addressEntity.HouseNumber
				}).ToList() ?? [];
			return ReneeOperationResult<List<AddressDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<List<AddressDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}