using ClosedXML.Attributes;

namespace Renee.Application.DTOs.ExcelExport;

public class AccompanyingFileExcelExportDto
{
	//General AccompanyingFile Field
	[XLColumn(Header = "Référence du dossier")]
	public string AccompanyingFileReference { get; set; } = null!;
	[XLColumn(Header = "Jalon")]
	public string? AccompanyingFileMilestone { get; set; }
	[XLColumn(Header = "Statut du dossier")]
	public string? AccompanyingFileStatus { get; set; }
	[XLColumn(Header = "Date de première viste")]
	public DateTime? FirstEncounterDate { get; set; }
	[XLColumn(Header = "Date de début d'accompagnement")]
	public DateTime? StartOfAccompanyingDate { get; set; }
	[XLColumn(Header = "Nombre de contact avec la famille (en présentiel ou à distance) au jalon 1")]
	public int? NumberOfEncounterWithFamillyForIdentificationMilestone { get; set; }
	[XLColumn(Header = "Date d'ouverture du dossier")]
	public DateTime? OpeningDate { get; set; }
	[XLColumn(Header = "Date de fermeture du dossier")]
	public DateTime? CloseDate { get; set; }
	[XLColumn(Header = "Nombre de contact avec la famille (en présentiel ou à distance) au jalon 2")]
	public int? NumberOfEncounterWithFamillyForOrganizeAndFinanceMilestone { get; set; }
	[XLColumn(Header = "Date de fin d'accompagnement")]
	public DateTime? EndOfAccompanyingDate { get; set; }
	[XLColumn(Header = "Date de fin de suivi")]
	public DateTime? EndOfEncounterDate { get; set; }
	[XLColumn(Header = "Nombre de contact avec la famille (en présentiel ou à distance) au jalon 3")]
	public int? NumberOfEncounterWithFamillyForRealizeAndFollowMilestone { get; set; }
	[XLColumn(Header = "Programme Territoire Zéro Exclusion Energétique")]
	public string? ZeroEnergyExclusionTerritoriesProgram { get; set; }
	[XLColumn(Header = "Type d'accompagnement")]
	public string? AccompanyingType { get; set; }
	[XLColumn(Header = "Territoire")]
	public string? AccompanyingFileTerritory { get; set; }
	[XLColumn(Header = "Délai de prise en charge")]
	public int? DeliveryTime { get; set; }
	[XLColumn(Header = "Date de validation du jalon 1")]
	public DateTime? IdentifySynthesisValidationDate { get; set; }
	[XLColumn(Header = "Date de validation du jalon 2")]
	public DateTime? OrganizeAndFinanceSynthesisValidationDate { get; set; }
	[XLColumn(Header = "Date de validation du jalon 3")]
	public DateTime? RealizeAndFollowSynthesisValidationDate { get; set; }
	[XLColumn(Header = "Structure de rattachement")]
	public string? ReportingStructure { get; set; }
	[XLColumn(Header = "Ensemblier Solidaire principal")]
	public string SolidarBuilder { get; set; } = string.Empty;
	[XLColumn(Header = "Second Ensemblier Solidaire")]
	public string? SecondSolidarBuilder { get; set; }
	[XLColumn(Header = "Troisième Ensemblier Solidaire")]
	public string? ThirdSolidarBuilder { get; set; }
	[XLColumn(Header = "Ensemblier Territorial principal")]
	public string? TerritorialBuilder { get; set; }
	[XLColumn(Header = "Second Ensemblier Territorial")]
	public string? SecondTerritorialBuilder { get; set; }
	[XLColumn(Header = "Coordinatrice associée")]
	public string? Coordinator { get; set; }
	[XLColumn(Header = "Dossier à déposer en priorité dans l'ANAH")]
	public string? ShouldAccompanyingFileBeSubmittedToAnah {  get; set; }
	[XLColumn(Header = "Numéro de dossier ANAH")]
	public string? AnahFolderNumber { get; set; }
	[XLColumn(Header = "Date de dépôt du dossier ANAH")]
	public DateTime? AnahFolderFilingDate { get; set; }
	[XLColumn(Header = "Temps d'accompagnement pour les ménages J1")]
	public string? AccompanyingTimeDurationForIdentificationMilestone { get; set; }
	[XLColumn(Header = "Temps d'accompagnement pour les ménages J2")]
	public string? AccompanyingTimeDurationForOrganizeAndFinanceMilestone { get; set; }
	[XLColumn(Header = "Temps d'accompagnement pour les ménages J3")]
	public string? AccompanyingTimeDurationForRealizeAndFollowMilestone { get; set; }
	[XLColumn(Header = "Raison d'abandon")]
	public string? AbortReasonLabel { get; set; }
	[XLColumn(Header = "Type d'abandon")]
	public string? AbortTypeLabel { get; set; }
	[XLColumn(Header = "Précisions sur la demande d'abandon")]
	public string? AbortRequestDetails { get; set; }
	[XLColumn(Header = "Présence de pièce jointe")]
	public string? HasAbortAttachment { get; set; }
	[XLColumn(Header = "Demande de facturation")]
	public string? IsBillingRequested { get; set; }
	[XLColumn(Header = "Nom du validateur")]
	public string? ValidatorName { get; set; }
	[XLColumn(Header = "Commentaire du validateur")]
	public string? ValidatorAbortComment { get; set; }
	[XLColumn(Header = "Rôle du validateur")]
	public string? ValidatorRole { get; set; }
	[XLColumn(Header = "Date de décision")]
	public DateTime? DecisionDate { get; set; }
	[XLColumn(Header = "Date d'octroi ANAH")]
	public DateTime? AnahGrantDate { get; set; }

