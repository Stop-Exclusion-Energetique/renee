using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Renee.Domain.Entity;
using Renee.Infrastructure.Data;
using Renee.McpServer.Extensions;
using Renee.McpServer.Models;

namespace Renee.McpServer.Tools;

[McpServerToolType]
public class AccompanyingFileGetTools(ReneeDbContext context, IHttpContextAccessor httpContextAccessor)
{
	private static readonly JsonSerializerOptions jsonSerializerOptions = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		WriteIndented = false
	};

	private IQueryable<AccompanyingFile> BuildFilteredQuery(
		string? fileReference,
		string? addressLabel,
		string? occupantLastName,
		string? occupantFirstName)
	{
		var query = context.AccompanyingFiles
			.AsQueryable()
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User);

		if (!string.IsNullOrWhiteSpace(fileReference))
			query = query.Where(af =>
				af.AccompanyingFileReference == fileReference ||
				af.AccompanyingFileReference.Contains(fileReference));

		if (!string.IsNullOrWhiteSpace(addressLabel))
			query = query.Where(af =>
				af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label != null &&
				af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label.Contains(addressLabel));

		if (!string.IsNullOrWhiteSpace(occupantLastName))
			query = query.Where(af =>
				af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName != null &&
				af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName.Contains(occupantLastName));

		if (!string.IsNullOrWhiteSpace(occupantFirstName))
			query = query.Where(af =>
				af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName != null &&
				af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName.Contains(occupantFirstName));

		return query;
	}

	/// <summary>
	/// Returns (earlyReturn, singleReference):
	/// - earlyReturn != null means return it directly (0 results or multiple results summary)
	/// - singleReference != null means exactly one match, proceed with full query
	/// </summary>
	private async Task<(CallToolResult? EarlyReturn, string? SingleReference)> ResolveSingleOrSummary(IQueryable<AccompanyingFile> baseQuery)
	{
		var matches = await baseQuery.Select(af => new
		{
			Reference = af.AccompanyingFileReference,
			Prenom = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName,
			Nom = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName,
			Adresse = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label
		}).ToListAsync();

		if (matches.Count == 0)
		{
			return (
				new CallToolResult
				{
					IsError = true,
					Content = [new TextContentBlock { Text = "Aucun dossier trouvé correspondant aux critères." }]
				},
				null
			);
		}

		if (matches.Count > 1)
		{
			var summary = new
			{
				Message = "Plusieurs dossiers correspondent à votre recherche. Demandez à l'utilisateur de choisir parmi les suivants :",
				Dossiers = matches.Select(m => new
				{
					m.Reference,
					Occupant = $"{m.Prenom} {m.Nom}".Trim(),
					m.Adresse
				})
			};

			return (
				new CallToolResult
				{
					StructuredContent = JsonSerializer.SerializeToElement(summary, jsonSerializerOptions)
				},
				null
			);
		}

		return (null, matches[0].Reference);
	}

	[McpServerTool]
	[Description("Recupere les donnees de menage d'un dossier d'accompagnement depuis la base de donnees a partir de la reference du dossier, du nom de l'occupant ou du libelle d'adresse. Au moins un des parametres ci-dessus doit etre fourni.")]
	[Authorize]
	public async Task<CallToolResult> GetAccompanyingFilesHouseholdDataAsJson(
		[Description("Reference du dossier optionnelle. Cela peut etre une portion de la reference du dossier.")] string? fileReference = null,
		[Description("Libelle d'adresse optionnel. Cela peut etre une portion du libelle d'adresse.")] string? addressLabel = null,
		[Description("Nom de l'occupant principal optionnel. Cela peut etre une portion du nom de l'occupant principal.")] string? occupantLastName = null,
		[Description("Prenom de l'occupant principal optionnel. Cela peut etre une portion du prenom de l'occupant principal.")] string? occupantFirstName = null
	)
	{
		if (string.IsNullOrWhiteSpace(fileReference)
			&& string.IsNullOrWhiteSpace(addressLabel)
			&& string.IsNullOrWhiteSpace(occupantLastName)
			&& string.IsNullOrWhiteSpace(occupantFirstName))
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Au moins un paramètre de recherche doit être fourni." }]
			};
		}

		var baseQuery = BuildFilteredQuery(fileReference, addressLabel, occupantLastName, occupantFirstName);
		var (earlyReturn, singleReference) = await ResolveSingleOrSummary(baseQuery);
		if (earlyReturn != null) return earlyReturn;

		var file = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.Where(af => af.AccompanyingFileReference == singleReference)
			.Select(af => new HouseholdDataDto
			{
				Reference = af.AccompanyingFileReference,
				Typology = af.AccompanyingFileHouseholdNavigation.HouseholdTypology,
				IsFollowedByASocialWorker = af.AccompanyingFileHouseholdNavigation.IsFollowedByAnSocialWorker,
				HasAnOccupantWithDisabilities = af.AccompanyingFileHouseholdNavigation.HasAnOccupantWithDisabilities,
				HasAnOccupantWithLongTermIllness = af.AccompanyingFileHouseholdNavigation.HasAnOccupantWithLongTermIllness,
				HasAnOccupantWithIndependenceLoss = af.AccompanyingFileHouseholdNavigation.HasAnOccupantWithIndependenceLoss,
				HasAnOccupantUnderCuratorship = af.AccompanyingFileHouseholdNavigation.HasAnOccupantUnderCuratorship,
				HasAnOccupantUnderGuardianship = af.AccompanyingFileHouseholdNavigation.HasAnOccupantUnderGuardianship,
				ReferenceIncomeTax = af.AccompanyingFileHouseholdNavigation.ReferenceIncomeTax,
				AnahCategory = af.AccompanyingFileHouseholdNavigation.AnahCategory,
				SocialContext = af.AccompanyingFileHouseholdNavigation.SocialContext,
				HouseholdProject = af.AccompanyingFileHouseholdNavigation.HouseholdProject,
				HouseholdAvailabilityForVisits = af.AccompanyingFileHouseholdNavigation.HouseholdAvailabilityForVisits,
				CommentsOnHouseholdDifficulties = af.AccompanyingFileHouseholdNavigation.CommentsOnHouseholdDifficulties,
				HasOverdueInvoice = af.AccompanyingFileHouseholdNavigation.HasOverdueInvoice,
				EnergyEffortRate = af.AccompanyingFileHouseholdNavigation.EnergyEffortRate,
				MainOccupant = new OccupantDto
				{
					FirstName = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.FirstName,
					LastName = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName,
					Trigram = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Trigram,
					Age = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Age,
					SocioProfessionalCategory = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.SocioProfessionalCategory,
					Job = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Job,
					PhoneNumber = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.PhoneNumber,
					Email = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Email,
					SocialProtectionFund = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.SocialProtectionFund,
					PensionFund = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.PensionFund,
					AdditionnalFund = af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.AdditionnalFund
				},
				Difficulties = af.AccompanyingFileHouseholdNavigation.HouseholdDifficulties
					.Select(hd => hd.DifficultyNavigation.Labels).ToList(),
				Expenses = af.AccompanyingFileHouseholdNavigation.HouseholdExpenses
					.Select(he => new LabeledValueDto { Label = he.Type.ToString(), Value = he.Value }).ToList(),
				Resources = af.AccompanyingFileHouseholdNavigation.HouseholdResources
					.Select(hr => new LabeledValueDto { Label = hr.HouseholdResourcesNavigation.Labels, Value = hr.Value }).ToList(),
				HeatingEnergies = af.AccompanyingFileHouseholdNavigation.HouseholdHeatingEnergies
					.Select(he => new LabeledValueDto { Label = he.HouseholdHeatingEnergyNavigation.Name, Value = he.Value }).ToList()
			})
			.FirstAsync();

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(file, jsonSerializerOptions)
		};
	}


	[McpServerTool]
	[Description("Recupere les donnees de logement d'un dossier d'accompagnement depuis la base de donnees a partir de la reference du dossier, du nom de l'occupant ou du libelle d'adresse. Au moins un des parametres ci-dessus doit etre fourni.")]
	[Authorize]
	public async Task<CallToolResult> GetAccompanyingFilesHousingDataAsJson(
		[Description("Reference du dossier optionnelle. Cela peut etre une portion de la reference du dossier.")] string? fileReference = null,
		[Description("Libelle d'adresse optionnel. Cela peut etre une portion du libelle d'adresse.")] string? addressLabel = null,
		[Description("Nom de l'occupant principal optionnel. Cela peut etre une portion du nom de l'occupant principal.")] string? occupantLastName = null,
		[Description("Prenom de l'occupant principal optionnel. Cela peut etre une portion du prenom de l'occupant principal.")] string? occupantFirstName = null
	)
	{
		if (string.IsNullOrWhiteSpace(fileReference)
			&& string.IsNullOrWhiteSpace(addressLabel)
			&& string.IsNullOrWhiteSpace(occupantLastName)
			&& string.IsNullOrWhiteSpace(occupantFirstName))
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Au moins un parametre de recherche doit etre fourni." }]
			};
		}

		var baseQuery = BuildFilteredQuery(fileReference, addressLabel, occupantLastName, occupantFirstName);
		var (earlyReturn, singleReference) = await ResolveSingleOrSummary(baseQuery);
		if (earlyReturn != null) return earlyReturn;

		var file = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.Where(af => af.AccompanyingFileReference == singleReference)
			.Select(af => new HousingDataDto
			{
				Reference = af.AccompanyingFileReference,
				Address = new AddressDto
				{
					Label = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Label,
					PostalCode = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode,
					City = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.City,
					Department = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Department,
					Region = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Region,
					HouseNumber = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.HouseNumber,
					Street = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.Street,
					AdditionnalComment = af.AccompanyingFileHousingNavigation.HousingAddressNavigation.AdditionnalComment
				},
				GeographicAreaTypology = af.AccompanyingFileHousingNavigation.GeographicAreaTypology,
				IsInAbfArea = af.AccompanyingFileHousingNavigation.IsInAbfarea,
				ArchitecturalOrTownPlanningStandards = af.AccompanyingFileHousingNavigation.ArchitecturalOrTownPlanningStandards,
				OwnershipStatus = af.AccompanyingFileHousingNavigation.OwnershipStatus,
				HousingType = af.AccompanyingFileHousingNavigation.HousingType,
				ConstructionYear = af.AccompanyingFileHousingNavigation.ConstructionYear,
				LivingSpace = af.AccompanyingFileHousingNavigation.LivingSpace,
				NumberOfRoom = af.AccompanyingFileHousingNavigation.NumberOfRoom,
				NumberOfFloor = af.AccompanyingFileHousingNavigation.NumberOfFloor,
				YearOfAcquisitionOrEntry = af.AccompanyingFileHousingNavigation.YearOfAcquisitionOrEntry,
				SunExposure = af.AccompanyingFileHousingNavigation.SunExposure,
				CeilingHeight = af.AccompanyingFileHousingNavigation.CeilingHeight,
				HasPreviousWork = af.AccompanyingFileHousingNavigation.HasPreviousWork,
				CommentOnPreviousWork = af.AccompanyingFileHousingNavigation.CommentOnPreviousWork,
				InitialState = new HousingInitialStateDto
				{
					DegradationIndex = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DegradationIndex,
					UnsanitaryCoefficient = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.UnsanitaryCoefficient,
					EnergyDepravation = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.EnergyDepravation,
					SummerThermalComfortLevel = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.SummerThermalComfortLevel,
					WinterThermalComfortLevel = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.WinterThermalComfortLevel,
					NoiseComfortLevel = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.NoiseComfortLevel,
					InitialStateDiagnosticCommentary = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.InitialStateDiagnosticCommentary,
					DisordersObservedCommentary = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DisordersObservedCommentary,
					Dpe = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe,
					Ges = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Ges,
					AnnualEnergyConsumption = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.AnnualEnergyConsumption,
					AnnualGesEmission = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.AnnualGesemission,
					HeatingEnergy = af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.HeatingEnergy
				},
				AfterWorkState = new HousingAfterWorkStateDto
				{
					EstimatedDpeClassJump = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeclassJump,
					EstimatedDpeAfterWork = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeafterWork,
					EstimatedAnnualEnergyConsumptionAfterWork = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedAnnualEnergyConsumptionAfterWork,
					EstimatedAnnualGesEmissionsAfterWork = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedAnnualGesemissionsAfterWork,
					EstimatedGesAfterWork = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedGesafterWork,
					FinalDpe = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpe,
					FinalDpeClassJump = af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpeClassJump
				}
			})
			.FirstAsync();

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(file, jsonSerializerOptions)
		};
	}

	[McpServerTool]
	[Description("Recupere les donnees de financement d'un dossier d'accompagnement depuis la base de donnees a partir de la reference du dossier, du nom de l'occupant ou du libelle d'adresse. Au moins un des parametres ci-dessus doit etre fourni.")]
	[Authorize]
	public async Task<CallToolResult> GetAccompanyingFilesFinancingPlanDataAsJson(
		[Description("Reference du dossier optionnelle. Cela peut etre une portion de la reference du dossier.")] string? fileReference = null,
		[Description("Libelle d'adresse optionnel. Cela peut etre une portion du libelle d'adresse.")] string? addressLabel = null,
		[Description("Nom de l'occupant principal optionnel. Cela peut etre une portion du nom de l'occupant principal.")] string? occupantLastName = null,
		[Description("Prenom de l'occupant principal optionnel. Cela peut etre une portion du prenom de l'occupant principal.")] string? occupantFirstName = null
	)
	{
		if (string.IsNullOrWhiteSpace(fileReference)
			&& string.IsNullOrWhiteSpace(addressLabel)
			&& string.IsNullOrWhiteSpace(occupantLastName)
			&& string.IsNullOrWhiteSpace(occupantFirstName))
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Au moins un parametre de recherche doit etre fourni." }]
			};
		}

		var baseQuery = BuildFilteredQuery(fileReference, addressLabel, occupantLastName, occupantFirstName);
		var (earlyReturn, singleReference) = await ResolveSingleOrSummary(baseQuery);
		if (earlyReturn != null) return earlyReturn;

		var file = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.Where(af => af.AccompanyingFileReference == singleReference)
			.Select(af => new FinancingPlanDataDto
			{
				Reference = af.AccompanyingFileReference,
				MaPrimeRenovGuidedPath = af.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovGuidedPath,
				MaPrimeRenovCoOwnerShip = af.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovCoOwnerShip,
				MaPrimeLogementDecent = af.AccompanyingFilePreFinancingPlanNavigation.MaPrimeLogementDecent,
				MaPrimeAdapt = af.AccompanyingFilePreFinancingPlanNavigation.MaPrimeAdapt,
				BonusForExitingEnergeticSieve = af.AccompanyingFilePreFinancingPlanNavigation.BonusForExitingEnergeticSieve,
				RegionalAids = af.AccompanyingFilePreFinancingPlanNavigation.RegionalAids,
				DepartmentalAids = af.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids,
				PublicEstablishmentsForInterCommunalCooperationAids = af.AccompanyingFilePreFinancingPlanNavigation.PublicEstablishmentsForInterCommunalCooperationAids,
				MunicipalityAids = af.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids,
				SolicitedBankLoanType = af.AccompanyingFilePreFinancingPlanNavigation.SolicitedBankLoanType,
				ClassicBankLoan = af.AccompanyingFilePreFinancingPlanNavigation.ClassicBankLoan,
				RemainingAmountFinancingSource = af.AccompanyingFilePreFinancingPlanNavigation.RemainingAmountFinancingSource,
				IsFinancingAskedToStopAssociation = af.AccompanyingFilePreFinancingPlanNavigation.IsFinancingAskedToStopAssociation,
				EstimatedRemainingAmount = af.AccompanyingFilePreFinancingPlanNavigation.EstimatedRemainingAmount,
				MdphFinancing = af.AccompanyingFilePreFinancingPlanNavigation.MdphFinancing,
				CeeFinancing = af.AccompanyingFilePreFinancingPlanNavigation.CeeFinancing,
				CafMsaFinancing = af.AccompanyingFilePreFinancingPlanNavigation.CafMsaFinancing,
				PensionFund = af.AccompanyingFilePreFinancingPlanNavigation.PensionFund,
				UnderprivilegedHousingFoundation = af.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation,
				LeroyMerlinFoundation = af.AccompanyingFilePreFinancingPlanNavigation.LeroyMerlinFoundation,
				WattForChangeFoundation = af.AccompanyingFilePreFinancingPlanNavigation.WattForChangeFoundation,
				SocialProtectionGroup = af.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup,
				StopEnergyExclusionFunds = af.AccompanyingFilePreFinancingPlanNavigation.StopEnergyExclusionFunds,
				HouseholdMaximumSavingAmountForRenovationProject = af.AccompanyingFilePreFinancingPlanNavigation.HouseholdMaximumSavingAmountForRenovationProject,
				OtherFamilyMemberMaximumSupportAmountForRenovationProject = af.AccompanyingFilePreFinancingPlanNavigation.OtherFamilyMemberMaximumSupportAmountForRenovationProject,
				FundingModes = af.AccompanyingFilePreFinancingPlanNavigation.FundingModes
					.Select(fm => new LabeledValueDto { Label = fm.Label, Value = fm.Value }).ToList()
			})
			.FirstAsync();

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(file, jsonSerializerOptions)
		};
	}

	[McpServerTool]
	[Description("Recuperation des donnees du plan de travaux. Cet outil extrait toutes les informations liees au plan de travaux d'un dossier de renovation energetique. Entrees possibles : reference du dossier, nom du beneficiaire, adresse du logement (au moins une doit etre fournie). Sortie : format de donnees structure contenant les details du projet tels que le type de renovation, les travaux urgents, les travaux recommandes, les montants estimes et la performance energetique attendue apres travaux.\r\n- Usage : facilite l'acces et l'analyse des donnees travaux pour le suivi et la planification des renovations.")]
	[Authorize]
	public async Task<CallToolResult> GetAccompanyingFilesWorkPlanDataAsJson(
		[Description("Reference du dossier optionnelle. Cela peut etre une portion de la reference du dossier.")] string? fileReference = null,
		[Description("Libelle d'adresse optionnel. Cela peut etre une portion du libelle d'adresse.")] string? addressLabel = null,
		[Description("Nom de l'occupant principal optionnel. Cela peut etre une portion du nom de l'occupant principal.")] string? occupantLastName = null,
		[Description("Prenom de l'occupant principal optionnel. Cela peut etre une portion du prenom de l'occupant principal.")] string? occupantFirstName = null
	)
	{
		if (string.IsNullOrWhiteSpace(fileReference)
			&& string.IsNullOrWhiteSpace(addressLabel)
			&& string.IsNullOrWhiteSpace(occupantLastName)
			&& string.IsNullOrWhiteSpace(occupantFirstName))
		{
			return new CallToolResult
			{
				IsError = true,
				Content = [new TextContentBlock { Text = "Au moins un parametre de recherche doit etre fourni." }]
			};
		}

		var baseQuery = BuildFilteredQuery(fileReference, addressLabel, occupantLastName, occupantFirstName);
		var (earlyReturn, singleReference) = await ResolveSingleOrSummary(baseQuery);
		if (earlyReturn != null) return earlyReturn;

		var file = await context.AccompanyingFiles
			.ApplyUserAccessFilter(httpContextAccessor.HttpContext!.User)
			.Where(af => af.AccompanyingFileReference == singleReference)
			.Select(af => new WorkPlanDataDto
			{
				Reference = af.AccompanyingFileReference,
				RenovationType = af.AccompanyingFilePreWorkPlanNavigation.RenovationType,
				NextStepAndVigilancePoint = af.AccompanyingFilePreWorkPlanNavigation.NextStepAndVigilancePoint,
				HasInterestInPossibleAraProcess = af.AccompanyingFilePreWorkPlanNavigation.HasInterestInPossibleAraprocess,
				HasNeedForTemporaryReHousing = af.AccompanyingFilePreWorkPlanNavigation.HasNeedForTemporaryReHousing,
				HasEmergencyWorks = af.AccompanyingFilePreWorkPlanNavigation.HasEmergencyWorks,
				HasEnergeticsRenovationWorks = af.AccompanyingFilePreWorkPlanNavigation.HasEnergeticsRenovationWorks,
				HasInducedWorks = af.AccompanyingFilePreWorkPlanNavigation.HasInducedWorks,
				HasSafetyAndHealthWorks = af.AccompanyingFilePreWorkPlanNavigation.HasSafetyAndHealthWorks,
				TreatedAirTightness = af.AccompanyingFilePreWorkPlanNavigation.TreatedAirTightness,
				TreatedThermalBridge = af.AccompanyingFilePreWorkPlanNavigation.TreatedThermalBridge,
				AreExistingHumidityAndVaporMigrationManagedAfterTreatment = af.AccompanyingFilePreWorkPlanNavigation.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
				WorksDetails = af.AccompanyingFilePreWorkPlanNavigation.WorksDetails,
				IsRgeLabelUpToDate = af.AccompanyingFilePreWorkPlanNavigation.IsRgeLabelUpToDate,
				OtherQualification = af.AccompanyingFilePreWorkPlanNavigation.OtherQualification,
				InsuranceTypes = af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanInsuranceTypes
					.Select(it => it.InsuranceTypeNavigation.Label).ToList(),
				ProjectTypes = af.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes
					.Select(pt => pt.ProjectTypeNavigation.Label).ToList(),
				WorkPackages = af.AccompanyingFilePreWorkPlanNavigation.WorkPackages
					.Select(wp => new WorkPackageDto
					{
						EnergeticsEffectAfterWorks = wp.EnergeticsEffectAfterWorks,
						Costs = wp.WorkPackageWorkTypeCosts
							.Select(c => new WorkTypeCostDto
							{
								WorkType = c.WorkTypeNavigation.Label,
								Cost = c.Cost,
								Description = c.Description
							}).ToList()
					}).ToList()
			})
			.FirstAsync();

		return new CallToolResult
		{
			StructuredContent = JsonSerializer.SerializeToElement(file, jsonSerializerOptions)
		};
	}
}
