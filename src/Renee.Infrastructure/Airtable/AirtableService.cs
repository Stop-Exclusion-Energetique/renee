using AirtableApiClient;
using Microsoft.Extensions.Configuration;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using System.Net.Http.Json;
using System.Text.Json;

namespace Renee.Infrastructure.Airtable;

public class AirtableService(ISendEventQuery sender, IConfiguration configuration, ITelemetryService telemetryService) : IAirtableService
{
	private readonly string baseId = configuration.GetValue<string>("Airtable:BaseId") ??
									throw new InvalidDataException("Airtable__BaseId is not set");
	private readonly string appKey = configuration.GetValue<string>("Airtable:AppKeyBearerToken") ??
									throw new InvalidDataException("Airtable__AppKey is not set");
	private readonly string tableId = configuration.GetValue<string>("Airtable:TableId") ??
									throw new InvalidDataException("Airtable__TableId is not set");
	private readonly string returnUrl = configuration.GetValue<string>("Airtable:ReturnUrl") ??
										throw new InvalidDataException("Airtable__ReturnUrl is not set");
	private readonly string returnUrlBearerToken = configuration.GetValue<string>("Airtable:ReturnUrlBearerToken") ??
												throw new InvalidDataException(
													"Airtable__ReturnUrlBearerToken is not set");