	//Household fields
	[XLColumn(Header = "Typologie de famille")]
	public string? HouseholdTypology { get; set; }
	[XLColumn(Header = "Le ménage est-il suivi par un travailleur social")]
	public string? IsFollowedByAnSocialWorker { get; set; }
	[XLColumn(Header = "Situation de handicap dans le foyer")]
	public string? HasAnOccupantWithDisabilities { get; set; }
	[XLColumn(Header = "Personne en maladie longue durée")]
	public string? HasAnOccupantWithLongTermIllness { get; set; }
	[XLColumn(Header = "Personne en perte d'autonomie dans le foyer")]
	public string? HasAnOccupantWithIndependenceLoss { get; set; }
	[XLColumn(Header = "Personne placée sous curatelle / sauvegarde de justice ")]
	public string? HasAnOccupantUnderCuratorship { get; set; }
	[XLColumn(Header = "Personne placée sous tutelle")]
	public string? HasAnOccupantUnderGardianship { get; set; }
	[XLColumn(Header = "Revenu fiscal annuel de référence du ménage (€)")]
	public double? ReferenceIncomeTax { get; set; }
	[XLColumn(Header = "Catégorie selon l'Anah")]
	public string? AnahCategory { get; set; }
	[XLColumn(Header = "Description du context social")]
	public string? SocialContext { get; set; }
	[XLColumn(Header = "Projet de la famille")]
	public string? HouseholdProject { get; set; }
	[XLColumn(Header = "Disponibilités de la famille pour les visites")]
	public string? HouseholdAvailabilityForVisits { get; set; }
	[XLColumn(Header = "Détails des difficultées rencontrées par le ménage")]
	public string? CommentsOnHouseholdDifficulties { get; set; }
	[XLColumn(Header = "Impayés de factures d'énergie depuis au moins 6 mois")]
	public string? HasOverdueInvoice { get; set; }
	[XLColumn(Header = "Taux d'effort énergétique")]
	public double? EnergyEffortRate { get; set; }
	[XLColumn(Header = "Difficultés rencontrées par la famille")]
	public string? HouseholdDifficulties {get; set;}
	[XLColumn(Header = "Dépenses du ménage")]
	public string? HouseholdExpenses {get; set;}
	[XLColumn(Header = "Typologie de ressources mensuelles du ménage")]
	public string? HouseholdResources {get; set;}

