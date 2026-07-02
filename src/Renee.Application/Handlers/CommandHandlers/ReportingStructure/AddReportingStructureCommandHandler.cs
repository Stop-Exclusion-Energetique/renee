using MediatR;
using Renee.Application.Commands.ReportingStructure;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.ReportingStructure;

public class AddReportingStructureCommandHandler(
	IReportingStructureRepository reportingStructureRepository,
	ITelemetryService telemetryService) : IRequestHandler<AddReportingStructureCommand, ReneeOperationResult<Guid>>
{
	public async Task<ReneeOperationResult<Guid>> Handle(AddReportingStructureCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.ReportingStructureName is null)
				return ReneeOperationResult<Guid>.Failure(Labels.Errors.RequiredReportingStructureName);

			var reportingStructureId = await reportingStructureRepository.AddReportingStructureAsync(new Domain.Entity.ReportingStructure
			{
				Name = request.ReportingStructureName
			});

			return ReneeOperationResult<Guid>.Success(reportingStructureId, Labels.AddReportingStructureSuccess);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<Guid>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