	public async Task<ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>> AddRecordAsync(
		string? userName,
		string? userEmail,
		string? userReportingStructure,
		string accompanyingFileReference,
		Guid userId,
		double askedStopFound)
	{
		try
		{
			var queryResult = await sender.Send(
				new GetAccompanyingFileDataForAirtableQuery
				{
					AccompanyingFileReference = accompanyingFileReference,
					UserId = userId
				});

			if (queryResult.IsSuccess)
			{
				using (AirtableBase airtableBase = new AirtableBase(appKey, baseId))
				{
					var fields = new Fields();

					fields.AddField("fld1aTREBMl5l8vdQ", userName); //NomEsemblier
					fields.AddField("flduSbPQDXnHKrGE5", userEmail); //email
					fields.AddField("fldosIf8XjE3l0lsc", userReportingStructure); //nom organisme
					fields.AddField(
						"fldunBue9YzouYN1E",
						queryResult.Value!.NumberOfPeopleInHousehold.ToString()); //Nombre de personnes dans le foyer
					fields.AddField(
						"fldHWQkWnQSJnA2ce",
						queryResult.Value!.MainOccupantLastName ?? string.Empty); //Noms du foyer
					fields.AddField(
						"fldEriktkRwnymJVy",
						queryResult.Value!.HousingPostalCode); //Code postal de la famille
					fields.AddField(
						"fldFhPqam2jzKTLxh",
						queryResult.Value!.HousingCity ?? string.Empty); //Ville ou village
					fields.AddField(
						"fldgdQfiTKH0REqVa",
						queryResult.Value!.IncomeTaxReference); //Revenu fiscal de référence
					fields.AddField(
						"fld1i0Gmbo6nOvHVB",
						queryResult.Value!.HouseholdResourcesTypology ?? string.Empty); //Types de ressources du ménage
					fields.AddField(
						"fldTi40CZR0MWavu4",
						queryResult.Value!.HouseholdDebt ?? string.Empty); //Endettement du ménage
					fields.AddField(
						"fldAOUIHZ7Jph5pgm",
						queryResult.Value!.HouseholdSocialSituation ?? string.Empty); //Situation sociale
					fields.AddField(
						"fldEFcsyPoBGsP4Z9",
						queryResult.Value!.HousingType ??
						string.Empty); //Type de logement ("Maison individuelle", "Maison mitoyenne", "Résidentiel collectif")
					fields.AddField(
						"fldkdvRful43bzPUs",
						queryResult.Value!.HousingYearOfConstruction); //Année de construction
					fields.AddField("flddbCr9faptaj7rj", queryResult.Value!.HousingSurface); //Superficie en m²
					fields.AddField(
						"fldSwTpApGgsI6JyT",
						queryResult.Value!
							.EnergeticPerformanceBeforeRenovation); //Consommation énergétique avant travaux (kWhEP/m²/an)
					fields.AddField(
						"fld0y40a7JeKmD8sj",
						queryResult.Value!
							.EnergeticPerformanceAfterRenovation); //Consommation énergétique après travaux (kWhEP/m²/an)
					fields.AddField(
						"fldvtnU3VsEWzyur2",
						queryResult.Value!.GesEmissionBeforeRenovation); //Emission de GES avant travaux
					fields.AddField(
						"fldHP4th7KtpKs692",
						queryResult.Value!.GesEmissionAfterRenovation); //Emission de GES après travaux
					fields.AddField(
						"fldHEvTbK0pSJcRDQ",
						queryResult.Value!.DpeLabelBeforeRenovation ??
						string.Empty); //Etiquette énergétique avant travaux (DPE) (G, F, E, D, C)
					fields.AddField(
						"fld3cNv2z0txD2Cpz",
						queryResult.Value!.DpeLabelAfterRenovation ??
						string.Empty); //Etiquette énergétique après travaux (DPE) (A,B,C,D,E,F,G)
					fields.AddField(
						"fldwsoMwxdt2rgjQk",
						queryResult.Value!.TotalDevisValue); //Montant devis total TTC (ARA et MO compris)
					fields.AddField(
						"fldBT826OD9Sn7ZqP",
						string.Empty); //Description système de chauffage ....champs nons pris en charge dans Renée
					fields.AddField(
						"fld328RIzbYenxrMo",
						string.Empty); //Description isolation ....champs nons pris en charge dans Renée
					fields.AddField(
						"fld34Ly5XwuddKqd7",
						string.Empty); //Description menuiseries ....champs nons pris en charge dans Renée
					fields.AddField(
						"fldjb0MtxZ1NSJPH2",
						string.Empty); //Description ventilation ....champs nons pris en charge dans Renée
					fields.AddField("fld7ug0ugLdJzDdRq", string.Empty); //Description ARA ...en discussion Odélia
					fields.AddField("fldO3qDHg3yfbqK8H", queryResult.Value!.AnahTotalAid); //Montant ANAH (total)
					fields.AddField("fld1jfz2qqOLbLh3F", queryResult.Value!.RegionAid); //Aides Région
					fields.AddField("fldV3OoasxDUE8shO", queryResult.Value!.DepartmentAid); //Aides Département 
					fields.AddField("fld8ybzJ8cD05GRR0", queryResult.Value!.CityAid); //Aides Métropole/Agglomération
					fields.AddField("fldf1uz2Kn6oiLMjO", queryResult.Value!.CommunityAid); //Aides Commune
					fields.AddField("fld0Sxly0dPlgD2tI", queryResult.Value!.CeeAid); //CEE
					fields.AddField(
						"fld4vt32PXh2ycraa",
						queryResult.Value!.UnderprivilegedHousingFoundation); //Fondation Abbé Pierre
					fields.AddField(
						"fld67pN3tbThooPc4",
						queryResult.Value!.SocialProtectionGroupAid); //Groupe de protection sociale
					fields.AddField("fldTrHuwxWJQpQjuE", queryResult.Value!.MdphAid); //MDPH
					fields.AddField("fldvmvJ6CohP9ATPl", askedStopFound); //Contribution sollicitée STOP EE
					fields.AddField(
						"fld0BxxKPINYt92lH",
						queryResult.Value!
							.HouseholdMaximumSavingsForRenovation); //Montant maximal des économies du foyer alloué à leur projet de rénovation
					fields.AddField(
						"fldEMuubtR1VKP6wS",
						queryResult.Value!
							.HouseholdMaximalFamilyAid); //Montant maximal du soutien des autres membres de la famille alloué au projet de rénovation
					fields.AddField(
						"fldQgFTzQY7LtVxF2",
						queryResult.Value!.TypeOfBankLoanRequested ??
						string.Empty); //Quels est le type de prêt bancaire sollicité ?
					fields.AddField(
						"fldZBFtbWU5kk09zz",
						queryResult.Value!.EnergyDepravation ??
						string.Empty); //Privation d'\''énergie (Totale, Aucune, Partielle)
					fields.AddField(
						"fldycfVBQdbfH9VoO",
						queryResult.Value!.FamilyMonthlyIncome); //Revenu mensuel (ce que perçoit le ménage pour vivre)
					fields.AddField(
						"fldwY6sSCmTYxieRz",
						string.Empty); //Description du système d'\''eau chaude sanitaire ....champs nons pris en charge dans Renée
					fields.AddField(
						"fldLCi2aNnevU5LX6",
						$"{(queryResult.Value!.IsInStopProgram.HasValue && queryResult.Value!.IsInStopProgram.Value ? "oui" : "non")}"); //TZEE ? (oui, non)
					fields.AddField(
						"fldM3PvuHQX57Mn6F",
						queryResult.Value!.WattForChangeAIds); //Watt Solidaire : montant *

					var response = await airtableBase.CreateRecord(tableId, fields, true);

					if (response.Success)
					{
						var httpClient = new HttpClient
						{
							BaseAddress = new Uri(returnUrl)
						};

						var payload = new { id = response.Record.Id };

						var content = JsonContent.Create(payload);
						content.Headers.ContentLength = payload.ToString()?.Length;
						content.Headers.Add("X-API-KEY", returnUrlBearerToken);
						var request = new HttpRequestMessage(HttpMethod.Post, "")
						{
							Content = content
						};
						var responseReplyUrl = await httpClient.SendAsync(request);

						if (responseReplyUrl.IsSuccessStatusCode)
						{
							var json = await responseReplyUrl.Content.ReadAsStringAsync();
							var document = JsonDocument.Parse(json);
							var root = document.RootElement;

							if (!string.IsNullOrEmpty(root.GetProperty("url").GetString()))
							{
								return ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>.Success(
									response,
									root.GetProperty("url").GetString()
								);
							}
						}
					}

					return ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>.Failure(
						Labels.Errors.AirtableError);
				}
			}

			return ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>.Failure(queryResult.Message!);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<AirtableCreateUpdateReplaceRecordResponse?>.Failure(
				Labels.Errors.AirtableError);
		}
	}
}