	// Main Occupant Fields
	[XLColumn(Header = "Trigramme")]
	public string Trigram { get; set; } = null!;
	[XLColumn(Header = "Date de naissance")]
	public DateTime? Birthdate { get; set; }
	[XLColumn(Header = "Age")]
	public int? Age { get; set; }
	[XLColumn(Header = "Catégorie Socioprofessionnel")]
	public string? SocioProfessionalCategory { get; set; }
	[XLColumn(Header = "Numéro de téléphone")]
	public string? PhoneNumber { get; set; }
	[XLColumn(Header = "Email")]
	public string? Email { get; set; }
	[XLColumn(Header = "Profession")]
	public string? Job { get; set; }
	[XLColumn(Header = "Caisse de protection social")]
	public string? SocialProtectionFund { get; set; }
	[XLColumn(Header = "Commentaire sur la caisse de protection social")]
	public string? CommentOnSocialProtectionFund { get; set; }
	[XLColumn(Header = "Fond de retraite")]
	public string? PensionFundName { get; set; }
	[XLColumn(Header = "Commentaire sur le fond de retraite")]
	public string? CommentOnPensionFund { get; set; }
	[XLColumn(Header = "Fond complémentaire")]
	public string? AdditionnalFund { get; set; }
	[XLColumn(Header = "Commentaire sur le fond complémentaire")]
	public string? CommentOnAdditionnalFund { get; set; }
	[XLColumn(Header = "Prénom")]
	public string? FirstName { get; set; }
	[XLColumn(Header = "Nom")]
	public string? LastName { get; set; }

	// Housing fields
	[XLColumn(Header = "Typologie de territoire")]
	public string? GeographicAreaTypology { get; set; }
	[XLColumn(Header = "Zone ABF")]
	public string? IsInAbfarea { get; set; }
	[XLColumn(Header = "Normes architecturales ou d'urbanisme")]
	public string? ArchitecturalOrTownPlanningStandards { get; set; }
	[XLColumn(Header = "Statut de propriété")]
	public string? OwnershipStatus { get; set; }
	[XLColumn(Header = "Type de logement")]
	public string? HousingType { get; set; }
	[XLColumn(Header = "Année de construction du logement")]
	public string? ConstructionYear { get; set; }
	[XLColumn(Header = "Surface habitable")]
	public double? LivingSpace { get; set; }
	[XLColumn(Header = "Nombre de pièces du logement")]
	public int? NumberOfRoom { get; set; }
	[XLColumn(Header = "Nombre de niveau")]
	public int? NumberOfFloor { get; set; }
	[XLColumn(Header = "Année d'acquisition/Entrée dans le logement")]
	public int? YearOfAcquisitionOrEntry { get; set; }
	[XLColumn(Header = "Référence cadastral")]
	public string? CadastralReference { get; set; }
	[XLColumn(Header = "Exposition au soleil")]
	public string? SunExposure { get; set; }
	[XLColumn(Header = "Nombre de portes")]
	public int? NumberOfDoor { get; set; }
	[XLColumn(Header = "Nombre de fenêtres")]
	public int? NumberOfWindow { get; set; }
	[XLColumn(Header = "Nombre de portes-fenêtres")]
	public int? NumberOfPatioDoor { get; set; }
	[XLColumn(Header = "Nombre de fenêtres de toits")]
	public int? NumberOfRoofDoor { get; set; }
	[XLColumn(Header = "Nombre de baies vitrées")]
	public int? NumberOfBayWindow { get; set; }
	[XLColumn(Header = "Hauteur sous plafond (m)")]
	public double? CeilingHeight { get; set; }
	[XLColumn(Header = "Travaux antérieurs")]
	public string? HasPreviousWork { get; set; }
	[XLColumn(Header = "Commentaire sur les travaux antérieurs")]
	public string? CommentOnPreviousWork { get; set; }

	//Adress Fields
	[XLColumn(Header = "Libellé de l'adresse")]
	public string Label { get; set; } = null!;
	[XLColumn(Header = "Code Postal")]
	public string PostalCode { get; set; } = null!;
	[XLColumn(Header = "Ville")]
	public string City { get; set; } = null!;
	[XLColumn(Header = "Département")]
	public string Department { get; set; } = null!;
	[XLColumn(Header = "Région")]
	public string Region { get; set; } = null!;
	[XLColumn(Header = "Complément d'adresse")]
	public string? AdditionalComment { get; set; }

