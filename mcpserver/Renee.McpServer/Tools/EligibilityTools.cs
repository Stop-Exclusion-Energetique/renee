using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Renee.Domain.Enums;
using Renee.Infrastructure.Data;
using Renee.McpServer.Models;
using Renee.McpServer.Services;
using Renee.McpServer.Extensions;

namespace Renee.McpServer.Tools;

[McpServerToolType]
public class EligibilityTools(
	ReneeDbContext context,
	RenoHelpClient renoHelpClient,
	InseeService inseeService,
	IHttpContextAccessor httpContextAccessor
)
{
	private static readonly Dictionary<string, string> PublicodesVariableLabels = new(StringComparer.OrdinalIgnoreCase)
	{
		["ménage . personnes"] = "Nombre de personnes dans le ménage",
		["ménage . revenu"] = "Revenu annuel du ménage (€)",
		["ménage . revenu . classe"] = "Classe de revenu du ménage",
		["ménage . commune"] = "Commune du ménage",
		["logement . type"] = "Type de logement (maison ou appartement)",
		["logement . surface"] = "Surface habitable du logement (m²)",
		["logement . période de construction"] = "Période de construction du logement",
		["logement . propriétaire occupant"] = "Le ménage est-il propriétaire occupant ?",
		["logement . résidence principale locataire"] = "Le logement est-il la résidence principale du locataire ?",
		["logement . résidence principale propriétaire"] = "Le logement est-il la résidence principale du propriétaire ?",
		["logement . prix d'achat"] = "Prix d'achat du logement (€)",
		["logement . taxe foncière"] = "Montant de la taxe foncière (€)",
		["logement . commune"] = "Commune du logement",
		["DPE . actuel"] = "Étiquette DPE actuelle du logement (A à G)",
		["projet . DPE visé"] = "Étiquette DPE visée après travaux (A à G)",
		["projet . travaux"] = "Montant total des travaux (€)",
		["projet . gain énergétique"] = "Gain énergétique visé",
		["vous . propriétaire . statut"] = "Statut de propriété (propriétaire, locataire, occupant à titre gratuit…)",
		["parcours d'aide"] = "Type de parcours d'aide",
		["denormandie . années de location"] = "Nombre d'années de location prévues (dispositif Denormandie)",
		["denormandie . gestes minimum"] = "Gestes minimum réalisés (dispositif Denormandie)",
		["taxe foncière . condition de dépenses"] = "Condition de dépenses liée à la taxe foncière",
	};

	private static string GetFriendlyVariableLabel(string variable) =>
		PublicodesVariableLabels.TryGetValue(variable, out var label) ? label : variable;

	private static string FormatEligibilityAsMarkdown(EligibilityReport report)
	{
		var sb = new StringBuilder();

		sb.AppendLine("## Aides éligibles");
		sb.AppendLine();
		if (report.AidesEligibles.Count == 0)
		{
			sb.AppendLine("Aucune aide éligible.");
		}
		else
		{
			foreach (var aide in report.AidesEligibles)
			{
				sb.Append($"- **{aide.Nom}** ({aide.Type}) — {aide.Montant}");
				if (aide.Taux != null) sb.Append($" | Taux : {aide.Taux}");
				if (aide.Duree != null) sb.Append($" | Durée : {aide.Duree}");
				sb.AppendLine();
			}
		}

		sb.AppendLine();
		sb.AppendLine("## Aides non éligibles");
		sb.AppendLine();
		if (report.AidesNonEligibles.Count == 0)
		{
			sb.AppendLine("Aucune.");
		}
		else
		{
			foreach (var aide in report.AidesNonEligibles)
				sb.AppendLine($"- {aide.Nom} ({aide.Type})");
		}

		if (report.AidesIncompletes.Count > 0)
		{
			sb.AppendLine();
			sb.AppendLine("## Aides incomplètes (informations manquantes dans le dossier)");
			sb.AppendLine();
			foreach (var aide in report.AidesIncompletes)
			{
				sb.AppendLine($"- **{aide.Nom}** ({aide.Type})");
				sb.AppendLine($"  - Variables manquantes : {string.Join(", ", aide.InformationsManquantes)}");
			}

			var missingVars = report.AidesIncompletes
				.SelectMany(a => a.InformationsManquantes)
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.OrderBy(v => v)
				.ToList();

			sb.AppendLine();
			sb.AppendLine("## Informations à collecter auprès de l'utilisateur");
			sb.AppendLine();
			sb.AppendLine("Les données suivantes sont absentes du dossier. Demande-les à l'utilisateur de manière conversationnelle pour compléter l'analyse d'éligibilité :");
			sb.AppendLine();
			foreach (var v in missingVars)
				sb.AppendLine($"- {GetFriendlyVariableLabel(v)}");
		}

		return sb.ToString().TrimEnd();
	}

	[McpServerTool]
	[Description("Recupere les informations d'eligibilite aux aides de renovation energetique a partir des donnees en base via la reference du dossier d'accompagnement. Cet outil interagit avec une API externe pour determiner les types d'aides disponibles pour un dossier donne.")]
	[Authorize]
	public async Task<CallToolResult> GetRenovationAidEligibility(
		[Description("Reference du dossier d'accompagnement pour recuperer les informations d'eligibilite.")] string accompanyingFileReference
	)
	{
		var fileExists = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.AnyAsync(af => af.AccompanyingFileReference == accompanyingFileReference);

		if (!fileExists)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Aucun dossier trouvé avec la référence {accompanyingFileReference}." }]
			};
		}

		var accompanyingFile = await context.AccompanyingFiles
			.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
			.Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
			.Include(af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation)
			.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
			.Include(af => af.AccompanyingFileHouseholdNavigation.SecondaryOccupants)
			.Include(af => af.AccompanyingFileHouseholdNavigation.HouseholdResources)
			.Include(af => af.AccompanyingFilePreWorkPlanNavigation.WorkPackages)
				.ThenInclude(wp => wp.WorkPackageWorkTypeCosts)
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.Where(af => af.AccompanyingFileReference == accompanyingFileReference)
			.FirstOrDefaultAsync();

		if (accompanyingFile == null)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Accès refusé : vous n'avez pas les droits nécessaires pour consulter le dossier {accompanyingFileReference}." }]
			};
		}

		string? periodeConstruction = null;
		if (accompanyingFile.AccompanyingFileHousingNavigation.ConstructionYear.HasValue)
		{
			var yearsSinceConstruction = DateTime.UtcNow.Year - accompanyingFile.AccompanyingFileHousingNavigation.ConstructionYear.Value;
			periodeConstruction = yearsSinceConstruction switch
			{
				>= 15 => "au moins 15 ans",
				>= 10 => "de 10 à 15 ans",
				>= 2 => "de 2 à 10 ans",
				_ => "moins de 2 ans"
			};
		}

		var ownershipStatus = accompanyingFile.AccompanyingFileHousingNavigation.OwnershipStatus;
		var isOwner = ownershipStatus.HasValue && ownershipStatus.Value is
			(int)OwnershipStatus.FullOwnership or
			(int)OwnershipStatus.CoOwner or
			(int)OwnershipStatus.DismemberedBarePropertyOnly or
			(int)OwnershipStatus.DismemberedUsurfructOnly or
			(int)OwnershipStatus.JointOwnership or
			(int)OwnershipStatus.RealEstateCompany;
		var isTenant = ownershipStatus.HasValue && ownershipStatus.Value is
			(int)OwnershipStatus.PrivateParkTenant or
			(int)OwnershipStatus.PublicParkTenant;

		var totalWorkCost = (int)accompanyingFile.AccompanyingFilePreWorkPlanNavigation.WorkPackages
			.SelectMany(wp => wp.WorkPackageWorkTypeCosts)
			.Sum(c => c.Cost);

		var inseeCode = await inseeService.GetInseeCodeAsync(
			accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode,
			accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation.City)
			?? accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode;

		// TODO: Still null (no DB source): LogementPrixAchat, LogementTaxeFonciere, ParcoursAide, TaxeFonciereConditionDepenses, ProjetGainEnergetique, MenageRevenueClasse

		var payload = new PublicodeRequest
		{
			DpeActuel = accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe.HasValue
				? accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe.Value + 1
				: null,
			MenageCommune = inseeCode,
			MenagePersonnes = accompanyingFile.AccompanyingFileHouseholdNavigation.SecondaryOccupants.Count + 1,
			MenageRevenu = (int)accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdResources.Sum(hr => hr.Value),
			LogementCommune = inseeCode,
			LogementPeriodeConstruction = periodeConstruction,
			LogementProprietaireOccupant = isOwner ? "oui" : "non",
			LogementResidencePrincipalePropriétaire = isOwner ? "oui" : "non",
			LogementResidencePrincipaleLocataire = isTenant ? "oui" : "non",
			LogementSurface = accompanyingFile.AccompanyingFileHousingNavigation.LivingSpace.HasValue
				? (int)accompanyingFile.AccompanyingFileHousingNavigation.LivingSpace.Value
				: null,
			LogementType = accompanyingFile.AccompanyingFileHousingNavigation.HousingType == (int)HousingType.IndividualHouse ? "maison" : "appartement",
			ParcoursAide = "rénovation énergétique",
			ProjetDpeVise = accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpe.HasValue
				? accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpe.Value + 1
				: accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeafterWork.HasValue
					? accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeafterWork.Value + 1
					: null,
			ProjetTravaux = totalWorkCost > 0 ? totalWorkCost : null,
			VousProprietaireStatut = ownershipStatus.HasValue
				? ownershipStatus.Value switch
				{
					(int)OwnershipStatus.PrivateParkTenant or (int)OwnershipStatus.PublicParkTenant => "locataire",
					(int)OwnershipStatus.OccupantFreeOfCharge => "occupant à titre gratuit",
					_ => "propriétaire"
				}
				: null,
		};

		try
		{
			var eligibilityReport = await renoHelpClient.GetEligibilityAsync(payload);
			return new CallToolResult
			{
				Content = [new TextContentBlock { Text = FormatEligibilityAsMarkdown(eligibilityReport) }]
			};
		}
		catch (Exception ex)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Error fetching eligibility information: {ex.Message}" }]
			};
		}
	}
}
