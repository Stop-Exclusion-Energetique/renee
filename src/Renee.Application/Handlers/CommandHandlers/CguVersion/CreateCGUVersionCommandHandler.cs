using MediatR;
using Renee.Application.Commands.CguVersion;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.CguVersion;

public class CreateCguVersionCommandHandler(
	ICguVersionRepository cguVersionRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateCguVersionCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(CreateCguVersionCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(request.CguVersionDto.Label) ||
				string.IsNullOrWhiteSpace(request.CguVersionDto.Version))
				return ReneeOperationResult<bool>.Failure(Labels.Errors.CguLabelAndVersionRequired);
			var existsVersion = await cguVersionRepository.VerifiyDuplicatedVersionAsync(request.CguVersionDto.Version);
			if (existsVersion != null && existsVersion == true)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.CguVersionAlreadyExists);
			var cgu = new Domain.Entity.CguVersion
			{
				Label = request.CguVersionDto.Label!,
				Version = request.CguVersionDto.Version!
			};
			var numberItemsChanged = await cguVersionRepository.AddCGUVersion(cgu);
			return numberItemsChanged > -1
				? ReneeOperationResult<bool>.Success(true, Labels.CreateCguVersionSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileCreatingCguVersion);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