	//Housing Initial State
	[XLColumn(Header = "Indice de dégradation")]
	public string? DegradationIndex { get; set; }
	[XLColumn(Header = "Coefficient d'insalubrité")]
	public string? UnsanitaryCoefficient { get; set; }
	[XLColumn(Header = "Privation d'énergie")]
	public string? EnergyDepravation { get; set; }
	[XLColumn(Header = "Niveau du confort thermique en été (manque de ventilation, refroidissement du logement difficile, …)")]
	public string? SummerThermalComfortLevel { get; set; }
	[XLColumn(Header = "Niveau du confort thermique en hiver (montée en température difficile, courants d'air, murs froids,…)")]
	public string? WinterThermalComfortLevel { get; set; }
	[XLColumn(Header = "Niveau du confort sonore")]
	public string? NoiseComfortLevel { get; set; }
	[XLColumn(Header = "Nuisibles/Moisissures")]
	public string? HasPestOrMold { get; set; }
	[XLColumn(Header = "Système électrique défaillant")]
	public string? HasFaultyElectricalSystem { get; set; }
	[XLColumn(Header = "Système de ventilation")]
	public string? HasVentilationSystem { get; set; }
	[XLColumn(Header = "Système de chauffage")]
	public string? HasHeatingSystem { get; set; }
	[XLColumn(Header = "Production d'eau chaude")]
	public string? HasHotWaterProduction { get; set; }
	[XLColumn(Header = "Toitures")]
	public string? HasHousingCover { get; set; }
	[XLColumn(Header = "Ouvertures")]
	public string? HasOpenings { get; set; }
	[XLColumn(Header = "Couvertures")]
	public string? HasInsulation { get; set; }
	[XLColumn(Header = "Commentaire sur les problèmes rencontrés avant travaux")]
	public string? DisordersObservedCommentary { get; set; }
	[XLColumn(Header = "Etiquette DPE avant travaux")]
	public string? Dpe { get; set; }
	[XLColumn(Header = "Etiquette GES avant travaux")]
	public string? Ges { get; set; }
	[XLColumn(Header = "Consommation énergétique annuelle avant travaux (kWhEP/m²)")]
	public double? AnnualEnergyConsumption { get; set; }
	[XLColumn(Header = "Émissions GES annuelles avant travaux (kgCO²e)")]
	public double? AnnualGesemission { get; set; }
	[XLColumn(Header = "Energie de chauffage avant travaux")]
	public string? HeatingEnergy { get; set; }

