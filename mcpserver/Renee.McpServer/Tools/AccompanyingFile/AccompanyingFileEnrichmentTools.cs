using System.ComponentModel;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Infrastructure.Data;
using Renee.Infrastructure.FileServices;
using Renee.McpServer.Models;

namespace Renee.McpServer.Tools;

[McpServerToolType]
public class AccompanyingFileEnrichmentTools(
	ReneeDbContext context,
	IHttpContextAccessor httpContextAccessor,
	ILogger<AccompanyingFileEnrichmentTools> logger,
	IConfiguration configuration,
	IEncryptionService encryptionService)
{
	private static readonly JsonSerializerOptions jsonSerializerOptions = new()
	{
		WriteIndented = false,
		PropertyNameCaseInsensitive = true,
	};

	[McpServerTool]
	[Description("""
        Récupère les données enrichissables actuelles d'un dossier (Housing, Address, MainOccupant, Household, HousingInitialState)
        sous forme d'un objet JSON compact contenant uniquement les champs modifiables via enrichissement.
        À appeler en premier lorsqu'un fichier texte doit être analysé pour proposer un enrichissement.
        Cela permet de comparer les valeurs actuelles du dossier avec les informations du fichier avant de proposer des modifications.
        """)]
	[Authorize]
	public async Task<CallToolResult> GetAccompanyingFileEnrichmentContext(
		[Description("Référence du dossier (portion acceptée).")] string fileReference
	)
	{
		var af = await context.AccompanyingFiles
			.Include(a => a.AccompanyingFileHousingNavigation.HousingAddressNavigation)
			.Include(a => a.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
			.Include(a => a.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
			.Where(a => a.IsDeleted != true &&
						(a.AccompanyingFileReference == fileReference ||
						 a.AccompanyingFileReference.Contains(fileReference)))
			.FirstOrDefaultAsync();

		if (af is null)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Aucun dossier trouvé pour '{fileReference}'." }]
			};
		}

		var h = af.AccompanyingFileHousingNavigation;
		var adr = h.HousingAddressNavigation;
		var ini = h.HousingInitialStateNavigation;
		var hh = af.AccompanyingFileHouseholdNavigation;
		var mo = hh.MainOccupantNavigation;

		var ctx = new
		{
			DossierId = af.Id,
			Reference = af.AccompanyingFileReference,
			Milestone = af.AccompanyingFileMilestone,
			Status = af.AccompanyingFileStatus,
			IdentificationLocked = af.IdentifyMilestoneValidatedBy is not null,
			OrganizeFinanceLocked = af.OrganizeAndFinanceMilestoneValidatedBy is not null,
			RealizeFollowLocked = af.RealizeAndFollowMilestoneValidatedBy is not null,
			Housing = new
			{
				h.LivingSpace,
				h.HousingType,
				h.ConstructionYear,
				h.OwnershipStatus,
				h.NumberOfRoom,
				h.NumberOfFloor,
				h.CadastralReference,
				h.HasPreviousWork,
				h.CommentOnPreviousWork,
				h.YearOfAcquisitionOrEntry,
				h.CeilingHeight,
				h.IsInAbfarea,
				h.SunExposure,
				h.NumberOfWindow,
				h.NumberOfDoor,
				h.NumberOfBayWindow,
				h.NumberOfPatioDoor,
				h.NumberOfRoofDoor,
				h.GeographicAreaTypology,
				h.ArchitecturalOrTownPlanningStandards
			},
			Address = new
			{
				adr.PostalCode,
				adr.City,
				adr.Department,
				adr.Region,
				adr.Street,
				adr.HouseNumber,
				adr.Label,
				adr.AdditionnalComment
			},
			MainOccupant = new
			{
				mo.FirstName,
				mo.LastName,
				mo.PhoneNumber,
				mo.Email,
				mo.Job,
				mo.Birthdate,
				mo.Age,
				mo.SocioProfessionalCategory
			},
			Household = new
			{
				hh.HouseholdTypology,
				hh.SocialContext,
				hh.HouseholdProject,
				hh.ReferenceIncomeTax,
				hh.IsFollowedByAnSocialWorker,
				hh.HasAnOccupantWithDisabilities,
				hh.HasAnOccupantWithLongTermIllness,
				hh.HasAnOccupantWithIndependenceLoss,
				hh.HasOverdueInvoice,
				hh.CommentsOnHouseholdDifficulties,
				hh.HouseholdAvailabilityForVisits
			},
			HousingInitialState = new
			{
				ini.Dpe,
				ini.Ges,
				ini.AnnualEnergyConsumption,
				ini.AnnualGesemission,
				ini.DegradationIndex,
				ini.UnsanitaryCoefficient,
				ini.EnergyDepravation,
				ini.HeatingEnergy,
				ini.InitialStateDiagnosticCommentary,
				ini.WinterThermalComfortLevel,
				ini.SummerThermalComfortLevel,
				ini.NoiseComfortLevel,
				ini.RoofingState,
				ini.WallsState,
				ini.FloorState,
				ini.CarpentryState,
				ini.HasPestOrMold,
				ini.HumidityState,
				ini.VentilationState,
				ini.HasVentilationSystem,
				ini.HeatingState,
				ini.HasHeatingSystem,
				ini.HotWaterProductionState,
				ini.HasHotWaterProduction,
				ini.ElectricalSafetyState,
				ini.HasFaultyElectricalSystem,
				ini.GasSafetyState,
				ini.FireSafetyState,
				ini.LeadAndAsbestosState,
				ini.SanitaryPlumbingState,
				ini.SanitationState,
				ini.InteriorDesignState,
				ini.HasHousingCover,
				ini.HasOpenings,
				ini.DisordersObservedCommentary,
				ini.HasInsulation
			}
		};

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(ctx, jsonSerializerOptions)
		};
	}

	[McpServerTool]
	[Description("""
        Applique des modifications d'enrichissement validées par l'utilisateur sur un dossier existant.
        IMPORTANT : n'appeler cet outil qu'après confirmation explicite de l'utilisateur.
        Le paramètre enrichmentChangesJson doit être un objet JSON avec un tableau 'changes' contenant des objets { section, field, value }.
        Sections disponibles : Housing, Address, MainOccupant, Household, HousingInitialState.
        Les champs de type enum doivent utiliser leur valeur entière (ex: HousingType 0=Maison individuelle, 1=Résidentiel collectif ; Dpe 0=A … 6=G ; états 0=Bon, 1=Moyen, 2=Mauvais).
        Retourne les changements appliqués, bloqués (jalon verrouillé) et les liens vers les pages modifiées.
        """)]
	[Authorize]
	public async Task<CallToolResult> ApplyAccompanyingFileEnrichment(
		[Description("Référence du dossier à enrichir (portion acceptée).")] string fileReference,
		[Description("""JSON des modifications validées. Format: { "changes": [{ "section": "Housing", "field": "LivingSpace", "value": "85" }] }""")] string enrichmentChangesJson
	)
	{
		EnrichmentRequest? request;
		try
		{
			request = JsonSerializer.Deserialize<EnrichmentRequest>(enrichmentChangesJson, jsonSerializerOptions);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "JSON invalide pour enrichissement : {Json}", enrichmentChangesJson);
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Le format JSON des modifications est invalide." }]
			};
		}

		if (request is null || request.Changes.Count == 0)
		{
			return new CallToolResult
			{
				Content = [new TextContentBlock { Text = "Aucune modification à appliquer." }]
			};
		}

		var accompanyingFile = await context.AccompanyingFiles
			.Include(af => af.AccompanyingFileHousingNavigation.HousingAddressNavigation)
			.Include(af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation)
			.Include(af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation)
			.Where(af => af.IsDeleted != true &&
						 (af.AccompanyingFileReference == fileReference ||
						  af.AccompanyingFileReference.Contains(fileReference)))
			.FirstOrDefaultAsync();

		if (accompanyingFile is null)
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = $"Aucun dossier trouvé pour la référence '{fileReference}'." }]
			};
		}

		var userRole = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
		var result = new EnrichmentResult { Success = true };
		var modifiedMilestones = new HashSet<AccompanyingFileStage>();

		foreach (var change in request.Changes)
		{
			var milestone = MilestoneForSection(change.Section);

			if (!CanEdit(userRole, accompanyingFile, milestone))
			{
				var lockedAt = LockedAt(accompanyingFile, milestone);
				result.BlockedChanges.Add(new BlockedChange
				{
					Section = change.Section,
					Field = change.Field,
					Reason = $"Le jalon '{MilestoneName(milestone)}' a été validé" +
							 (lockedAt.HasValue ? $" le {lockedAt:dd/MM/yyyy}" : "") +
							 ". Seuls les rôles Admin, ET, CD et CC peuvent modifier."
				});
				continue;
			}

			var (applied, warning) = ApplyChange(change, accompanyingFile);

			if (applied)
			{
				result.AppliedChanges.Add(change);
				modifiedMilestones.Add(milestone);
			}
			else if (warning is not null)
			{
				result.Warnings.Add(warning);
			}
		}

		if (result.AppliedChanges.Count > 0)
		{
			try
			{
				accompanyingFile.LastUpdateDate = DateTime.UtcNow;
				await context.SaveChangesAsync();
				result.ModifiedPages = [.. modifiedMilestones.Select(m => PageRoute(m, accompanyingFile.Id))];
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Erreur DB lors de l'enrichissement du dossier {Reference}", fileReference);
				return new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = "Erreur lors de la sauvegarde. Aucune modification n'a été appliquée." }]
				};
			}
		}

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(result, jsonSerializerOptions)
		};
	}

	[McpServerTool]
	[Description("""
        Analyse le contenu d'un document PDF stocké dans les pièces jointes d'un dossier (blob storage).
        Extrait le texte, les cases à cocher et les tableaux via Azure Document Intelligence.
        À appeler UNIQUEMENT si l'utilisateur demande d'analyser un document déjà présent dans les pièces jointes du dossier et que son contenu n'est PAS dans le message courant.
        Ne PAS appeler si le message contient déjà un bloc [PDF joint : ...] : dans ce cas, le contenu extrait est déjà dans le message, l'analyser directement sans appeler cet outil.
        """)]
	[Authorize]
	public async Task<CallToolResult> AnalyzeDossierDocumentContent(
		[Description("Référence du dossier.")] string fileReference,
		[Description("Nom du document tel qu'il apparaît dans les pièces jointes du dossier.")] string documentName
	)
	{
		var endpoint = configuration["AzureDocumentIntelligence:Endpoint"];
		var apiKey = configuration["AzureDocumentIntelligence:ApiKey"];
		if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Azure Document Intelligence non configuré." }]
			};

		try
		{
			var connectionString = configuration.GetConnectionString("AzureBlobStorage");
			var containerName = configuration["BlobStorageAzure:ContainerName"];
			var blobServiceClient = new BlobServiceClient(connectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

			var prefix = $"{fileReference}_{documentName}";
			string? resolvedBlobName = null;
			await foreach (var item in containerClient.GetBlobsAsync(prefix: prefix))
			{
				resolvedBlobName = item.Name;
				break;
			}

			if (resolvedBlobName is null)
			{
				return new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = $"Document introuvable pour le dossier '{fileReference}' et le nom '{documentName}'." }]
				};
			}

			var blobClient = containerClient.GetBlobClient(resolvedBlobName);
			var download = (await blobClient.DownloadAsync()).Value;

			var encryptedStream = new MemoryStream();
			await download.Content.CopyToAsync(encryptedStream);
			encryptedStream.Position = 0;

			var decryptedStream = new MemoryStream();
			await encryptionService.DecryptFile(encryptedStream, decryptedStream);
			decryptedStream.Position = 0;

			var diClient = new DocumentIntelligenceClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
			var operation = await diClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", await BinaryData.FromStreamAsync(decryptedStream));
			var result = operation.Value;

			var content = DocumentIntelligenceHelper.ProcessContent(result.Content);
			var tables = DocumentIntelligenceHelper.RenderTables(result);
			var full = string.IsNullOrWhiteSpace(tables)
				? content
				: $"{content}\n\n---\nTableaux :\n{tables}";

			if (string.IsNullOrWhiteSpace(full))
			{
				return new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = "Le document ne contient aucun contenu lisible." }]
				};
			}

			return new CallToolResult
			{
				Content = [new TextContentBlock { Text = full.Trim() }]
			};
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Erreur lors de l'analyse du document {Document} du dossier {Reference}", documentName, fileReference);
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Erreur lors de l'analyse du document." }]
			};
		}
	}

	private static bool CanEdit(string role, AccompanyingFile af, AccompanyingFileStage milestone)
	{
		if (role is Constants.AdminRole
				 or Constants.TerritorialBuilderRole
				 or Constants.DiffuseCoordinatorRole
				 or Constants.TargetedCoordinatorRole)
			return true;

		if (role == Constants.StructuralReferentRole)
			return false;

		return milestone switch
		{
			AccompanyingFileStage.Identify => af.IdentifyMilestoneValidatedBy is null,
			AccompanyingFileStage.OrganizingAndFinancing => af.OrganizeAndFinanceMilestoneValidatedBy is null,
			AccompanyingFileStage.RealisationAndFollowing => af.RealizeAndFollowMilestoneValidatedBy is null,
			_ => false
		};
	}

	private static AccompanyingFileStage MilestoneForSection(string section) => section switch
	{
		"HousingInitialState" => AccompanyingFileStage.OrganizingAndFinancing,
		_ => AccompanyingFileStage.Identify
	};

	private static DateTime? LockedAt(AccompanyingFile af, AccompanyingFileStage milestone) => milestone switch
	{
		AccompanyingFileStage.Identify => af.IdentifySynthesisValidationDate,
		AccompanyingFileStage.OrganizingAndFinancing => af.OrganizeAndFinanceSynthesisValidationDate,
		AccompanyingFileStage.RealisationAndFollowing => af.RealizeAndFollowSynthesisValidationDate,
		_ => null
	};

	private static string MilestoneName(AccompanyingFileStage milestone) => milestone switch
	{
		AccompanyingFileStage.Identify => "Identification",
		AccompanyingFileStage.OrganizingAndFinancing => "Organiser et financer",
		AccompanyingFileStage.RealisationAndFollowing => "Réaliser et suivre",
		_ => "Inconnu"
	};

	private static string PageRoute(AccompanyingFileStage milestone, Guid id) => milestone switch
	{
		AccompanyingFileStage.Identify => $"/newoccupantdisplay/{id}",
		AccompanyingFileStage.OrganizingAndFinancing => $"/organizeAndFinance/{id}",
		AccompanyingFileStage.RealisationAndFollowing => $"/realiseAndFollow/{id}",
		_ => $"/menu/{id}"
	};

	private static (bool applied, string? warning) ApplyChange(EnrichmentChange change, AccompanyingFile af)
		=> change.Section switch
		{
			"Housing" => ApplyHousing(change.Field, change.Value, af.AccompanyingFileHousingNavigation),
			"Address" => ApplyAddress(change.Field, change.Value, af.AccompanyingFileHousingNavigation.HousingAddressNavigation),
			"MainOccupant" => ApplyMainOccupant(change.Field, change.Value, af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation),
			"Household" => ApplyHousehold(change.Field, change.Value, af.AccompanyingFileHouseholdNavigation),
			"HousingInitialState" => ApplyInitialState(change.Field, change.Value, af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation),
			_ => (false, $"Section inconnue : '{change.Section}'. Valides : Housing, Address, MainOccupant, Household, HousingInitialState.")
		};

	private static (bool, string?) ApplyHousing(string field, string? value, Housing h) => field switch
	{
		"LivingSpace" => ParseDouble(value, out var d) ? Set(() => h.LivingSpace = d) : Warn($"Housing.LivingSpace : '{value}' non convertible en nombre."),
		"HousingType" => ParseInt(value, out var i) ? Set(() => h.HousingType = i) : Warn($"Housing.HousingType : '{value}' non convertible en entier."),
		"ConstructionYear" => ParseInt(value, out var i) ? Set(() => h.ConstructionYear = i) : Warn($"Housing.ConstructionYear : '{value}' non convertible en entier."),
		"OwnershipStatus" => ParseInt(value, out var i) ? Set(() => h.OwnershipStatus = i) : Warn($"Housing.OwnershipStatus : '{value}' non convertible en entier."),
		"NumberOfRoom" => ParseInt(value, out var i) ? Set(() => h.NumberOfRoom = i) : Warn($"Housing.NumberOfRoom : '{value}' non convertible en entier."),
		"NumberOfFloor" => ParseInt(value, out var i) ? Set(() => h.NumberOfFloor = i) : Warn($"Housing.NumberOfFloor : '{value}' non convertible en entier."),
		"CadastralReference" => Set(() => h.CadastralReference = value),
		"HasPreviousWork" => ParseBool(value, out var b) ? Set(() => h.HasPreviousWork = b) : Warn($"Housing.HasPreviousWork : '{value}' non convertible en booléen."),
		"CommentOnPreviousWork" => Set(() => h.CommentOnPreviousWork = value),
		"YearOfAcquisitionOrEntry" => ParseInt(value, out var i) ? Set(() => h.YearOfAcquisitionOrEntry = i) : Warn($"Housing.YearOfAcquisitionOrEntry : '{value}' non convertible en entier."),
		"CeilingHeight" => ParseDouble(value, out var d) ? Set(() => h.CeilingHeight = d) : Warn($"Housing.CeilingHeight : '{value}' non convertible en nombre."),
		"IsInAbfarea" => ParseBool(value, out var b) ? Set(() => h.IsInAbfarea = b) : Warn($"Housing.IsInAbfarea : '{value}' non convertible en booléen."),
		"SunExposure" => ParseInt(value, out var i) ? Set(() => h.SunExposure = i) : Warn($"Housing.SunExposure : '{value}' non convertible en entier (0=Est, 1=Nord-Est, 2=Nord, 3=Nord-Ouest, 4=Ouest, 5=Sud-Ouest, 6=Sud, 7=Sud-Est)."),
		"NumberOfWindow" => ParseInt(value, out var i) ? Set(() => h.NumberOfWindow = i) : Warn($"Housing.NumberOfWindow : '{value}' non convertible en entier."),
		"NumberOfDoor" => ParseInt(value, out var i) ? Set(() => h.NumberOfDoor = i) : Warn($"Housing.NumberOfDoor : '{value}' non convertible en entier."),
		"NumberOfBayWindow" => ParseInt(value, out var i) ? Set(() => h.NumberOfBayWindow = i) : Warn($"Housing.NumberOfBayWindow : '{value}' non convertible en entier."),
		"NumberOfPatioDoor" => ParseInt(value, out var i) ? Set(() => h.NumberOfPatioDoor = i) : Warn($"Housing.NumberOfPatioDoor : '{value}' non convertible en entier."),
		"NumberOfRoofDoor" => ParseInt(value, out var i) ? Set(() => h.NumberOfRoofDoor = i) : Warn($"Housing.NumberOfRoofDoor : '{value}' non convertible en entier."),
		"GeographicAreaTypology" => ParseInt(value, out var i) ? Set(() => h.GeographicAreaTypology = i) : Warn($"Housing.GeographicAreaTypology : '{value}' non convertible en entier."),
		"ArchitecturalOrTownPlanningStandards" => Set(() => h.ArchitecturalOrTownPlanningStandards = value),
		_ => (false, $"Champ Housing.{field} non reconnu ou non modifiable via enrichissement.")
	};

	private static (bool, string?) ApplyAddress(string field, string? value, Address a) => field switch
	{
		"PostalCode" => !string.IsNullOrWhiteSpace(value) ? Set(() => a.PostalCode = value) : (false, "Address.PostalCode ne peut pas être vide."),
		"City" => !string.IsNullOrWhiteSpace(value) ? Set(() => a.City = value) : (false, "Address.City ne peut pas être vide."),
		"Department" => Set(() => a.Department = value ?? string.Empty),
		"Region" => Set(() => a.Region = value ?? string.Empty),
		"Street" => Set(() => a.Street = value),
		"HouseNumber" => Set(() => a.HouseNumber = value),
		"Label" => !string.IsNullOrWhiteSpace(value) ? Set(() => a.Label = value) : (false, "Address.Label ne peut pas être vide."),
		"AdditionnalComment" => Set(() => a.AdditionnalComment = value),
		_ => (false, $"Champ Address.{field} non reconnu ou non modifiable via enrichissement.")
	};

	private static (bool, string?) ApplyMainOccupant(string field, string? value, MainOccupant o) => field switch
	{
		"FirstName" => Set(() => o.FirstName = value),
		"LastName" => Set(() => o.LastName = value),
		"PhoneNumber" => Set(() => o.PhoneNumber = value),
		"Email" => Set(() => o.Email = value),
		"Job" => Set(() => o.Job = value),
		"Birthdate" => ParseDate(value, out var dt) ? Set(() => o.Birthdate = dt) : Warn($"MainOccupant.Birthdate : '{value}' non convertible en date."),
		"Age" => ParseInt(value, out var i) ? Set(() => o.Age = i) : Warn($"MainOccupant.Age : '{value}' non convertible en entier."),
		"SocioProfessionalCategory" => ParseInt(value, out var i) ? Set(() => o.SocioProfessionalCategory = i) : Warn($"MainOccupant.SocioProfessionalCategory : '{value}' non convertible en entier."),
		_ => (false, $"Champ MainOccupant.{field} non reconnu ou non modifiable via enrichissement.")
	};

	private static (bool, string?) ApplyHousehold(string field, string? value, Household h) => field switch
	{
		"HouseholdTypology" => ParseInt(value, out var i) ? Set(() => h.HouseholdTypology = i) : Warn($"Household.HouseholdTypology : '{value}' non convertible en entier (0=Couple+adulte hébergé, 1=Couple+enfants, 2=Couple sans enfants, 3=Famille monoparentale, 4=Personne seule, 5=Personne seule+adulte hébergé)."),
		"SocialContext" => Set(() => h.SocialContext = value),
		"HouseholdProject" => Set(() => h.HouseholdProject = value),
		"ReferenceIncomeTax" => ParseDouble(value, out var d) ? Set(() => h.ReferenceIncomeTax = d) : Warn($"Household.ReferenceIncomeTax : '{value}' non convertible en nombre."),
		"IsFollowedByAnSocialWorker" => ParseBool(value, out var b) ? Set(() => h.IsFollowedByAnSocialWorker = b) : Warn($"Household.IsFollowedByAnSocialWorker : '{value}' non convertible en booléen."),
		"HasAnOccupantWithDisabilities" => ParseBool(value, out var b) ? Set(() => h.HasAnOccupantWithDisabilities = b) : Warn($"Household.HasAnOccupantWithDisabilities : '{value}' non convertible en booléen."),
		"HasAnOccupantWithLongTermIllness" => ParseBool(value, out var b) ? Set(() => h.HasAnOccupantWithLongTermIllness = b) : Warn($"Household.HasAnOccupantWithLongTermIllness : '{value}' non convertible en booléen."),
		"HasAnOccupantWithIndependenceLoss" => ParseBool(value, out var b) ? Set(() => h.HasAnOccupantWithIndependenceLoss = b) : Warn($"Household.HasAnOccupantWithIndependenceLoss : '{value}' non convertible en booléen."),
		"HasOverdueInvoice" => ParseBool(value, out var b) ? Set(() => h.HasOverdueInvoice = b) : Warn($"Household.HasOverdueInvoice : '{value}' non convertible en booléen."),
		"CommentsOnHouseholdDifficulties" => Set(() => h.CommentsOnHouseholdDifficulties = value),
		"HouseholdAvailabilityForVisits" => Set(() => h.HouseholdAvailabilityForVisits = value),
		_ => (false, $"Champ Household.{field} non reconnu ou non modifiable via enrichissement.")
	};

	private static (bool, string?) ApplyInitialState(string field, string? value, HousingInitialState s) => field switch
	{
		"Dpe" => ParseInt(value, out var i) ? Set(() => s.Dpe = i) : Warn($"HousingInitialState.Dpe : '{value}' non convertible en entier (0=A … 6=G)."),
		"Ges" => ParseInt(value, out var i) ? Set(() => s.Ges = i) : Warn($"HousingInitialState.Ges : '{value}' non convertible en entier (0=A … 6=G)."),
		"AnnualEnergyConsumption" => ParseDouble(value, out var d) ? Set(() => s.AnnualEnergyConsumption = d) : Warn($"HousingInitialState.AnnualEnergyConsumption : '{value}' non convertible en nombre."),
		"AnnualGesemission" => ParseDouble(value, out var d) ? Set(() => s.AnnualGesemission = d) : Warn($"HousingInitialState.AnnualGesemission : '{value}' non convertible en nombre."),
		"DegradationIndex" => ParseInt(value, out var i) ? Set(() => s.DegradationIndex = i) : Warn($"HousingInitialState.DegradationIndex : '{value}' non convertible en entier (0=ID<0.35, 1=0.35≤ID<0.55, 2=ID≥0.55)."),
		"UnsanitaryCoefficient" => ParseInt(value, out var i) ? Set(() => s.UnsanitaryCoefficient = i) : Warn($"HousingInitialState.UnsanitaryCoefficient : '{value}' non convertible en entier."),
		"EnergyDepravation" => ParseInt(value, out var i) ? Set(() => s.EnergyDepravation = i) : Warn($"HousingInitialState.EnergyDepravation : '{value}' non convertible en entier (0=Aucune, 1=Partielle, 2=Totale)."),
		"InitialStateDiagnosticCommentary" => Set(() => s.InitialStateDiagnosticCommentary = value),
		"HeatingEnergy" => Set(() => s.HeatingEnergy = value),
		"WinterThermalComfortLevel" => ParseInt(value, out var i) ? Set(() => s.WinterThermalComfortLevel = i) : Warn($"HousingInitialState.WinterThermalComfortLevel : '{value}' non convertible en entier."),
		"SummerThermalComfortLevel" => ParseInt(value, out var i) ? Set(() => s.SummerThermalComfortLevel = i) : Warn($"HousingInitialState.SummerThermalComfortLevel : '{value}' non convertible en entier."),
		"RoofingState" => ParseInt(value, out var i) ? Set(() => s.RoofingState = i) : Warn($"HousingInitialState.RoofingState : '{value}' non convertible en entier."),
		"WallsState" => ParseInt(value, out var i) ? Set(() => s.WallsState = i) : Warn($"HousingInitialState.WallsState : '{value}' non convertible en entier."),
		"FloorState" => ParseInt(value, out var i) ? Set(() => s.FloorState = i) : Warn($"HousingInitialState.FloorState : '{value}' non convertible en entier."),
		"HasPestOrMold" => ParseBool(value, out var b) ? Set(() => s.HasPestOrMold = b) : Warn($"HousingInitialState.HasPestOrMold : '{value}' non convertible en booléen."),
		"HumidityState" => ParseInt(value, out var i) ? Set(() => s.HumidityState = i) : Warn($"HousingInitialState.HumidityState : '{value}' non convertible en entier."),
		"VentilationState" => ParseInt(value, out var i) ? Set(() => s.VentilationState = i) : Warn($"HousingInitialState.VentilationState : '{value}' non convertible en entier."),
		"HasVentilationSystem" => ParseBool(value, out var b) ? Set(() => s.HasVentilationSystem = b) : Warn($"HousingInitialState.HasVentilationSystem : '{value}' non convertible en booléen."),
		"HeatingState" => ParseInt(value, out var i) ? Set(() => s.HeatingState = i) : Warn($"HousingInitialState.HeatingState : '{value}' non convertible en entier."),
		"HotWaterProductionState" => ParseInt(value, out var i) ? Set(() => s.HotWaterProductionState = i) : Warn($"HousingInitialState.HotWaterProductionState : '{value}' non convertible en entier."),
		"HasHotWaterProduction" => ParseBool(value, out var b) ? Set(() => s.HasHotWaterProduction = b) : Warn($"HousingInitialState.HasHotWaterProduction : '{value}' non convertible en booléen."),
		"DisordersObservedCommentary" => Set(() => s.DisordersObservedCommentary = value),
		"HasInsulation" => ParseBool(value, out var b) ? Set(() => s.HasInsulation = b) : Warn($"HousingInitialState.HasInsulation : '{value}' non convertible en booléen."),
		"NoiseComfortLevel" => ParseInt(value, out var i) ? Set(() => s.NoiseComfortLevel = i) : Warn($"HousingInitialState.NoiseComfortLevel : '{value}' non convertible en entier."),
		"CarpentryState" => ParseInt(value, out var i) ? Set(() => s.CarpentryState = i) : Warn($"HousingInitialState.CarpentryState : '{value}' non convertible en entier."),
		"ElectricalSafetyState" => ParseInt(value, out var i) ? Set(() => s.ElectricalSafetyState = i) : Warn($"HousingInitialState.ElectricalSafetyState : '{value}' non convertible en entier."),
		"GasSafetyState" => ParseInt(value, out var i) ? Set(() => s.GasSafetyState = i) : Warn($"HousingInitialState.GasSafetyState : '{value}' non convertible en entier."),
		"FireSafetyState" => ParseInt(value, out var i) ? Set(() => s.FireSafetyState = i) : Warn($"HousingInitialState.FireSafetyState : '{value}' non convertible en entier."),
		"LeadAndAsbestosState" => ParseInt(value, out var i) ? Set(() => s.LeadAndAsbestosState = i) : Warn($"HousingInitialState.LeadAndAsbestosState : '{value}' non convertible en entier."),
		"SanitaryPlumbingState" => ParseInt(value, out var i) ? Set(() => s.SanitaryPlumbingState = i) : Warn($"HousingInitialState.SanitaryPlumbingState : '{value}' non convertible en entier."),
		"SanitationState" => ParseInt(value, out var i) ? Set(() => s.SanitationState = i) : Warn($"HousingInitialState.SanitationState : '{value}' non convertible en entier."),
		"InteriorDesignState" => ParseInt(value, out var i) ? Set(() => s.InteriorDesignState = i) : Warn($"HousingInitialState.InteriorDesignState : '{value}' non convertible en entier."),
		"HasFaultyElectricalSystem" => ParseBool(value, out var b) ? Set(() => s.HasFaultyElectricalSystem = b) : Warn($"HousingInitialState.HasFaultyElectricalSystem : '{value}' non convertible en booléen."),
		"HasHeatingSystem" => ParseBool(value, out var b) ? Set(() => s.HasHeatingSystem = b) : Warn($"HousingInitialState.HasHeatingSystem : '{value}' non convertible en booléen."),
		"HasHousingCover" => ParseBool(value, out var b) ? Set(() => s.HasHousingCover = b) : Warn($"HousingInitialState.HasHousingCover : '{value}' non convertible en booléen."),
		"HasOpenings" => ParseBool(value, out var b) ? Set(() => s.HasOpenings = b) : Warn($"HousingInitialState.HasOpenings : '{value}' non convertible en booléen."),
		_ => (false, $"Champ HousingInitialState.{field} non reconnu ou non modifiable via enrichissement.")
	};

	private static bool ParseDouble(string? value, out double result)
	{
		result = 0;
		if (string.IsNullOrWhiteSpace(value)) return false;
		return double.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out result);
	}

	private static bool ParseInt(string? value, out int result)
	{
		result = 0;
		return !string.IsNullOrWhiteSpace(value) && int.TryParse(value.Trim(), out result);
	}

	private static bool ParseBool(string? value, out bool result)
	{
		result = false;
		if (string.IsNullOrWhiteSpace(value)) return false;
		switch (value.Trim().ToLowerInvariant())
		{
			case "true": case "oui": case "yes": case "1": result = true; return true;
			case "false": case "non": case "no": case "0": result = false; return true;
			default: return false;
		}
	}

	private static bool ParseDate(string? value, out DateTime result)
	{
		result = DateTime.MinValue;
		if (string.IsNullOrWhiteSpace(value)) return false;
		return DateTime.TryParse(value, CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None, out result)
			|| DateTime.TryParse(value, out result);
	}

	private static (bool, string?) Set(Action apply) { apply(); return (true, null); }
	private static (bool, string?) Warn(string msg) => (false, msg);
}
