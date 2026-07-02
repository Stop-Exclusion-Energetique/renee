using System.Text.Json.Serialization;

namespace Renee.McpServer.Models;

public class PublicodeRequest
{
	[JsonPropertyName("denormandie . années de location")]
	public int? DenormandieAnneesLocation { get; set; }

	[JsonPropertyName("denormandie . gestes minimum")]
	public string? DenormandieGestesMinimum { get; set; }

	[JsonPropertyName("DPE . actuel")]
	public int? DpeActuel { get; set; }

	[JsonPropertyName("ménage . commune")]
	public string? MenageCommune { get; set; }

	[JsonPropertyName("ménage . personnes")]
	public int? MenagePersonnes { get; set; }

	[JsonPropertyName("ménage . revenu")]
	public int? MenageRevenu { get; set; }

	[JsonPropertyName("ménage . revenu . classe")]
	public string? MenageRevenueClasse { get; set; }

	[JsonPropertyName("logement . commune")]
	public string? LogementCommune { get; set; }

	[JsonPropertyName("logement . période de construction")]
	public string? LogementPeriodeConstruction { get; set; }

	[JsonPropertyName("logement . propriétaire occupant")]
	public string? LogementProprietaireOccupant { get; set; }

	[JsonPropertyName("logement . résidence principale locataire")]
	public string? LogementResidencePrincipaleLocataire { get; set; }

	[JsonPropertyName("logement . résidence principale propriétaire")]
	public string? LogementResidencePrincipalePropriétaire { get; set; }

	[JsonPropertyName("logement . prix d'achat")]
	public int? LogementPrixAchat { get; set; }

	[JsonPropertyName("logement . surface")]
	public int? LogementSurface { get; set; }

	[JsonPropertyName("logement . taxe foncière")]
	public int? LogementTaxeFonciere { get; set; }

	[JsonPropertyName("logement . type")]
	public string? LogementType { get; set; }

	[JsonPropertyName("parcours d'aide")]
	public string? ParcoursAide { get; set; }

	[JsonPropertyName("projet . DPE visé")]
	public int? ProjetDpeVise { get; set; }

	[JsonPropertyName("projet . gain énergétique")]
	public string? ProjetGainEnergetique { get; set; }

	[JsonPropertyName("projet . travaux")]
	public int? ProjetTravaux { get; set; }

	[JsonPropertyName("taxe foncière . condition de dépenses")]
	public string? TaxeFonciereConditionDepenses { get; set; }

	[JsonPropertyName("vous . propriétaire . statut")]
	public string? VousProprietaireStatut { get; set; }
}

public class EligibilityReport
{
	[JsonPropertyName("aidesEligibles")]
	public List<EligibleAid> AidesEligibles { get; set; } = [];

	[JsonPropertyName("aidesNonEligibles")]
	public List<NonEligibleAid> AidesNonEligibles { get; set; } = [];

	[JsonPropertyName("aidesIncomplètes")]
	public List<IncompleteAid> AidesIncompletes { get; set; } = [];
}

public class EligibleAid
{
	[JsonPropertyName("nom")]
	public string Nom { get; set; } = string.Empty;

	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("montant")]
	public string Montant { get; set; } = string.Empty;

	[JsonPropertyName("taux")]
	public string? Taux { get; set; }

	[JsonPropertyName("durée")]
	public string? Duree { get; set; }
}

public class NonEligibleAid
{
	[JsonPropertyName("nom")]
	public string Nom { get; set; } = string.Empty;

	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;
}

public class IncompleteAid
{
	[JsonPropertyName("nom")]
	public string Nom { get; set; } = string.Empty;

	[JsonPropertyName("type")]
	public string Type { get; set; } = string.Empty;

	[JsonPropertyName("informationsManquantes")]
	public List<string> InformationsManquantes { get; set; } = [];
}