	//Housing After work state
	[XLColumn(Header = "Saut de classe énergétique estimé")]
	public string? EstimatedDpeclassJump { get; set; }
	[XLColumn(Header = "Étiquette énergie après travaux estimée")]
	public string? EstimatedDpeafterWork { get; set; }
	[XLColumn(Header = "Consommation énergétique annuelle après travaux (kWhEP/m²)")]
	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; set; }
	[XLColumn(Header = "Emission GES annuelles estimées après travaux (TonnesEqCO²)")]
	public double? EstimatedAnnualGesemissionsAfterWork { get; set; }
	[XLColumn(Header = "Étiquette climat après travaux estimée")]
	public string? EstimatedGesafterWork { get; set; }
	[XLColumn(Header = "Étiquette DPE finale")]
	public string? FinalDpe { get; set; }
	[XLColumn(Header = "Saut de classe énergétique final")]
	public string? FinalDpeClassJump { get; set; }

	//Pre Work Plan
	[XLColumn(Header = "Type de rénovation")]
	public string? RenovationType { get; set; }
	[XLColumn(Header = "Prochaine(s) étape(s) et points de vigilance")]
	public string? NextStepAndVigilancePoint { get; set; }
	[XLColumn(Header = "Intérêt pour la démarche auto-réhabilitation accompagnée (ARA) éventuelle")]
	public string? HasInterestInPossibleAraprocess { get; set; }
	[XLColumn(Header = "Besoin d'une solution de relogement temporaire")]
	public string? HasNeedForTemporaryReHousing { get; set; }
	[XLColumn(Header = "Travaux d'urgences (pré plan de travaux)")]
	public string? HasEmergencyWorksPreWorkPlan { get; set; }
	[XLColumn(Header = "Travaux de rénovation énergétique")]
	public string? HasEnergeticsRenovationWorks { get; set; }
	[XLColumn(Header = "Travaux induits")]
	public string? HasInducedWorks { get; set; }
	[XLColumn(Header = "Travaux de sécurité et salubrité")]
	public string? HasSafetyAndHealthWorks { get; set; } 
	[XLColumn(Header = "Étanchéité à l'air traitée")]
	public string? TreatedAirTightnessPreworkPlan { get; set; }
	[XLColumn(Header = "Ponts thermiques traités (pré plan de travaux)")]
	public string? TreatedThermalBridge { get; set; }
	[XLColumn(Header = "Gestion de l’humidité existante et de la migration de vapeur après isolation traitée (pré plan de travaux)")]
	public string? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; set; }
	[XLColumn(Header = "La famille est prête à s'engager dans la démarche ARA")]
	public string? IsHouseholdReadyToStartAraprocess { get; set; }
	[XLColumn(Header = "Prise en compte des capacités physiques de la famille")]
	public string? AreHouseholdPhysicalCapacitiesTakenIntoAccount { get; set; }
	[XLColumn(Header = "La famille peut mobiliser son entourage sur le chantier")]
	public string? DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean { get; set; }
	[XLColumn(Header = "Précisions sur les travaux")]
	public string? WorksDetails { get; set; }
	[XLColumn(Header = "Disponibilités de la famille pour organiser le chantier ARA")]
	public string? HouseholdAvailabilitiyToOrganizeArasite { get; set; }
	[XLColumn(Header = "Label RGE à jour")]
	public string? IsRgeLabelUpToDate { get; set; }
	[XLColumn(Header = "Autres qualifications")]
	public string? OtherQualification { get; set; }
	[XLColumn(Header = "Type de projet")]
	public string? ProjectType {get; set;}
	[XLColumn(Header = "Type d'assurance")]
	public string? InsuranceType {get; set;}
	
	// Pre financing plan 
	[XLColumn(Header = "MaPrimeRénov' Parcours Accompagné (€)")]
	public double? MaPrimeRenoveGuidedPath { get; set; }
	[XLColumn(Header = "MaPrimeRénov' Copropriété (€)")]
	public double? MaPrimeRenoveCoOwnerShip { get; set; }
	[XLColumn(Header = "Ma Prime Logement Décent (€)")]
	public double? MaPrimeLogementDecent { get; set; }
	[XLColumn(Header = "MaPrimeAdapt' (€)")]
	public double? MaPrimeAdapt { get; set; }
	[XLColumn(Header = "Bonus sortie de passoire thermique (€)")]
	public double? BonusForExitingEnergeticSieve { get; set; }
	[XLColumn(Header = "Région (€)")]
	public double? RegionalAids { get; set; }
	[XLColumn(Header = "Département (€)")]
	public double? DepartmentalAids { get; set; }
	[XLColumn(Header = "Etablissements publics de coopération intercommunale/Agglomération (€)")]
	public double? PublicEstablishmentsForInterCommunalCooperationAids { get; set; }
	[XLColumn(Header = "Commune (€)")]
	public double? MunicipalityAids { get; set; }
	[XLColumn(Header = "Quel est le type de prêt bancaire sollicité ?")]
	public string? SolicitedBankLoanType { get; set; }
	[XLColumn(Header = "Prêt bancaire classique (€)")]
	public double? ClassicBankLoan { get; set; }
	[XLColumn(Header = "MDPH (€)")]
	public double? MdphFinancing { get; set; }
	[XLColumn(Header = "CEE (€)")]
	public double? CeeFinancing { get; set; }
	[XLColumn(Header = "CAF/MSA")]
	public double? CafMsaFinancing { get; set; }
	[XLColumn(Header = "Caisse de retraite")]
	public double? PensionFund { get; set; }
	[XLColumn(Header = "Fondation Abbé Pierre (€)")]
	public double? AbePierreFundation { get; set; }
	[XLColumn(Header = "Fondation Leroy Merlin (€)")]
	public double? LeroyMerlinFundation { get; set; }
	[XLColumn(Header = "Fondation Watt For Change (€)")]
	public double? WattForChangeFundation { get; set; }
	[XLColumn(Header = "Groupe de protection sociale (€)")]
	public double? SocialProtectionGroup { get; set; }
	[XLColumn(Header = "Montant maximal des économies du foyer alloué à leur projet de rénovation (€)")]
	public double? HouseholdMaximumSavingAmountForRenovationProject { get; set; }
	[XLColumn(Header = "Montant maximal du soutien des autres membres de la famille alloué au projet de rénovation (€)")]
	public double? OtherFamilyMemberMaximumSupportAmountForRenovationProject { get; set; }
	[XLColumn(Header="Mode de financement")]
	public string? FundingMode {get; set;}

	//Work Monitoring
	[XLColumn(Header = "Coût de l'accompagnement (HT)")]
	public double? AccompanyingCost { get; set; }
	[XLColumn(Header = "Autofinancement du ménage")]
	public double? HouseholdSelfFinancing { get; set; }
	[XLColumn(Header = "Résultat du test intermédiaire d’étanchéité à l’air si rénovation globale")]
	public string? IntermediateAirtightnessTestResult { get; set; }
	[XLColumn(Header = "Si pas de test, justification et actions mises en place pour le traitement de l’étanchéité à terme")]
	public string? JustificationAndActionsPutInPlaceIfNoTest { get; set; }
	[XLColumn(Header = "Respect effectif des préconisations de travaux (comparaison devis / factures)")]
	public string? HasEffectiveComplianceWithWorkRecommendations { get; set; }
	[XLColumn(Header = "Changement d’étiquette énergétique prévisionnel finale")]
	public string? ShouldChangeFinalEstimatedDpe { get; set; }
	[XLColumn(Header = "Travaux ayant permis le maintien à domicile")]
	public string? HasWorksEnabledHouseholdToStayAtHome { get; set; }
	[XLColumn(Header = "Notation relative au bien-être")]
	public int? WellBeingRating { get; set; }
	[XLColumn(Header = "Notation relative au cadre éducatif")]
	public int? EducationnalFrameworkRating { get; set; }
	[XLColumn(Header = "Satisfaction de la famille concernant l'accompagnement")]
	public int? FamilySatisfaction { get; set; }
	[XLColumn(Header = "Retour à l'emploi (si recherche d'emploi au début de l'accompagnement)")]
	public string? ReturnToEmployment { get; set; }
	[XLColumn(Header = "Travaux d'adaptation du logement")]
	public string? HasHousingAdaptationWorks { get; set; }
	[XLColumn(Header = "Travaux de finition")]
	public string? HasFinishingWorks { get; set; }
	[XLColumn(Header = "Travaux de sécurisation")]
	public string? HasSafetyWorks { get; set; }
	[XLColumn(Header = "Travaux de préparation")]
	public string? HasPreparationWorks { get; set; }
	[XLColumn(Header = "Travaux d'urgence")]
	public string? HasEmergencyWorks { get; set; }
	[XLColumn(Header = "Sortie d'insalubrité")]
	public string? HasUnsanitaryExit { get; set; }
	[XLColumn(Header = "Étanchéité à l’air traitée")]
	public string? TreatedAirTightness { get; set; }
	[XLColumn(Header = "Ponts thermiques traités")]
	public string? TreatedThermalBridges { get; set; }
	[XLColumn(Header = "Gestion de l’humidité existante et de la migration de vapeur après isolation traitée")]
	public string? HasHumidityManagement { get; set; }
	[XLColumn(Header = "Coût total des travaux (€) TTC")]
	public double? WorkTotalCost { get; set; }

	// SiteSupervision
	[XLColumn(Header = "Date de la réunion de pré-chantier")]
	public DateTime? PreSiteSupervisionMeetingDate { get; set; }
	[XLColumn(Header = "Date du lancement de chantier")]
	public DateTime? OverallStartDate { get; set; }
	[XLColumn(Header = "Date de signature du procès verbal de réception")]
	public DateTime? ActualOverallEndDate { get; set; }
}

