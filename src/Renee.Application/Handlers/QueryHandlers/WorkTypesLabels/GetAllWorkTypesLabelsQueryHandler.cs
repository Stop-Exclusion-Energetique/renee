using MediatR;
using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Application.Interfaces;
using Renee.Application.Queries.WorkTypesLabels;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.WorkTypesLabels;

public class GetAllWorkTypesLabelsQueryHandler(
	IWorkTypesLabelsRepository workTypesLabelsRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllWorkTypesLabelsQuery, ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>>
{
	private readonly Dictionary<string, int> _customeOrder = new()
	{
		{ "Aménagement intérieur", 1 },
		{ "Assainissement", 2 },
		{ "Réseau eaux pluviales", 3 },
		{ "Réseau eaux usées", 4 },
		{ "Assistance à maîtrise d'ouvrage (dont audit énergétique)", 5 },
		{ "Assurances", 6 },
		{ "Diagnostics préalables (amiante, plomb, mérule, structure, etc.)", 7 },
		{ "Isolation combles perdus", 8 },
		{ "Isolation murs périphériques", 9 },
		{ "Isolation toits terrasses", 10 },
		{ "Isolation rampant", 11 },
		{ "Isolation planchers bas", 12 },
		{ "Maîtrise d'œuvre", 13 },
		{ "Menuiseries : fenêtres", 14 },
		{ "Menuiseries : portes", 15 },
		{ "Menuiseries : fenêtres de toit (avec protection solaire)", 16 },
		{
			"Travaux conservatoires et de mise en sécurité préalable du logement (structure, installations, systèmes, etc.)",
			17
		},
		{ "Plomberie sanitaire", 18 },
		{ "Adaptation du logement et maintien à domicile", 19 },
		{ "Prise en charge plomb et amiante", 20 },
		{ "Production d'eau chaude", 21 },
		{ "Chauffage / rafraîchissement", 22 },
		{ "Protections solaires", 23 },
		{ "Réfection de la toiture", 24 },
		{ "Réfection de la charpente", 25 },
		{ "Réfection complète de l'installation électrique", 26 },
		{ "Mise aux normes du tableau électrique", 27 },
		{ "Ventilation (VMC, brasseurs d'air)", 28 },
		{ "Autre", 29 }
	};

	public async Task<ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>> Handle(
		GetAllWorkTypesLabelsQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var unsortedList = (await workTypesLabelsRepository.GetAllAsync())
				.Select(wt => new WorkTypesLabelsDto(wt.Id, wt.Label)).ToList();
			return ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>.Success(unsortedList.OrderBy(wt => _customeOrder[wt.Label]).ToList());
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}