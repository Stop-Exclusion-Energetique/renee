using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Renee.Application.DTOs.AI;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.ReneeError;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Renee.Infrastructure.AI;

public class AIDossierSynthesisService(
    IAccompanyingFileService accompanyingFileService,
    IConfiguration configuration) : IAIDossierSynthesisService
{
    private static readonly HashSet<string> AllowedJalons =
    [
        "Identifier",
        "Organiser et Financer",
        "Réaliser et Suivre"
    ];

    private static readonly JsonSerializerOptions NullOmittingSerializerOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private IChatClient CreateClient()
    {
        var uri = configuration["AzureFoundryResources:ClientUri"]
            ?? throw new InvalidOperationException("Configuration 'AzureFoundryResources:ClientUri' manquante.");
        var key = configuration["AzureFoundryResources:AzureKeyCredential"]
            ?? throw new InvalidOperationException("Configuration 'AzureFoundryResources:AzureKeyCredential' manquante.");
        var deployment = configuration["AzureFoundryResources:DeploymentName"]
            ?? throw new InvalidOperationException("Configuration 'AzureFoundryResources:DeploymentName' manquante.");

        return new AzureOpenAIClient(new Uri(uri), new AzureKeyCredential(key))
            .GetChatClient(deployment)
            .AsIChatClient();
    }

    private static ReneeOperationResult<AISynthesisResult> ParseResponse(string? rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse))
            return ReneeOperationResult<AISynthesisResult>.Failure("L'IA n'a pas retourné de réponse.");

        try
        {
            string cleaned = ExtractJsonObject(rawResponse);
            JsonDocument doc = JsonDocument.Parse(cleaned);
            JsonElement root = doc.RootElement;

            List<AnomalyItem> anomalies = [];
            if (root.TryGetProperty("anomalies", out var anomaliesEl) && anomaliesEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var a in anomaliesEl.EnumerateArray())
                {
                    string field = a.TryGetProperty("field", out var f) ? f.GetString()?.Trim() ?? "" : "";
                    string description = a.TryGetProperty("description", out var d) ? d.GetString()?.Trim() ?? "" : "";
                    string jalon = a.TryGetProperty("jalon", out var j) ? j.GetString()?.Trim() ?? "" : "";

                    if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(description))
                        continue;

                    if (!AllowedJalons.Contains(jalon))
                        continue;

                    anomalies.Add(new AnomalyItem
                    {
                        Field = field,
                        Description = description,
                        Jalon = jalon,
                        Source = "IA"
                    });
                }
            }

            return ReneeOperationResult<AISynthesisResult>.Success(new AISynthesisResult { Anomalies = anomalies });
        }
        catch
        {
            return ReneeOperationResult<AISynthesisResult>.Failure("Impossible d'analyser la réponse de l'IA.");
        }
    }

    private static string ExtractJsonObject(string rawResponse)
    {
        string cleaned = rawResponse.Trim();
        if (cleaned.StartsWith("```"))
        {
            int startFence = cleaned.IndexOf('\n');
            int endFence = cleaned.LastIndexOf("```");
            if (startFence >= 0 && endFence > startFence)
                cleaned = cleaned[(startFence + 1)..endFence].Trim();
        }

        int start = cleaned.IndexOf('{');
        int end = cleaned.LastIndexOf('}');
        if (start >= 0 && end > start)
            return cleaned[start..(end + 1)];

        return cleaned;
    }

    public async Task<ReneeOperationResult<AISynthesisResult>> AnalyzeAsync(Guid accompanyingFileId)
    {
        var identificationResult = await accompanyingFileService.GetAccompanyingFileSynthesis(accompanyingFileId);
        if (identificationResult is null || !identificationResult.IsSuccess || identificationResult.Value is null)
            return ReneeOperationResult<AISynthesisResult>.Failure("Dossier introuvable.");

        var identification = identificationResult.Value;

        GetOrganizeAndFinanceSynthesisQueryObjectResult? organizeDto = null;

        try
        {
            var organizeResult = await accompanyingFileService.GetOrganizeAndFinanceSynthesis(accompanyingFileId);
            if (organizeResult.IsSuccess) organizeDto = organizeResult.Value;
        }
        catch { }

        var aiData = new
        {
            Identification = new
            {
                Âge = identification.Age,
                CategorieSocioProfessionnelle = identification.SocioProfessionalCategory?.GetDescription(),
                NombreOccupants = identification.NumberOfOccupants,
                TypeMénage = identification.HouseholdTypology?.GetDescription(),
                StatutOccupation = identification.OwnershipStatus?.GetDescription(),
                SurfaceHabitable = identification.LivingSpaceInSquareMeter,
                ZoneGéographique = identification.GeographicAreaTypology?.GetDescription(),
                RevenuFiscalDeRéférence = identification.TaxIncome,
                CategorieDossier = identification.AnahCategory,
                ÉtiquetteDPE = identification.DpeLabel,
                ConsommationÉnergétiqueAnnuelle = identification.EnergyConsumption,
                SituationHandicap = identification.HasDisabilitySituation.HasValue ? (identification.HasDisabilitySituation.Value ? "Oui" : "Non") : null,
                MaladiesLongueDurée = identification.HasPersonWithLongTermIllness.HasValue ? (identification.HasPersonWithLongTermIllness.Value ? "Oui" : "Non") : null,
                PerteAutonomie = identification.HasPersonWithLossOfIndependence.HasValue ? (identification.HasPersonWithLossOfIndependence.Value ? "Oui" : "Non") : null,
                SuiviTravailleurSocial = identification.IsFollowedBySocialWorker.HasValue ? (identification.IsFollowedBySocialWorker.Value ? "Oui" : "Non") : null,
                ContexteSocial = identification.SocialContext,
                ProjetFamille = identification.FamilyProject,
                Ressources = identification.HouseholdResourcesTypologies
                    .Select(r => new { Libellé = r.Name, Montant = r.Value }).ToList(),
                ÉnergiesDechauffage = identification.HouseholdHeatingEnergies
            },
            OrganiserEtFinancer = organizeDto is null ? null : (object)new
            {
                TypeProjet = organizeDto.PreWorkPlanProjectType,
                TypesTravaux = organizeDto.WorkPackageSummary
                    .Select(w => new { Types = w.CurrentWorkPackageWorkTypes, CoûtTTC = w.WorkPackageTotalCost }).ToList(),
                CoûtTotalTTC = organizeDto.WorkPackageSummary.Sum(w => w.WorkPackageTotalCost),
                ÉtiquetteDPEDépart = organizeDto.InitialDpeLabel,
                ÉtiquetteDPEAprèsTravaux = organizeDto.EstimatedEnergyDpeAfterWork,
                ConsommationAnnuelleAprèsTravaux = organizeDto.EstimatedAnnualEnergyConsumptionAfterWork,
                GainÉnergétiqueEnClasses = organizeDto.EstimatedEnergyClassJump,
                MPRParcoursAccompagné = organizeDto.GuidedPathwayBonus,
                MPLD = organizeDto.DecentHousingBonus,
                MaPrimeAdapt = organizeDto.AdaptationBonus,
                MDPH = organizeDto.DepartmentalHouseForDisabledPersons,
                BonusSortiePassoire = organizeDto.ExitEnergySieveBonus,
                CEE = organizeDto.EnergySavingCertificates,
                ResteACharge = organizeDto.EstimatedRemainingAmount,
                ÉconomiesDuFoyer = organizeDto.HouseholdMaximumSavingAmountForRenovationProject,
                SoutienFamille = organizeDto.MaximumAmountSupportFamilyMembersRenovationProject,
                CapaciteEmprunt = organizeDto.ClassicBankLoan,
                AutresAides = organizeDto.FundingModes
                    .Select(fm => new { Libellé = fm.Label, Montant = fm.Value }).ToList()
            }
        };

        Console.WriteLine($"Données envoyées à l'IA : {aiData.Identification.SuiviTravailleurSocial}");

        var json = JsonSerializer.Serialize(aiData, NullOmittingSerializerOptions);
        var client = CreateClient();

        ChatOptions chatOptions = new()
        {
            Instructions = ReneeAiPromptSystem.GlobalSynthesisAnalysisInstructions,
            MaxOutputTokens = 1024,
            Temperature = 0.1f
        };

        var response = await client.GetResponseAsync(
            [
                new ChatMessage(ChatRole.User, $"{ReneeAiPromptSystem.GlobalSynthesisAnalysisPrompt}\n\nDonnées du dossier :\n{json}")
            ],
            chatOptions
        );

        return ParseResponse(response.Text);
    }
}