public class AccompanyingFileWorkPackageExportDto
{
	[XLColumn(Header = "Travaux prévus, coûts et descriptions")]
	public List<string> WorkpackageDisplay { get; set; } = new();
	[XLColumn(Header = "Montant total du devis (€) TTC")]
	public string? EnergeticsEffectAfterWorks { get; set; }
}

public class AccompanyingFileForBillingAndAdministrationExcelExportDto
{
	//Equipe d'accompagnement
	[XLColumn(Header = "Référence du dossier")]
	public string? AccompanyingFileReference { get; set; }
	[XLColumn(Header = "Référence externe du dossier")]
	public string? AccompanyingFileExternalReference { get; set; }
	[XLColumn(Header = "Nom opérateur/structure de rattachement")]
	public string? ReportingStructure { get; set; }
	[XLColumn(Header = "ES principal")]
	public string? FirstSolidarBuilderName { get; set; }
	[XLColumn(Header = "ES secondaire")]
	public string? SecondSolidarBuilderName { get; set; }
	[XLColumn(Header = "ES tertiaire")]
	public string? ThirdSolidarBuilderName { get; set; }
	[XLColumn(Header = "ET principal")]
	public string? FirstTerritorialBuilderName { get; set; }
	[XLColumn(Header = "ET secondaire")]
	public string? SecondTerritorialBuilderName { get; set; }
	[XLColumn(Header = "Coordinatrice diffus")]
	public string? DiffuseCoordinatorName { get; set; }
	[XLColumn(Header = "Coordinatrice ciblé")]
	public string? TargetCoordinatorName { get; set; }

	//Type d'accompagnement et territoire
	[XLColumn(Header = "Type d'accompagnement")]
	public string? AccompanyingType { get; set; }
	[XLColumn(Header = "Typologie de la zone géographique")]
	public string? GeographicAreaTypology { get; set; }
	[XLColumn(Header = "Territoire")]
	public string? AccompanyingFileTerritory { get; set; }

	//AccompanyingFile stage and status
	[XLColumn(Header = "Jalon")]
	public string? AccompanyingFileMilestone { get; set; }
	[XLColumn(Header = "Statut du dossier")]
	public string? AccompanyingFileStatus { get; set; }

	//First Stage Billing Information
	[XLColumn(Header = "J1 facturé")]
	public string? IsFirstStageBilled { get; set; }
	[XLColumn(Header = "Numéros de facture J1")]
	public string? FirstStageInvoiceNumber { get; set; }
	[XLColumn(Header = "Montant facture J1 (€)")]
	public double? FirstStageAmountBilled { get; set; }
	[XLColumn(Header = "Date de facturation J1")]
	public DateTime? FirstStageBillingDate { get; set; }
	[XLColumn(Header = "Date d'appel de fonds J1")]
	public DateTime? FirstStageCallForFundsDate { get; set; }

	//Second Stage Billing Information
	[XLColumn(Header = "J2 facturé")]
	public string? IsSecondStageBilled { get; set; }
	[XLColumn(Header = "Numéros de facture J2")]
	public string? SecondStageInvoiceNumber { get; set; }
	[XLColumn(Header = "Montant facture J2 (€)")]
	public double? SecondStageAmountBilled { get; set; }
	[XLColumn(Header = "Date de facturation J2")]
	public DateTime? SecondStageBillingDate { get; set; }
	[XLColumn(Header = "Date d'appel de fonds J2")]
	public DateTime? SecondStageCallForFundsDate { get; set; }

	//Third Stage Billing Information
	[XLColumn(Header = "J3 facturé")]
	public string? IsThirdStageBilled { get; set; }
	[XLColumn(Header = "Numéros de facture J3")]
	public string? ThirdStageInvoiceNumber { get; set; }
	[XLColumn(Header = "Montant facture J3 (€)")]
	public double? ThirdStageAmountBilled { get; set; }
	[XLColumn(Header = "Date de facturation J3")]
	public DateTime? ThirdStageBillingDate { get; set; }
	[XLColumn(Header = "Date d'appel de fonds J3")]
	public DateTime? ThirdStageCallForFundsDate { get; set; }

	//Anah Information
	[XLColumn(Header = "Numéro de dossier Anah")]
	public string? AnahFolderNumber { get; set; }
	[XLColumn(Header = "Date de dépôt du dossier Anah")]
	public DateTime? AnahFolderFilingDate { get; set; }
}