using System.Reflection.Metadata;

namespace Renee.Domain;

public static class Constants
{
	public const string AdminRole = "Admin";
	public const string AssociationMemberRole = "Mem";
	public const int CivilMajority = 18;
	public const int DebtRateForCredit = 35;
	public const string DiffuseCoordinatorRole = "CD";
	public const string FrenchDecimalPattern = @"^\d*(\,)?(\d*)?$";
	public const string FrenchIntPattern = @"^\d*$";
	public const string HouseNumberApiType = "housenumber";
	public const string LocalityApiType = "locality";
	public const long MaxFileSize = 1024 * 1024 * 10;
	public const int MinimalYear = 1500;
	public const string NotAuthorizedPage = "Vous n'êtes pas autorisé à accéder à cette page.";
	public const int OverIndebtednessRate = 50;
	public const string SolidarBuilderRole = "ES";
	public const string TargetedCoordinatorRole = "CC";
	public const string TerritorialBuilderRole = "ET";
	public const string Title = "Renee";
	public const string TrustedTierRole = "TC";
	public const string StructuralReferentRole = "RS";
	public const double TvaValue = 0.20;
	public const string DefaultTerritorialCoordinatorFirstName = "Pauline";
	public const string DefaultDiffuseCoordinatorFirstName = "Marise";
	public const double TvaPercentage = 5.5;
}

public static class CustomClaimTypes
{
	public const string BasedEmail = "BasedEmail";
	public const string BasedIdentity = "BasedIdentity";
	public const string BasedName = "BasedName";
	public const string BasedRole = "BasedRole";
	public const string NotAuthorizedPage = "Vous n'êtes pas autorisé à accéder à cette page.";
	public const string ShouldCheckAnahFiles = "should_check_anah_files";
}

public static class Labels
{
	public const string UnderprivilegedHousingFoundation = "Fondation pour le Logement des Défavorisés (€)";
	public const string UnderprivilegedHousingFoundationWithoutUnit = "Fondation pour le Logement des Défavorisés";
	public const string AbfZone = "Zone ABF";
    public const string AddFileCGU = "Veuillez donner le CGU en format pdf";
    public const string InfoCGU = "Veuillez lire et accepter les CGU avant de continuer.";
    public const string ViewAndAcceptCGU = "J’ai lu et j’accepte les CGU.";
    public const string VersionCGUExists = "Veuillez revoir les informations de votre nouvelle version si elle n'existe pas déjà";
    public const string VersionCGUAdded = "Votre nouvelle version à été ajouté avec succès";
    public const string AccessDashboardAccompanyingFileButtonAltText =
		"Afficher le tableau bord du dossier d'accompagnement.";
	public const string Accompaniement = "Accompagnement";
	public const string AssociatedResourceAddress = "Adresse";
	public const string AccompanyingFileClosedDate = "Date de fermeture";
	public const string AccompanyingFileContact = "Contact";
	public const string AccompanyingFileCreation = "Créer un nouveau dossier";
	public const string Coproperty = "Liste des copropriétés";
	public const string AddCoproperty = "Créer une fiche de copropriété";
	public const string QuickAddCopropertyTitle = "Fiche Copropriété : Création rapide";
	public const string CopropertyGeneralInfoTitle = "Informations générales";
	public const string CopropertyGovernanceContactsTitle = "Gouvernance & Contacts";
	public const string CopropertyDiagnosticsTitle = "Diagnostics & Performances";
	public const string CopropertyWorkFinance = "Travaux & finances";
	public const string CopropertyWorkFinanceTitle = "Financement Copropriété";
	public const string CopropertyWorkTracking = "Suivi réalisation";
    public const string AccompanyingFileLastModificationDate = "Trier les dossiers par date de modification";
	public const string AccompanyingFileList = "Liste des dossiers";
	public const string CguVersions = "Versions CGU";
	public const string ImportCsv = "Import CSV";
	public const string CheckSynthesis = "Vérifier la synthèse";
	public const string CheckAbortRequest = "Vérifier la demande d'abandon";
    public const string AccompanyingFileLowRisk = "Faible";
	public const string AccompanyingFileNumber = "Numéro de dossier";
	public const string AssociatedResourceOpeningDate = "Date d'ouverture";
	public const string AccompanyingFileReference = "Référence dossier";
	public const string AssociatedResourceStage = "Jalon";
	public const string AssociatedResourceStatus = "Statut";
	public const string AccompanyingFileSubmissionForm = "Formulaire de dépôt du dossier";
	public const string AccompanyingType = "Type d'accompagnement";
	public const string AccountSupressionDemand = "Demande de suppression de compte";
	public const string AdaptationBonus = "MaPrimeAdapt' (€)";
	public const string AdaptationBonusWithoutUnit = "MaPrimeAdapt'";
	public const string AddFinancingMode = "Ajouter un autre mode de financement";
	public const string AddingReportingStructureAvailable = "Ajout d'opérateurs disponibles";
	public const string AdditionalAddress = "Complément d'adresse";
	public const string AdditionalCommentary = "Commentaires";
	public const string AdditionalFund = "Caisse complémentaire";
	public const string AddLine = "Ajouter une ligne";
	public const string AddOccupant = "Ajouter un occupant";
	public const string AddReportingStructure =
		"Voulez-vous ajouter cette structure de rattachement à la liste des structures de rattachement ?";
	public const string Address = "Adresse";
	public const string AdministrationContactOccupant = "Contacts et références administratives";
	public const string AgeOccupant = "Âge";
	public const string AgeUnit = "ans";
	public const string AllFiles = "Tous les dossiers";
	public const string AnahCategories = "les conditions de ressources (ANAH)";
	public const string AnahCategory = "Catégorie du ménage selon l'ANAH";
	public const string AnahCategoryTitle = "Catégories ANAH";
	public const string AnahNotification = "Accusé de réception du dossier de demande d’aide.";
	public const string Approve = "Approuver";
	public const string ApproveAccountDeletionRequest =
		"Voulez-vous approuver cette demande de suppression de compte ?";
	public const string AraOpeningStatementSent = "Déclaration d'ouverture ARA envoyée";
	public const string ArchitecturalOrUrbanPlanningStandards = "Normes architecturales ou d'urbanisme";
	public const string AskedAt = "Demande faite le ";
	public const string AskedReportingStructure = "Nom de la structure de rattachement demandée";
	public const string AskedRole = "Rôle demandé dans l'application";
	public const string AskedVerificationCode = "Demander un code de validation";
	public const string AskingRemainingAmountFound = "Faire une demande au Fond Stop Exclusion Énergétique";
	public const string AvailableBudget = "Reste à vivre mensuel du ménage";
	public const string BankLoan = "Prêt bancaire alloué au reste à charge";
	public const string BankLoanToolTipText = "Le prêt bancaire réduit le reste à charge immédiat mais reste une charge future à rembourser par le ménage.";
	public const string BankLoanType = "Quel est le type de prêt bancaire sollicité ?";
	public const string BayWindowCounter = "Nombre de baies vitrées";
	public const string BirthdayOccupant = "Date de naissance";
	public const string BuildingConfiguration = "Configuration du batiment";
	public const string Cancel = "Annuler";
	public const string CanHaveAid = "Le ménage peut prétendre à un crédit";
	public const string CantHaveAid = "Le foyer ne peut prétendre à un crédit";
	public const string Carpentry = "Menuiseries";
	public const string CeilingHeight = "Hauteur sous plafond (m)";
	public const string CGU = "Conditions générales d'utilisation";
	public const string CheckOnYourEt =
		"Veuillez vous rapprocher de votre Ensemblier·ère Territorial·e de référence au sujet de ce dossier";
	public const string ChildrenExpenditure = "Pension alimentaire (€)";
	public const string ClassJump2 = "2";
	public const string ClassJump3 = "3";
	public const string ClassJump4 = "4";
	public const string ClassJump5 = "5";
	public const string ClassJump6 = "6";
	public const string Clothing = "Habillement (€)";
	public const string CommentaryPlaceholder = "Détailler chaque typologie de difficultés rencontrées";
	public const string ConstructionYear = "Année de construction du logement";
	public const string ContactWithFamily = "Contacts avec la famille";
	public const string ContactWithFamilyInput = "Nombre de contacts avec la famille (en présentiel ou à distance)";
	public const string ClassicBankLoan = "Montant du prêt bancaire (€)";
	public const string ClassicBankLoanWithoutUnit = "Montant du prêt bancaire";
	public const string CoOwnershipBonus = "MaPrimeRénov' Copropriété (€)";
	public const string CoOwnershipBonusWithoutUnit = "MaPrimeRénov' Copropriété";
	public const string CraftsmenQualificationTitle = "Qualification des artisans";
	public const string CreatedAccompanyingFileList = "Mes dossiers";
	public const string Curatorship = "Personne placée sous curatelle / sauvegarde de justice ?";
	public const string CuratorshipTitle = "Protection juridique";
	public const string DateFormatUtc = "dd/MM/yyyy";
	public const string DateFormatV3Utc = "dd/MM/yyyy HH:mm:ss";
	public const string DateSigningHouseholdSupport = "Date de la signature du contrat d'accompagnement du ménage";
	public const string DebtRate = "Taux d'endettement";
	public const string DecentHousingBonus = "Ma Prime Logement Décent (€)";
	public const string DecentHousingBonusWithoutUnit = "Ma Prime Logement Décent";
	public const string DegradationIndex = "Indice de dégradation";
	public const string Delete = "Supprimer";
	public const string DeleteAccompanyingFileModalText =
		"Êtes-vous sûr de vouloir supprimer ce dossier de manière définitive ?";
	public const string DeleteAccompanyingFileModalTitle = "Supprimer un dossier";
	public const string DeleteAccount = "Supprimer mon compte";
	public const string DeleteAccountModalAccompanyingFileAttribution = "Avez-vous bien réattribué vos dossiers ?";
	public const string DeleteAccountModalAccountDeletionConfirmation =
		"Êtes-vous bien sûr de vouloir supprimer votre compte ?";
	public const string DeleteAccountModalTitle = "Suppression de compte";
	public const string DeleteQuestionProfile = "Etes-vous sûr de vouloir supprimer cette fiche ?";
	public const string DeliveryTime = "Délai de prise en charge (en mois) : ";
	public const string Department = "Département";
	public const string DepartmentalHouseForDisabledPersons = "MDPH";
	public const string DepartmentalHouseForDisabledPersonsPreFinancingPlan = "MDPH (€)";
	public const string DepartmentPreFinancingPlan = "Département (€)";
	public const string Details = "Précisions";
	public const string DifficultyEncountered = "Difficultés rencontrées par la famille";
	public const string DifficultyFacedByFamilySynthesisCuratorship = "curatelle";
	public const string DifficultyFacedByFamilySynthesisDisability = "handicap";
	public const string DifficultyFacedByFamilySynthesisGuardianship = "tutelle";
	public const string DifficultyFacedByFamilySynthesisLongTermIllness = "maladie";
	public const string DifficultyFacedByFamilySynthesisLossOfIndependence = "perte d'autonomie";
	public const string DifficultyFacedByFamilySynthesisOverIndebted = "surendettement";
	public const string Disability = "Situation de handicap dans le foyer ?";
	public const string DisordersObserved = "Désordres constatés";
	public const string DisregardTabIfSupportedSelfRehabilitationApproachIsNotConsidered =
		"Veuillez ignorer ces précisions si l'auto-réhabilitation accompagnée (ARA) n'est pas envisagée.";
	public const string DoorCounter = "Nombre de portes";
	public const string DpeA = "A";
	public const string DpeB = "B";
	public const string DpeC = "C";
	public const string DpeD = "D";
	public const string DpeE = "E";
	public const string DpeF = "F";
	public const string DpeG = "G";
	public const string DropDownPlaceholder = "Choisir...";
	public const string East = "Est";
	public const string EditAccompanyingFileButtonAltText = "Éditer le dossier";
	public const string EitherDegradationIndexOr0UnsanitaryCoefficientMustBeChecked =
		"Il est nécessaire de renseigner l'indice de dégradation et/ou le coefficient d'insalubrité";
	public const string ElectricalSafety = "Sécurité électrique";
	public const string Electricity = "Electricité (€) (hors chauffage)";
	public const string EmailAddress = "Adresse email";
	public const string EmailOccupant = "Email";
	public const string EmergencyWorks = "Travaux d'urgence";
	public const string EnergeticsEffectOfWorks = "Effets énergétiques attendus de ces travaux";
	public const string EnergeticsRenovationWorks = "Travaux de rénovation énergétique";
	public const string EnergeticTotalSynthesis = "Dépenses énergétiques (€)";
	public const string EnergyAudit = "Audit énergétique";
	public const string EnergyCharacteristics = "Performances énergétiques et environnementales avant travaux";
	public const string EnergyConsumption = "Consommation énergétique annuelle avant travaux (kWhEP/m²)";
	public const string EnergyConsumptionBeforeWork = "Consommation énergétique annuelle avant travaux (kWhEP/m²)";
	public const string EnergyDeprivation = "Privation d'énergie";
	public const string EnergyEffortRateBeforeWorks = "Taux d'effort énergétique avant travaux";
	public const string EnergyEffortRate = "Taux d'effort énergétique %";
	public const string EnergyGain = "Gain énergétique";
	public const string EnergySavingCertificates = "CEE";
	public const string EnergySavingCertificatesPreFinancingPlan = "CEE (€)";
	public const string EnterOnlyNumber = "Veuillez entrer uniquement des chiffres.";
	public const string EsBoardWarning =
		" Les données ci-dessous sont relatives à vos accompagnements en tant qu'Ensemblier·ère Solidaire.";
	public const string EstimatedAnnualEnergyConsumptionAfterWork =
		"Consommation énergétique annuelle estimée après travaux (kWh/m²)";
	public const string EstimatedAnnualGhgEmissionsAfterWork =
		"Emissions GES annuelles estimées après travaux (TonnesEqCO²)";
	public const string EstimatedEnergyClassJump = "Saut de classe énergétique estimé";
	public const string EstimatedEnergyDpeAfterWork = "Étiquette énergie après travaux estimée";
	public const string EstimatedEnergyGesAfterWork = "Étiquette climat après travaux estimée";
	public const string EstimatedRemainingAmount = "Fond Stop Exclusion Énergétique demandé (€)";
	public const string ExistingHumidityAndVaporMigrationManagedAfterTreatment =
		"Gestion de l’humidité existante et de la migration de vapeur après isolation traitée";
	public const string ExitEnergySieveBonus = "Bonus sortie de passoire thermique (€)";
	public const string ExitEnergySieveBonusWithoutUnit = "Bonus sortie de passoire thermique";
	public const string Explanations = "Explications";
	public const string FamilyAllowanceFund = "CAF/MSA";
	public const string FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite =
		"Disponibilités de la famille pour organiser le chantier ARA";
	public const string FamilyCanMobilizeSocialCircleOnConstructionSite =
		"La famille peut mobiliser son entourage sur le chantier";
	public const string FamilyPhysicalCapabilitiesHaveBeenTakenIntoAccount =
		"Prise en compte des capacités physiques de la famille";
	public const string FamilyProject = "Projet de la famille";
	public const string FaultyElectricalSystem = "Système électrique défaillant";
	public const string FileLoaded = " a bien été chargée.";
	public const string FileNotLoaded = "Chargement impossible de la pièce jointe";
	public const string FilesWaitingForValidation = "Dossiers à valider";
	public const string FilterBy = "Filtrer par";
	public const string FilterByReportingStructure = "Filtrer par Structure de rattachement";
	public const string FilterBySolidarBuilder = "Filtrer par Ensemblier·ère Solidaire";
	public const string FilterByAccompanyingFileMilestone = "Filtrer par jalon";
	public const string FilterByAccompanyingFileStatus = "Filtrer par statut";
	public const string FilterByTerritory = "Filtrer par Territoire";
	public const string FilteringOnReference = "Recherche par référence, adresse, nom ou prénom";
	public const string Finance = "Finances";
	public const string FinancingMode = "Mode de financement";
	public const string FinancingModeAmount = "Montant (€)";
	public const string FinancingRequest =
		"Je demande le financement de cet accompagnement par Territoires Zéro Exclusion Energétique.";
	public const string FinancingSources =
		"Quelles sources de financement sont envisagées pour le reste à charge (autres que le fond STOP Exclusion Energétique) ?";
	public const string FinishedAccompanyingFileStage = "Clôturé";
	public const string FireSafety = "Sécurité incendie";
	public const string FirstContactDate = "Date de la première visite";
	public const string FirstName = "Prénom";
	public const string Floors = "Planchers";
	public const string FoldersWithoutSolidarBuilder = "Dossiers sans ES";
	public const string Food = "Alimentation, hygiène (€)";
	public const string FullOwnership = "Pleine propriété";
	public const string Furniture = "Equipements divers (€)";
	public const string GasSafety = "Sécurité gaz";
	public const string Gaz = "Gaz (€) (hors chauffage)";
	public const string GenerateDocuments = "Génération de documents";
	public const string GesA = "A";
	public const string GesB = "B";
	public const string GesC = "C";
	public const string GesD = "D";
	public const string GesE = "E";
	public const string GesEmissions = "Émissions GES annuelles avant travaux (kgCO²e)";
	public const string GesEtiquette = "Étiquette climat (GES) de départ";
	public const string GesF = "F";
	public const string GesG = "G";
	public const string GhgEmissionsAvoidedPerYear = "Emissions GES évitées par an";
	public const string Guardianship = "Personne placée sous tutelle ?";
	public const string GuidedPathwayBonus = "MaPrimeRénov' Parcours Accompagné (€)";
	public const string GuidedPathwayBonusAdaptationBonus = "Ma Prime Rénov' Parcours Accompagné + Ma Prime Adapt'";
	public const string GuidedPathwayBonusWithoutUnit = "MaPrimeRénov' Parcours Accompagné";
	public const string HasUnpaidEnergyBills = "Impayés de factures d'énergie depuis au moins 6 mois";
	public const string Health = "Santé (€)";
	public const string Heating = "Chauffage";
	public const string HeatingEnergy = "Energie de chauffage avant travaux";
	public const string HeatingEnergyTooltipText = "Exemples : Gaz naturel, fioul, électricité...";
	public const string HeatingSystem = "Système de chauffage";
	public const string HighDegradationIndex = "ID≥0,55";
	public const string HighUnsanitaryCoefficient = "CI≥0,4";
	public const string Home = "Page d'accueil";
	public const string HomeAdmin = "Bienvenue dans votre Tableau d'administrateur·rice";
	public const string HomeComposition = "Composition du foyer";
	public const string HomeEt = "Page d'acceuil ET";
	public const string HomeResources = "Ressources du ménage";
	public const string HotWaterProduction = "Production d'eau chaude";
	public const string HouseEtiquette = "Étiquette énergie avant travaux";
	public const string HouseGesEtiquette = "Étiquette climat (GES) de départ";
	public const string Household = "Ménage";
	public const string HouseholdBudget = "Reste à vivre mensuel du ménage (€)";
	public const string HouseholdContribution = "Participation du ménage";
	public const string HouseholdCurrentExpense = "Autres dépenses courantes mensuelles";
	public const string HouseholdDifficulties = "Difficultés dans le ménage";
	public const string HouseholdExpenses = "Dépenses mensuelles";
	public const string HouseholdFixedExpense = "Autres dépenses fixes mensuelles";
	public const string HouseholdFollowedBySocialWorker = "Le ménage est-il suivi par un travailleur social ?";
	public const string HouseholdIdentity = "Identité du foyer";
	public const string HouseholdIdentityTitle = "1. Identité du foyer";
	public const string HouseholdOccasionalExpense = " Autres dépenses occasionnelles mensuelles";
	public const string HouseholdTotalExpense = "Total des dépenses mensuelles moyennes du ménage";
	public const string HouseholdTypology = "Typologie de famille";
	public const string HouseState = "Etat du logement";
	public const string Housing = "Logement";
	public const string HousingCharacteristics = "Caractéristiques du logement";
	public const string HousingCover = "Couverture";
	public const string HousingExpenditure = "Logement (€)";
	public const string HousingTitle = "2. Logement";
	public const string Humidity = "Humidité";
	public const string IdentificationSynthesisButtonTitle = "1. Identifier";
	public const string IdentifyAccompanyingFileStage = "Identifier";
	public const string IdentifyCeeProgramValidityCondition =
		"Cet accompagnement peut rentrer dans le cadre du programme CEE Territoires Zéro Exclusion Energétique, tant de l'approche diffuse que de l'approche ciblée (TZEE), afin de bénéficier des financements associés. \nPour bénéficier des financement associés, le foyer doit : \n - Etre modeste ou très modeste au sens de l'ANAH; \n - Etre propriétaire occupant d'un logement indigne, d'une passoire thermique ou d'un logement énergivore (DPE classes E, F, G) \n - Etre dans une démarche de rénovation performante (saut de 2 classes DPE minimum)";
	public const string IdentifyMilestone = "Suivi du jalon Identifier";
	public const string ImportDataLabel = "Import Excel";
	public const string IncomeTaxReference = "Revenu fiscal annuel de référence du ménage (€)";
	public const string InducedWorks = "Travaux induits";
	public const string InitialEnergyProfile = "Profil énergétique";
	public const string InitialEnergyProfileTitle = "3. Profil énergétique";
	public const string InitialHousingStateTitle = "1. Etat initial du logement";
	public const string InitialStateDiagnosis = "Diagnostic de l'état initial";
	public const string InProgressStatus = "En cours";
	public const string InscriptionForm = "Formulaire d'inscription";
	public const string Insurance = "Assurances (€)";
	public const string InsurancesBlocTitle = "Assurances";
	public const string InsuranceType = "Type d'assurance";
	public const string InterestInPossibleSupportedSelfRehabilitationAra =
		"Intérêt pour la démarche auto-réhabilitation accompagnée (ARA) éventuelle";
	public const string InteriorDesign = "Aménagement intérieur";
	public const string Internet = "Internet (€)";
	public const string IsFamilyReadyForSupportedSelfRehabilitationApproach =
		"La famille est prête à s'engager dans la démarche ARA";
	public const string IsFinancingAsked =
		"Souhaitez-vous faire une demande de financement au Fond STOP Exclusion Energétique ?";
	public const string IsInIleDeFrance = "En Île-de-France";
	public const string IsReadyForOrganizeAndFinanceMilestone = "Voulez-vous passer au jalon Organiser et Financer ?";
	public const string IsReadyForRealiseAndFollowMilestone = "Voulez-vous passer au jalon Réaliser et suivre ?";
	public const string IsRgeLabelUpToDate = "Label RGE à jour";
	public const string IsSolihaStructure = "soliha";
    public const string Job = "Fonction";
	public const string KilogrammeCO2 = "kgCO²";
	public const string KilowattHour = "kWh";
	public const string KWhSavedPerYear = "kWh/m² économisés par an";
	public const string LackOfAutonomy = "Personne en perte d'autonomie dans le foyer ?";
	public const string LandRegisterReference = "Référence cadastrale";
	public const string LastModificationDate = "Date de dernière modification";
	public const string LeadAndAsbestos = "Plomb et amiante";
	public const string LegalNotice = "Mentions légales";
	public const string Leisure = "Sports, loisirs (€)";
	public const string LeroyMerlinFoundation = "Fondation Leroy Merlin (€)";
	public const string LeroyMerlinFoundationWithoutUnit = "Fondation Leroy Merlin";
	public const string LivingSpaceInSquareMeterSynthesis = "Surface habitable";
	public const string LocalPublicAid = "Aides publiques locales";
	public const string LocalPublicAidTotalAmount = "Montant total des aides publiques locales";
	public const string Login = "SE CONNECTER";
	public const string LoginPageHelpTipText =
		"Si vous avez le moindre problème de connexion, n'hésitez pas à contacter l'administrateur·rice de l'outil à l'adresse suivante : reneeadmin@stopexclusionenergetique.org.";
	public const string Logout = "Déconnexion";
	public const string LongTermIllness = "Personne en maladie longue durée ?";
	public const string LowDegradationIndex = "ID<0,35";
	public const string LowIncomeHouseholdsAmount = "MO - modeste";
	public const string LowUnsanitaryCoefficient = "CI<0,3";
	public const string Mail = "E-mail";
	public const string MailContact = "Contact";
	public const string MainOccupant = "Occupant principal";
	public const string MainOccupantAdjective = "principal";
	public const string ManageDeletedAccount = "Gérer les supressions de compte";
	public const string ManageReportingStructure = "Gérer la liste des opérateurs";
	public const string ManageSubscriptions = "Gérer les inscriptions";
	public const string ManagingAccountSupression = "Gérer les suppressions de compte";
	public const string Mar = "Type de parcours MAR";
	public const string NoMar = "Pas MAR";
	public const string ClassicMar = "MAR classique";
	public const string IntensiveMar = "MAR renforcé";
	public const string MarkerNature = "Nature du repérant";
	public const string HouseholdMaximumSavingAmountForRenovationProject =
		"Part des économies de la famille pour financer le reste à charge (€)";
	public const string HouseholdMaximumSavingAmountForRenovationProjectWithoutUnit =
		"Part des économies de la famille pour financer le reste à charge";
	public const string MaximumAmountSupportFamilyMembersRenovationProject =
		"Montant du soutien des autres membres de la famille alloué au reste à charge (€)";
	public const string MaximumAmountSupportFamilyMembersRenovationProjectWithoutUnit =
		"Montant du soutien des autres membres de la famille alloué au reste à charge";
	public const string MediumDegradationIndex = "0,35≤ID<0,55";
	public const string MediumUnsanitaryCoefficient = "0,3≤CI<0,4";
	public const string Month = "mois";
	public const string Municipality = "Commune";
	public const string MunicipalityPreFinancingPlan = "Commune (€)";
	public const string Mutual = "Mutuelle (€)";
	public const string MyAccompanyingFiles = "Mes dossiers";
	public const string MyReportingStructure = "Ma structure de rattachement";
	public const string MyTasks = "Mes tâches";
	public const string Name = "Nom";
	public const string NationalHousingAgency = "ANAH";
	public const string NationalHousingAgencyAmount = "Montant total des aides de l'ANAH";
	public const string NeedTemporaryRehousingSolution = "Besoin d'une solution de relogement temporaire";
	public const string Next = "Suivant";
	public const string NextStageValidationSucces = "Dossier d'accompagnement validé pour le passage au jalon suivant";
	public const string NextStepAndVigilancePoints = "Prochaine(s) étape(s) et points de vigilance";
	public const string No = "Non";
	public const string NoAccompanyingFile = "Aucun dossier";
	public const string NoAccompanyingFileMatchCriteria = "Aucun dossier ne correspond à ces critères";
	public const string NoAccountDeletionRequests = "Aucune demande de suppression de compte non référencée";
	public const string NoAid = "Aucune aide";
	public const string NoDemands = "Aucune demande d'inscription";
	public const string NoMoreUpdateAllowedAfterValidation =
		"Êtes-vous certain de vouloir soumettre les informations de ce jalon ? Une fois le dossier soumis, vous ne pourrez plus les modifier.";
	public const string NoReportingStructure = "Aucune structure de rattachement non référencée";
	public const string NoTasksForUser = "Aucune tâche à traiter.";
	public const string North = "Nord";
	public const string NorthEast = "Nord-Est";
	public const string NorthWest = "Nord-Ouest";
	public const string NumberOfFloor = "Nombre de niveaux";
	public const string NumberOfOccupants = "Nombre d'occupants";
	public const string NumberOfRooms = "Nombre de pièces du logement";
	public const string OccupantTitle = "Occupant";
	public const string OnHold = "En attente";
	public const string Openings = "Ouvertures";
	public const string OpenInNewTab = "Ouvrir dans un nouvel onglet";
	public const string OrganizeAndFinanceAccompanyingFileStage = "Organiser et financer";
	public const string OrganizeAndFinanceCeeProgramValidityCondition =
		"Cet accompagnement peut rentrer dans le cadre du programme CEE Territoires Zéro Exclusion Energétique, tant de l'approche diffuse que de l'approche ciblée (TZEE), afin de bénéficier des financements associés. \nPour bénéficier des financement associés, le foyer doit : \n - Etre modeste ou très modeste au sens de l'ANAH; \n - Être propriétaire occupant d'un logement indigne, d'une passoire thermique ou d'un logement énergivore (étiquette énergétique ou climatique de classes E, F, G) \n - Etre dans une démarche de rénovation performante (saut de 2 classes DPE minimum)";
	public const string OrganizeAndFinanceSynthesisButtonTitle = "2. Organiser et financer";
	public const string OrganizeAndFinanceSynthesisTitle = "Synthèse du jalon Organiser et financer";
	public const string OrganizingAndFinancingMilestone = "Suivi du jalon Organiser et financer";
	public const string Other = "Autre";
	public const string ManualAddress = "Saisir manuellement l’adresse";
    public const string OtherAdditionalFund = "Autre caisse";
	public const string OtherPensionFund = "Autres régimes (EDF, SNCF, fonctionnaires ...)";
	public const string OtherQualification = "Autres qualifications";
	public const string OtherReportingStructure = "Précisez votre structure de rattachement";
	public const string OtherSocialProtectionFund = "Autre caisse";
	public const string PatioDoorCounter = "Nombre de portes-fenêtres";
	public const string PensionFund = "Caisse de retraite";
	public const string PensionFunds = "Caisses de retraite";
	public const string PeopleNumber = "Nombre de personnes composant le ménage";
	public const string PerAdditionalPeople = "Par personne supplémentaire";
	public const string PercentUnit = "%";
	public const string PersonalInformation = "Informations personnelles";
	public const string PersonalInfos = "Informations personnelles";
	public const string PestOrMold = "Nuisibles / moisissures";
	public const string PhoneExpenditure = "Téléphone (€)";
	public const string PhoneNumber = "Numéro de téléphone";
	public const string PhoneNumberOccupant = "N° de téléphone";
	public const string PostalCode = "Code postal";
	public const string PreFinancingPlanSynthesisTitle = "Pré-plan de financement";
	public const string PreFinancingPlanTitle = "3. Pré-plan de financement";
	public const string Previous = "Précédent";
	public const string PreWorkPlanAddButton = "Ajouter un devis";
	public const string PreWorkPlanProjectType = "Type de projet";
	public const string PreWorkPlanSynthesisTitle = "Pré-plan de travaux";
	public const string PreWorkPlanTitle = "2. Pré-plan de travaux";
	public const string PrivacyPolicy = "Politique de confidentialité";
	public const string PrivateAid = "Aides privées";
	public const string PrivateAidTotalAmount = "Montant total des aides privées";
	public const string Profession = "Profession";
	public const string ProfessionalInfos = "Informations professionnelles";
	public const string ProfessionalSituation = "Situation professionnelle";
	public const string Profil = "Mon profil";
	public const string PropertyStatus = "Statut de propriété";
	public const string PropertyType = "Type de Logement";
	public const string PublicEstablishmentsIntercommunalCooperation =
		"Etablissements publics de coopération intercommunale/Agglomération (€)";
	public const string PublicEstablishmentsIntercommunalCooperationWithoutUnit =
		"Etablissements publics de coopération intercommunale";
	public const string QuickAddAccompanyingFile = "Fiche Foyer : Création rapide";
	public const string RealizeAndFollowAccompanyingFileStage = "Réaliser et suivre";
	public const string RealizeAndFollowSynthesisButtonTitle = "3. Réaliser et suivre";
	public const string ReasonForHold = "Justifications";
	public const string ReasonOfProject = "Raison du projet";
	public const string ReasonOfProjectSocialContext = "Description du contexte social";
	public const string ReasonOfProjectTitle = "4. Raison du projet";
	public const string RecommendedWork = "Postes de travaux préconisés";
	public const string RecommendedWorkLabel = "Récapitulatif devis";
	public const string ReferentDiffuseCoordinator = "Coordinateur·rice du Diffus Référent·e";
	public const string ReferentEt = "Ensemblier·ère Territorial·e Référent·e";
    public const string SecondReferentEt = "Second·e Ensemblier·ère Territorial·e Référent·e";
    public const string ReferentSolidarBuilder = "Ensemblier·ère Solidaire Référent·e";

	public const string ReferentTargetedCoordinator = "Coordinateur·rice du Ciblé·e Référent·e";
	public const string Region = "Région";
	public const string RegionPreFinancingPlan = "Région (€)";
	public const string RegisteredUserList = "Liste des utilisateurs inscrits";
	public const string Reject = "Rejeter";
	public const string RejectedStageValidationSucces =
		"Dossier d'accompagnement refusé pour le passage au jalon suivant";
	public const string RejectedStatus = "Rejeté";
	public const string OwnedFunds = "Fonds propres";
	public const string RemainingAmountCanBeAskedAfterStageTwoValidation =
		"Vous pourrez faire une demande au fond du reste à charges une fois les informations de ce jalon validées";
	public const string RemainingAmountFoundMention =
		"Seuls les dossiers se trouvant au jalon - Réaliser et suivre peuvent faire l'objet d'une demande";
	public const string RemainingAmountFoundTitle = "Demande au fond reste à charge Stop Exclusion Énergétique";
	public const string ReneeHeadBandDescription = "Renée, l’appli qui lutte contre l’exclusion énergétique";
	public const string ReneeSubTitle = "l'appli qui lutte contre l'exclusion énergétique";
	public const string ReneeTitle = "BIENVENUE SUR RENÉE";
	public const string RenovationType = "Type de rénovation";
	public const string RenovationWork = "Travaux antérieurs";
	public const string RepaymentOfCredit = "Remboursement de crédits (€)";
	public const string Resources = "Ressources";
	public const string ResourcesSum = "Total des ressources mensuelles moyennes du ménage";
	public const string ResourcesTypology = "Typologie de ressources mensuelles du ménage";
	public const string Role = "Rôle";
	public const string Roofing = "Toitures";
	public const string RoofWindowCounter = "Nombre de fenêtres de toits";
	public const string RoomCounter = "Nombre de pièces";
	public const string SafetyAndHealthWorks = "Travaux de sécurité et salubrité";
	public const string SanitaryPlumbing = "Plomberie sanitaire";
	public const string Sanitation = "Assainissement";
	public const string Save = "Sauvegarder";
	public const string SecondaryOccupantIsDependent = "Personne à charge";
	public const string SecondReferentSolidarBuilder = "Second Ensemblier·ère Solidaire Référent·e";
	public const string SelectAll = "Tout Sélectionnner";
	public const string ShowSynthesisButtonAltText = "Afficher la synthèse du dossier.";
	public const string SignedHouseholdSupportContract =
		"Veuillez charger le contrat d'accompagnement du ménage signé.";
	public const string SignedHouseholdSupportContractLabel = "Contrat d'accompagnement du ménage signé";
	public const string Signin = "S'INSCRIRE";
	public const string SignsOfOverIndebtedness = "Le foyer est surendetté";
	public const string SiretNumber = "Numéro de SIRET";
	public const string SocialProtectionFund = "Caisse de protection sociale";
	public const string SocialProtectionGroup = "Groupe de protection sociale (€)";
	public const string SocialProtectionGroupWithoutUnit = "Groupe de protection sociale";
	public const string SocioProfessionalCategory = "Catégorie socio-professionelle selon l'INSEE";
	public const string SocioProfessionalCategorySynthesis = "Catégorie socio-professionnelle";
	public const string SoundComfort = "Niveau de confort sonore";
	public const string South = "Sud";
	public const string SouthEast = "Sud-Est";
	public const string SouthWest = "Sud-Ouest";
	public const string SquareMeterUnit = "m²";
	public const string StageValidationPopUpTextForAcceptance =
		"Êtes-vous bien sûr de valider le passage au jalon suivant de ce dossier ?";
	public const string StageValidationPopUpTextForRejection =
		"Êtes-vous bien sûr de refuser le passage au jalon suivant de ce dossier ?";
	public const string CommentOnSynthesisValidation = "Commentaire sur le refus du jalon";

    public const string StreetNumberName = "Numéro et nom de rue";
	public const string StructureAssociated = "Structure de rattachement";
	public const string Submit = "Soumettre";
	public const string SummerComfort =
		"Niveau du confort thermique en été (manque de ventilation, refroidissement du logement difficile, …)";
	public const string SunExposure = "Exposition";
	public const string SupportedSelfRehabilitationApproachDetails = "Précisions sur la démarche ARA";
	public const string SupportedSelfRehabilitationTitle = "4. Auto-réhabilitation accompagnée";
	public const string SupportingDocument = " La pièce justificative";
	public const string SupportingDocumentRequest =
		"Pièce justificative (notification d'octroi des aides ANAH) fournie*";
	public const string Surface = "Surface habitable (m²)";
	public const string SynthesisAccompaniementValidityText =
		"Cet accompagnement rentre-t-il dans le cadre du programme CEE Territoires Zéro Exclusion Energétique ?";
	public const string SynthesisModalTitle = "Validation";
	public const string SynthesisNotAvailable = "La synthèse de ce jalon d'accompagnement n'est pas encore disponible.";
	public const string SynthesisTitle = "Synthèse";
	public const string TargetedAnnualEnergyPerformanceAfterWorkTitle =
		"Performances énergétiques annuelles visées après travaux";
	public const string Tax = "Impôts (€)";
	public const string TaxIncomeSynthesis = "Revenu fiscal annuel de référence du ménage (€)";
	public const string Tenant = "Locataire";
	public const string TenYearCivilLiabilityAra = "Responsabilité civile décennale ARA";
	public const string Territory = "Territoire";
	public const string ThanksToSubscribe =
		"Merci de vous être inscrit, vous recevrez sous peu un retour quant à la validité ou non de votre profil";
	public const string ThirdReferentSolidarBuilder = "Troisième Ensemblier·ère Solidaire Référent·e";
	public const string ThirdReferentSolidarBuilderQuickAddLabel =
		"Troisième Ensemblier·ère Solidaire Référent·e (dans le cas où vous avez un chef de projet)";
	public const string TonnesOfCo2Equivalent = "TonnesEqCO²";
	public const string TotalCostOfRecommendedWork = "Montant total de tous les devis (€ TTC)";
	public const string TotalCostOfWorkPackage = "Montant total du devis";
	public const string TotalEuroTtc = "(€ TTC)";
	public const string Transport = "Transports (€)";
	public const string Travel = "Vacances, voyages (€)";
	public const string TreatedAirTightness = "Étanchéité à l'air traitée";
	public const string TreatedThermalBridge = "Ponts thermiques traités";
	public const string Trigram = "Trigramme";
	public const string TrigramTooltipText = "Exemple : pour Nicolas Dupont NDU.";
	public const string TrustedTier = "Tiers de confiance";
	public const string TrustedTierAskedRole = "Précisez le rôle du Tiers de confiance";
	public const string TrustedTierEmail = "Adresse mail du Tiers de confiance";
	public const string TrustedTierFirstName = "Prénom du Tiers de confiance";
	public const string TrustedTierLastName = "Nom du Tiers de confiance";
	public const string TrustedTierPhoneNumber = "Numéro de téléphone du Tiers de confiance";
	public const string TrustedTierRole = "Rôle du Tiers de confiance";
	public const string TrustedTierStructureName = "Nom de la structure du Tiers de confiance";
	public const string Typology = "Typologie de territoire";
	public const string UnsanitaryCoefficient = "Coefficient d'insalubrité";
	public const string UpdateSupportTeamModalTitle = "Modifier l'équipe d'accompagnement";
	public const string UpdateSupportTeamWarningMessage =
		"Attention : seules les personnes disposant d’un compte peuvent être renseignées.";
	public const string UserSubscriptionAsked = "Demande d'inscription";
	public const string ValidationCodeSentAndEnterIt =
		"Un code de validation a été envoyé à l'adresse mail renseignée. Entrer le code reçu par mail.";
	public const string ValidationHouseholdSynthesis = "Synthèse du jalon Identifier";
	public const string StageReadyModalPopUpToastMessage =
		"Votre dossier a bien été soumis. Il doit être validé avant de pouvoir passer au jalon suivant.";
	public const string Ventilation = "Ventilation";
	public const string VentilationSystem = "Système de ventilation";
	public const string VeryLowIncomeHouseholdsAmount = "TMO - très modeste";
	public const string VisitAvailability = "Disponibilités de la famille pour les visites";
	public const string Walls = "Parois";
	public const string Water = "Eau (€)";
	public const string WattForChangeFoundation = "Fondation Watt For Change (€)";
	public const string WattForChangeFoundationWithoutUnit = "Fondation Watt For Change";
	public const string West = "Ouest";
	public const string WindowCounter = "Nombre de fenêtres";
	public const string WinterComfort =
		"Niveau du confort thermique en hiver (montée en température difficile, courants d'air, murs froids,…)";
	public const string WorkDetails = "Précisions sur les travaux";
	public const string WorkProjectTitle = "Projet de travaux";
	public const string WorkQuote = "Devis";
	public const string Year = "Année";
	public const string YearOfAcquisition = "Année d'acquisition/Entrée dans le logement";
	public const string Yes = "Oui";
	public const string ZeroEnergyExclusionTerritoriesProgram = "Programme Territoires Zéro Exclusion Énergétique";
	public const string MonthlyEnergecticsExpenses = "Dépenses d’énergie mensuelles (€)";
	public const string EnergeticsExpense = "Dépenses mensuelles d'énergie";
    public const string InfoEnergeticExpense = "Les dépenses d'énergie mensuelles incluent l’électricité hors chauffage et le gaz hors chauffage. L’eau n’est pas à renseigner ici. L’électricité et le gaz hors chauffage sont déjà comptabilisés dans ce champ.";
    public const string InfoTitleEnergeticExpense = "Dépenses incluses";
    public const string HeatingEnergies = "Énergie(s) de chauffage principale(s)";
	public const string EnnergyEffortRateFormula = "(Dépenses annuelles d’énergie ÷ Revenu fiscal annuel)×100 (Source : Ministère de la Transition écologique)";
	public const string ValueFormula = "Méthode de calcul";
	public const string EnergyDeprivationTootltip = "Ce champ signale si le ménage a dû restreindre ou a été privé de chauffage ou d’électricité. Il est obligatoire pour les foyers dont l’effort énergétique est inférieur à 8 %, afin d’identifier les situations sociales fragiles.";
    public const string FileDeletionModalText = "❓ Êtes-vous sûr·e de vouloir supprimer cette pièce jointe ? Cette action est irréversible.";
	public const string FileDeletionButtonText = "Supprimer définitivement";
	public const string FileDeletedSuccessfully = "Le fichier a été supprimé avec succès.";
	public const string StageValidationModalPopUpToastMessage = "Votre validation a bien été prise en compte.";
	public const string Version = "Version";
	public const string AddingNewCguVersion = "Ajouter une nouvelle version";
	public const string Add = "Ajouter";
	public const string CguVersionsList = "Liste des versions des CGU";
	public const string NoCguVersionsRegistered = "Aucune version des CGU n'est enregistrée pour le moment.";
	public const string Loading = "Chargement en cours...";
	public const string Accept = "Accepter";
	public const string AccompayingTimeDuration = "Temps d'accompagnement des ménages au jalon {0}";
	public const string AccompayingTimeDurationTooltipText =
		"Comptabiliser l'intégralité du travail réalisé sur ce jalon : temps administratif, montage du plan de financement au jalon 2, analyse des devis et audit, suivi de chantier au jalon 3, saisies, relance... et pas uniquement les rendez-vous physiques et téléphoniques avec le ménage.";
	public const string NoRejectionCommentEmailText = "(non précisée)";
	public const string PreFinancialPlanWarning =
					"Les montants indiqués constituent un plan de pré-financement provisoire. Vous pourrez les vérifier et les ajuster au Jalon 3 (Réaliser et Suivre) avant validation définitive.";
	public const string WorkPackageTotalCost = "Total devis";
	public const string FinancementPlanTotalCost = "Total plan de Préfinancement";
	public const string AbsolutGapBetweenWorkPackageCostAndFinancingPlanCost =
		"Écart entre le coût du devis et le plan de préfinancement";
	public const string Totals = "Totaux";
	public const string FinancingDifferentialModalText = "Écart de <strong>{0}</strong> € détecté entre le montant des devis renseignés et le montant total de votre plan de pré-financement. Êtes-vous certain.e de vouloir soumettre le dossier ?";
	public const string StopEnergyExclusionFunds = "Fonds Stop Exclusion Énergétique (€)";
	public const string StopEnergyExclusionFundsWithoutUnit = "Fonds Stop Exclusion Énergétique";
	public const string RemainingAmountPopuptText =  "Votre demande de fonds a bien été enregistrée. Un nouvel onglet s’est ouvert pour compléter le formulaire. S'il ne s'est pas ouvert veuillez cliquer sur <a href=\"{0}\" target=\"_blank\" rel=\"noopener noreferrer\" class=\"text-primary hover:underline\">ce lien</a> pour forcer l'ouverture de l'onglet";
	public const string RemainingAmountPopupTitle = "Lien du formulaire complémentaire";
	public const string NumberOfDisplayedFiles = "Nombre de dossiers affichés : {0}";
	public const string AbortAccompanyingFileButtonText = "Abandonner le dossier";
	public const string AbortAccompanyingFileModalTitle = "Abandon du dossier";
	public const string AbortReason = "Raison d'abandon du dossier";
	public const string Aborted = "Abandonné";
	public const string WaitingForAbortion = "En attente d'abandon";
	public const string AbortionJustification = "Fichier_justificatif_d'abandon";
	public const string AccompanyingFileAbortedSuccessfully = "Le dossier a bien été statué comme abandonné.";
	public const string AccompanyingFileRevertedSuccessfully = "Annulation de l'abandon du dossier réussie.";
	public const string AccompanyingFileAbortConfirmationMessage = "Le dossier est bien passé en attente d'abandon.";
	public const string AbortFile = "Pièce jointe justifiant l'abandon du dossier";
	public const string NumberOfLots = "Nombre de lots";
	public const string NatureOfSyndic = "Nature du Syndic";
	public const string NameOfSyndic = "Nom du syndic";
	public const string PhoneOfSyndic = "N° de Télephone du Syndic";
	public const string MailOfSyndic = "Mail du Syndic";
	public const string NameOfAmo = "Nom de l'AMO";
	public const string ContactOfAmo = "Coordonnées de l'AMO";
	public const string NumberOfContacts = "Nombre de contacts Syndic/AMO";
	public const string DiagnosticsPerformance = "Diagnostics & Performance";
	public const string BuildingDpeLabel = "Etiquette énergétique de l'immeuble";
	public const string BuildingDpeEnergy = "Consommation énergétique de l'immeuble";
	public const string ApartmentDpeLabel = "Etiquette énergétique de l'appartement";
	public const string ApartmentDpeEnergy = "Consommation énergétique de l'appartement";
	public const string BuidlingOrApartmentEnergy = "Il est nécessaire de renseigner l'étiquette et la consommation énergétique de l'immeuble et/ou ceux de l'apartement";
    public const string HousingCaracteristics = "Caractéristiques du Logement";
	public const string HousingTypology = "Typologie du logement";
	public const string HeatingType = "Type de chauffage";
	public const string PerilType = "Périls";
	public const string DateOfAgVote = "Date vote AG travaux collectifs";
	public const string MprCoproAids = "Aides MPR CoPro";
	public const string ComplementaryAids = "Aides complémentaires à la COPRO";
	public const string CollectiveWorksStartDate = "Date vote AG travaux collectifs";
	public const string PlannedEndDate = "Date de fin prévisionnelle";
	public const string ProgressPercentage = "Avancement travaux en %";
	public const string ActualCompletionDate = "Date de fin des travaux effective";
	public const string InvoiceTotalAmount = "Coût total des travaux pour la facture";
	public const string FollowUpComment = "Commentaire de suivi";
	public const string CopropertyIdentitySynthesis = "Synthèse du jalon Identifier";
	public const string BuildingDpe = "DPE Immeuble";
	public const string ApartmentDpe = "DPE Appartement";
	public const string AmoIdentity = "Identité de l'AMO";
	public const string SyndicIdentity = "Identité du Syndic";
	public const string AuditReportFile = "Audit / Étude thermique";
	public const string GeneralMeetingMinutesDocument = "PV d’Assemblée Générale";
	public const string AnahAidRequestNotificationDocument = "Notification de demandes d'Aides Anah";
	public const string AidsAndCollectifsWorks = "Aides et travaux collectifs";
	public const string HandoverReportDocument = "PV de réception des travaux";
	public const string WorkProgress = "suivi des travaux";
	public const string StageValidationCopropertySynthesisSuccess = "La validation du jalon a été fait avec succès";
	public const string CopropertyProfileReference = "Référence de fiche";
	public const string CopropertyProfilePageTitle = "Mes fiches de copropriété";
	public const string NoCopropertyProfile = "Aucune fiche de copropriété";
    public const string DeleteCoproertyProfileModalText = "Êtes-vous sûr de vouloir supprimer cette fiche de manière définitive ?";
	public const string DeleteCopropertyProfileModalTitle = "Supprimer une fiche";
    public const string AccompanyingFileHaveToBeFiledToAnahTooltipTitle = "Déposer dossier à l'ANAH";
	public const string AccompanyingFileHaveToBeFiledToAnahTooltipMessage = "Ce dossier doit être déposé à l’ANAH. L’indicateur disparaîtra automatiquement dès la soumission du Jalon \"Organiser et Financer\".";
	public const string AccompanyingFileHaveToBeFiledToAnahIndicatorText = "⚠️ Déposer à l'ANAH";
	public const string ShouldAccompanyingFileBeSubmittedToAnah = "Dossier à déposer en priorité dans l'ANAH";
	public const string AnahFolderNumber = "Numéro de dossier ANAH";
	public const string AnahFolderNumberTooltipText = "Ce numéro correspond au dossier déposé à l’ANAH. Il est indispensable pour que les dossiers TZEE soient identifiés et sécurisés par l’ANAH.";
	public const string AnahFolderFilingDate = "Date de dépôt du dossier ANAH";
	public const string AnahFolderFilingDateTooltipText = "Indiquez la date de dépôt officielle du dossier ANAH. Cette information permet le suivi de l'instruction des dossiers déposés à l'ANAH.";
	public const string ProjectManagementSupportDeductionText = "L’Assistance à Maîtrise d’Ouvrage (AMO) est financée directement par l’ANAH et TZEE. Le montant de l’AMO doit donc être déduit de la subvention ANAH.";
	public const string RgpdMentionForFreeInput = "Restez toujours objectif, pertinent et juste dans vos commentaires. Il est strictement interdit d’intégrer des commentaires discriminants, injurieux, racistes, sexistes, etc.";
	public const string RgpdMentionSubscriptionPage = "En cliquant sur “Soumettre”, vous acceptez nos <a class='a-no-style' style='color: var(--primary) !important' href='legal/Conditions Générales Utilisation.pdf' target='_blank'>Conditions Générales d’Utilisation</a> et notre <a class='a-no-style' style='color: var(--primary) !important' href='legal/Politique de confidentialité.pdf' target='_blank'>Politique de Confidentialité</a>.";
	public const string InvalidConfirmationCode = "Le code de confirmation est incorrect.";
	public const string UnsavedChangesModalText = "Des modifications non enregistrées ont été détectées. Voulez-vous enregistrer avant de quitter ?";
	public const string NoComment = "Aucun commentaire";
	public const string CommentToSolidarBuilderAttention = "Commentaire à l'attention de l'Ensemblier·ère Solidaire";
	public const string OtherPublicAid = "Autres aides publiques";
	public const string OtherPublicAidTotalAmount = "Montant total des autres aides publiques";
	public const string RemainingAmountAfterPublicAidDeductionTooltipText = "Montant restant après déduction des aides publiques = Total devis TTC – (Total aides ANAH + Total aides publiques locales + Total autres aides publiques)";
	public const string RemainingAmountAfterPublicAidDeduction = "Montant restant après déduction des aides publiques";
	public const string RemaingAmountAfterPublicAndPrivateAidDeductionTooltipText = "Reste à charge après déduction des aides publiques et privées = Total devis TTC – (Total aides ANAH + Total aides publiques locales + Total autres aides publiques + Total aides privées)";
	public const string RemainingAmountAfterPublicAndPrivateAidDeduction = "Reste à charge après déduction des aides publiques et privées";
	public const string PreFinancingPlanBalancingTooltipText = "Équilibrage du plan de financement = Total devis TTC – Total plan de financement (toutes aides + prêts + fonds propres)\r\n\r\nSi = 0 € → vert ✅\r\n\r\nSi > 0 € → orange ⚠️ (excédent de financement)\r\n\r\nSi < 0 € → rouge ❌ (financement insuffisant)";
	public const string PreFinancingPlanBalancing = "Équilibrage du plan de financement";
	public const string PreFinancingPlanValidationWarning = "Le plan de financement présente un déséquilibre de <strong>{0}</strong> €. Souhaitez-vous tout de même valider ?";
	public const string UpdateOfAccompanyingFileTargetInformationSuccess = "La mise à jour des informations de ciblage du dossier d'accompagnement a été effectuée avec succès.";
	public const string AccompanyingFileFacturationInformationUpdateSuccessMessage = "La mise à jour des informations de facturation du dossier d'accompagnement a été effectuée avec succès.";
	public const string FirstStageFacturation = "Facturé J1";
	public const string SecondStageFacturation = "Facturé J2";
	public const string ThirdStageFacturation = "Facturé J3";
	public const string FacturationToolTipMessage = "Facture émise: n° {0}, montant {1} €, le {2}";
	public const string CannotDeleteAccompanyingFileBecauseAStageIsValidated = "Suppression impossible : ce dossier comporte au moins un jalon validé. Pour toute correction, contactez votre Coordinatrice / ET.";
	public const string CannotDeleteAccompanyingFileBecauseItHasBillingInformation = "Suppression impossible : ce dossier a fait l’objet d’une facturation ({0} – facture n° {1}, émise le {2}). Pour toute correction, contactez l’administration.";
	public const string CannotDeleteAccompanyingFileBecauseAStageIsValidatedAndItHasBillingInformation = "Suppression impossible : ce dossier comporte un jalon validé et une facturation émise (J{0} – facture n° {1}, le {2}). La suppression est bloquée pour des raisons administratives et comptables.";
	public const string ToBillStageOne = "À facturer J1";
	public const string ToBillStageTwo = "À facturer J2";
	public const string ToBillStageThree = "À facturer J3";
	public const string FilterByBillingStatus = "Filtrer par statut de facturation";
	public const string AccompanyingFileDocuments = "Documents du dossier";
	public const string OptionnalDocuments = "Autres documents (optionnels)";
	public const string AddOptionnalDocumentButtonText = "Ajouter un document optionnel";
	public const string NoDocumentFound = "Aucun document déposé pour <span class='fw-bold'>{0}</span>";
	public const string FileLoadedMessageAccompanyingFileMenu = "<strong>{0}</strong> déposé le <strong>{1}</strong>{2}";
	public const string AbortRequestDetails = "Précisions sur l'abandon";
	public const string AbortWithBillingRequest = "Abandon demande de facturation";
	public const string AbortWithoutBillingRequest = "Abandon simple (sans facturation)";
	public const string AbortWithBillingRequestLabel = "Abandonné avec demande de facturation J{0}";
	public const string AbortWithoutBillingRequestLabel = "Abandonné sans demande de facturation";
	public const string ChangesSuccessfullySaved = "Vos modifications ont bien été prises en compte.";
	public const string PendingAnahGrant = "Toujours en attente";
	public const string GrantReceived = "Octroi reçu";
	public const string BlockingPopupAnahCheckTitle = "Mise à jour obligatoire des dossiers ANAH";
	public const string BlockingPopupAnahCheckText = "⚠️ Attention : Vous avez {0} dossier(s) en attente de réponse ANAH depuis plus de 6 mois.\r\n Veuillez mettre à jour le statut d'octroi pour chaque dossier avant de continuer.";
	public const string ValidateAndClose = "Valider et fermer";
	public const string BillingBadgeWithBilling = "⚠️ Avec facturation";
	public const string BillingBadgeWithoutBilling = "Sans facturation";
	public const string AbortWithBillingModalTitle = "⚠️ Attention : Cette demande inclut une facturation. Vérifiez impérativement la conformité de la pièce jointe justifiant l'abandon avant de valider.";
	public const string ValidateAbortRequest = "Valider l'abandon";
	public const string ValidateAbortRequestAndBilling = "Valider l'abandon ET la facturation";
	public const string RefuseAbortRequest = "Refuser l'abandon";
	public const string SaveMilestoneSuccess = "Les informations du jalon ont été enregistrées avec succès.";
	public const string DeleteAccompanyingFileSuccess = "Le dossier d'accompagnement a été supprimé avec succès.";
	public const string DeleteCopropertyProfileSuccess = "La fiche de copropriété a été supprimée avec succès.";
	public const string CreateAccompanyingFileSuccess = "Le dossier d'accompagnement a été créé avec succès.";
	public const string CreateCopropertyProfileSuccess = "La fiche de copropriété a été créée avec succès.";
	public const string UpdateSupportTeamSuccess = "L'équipe d'accompagnement a été mise à jour avec succès.";
	public const string SaveAnahGrantCheckSuccess = "Les dates d'octroi ANAH ont été enregistrées avec succès.";
	public const string CreateUserSuccess = "L'inscription a été effectuée avec succès.";
	public const string DeleteUserSuccess = "L'utilisateur a été supprimé avec succès.";
	public const string UpdateUserSuccess = "Les informations de l'utilisateur ont été mises à jour avec succès.";
	public const string RegisterUserSuccess = "L'utilisateur a été enregistré avec succès.";
	public const string DeleteUserAccountSuccess = "Le compte utilisateur a été supprimé avec succès.";
	public const string ChangeReportingStructureSuccess = "La structure de rattachement a été modifiée avec succès.";
	public const string UpdateAccountDeletionRequestSuccess = "Le statut de la demande de suppression de compte a été mis à jour.";
	public const string ImpersonateUserSuccess = "La connexion en tant qu'utilisateur a été effectuée avec succès.";
	public const string StopImpersonationSuccess = "La session d'usurpation d'identité a été arrêtée avec succès.";
	public const string CreateCguVersionSuccess = "La version des CGU a été créée avec succès.";
	public const string DeleteCguVersionSuccess = "La version des CGU a été supprimée avec succès.";
	public const string SendMailSuccess = "L'email a été envoyé avec succès.";
	public const string CreateTaskSuccess = "La tâche a été créée avec succès.";
	public const string DeleteTaskSuccess = "La tâche a été supprimée avec succès.";
	public const string UpdateTaskSuccess = "La tâche a été mise à jour avec succès.";
	public const string StageValidationSuccess = "La validation du jalon a été effectuée avec succès.";
	public const string CreateAnahCategorySuccess = "La catégorie ANAH a été créée avec succès.";
	public const string UpdateAnahCategorySuccess = "La catégorie ANAH a été mise à jour avec succès.";
	public const string CreateSupplementaryOccupantRuleSuccess = "La règle pour personnes supplémentaires a été créée avec succès.";
	public const string UpdateSupplementaryOccupantRuleSuccess = "La règle pour personnes supplémentaires a été mise à jour avec succès.";
	public const string AddReportingStructureSuccess = "La structure de rattachement a été ajoutée avec succès.";
	public const string CreateAzureUserSuccess = "L'utilisateur Azure a été créé avec succès.";
	public const string UpdateCguVersionSuccess = "La version des CGU a été mise à jour avec succès.";
	public const string ImportCsvSuccess = "L'import a été effectué avec succès.";
	public const string CopropertyAttachment = "Rattacher à une copropriété";
	public const string CreateANewCopropertyProfile = "Créer une nouvelle copropriété";
	public const string DeadlineReachedModalTitle = "⚠️ Modification dossier impossible";
	public const string DeadlineReachedModalMessage = "La modification ou la complétion des dossiers TZEE se fait désormais exclusivement sur la nouvelle plateforme Marius.";
	public const string DeadlineReachedModalRedirection = "Rendez-vous sur la plateforme Marius pour modifier ou compléter votre dossier.";
	public const string DeadlineReachedModalLinkText = "Accéder à Marius";
	public const string MariusURL = "https://marius-renov.fr";
	public const string Close = "Fermer";

	public static class Alt
	{
		public const string DeleteAccompanyingFileButtonAltText = "Supprimer le dossier";
		public const string Logo = "Logo stop à l'exclusion énergétique";
		public const string UploadIcon = "Icône représentant un utilisateur voulant déposer un fichier";
		public const string UserConnect = "Icône représentant un utilisateur voulant se connecter";
		public const string UserSignUp = "Icône représentant un utilisateur voulant s'inscrire";
	}

	public static class Errors
	{
		public const string AccompanyingFileIsNotInThirdStage =
			"Vous ne pouvez faire une demande au fond de reste à charge que lorsque votre dossier est au jalon Réaliser et suivre";
		public const string AirtableError = "Erreur lors de la création de la demande de reste à charge.";
		public const string BadUploadedFileExtension =
			"Le fichier que vous avez tenté de télécharger n’est pas dans un format valide. Veuillez sélectionner un fichier au format PDF, PNG, JPG, JPEG, WEBP.";
		public const string ConstructionYearRangeError = "Veuillez rentrer une année de construction valide.";
		public const string DateSigningHouseholdSupportInvalidRange =
			"Veuillez saisir une date de début d'accompagnement ultérieure à la date de première visite.";
		public const string DpeMustBeSelected = "Sélectionner l'étiquette énergétique.";
		public const string DuplicateAddress = "Un dossier existe déjà pour cette adresse.";
		public const string DuplicateEmail = "Un dossier existe déjà pour cette adresse mail.";
		public const string DuplicateOccupant = "Un dossier existe déjà pour cet occupant.";
		public const string DuplicatePhoneNumber = "Un dossier existe déjà pour ce numéro de téléphone.";
		public const string EmailAddressFormatInput = "Veuillez entrer un email valide.";
		public const string EnterFamilyProject = "Veuillez saisir le projet de famille.";
		public const string EnterSocialContext = "Veuillez saisir le contexte social.";
		public const string ErrorWhileRetrievingAnahCategories = "Erreur lors du chargement des catégories ANAH";
		public const string ExistingFundingModeName = "Ce mode de financement existe déjà.";
		public const string FirstContactDateInvalidRange =
			"Veuillez saisir une date de première visite antérieure à la date de début d'accompagnement.";
		public const string FirstNameRequiredInQuickAdd = "Veuillez renseigner le prénom.";
		public const string GesMustBeSelected = "Sélectionner l'étiquette climatique.";
		public const string IncorrectFormatOfSiretNumber =
			"Le numéro de SIRET n'est pas conforme, 14 chiffres sont nécessaires.";
		public const string IntShouldBeBetween = "La valeur doit être entre {0} et {1}";
		public const string InvalidDateRange = "Veuillez saisir une date comprise entre 1950 et 2100.";
		public const string InvalidEmail = "Veuillez saisir votre adresse e-mail.";
		public const string InvalidFormatDateSigningHouseholdSupport =
			"Vérifiez le format du champ \"Date de début d'accompagnement\".";
		public const string InvalidFormatFirstContactDate = "Vérifiez le format du champ \"date du début de suivi\".";
		public const string LastNameRequiredInQuickAdd = "Veuillez renseigner le nom.";
		public const string List = "Erreurs :";
		public const string MaximumFileSizeExceeded =
			"Le fichier que vous avez essayé de charger dépasse la taille maximale autorisée de 10 Mo.\r\nVeuillez choisir un fichier plus léger ou compresser le document avant de réessayer.";
		public const string NoData = "NO_DATA_ERROR";
		public const string NumberNotBeEmpty = "La valeur ne peut être vide";
		public const string NumberShouldBeCorrect = "La valeur n'est pas dans le format attendu";
		public const string PhoneNumberFormatInput = "Veuillez saisir un numéro de téléphone valide.";
		public const string RequiredAccompanyingType = "Veuillez renseigner le champ type d'accompagnement.";
		public const string RequiredAdditionalFundFreeInput = "Veuillez saisir la caisse complémentaire.";
		public const string RequiredAgeMainOccupant = "Veuillez renseigner l'âge.";
		public const string RequiredAskedPropertyStatusInput = "Veuillez sélectionner le statut de propriété.";
		public const string RequiredAskedPropertyTypeInput = "Veuillez sélectionner le type de logement.";
		public const string RequiredCopropertyProfileInput = "Veuillez sélectionner une copropriété.";
		public const string RequiredAskedConstructionYear = "Veuillez renseigner l’année de construction.";
		public const string RequiredAskedResourcesTypologyInput =
			"Veuillez remplir la topologie de ressources mensuelles du ménage.";
		public const string RequiredAskedSocioProfessionalCategoryInput =
			"Veuillez sélectionner une catégorie socioprofessionelle.";
		public const string RequiredAssociatedStructure = "Veuillez sélectionner une structure de rattachement.";
		public const string RequiredBirthdayInput = "Veuillez saisir une date de naissance.";
		public const string RequiredCommentOnMarkerNature = "Veuillez spécifier la nature du repérant.";
		public const string RequiredContactWithFamily = "Veuillez saisir le nombre de contacts avec la famille.";
		public const string RequiredDateSigningHouseholdSupport =
			"Veuillez renseigner la date de signature du contrat d'accompagnement du ménage.";
		public const string RequiredDepartmentAddressInput = "Veuillez saisir le departement.";
		public const string RequiredDiffuseCoordinator = "Veuillez renseigner le coordinateur·rice du Diffus";
		public const string RequiredMonthlyEnergeticsExpenses =
			"Veuillez renseigner les dépenses totales d'énergie mensuelles.";
		public const string RequiredEnergyConsumption =
			"Veuillez renseigner la consommation énergétique annuelle avant travaux.";
		public const string RequiredEnergyDeprivation = "Veuillez sélectionner la privation d'énergie.";
		public const string RequiredEtReferent = "Veuillez renseigner l'Ensemblier·ère Territorial·e";
		public const string RequiredField = "Veuillez renseigner le champ ci-dessus.";
		public const string RequiredFirstContactDate = "Veuillez entrer une date de début de suivi.";
		public const string RequiredFirstnameInput = "Veuillez saisir le prénom.";
		public const string RequiredHousingTypologyInput = "Veuillez sélectionner la typologie de territoire.";
		public const string RequiredLastnameInput = "Veuillez saisir le nom.";
		public const string RequiredMarkerNature = "Veuillez indiquer la nature du repérant.";
		public const string RequiredNatureOfSyndic = "Veuillez donner la nature du Syndic";
		public const string RequiredNameOfSyndic = "Veuillez donner le nom du Syndic";
		public const string RequiredPhoneOfSyndic = "Veuillez donner le numéro de télephone du Syndic si le logement n'est pas en résidentiel collectif.";
		public const string RequiredMailOfSyndic = "Veuillez donner l'adresse Mail du Syndic si le logement n'est pas en résidentiel collectif.";
		public const string RequiredNameOfAmo = "Veuillez donner le nom de l'AMO";
		public const string RequiredMunicipalityAddressInput = "Veuillez saisir la commune.";
		public const string RequiredOtherReportingStructure = "Veuillez renseigner votre structure de rattachement.";
		public const string RequiredPensionFreeInput = "Veuillez saisir la caisse de retraite.";
		public const string RequiredPhoneNumberInput = "Veuillez saisir un numéro de téléphone.";
		public const string RequiredPostalCodeAddressInput = "Veuillez saisir le code postal.";
		public const string RequiredRegionAddressInput = "Veuillez saisir la région.";
		public const string RequiredRemainingAmount =
			"Veuillez renseigner le montant du Fond STOP Exclusion Énergétique demandé ";
		public const string RequiredRenovationExplanationsInput = "Veuillez renseigner les travaux antérieurs.";
		public const string RequiredRole = "Veuillez sélectionner un rôle.";
		public const string RequiredSignedHouseholdSupportContract = "Veuillez charger le document ci-dessus.";
		public const string RequiredSiretNumber = "Veuillez renseigner le numéro de SIRET.";
		public const string RequiredSocialProtectionFreeInput = "Veuillez saisir la caisse de protection sociale.";
		public const string RequiredSolidarBuilderReferent =
			"Veuillez sélectionner le nom de l'Ensemblier·ère Solidaire référent·e.";
		public const string RequiredStreetNumberNameAddressInput = "Veuillez saisir la rue.";
		public const string RequiredSurfaceInput = "Veuillez saisir la surface habitable";
		public const string RequiredTargetCoordinator = "Veuillez renseigner le coordinateur·rice du Ciblé";
		public const string RequiredTaxIncomeInput = "Veuillez saisir le revenu fiscal de référence du ménage.";
		public const string RequiredTerritory = "Veuillez renseigner le territoire.";
		public const string RequiredTrigramInput = "Veuillez saisir le trigramme.";
		public const string RequiredUnpaidEnergyBills =
			"Veuillez indiquer si le ménage a des impayés de factures d'énergie depuis au moins 6 mois.";
		public const string RequiredUnsanitaryCoefficientOrDegradationIndex =
			"Veuillez renseigner l'indice de dégradation ou le coefficient d'insalubrité.";
		public const string RequiredZeroEnergyExclusionTerritoriesProgram =
			"Veuillez renseigner si l'accompagnement rentre dans le cadre du Programme Territoires Zéro Exclusion Énergétique.";
		public const string StageValidationErrorAccompanyingFileNotFound = "Erreur: le dossier n'a pas été trouvé.";
		public const string StageValidationErrorAccompanyingFileUpdateFailed =
			"Erreur: la mise à jour du dossier a échoué.";
		public const string SystemError = "Erreur lors du traitement de la requête. Veuillez ressayer.";
		public const string TrigramInputLength = "Le Trigramme doit avoir 3 caractères.";
		public const string UnhandledErrorOccured =
			"Une erreur est survenue lors du traitement de votre requête, veuillez réessayer.";
		public const string UnknownErrorFormField = "Une erreur est présente dans un champ du formulaire.";
		public const string UserDuplicateEmail = "Un utilisateur existe déjà avec cette adresse mail.";
		public const string UserNotAllowedToCreateAirtableRecord =
			"Vous ne pouvez pas faire de demande, car ce dossier ne vous appartient pas.";
		public const string YearOfAcquisitionRangeError = "Veuillez rentrer une année d'acquisition/entrée valide.";
		public const string HouseholdTypologyError = "Veuillez sélectionner une typologie de famille.";
		public const string UserNotFound = "Aucun utilisateur trouvé";
		public const string CguNotFound = "Aucun CGU n'a été trouvé";
		public const string FileErrorNotificationSummary = "Echec de la suppression du fichier.";
		public const string FileErrorNotificationDetails = "Une erreur est survenue lors de la suppression du fichier. Veuillez réessayer.";
		public const string NoTerritorialBuildersAffectedOnTargetedAccompanyingFileType = "Erreur: ce dossier en accompagnement ciblé n'a pas d'ensemblier territorial affecté.";
		public const string ErrorWhileLoadingAccompanyingFile = "Une erreur est survenue pendant le chargement du dossier d'accompagnement.";
		public const string UserNotAllowedToSeeAccompanyingFile = "Vous n’avez pas accès à ce dossier.";
		public const string UserNotAllowedToSeeCopropertyProfile = "Vous n'avez pas accès à cette fiche";
		public const string LoadCguVersionsError = "Merci de fournir le fichier CGU et sa version.";
		public const string RequiredAccompayingTimeDuration = "Veuillez renseigner le temps d'accompagnement avant de poursuivre.";
		public const string RequiredReportingStructureName = "Erreur : Le nom de la structure de rattachement est obligatoire.";
		public const string RequiredFundingModeName = "Veuillez renseigner le nom du mode de financement.";
		public const string ErrorWhileCreatingRemainingAmount = "Echec de l'envoi au reste à charge.";
		public const string ErrorWhileLoadingAccompanyingFiles = "Une erreur est survenue lors de la récupération des dossiers.";
		public const string RequiredAbortReason = "Veuillez sélectionner une raison d'abandon.";
		public const string RequiredAbortJustification = "Veuillez ajouter une pièce justificative.";
		public const string ErrorWhileUpdatingAccompanyingFile = "Une erreur technique est survenue lors de la mise à jour du dossier.";
		public const string RequiredAnahFolderNumber = "Veuillez renseigner le numéro de dossier ANAH.";
		public const string RequiredAnahFolderFilingDate = "Veuillez renseigner la date de dépôt du dossier ANAH.";
		public const string RequiredHousingTypology = "Veuillez saisir le typologie de logement";
		public const string RequiredHeatingType = "Veuillez saisir le type de chauffage utilisé";
		public const string RequiredPerilType = "Veuillez selectionner le péril constaté";
		public const string ErrorWhileLoadingCopropertyProfile = "Une erreur est survenue pendant le chargement de la fiche de copropriété.";
		public const string UploadSynthesisFile = "Veuillez charger le document ci-dessus";
		public const string ErrorCopropertyProfileNotFound = "Erreur: la fiche de copropriété n'a pas été trouvé";
		public const string NoTerritorialBuildersAffectedOnTargetedCopropertyProfileType
			= "Erreur: cette fiche de copropriété en accompagnement ciblé n'a pas d'ensemblier territorial affecté.";
		public const string StageValidationErrorCopropertyProfileUpdateFailed = "Erreur: la validation du jalon de la fiche à échoué.";
		public const string RequiredDpeLabel = "Veuillez sélectionner au moins un des deux étiquettes (étiquette de l'immeuble ou etiquette de l'appartement)";
		public const string RequiredBuildingDpeEnergy = "Veuillez saisir la consommation énergétique de l'immeuble";
		public const string RequiredApartmentDpeEnergy = "Veuillez saisir la consommation énergétique de l'appartement";
		public const string ErrorWhileRetrievingAccompanyingTeamInformation = "Erreur lors du chargement des informations de l'équipe d'accompagnement";
		public const string RequiredWorkPackage = "Veuillez renseigner au moins un devis.";
		public const string RequiredWorkTypes = "Veuillez renseigner au moins un poste de travaux préconisé.";
		public const string RequiredWorkPackagePrice = "Veuillez renseigner le prix du devis.";
		public const string ErrorWhileUpdatingTargetInformation = "Erreur lors de la mise à jour des informations de ciblage";
		public const string NoRecipientFound = "La destinatire de l'email n'a pas été trouvé.";
		public const string RequiredAbortRequestDetails = "Veuillez renseigner les précisions sur l'abandon.";
		public const string RequiredValidatorComment = "Veuillez renseigner le commentaire à l'attention de l'Ensemblier.ère Solidaire.";
		public const string ErrorWhileUpdatingAccompanyingFiles = "Une erreur technique est survenue lors de la mise à jour des dossiers. Veuillez réessayer.";
		public const string ErrorWhileSavingMilestone = "Une erreur est survenue lors de l'enregistrement du jalon. Veuillez réessayer.";
		public const string ErrorWhileDeletingAccompanyingFile = "Une erreur est survenue lors de la suppression du dossier d'accompagnement.";
		public const string ErrorWhileDeletingCopropertyProfile = "Une erreur est survenue lors de la suppression de la fiche de copropriété.";
		public const string ErrorWhileCreatingAccompanyingFile = "Une erreur est survenue lors de la création du dossier d'accompagnement.";
		public const string ErrorWhileCreatingCopropertyProfile = "Une erreur est survenue lors de la création de la fiche de copropriété.";
		public const string SolidarBuilderRequired = "L'Ensemblier·ère Solidaire est obligatoire.";
		public const string AssignedUserRequired = "L'utilisateur assigné est obligatoire.";
		public const string AssignedUserNotFound = "L'utilisateur assigné n'a pas été trouvé.";
		public const string InvalidUserData = "Les données de l'utilisateur sont invalides ou incomplètes.";
		public const string ErrorWhileCreatingUser = "Une erreur est survenue lors de la création de l'utilisateur.";
		public const string ErrorWhileDeletingUser = "Une erreur est survenue lors de la suppression de l'utilisateur.";
		public const string ErrorWhileUpdatingUser = "Une erreur est survenue lors de la mise à jour de l'utilisateur.";
		public const string ErrorWhileRegisteringUser = "Une erreur est survenue lors de l'inscription de l'utilisateur.";
		public const string ErrorWhileDeletingUserAccount = "Une erreur est survenue lors de la suppression du compte utilisateur.";
		public const string ErrorWhileChangingReportingStructure = "Une erreur est survenue lors du changement de structure de rattachement.";
		public const string ErrorWhileUpdatingAccountDeletionRequest = "Une erreur est survenue lors de la mise à jour du statut de la demande de suppression.";
		public const string ImpersonateUserNotFound = "L'utilisateur à impersonner n'a pas été trouvé.";
		public const string ErrorWhileImpersonating = "Une erreur est survenue lors de la connexion en tant qu'utilisateur.";
		public const string ErrorWhileStoppingImpersonation = "Une erreur est survenue lors de l'arrêt de la session d'usurpation.";
		public const string CguLabelAndVersionRequired = "Le libellé et la version des CGU sont obligatoires.";
		public const string CguVersionAlreadyExists = "Cette version des CGU existe déjà.";
		public const string ErrorWhileCreatingCguVersion = "Une erreur est survenue lors de la création de la version des CGU.";
		public const string ErrorWhileDeletingCguVersion = "Une erreur est survenue lors de la suppression de la version des CGU.";
		public const string MailTypeNotFound = "Le type d'email n'a pas été trouvé.";
		public const string ErrorWhileSendingMail = "Une erreur est survenue lors de l'envoi de l'email.";
		public const string InvalidTaskData = "Les données de la tâche sont invalides.";
		public const string ErrorWhileCreatingTask = "Une erreur est survenue lors de la création de la tâche.";
		public const string ErrorWhileDeletingTask = "Une erreur est survenue lors de la suppression de la tâche.";
		public const string ErrorWhileUpdatingTask = "Une erreur est survenue lors de la mise à jour de la tâche.";
		public const string EmailRequired = "L'adresse email est obligatoire.";
		public const string ErrorWhileUpdatingAzureUser = "Une erreur est survenue lors de la mise à jour de l'utilisateur Azure.";
		public const string ErrorWhileCreatingAzureUser = "Une erreur est survenue lors de la création de l'utilisateur Azure.";
		public const string ErrorWhileUpdatingLastLoginDate = "Une erreur est survenue lors de la mise à jour de la date de connexion.";
		public const string ErrorWhileUpdatingAnahGrantCheckDate = "Une erreur est survenue lors de la mise à jour de la date de vérification ANAH.";
		public const string InvalidCguData = "Les données des CGU sont invalides ou incomplètes.";
		public const string ErrorWhileUpdatingCguVersion = "Une erreur est survenue lors de la mise à jour de la version des CGU.";
		public const string ImportRunNotFound = "L'import n'a pas été trouvé.";
		public const string ErrorWhileSavingImportErrors = "Une erreur est survenue lors de l'enregistrement des erreurs d'import.";
		public const string ErrorWhileCreatingImportRun = "Une erreur est survenue lors de la création de l'import.";
		public const string TaskNotFound = "La tâche n'a pas été trouvée.";
		public const string CopropertyProfileCreationFailed = "La création de la fiche de copropriété a échoué.";
		public const string TzeeProgramDeadlineReached = "La date limite de création de dossiers du programme TZEE est atteinte. Veuillez contacter l’administrateur.";
		public const string TzeeProgramMilestone1DeadlineReached = "La date limite de création de dossiers du programme TZEE au jalon 1 est atteinte. Veuillez contacter l’administrateur.";
	}
}

public static class RealiseAndFollowMilestone
{
	public static class Labels
	{
		public const string AccompanyingCost = "Coût de l'accompagnement (HT)";
		public const string AccompanyingFileReport = "Rapport d'accompagnement du ménage";
		public const string AccompanyingFileReportLabel = "Veuillez charger le rapport d'accompagnement du ménage.";
		public const string AccompanyingTime = "Durée de l'accompagnement (en mois) : ";
		public const string AddAnInvoice = "Ajouter une facture";
		public const string BilledWorkForce = "Dont main d'œuvre facturée";
		public const string EducationalFrameworkRating = "Notation relative au cadre éducatif";
		public const string EffectiveComplianceWithWorkRecommendations =
			"Respect effectif des préconisations de travaux (comparaison devis / factures)";
		public const string EndOfAccompanyingDate = "Date de clôture de la prestation d’accompagnement";
		public const string EndOfAccompanyingDateTooltip = "Indiquée page 3, avant la signature du ménage, dans le « Rapport d’accompagnement » de l'Anah.";
		public const string EndOfEncounterDate = "Date de fin du suivi du projet de rénovation";
		public const string EndOfEncounterDateTooltip = "Dernières étapes du parcours : demande de paiement auprès de l’Anah et des autres financeurs, règlement des factures des artisans, prise en main du logement rénové, etc.";
		public const string Evaluations = "Evaluations";
		public const string EvaluationsTitle = "5. Evaluations";
		public const string FamilySatisfactionWithSupport = "Satisfaction de la famille concernant l'accompagnement";
		public const string HasEmergencyWorks = "Travaux d'urgence";
		public const string HasFinishingWorks = "Travaux de finition";
		public const string HasHousingAdaptationWorks = "Travaux d'adaptation du logement";
		public const string HasHumidityManagement =
			"Gestion de l’humidité existante et de la migration de vapeur après isolation traitée";
		public const string HasPreparationWorks = "Travaux de préparation";
		public const string HasSafetyWorks = "Travaux de sécurisation";
		public const string HasUnsanitaryExit = "Sortie d'insalubrité";
		public const string HasWorkEnablingHomeSupport = "Travaux ayant permis le maintien à domicile";
		public const string HouseholdAutoFinancing = "Autofinancement du ménage";
		public const string IntermediateAirtightnessTestResult =
			"Résultat du test intermédiaire d’étanchéité à l’air si rénovation globale";
		public const string Invoice = "Facture";
		public const string IsBackToEmployment =
			"Retour à l'emploi (si recherche d'emploi au début de l'accompagnement)";
		public const string NotationTooltipText = "0 note minimum, 10 note maximum";
		public const string ProjectCost = "Coûts du projet";
		public const string ProjectCostTitle = "2. Coût du projet";
		public const string ProjectEnd = "Fin du projet";
		public const string ProjectEndTitle = "6. Fin du projet";
		public const string RealiseAndFollowSynthesisTitle = "Synthèse du jalon Réaliser et suivre";
		public const string TotalCost = "Coût total des travaux pour la facture ";
		public const string TreatedAirTightness = "Étanchéité à l’air traitée";
		public const string TreatedThermalBridges = "Ponts thermiques traités";
		public const string WaterproofingTreatmentActions =
			"Si pas de test, justification et actions mises en place pour le traitement de l’étanchéité à terme ";
		public const string WellBeingRating = "Notation relative au bien-être";
		public const string WorksReceiptPv = "PV de réception des travaux";
		public const string WorksReceiptPvFileLabel = "Veuillez charger le PV de réception des travaux.";
		public const string WorkSummary = "Récapitulatif des travaux";
		public const string WorkSummaryTitle = "4. Récapitulatif des travaux";
		public const string WorkTotalCost = "Coût total des travaux (€) TTC";
		public const string AnahHelpObtention = "Notification de l’octroi de l’aide ANAH";
		public const string AnahHelpObtentionFileLabel = "Veuillez charger la notification de l’octroi de l’aide ANAH.";
		public const string FinalFinancingPlanTitle = "3. Plan de financement final";
		public const string FinalFinancialPlanWarning =
						"Ces montants ont été pré-remplis à partir du plan de pré-financement provisoire validé au Jalon 2. Veuillez vérifier attentivement chaque montant, les modifier si nécessaire, puis valider définitivement ce plan de financement.";
		public const string SiteSupervisionTitle = "1. Suivi de chantier";
		public const string OverallStartDate = "Date du lancement de chantier";
		public const string EstimatedOverallCompletionDate = "Date prévisionnelle de fin globale";
		public const string ActualOverallEndDate = "Date de signature du procès verbal de réception";
		public const string OverallProgress = "Avancement global (%)";
		public const string NextCoordinationMeetingScheduledFor = "Prochain point de coordination prévu le…";
		public const string OverallObservations = "Observations globales";
		public const string ConstructionSite = "Chantier";
		public const string WorkParticipant = "Intervenant travaux";
		public const string ParticipantType = "Type d'intervenant";
		public const string WorkTypesLabel = "Lot / Type de travaux";
		public const string ParticipantName = "Nom de l’intervenant";
		public const string ContactAdvisor = "Contact référent";
		public const string StartDateOfWork = "Date de démarrage des travaux (lot)";
		public const string EstimatedCompletionDate = "Date prévisionnelle de fin (lot)";
		public const string ActualEndDate = "Date réelle de fin (lot)";
		public const string Progress = "Avancement (%)";
		public const string WorkQuality = "Qualité des travaux";
		public const string CommentOnWorkQuality = "Commentaire sur la qualité des travaux";
		public const string WorkParticipantDificulties = "Difficultés rencontrées";
		public const string CommentOnWorkParticipantDifficulties = "Commentaire sur les difficultés rencontrées";
		public const string SpecificComments = "Commentaires spécifiques";
		public const string ShouldChangeFinalEstimatedDpe = "Changement d’étiquette énergétique prévisionnel finale";
		public const string FinalDpe = "Étiquette énergétique finale";
		public const string FinalDpeClassJump = "Saut de classe énergétique final";
		public const string AnahGrantDate = "Date d'octroi ANAH";
		public const string PreSiteSupervisionMeetingDate = "Date de la réunion pré-chantier";
		public const string PreSiteSupervisionMeetingDateTooltip = "Date du rendez-vous ou l'ensemble des artisans planifient leur intervention et la date du lancement du chantier";
		public const string OverallStartDateTooltip = "Date où le premier artisans intervient dans le logement de la famille";
		public const string ActualOverallEndDateTooltip = "Date de signature du dernier procès verbal de réception de travaux";
	}

	public static class Errors
	{
		public const string EndOfAccompanyingDateInvalidRange =
			"Veuillez saisir une date de fin d'accompagnement ultérieure à la date de début d'accompagnement.";
		public const string EndOfAccompanyingDateNotBeforeEndOfEncounterDate =
			"Veuillez saisir une date de fin d'accompagnement antérieure à la date de fin de suivi.";
		public const string EndOfEncounterDateNotLaterThanStartOfAccompanyingDate =
			"Veuillez saisir une date de fin de suivi ultérieure à la date de début d'accompagnement.";
		public const string RequiredContactWithFamily =
			"Veuillez saisir le champ nombre de contacts avec la famille (présentiel ou à distance).";
		public const string RequiredEducationalFrameworkRating =
			"Veuillez sélectionner la notation relative au cadre éducatif.";
		public const string RequiredEffectiveComplianceWithWorkRecommendations =
			"Veuillez renseigner le champ respect effectif des préconisations de travaux.";
		public const string RequiredEndOfAccompanyingDate =
			"Veuillez saisir le champ date de fin d'accompagnement (réception des travaux).";
		public const string RequiredEndOfEncounterDate = "Veuillez saisir le champ date de fin de suivi.";
		public const string RequiredFamilySatisfactionWithSupport =
			"Veuillez saisir la satisfaction de la famille concernant l'accompagnement.";
		public const string RequiredWellBeingRating = "Veuillez sélectionner la notation relative au bien-être.";
		public const string ProgressInvalidRange = "Veuillez saisir une valeur entre 0 et 100.";
		public const string FinalDpeValidationError = "L'étiquette finale sélectionnée est la même que l'étiquette prévisionnelle, veuillez la modifier ou indiquer qu'il n'y a pas de changement d'étiquette.";
		public const string RequiredAnahGrantDate = "Veuillez saisir la date d'octroi ANAH.";
	}
}

public static class MarkerNatureLabels
{
	public const string Association = "Associations";
	public const string CityHall = "Mairie";
	public const string DirectCall = "Sollicitation directe";
	public const string HealthFunds = "Caisses prévoyance et santé, employeurs";
	public const string Operator = "Opérateur";
	public const string Other = "Autre";
	public const string PublicActor =
		"Acteurs publics (travailleurs sociaux, collectivités, CCAS, CAF, France Rénov',...)";
	public const string PublicActorv3 =
		"Acteurs publics (travailleurs sociaux, collectivités, CCAS, CAF, France Rénov', …)";
	public const string Slime = "SLIME";
	public const string Volunteers = "Bénévoles associatifs et jeunes en Service Civique";
}

public static class GeographicalAreaTypologyLabel
{
	public const string Rural = "Rural";
	public const string Urban = "Urbain";
}

public static class HouseholdTypologyLabel
{
	public const string CoupleWithAdultStaying = "Couple avec adulte hébergé";
	public const string CoupleWithChildren = "Couple avec enfant";
	public const string CoupleWithoutChildren = "Couple sans enfants";
	public const string SingleParentFamily = "Famille monoparentale";
	public const string SinglePerson = "Personne seule";
	public const string SinglePersonWithAdultStaying = "Personne seule avec adulte hébergé";
}

public static class SocioProfessionalCategoryLabel
{
	public const string Artisan = "Artisan, commerçant, chef d'entreprise";
	public const string Cadre = "Cadre, profession intellectuelle supérieure";
	public const string Employee = "Employé(e)";
	public const string Farmer = "Agriculteur exploitant";
	public const string IntermediateProfession = "Profession intermédiaire";
	public const string Retired = "Retraité";
	public const string SearchingJob = "En recherche d'emploi";
	public const string Unemployed = "Sans activité professionnelle";
	public const string Worker = "Ouvrier";
}

public static class SocialProtectionFundLabel
{
	public const string Caf = "CAF";
	public const string Carsat = "CARSAT";
	public const string Cgss = "CGSS";
	public const string Cpam = "CPAM";
	public const string Msa = "MSA";
	public const string Other = "Autre caisse";
	public const string Rsi = "RSI";
	public const string Urssaf = "URSSAF";
}

public static class PensionFundLabel
{
	public const string AgriculturalSocialMutuality = "Mutualité sociale agricole (non salarié)";
	public const string AgriculturalSocialMutualityEmployee = "Mutualité sociale agricole (salarié agricole)";
	public const string Carsat = "CARSAT";
	public const string Cnav = "CNAV";
	public const string Cram = "CRAM";
	public const string Crav = "CRAB";

	public const string Other = "Autres régimes (EDF, SNCF, fonctionnaires ...)";

	public const string RetirementInsurance =
        "Assurance retraite (indépendants : commerçant, artisan, profession libérale ...)";
}

public static class AdditionalFundLabel
{
	public const string Agir = "AGIR";
	public const string Arrco = "ARRCO";
	public const string Cgss = "CGSS";
	public const string Ircantec = "IRCANTEX";
	public const string Other = "Autre caisse";
	public const string Rafp = "RAFP";
}

public static class OwnershipStatusLabel
{
	public const string CoOwner = "Copropriétaire";
	public const string DismemberedBarePropertyOnly = "Démembrée - nuepropriété uniquement";
	public const string DismemberedUsufructOnly = "Démembrée - usufruit uniquement";
	public const string FullOwnership = "Pleine propriété";
	public const string JointOwnership = "Indivision";
	public const string OccupantFreeOfCharge = "Occupant à titre gracieux";
	public const string OccupantWithoutRightAndTitle = "Occupant sans droit ni titre";
	public const string PrivateParkTenant = "Locataire parc privé";
	public const string PublicParkTenant = "Locataire parc social";
	public const string RealEstateCompany = "Société civile immobilière";
}

public static class HousingTypeLabel
{
	public const string IndividualHouse = "Maison individuelle";
	public const string ResidentialCollective = "Résidentiel collectif";
}

public static class HeatingTypeLabel
{
	public const string IndividualHeating = "Chauffage individuel";
	public const string CollectiveHeating = "Chauffage collectif";
}

public static class PerilTypeLabel
{
	public const string TechnicalReliability = "Fiabilité technique";
	public const string BudgetControl = "Maîtrise budgétaire";
	public const string DeadLineCompliance = "Respect des délais";
	public const string CollectiveAdhesion = "Adhésion collective";
	public const string RegulatoryCompliance = "Conformité réglementaire";
	public const string EnergyPerformance = "Performance énergétique réelle";
}

public static class NatureOfSyndicTypeLabel
{
	public const string ProfessionalType = "Professionnel";
	public const string BenevoleType = "Bénévole";
}

public static class ComfortLevelLabel
{
	public const string Bad = "Mauvais";
	public const string Good = "Bon";
	public const string Medium = "Moyen";
}

public static class EnergyDeprivationLabel
{
	public const string None = "Aucune";
	public const string Partial = "Partielle";
	public const string Total = "Totale";
}

public static class TrustedTierRoleLabel
{
	public const string FinancialMonitoring = "Suivi financier";
	public const string Identifier = "Identificateur";
	public const string MainContact = "Interlocuteur principal";
	public const string Other = "Autre";
	public const string SocialMonitoring = "Suivi social";
	public const string SocialWorker = "Travailleur social";
	public const string TechnicalMonitoring = "Suivi technique";
}

public static class DiagnosisLevelLabel
{
	public const string Bad = "Mauvais";
	public const string Good = "Bon";
	public const string Poor = "Médiocre";
	public const string VeryBad = "Très mauvais";
}

public static class RenovationTypeLabel
{
	public const string EfficientRenovationInStages = "Rénovation performante par étape";
	public const string MajorRenovation = "Rénovation d'ampleur selon l'ANAH";
}

public static class PartlyStateTreatmentLabel
{
	public const string No = "Non";
	public const string Partly = "Partiellement";
	public const string Yes = "Oui";
}

public static class AccompanyingTypeLabel
{
	public const string Diffuse = "Diffus";
	public const string Targeted = "Ciblé";
}

public static class PreWorkPlanTabLabel
{
	public static class RequiredFields
	{
		public const string EstimatedAnnualEnergyConsumptionAfterWork =
			"Veuillez saisir la consommation énergétique annuelle estimée après travaux";
		public const string EstimatedDpeAfterWork =
			"Veuilez sélectionner l'étiquette énergétique après travaux estimée.";

		public const string EstimatedGesAfterWork =
			"Veuilez sélectionner l'étiquette climatique après travaux estimée.";

		public const string ExistingHumidityAndVaporMigrationManagedAfterTreatment =
			"Veuilez indiquer si la gestion de l’humidité existante et de la migration de vapeur après isolation est traitée.";

		public const string IsRgeLabelUpToDate = "Veuilez indiquer si le label RGE est à jour.";
		public const string RenovationType = "Veuillez sélectionner le type de rénovation.";
		public const string TreatedAirTightness = "Veuilez indiquer si l'étanchéité à l'air est traitée.";
		public const string TreatedThermalBridge = "Veuilez indiquer si les ponts thermiques sont traités.";
	}
}

public static class CsvDataLabel
{
	public const string AccompanyingFileSuccessfullyLoaded = "Le dossier a bien été chargé.";
	public const string ImportAccompanyingFile = "Import des dossiers d'accompagnement CSV";
	public const string ImportV3AccompanyingFileTextButton = "Importer v3";
	public const string WaitingWhileExportingCsv =
		"⏳ L’export de vos données est en cours. Vous pouvez changer d’onglet pendant le traitement, mais ne fermez ni n'actualisez pas cette page tant que le téléchargement n’a pas commencé. Le fichier sera automatiquement téléchargé dès qu’il est prêt (cela peut prendre quelques secondes).";
	public const string NoValue = "0";
	public const string YesValue = "1";
	public const string LabelValueSeparator = ":";
	public const string ListElementsSeparator = ";";
	public const string AccompanyingFileSuccessfullyCreated = "Le dossier '{0}' (référence externe : '{1}') a été ajouté.";
	public const string AccompanyingFileSuccessfullyUpdated = "Le dossier '{0}' (référence externe : '{1}') a été mis à jour.";
	public const string OperatorApostrophe = "’";
	public const string MainApostrophe = "'";
	public const string NoAccompanyingFilesToCreate = "Les dossiers renseignés existent déjà, ils seront mis à jour.";
	public const string TransformedOsloCsvButton = "Formater le csv Oslo";
	public const string DownloadErrorMessages = "Télécharger les erreurs";
	public const string SuccessMessage  = "Traitement terminé. Le fichier csv généré est téléchargé automatiquement.";


    public static class CsvColumnsNames
	{
		public const string Reference = "Référence du dossier";
		public const string FirstEncounterDate = "Date de première visite";
		public const string StartOfAccompanyingDate = "Date de début d'accompagnement";
		public const string OpeningDate = "Date d'ouverture du dossier";
		public const string CloseDate = "Date de fermeture du dossier";
		public const string EndOfAccompanyingDate = "Date de fin d'accompagnement";
		public const string EndOfEncounterDate = "Date de fin de suivi";
		public const string ZeroEnergyExclusionTerritoriesProgram = "Programme Territoire Zéro Exclusion Energétique";
		public const string AccompanyingType = "Type d'accompagnement";
		public const string AccompanyingFileTerritory = "Territoire";
		public const string HouseholdTypology = "Typologie de famille";
		public const string IsFollowedByAnSocialWorker = "Le méange est-il suivi par un travailleur social";
		public const string HasAnOccupantWithDisabilities = "Situation de handicap dans le foyer";
		public const string HasAnOccupantWithLongTermIllness = "Personne en maladie longue durée";
		public const string HasAnOccupantWithIndependenceLoss = "Personne en perte d'autonomie dans le foyer";
		public const string HasAnOccupantUnderCuratorship = "Personne placée sous curatelle / sauvegarde de justice";
		public const string HasAnOccupantUnderGuardianship = "Personne placée sous tutelle";
		public const string ReferenceIncomeTax = "Revenu fiscal annuel de référence du ménage (€)";
		public const string SocialContext = "Description du context social";
		public const string HouseholdProject = "Projet de la famille";
		public const string HouseholdAvailabilityForVisits = "Disponibilités de la famille pour les visites";
		public const string CommentsOnHouseholdDifficulties = "Détails des difficultées rencontrées par le ménage";
		public const string HasOverdueInvoice = "Impayés de factures d'énergie depuis au moins 6 mois";
		public const string HouseholdDifficulties = "Difficultés rencontrées par la famille";
		public const string HouseholdExpenses = "Dépenses du ménage";
		public const string HouseholdResources = "Typologie de ressources mensuelles du ménage";
		public const string Birthdate = "Date de naissance";
		public const string SocioProfessionalCategory = "Catégorie Socioprofessionnel";
		public const string PhoneNumber = "Numéro de téléphone";
		public const string Email = "Email";
		public const string Job = "Profession";
		public const string SocialProtectionFund = "Caisse de protection social";
		public const string CommentOnSocialProtectionFund = "Commentaire sur la caisse de protection social";
		public const string PensionFundOccupant = "Fond de retraite";
		public const string CommentOnPensionFund = "Commentaire sur le fond de retraite";
		public const string AdditionnalFund = "Fond complémentaire";
		public const string CommentOnAdditionnalFund = "Commentaire sur le fond complémentaire";
		public const string FirstName = "Prénom";
		public const string LastName = "Nom";
		public const string GeographicAreaTypology = "Typologie de territoire";
		public const string IsInABFArea = "Zone ABF";
		public const string ArchitecturalOrTownPlanningStandards = "Normes architecturales ou d'urbanisme";
		public const string OwnershipStatus = "Statut de propriété";
		public const string HousingType = "Type de logement";
		public const string ConstructionYear = "Année de construction du logement";
		public const string LivingSpace = "Surface habitable";
		public const string NumberOfRoom = "Nombre de pièces du logement";
		public const string NumberOfFloor = "Nombre de niveau";
		public const string YearOfAcquisitionOrEntry = "Année d'acquisition/Entrée dans le logement";
		public const string CadastralReference = "Référence cadastral";
		public const string SunExposure = "Exposition au soleil";
		public const string NumberOfDoor = "Nombre de portes";
		public const string NumberOfWindow = "Nombre de fenêtres";
		public const string NumberOfPatioDoor = "Nombre de portes-fenêtres";
		public const string NumberOfRoofDoor = "Nombre de fenêtres de toits";
		public const string NumberOfBayWindow = "Nombre de baies vitrées";
		public const string CeilingHeight = "Hauteur sous plafond (m)";
		public const string HasPreviousWork = "Travaux antérieurs";
		public const string CommentOnPreviousWork = "Commentaire sur les travaux antérieurs";
		public const string Label = "Libellé de l'adresse";
		public const string PostalCode = "Code Postal";
		public const string City = "Ville";
		public const string Department = "Département";
		public const string Region = "Région";
		public const string AdditionnalComment = "Complément d'adresse";
		public const string DegradationIndex = "Indice de dégradation";
		public const string UnsanitaryCoefficient = "Coefficient d'insalubrité";
		public const string EnergyDepravation = "Privation d'énergie";
		public const string SummerThermalComfortLevel = "Niveau du confort thermique en été (manque de ventilation, refroidissement du logement difficile, …)";
		public const string WinterThermalComfortLevel = "Niveau du confort thermique en hiver (montée en température difficile, courants d'air, murs froids,…)";
		public const string NoiseComfortLevel = "Niveau du confort sonore";
		public const string HasPestOrMold = "Nuisibles/Moisissures";
		public const string HasFaultyElectricalSystem = "Système électrique défaillant";
		public const string HasVentilationSystem = "Système de ventilation";
		public const string HasHeatingSystem = "Système de chauffage";
		public const string HasHotWaterProduction = "Production d'eau chaude";
		public const string RoofingState = "Toitures";
		public const string HasOpenings = "Ouvertures";
		public const string HasHousingCover = "Couvertures";
		public const string DisordersObservedCommentary = "Commentaire sur les problèmes rencontrés avant travaux";
		public const string Dpe = "Etiquette DPE avant travaux";
		public const string Ges = "Etiquette GES avant travaux";
		public const string AnnualEnergyConsumption = "Consommation énergétique annuelle avant travaux (kWhEP/m²)";
		public const string AnnualGesEmission = "Émissions GES annuelles avant travaux (kgCO²e)";
		public const string HeatingEnergy = "Energie de chauffage avant travaux";
		public const string EstimatedDpeAfterWork = "Étiquette énergie après travaux estimée";
		public const string EstimatedAnnualEnergyConsumptionAfterWork = "Consommation énergétique annuelle après travaux (kWhEP/m²)";
		public const string EstimatedAnnualGesEmissionsAfterWork = "Emission GES annuelles estimées après travaux (TonnesEqCO²)";
		public const string EstimatedGesAfterWork = "Étiquette climat après travaux estimée";
		public const string RenovationType = "Type de rénovation";
		public const string NextStepAndVigilancePoint = "Prochaine(s) étape(s) et points de vigilance";
		public const string HasInterestInPossibleARAProcess = "Intérêt pour la démarche auto-réhabilitation accompagnée (ARA) éventuelle";
		public const string HasNeedForTemporaryReHousing = "Besoin d'une solution de relogement temporaire";
		public const string HasEmergencyWorks = "Travaux d'urgences (pré plan de travaux)";
		public const string HasEnergeticsRennovationWorks = "Travaux de rénovation énergétique";
		public const string HasInducedWorks = "Travaux induits";
		public const string HasSafetyAndHealthWorks = "Travaux de sécurité et salubrité";
		public const string TreatedAirTightness = "Étanchéité à l'air traitée";
		public const string TreatedThermalBridge = "Ponts thermiques traités (pré plan de travaux)";
		public const string AreExistingHumidityAndVaporMigrationManagedAfterTreatment = "Gestion de l’humidité existante et de la migration de vapeur après isolation traitée (pré plan de travaux)";
		public const string IsHouseholdReadyToStartARAProcess = "La famille est prête à s'engager dans la démarche ARA";
		public const string AreHouseholdPhysicalCapacitiesTakenIntoAccount = "Prise en compte des capacités physiques de la famille";
		public const string DoHouseholdCanMobilizeSocialCircleOnConstructionSite = "La famille peut mobiliser son entourage sur le chantier";
		public const string WorksDetails = "Précisions sur les travaux";
		public const string HouseholdAvailabilitiyToOrganizeARASite = "Disponibilités de la famille pour organiser le chantier ARA";
		public const string IsRgeLabelUpToDate = "Label RGE à jour";
		public const string OtherQualification = "Autres qualifications";
		public const string ProjectType = "Type de projet";
		public const string InsuranceType = "Type d'assurance";
		public const string MaPrimeRenovGuidedPath = "MaPrimeRénov' Parcours Accompagné (€)";
		public const string MaPrimeRenovCoOwnerShip = "MaPrimeRénov' Copropriété (€)";
		public const string MaPrimeLogementDecent = "Ma Prime Logement Décent (€)";
		public const string MaPrimeAdapt = "MaPrimeAdapt' (€)";
		public const string BonusForExitingEnergeticSieve = "Bonus sortie de passoire thermique (€)";
		public const string RegionalAids = "Région (€)";
		public const string DepartmentalAids = "Département (€)";
		public const string PublicEstablishmentsForInterCommunalCooperationAids = "Etablissements publics de coopération intercommunale/Agglomération (€)";
		public const string MunicipalityAids = "Commune (€)";
		public const string SolicitedBankLoanType = "Quel est le type de prêt bancaire sollicité ?";
		public const string NewBorrowingCapacity = "Nouvelle capacité d'emprunt (€)";
		public const string ClassicBankLoan = "Prêt bancaire classique (€)";
		public const string EcoPtz = "Eco-PTZ (€)";
		public const string MdphFinancing = "MDPH (€)";
		public const string CeeFinancing = "CEE (€)";
		public const string CafMsaFinancing = "CAF/MSA";
		public const string PensionFund = "Caisse de retraite";
		public const string UnderprivilegedHousingFoundation = "Fondation Abbé Pierre (€)";
		public const string LeroyMerlinFoundation = "Fondation Leroy Merlin (€)";
		public const string WattForChangeFoundation = "Fondation Watt For Change (€)";
		public const string SocialProtectionGroup = "Groupe de protection sociale (€)";
		public const string HouseholdMaximumSavingAmountForRenovationProject = "Montant maximal des économies du foyer alloué à leur projet de rénovation (€)";
		public const string OtherFamilyMemberMaximumSupportAmountForRenovationProject = "Montant maximal du soutien des autres membres de la famille alloué au projet de rénovation (€)";
		public const string FundingModeLabel = "Mode de financement";
		public const string AccompanyingCost = "Coût de l'accompagnement (HT)";
		public const string HouseholdSelfFinancing = "Autofinancement du ménage";
		public const string IntermediateAirtightnessTestResult = "Résultat du test intermédiaire d’étanchéité à l’air si rénovation globale";
		public const string JustificationAndActionsPutInPlaceIfNoTest = "Si pas de test, justification et actions mises en place pour le traitement de l’étanchéité à terme";
		public const string EffectiveComplianceWithWorkRecommendations = "Respect effectif des préconisations de travaux (comparaison devis / factures)";
		public const string HasWorksEnabledHouseholdToStayAtHome = "Travaux ayant permis le maintien à domicile";
		public const string WellBeingRating = "Notation relative au bien-être";
		public const string EducationalFrameworkRating = "Notation relative au cadre éducatif";
		public const string FamilySatisfaction = "Satisfaction de la famille concernant l'accompagnement";
		public const string ReturnToEmployment = "Retour à l'emploi (si recherche d'emploi au début de l'accompagnement)";
		public const string HasHousingAdaptationWorks = "Travaux d'adaptation du logement";
		public const string HasFinishingWorks = "Travaux de finition";
		public const string HasSafetyWorks = "Travaux de sécurisation";
		public const string HasPreparationWorks = "Travaux de préparation";
		public const string HasEmergencyWorksMonitoring = "Travaux d'urgence";
		public const string HasUnsanitaryExit = "Sortie d'insalubrité";
		public const string TreatedAirTightnessWorks = "Étanchéité à l’air traitée";
		public const string TreatedThermalBridgesWorks = "Ponts thermiques traités";
		public const string HasHumidityManagement = "Gestion de l’humidité existante et de la migration de vapeur après isolation traitée";
		public const string WorkTotalCost = "Coût total des travaux (€) TTC";
		public const string SolidarBuilder = "Ensemblier Solidaire Référent";
		public const string MarkerNature = "Nature du repérant";
		public const string CommentOnMarkerNature = "Autre";
		public const string IsDeleted = "Dossier à supprimer";
		public const string AccompanyingTimeDurationForIdentificationMilestone = "Temps d'accompagnement pour les ménages au jalon 1";
		public const string AccompanyingTimeDurationForOrganizeAndFinanceMilestone = "Temps d'accompagnement pour les ménages au jalon 2";
		public const string AccompanyingTimeDurationForRealizeAndFollowMilestone = "Temps d'accompagnement pour les ménages au jalon 3";
		public const string NumberOfOccupants = "Nombre d'occupants du ménage";
		public const string MonthlyEnergeticsExpenses = "Dépenses d’énergie mensuelles (€)";
		public const string StopEnergyExclusionFunds = "Fonds Stop Exclusion Énergétique";
	}

	public static class Errors
	{
		public const string NoDataToImport = "Aucun dossier valide ne peut être importé.";
		public const string ErrorWhileCreatingAccompanyingFiles = "Erreur technique lors de la création des dossiers, aucun dossier n'a été créé.";
		public const string FormatError = "le champ '{0}' avec la valeur '{1}' n'est pas au format attendu.";
		public const string ErrorWhileUpdatingAccompanyingFile = "erreur technique lors de la mise à jour du dossier {0}.";
		public const string AccompanyingFileNotFoundInDataBase = "le dossier '{0}' n'existe pas dans l'application.";
		public const string DuplicatedExternalReference = "la référence externe '{0}' est dupliquée dans le fichier csv. Seule la première occurrence sera conservée.";
		public const string IncorrectNumberOfFieldsOnLine = "{0} champ(s) renseigné(s) (attendu(s) {1}).";
		public const string UnknownError = "une erreur non gérée s'est produite.";
		public const string RequiredFieldToCreateAccompanyingFile = "le champ '{0}' est requis pour la création du dossier.";
		public const string NoAccompanyingFilesCreated = "Aucun dossier n'a pu être créé. Veuillez vérifier les données dans le fichier csv.";
		public const string NoAccompanyingFileUpdated = "une erreur en base de donnée est survenue pendant la mise à jour du dossier '{0}'.";
		public const string InvalidSolidarBuilder = "L'Ensemblier solidaire renseigné pour le dossier '{0}' n'existe pas dans l'application, impossible de créer ce dossier";
		public const string NoSolidarBuilder = "Aucun Ensemblier solidaire n'a été renseigné pour le dossier '{0}'";
		public const string ServerError = "Une erreur inconnue est survenue côté serveur, veuillez réessayer.";
		public const string ErrorOnLine = "Erreur sur la ligne {0}, id - {1} : {2}.";
		public const string ErrorOnLineWithoutId = "Erreur sur la ligne {0} : {1}.";
		public const string ErrorWhileCreatingErrorsReporting = "Une erreur en base de données est survenue lors de la création du reporting des erreurs, veuillez réessayer.";
	}

	public static class OsloCsvColumns
	{
		public const string ProjetStatus = "projet_status";
		public const string ProjetId = "projet_id";
		public const string ProjectVisitDate = "projet_date_visite";
		public const string ApplicantContactDate = "demandeur.date_contact";
		public const string ControlVisitToScheduleDate = "date_visite_controle_a_programmer";
		public const string SettlementDate = "date_solde";
		public const string ProjectNationalPartners = "projet_partenaires_nationaux";
		public const string ProjectCity = "projet_ville";
		public const string RfrAmounts = "rfr_montants";
		public const string RfrYears = "rfr_annees";
		public const string ApplicantBirthDate = "demandeur.date_naissance";
		public const string ApplicantProfession = "demandeur.profession";
		public const string ApplicantPhones = "demandeur_telephones";
		public const string ApplicantEmail = "demandeur.email";
		public const string ApplicantFirstName = "demandeur.prenom";
		public const string ApplicantLastName = "demandeur.nom";
		public const string ApplicantAdultCount = "demandeur.nombre_adultes";
		public const string ApplicantMinorCount = "demandeur.nombre_mineurs";
		public const string HousingType = "logement.type";
		public const string HousingSurface = "logement.surface_hab";
		public const string HousingPurchaseYear = "logement.annee_achat";
		public const string ProjectAddress = "projet_adresse";
		public const string HousingConstructionYear = "logement.annee_construction";
		public const string ProjectZipCity = "projet_cpville";
		public const string ApplicantCadastralReference = "demandeur.reference_cadastrale";
		public const string HousingInitialEnergyClass = "logement.classe_energie_initiale";
		public const string HousingInitialGesClass = "logement.classe_GES_initiale";
		public const string HousingInitialEnergyConsumption = "logement.conso_NRJ_initiale";
		public const string HousingInitialGesEmission = "logement.emission_GES_initiale";
		public const string HousingTargetEnergyClass = "logement.classe_energie_vise";
		public const string HousingTargetEnergyConsumption = "logement.conso_NRJ_visee";
		public const string HousingTargetGesEmission = "logement.emission_GES_visee";
		public const string HousingTargetGesClass = "logement.classe_GES_visee";
		public const string Referents = "referents";
	}

	public static class OsloCsvDataColumn
	{
		public const string HousingTypeHome = "Maison";
		public const string HousingTypeApartment = "Appartement";
		public const string HousingTypeOther = "Autre";
		public const string HousingTypebuilding = "Immeuble";
		public const string HousingTypeLocalCommercial = "Local commercial";
		public const string Employee = "Salarié";
		public const string RetiredOther = "Retraité autre";
		public const string IndependentWorker = "Travailleur Indépendant";
		public const string PublicServant = "Fonction publique";
		public const string Unemployed = "Sans emploi";
		public const string Student = "Étudiant";
		public const string Disabled = "Invalidité";
	}

    public static class OsloDepartments
    {
		public const string D22Code = "22";
		public const string D67Code = "67";
		public const string D75Code = "75";
		public const string D93Code = "93";

        public const string D22Name = "Kreiz Breizh";
        public const string D22Display = "Côtes-d’Armor (22)";
        public const string D22Region = "Bretagne";

        public const string D67Name = "Strasbourg";
        public const string D67Display = "Bas-Rhin (67)";
        public const string D67Region = "Grand Est";

        public const string D75Name = "Paris";
        public const string D75Display = "Paris (75)";
        public const string D75Region = "Île-de-France";

        public const string D93Name = "Grand Paris Grand Est";
        public const string D93Display = "Seine-Saint-Denis (93)";
        public const string D93Region = "Île-de-France";
    }

    public static class OsloCsvErrors
	{
		public const string RequiredFieldToCreateAccompanyingFile = "le champ '{0}' est requis pour la création du dossier.";
		public const string UnknownError = "une erreur non gérée s'est produite.";
		public const string NullProfession = "Profession vide ou nulle";
		public const string UnknownProfession = "Profession inconnue : '{0}'";
		public const string NullHousingType = "Type de logement vide ou nulle";
		public const string UnknownHousingType = "Type de logement inconnue: '{0}'";
		public const string UnknownHouseholdTypology = "Typologie non définie pour nbAdultes={0}, nbMineurs={1}";
		public const string NullIncomingTax = "Revenu fiscal : valeur vide ou nulle";
		public const string InvalidIncomingTax = "Revenu fiscal : valeur invalide '{0}'";
		public const string UnknownPostalCode = "Code postal inconnu";
		public const string UnknownTerritory = "Territoire inconnu pour ce code: {0}";
    }
}

public static class ExcelDataLabel
{
	public const string AccompanyingFileSuccessfullyLoaded = "Le dossier a bien été chargé.";
	public const string ImportAccompanyingFile = "Import des dossiers d'accompagnement";
	public const string ImportExcelDataText =
		"Pour pouvoir importer un dossier déjà existant vous devez remplir les champs suivants";
	public const string ImportExcelFile = "Veuillez charger le dossier d'accompagnement";
	public const string ImportOsloCsvFile = "Veuillez charger le fichier csv source";
	public const string ImportV2AccompanyingFileTextButton = "Importer v2";
	public const string ImportV3AccompanyingFileTextButton = "Importer v3";
	public const string WaitingWhileExportingExcel =
		"⏳ L’export de vos données est en cours. Vous pouvez changer d’onglet pendant le traitement, mais ne fermez ni n'actualisez pas cette page tant que le téléchargement n’a pas commencé. Le fichier sera automatiquement téléchargé dès qu’il est prêt (cela peut prendre quelques secondes).";
	public const string RgpdMention = "Document confidentiel – Données personnelles – usage strictement interne";
	public const string RgpdMessage = "Ces données sont confidentielles. Leur utilisation est strictement limitée au cadre du programme TZEE. Vous êtes responsable de leur conservation et de leur suppression lorsqu’elles ne sont plus nécessaires.";

	public static class IdentificationMilestone
	{
		public const string AdditionalAddress = "Complément d'adresse";
		public const string AnahCategory = "Catégorie du ménage selon l'ANAH";
		public const string AnnualEnergyConsumptionBeforeWork =
			"Consommation énergétique annuelle avant travaux (kWh/ m²)";
		public const string CommentOnMarkerNature = ">> si autre, précisez ici :";
		public const string ConstructionYear = "Année de construction du logement";
		public const string ContactWithFamily = "Nombre de contacts avec la famille (présentiel ou à distance)";
		public const string DegradationIndex = "Indice de dégradation";
		public const string Department = "Département";
		public const string EnergyDeprivation = "Privation d’énergie";
		public const string FirstContactDate = "Date de la première visite";
		public const string GeographicalHousingAreaTypology = "Typologie de territoire";
		public const string HasOverdueInvoice = "Impayés de factures d'énergie depuis au moins 6 mois";
		public const string HouseholdTypology = "Typologie de famille";
		public const string HousingType = "Type de logement";
		public const string IncomeTaxReference = "Revenu fiscal annuel de référence du foyer (€)";
		public const string LivingSpace = "Surface habitable (m²)";
		public const string MainOccupant = "Occupant principal";
		public const string MainOccupantDateOfBirth = "Date de naissance";
		public const string MainOccupantSocialProfessionalCategory =
			"Catégorie socio-professionnelle de l'occupant principal";
		public const string MarkerNature = "Nature du repérant";
		public const string Milestone = "Statut d'avancement du dossier";
		public const string MilestoneValidation =
			"Validation Jalon 1  Pièce justificative (contrat d'accompagnement du ménage signé) fournie";
		public const string Municipality = "Commune";
		public const string OwnershipStatus = "Statut de propriété";
		public const string PostalCode = "Code postal";
		public const string Reference = "Référence interne";
		public const string Region = "Région";
		public const string SocialContext = "Description du contexte social";
		public const string SolidarBuilderFullName = "Ensemblier·ère Solidaire référent·e";
		public const string StartingDpe = "Etiquette énergétique de départ";
		public const string StartSupportDate = "Date de début d'accompagnement";
		public const string StreetNumberName = "Numéro et nom de rue";
		public const string Structure = "Opérateur";
		public const string Trigram = "Trigramme";
		public const string UnsanitaryCoefficient = "Coefficient d'insalubrité";
	}

	public static class OrganizeAndFinanceMilestone
	{
		public const string AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
			"Gestion de l’humidité existante et de la migration de vapeur après isolation traitée";
		public const string ContactWithFamily = "Nombre de contacts avec la famille (présentiel ou à distance)";
		public const string DepartmentAids = "Département (€)";
		public const string EnergeticEfficiency = "Efficacité énergétique";
		public const string EstimatedDpeJumpClass = "Saut de classe énergétique estimé";
		public const string EstimatedDpeLabelAfterWork = "Etiquette énergétique après travaux estimée";
		public const string EstimatedEnergyConsumptionAfterWork =
			"Consommation énergétique annuelle estimée après travaux (kWh/m²)";
		public const string EstimatedRemainingAmount = "Reste à charge estimé (€)";
		public const string FightAgainstSubstandardHousing = "Lutte contre l'habitat indigne";
		public const string FinishingWork = "Travaux de finition";
		public const string HousingAdaptationWork = "Travaux d'adaptation du logement";
		public const string IsEmergencyWorks = "Travaux d'urgence";
		public const string HouseholdMaximumSavingAmountForRenovationProject =
			"Montant maximal des économies du foyer alloué à leur projet de rénovation (€)";
		public const string MaximumAmountSupportFamilyMembersRenovationProject =
			"Montant maximal du soutien des autres membres de la famille alloué au projet de rénovation (€)";
		public const string MilestoneValidation =
			"Validation Jalon 2 Pièce justificative (notification d'octroi des aides ANAH) fournie";
		public const string MunicipalityAids = "Commune (€)";
		public const string NextStepAndVigilancePoints =
			"Prochaine(s) étape(s) et points de vigilance (si rénovation performante par étape)";
		public const string PensionFunds = "Caisses de retraite (€)";
		public const string PreparationWork = "Travaux de préparation";
		public const string PrivateActors = "Acteurs privés (€)";
		public const string PublicEstablishmentsIntercommunalCooperation =
			"Etablissements publics de coopération intercommunale/Agglomération (€)";
		public const string RegionAids = "Région (€)";
		public const string RenovationType = "Type de rénovation";
		public const string SecurityWork = "Travaux de sécurisation";
		public const string TreatedAirTightness = "Étanchéité à l’air traitée";
		public const string TreatedThermalBridge = "Ponts thermiques traités";
		public const string UnsanitaryExit = "Sortie d'insalubrité";
	}

	public static class RealizeAndFollowMilestone
	{
		public const string BilledWorkForce = "dont main d'œuvre facturée en €";
		public const string ContactWithFamily = "Nombre de contacts avec la famille (présentiel ou à distance)";
		public const string EducationalFrameworkRating = "Notation relative au cadre éducatif";
		public const string EffectiveComplianceWithWorkRecommendations =
			"Respect effectif des préconisations de travaux (comparaison devis / factures)";
		public const string EndOfAccompanyingDate = "Date de fin d'accompagnement (réception des travaux)";
		public const string EndOfEncounterDate = "Date de fin de suivi";
		public const string FamilySatisfactionWithSupport = "Satisfaction de la famille concernant l'accompagnement";
		public const string HasWorkEnablingHomeSupport = "Travaux ayant permis le maintien à domicile";
		public const string HouseholdAutoFinancing = "Capacité d’autofinancement du ménage en €";
		public const string IntermediateAirtightnessTestResult =
			"Résultat du test intermédiaire d’étanchéité à l’air si rénovation globale";
		public const string IsBackToEmployment =
			"Retour à l'emploi (si recherche d'emploi au début de l'accompagnement)";
		public const string MilestoneValidation =
			"Validation Jalon 3 Pièces justificatives (PV de réception des travaux et rapport d'accompagnement du ménage) fournies";
		public const string TotalCost = "Coût total des travaux par lots (factures) en €";
		public const string WaterproofingTreatmentActions =
			"Si pas de test, justification et actions mises en place pour le traitement de l’étanchéité à terme";
		public const string WellBeingRating = "Notation relative au bien-être";
	}

	public static class Errors
	{
		public const string ErrorWhileGeneratingExcelFile =
			"Une erreur est survenue pendant l'export, veuillez réessayer.";
		public const string FirstJalonError = "Erreur lors de la lecture du jalon 1.";
		public const string RequiredMainOccupantDateOfBirth =
			"Le champ \"Date de naissance\" de l'occupant principal est requis.";
		public const string RequiredMainOccupantSocioProfessionalCategory =
			"Le champ \"Catégorie socioprofessionelle\" de l'occupant principal est requis.";
		public const string RequiredMainOccupantTrigram = "Le champ \"Trigramme\" de l'occupant principal est requis.";
		public const string RequiredSolidarBuilderName = "Le champ \"Ensemblier·ère Solidaire référent·e\" est requis.";
		public const string SolidarBuilderNotFound = "\"L'Ensemblier·ère Solidaire sélectionné·e est introuvable.\"";
		public const string UnknownMilestone = "Jalon non reconnu.";
		public const string WhileReadingFile =
			"Erreur lors de la lecture du fichier. Veuillez vérifier le format du fichier.";
		public const string WhileSavingFolder = "Erreur lors de l'enregistrement du dossier.";
	}
}

public static class Icons
{
	public const string Account = "account_box";
	public const string AddButton = "person_add";
	public const string AscendantArrow = "/images/menu/ascendant-arrow.png";
	public const string CancelFilter = "/images/cancel-filter.svg";
	public const string DeleteAccount = "/images/deleteAccount.png";
	public const string DeleteButton = "delete_forever";
	public const string DescendantArrow = "/images/menu/descendant-arrow.png";
	public const string Euro = "euro";
	public const string FileLoaded = "/images/file_loaded.png";
	public const string Logout = "logout";
	public const string ShowPersonalInfo = "/images/showPersonalInfo.png";
	public const string Trash = "delete";
	public const string UploadIcon = "/images/upload-icon.svg";

	public static class Svg
	{
		public const string AccompanyingFileCreation =
			"<svg preserveAspectRatio='xMinYMin meet' width='21' height='19' viewBox='0 0 21 19' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M11 15C11 15.34 11.04 15.67 11.09 16H2C1.46957 16 0.960859 15.7893 0.585786 15.4142C0.210714 15.0391 0 14.5304 0 14V2C0 0.89 0.89 0 2 0H8L10 2H18C18.5304 2 19.0391 2.21071 19.4142 2.58579C19.7893 2.96086 20 3.46957 20 4V9.81C19.39 9.46 18.72 9.22 18 9.09V4H2V14H11.09C11.04 14.33 11 14.66 11 15ZM18 14V11H16V14H13V16H16V19H18V16H21V14H18Z'/></svg>";
		public const string AccompanyingFileList =
			"<svg preserveAspectRatio='xMinYMin meet' width='20' height='16' viewBox='0 0 20 16' xmlns='http://www.w3.org/2000/svg'><path d='M18 2H10L8 0H2C0.9 0 0.00999999 0.9 0.00999999 2L0 14C0 15.1 0.9 16 2 16H18C19.1 16 20 15.1 20 14V4C20 2.9 19.1 2 18 2ZM18 14H2V4H18V14Z'/></svg>";
		public const string Address =
			"<svg preserveAspectRatio='xMinYMin meet' id='address' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,63.6c-6.8,0-12.4-5.5-12.4-12.4s5.5-12.4,12.4-12.4,12.4,5.5,12.4,12.4-5.5,12.4-12.4,12.4ZM60.5,43.9c-4.1,0-7.4,3.3-7.4,7.4s3.3,7.4,7.4,7.4,7.4-3.3,7.4-7.4-3.3-7.4-7.4-7.4Z'/><path d='M60.5,100.2c-9.3,0-30.4-25.1-30.4-48.9s13.7-30.4,30.4-30.4,30.4,13.7,30.4,30.4c0,23.9-21.1,48.9-30.4,48.9ZM60.5,25.8c-14,0-25.4,11.4-25.4,25.4,0,23.1,20.8,43.9,25.4,43.9s25.4-20.8,25.4-43.9-11.4-25.4-25.4-25.4Z'/></svg>";
		public const string Alert =
			"<svg preserveAspectRatio='xMinYMin meet' id='alert' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M24.2,38.9c-.9,0-1.7-.4-2.2-1.2-.7-1.2-.3-2.7.9-3.4l16.3-9.6c2.2-1.3,4.5-2.4,6.7-3.2,1.3-.5,2.7.2,3.2,1.5.5,1.3-.2,2.7-1.5,3.2-2,.7-3.9,1.6-5.9,2.8l-16.3,9.6c-.4.2-.8.3-1.3.3Z'/><path d='M84.8,99.5c-.4,0-.9-.1-1.3-.3-1.2-.7-1.6-2.2-.9-3.4l9.6-16.3c1.1-1.9,2-3.9,2.8-5.9.5-1.3,1.9-2,3.2-1.5,1.3.5,2,1.9,1.5,3.2-.8,2.3-1.9,4.5-3.2,6.7l-9.6,16.3c-.5.8-1.3,1.2-2.2,1.2Z'/><path d='M61.9,99.9c-3.7,0-7.2-1.5-9.9-4.1l-26.6-26.6c-3.1-3.1-4.6-7.4-4-11.7.6-4.4,3.1-8.1,6.8-10.3l18-10.5c11-6.4,25-4.6,34,4.4h0s0,0,0,0c9,9,10.8,23,4.4,34l-10.5,18c-2.2,3.8-6,6.3-10.3,6.8-.6,0-1.2.1-1.8.1ZM60.4,37.8c-4,0-8,1-11.6,3.2l-18,10.5c-2.4,1.4-4,3.9-4.4,6.7-.4,2.8.6,5.6,2.6,7.6l26.6,26.6c2,2,4.7,2.9,7.6,2.6,2.8-.4,5.2-2,6.7-4.4l10.5-18c5.3-9,3.8-20.5-3.6-27.9h0c-4.4-4.4-10.3-6.7-16.3-6.7Z'/><path d='M40.5,96.5c-4,0-8.1-1.5-11.2-4.6h0s0,0,0,0c-3-3-4.6-6.9-4.6-11.2s1.6-8.2,4.6-11.2c1-1,2.6-1,3.5,0l18.8,18.8c1,1,1,2.6,0,3.5-3.1,3.1-7.1,4.6-11.2,4.6ZM31.3,75.1c-1,1.7-1.6,3.6-1.6,5.7,0,2.9,1.1,5.6,3.2,7.6h0c3.6,3.6,9.1,4.1,13.3,1.6l-14.8-14.8Z'/><path d='M78.4,45.3c-.6,0-1.3-.2-1.8-.7-1-1-1-2.6,0-3.5l5.1-5.1c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-5.1,5.1c-.5.5-1.1.7-1.8.7Z'/></svg>";
		public const string ApproveFill =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' id='approve' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M51.7,80.6c-.6,0-1.3-.2-1.8-.7l-17.5-17.5c-1-1-1-2.6,0-3.5,1-1,2.6-1,3.5,0l15.7,15.7,33.2-33.2c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-35,35c-.5.5-1.1.7-1.8.7Z'/></svg>";
		public const string Arrow =
			"<svg preserveAspectRatio='xMinYMin meet' id='arrow' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M52.8,77.3c-.8,0-1.5-.3-2.1-.9-1.2-1.2-1.2-3,0-4.2l12.5-12.5c0,0,.1-.2.1-.3s0-.2-.1-.3l-12.5-12.5c-1.2-1.2-1.2-3,0-4.2,1.2-1.2,3-1.2,4.2,0l12.5,12.5c1.2,1.2,1.8,2.8,1.8,4.5s-.7,3.3-1.8,4.5l-12.5,12.5c-.6.6-1.3.9-2.1.9Z'/><path d='M59.5,102c-23.4,0-42.5-19.1-42.5-42.5S36.1,17,59.5,17s42.5,19.1,42.5,42.5-19.1,42.5-42.5,42.5ZM59.5,22.9c-20.2,0-36.6,16.4-36.6,36.6s16.4,36.6,36.6,36.6,36.6-16.4,36.6-36.6-16.4-36.6-36.6-36.6Z'/></svg>";
		public const string Back =
			"<svg preserveAspectRatio='xMinYMin meet' id='back' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M96.5,93.8c-1.4,0-2.5-1.1-2.5-2.5,0-11-9-20-20-20h-20.3v10c0,1-.6,1.9-1.5,2.3-.9.4-2,.2-2.7-.5l-26.8-25.7c-.5-.5-.8-1.1-.8-1.8s.3-1.3.8-1.8l26.8-25.7c.7-.7,1.8-.9,2.7-.5.9.4,1.5,1.3,1.5,2.3v10h15.3c16.6,0,30,13.5,30,30v21.5c0,1.4-1.1,2.5-2.5,2.5ZM51.2,66.4h22.8c8.1,0,15.4,3.9,20,10v-6.5c0-13.8-11.2-25-25-25h-17.8c-1.4,0-2.5-1.1-2.5-2.5v-6.6l-20.7,19.9,20.7,19.9v-6.6c0-1.4,1.1-2.5,2.5-2.5Z'/></svg>";
		public const string Burger =
			"<svg preserveAspectRatio='xMinYMin meet' id='burger' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><rect x='25.1' y='20.4' width='69.5' height='10.3'/><rect x='25.1' y='39.4' width='69.5' height='10.3'/><rect x='25.1' y='58.4' width='69.5' height='10.3'/><g> <path d='M36.3,81.5l4.5,12.1,4.5-12.1h3.5v15.7h-2.7v-5.2l.3-6.9-4.6,12.1h-2l-4.6-12.1.3,6.9v5.2h-2.7v-15.7h3.5Z'/><path d='M61.3,90.2h-6.5v4.8h7.5v2.2h-10.3v-15.7h10.2v2.2h-7.5v4.3h6.5v2.2Z'/><path d='M77.1,97.2h-2.7l-7-11.2v11.2h-2.7v-15.7h2.7l7,11.2v-11.2h2.7v15.7Z'/><path d='M91.8,81.5v10.5c0,1.7-.5,3-1.6,4-1.1,1-2.5,1.5-4.3,1.5s-3.2-.5-4.3-1.4-1.6-2.3-1.6-4v-10.5h2.7v10.5c0,1.1.3,1.9.8,2.4.5.6,1.3.8,2.4.8,2.1,0,3.2-1.1,3.2-3.3v-10.4h2.7Z'/></g></svg>";
		public const string ConfirmDeleteFill =
			"<svg preserveAspectRatio='xMinYMin meet' id='confirmdelete' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M88.7,42h-56.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h56.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M77.3,96.6h-33.6c-3.4,0-6.2-2.8-6.2-6.2v-50.8c0-1.4,1.1-2.5,2.5-2.5h41.1c1.4,0,2.5,1.1,2.5,2.5v50.8c0,3.4-2.8,6.2-6.2,6.2ZM42.5,42v48.3c0,.7.6,1.2,1.2,1.2h33.6c.7,0,1.2-.6,1.2-1.2v-48.3h-36.1Z'/><path d='M74.5,42h-27.9c-1.4,0-2.5-1.1-2.5-2.5v-7.1c0-4.4,3.6-8,8-8h17c4.4,0,8,3.6,8,8v7.1c0,1.4-1.1,2.5-2.5,2.5ZM49,37h22.9v-4.6c0-1.6-1.3-3-3-3h-17c-1.6,0-3,1.3-3,3v4.6Z'/><path d='M67.3,77.3c-1.4,0-2.5-1.1-2.5-2.5v-16.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v16.1c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M53.7,77.3c-1.4,0-2.5-1.1-2.5-2.5v-16.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v16.1c0,1.4-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string Create =
			"<svg preserveAspectRatio='xMinYMin meet' id='create' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M83.5,108h-46.3c-7.9,0-14.3-6.4-14.3-14.2V23.4c0-7.9,6.4-14.3,14.3-14.3h19.1c3.2,0,6.3.7,9.1,2.1,2.2,1,4.2,2.4,6,4.2l20,19.9c1.8,1.8,3.2,3.8,4.3,6.1,1.3,2.8,2,5.9,2,9v43.2c0,7.9-6.4,14.3-14.3,14.3ZM37.2,15.5c-4.4,0-7.9,3.5-7.9,7.9v70.4c0,4.3,3.5,7.9,7.9,7.9h46.3c4.4,0,7.9-3.5,7.9-7.9v-43.2c0-2.2-.5-4.4-1.4-6.3-.7-1.6-1.8-3.1-3-4.3l-20-19.9c-1.3-1.3-2.7-2.3-4.2-3-2.1-1-4.2-1.5-6.4-1.5h-19.1Z'/><path d='M92.8,46h-28.8c-1.8,0-3.2-1.4-3.2-3.2V14.1c0-1.1.5-2.1,1.4-2.7s2-.7,3-.2c2.3,1,4.3,2.4,6.1,4.3l20,19.9c1.8,1.8,3.2,3.8,4.3,6.1.5,1,.4,2.1-.2,3-.6.9-1.6,1.5-2.7,1.5ZM67.2,39.7h19.5l-19.5-19.5v19.5Z'/><path d='M69.4,72.5h-21.5c-1.8,0-3.2-1.4-3.2-3.2s1.4-3.2,3.2-3.2h21.5c1.8,0,3.2,1.4,3.2,3.2s-1.4,3.2-3.2,3.2Z'/><path d='M58.7,83.2c-1.8,0-3.2-1.4-3.2-3.2v-21.5c0-1.8,1.4-3.2,3.2-3.2s3.2,1.4,3.2,3.2v21.5c0,1.8-1.4,3.2-3.2,3.2Z'/></svg>";
		public const string DeleteFill =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' id='delete' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M38,85.6c-.6,0-1.3-.2-1.8-.7-1-1-1-2.6,0-3.5l44.9-44.9c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-44.9,44.9c-.5.5-1.1.7-1.8.7Z'/><path d='M83,85.6c-.6,0-1.3-.2-1.8-.7l-44.9-44.9c-1-1-1-2.6,0-3.5,1-1,2.6-1,3.5,0l44.9,44.9c1,1,1,2.6,0,3.5-.5.5-1.1.7-1.8.7Z'/></svg>";
		public const string Doc =
			"<svg preserveAspectRatio='xMinYMin meet' id='doc' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M42.1,94.1h-10.2c-2.1,0-3.7-1.7-3.7-3.7V28.8c0-2.1,1.7-3.7,3.7-3.7h10.2c2.1,0,3.7,1.7,3.7,3.7v61.6c0,2.1-1.7,3.7-3.7,3.7ZM32.7,89.6h8.5V29.6h-8.5v60Z'/><path d='M43.5,79.9h-13.1c-1.3,0-2.3-1-2.3-2.3v-36.1c0-1.3,1-2.3,2.3-2.3h13.1c1.3,0,2.3,1,2.3,2.3v36.1c0,1.3-1,2.3-2.3,2.3ZM32.7,75.4h8.5v-31.6h-8.5v31.6Z'/><g><path d='M55.2,94.1h-10.2c-2.1,0-3.7-1.7-3.7-3.7V28.8c0-2.1,1.7-3.7,3.7-3.7h10.2c2.1,0,3.7,1.7,3.7,3.7v61.6c0,2.1-1.7,3.7-3.7,3.7ZM45.8,89.6h8.5V29.6h-8.5v60Z'/><path d='M56.6,79.9h-13.1c-1.3,0-2.3-1-2.3-2.3v-36.1c0-1.3,1-2.3,2.3-2.3h13.1c1.3,0,2.3,1,2.3,2.3v36.1c0,1.3-1,2.3-2.3,2.3ZM45.8,75.4h8.5v-31.6h-8.5v31.6Z'/></g><g><path d='M82.7,94.9c-.5,0-1,0-1.4-.3-.9-.4-1.6-1.1-2-2l-23.5-56.9c-.8-1.9.1-4.1,2-4.9l9.4-3.9c.9-.4,1.9-.4,2.9,0,.9.4,1.6,1.1,2,2l23.5,56.9c.4.9.4,1.9,0,2.9-.4.9-1.1,1.6-2,2h0l-9.4,3.9c-.5.2-.9.3-1.4.3ZM60.3,34.7l22.9,55.4,7.9-3.3-22.9-55.4-7.9,3.3ZM92.6,88.7h0,0ZM91.8,86.6s0,0,0,0h0Z'/><path d='M76.5,82.3c-.3,0-.6,0-.9-.2-.6-.2-1-.7-1.2-1.2l-13.8-33.4c-.2-.6-.2-1.2,0-1.7.2-.6.7-1,1.2-1.2l12.1-5c1.2-.5,2.5,0,3,1.2l13.8,33.4c.2.6.2,1.2,0,1.7-.2.6-.7,1-1.2,1.2l-12.1,5c-.3.1-.6.2-.9.2ZM65.7,47.8l12,29.2,7.9-3.3-12-29.2-7.9,3.3Z'/></g></svg>";
		public const string Edit =
			"<svg preserveAspectRatio='xMinYMin meet' id='edit' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.9,95.9c-.7,0-1.3-.3-1.8-.7s-.8-1.2-.7-1.9l.4-8.9c0-.6.3-1.2.7-1.7l23.7-23.7h0c3.3-3.3,8.8-3.3,12.1,0,3.3,3.3,3.3,8.8,0,12.1l-23.7,23.7c-.4.4-1,.7-1.7.7l-8.9.4s0,0-.1,0ZM63.7,85.5l-.2,5.2,5.2-.2,23-23c1.4-1.4,1.4-3.6,0-5-1.4-1.4-3.6-1.4-5,0l-23,23Z'/><path d='M45.3,95.9h-13.1c-5.5,0-9.9-4.5-9.9-9.9v-43.2c0-5.5,4.5-9.9,9.9-9.9h49.7c5.5,0,9.9,4.5,9.9,9.9v5.3c0,1.4-1.1,2.5-2.5,2.5s-2.5-1.1-2.5-2.5v-5.3c0-2.7-2.2-4.9-4.9-4.9h-49.7c-2.7,0-4.9,2.2-4.9,4.9v43.2c0,2.7,2.2,4.9,4.9,4.9h13.1c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M89.4,50.5H24.8c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h64.6c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><g><path d='M68.9,37.8c-1.4,0-2.5-1.1-2.5-2.5v-7.7c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v7.7c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M45.3,37.8c-1.4,0-2.5-1.1-2.5-2.5v-7.7c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v7.7c0,1.4-1.1,2.5-2.5,2.5Z'/></g><path d='M59.8,67.7h-22c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M48.9,81.3h-11c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h11c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string EditFile =
			"<svg preserveAspectRatio='xMinYMin meet' id='edit' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,72.9c-6.8,0-12.4-5.5-12.4-12.4s5.5-12.4,12.4-12.4,12.4,5.5,12.4,12.4-5.5,12.4-12.4,12.4ZM60.5,53.1c-4.1,0-7.4,3.3-7.4,7.4s3.3,7.4,7.4,7.4,7.4-3.3,7.4-7.4-3.3-7.4-7.4-7.4Z'/><path d='M62.6,91.3h-4.3c-2,0-3.8-1.2-4.6-3l-2.4-5.4c-2.1-.8-4-1.9-5.7-3.3l-5.9.6c-2,.2-3.9-.8-4.9-2.5l-2.1-3.7c-1-1.7-.9-3.9.3-5.5l3.5-4.7c-.2-1.2-.2-2.3-.2-3.3s0-2.2.2-3.3l-3.4-4.8c-1.2-1.6-1.3-3.8-.3-5.5l2.1-3.7c1-1.7,2.9-2.7,4.9-2.5l5.8.6c1.8-1.4,3.7-2.5,5.7-3.3l2.4-5.4c.8-1.8,2.6-3,4.6-3h4.3c2,0,3.8,1.2,4.6,3l2.4,5.4c2.1.8,4,1.9,5.7,3.3l5.9-.6c2-.2,3.9.8,4.9,2.5l2.1,3.7c1,1.7.9,3.9-.3,5.5l-3.5,4.7c.2,1.2.2,2.3.2,3.3s0,2.2-.2,3.3l3.4,4.8c1.2,1.6,1.3,3.8.3,5.5l-2.1,3.7c-1,1.7-2.9,2.7-4.9,2.5l-5.8-.6c-1.8,1.4-3.7,2.5-5.7,3.3l-2.4,5.4c-.8,1.8-2.6,3-4.6,3ZM45.6,74.6c1.1,0,2.2.4,3.1,1.1,1.4,1.1,2.9,1.9,4.5,2.6,1.2.5,2.2,1.4,2.7,2.6l2.4,5.4h4.3s2.4-5.4,2.4-5.4c.5-1.2,1.5-2.1,2.7-2.6,1.6-.7,3.1-1.5,4.5-2.6,1.1-.8,2.3-1.2,3.6-1.1l5.8.6,2.2-3.7-3.5-4.7c-.8-1.1-1.1-2.4-.9-3.7.1-.9.2-1.7.2-2.6s0-1.7-.2-2.6c-.2-1.3.1-2.6.9-3.7l3.5-4.7-2.1-3.7-5.8.6c-1.3.1-2.6-.2-3.6-1.1-1.4-1.1-2.9-1.9-4.5-2.6-1.2-.5-2.2-1.4-2.7-2.6l-2.4-5.4h-4.3s-2.4,5.4-2.4,5.4c-.5,1.2-1.5,2.1-2.7,2.6-1.6.7-3.1,1.5-4.5,2.6-1.1.8-2.3,1.2-3.6,1.1l-5.8-.6-2.2,3.7,3.5,4.7c.8,1.1,1.1,2.4.9,3.7-.1.9-.2,1.7-.2,2.6s0,1.7.2,2.6c.2,1.3-.1,2.6-.9,3.7l-3.5,4.7,2.1,3.7,5.8-.6c.2,0,.4,0,.5,0Z'/></svg>";
		public const string FileBoard =
			"<svg preserveAspectRatio='xMinYMin meet' version='1.1' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 113 113' width='113' height='113'> <path d='M0 0 C37.29 0 74.58 0 113 0 C113 37.29 113 74.58 113 113 C75.71 113 38.42 113 0 113 C0 75.71 0 38.42 0 0 Z ' fill='none' transform='translate(0,0)'/> <path d='M0 0 C37.29 0 74.58 0 113 0 C113 37.29 113 74.58 113 113 C75.71 113 38.42 113 0 113 C0 75.71 0 38.42 0 0 Z M22 11 C21.79600082 13.87577345 21.71684573 16.65564024 21.7253418 19.53295898 C21.72023087 20.41600708 21.71511993 21.29905518 21.70985413 22.2088623 C21.69591451 25.13800774 21.69691818 28.06691902 21.69921875 30.99609375 C21.69533418 33.02629179 21.69100001 35.05648902 21.68623352 37.08668518 C21.67876187 41.34630973 21.67906939 45.60585142 21.68432617 49.86547852 C21.69012159 55.33126735 21.67319256 60.79671742 21.6499815 66.26245022 C21.63526827 70.45654798 21.63436918 74.6505637 21.63749123 78.84468269 C21.63697678 80.86016575 21.63171612 82.87565325 21.62167931 84.89111137 C21.60937907 87.70684865 21.61606793 90.52192032 21.62768555 93.33764648 C21.62004181 94.17463577 21.61239807 95.01162506 21.60452271 95.87397766 C21.52874695 100.07217044 21.52874695 100.07217044 23.32414722 103.71381664 C26.07526423 105.82524371 28.77460277 105.38201454 32.17773438 105.38818359 C32.90487137 105.39344055 33.63200836 105.39869751 34.38117981 105.40411377 C36.78558816 105.41732141 39.18933487 105.40857339 41.59375 105.3984375 C43.26379524 105.40045803 44.93383962 105.40336928 46.60388184 105.40713501 C50.10433348 105.41155938 53.60455551 105.40512378 57.10498047 105.39111328 C61.59440131 105.37402119 66.08328414 105.38386179 70.57268524 105.40183067 C74.02134259 105.41266553 77.46987215 105.40922357 80.91853333 105.4014473 C82.57396 105.39944446 84.22939823 105.40190751 85.8848114 105.40888596 C88.19760926 105.41649572 90.50953296 105.40490388 92.82226562 105.38818359 C94.13892212 105.38579681 95.45557861 105.38341003 96.81213379 105.38095093 C99.92573795 105.28441757 99.92573795 105.28441757 101.67585278 103.71102333 C103.97715216 100.73735329 103.40571931 97.05541324 103.37231445 93.4453125 C103.37596512 92.57187195 103.37961578 91.6984314 103.38337708 90.79852295 C103.39149267 87.90715174 103.37786408 85.01633918 103.36328125 82.125 C103.36276276 80.11865235 103.36328002 78.11230418 103.36479187 76.10595703 C103.3647021 71.899337 103.35426313 67.69290844 103.33618164 63.48632812 C103.31349438 58.08899904 103.3129499 52.69194983 103.31969929 47.29458618 C103.32303771 43.15109766 103.31641155 39.00767317 103.30657005 34.86419678 C103.30271559 32.87384374 103.30157086 30.88348358 103.3031559 28.89312744 C103.30352596 26.11350894 103.2909228 23.33425182 103.2746582 20.5546875 C103.27713562 19.72862 103.27961304 18.90255249 103.28216553 18.05145264 C103.24594444 14.18097033 103.19696262 12.29544394 101 9 C98.49733727 8.70270758 96.24117043 8.60070647 93.73632812 8.62768555 C92.61417152 8.62220955 92.61417152 8.62220955 91.46934509 8.61662292 C88.98975108 8.60849888 86.51080679 8.62214241 84.03125 8.63671875 C82.31254069 8.63723714 80.59383079 8.63672027 78.87512207 8.63520813 C75.27033658 8.63529794 71.66577434 8.64574434 68.06103516 8.66381836 C63.4336942 8.68650589 58.80667966 8.68705056 54.1792984 8.68030071 C50.62946659 8.67696379 47.0797096 8.68358558 43.52989197 8.69342995 C41.82340384 8.69728702 40.1169074 8.69842807 38.41041565 8.6968441 C36.02800561 8.69647442 33.64601856 8.70906598 31.26367188 8.7253418 C30.55399765 8.72286438 29.84432343 8.72038696 29.11314392 8.71783447 C25.11053935 8.65546153 25.11053935 8.65546153 22 11 Z ' transform='translate(0,0)'/> <path d='M0 0 C37.29 0 74.58 0 113 0 C113 37.29 113 74.58 113 113 C109.04 113 105.08 113 101 113 C105 110 105 110 107 110 C107.4640625 108.824375 107.4640625 108.824375 107.9375 107.625 C109 105 109 105 110 103 C110.11424488 100.13859506 110.16786227 97.30098125 110.17700195 94.4387207 C110.18357315 93.55234421 110.19014435 92.66596771 110.19691467 91.75273132 C110.2166735 88.81041382 110.22831729 85.86814747 110.23828125 82.92578125 C110.24235678 81.92135672 110.2464323 80.91693218 110.25063133 79.88207054 C110.27150516 74.56371643 110.28580002 69.2453882 110.29516602 63.92700195 C110.30622067 58.43689358 110.34060609 52.94719162 110.38033772 47.45722675 C110.40650522 43.23381301 110.41485809 39.01049596 110.41844749 34.78700829 C110.42331472 32.76364593 110.43493256 30.74028793 110.45348549 28.71700478 C110.47787481 25.88042848 110.47691197 23.04490136 110.4699707 20.20825195 C110.48278076 19.37629303 110.49559082 18.54433411 110.50878906 17.68716431 C110.45824851 12.00386241 109.36906271 8.6265782 106 4 C102.59616374 1.26678774 98.46852077 1.7042441 94.28149414 1.70947266 C93.50865585 1.70275543 92.73581757 1.69603821 91.93955994 1.68911743 C89.38802643 1.67013916 86.83675592 1.66621353 84.28515625 1.6640625 C82.50923047 1.65763508 80.73330575 1.65090676 78.9573822 1.64389038 C75.23596745 1.63191851 71.51464554 1.62820953 67.79321289 1.62939453 C63.0270706 1.62959786 58.26152364 1.60228108 53.4955225 1.56785011 C49.82805146 1.54560342 46.16072428 1.54155628 42.49319267 1.54261398 C40.73612857 1.54025522 38.97906067 1.5314162 37.22206306 1.51594925 C34.76358537 1.49633133 32.30639931 1.50210971 29.84790039 1.51416016 C29.12410187 1.50292114 28.40030334 1.49168213 27.65457153 1.48010254 C22.82503484 1.54019357 20.40475156 2.59736225 17 6 C14.89068858 9.13161573 14.64110828 12.02448409 14.375 15.75 C14.30023438 16.73484375 14.22546875 17.7196875 14.1484375 18.734375 C14.09945313 19.48203125 14.05046875 20.2296875 14 21 C9.05 21.495 9.05 21.495 4 22 C3.61781233 23.65614657 3.28604898 25.32457024 3 27 C4.66342837 28.66342837 6.7825709 28.36048097 9.0625 28.5625 C10.691875 28.706875 12.32125 28.85125 14 29 C14 33.29 14 37.58 14 42 C9.05 42.495 9.05 42.495 4 43 C3.61781233 44.65614657 3.28604898 46.32457024 3 48 C4.66342837 49.66342837 6.7825709 49.36048097 9.0625 49.5625 C10.691875 49.706875 12.32125 49.85125 14 50 C14 54.29 14 58.58 14 63 C9.05 63.495 9.05 63.495 4 64 C3.61781233 65.65614657 3.28604898 67.32457024 3 69 C4.66342837 70.66342837 6.7825709 70.36048097 9.0625 70.5625 C10.691875 70.706875 12.32125 70.85125 14 71 C14 75.62 14 80.24 14 85 C9.05 85.495 9.05 85.495 4 86 C3.61781233 87.65614657 3.28604898 89.32457024 3 91 C4.66342837 92.66342837 6.7825709 92.36048097 9.0625 92.5625 C10.44115234 92.68818359 10.44115234 92.68818359 11.84765625 92.81640625 C12.55792969 92.87699219 13.26820312 92.93757813 14 93 C14.08636719 93.88042969 14.17273437 94.76085937 14.26171875 95.66796875 C14.82459206 103.07094307 14.82459206 103.07094307 18.5 109.1875 C19.325 109.785625 20.15 110.38375 21 111 C21.66 111.66 22.32 112.32 23 113 C15.41 113 7.82 113 0 113 C0 75.71 0 38.42 0 0 Z ' fill='#FFFFFF' transform='translate(0,0)'/> <path d='M0 0 C6.37381226 3.62171676 11.15919154 8.97521728 13.625 15.9375 C13.625 20.03721664 13.4221167 22.08133049 10.6015625 25.11083984 C6.62963136 27.54833892 3.57564146 27.54962401 -1 27.5390625 C-1.8245166 27.54763275 -2.6490332 27.556203 -3.49853516 27.56503296 C-5.23661638 27.5764861 -6.97479343 27.57754458 -8.71289062 27.56884766 C-11.36754993 27.56251776 -14.0177653 27.60949404 -16.671875 27.66015625 C-18.36457773 27.664702 -20.0572924 27.66610933 -21.75 27.6640625 C-22.93456787 27.69169174 -22.93456787 27.69169174 -24.14306641 27.71987915 C-27.26349661 27.66764961 -29.4802109 27.61905524 -32.01391602 25.68914795 C-34.67267758 22.71165758 -35.36776409 21.08704215 -35.5625 17.0625 C-33.98904934 9.42835049 -28.2677924 4.45358444 -22.20703125 0.06640625 C-15.69255767 -3.94784293 -6.69652689 -3.09930304 0 0 Z ' transform='translate(73.375,66.0625)'/> <path d='M0 0 C4.31775797 2.3774511 7.08779482 5.4673655 9 10 C9.74186662 15.51100916 8.55277782 18.81816474 5.484375 23.265625 C2.36157588 26.91436924 -0.79253209 27.84632227 -5.4375 28.4375 C-10.42992833 27.82439477 -14.04226343 26.32625168 -17.328125 22.421875 C-19.95474828 18.31546396 -20.71439663 14.91147681 -20 10 C-15.37871429 1.27090477 -9.74099141 -1.86737174 0 0 Z ' transform='translate(68,28)'/> <path d='M0 0 C4.26614081 2.34763962 6.35009566 5.28219267 8.8125 9.4375 C8.8125 10.0975 8.8125 10.7575 8.8125 11.4375 C-2.0775 11.4375 -12.9675 11.4375 -24.1875 11.4375 C-22.02116562 6.02166405 -20.31711835 2.84111209 -15.4375 -0.375 C-9.85914475 -2.41324519 -5.37266962 -2.34738108 0 0 Z ' fill='#FFFFFF' transform='translate(70.1875,73.5625)'/> <path d='M0 0 C1.9375 1.1875 1.9375 1.1875 3 3 C3.5 5.4375 3.5 5.4375 3 8 C0.8125 10.375 0.8125 10.375 -2 12 C-4.92738742 11.9303003 -6.41595862 11.40278716 -8.8125 9.75 C-10.45364719 7.3314673 -10.42967368 5.86449123 -10 3 C-7.66004009 -0.99169632 -4.2277022 -0.45296809 0 0 Z ' fill='#FFFFFF' transform='translate(66,36)'/> </svg>";
		public const string FileEdit =
			"<svg preserveAspectRatio='xMinYMin meet' viewBox='0 0 83 84' xmlns='http://www.w3.org/2000/svg' version='1.1' width='83' height='84'><path d='M0 0 C27.39 0 54.78 0 83 0 C83 27.72 83 55.44 83 84 C55.61 84 28.22 84 0 84 C0 56.28 0 28.56 0 0 Z ' fill='none' transform='translate(0,0)'/><path d='M0 0 C0.81409332 -0.00315216 1.62818665 -0.00630432 2.46694946 -0.009552 C3.33685516 -0.00752777 4.20676086 -0.00550354 5.10302734 -0.00341797 C6.00266205 -0.00437469 6.90229675 -0.00533142 7.82919312 -0.00631714 C9.72711253 -0.00699664 11.62503429 -0.00515841 13.52294922 -0.00097656 C16.43511128 0.00438743 19.34711999 -0.00093575 22.25927734 -0.00732422 C24.10302769 -0.00666364 25.94677793 -0.0053829 27.79052734 -0.00341797 C29.10145859 -0.00645432 29.10145859 -0.00645432 30.43887329 -0.009552 C36.58403114 0.01442768 36.58403114 0.01442768 38.81396484 1.12939453 C38.48396484 2.77939453 38.15396484 4.42939453 37.81396484 6.12939453 C36.52707764 6.14397705 35.24019043 6.15855957 33.91430664 6.17358398 C29.14497935 6.23150601 24.37612722 6.30873916 19.60717773 6.3918457 C17.54215195 6.42512377 15.47703457 6.45314667 13.41186523 6.47583008 C10.44524052 6.50932308 7.47945736 6.56163598 4.51318359 6.61767578 C3.12555061 6.62726067 3.12555061 6.62726067 1.70988464 6.63703918 C0.41888603 6.66673042 0.41888603 6.66673042 -0.89819336 6.69702148 C-1.65542191 6.70812454 -2.41265045 6.7192276 -3.19282532 6.73066711 C-5.81877215 7.25596906 -6.67211084 7.92084157 -8.18603516 10.12939453 C-8.62857872 12.79164599 -8.62857872 12.79164599 -8.54248047 15.83837891 C-8.54548157 17.0096936 -8.54848267 18.1810083 -8.55157471 19.38781738 C-8.53922791 20.65347534 -8.5268811 21.9191333 -8.51416016 23.22314453 C-8.51228699 24.52135132 -8.51041382 25.81955811 -8.50848389 27.15710449 C-8.50388275 29.90434214 -8.48946527 32.65128635 -8.46728516 35.3984375 C-8.43900292 38.91916862 -8.42807331 42.4396687 -8.42360306 45.96050262 C-8.41844536 49.31891888 -8.40390593 52.67725579 -8.38916016 56.03564453 C-8.38491028 57.30130249 -8.3806604 58.56696045 -8.37628174 59.87097168 C-8.36667419 61.04228638 -8.35706665 62.21360107 -8.34716797 63.42041016 C-8.34033997 64.45282837 -8.33351196 65.48524658 -8.32647705 66.5489502 C-8.29359544 69.24705404 -8.29359544 69.24705404 -7.18603516 72.12939453 C-3.80551241 73.8196559 0.03656626 73.37384412 3.75927734 73.44580078 C5.0867952 73.47697243 5.0867952 73.47697243 6.44113159 73.5087738 C9.27366226 73.57426052 12.10628709 73.63315016 14.93896484 73.69189453 C16.85694341 73.73508059 18.77491238 73.77869491 20.69287109 73.82275391 C25.39980535 73.929941 30.10683452 74.03121093 34.81396484 74.12939453 C34.80872803 73.43789307 34.80349121 72.7463916 34.7980957 72.03393555 C34.77738984 68.91993101 34.76432521 65.80594092 34.75146484 62.69189453 C34.74308594 61.60328125 34.73470703 60.51466797 34.72607422 59.39306641 C34.72285156 58.35859375 34.71962891 57.32412109 34.71630859 56.25830078 C34.71107178 55.29996338 34.70583496 54.34162598 34.70043945 53.35424805 C34.81396484 51.12939453 34.81396484 51.12939453 35.81396484 50.12939453 C37.51960263 49.97198848 39.23123729 49.87846061 40.94287109 49.81298828 C41.97734375 49.77109375 43.01181641 49.72919922 44.07763672 49.68603516 C45.16625 49.64671875 46.25486328 49.60740234 47.37646484 49.56689453 C49.01518555 49.50211914 49.01518555 49.50211914 50.68701172 49.43603516 C53.39583253 49.32957495 56.10488135 49.22894647 58.81396484 49.12939453 C59.14396484 41.86939453 59.47396484 34.60939453 59.81396484 27.12939453 C61.13396484 26.79939453 62.45396484 26.46939453 63.81396484 26.12939453 C66.33060277 28.64603246 65.04825687 35.43771669 65.06396484 38.81689453 C65.08458984 39.90679687 65.10521484 40.99669922 65.12646484 42.11962891 C65.13419922 43.70807617 65.13419922 43.70807617 65.14208984 45.32861328 C65.15143555 46.29629639 65.16078125 47.26397949 65.17041016 48.26098633 C64.1425612 56.53235438 57.82403781 61.70367552 52.25146484 67.31689453 C50.9665918 68.66944336 50.9665918 68.66944336 49.65576172 70.04931641 C41.29439076 78.49876802 34.69325107 80.26708061 23.13818359 80.49267578 C20.7427134 80.50250997 18.34872596 80.49037482 15.95336914 80.46557617 C13.48925536 80.44177661 11.02621297 80.44505896 8.56201172 80.45166016 C6.98322927 80.44608835 5.40445132 80.43898161 3.82568359 80.43017578 C3.09540451 80.43108719 2.36512543 80.4319986 1.61271667 80.43293762 C-4.06427019 80.35429542 -7.42326459 79.60070873 -11.78909302 75.95672607 C-14.68336248 72.54276826 -14.8218561 71.01527721 -14.84887695 66.59838867 C-14.86786545 65.29676285 -14.88685394 63.99513702 -14.90641785 62.65406799 C-14.90078581 61.2408892 -14.89369549 59.82771558 -14.88525391 58.41455078 C-14.8911667 56.95632851 -14.89841505 55.49811109 -14.90693665 54.03990173 C-14.91880263 50.98752504 -14.91213502 47.93587815 -14.89282227 44.88354492 C-14.86994844 40.98127055 -14.89670136 37.0811756 -14.93788433 33.17911625 C-14.96352313 30.16858761 -14.96086618 27.15857619 -14.94999123 24.14797783 C-14.94850919 22.70992492 -14.9561408 21.27183146 -14.97324181 19.83387947 C-15.09900036 7.20995311 -15.09900036 7.20995311 -11.38842773 3.26391602 C-7.44467668 0.63524974 -4.6668415 0.00520419 0 0 Z ' transform='translate(15.18603515625,1.87060546875)'/><path d='M0 0 C3.5585814 2.44652471 4.73425876 4.03194261 5.875 8.1875 C4.93560748 12.28056742 4.14817371 13.27985151 1.37109375 16.265625 C0.55479492 17.14500732 -0.26150391 18.02438965 -1.10253906 18.93041992 C-1.79228821 19.66100113 -1.79228821 19.66100113 -2.49597168 20.40634155 C-4.12489037 22.132333 -5.74131234 23.8696107 -7.35839844 25.60668945 C-10.48142058 28.94620717 -13.64872148 32.23980833 -16.8371582 35.5168457 C-18.15955416 36.88631248 -19.47106785 38.26636747 -20.77172852 39.65649414 C-33.01768013 52.72467174 -33.01768013 52.72467174 -40.82421875 53.52734375 C-45.54323212 53.45676788 -45.54323212 53.45676788 -48 51 C-49.05312972 46.18083641 -49.05400031 41.76983435 -46.49438477 37.47183228 C-44.29223226 34.88829717 -41.97687602 32.53024838 -39.53125 30.17578125 C-38.66238159 29.30350388 -37.79351318 28.4312265 -36.89831543 27.53251648 C-35.06194251 25.70178587 -33.21201261 23.88456567 -31.34960938 22.08032227 C-28.50327045 19.31567659 -25.7112154 16.50366093 -22.921875 13.68164062 C-21.12800243 11.90581609 -19.33122112 10.13292403 -17.53125 8.36328125 C-16.69758911 7.52283768 -15.86392822 6.6823941 -15.00500488 5.81648254 C-9.93746393 0.96995295 -7.09108771 -1.28644053 0 0 Z ' transform='translate(69,7)'/><path d='M0 0 C0.74513351 0.22885391 1.49026703 0.45770782 2.25798035 0.6934967 C2.95820618 2.45283508 2.95820618 2.45283508 3.25798035 4.6934967 C2.00721741 6.57917786 2.00721741 6.57917786 0.05436707 8.54408264 C-0.67033783 9.28454834 -1.39504272 10.02501404 -2.14170837 10.78791809 C-2.94051422 11.57660278 -3.73932007 12.36528748 -4.56233215 13.1778717 C-5.3731424 13.99346558 -6.18395264 14.80905945 -7.01933289 15.64936829 C-8.74008188 17.37352454 -10.46718024 19.09136338 -12.20002747 20.80335999 C-14.85282932 23.42944926 -17.47799701 26.0810391 -20.10139465 28.73646545 C-21.77427327 30.40603946 -23.44870257 32.07406146 -25.12483215 33.7403717 C-26.30482285 34.93554413 -26.30482285 34.93554413 -27.50865173 36.15486145 C-28.25226959 36.88271851 -28.99588745 37.61057556 -29.76203918 38.36048889 C-30.41125336 39.0057251 -31.06046753 39.6509613 -31.72935486 40.31575012 C-34.22659479 42.02520705 -35.77726482 41.96783168 -38.74201965 41.6934967 C-39.22322083 39.22450256 -39.22322083 39.22450256 -38.74201965 35.6934967 C-36.51995003 32.53000622 -33.79222802 29.90356223 -31.0428009 27.19740295 C-30.25560165 26.40446335 -29.4684024 25.61152374 -28.65734863 24.79455566 C-26.99087851 23.12400557 -25.31681912 21.46099684 -23.63581848 19.80506897 C-21.06036287 17.26176698 -18.51849581 14.6880245 -15.97834778 12.10951233 C-14.35591955 10.49104815 -12.73163149 8.87444576 -11.1053009 7.25990295 C-10.34365128 6.48610794 -9.58200165 5.71231293 -8.79727173 4.91506958 C-8.07690231 4.21361313 -7.3565329 3.51215668 -6.61433411 2.78944397 C-5.98590103 2.16578445 -5.35746796 1.54212494 -4.70999146 0.89956665 C-2.74201965 -0.3065033 -2.74201965 -0.3065033 0 0 Z ' fill='#FFFFFF' transform='translate(65.74201965332031,12.306503295898438)'/><path d='M0 0 C4.62 0 9.24 0 14 0 C9.90473413 4.89434213 5.73642915 9.70720334 1 14 C0.67 14 0.34 14 0 14 C0 9.38 0 4.76 0 0 Z ' fill='#FFFFFF' transform='translate(56,57)'/></svg>";
		public const string FileSynthesis =
			"<svg preserveAspectRatio='xMinYMin meet' viewBox='0 0 97 127' version='1.1' xmlns='http://www.w3.org/2000/svg' width='97' height='127'> <path d='M0 0 C32.01 0 64.02 0 97 0 C97 41.91 97 83.82 97 127 C64.99 127 32.98 127 0 127 C0 85.09 0 43.18 0 0 Z ' fill='none' transform='translate(0,0)'/> <path d='M0 0 C32.01 0 64.02 0 97 0 C97 41.91 97 83.82 97 127 C64.99 127 32.98 127 0 127 C0 85.09 0 43.18 0 0 Z M12 13 C10.30722911 16.38554178 10.83434673 20.23503066 10.82299805 23.96020508 C10.81642685 24.88880844 10.80985565 25.8174118 10.80308533 26.77415466 C10.78332841 29.85594762 10.77168471 32.93769181 10.76171875 36.01953125 C10.75764322 37.07058233 10.7535677 38.12163342 10.74936867 39.20453453 C10.72849822 44.7669696 10.71420043 50.32937993 10.70483398 55.8918457 C10.69376445 61.64221495 10.65936084 67.39219776 10.61966228 73.14243031 C10.5935391 77.55963367 10.58514599 81.97674435 10.58155251 86.39401817 C10.57667583 88.51392923 10.56503125 90.63383585 10.54651451 92.75367165 C10.52221366 95.71996461 10.5230716 98.68524968 10.5300293 101.65161133 C10.51721924 102.52816376 10.50440918 103.40471619 10.49121094 104.30783081 C10.52591209 108.36444355 10.68650119 110.48567433 12.84187317 114.02177429 C16.37656938 117.26181876 19.43625177 117.13246197 24.04443359 117.16113281 C25.28937592 117.17074036 26.53431824 117.1803479 27.81698608 117.19024658 C29.17746802 117.19509453 30.53795209 117.19937336 31.8984375 117.203125 C33.29464723 117.20887904 34.69085695 117.21463674 36.08706665 117.22039795 C39.01410286 117.23089849 41.94111167 117.23674738 44.86816406 117.24023438 C48.61301519 117.24571806 52.35750253 117.26974696 56.10223961 117.29820633 C58.98649137 117.31685868 61.87065594 117.32203967 64.75496292 117.32357025 C66.13522964 117.32659132 67.51549502 117.33460147 68.89570236 117.34775543 C70.82932046 117.36486869 72.76308908 117.36108324 74.69677734 117.35644531 C75.7958139 117.36009094 76.89485046 117.36373657 78.02719116 117.36749268 C81.42840652 116.94704124 83.26721469 116.03971115 86 114 C88.35078429 110.47382356 88.24370599 109.04835119 88.22705078 104.8762207 C88.22689972 103.61513992 88.22674866 102.35405914 88.22659302 101.05476379 C88.21637976 99.67843468 88.20594777 98.30210717 88.1953125 96.92578125 C88.19158172 95.51759542 88.18873506 94.109407 88.18673706 92.70121765 C88.17910564 88.9972719 88.15946148 85.29348332 88.1373291 81.58959961 C88.11686003 77.80910366 88.10773308 74.02858228 88.09765625 70.24804688 C88.07621504 62.83195964 88.04208513 55.41600186 88 48 C86.95021973 47.98018066 85.90043945 47.96036133 84.81884766 47.93994141 C80.93106739 47.86397802 77.04366597 47.77524702 73.15625 47.68261719 C71.47240671 47.64428896 69.78848045 47.60944288 68.10449219 47.578125 C65.68654694 47.53255713 63.26917472 47.47455077 60.8515625 47.4140625 C60.0963028 47.40251129 59.34104309 47.39096008 58.56289673 47.37905884 C53.22774341 47.22774341 53.22774341 47.22774341 51 45 C50.76670796 42.36348031 50.65063108 39.82387036 50.62109375 37.18359375 C50.60409927 36.41193375 50.5871048 35.64027374 50.56959534 34.8452301 C50.51638048 32.37607022 50.47612605 29.9069279 50.4375 27.4375 C50.39362768 24.96458732 50.34697526 22.49180774 50.29469299 20.01905823 C50.26264361 18.47890681 50.23578258 16.93863747 50.21446228 15.39830017 C50.460547 11.90589146 50.460547 11.90589146 49 9 C44.83444658 8.73250432 40.66510568 8.7286293 36.49169922 8.70483398 C34.40393631 8.68773696 32.31780595 8.64388922 30.23046875 8.59960938 C20.41244887 8.46772169 20.41244887 8.46772169 12 13 Z ' transform='translate(0,0)'/> <path d='M0 0 C32.01 0 64.02 0 97 0 C97 41.91 97 83.82 97 127 C64.99 127 32.98 127 0 127 C0 85.09 0 43.18 0 0 Z M6.0625 8.125 C2.64605078 12.88732315 2.84589285 16.50605933 2.83886719 22.21972656 C2.83390228 23.17598434 2.82893738 24.13224213 2.82382202 25.11747742 C2.80917644 28.29186718 2.8023938 31.46620695 2.796875 34.640625 C2.79112476 36.84299725 2.78536701 39.04536949 2.77960205 41.2477417 C2.76908472 45.87138096 2.7632471 50.4950028 2.75976562 55.11865234 C2.75428462 61.04148394 2.73026795 66.96408522 2.70179367 72.88684464 C2.68316178 77.43876821 2.67796135 81.99063642 2.67642975 86.54259491 C2.67340308 88.72586067 2.66537408 90.90912545 2.65224457 93.09235382 C2.63517567 96.14931184 2.63706675 99.20570887 2.64355469 102.26269531 C2.6343399 103.16370468 2.62512512 104.06471405 2.6156311 104.99302673 C2.65519406 110.94868303 3.5295505 115.32180815 7.71875 119.86328125 C8.60046875 120.58064453 8.60046875 120.58064453 9.5 121.3125 C10.0878125 121.80363281 10.675625 122.29476562 11.28125 122.80078125 C14.83458747 125.28004171 18.16955712 125.14634718 22.36401367 125.19287109 C23.67935593 125.20893402 24.99469818 125.22499695 26.34989929 125.24154663 C27.78924918 125.25303615 29.22860632 125.26364434 30.66796875 125.2734375 C31.39441401 125.27876118 32.12085926 125.28408485 32.86931801 125.28956985 C36.71698576 125.31628149 40.56462135 125.33565975 44.41235352 125.35009766 C47.58712711 125.36341657 50.76125484 125.39123258 53.93579102 125.43212891 C57.77782138 125.48161754 61.61926772 125.5062385 65.46160316 125.51332474 C66.92037649 125.52001095 68.37913813 125.53516426 69.8377285 125.55921555 C77.92379701 125.68490705 84.74425058 125.74923154 91 120 C95.32654128 115.34064785 96.12206388 112.01226454 96.19287109 105.75952148 C96.21696548 103.82622231 96.21696548 103.82622231 96.24154663 101.85386658 C96.25303872 100.44294906 96.26364648 99.0320241 96.2734375 97.62109375 C96.28142302 96.55171916 96.28142302 96.55171916 96.28956985 95.46074104 C96.31628821 91.68209027 96.33566121 87.90347227 96.35009766 84.12475586 C96.36339558 81.01214946 96.39117137 77.90020511 96.43212891 74.7878418 C96.48174115 71.01712282 96.50625392 67.24700066 96.51332474 63.47596931 C96.5199933 62.04675847 96.53509939 60.61755862 96.55921555 59.18853569 C96.7352563 48.11933838 95.22401544 40.50450232 87.7578125 32.15844727 C86.55989945 30.94806972 85.34844151 29.75097383 84.125 28.56640625 C83.47805176 27.9217894 82.83110352 27.27717255 82.16455078 26.61302185 C80.80894147 25.26707064 79.44737009 23.92709926 78.08007812 22.59301758 C75.99267964 20.55535406 73.92485737 18.49930681 71.859375 16.43945312 C70.53234335 15.12649817 69.20425467 13.81461054 67.875 12.50390625 C67.26027832 11.89454727 66.64555664 11.28518829 66.01220703 10.65736389 C55.80130728 0.72594351 45.74886955 0.63849425 31.9375 0.625 C30.60090412 0.6006599 29.26431542 0.57592275 27.92773438 0.55078125 C18.99253532 0.52393538 12.21629289 1.03607192 6.0625 8.125 Z ' fill='#FFFFFF' transform='translate(0,0)'/> <path d='M0 0 C5.28 0 10.56 0 16 0 C16.33 0.66 16.66 1.32 17 2 C16.46890625 2.23074219 15.9378125 2.46148437 15.390625 2.69921875 C10.06461875 5.1581231 6.69677033 7.60645934 4 13 C3.89071162 16.08045044 3.84265577 19.13863551 3.83886719 22.21972656 C3.83390228 23.17598434 3.82893738 24.13224213 3.82382202 25.11747742 C3.80917644 28.29186718 3.8023938 31.46620695 3.796875 34.640625 C3.79112476 36.84299725 3.78536701 39.04536949 3.77960205 41.2477417 C3.76908472 45.87138096 3.7632471 50.4950028 3.75976562 55.11865234 C3.75428462 61.04148394 3.73026795 66.96408522 3.70179367 72.88684464 C3.68316178 77.43876821 3.67796135 81.99063642 3.67642975 86.54259491 C3.67340308 88.72586067 3.66537408 90.90912545 3.65224457 93.09235382 C3.63517567 96.14931184 3.63706675 99.20570887 3.64355469 102.26269531 C3.6343399 103.16370468 3.62512512 104.06471405 3.6156311 104.99302673 C3.65412786 110.78818045 4.43559845 115.18850768 8.5 119.64453125 C13.30872434 123.38882442 16.35656334 124.12623789 22.36401367 124.19287109 C23.67935593 124.20893402 24.99469818 124.22499695 26.34989929 124.24154663 C27.78924918 124.25303615 29.22860632 124.26364434 30.66796875 124.2734375 C31.39441401 124.27876118 32.12085926 124.28408485 32.86931801 124.28956985 C36.71698576 124.31628149 40.56462135 124.33565975 44.41235352 124.35009766 C47.58712711 124.36341657 50.76125484 124.39123258 53.93579102 124.43212891 C57.77782138 124.48161754 61.61926772 124.5062385 65.46160316 124.51332474 C66.92037649 124.52001095 68.37913813 124.53516426 69.8377285 124.55921555 C77.92554163 124.68493417 84.73360551 124.74132112 91 119 C94.68063757 114.9631717 95.12822251 111.40172599 95.20532227 106.05981445 C95.22526749 104.83877518 95.24521271 103.6177359 95.26576233 102.35969543 C95.28247482 101.04087875 95.29918732 99.72206207 95.31640625 98.36328125 C95.3369826 96.99992756 95.35797925 95.63658015 95.37937927 94.27323914 C95.4345297 90.68778578 95.48396987 87.10227552 95.53222656 83.51672363 C95.58249299 79.8568008 95.63815219 76.19696043 95.69335938 72.53710938 C95.80085494 65.35814279 95.9023189 58.17910755 96 51 C96.33 51 96.66 51 97 51 C97 76.08 97 101.16 97 127 C64.99 127 32.98 127 0 127 C0 85.09 0 43.18 0 0 Z ' fill='none' transform='translate(0,0)'/> <path d='M0 0 C1.25472107 0.00607269 1.25472107 0.00607269 2.53479004 0.01226807 C3.39286255 0.01035461 4.25093506 0.00844116 5.13500977 0.00646973 C6.95072726 0.00510763 8.76645449 0.00880579 10.58215332 0.01715088 C13.36736854 0.02785015 16.15193872 0.01725653 18.93713379 0.00445557 C20.69885397 0.00577716 22.46057374 0.00833954 24.22229004 0.01226807 C25.05877075 0.0082196 25.89525146 0.00417114 26.75708008 0 C32.6486217 0.04797473 32.6486217 0.04797473 34.87854004 2.27789307 C35.00354004 4.90289307 35.00354004 4.90289307 34.87854004 7.27789307 C31.50336335 8.96548141 27.67998405 8.41942665 23.96838379 8.41070557 C23.08574356 8.41166229 22.20310333 8.41261902 21.29371643 8.41360474 C19.42672657 8.41428575 17.55973434 8.41243691 15.69274902 8.40826416 C12.82428716 8.40290846 9.95598143 8.40821505 7.08752441 8.41461182 C5.27697719 8.41395115 3.46643007 8.41267021 1.65588379 8.41070557 C0.79236801 8.4127298 -0.07114777 8.41475403 -0.96083069 8.4168396 C-7.00650557 8.39284746 -7.00650557 8.39284746 -8.12145996 7.27789307 C-8.30895996 4.84039307 -8.30895996 4.84039307 -8.12145996 2.27789307 C-5.48960727 -0.35395963 -3.58277127 0.02917445 0 0 Z ' transform='translate(36.1214599609375,86.72210693359375)'/> <path d='M0 0 C7.88863139 6.76168405 7.88863139 6.76168405 11.18359375 10.05078125 C11.88291016 10.74880859 12.58222656 11.44683594 13.30273438 12.16601562 C14.00720703 12.87435547 14.71167969 13.58269531 15.4375 14.3125 C16.16904297 15.04017578 16.90058594 15.76785156 17.65429688 16.51757812 C18.34458984 17.20916016 19.03488281 17.90074219 19.74609375 18.61328125 C20.37185303 19.24016846 20.9976123 19.86705566 21.64233398 20.51293945 C23 22 23 22 23 23 C15.41 23 7.82 23 0 23 C0 15.41 0 7.82 0 0 Z ' transform='translate(58,16)'/> <path d='M0 0 C1.29292969 -0.00257812 2.58585937 -0.00515625 3.91796875 -0.0078125 C4.59263184 -0.00362305 5.26729492 0.00056641 5.96240234 0.00488281 C8.02341157 0.0155843 10.08354919 0.00498557 12.14453125 -0.0078125 C13.43746094 -0.00523438 14.73039062 -0.00265625 16.0625 0 C17.25423828 0.00225586 18.44597656 0.00451172 19.67382812 0.00683594 C22.53125 0.265625 22.53125 0.265625 24.53125 2.265625 C24.71875 4.828125 24.71875 4.828125 24.53125 7.265625 C22.62275052 9.17412448 19.21646226 8.39622133 16.6875 8.3984375 C16.00320923 8.39939423 15.31891846 8.40035095 14.6138916 8.40133667 C13.16463123 8.4020183 11.71536782 8.40016523 10.26611328 8.39599609 C8.04062072 8.39064752 5.81532999 8.3959416 3.58984375 8.40234375 C2.18489539 8.40168297 0.77994715 8.4004018 -0.625 8.3984375 C-1.91212891 8.39730957 -3.19925781 8.39618164 -4.52539062 8.39501953 C-7.46875 8.265625 -7.46875 8.265625 -8.46875 7.265625 C-8.65625 4.828125 -8.65625 4.828125 -8.46875 2.265625 C-5.67255868 -0.53056632 -3.90990287 0.00740111 0 0 Z ' transform='translate(36.46875,62.734375)'/></svg>";
		public const string FileTrash =
			"<svg preserveAspectRatio='xMinYMin meet' version='1.1' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 75 85' width='75' height='85'><path d='M0 0 C24.75 0 49.5 0 75 0 C75 28.05 75 56.1 75 85 C50.25 85 25.5 85 0 85 C0 56.95 0 28.9 0 0 Z ' fill='none' transform='translate(0,0)'/><path d='M0 0 C24.75 0 49.5 0 75 0 C75 28.05 75 56.1 75 85 C50.25 85 25.5 85 0 85 C0 56.95 0 28.9 0 0 Z M20.69921875 3.22265625 C18.919175 6.13210706 18.43723759 7.45898371 18.6875 10.8125 C18.8421875 12.8853125 18.8421875 12.8853125 19 15 C14.05 15 9.1 15 4 15 C3.67 16.32 3.34 17.64 3 19 C3.98267859 20.20561184 3.98267859 20.20561184 5.84765625 20.09765625 C7.8984375 20.06510417 9.94921875 20.03255208 12 20 C11.99333817 20.73249466 11.98667633 21.46498932 11.97981262 22.21968079 C11.91971767 29.12056803 11.87403383 36.0214162 11.84456348 42.92249775 C11.82890307 46.47049936 11.80765849 50.01828487 11.77368164 53.56616211 C11.73486993 57.64438967 11.72029398 61.72239001 11.70703125 65.80078125 C11.69154739 67.07585037 11.67606354 68.35091949 11.66011047 69.66462708 C11.65988388 70.84562485 11.65965729 72.02662262 11.65942383 73.2434082 C11.65276199 74.28487503 11.64610016 75.32634186 11.63923645 76.39936829 C11.80484198 79.00400195 11.80484198 79.00400195 13.24578857 80.83354187 C15.44595374 82.29653573 16.77408322 82.39151752 19.40795898 82.41992188 C20.29575546 82.43583374 21.18355194 82.45174561 22.09825134 82.46813965 C23.05453934 82.46834106 24.01082733 82.46854248 24.99609375 82.46875 C25.98118423 82.47636353 26.96627472 82.48397705 27.98121643 82.49182129 C30.06500717 82.50262887 32.1488521 82.50547273 34.23266602 82.50097656 C37.4223053 82.50000463 40.60953659 82.541611 43.79882812 82.5859375 C45.82291164 82.59114469 47.84700353 82.59383269 49.87109375 82.59375 C50.82535751 82.61014526 51.77962128 82.62654053 52.76280212 82.64343262 C56.77144709 82.60156474 59.23053023 82.51353195 62.60864258 80.2590332 C64.84458379 75.02169697 64.48756002 69.89196474 64.390625 64.26171875 C64.38496521 63.02581497 64.37930542 61.78991119 64.37347412 60.51655579 C64.35490285 57.24308402 64.31901355 53.97040921 64.2746582 50.69720459 C64.23361703 47.35224752 64.21578588 44.00725563 64.1953125 40.66210938 C64.15123432 34.10776231 64.08431234 27.55394631 64 21 C66.31 21 68.62 21 71 21 C71.38218767 19.34385343 71.71395102 17.67542976 72 16 C70.83722968 14.60843516 70.83722968 14.60843516 67.62109375 14.90234375 C66.22652923 14.90901631 64.83198257 14.92097468 63.4375 14.9375 C62.72658203 14.94201172 62.01566406 14.94652344 61.28320312 14.95117188 C59.52209987 14.96299136 57.76103964 14.98092737 56 15 C56.03480469 14.39671875 56.06960937 13.7934375 56.10546875 13.171875 C56.32809339 8.14979759 56.32809339 8.14979759 54.6875 3.5 C50.13398746 -0.54756671 42.83756231 0.69293317 37.125 0.625 C35.94035156 0.58761719 34.75570312 0.55023438 33.53515625 0.51171875 C26.71170126 0.36376256 26.71170126 0.36376256 20.69921875 3.22265625 Z ' fill='none' transform='translate(0,0)'/><path d='M0 0 C1.18400391 0.00580078 2.36800781 0.01160156 3.58789062 0.01757812 C13.49931935 0.15796663 13.49931935 0.15796663 16.9375 2.4375 C19.73913026 6.6399454 19.08239712 9.43854935 18.9375 14.4375 C23.8875 14.4375 28.8375 14.4375 33.9375 14.4375 C34.4325 16.9125 34.4325 16.9125 34.9375 19.4375 C33.9375 20.4375 33.9375 20.4375 30.375 20.5 C29.240625 20.479375 28.10625 20.45875 26.9375 20.4375 C26.94638245 21.12796921 26.95526489 21.81843842 26.9644165 22.52983093 C27.05290816 29.71601077 27.11324231 36.90203582 27.15722656 44.08862305 C27.17508325 46.77077823 27.20291436 49.4525546 27.23925781 52.13452148 C27.28777542 55.98953066 27.31046994 59.84395998 27.328125 63.69921875 C27.34877014 64.89834396 27.36941528 66.09746918 27.39068604 67.33293152 C27.39191692 71.89910977 27.35007634 75.47110563 25.54614258 79.6965332 C21.46063138 82.42313876 17.61561788 82.11383977 12.80859375 82.03125 C11.82254654 82.03129028 10.83649933 82.03133057 9.8205719 82.03137207 C7.73847064 82.02601563 5.65636997 82.00842766 3.57446289 81.97949219 C0.38300003 81.937605 -2.80635529 81.9384259 -5.99804688 81.9453125 C-8.02084803 81.93482124 -10.0436383 81.92188348 -12.06640625 81.90625 C-13.02269424 81.90604858 -13.97898224 81.90584717 -14.96424866 81.90563965 C-15.85204514 81.88972778 -16.73984161 81.87381592 -17.65454102 81.85742188 C-18.43523453 81.84900269 -19.21592804 81.8405835 -20.02027893 81.83190918 C-22.68390192 81.31749016 -23.53786925 80.6814396 -25.0625 78.4375 C-25.42326355 75.83686829 -25.42326355 75.83686829 -25.40307617 72.6809082 C-25.40284958 71.49991043 -25.40262299 70.31891266 -25.40238953 69.10212708 C-25.38690567 67.82705795 -25.37142181 66.55198883 -25.35546875 65.23828125 C-25.35122391 63.93295944 -25.34697906 62.62763763 -25.34260559 61.28276062 C-25.33116089 57.81544721 -25.30169754 54.34851187 -25.26849365 50.88134766 C-25.23778433 47.34217442 -25.22409845 43.80293999 -25.20898438 40.26367188 C-25.17682705 33.32145945 -25.12563554 26.37949789 -25.0625 19.4375 C-27.7025 19.4375 -30.3425 19.4375 -33.0625 19.4375 C-33.625 17.5 -33.625 17.5 -34.0625 15.4375 C-33.0625 14.4375 -33.0625 14.4375 -29.68359375 14.33984375 C-28.28902923 14.34651631 -26.89448257 14.35847468 -25.5 14.375 C-24.78908203 14.37951172 -24.07816406 14.38402344 -23.34570312 14.38867188 C-21.58459987 14.40049136 -19.82353964 14.41842737 -18.0625 14.4375 C-18.12050781 13.84582031 -18.17851562 13.25414062 -18.23828125 12.64453125 C-18.28339844 11.85433594 -18.32851563 11.06414063 -18.375 10.25 C-18.43300781 9.47269531 -18.49101562 8.69539062 -18.55078125 7.89453125 C-16.66995176 -1.56980277 -7.60677437 -0.21187629 0 0 Z M-20.0625 20.4375 C-20.08540171 28.25207074 -20.10533103 36.066526 -20.11743164 43.88110352 C-20.12247383 46.54191481 -20.12930736 49.20272332 -20.13793945 51.86352539 C-20.15001009 55.67855516 -20.15572929 59.49354844 -20.16015625 63.30859375 C-20.16531754 64.50649033 -20.17047882 65.7043869 -20.17579651 66.93858337 C-20.17587204 68.0396492 -20.17594757 69.14071503 -20.17602539 70.27514648 C-20.178246 71.24999496 -20.18046661 72.22484344 -20.18275452 73.22923279 C-20.31031585 75.41222116 -20.31031585 75.41222116 -19.0625 76.4375 C-16.18985944 76.53856874 -13.34176372 76.57719672 -10.46875 76.5703125 C-9.1759613 76.57174759 -9.1759613 76.57174759 -7.85705566 76.57321167 C-6.03116788 76.5738935 -4.20527769 76.57203904 -2.37939453 76.56787109 C0.42551078 76.56252286 3.23025595 76.56781599 6.03515625 76.57421875 C7.80468786 76.57355797 9.57421936 76.57227681 11.34375 76.5703125 C12.18852905 76.57233673 13.03330811 76.57436096 13.90368652 76.57644653 C17.7293686 76.93674511 17.7293686 76.93674511 20.9375 75.4375 C21.0310919 73.71884591 21.05494611 71.99634256 21.05102539 70.27514648 C21.05094986 69.17408066 21.05087433 68.07301483 21.05079651 66.93858337 C21.04563522 65.7406868 21.04047394 64.54279022 21.03515625 63.30859375 C21.0337413 62.08922623 21.03232635 60.8698587 21.03086853 59.61354065 C21.02524967 55.7006693 21.01269396 51.78785419 21 47.875 C20.99498743 45.22981853 20.9904241 42.58463617 20.98632812 39.93945312 C20.97612296 33.43876274 20.95655121 26.93819467 20.9375 20.4375 C7.4075 20.4375 -6.1225 20.4375 -20.0625 20.4375 Z ' transform='translate(37.0625,0.5625)'/><path d='M0 0 C1.61358398 -0.01836914 1.61358398 -0.01836914 3.25976562 -0.03710938 C4.28650391 -0.03904297 5.31324219 -0.04097656 6.37109375 -0.04296875 C7.79119751 -0.04913208 7.79119751 -0.04913208 9.23999023 -0.05541992 C11.5625 0.1875 11.5625 0.1875 13.5625 2.1875 C13.5625 4.4975 13.5625 6.8075 13.5625 9.1875 C4.9825 9.1875 -3.5975 9.1875 -12.4375 9.1875 C-12.4375 -2.49561486 -10.02098891 0.00598268 0 0 Z ' fill='#FFFFFF' transform='translate(37.4375,5.8125)'/><path d='M0 0 C1.65 0.33 3.3 0.66 5 1 C5 7.93 5 14.86 5 22 C3.35 22.33 1.7 22.66 0 23 C-1.62200515 21.37799485 -1.1299075 19.57976969 -1.1328125 17.34375 C-1.13410156 16.40402344 -1.13539062 15.46429688 -1.13671875 14.49609375 C-1.13285156 13.50738281 -1.12898438 12.51867187 -1.125 11.5 C-1.12886719 10.51128906 -1.13273437 9.52257812 -1.13671875 8.50390625 C-1.13542969 7.56417969 -1.13414063 6.62445313 -1.1328125 5.65625 C-1.13168457 4.78806641 -1.13055664 3.91988281 -1.12939453 3.02539062 C-1 1 -1 1 0 0 Z ' transform='translate(28,37)'/><path d='M0 0 C2.475 0.495 2.475 0.495 5 1 C5 7.93 5 14.86 5 22 C3.35 22.33 1.7 22.66 0 23 C0 15.41 0 7.82 0 0 Z ' transform='translate(43,37)'/></svg>";
		public const string Folder =
			"<svg preserveAspectRatio='xMinYMin meet' id='folder' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M74.8,83h-21c-1.5,0-2.8-.6-3.7-1.8-.9-1.1-1.3-2.6-.9-4.1,1.6-7,7.9-12.2,15.1-12.2h0c7.2,0,13.6,5.1,15.1,12.2.3,1.4,0,2.9-.9,4.1-.9,1.1-2.3,1.8-3.7,1.8ZM74.6,78.2h0ZM54.2,78h20.4c-1.1-4.7-5.4-8-10.2-8s-9.1,3.4-10.2,8Z'/><path d='M64.4,59.9c-4.9,0-8.9-4-8.9-8.9s4-8.9,8.9-8.9,8.9,4,8.9,8.9-4,8.9-8.9,8.9ZM64.4,47c-2.2,0-3.9,1.8-3.9,3.9s1.8,3.9,3.9,3.9,3.9-1.8,3.9-3.9-1.8-3.9-3.9-3.9Z'/><path d='M86.6,95.7h-44.5c-4.4,0-8-3.6-8-8v-54.4c0-4.4,3.6-8,8-8h44.5c4.4,0,8,3.6,8,8v54.4c0,4.4-3.6,8-8,8ZM42.1,30.3c-1.7,0-3,1.4-3,3v54.4c0,1.7,1.4,3,3,3h44.5c1.7,0,3-1.4,3-3v-54.4c0-1.7-1.4-3-3-3h-44.5Z'/><path d='M36.6,43h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M36.6,83h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M36.6,69.7h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M36.6,56.3h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string Folder2 =
			"<svg preserveAspectRatio='xMinYMin meet' id='folder2' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M89.2,91.3H31.5c-4.5,0-8.2-3.7-8.2-8.2v-38.8c0-1.4,1.1-2.6,2.6-2.6h69c1.4,0,2.6,1.1,2.6,2.6v38.8c0,4.5-3.7,8.2-8.2,8.2ZM28.4,46.9v36.3c0,1.7,1.4,3.1,3.1,3.1h57.7c1.7,0,3.1-1.4,3.1-3.1v-36.3H28.4Z'/><path d='M94.9,46.9H25.9c-1.4,0-2.6-1.1-2.6-2.6v-5c0-8.6,7-15.6,15.6-15.6h6.3c3.1,0,6.1.9,8.6,2.6,3,2,6.8,3,11,3h23.8c4.8,0,8.7,3.9,8.7,8.7v6.2c0,1.4-1.1,2.6-2.6,2.6ZM28.4,41.7h63.9v-3.6c0-2-1.6-3.6-3.6-3.6h-23.8c-5.3,0-10.1-1.3-13.9-3.9-1.7-1.2-3.7-1.8-5.8-1.8h-6.3c-5.8,0-10.4,4.7-10.4,10.4v2.5Z'/></svg>";
		public const string GenerateFiles =
			"<svg preserveAspectRatio='xMinYMin meet' width='25' height='25' viewBox='0 0 25 25' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M13.5417 3.125L19.7917 9.375V21.875H5.20834V3.125H13.5417Z' stroke-width='2.08333' stroke-linecap='round' stroke-linejoin='round'/><path d='M13.0208 3.125V8.85417H19.7917' stroke-width='1.04167' stroke-linecap='round' stroke-linejoin='round'/></svg>";
		public const string Home =
			"<svg preserveAspectRatio='xMinYMin meet' width='22' height='21' viewBox='0 0 22 21' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M1 10.2844L9.955 1.32942C10.0594 1.22499 10.1833 1.14214 10.3197 1.08562C10.4561 1.02909 10.6023 1 10.75 1C10.8977 1 11.0439 1.02909 11.1803 1.08562C11.3167 1.14214 11.4406 1.22499 11.545 1.32942L20.5 10.2844M3.25 8.03442V18.1594C3.25 18.7804 3.754 19.2844 4.375 19.2844H8.5V14.4094C8.5 13.7884 9.004 13.2844 9.625 13.2844H11.875C12.496 13.2844 13 13.7884 13 14.4094V19.2844H17.125C17.746 19.2844 18.25 18.7804 18.25 18.1594V8.03442M7 19.2844H15.25' stroke-width='1.5' stroke-linecap='round' stroke-linejoin='round'/></svg>";
		public const string HomeFill =
			"<svg preserveAspectRatio='xMinYMin meet' id='home' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M93.4,97.2h-16.3c-3.5,0-6.3-2.8-6.3-6.3v-24.3c0-.7-.6-1.3-1.3-1.3h-19c-.7,0-1.3.6-1.3,1.3v24.3c0,3.5-2.8,6.3-6.3,6.3h-16.3c-3.5,0-6.3-2.8-6.3-6.3v-32.8c0-3.3,1.4-6.4,4-8.6l28.4-24.1c4.2-3.6,10.3-3.6,14.5,0l28.5,24.1c2.5,2.1,4,5.3,4,8.6v32.8c0,3.5-2.8,6.3-6.3,6.3ZM50.5,60.3h19c3.5,0,6.3,2.8,6.3,6.3v24.3c0,.7.6,1.3,1.3,1.3h16.3c.7,0,1.3-.6,1.3-1.3v-32.8c0-1.8-.8-3.6-2.2-4.7l-28.5-24.1c-2.3-2-5.7-2-8,0l-28.4,24.1c-1.4,1.2-2.2,2.9-2.2,4.7v32.8c0,.7.6,1.3,1.3,1.3h16.3c.7,0,1.3-.6,1.3-1.3v-24.3c0-3.5,2.8-6.3,6.3-6.3Z'/></svg>";
		public const string Image =
			"<svg preserveAspectRatio='xMinYMin meet' id='image' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M92.7,99H28.3c-3.5,0-6.3-2.8-6.3-6.3V28.3c0-3.5,2.8-6.3,6.3-6.3h64.5c3.5,0,6.3,2.8,6.3,6.3v64.5c0,3.5-2.8,6.3-6.3,6.3ZM28.3,27c-.7,0-1.3.6-1.3,1.3v64.5c0,.7.6,1.3,1.3,1.3h64.5c.7,0,1.3-.6,1.3-1.3V28.3c0-.7-.6-1.3-1.3-1.3H28.3Z'/><path d='M87.8,99h-54.6c-6.2,0-11.2-5-11.2-11.2v-22.4c0-.7.3-1.3.7-1.8l11.5-11.5c3.7-3.7,9.8-3.7,13.6,0l15.1,15.2c.9.9,2,1.3,3.3,1.3s2.4-.5,3.3-1.3c3.8-3.7,9.9-3.7,13.6,0l15.3,15.3c.5.5.7,1.1.7,1.8v3.4c0,6.2-5,11.2-11.2,11.2ZM27,66.5v21.3c0,3.4,2.8,6.2,6.2,6.2h54.6c3.4,0,6.2-2.8,6.2-6.2v-2.4l-14.5-14.5c-1.8-1.8-4.7-1.8-6.5,0-1.8,1.8-4.2,2.8-6.8,2.8s-5-1-6.8-2.8l-15.1-15.1c-1.8-1.8-4.7-1.8-6.5,0l-10.7,10.7Z'/><path d='M68.9,55.7c-5.9,0-10.7-4.8-10.7-10.7s4.8-10.7,10.7-10.7,10.7,4.8,10.7,10.7-4.8,10.7-10.7,10.7ZM68.9,39.3c-3.2,0-5.7,2.6-5.7,5.7s2.6,5.7,5.7,5.7,5.7-2.6,5.7-5.7-2.6-5.7-5.7-5.7Z'/></svg>";
		public const string Info =
			"<svg preserveAspectRatio='xMinYMin meet' id='infos' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M48.2,99.9h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM26.1,94.9h19.6v-19.6h-19.6v19.6Z'/><path d='M72.8,99.9h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM50.7,94.9h19.6v-19.6h-19.6v19.6Z'/><path d='M97.4,99.9h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM75.3,94.9h19.6v-19.6h-19.6v19.6Z'/><path d='M97.4,75.3h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM75.3,70.3h19.6v-19.6h-19.6v19.6Z'/><path d='M97.4,50.7h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM75.3,45.7h19.6v-19.6h-19.6v19.6Z'/><path d='M72.8,75.3h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM50.7,70.3h19.6v-19.6h-19.6v19.6Z'/></svg>";
		public const string InfoFile =
			"<svg preserveAspectRatio='xMinYMin meet' id='infos2' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M102,96.4H29.1c-6.9,0-12.6-5.6-12.6-12.6V27.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v56.5c0,4.2,3.4,7.6,7.6,7.6h72.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M34.1,85.3c-1.4,0-2.5-1.1-2.5-2.5v-19.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v19.3c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M49.2,85.3c-1.4,0-2.5-1.1-2.5-2.5v-28.6c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v28.6c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M64.3,85.3c-1.4,0-2.5-1.1-2.5-2.5v-19.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v19.3c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M79.4,85.3c-1.4,0-2.5-1.1-2.5-2.5v-28.6c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v28.6c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M64.3,50.1h0c-.7,0-1.3-.3-1.8-.7l-13.3-13.3-13.3,13.3c-1,1-2.6,1-3.5,0-1-1-1-2.6,0-3.5l15-15c.5-.5,1.1-.8,1.8-.8s1.4.3,1.8.8l13.3,13.3,18.5-18.5c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-20.2,20.2c-.5.5-1.1.7-1.8.7Z'/></svg>";
		public const string Lock =
			"<svg preserveAspectRatio='xMinYMin meet' id='lock' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M83.9,93.7h-46.8c-4.3,0-7.8-3.5-7.8-7.8v-32.4c0-4.3,3.5-7.8,7.8-7.8h46.8c4.3,0,7.8,3.5,7.8,7.8v32.4c0,4.3-3.5,7.8-7.8,7.8ZM37.1,50.5c-1.6,0-2.8,1.3-2.8,2.8v32.4c0,1.6,1.3,2.8,2.8,2.8h46.8c1.6,0,2.8-1.3,2.8-2.8v-32.4c0-1.6-1.3-2.8-2.8-2.8h-46.8Z'/><path d='M78.7,50.5h-36.4c-1.4,0-2.5-1.1-2.5-2.5v-10.3c0-5.7,4.7-10.4,10.4-10.4h20.6c5.7,0,10.4,4.7,10.4,10.4v10.3c0,1.4-1.1,2.5-2.5,2.5ZM44.8,45.5h31.4v-7.8c0-3-2.4-5.4-5.4-5.4h-20.6c-3,0-5.4,2.4-5.4,5.4v7.8Z'/><path d='M60.5,77.2c-1.4,0-2.5-1.1-2.5-2.5v-10.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v10.3c0,1.4-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string New =
			"<svg preserveAspectRatio='xMinYMin meet' id='new' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M69.2,70.7h-17.9c-1.5,0-2.7-1.2-2.7-2.7s1.2-2.7,2.7-2.7h17.9c1.5,0,2.7,1.2,2.7,2.7s-1.2,2.7-2.7,2.7Z'/><g><g><path d='M90.3,93.9H30.1c-4.7,0-8.5-3.8-8.5-8.5v-40.4c0-1.5,1.2-2.7,2.7-2.7h71.9c1.5,0,2.7,1.2,2.7,2.7v40.4c0,4.7-3.8,8.5-8.5,8.5ZM26.9,47.5v37.8c0,1.8,1.4,3.2,3.2,3.2h60.2c1.8,0,3.2-1.4,3.2-3.2v-37.8H26.9Z'/><path d='M96.2,47.5H24.2c-1.5,0-2.7-1.2-2.7-2.7v-5.2c0-8.9,7.3-16.2,16.2-16.2h6.6c3.2,0,6.3.9,9,2.7,3.1,2.1,7.1,3.2,11.5,3.2h24.8c5,0,9.1,4.1,9.1,9.1v6.4c0,1.5-1.2,2.7-2.7,2.7ZM26.9,42.2h66.6v-3.8c0-2.1-1.7-3.8-3.8-3.8h-24.8c-5.5,0-10.5-1.4-14.5-4.1-1.8-1.2-3.9-1.8-6-1.8h-6.6c-6,0-10.9,4.9-10.9,10.9v2.6Z'/></g><path d='M60.2,79.7c-1.5,0-2.7-1.2-2.7-2.7v-17.9c0-1.5,1.2-2.7,2.7-2.7s2.7,1.2,2.7,2.7v17.9c0,1.5-1.2,2.7-2.7,2.7Z'/></g></svg>";
		public const string Open =
			"<svg preserveAspectRatio='xMinYMin meet' id='open' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><g><path d='M60.5,74.4c-1.2,0-2.4-.5-3.4-1.4l-8.4-8.4c-1-1-1-2.6,0-3.5s2.6-1,3.5,0l8.3,8.3,8.3-8.3c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-8.4,8.4c-.9.9-2.1,1.4-3.4,1.4Z'/><path d='M60.5,74.4c-1.4,0-2.5-1.1-2.5-2.5v-23.5c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v23.5c0,1.4-1.1,2.5-2.5,2.5Z'/></g><path d='M60.5,93.9c-3,0-5.8-1.2-7.9-3.3l-4.8-4.8c-1.2-1.2-2.7-1.8-4.4-1.8h-7.6c-6.2,0-11.2-5-11.2-11.2v-34.3c0-6.2,5-11.2,11.2-11.2h49.5c6.2,0,11.2,5,11.2,11.2v34.3c0,6.2-5,11.2-11.2,11.2h-7.6c-1.7,0-3.2.6-4.4,1.8l-4.8,4.8c-2.1,2.1-4.9,3.3-7.9,3.3ZM35.7,32.1c-3.4,0-6.2,2.8-6.2,6.2v34.3c0,3.4,2.8,6.2,6.2,6.2h7.6c3,0,5.8,1.2,7.9,3.3l4.8,4.8c1.2,1.2,2.7,1.8,4.4,1.8s3.2-.6,4.4-1.8l4.8-4.8c2.1-2.1,4.9-3.3,7.9-3.3h7.6c3.4,0,6.2-2.8,6.2-6.2v-34.3c0-3.4-2.8-6.2-6.2-6.2h-49.5Z'/></svg>";

		public const string PictoAccueilSvgOption2 =
			"<svg preserveAspectRatio='xMinYMin meet'  style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M93.4,97.2h-16.3c-3.5,0-6.3-2.8-6.3-6.3v-24.3c0-.7-.6-1.3-1.3-1.3h-19c-.7,0-1.3.6-1.3,1.3v24.3c0,3.5-2.8,6.3-6.3,6.3h-16.3c-3.5,0-6.3-2.8-6.3-6.3v-32.8c0-3.3,1.4-6.4,4-8.6l28.4-24.1c4.2-3.6,10.3-3.6,14.5,0l28.5,24.1c2.5,2.1,4,5.3,4,8.6v32.8c0,3.5-2.8,6.3-6.3,6.3ZM50.5,60.3h19c3.5,0,6.3,2.8,6.3,6.3v24.3c0,.7.6,1.3,1.3,1.3h16.3c.7,0,1.3-.6,1.3-1.3v-32.8c0-1.8-.8-3.6-2.2-4.7l-28.5-24.1c-2.3-2-5.7-2-8,0l-28.4,24.1c-1.4,1.2-2.2,2.9-2.2,4.7v32.8c0,.7.6,1.3,1.3,1.3h16.3c.7,0,1.3-.6,1.3-1.3v-24.3c0-3.5,2.8-6.3,6.3-6.3Z' /></svg>";
		public const string PictoAdresse =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,63.6c-6.8,0-12.4-5.5-12.4-12.4s5.5-12.4,12.4-12.4,12.4,5.5,12.4,12.4-5.5,12.4-12.4,12.4ZM60.5,43.9c-4.1,0-7.4,3.3-7.4,7.4s3.3,7.4,7.4,7.4,7.4-3.3,7.4-7.4-3.3-7.4-7.4-7.4Z' /><path d='M60.5,100.2c-9.3,0-30.4-25.1-30.4-48.9s13.7-30.4,30.4-30.4,30.4,13.7,30.4,30.4c0,23.9-21.1,48.9-30.4,48.9ZM60.5,25.8c-14,0-25.4,11.4-25.4,25.4,0,23.1,20.8,43.9,25.4,43.9s25.4-20.8,25.4-43.9-11.4-25.4-25.4-25.4Z' /></svg>";
		public const string PictoAlerte =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M24.2,38.9c-.9,0-1.7-.4-2.2-1.2-.7-1.2-.3-2.7.9-3.4l16.3-9.6c2.2-1.3,4.5-2.4,6.7-3.2,1.3-.5,2.7.2,3.2,1.5.5,1.3-.2,2.7-1.5,3.2-2,.7-3.9,1.6-5.9,2.8l-16.3,9.6c-.4.2-.8.3-1.3.3Z' /> <path d='M84.8,99.5c-.4,0-.9-.1-1.3-.3-1.2-.7-1.6-2.2-.9-3.4l9.6-16.3c1.1-1.9,2-3.9,2.8-5.9.5-1.3,1.9-2,3.2-1.5,1.3.5,2,1.9,1.5,3.2-.8,2.3-1.9,4.5-3.2,6.7l-9.6,16.3c-.5.8-1.3,1.2-2.2,1.2Z' /> <path d='M61.9,99.9c-3.7,0-7.2-1.5-9.9-4.1l-26.6-26.6c-3.1-3.1-4.6-7.4-4-11.7.6-4.4,3.1-8.1,6.8-10.3l18-10.5c11-6.4,25-4.6,34,4.4h0s0,0,0,0c9,9,10.8,23,4.4,34l-10.5,18c-2.2,3.8-6,6.3-10.3,6.8-.6,0-1.2.1-1.8.1ZM60.4,37.8c-4,0-8,1-11.6,3.2l-18,10.5c-2.4,1.4-4,3.9-4.4,6.7-.4,2.8.6,5.6,2.6,7.6l26.6,26.6c2,2,4.7,2.9,7.6,2.6,2.8-.4,5.2-2,6.7-4.4l10.5-18c5.3-9,3.8-20.5-3.6-27.9h0c-4.4-4.4-10.3-6.7-16.3-6.7Z' /> <path d='M40.5,96.5c-4,0-8.1-1.5-11.2-4.6h0s0,0,0,0c-3-3-4.6-6.9-4.6-11.2s1.6-8.2,4.6-11.2c1-1,2.6-1,3.5,0l18.8,18.8c1,1,1,2.6,0,3.5-3.1,3.1-7.1,4.6-11.2,4.6ZM31.3,75.1c-1,1.7-1.6,3.6-1.6,5.7,0,2.9,1.1,5.6,3.2,7.6h0c3.6,3.6,9.1,4.1,13.3,1.6l-14.8-14.8Z' /> <path d='M78.4,45.3c-.6,0-1.3-.2-1.8-.7-1-1-1-2.6,0-3.5l5.1-5.1c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-5.1,5.1c-.5.5-1.1.7-1.8.7Z' /> </svg>";
		public const string PictoArgent =
			"<svg preserveAspectRatio='xMinYMin meet' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path class='st0' d='M49.2,66.3c-12.7,0-22.4-5.1-22.4-11.8s9.6-11.8,22.4-11.8,22.4,5.1,22.4,11.8-9.6,11.8-22.4,11.8ZM49.2,47.7c-10.6,0-17.4,4-17.4,6.8s6.8,6.8,17.4,6.8,17.4-4,17.4-6.8-6.8-6.8-17.4-6.8Z'/>  <path class='st0' d='M49.2,80.6c-12.7,0-22.4-5.1-22.4-11.8s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,2.8,6.8,6.8,17.4,6.8s17.4-4,17.4-6.8,1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,6.7-9.6,11.8-22.4,11.8Z'/>  <path class='st0' d='M49.2,94.8c-12.7,0-22.4-5.1-22.4-11.8s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,2.8,6.8,6.8,17.4,6.8s17.4-4,17.4-6.8,1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,6.7-9.6,11.8-22.4,11.8Z'/>  <path class='st0' d='M29.3,86.4c-1.4,0-2.5-1.1-2.5-2.5v-25.7c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v25.7c0,1.4-1.1,2.5-2.5,2.5Z'/>  <path class='st0' d='M69,85.5c-1.4,0-2.5-1.1-2.5-2.5v-28.5c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v28.5c0,1.4-1.1,2.5-2.5,2.5Z'/>  <path class='st0' d='M70.8,73.5c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5c10.6,0,17.4-4,17.4-6.8s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,6.7-9.6,11.8-22.4,11.8Z'/>  <path class='st0' d='M70.8,45c-12.7,0-22.4-5.1-22.4-11.8s9.6-11.8,22.4-11.8,22.4,5.1,22.4,11.8-9.6,11.8-22.4,11.8ZM70.8,26.3c-10.6,0-17.4,4-17.4,6.8s6.8,6.8,17.4,6.8,17.4-4,17.4-6.8-6.8-6.8-17.4-6.8Z'/>  <path class='st0' d='M70.8,59.2c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5c10.6,0,17.4-4,17.4-6.8s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,6.7-9.6,11.8-22.4,11.8Z'/>  <path class='st0' d='M70.8,87.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5c10.6,0,17.4-4,17.4-6.8s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,6.7-9.6,11.8-22.4,11.8Z'/>  <path class='st0' d='M90.7,78.4c-1.4,0-2.5-1.1-2.5-2.5v-42.7c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v42.7c0,1.4-1.1,2.5-2.5,2.5Z'/>  <path class='st0' d='M51,47.7c-1.4,0-2.5-1.1-2.5-2.5v-12.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v12.1c0,1.4-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string PictoAstuce =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,97.6c-5.9,0-10.6-4.8-10.6-10.6v-3.4c0-5.8-2.4-11.3-6.8-15.2-5.4-4.9-8.4-11.8-8.4-19,0-14.2,11.6-25.8,25.8-25.8s25.8,11.6,25.8,25.8-2.7,13.5-7.7,18.4c-.1.2-.2.3-.4.4-4.5,3.9-7,9.6-7,15.5v3.4c0,5.9-4.8,10.6-10.6,10.6ZM60.5,28.4c-11.5,0-20.8,9.3-20.8,20.8s2.5,11.4,6.8,15.3c5.4,4.9,8.5,11.8,8.5,18.9v3.4c0,3.1,2.5,5.6,5.6,5.6s5.6-2.5,5.6-5.6v-3.4c0-7.1,3-14,8.3-18.8,0-.1.2-.2.3-.3,4.2-4,6.6-9.4,6.6-15.1,0-11.5-9.3-20.8-20.8-20.8Z' /> <path d='M68.8,84.1h-16.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h16.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M60.6,84.1c-1.4,0-2.5-1.1-2.5-2.5v-27.9c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v27.9c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M60.6,56.2c-4.5,0-8.3-3.7-8.3-8.3s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5,1.5,3.3,3.3,3.3,3.3-1.5,3.3-3.3,1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,4.6-3.7,8.3-8.3,8.3Z' /> </svg>";
		public const string PictoBulleInfo =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,96.3c-19.7,0-35.8-16-35.8-35.8S40.8,24.7,60.5,24.7s35.8,16,35.8,35.8-16,35.8-35.8,35.8ZM60.5,29.7c-17,0-30.8,13.8-30.8,30.8s13.8,30.8,30.8,30.8,30.8-13.8,30.8-30.8-13.8-30.8-30.8-30.8Z' /><g><path d='M60.5,67.2c-1.4,0-2.5-1.1-2.5-2.5v-18.8c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v18.8c0,1.4-1.1,2.5-2.5,2.5Z' /><path d='M60.5,77.6c-.7,0-1.3-.3-1.8-.7-.5-.5-.7-1.1-.7-1.8s.3-1.3.7-1.8c.1-.1.2-.2.4-.3.1,0,.3-.2.4-.2.2,0,.3-.1.5-.1.8-.2,1.7.1,2.2.7.5.5.7,1.1.7,1.8s-.3,1.3-.7,1.8c-.5.5-1.1.7-1.8.7Z' /></g></svg>";
		public const string PictoConnexion =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <g id='bKPW0V'><g> <path d='M61.9,30.5h-29.2c-1.2,0-4.1,4.8-4.4,6.5-1.4,8.5-1,35.2-.1,44.3.2,2.2,2.3,8.3,4.5,8.3h29.2v-6.9h-26.8v-45.3h26.8v-6.9ZM80.8,63.9l-6.9,7.5,4.4,5.3,16.3-17.1-16.3-16.4-4.4,5.3,6.9,7.5h-31.2c-2.3,1.9-2.2,6.1,0,7.9h31.2Z' /> <path d='M61.9,30.5v6.9h-26.8v45.3h26.8v6.9h-29.2c-2.2,0-4.3-6.1-4.5-8.3-.8-9.1-1.3-35.8.1-44.3.3-1.7,3.2-6.5,4.4-6.5h29.2Z' /> <path d='M80.8,63.9h-31.2c-2.2-1.8-2.3-6,0-7.9h31.2l-6.9-7.5,4.4-5.3,16.3,16.4-16.3,17.1-4.4-5.3,6.9-7.5Z' /> </g> </g> </svg>";
		public const string PictoConsulterFiche =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <g> <path d='M64.4,85.4h-20.3c-7.1,0-12.9-5.8-12.9-12.9v-35.9c0-7.1,5.8-12.9,12.9-12.9h14.5c1.5,0,3,.3,4.4,1,1,.4,2,1.1,2.9,2l8.4,8.4c.8.8,1.5,1.8,2,2.9.6,1.3,1,2.8,1,4.3v30.2c0,7.1-5.8,12.9-12.9,12.9ZM44.1,28.8c-4.3,0-7.9,3.5-7.9,7.9v35.9c0,4.3,3.5,7.9,7.9,7.9h20.3c4.3,0,7.9-3.5,7.9-7.9v-30.2c0-.8-.2-1.5-.5-2.2-.3-.6-.6-1.1-1-1.5l-8.4-8.4c-.4-.4-.9-.8-1.4-1-.7-.4-1.5-.5-2.2-.5h-14.5Z' /> <path d='M74,41.6h-12.1c-1.4,0-2.5-1.1-2.5-2.5v-12.1c0-.8.4-1.6,1.1-2.1.7-.5,1.6-.5,2.4-.2,1.1.5,2.1,1.2,2.9,2l8.4,8.4c.8.8,1.5,1.8,2,2.9.4.8.3,1.7-.2,2.4-.5.7-1.3,1.2-2.1,1.2ZM64.4,36.6h4.3l-4.3-4.3v4.3Z' /> </g> <path d='M68,97.2h-19.6c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h19.6c9.2,0,16.7-7.5,16.7-16.7v-27.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v27.1c0,12-9.8,21.7-21.7,21.7Z' /></svg>";
		public const string PictoCreerDocument =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M83.5,108h-46.3c-7.9,0-14.3-6.4-14.3-14.2V23.4c0-7.9,6.4-14.3,14.3-14.3h19.1c3.2,0,6.3.7,9.1,2.1,2.2,1,4.2,2.4,6,4.2l20,19.9c1.8,1.8,3.2,3.8,4.3,6.1,1.3,2.8,2,5.9,2,9v43.2c0,7.9-6.4,14.3-14.3,14.3ZM37.2,15.5c-4.4,0-7.9,3.5-7.9,7.9v70.4c0,4.3,3.5,7.9,7.9,7.9h46.3c4.4,0,7.9-3.5,7.9-7.9v-43.2c0-2.2-.5-4.4-1.4-6.3-.7-1.6-1.8-3.1-3-4.3l-20-19.9c-1.3-1.3-2.7-2.3-4.2-3-2.1-1-4.2-1.5-6.4-1.5h-19.1Z' /> <path d='M92.8,46h-28.8c-1.8,0-3.2-1.4-3.2-3.2V14.1c0-1.1.5-2.1,1.4-2.7s2-.7,3-.2c2.3,1,4.3,2.4,6.1,4.3l20,19.9c1.8,1.8,3.2,3.8,4.3,6.1.5,1,.4,2.1-.2,3-.6.9-1.6,1.5-2.7,1.5ZM67.2,39.7h19.5l-19.5-19.5v19.5Z' /> <path d='M69.4,72.5h-21.5c-1.8,0-3.2-1.4-3.2-3.2s1.4-3.2,3.2-3.2h21.5c1.8,0,3.2,1.4,3.2,3.2s-1.4,3.2-3.2,3.2Z' /> <path d='M58.7,83.2c-1.8,0-3.2-1.4-3.2-3.2v-21.5c0-1.8,1.4-3.2,3.2-3.2s3.2,1.4,3.2,3.2v21.5c0,1.8-1.4,3.2-3.2,3.2Z' /> </svg>";
		public const string PictoCreerDossier =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M69.2,70.7h-17.9c-1.5,0-2.7-1.2-2.7-2.7s1.2-2.7,2.7-2.7h17.9c1.5,0,2.7,1.2,2.7,2.7s-1.2,2.7-2.7,2.7Z' /><g> <g> <path d='M90.3,93.9H30.1c-4.7,0-8.5-3.8-8.5-8.5v-40.4c0-1.5,1.2-2.7,2.7-2.7h71.9c1.5,0,2.7,1.2,2.7,2.7v40.4c0,4.7-3.8,8.5-8.5,8.5ZM26.9,47.5v37.8c0,1.8,1.4,3.2,3.2,3.2h60.2c1.8,0,3.2-1.4,3.2-3.2v-37.8H26.9Z' /><path d='M96.2,47.5H24.2c-1.5,0-2.7-1.2-2.7-2.7v-5.2c0-8.9,7.3-16.2,16.2-16.2h6.6c3.2,0,6.3.9,9,2.7,3.1,2.1,7.1,3.2,11.5,3.2h24.8c5,0,9.1,4.1,9.1,9.1v6.4c0,1.5-1.2,2.7-2.7,2.7ZM26.9,42.2h66.6v-3.8c0-2.1-1.7-3.8-3.8-3.8h-24.8c-5.5,0-10.5-1.4-14.5-4.1-1.8-1.2-3.9-1.8-6-1.8h-6.6c-6,0-10.9,4.9-10.9,10.9v2.6Z' /></g> <path d='M60.2,79.7c-1.5,0-2.7-1.2-2.7-2.7v-17.9c0-1.5,1.2-2.7,2.7-2.7s2.7,1.2,2.7,2.7v17.9c0,1.5-1.2,2.7-2.7,2.7Z' /></g> </svg>";
		public const string PictoDocumentation =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M42.1,94.1h-10.2c-2.1,0-3.7-1.7-3.7-3.7V28.8c0-2.1,1.7-3.7,3.7-3.7h10.2c2.1,0,3.7,1.7,3.7,3.7v61.6c0,2.1-1.7,3.7-3.7,3.7ZM32.7,89.6h8.5V29.6h-8.5v60Z' /> <path d='M43.5,79.9h-13.1c-1.3,0-2.3-1-2.3-2.3v-36.1c0-1.3,1-2.3,2.3-2.3h13.1c1.3,0,2.3,1,2.3,2.3v36.1c0,1.3-1,2.3-2.3,2.3ZM32.7,75.4h8.5v-31.6h-8.5v31.6Z' /> <g> <path d='M55.2,94.1h-10.2c-2.1,0-3.7-1.7-3.7-3.7V28.8c0-2.1,1.7-3.7,3.7-3.7h10.2c2.1,0,3.7,1.7,3.7,3.7v61.6c0,2.1-1.7,3.7-3.7,3.7ZM45.8,89.6h8.5V29.6h-8.5v60Z' /> <path d='M56.6,79.9h-13.1c-1.3,0-2.3-1-2.3-2.3v-36.1c0-1.3,1-2.3,2.3-2.3h13.1c1.3,0,2.3,1,2.3,2.3v36.1c0,1.3-1,2.3-2.3,2.3ZM45.8,75.4h8.5v-31.6h-8.5v31.6Z' /> </g> <g> <path d='M82.7,94.9c-.5,0-1,0-1.4-.3-.9-.4-1.6-1.1-2-2l-23.5-56.9c-.8-1.9.1-4.1,2-4.9l9.4-3.9c.9-.4,1.9-.4,2.9,0,.9.4,1.6,1.1,2,2l23.5,56.9c.4.9.4,1.9,0,2.9-.4.9-1.1,1.6-2,2h0l-9.4,3.9c-.5.2-.9.3-1.4.3ZM60.3,34.7l22.9,55.4,7.9-3.3-22.9-55.4-7.9,3.3ZM92.6,88.7h0,0ZM91.8,86.6s0,0,0,0h0Z' /> <path d='M76.5,82.3c-.3,0-.6,0-.9-.2-.6-.2-1-.7-1.2-1.2l-13.8-33.4c-.2-.6-.2-1.2,0-1.7.2-.6.7-1,1.2-1.2l12.1-5c1.2-.5,2.5,0,3,1.2l13.8,33.4c.2.6.2,1.2,0,1.7-.2.6-.7,1-1.2,1.2l-12.1,5c-.3.1-.6.2-.9.2ZM65.7,47.8l12,29.2,7.9-3.3-12-29.2-7.9,3.3Z' /> </g> </svg>";
		public const string PictoDossier =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M89.2,91.3H31.5c-4.5,0-8.2-3.7-8.2-8.2v-38.8c0-1.4,1.1-2.6,2.6-2.6h69c1.4,0,2.6,1.1,2.6,2.6v38.8c0,4.5-3.7,8.2-8.2,8.2ZM28.4,46.9v36.3c0,1.7,1.4,3.1,3.1,3.1h57.7c1.7,0,3.1-1.4,3.1-3.1v-36.3H28.4Z' /> <path d='M94.9,46.9H25.9c-1.4,0-2.6-1.1-2.6-2.6v-5c0-8.6,7-15.6,15.6-15.6h6.3c3.1,0,6.1.9,8.6,2.6,3,2,6.8,3,11,3h23.8c4.8,0,8.7,3.9,8.7,8.7v6.2c0,1.4-1.1,2.6-2.6,2.6ZM28.4,41.7h63.9v-3.6c0-2-1.6-3.6-3.6-3.6h-23.8c-5.3,0-10.1-1.3-13.9-3.9-1.7-1.2-3.7-1.8-5.8-1.8h-6.3c-5.8,0-10.4,4.7-10.4,10.4v2.5Z' /> </svg>";
		public const string PictoEnregistrer =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M88.1,96.4h-55.3c-4.3,0-7.7-3.5-7.7-7.7v-56.2c0-4.3,3.5-7.7,7.7-7.7h42.4c1.7,0,3.3.4,4.8,1.1,1.1.5,2.2,1.2,3.1,2.2l9.4,9.4c.9.9,1.7,2,2.2,3.2.7,1.4,1,3.1,1,4.7v43.4c0,4.3-3.5,7.7-7.7,7.7ZM32.9,29.6c-1.5,0-2.7,1.2-2.7,2.7v56.2c0,1.5,1.2,2.7,2.7,2.7h55.3c1.5,0,2.7-1.2,2.7-2.7v-43.4c0-.9-.2-1.8-.6-2.6-.3-.7-.7-1.3-1.2-1.8l-9.4-9.4c-.5-.5-1.1-.9-1.7-1.2-.9-.4-1.7-.6-2.6-.6h-42.4Z' /> <path d='M75.8,45.7h-30.6c-2.8,0-5.2-2.3-5.2-5.2v-13.3c0-1.4,1.1-2.5,2.5-2.5h35.9c1.4,0,2.5,1.1,2.5,2.5v13.3c0,2.8-2.3,5.2-5.2,5.2ZM45,29.8v10.8c0,0,0,.2.2.2h30.6c0,0,.2,0,.2-.2v-10.8h-30.9Z' /> <path d='M60.5,82.1c-7,0-12.6-5.7-12.6-12.6s5.7-12.6,12.6-12.6,12.6,5.7,12.6,12.6-5.7,12.6-12.6,12.6ZM60.5,61.9c-4.2,0-7.6,3.4-7.6,7.6s3.4,7.6,7.6,7.6,7.6-3.4,7.6-7.6-3.4-7.6-7.6-7.6Z' /> </svg>";
		public const string PictoFiche =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path class='st0' d='M69,94.8h-23c-8.2,0-14.9-6.7-14.9-14.9v-43.4c0-8.2,6.7-14.9,14.9-14.9h16.3c1.8,0,3.4.4,5,1.1,1.1.5,2.3,1.3,3.3,2.3l9.9,9.9c1,1,1.7,2.1,2.3,3.3.7,1.5,1.1,3.2,1.1,4.9v36.8c0,8.2-6.7,14.9-14.9,14.9ZM46.1,26.5c-5.5,0-9.9,4.5-9.9,9.9v43.4c0,5.5,4.5,9.9,9.9,9.9h23c5.5,0,9.9-4.5,9.9-9.9v-36.8c0-1-.2-1.9-.6-2.8-.3-.7-.8-1.4-1.3-1.9l-9.9-9.9c-.6-.6-1.2-1-1.8-1.3-.9-.5-1.9-.7-2.8-.7h-16.3Z'/>  <path class='st0' d='M80.6,41.7h-14.3c-1.4,0-2.5-1.1-2.5-2.5v-14.3c0-.8.4-1.6,1.1-2.1.7-.5,1.6-.5,2.4-.2,1.2.5,2.3,1.3,3.3,2.3l9.9,9.9c1,1,1.7,2.1,2.3,3.3.4.8.3,1.7-.2,2.4-.5.7-1.3,1.2-2.1,1.2ZM68.7,36.7h6.6l-6.6-6.6v6.6Z'/>  <path class='st0' d='M57.5,54.2h-10.3c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h10.3c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/>  <path class='st0' d='M69.8,66.9h-22.6c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22.6c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/>  <path class='st0' d='M69.8,80c-2.1,0-3.3-.2-4.3-.5-.9-.2-1.6-.3-3.2-.3s-2.4.2-3.2.3c-1,.2-2.1.5-4.3.5s-3.3-.2-4.3-.5c-.9-.2-1.6-.3-3.2-.3s-2.5-1.1-2.5-2.5,1.1-2.5,2.5-2.5c2.1,0,3.3.2,4.3.5.9.2,1.6.3,3.2.3s2.4-.2,3.2-.3c1-.2,2.1-.5,4.3-.5s3.3.2,4.3.5c.9.2,1.6.3,3.2.3s2.5,1.1,2.5,2.5-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string PictoFicheDossier =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M74.8,83h-21c-1.5,0-2.8-.6-3.7-1.8-.9-1.1-1.3-2.6-.9-4.1,1.6-7,7.9-12.2,15.1-12.2h0c7.2,0,13.6,5.1,15.1,12.2.3,1.4,0,2.9-.9,4.1-.9,1.1-2.3,1.8-3.7,1.8ZM74.6,78.2h0ZM54.2,78h20.4c-1.1-4.7-5.4-8-10.2-8s-9.1,3.4-10.2,8Z' /> <path d='M64.4,59.9c-4.9,0-8.9-4-8.9-8.9s4-8.9,8.9-8.9,8.9,4,8.9,8.9-4,8.9-8.9,8.9ZM64.4,47c-2.2,0-3.9,1.8-3.9,3.9s1.8,3.9,3.9,3.9,3.9-1.8,3.9-3.9-1.8-3.9-3.9-3.9Z' /> <path d='M86.6,95.7h-44.5c-4.4,0-8-3.6-8-8v-54.4c0-4.4,3.6-8,8-8h44.5c4.4,0,8,3.6,8,8v54.4c0,4.4-3.6,8-8,8ZM42.1,30.3c-1.7,0-3,1.4-3,3v54.4c0,1.7,1.4,3,3,3h44.5c1.7,0,3-1.4,3-3v-54.4c0-1.7-1.4-3-3-3h-44.5Z' /> <path d='M36.6,43h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M36.6,83h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M36.6,69.7h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M36.6,56.3h-7.7c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoFicheInfo =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M48.2,99.9h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM26.1,94.9h19.6v-19.6h-19.6v19.6Z' /> <path d='M72.8,99.9h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM50.7,94.9h19.6v-19.6h-19.6v19.6Z' /> <path d='M97.4,99.9h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM75.3,94.9h19.6v-19.6h-19.6v19.6Z' /> <path d='M97.4,75.3h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM75.3,70.3h19.6v-19.6h-19.6v19.6Z' /> <path d='M97.4,50.7h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM75.3,45.7h19.6v-19.6h-19.6v19.6Z' /> <path d='M72.8,75.3h-24.6c-1.4,0-2.5-1.1-2.5-2.5v-24.6c0-1.4,1.1-2.5,2.5-2.5h24.6c1.4,0,2.5,1.1,2.5,2.5v24.6c0,1.4-1.1,2.5-2.5,2.5ZM50.7,70.3h19.6v-19.6h-19.6v19.6Z' /> </svg>";
		public const string PictoFicheInfo2 =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M102,96.4H29.1c-6.9,0-12.6-5.6-12.6-12.6V27.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v56.5c0,4.2,3.4,7.6,7.6,7.6h72.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M34.1,85.3c-1.4,0-2.5-1.1-2.5-2.5v-19.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v19.3c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M49.2,85.3c-1.4,0-2.5-1.1-2.5-2.5v-28.6c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v28.6c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M64.3,85.3c-1.4,0-2.5-1.1-2.5-2.5v-19.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v19.3c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M79.4,85.3c-1.4,0-2.5-1.1-2.5-2.5v-28.6c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v28.6c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M64.3,50.1h0c-.7,0-1.3-.3-1.8-.7l-13.3-13.3-13.3,13.3c-1,1-2.6,1-3.5,0-1-1-1-2.6,0-3.5l15-15c.5-.5,1.1-.8,1.8-.8s1.4.3,1.8.8l13.3,13.3,18.5-18.5c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-20.2,20.2c-.5.5-1.1.7-1.8.7Z' /> </svg>";
		public const string PictoFicheModification =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,72.9c-6.8,0-12.4-5.5-12.4-12.4s5.5-12.4,12.4-12.4,12.4,5.5,12.4,12.4-5.5,12.4-12.4,12.4ZM60.5,53.1c-4.1,0-7.4,3.3-7.4,7.4s3.3,7.4,7.4,7.4,7.4-3.3,7.4-7.4-3.3-7.4-7.4-7.4Z' /> <path d='M62.6,91.3h-4.3c-2,0-3.8-1.2-4.6-3l-2.4-5.4c-2.1-.8-4-1.9-5.7-3.3l-5.9.6c-2,.2-3.9-.8-4.9-2.5l-2.1-3.7c-1-1.7-.9-3.9.3-5.5l3.5-4.7c-.2-1.2-.2-2.3-.2-3.3s0-2.2.2-3.3l-3.4-4.8c-1.2-1.6-1.3-3.8-.3-5.5l2.1-3.7c1-1.7,2.9-2.7,4.9-2.5l5.8.6c1.8-1.4,3.7-2.5,5.7-3.3l2.4-5.4c.8-1.8,2.6-3,4.6-3h4.3c2,0,3.8,1.2,4.6,3l2.4,5.4c2.1.8,4,1.9,5.7,3.3l5.9-.6c2-.2,3.9.8,4.9,2.5l2.1,3.7c1,1.7.9,3.9-.3,5.5l-3.5,4.7c.2,1.2.2,2.3.2,3.3s0,2.2-.2,3.3l3.4,4.8c1.2,1.6,1.3,3.8.3,5.5l-2.1,3.7c-1,1.7-2.9,2.7-4.9,2.5l-5.8-.6c-1.8,1.4-3.7,2.5-5.7,3.3l-2.4,5.4c-.8,1.8-2.6,3-4.6,3ZM45.6,74.6c1.1,0,2.2.4,3.1,1.1,1.4,1.1,2.9,1.9,4.5,2.6,1.2.5,2.2,1.4,2.7,2.6l2.4,5.4h4.3s2.4-5.4,2.4-5.4c.5-1.2,1.5-2.1,2.7-2.6,1.6-.7,3.1-1.5,4.5-2.6,1.1-.8,2.3-1.2,3.6-1.1l5.8.6,2.2-3.7-3.5-4.7c-.8-1.1-1.1-2.4-.9-3.7.1-.9.2-1.7.2-2.6s0-1.7-.2-2.6c-.2-1.3.1-2.6.9-3.7l3.5-4.7-2.1-3.7-5.8.6c-1.3.1-2.6-.2-3.6-1.1-1.4-1.1-2.9-1.9-4.5-2.6-1.2-.5-2.2-1.4-2.7-2.6l-2.4-5.4h-4.3s-2.4,5.4-2.4,5.4c-.5,1.2-1.5,2.1-2.7,2.6-1.6.7-3.1,1.5-4.5,2.6-1.1.8-2.3,1.2-3.6,1.1l-5.8-.6-2.2,3.7,3.5,4.7c.8,1.1,1.1,2.4.9,3.7-.1.9-.2,1.7-.2,2.6s0,1.7.2,2.6c.2,1.3-.1,2.6-.9,3.7l-3.5,4.7,2.1,3.7,5.8-.6c.2,0,.4,0,.5,0Z' /> </svg>";
		public const string PictoFlecheButton =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M52.8,77.3c-.8,0-1.5-.3-2.1-.9-1.2-1.2-1.2-3,0-4.2l12.5-12.5c0,0,.1-.2.1-.3s0-.2-.1-.3l-12.5-12.5c-1.2-1.2-1.2-3,0-4.2,1.2-1.2,3-1.2,4.2,0l12.5,12.5c1.2,1.2,1.8,2.8,1.8,4.5s-.7,3.3-1.8,4.5l-12.5,12.5c-.6.6-1.3.9-2.1.9Z' /> <path d='M59.5,102c-23.4,0-42.5-19.1-42.5-42.5S36.1,17,59.5,17s42.5,19.1,42.5,42.5-19.1,42.5-42.5,42.5ZM59.5,22.9c-20.2,0-36.6,16.4-36.6,36.6s16.4,36.6,36.6,36.6,36.6-16.4,36.6-36.6-16.4-36.6-36.6-36.6Z' /> </svg>";
		public const string PictoGroupeUsers =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M74.2,95.7h-27.5c-1.7,0-3.2-.7-4.3-2-1.1-1.3-1.5-3-1.1-4.7,2-8.9,10-15.3,19.1-15.3s17.1,6.4,19.1,15.3c.4,1.7,0,3.4-1.1,4.7-1,1.3-2.6,2-4.3,2ZM60.5,78.6c-6.8,0-12.8,4.8-14.2,11.4,0,.2,0,.4.1.5,0,.1.2.2.4.2h27.5c.2,0,.3,0,.4-.2,0,0,.2-.2.1-.5-1.5-6.6-7.4-11.4-14.2-11.4Z' /> <path d='M60.5,68.3c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM60.5,52.1c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z' /> <path d='M95.3,73.9h-15.9c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h15.9c.2,0,.3,0,.4-.2,0,0,.2-.2.1-.5-1.5-6.6-7.4-11.4-14.2-11.4s-2.5-1.1-2.5-2.5,1.1-2.5,2.5-2.5c9.1,0,17.1,6.4,19.1,15.3.4,1.7,0,3.4-1.1,4.7-1,1.3-2.6,2-4.3,2Z' /> <path d='M81.6,46.5c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM81.6,30.3c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z' /> <path d='M41.6,73.9h-15.9c-1.7,0-3.2-.7-4.3-2-1.1-1.3-1.5-3-1.1-4.7,2-8.9,10-15.3,19.1-15.3s2.5,1.1,2.5,2.5-1.1,2.5-2.5,2.5c-6.8,0-12.8,4.8-14.2,11.4,0,.2,0,.4.1.5,0,.1.2.2.4.2h15.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M39.4,46.5c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM39.4,30.3c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z' /> </svg>";
		public const string PictoImage =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M92.7,99H28.3c-3.5,0-6.3-2.8-6.3-6.3V28.3c0-3.5,2.8-6.3,6.3-6.3h64.5c3.5,0,6.3,2.8,6.3,6.3v64.5c0,3.5-2.8,6.3-6.3,6.3ZM28.3,27c-.7,0-1.3.6-1.3,1.3v64.5c0,.7.6,1.3,1.3,1.3h64.5c.7,0,1.3-.6,1.3-1.3V28.3c0-.7-.6-1.3-1.3-1.3H28.3Z' /> <path d='M87.8,99h-54.6c-6.2,0-11.2-5-11.2-11.2v-22.4c0-.7.3-1.3.7-1.8l11.5-11.5c3.7-3.7,9.8-3.7,13.6,0l15.1,15.2c.9.9,2,1.3,3.3,1.3s2.4-.5,3.3-1.3c3.8-3.7,9.9-3.7,13.6,0l15.3,15.3c.5.5.7,1.1.7,1.8v3.4c0,6.2-5,11.2-11.2,11.2ZM27,66.5v21.3c0,3.4,2.8,6.2,6.2,6.2h54.6c3.4,0,6.2-2.8,6.2-6.2v-2.4l-14.5-14.5c-1.8-1.8-4.7-1.8-6.5,0-1.8,1.8-4.2,2.8-6.8,2.8s-5-1-6.8-2.8l-15.1-15.1c-1.8-1.8-4.7-1.8-6.5,0l-10.7,10.7Z' /> <path d='M68.9,55.7c-5.9,0-10.7-4.8-10.7-10.7s4.8-10.7,10.7-10.7,10.7,4.8,10.7,10.7-4.8,10.7-10.7,10.7ZM68.9,39.3c-3.2,0-5.7,2.6-5.7,5.7s2.6,5.7,5.7,5.7,5.7-2.6,5.7-5.7-2.6-5.7-5.7-5.7Z' /> </svg>";
		public const string PictoImprimer =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M74.1,97.1h-27.2c-3.1,0-5.6-2.5-5.6-5.6v-20.8c0-2.4,2-4.4,4.4-4.4h29.7c2.4,0,4.4,2,4.4,4.4v20.8c0,3.1-2.5,5.6-5.6,5.6ZM46.3,71.3v20.2c0,.4.3.6.6.6h27.2c.4,0,.6-.3.6-.6v-20.2h-28.5Z' /> <path d='M76.9,45h-32.9c-1.5,0-2.8-1.3-2.8-2.8v-12.6c0-3.1,2.5-5.6,5.6-5.6h27.2c3.1,0,5.6,2.5,5.6,5.6v12.6c0,1.5-1.3,2.8-2.8,2.8ZM46.3,40h28.5v-10.4c0-.4-.3-.6-.6-.6h-27.2c-.4,0-.6.3-.6.6v10.4Z' /> <path d='M78.7,87.4h-1.5c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h1.5c7,0,12.7-5.7,12.7-12.7v-12.1c0-7-5.7-12.7-12.7-12.7h-36.5c-7,0-12.7,5.7-12.7,12.7v12.1c0,7,5.7,12.7,12.7,12.7h1.5c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5h-1.5c-9.8,0-17.7-7.9-17.7-17.7v-12.1c0-9.8,7.9-17.7,17.7-17.7h36.5c9.8,0,17.7,7.9,17.7,17.7v12.1c0,9.8-7.9,17.7-17.7,17.7Z' /> <path d='M74.8,58.1h-8.2c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h8.2c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoLoupe =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M54.1,82.4c-15.5,0-28.2-12.6-28.2-28.2s12.6-28.2,28.2-28.2,28.2,12.6,28.2,28.2-12.6,28.2-28.2,28.2ZM54.1,31.1c-12.8,0-23.2,10.4-23.2,23.2s10.4,23.2,23.2,23.2,23.2-10.4,23.2-23.2-10.4-23.2-23.2-23.2Z' /> <path d='M92.3,94.9c-.6,0-1.3-.2-1.8-.7l-20-20c-1-1-1-2.6,0-3.5,1-1,2.6-1,3.5,0l20,20c1,1,1,2.6,0,3.5-.5.5-1.1.7-1.8.7Z' /> </svg>";
		public const string PictoMenuBurger =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <rect x='25.1' y='20.4' width='69.5' height='10.3' /> <rect x='25.1' y='39.4' width='69.5' height='10.3' /> <rect x='25.1' y='58.4' width='69.5' height='10.3' /> <g> <path d='M36.3,81.5l4.5,12.1,4.5-12.1h3.5v15.7h-2.7v-5.2l.3-6.9-4.6,12.1h-2l-4.6-12.1.3,6.9v5.2h-2.7v-15.7h3.5Z' /> <path d='M61.3,90.2h-6.5v4.8h7.5v2.2h-10.3v-15.7h10.2v2.2h-7.5v4.3h6.5v2.2Z' /> <path d='M77.1,97.2h-2.7l-7-11.2v11.2h-2.7v-15.7h2.7l7,11.2v-11.2h2.7v15.7Z' /> <path d='M91.8,81.5v10.5c0,1.7-.5,3-1.6,4-1.1,1-2.5,1.5-4.3,1.5s-3.2-.5-4.3-1.4-1.6-2.3-1.6-4v-10.5h2.7v10.5c0,1.1.3,1.9.8,2.4.5.6,1.3.8,2.4.8,2.1,0,3.2-1.1,3.2-3.3v-10.4h2.7Z' /> </g> </svg>";
		public const string PictoOuvrirDossier =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <g> <path d='M60.5,74.4c-1.2,0-2.4-.5-3.4-1.4l-8.4-8.4c-1-1-1-2.6,0-3.5s2.6-1,3.5,0l8.3,8.3,8.3-8.3c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-8.4,8.4c-.9.9-2.1,1.4-3.4,1.4Z' /> <path d='M60.5,74.4c-1.4,0-2.5-1.1-2.5-2.5v-23.5c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v23.5c0,1.4-1.1,2.5-2.5,2.5Z' /> </g> <path d='M60.5,93.9c-3,0-5.8-1.2-7.9-3.3l-4.8-4.8c-1.2-1.2-2.7-1.8-4.4-1.8h-7.6c-6.2,0-11.2-5-11.2-11.2v-34.3c0-6.2,5-11.2,11.2-11.2h49.5c6.2,0,11.2,5,11.2,11.2v34.3c0,6.2-5,11.2-11.2,11.2h-7.6c-1.7,0-3.2.6-4.4,1.8l-4.8,4.8c-2.1,2.1-4.9,3.3-7.9,3.3ZM35.7,32.1c-3.4,0-6.2,2.8-6.2,6.2v34.3c0,3.4,2.8,6.2,6.2,6.2h7.6c3,0,5.8,1.2,7.9,3.3l4.8,4.8c1.2,1.2,2.7,1.8,4.4,1.8s3.2-.6,4.4-1.8l4.8-4.8c2.1-2.1,4.9-3.3,7.9-3.3h7.6c3.4,0,6.2-2.8,6.2-6.2v-34.3c0-3.4-2.8-6.2-6.2-6.2h-49.5Z' /> </svg>";
		public const string PictoPlus =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M60.5,96.3c-19.7,0-35.8-16-35.8-35.8S40.8,24.7,60.5,24.7s35.8,16,35.8,35.8-16,35.8-35.8,35.8ZM60.5,29.7c-17,0-30.8,13.8-30.8,30.8s13.8,30.8,30.8,30.8,30.8-13.8,30.8-30.8-13.8-30.8-30.8-30.8Z' /> <path d='M60.5,74.5c-1.4,0-2.5-1.1-2.5-2.5v-23c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v23c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M72,63h-23c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h23c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoProfil =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <circle cx='57.7' cy='46.4' r='24.5' fill='#f5f5f5'/> <g> <path d='M12,82.1l3.6,9.5,3.6-9.5h2.8v12.4h-2.2v-4.1l.2-5.5-3.7,9.6h-1.5l-3.7-9.6.2,5.5v4.1h-2.2v-12.4h2.8Z' fill='#fff'/> <path d='M34.5,88.6c0,1.2-.2,2.3-.6,3.2-.4.9-1,1.6-1.8,2.1s-1.7.7-2.7.7-1.9-.2-2.7-.7c-.8-.5-1.4-1.2-1.8-2.1-.4-.9-.6-2-.7-3.2v-.7c0-1.2.2-2.3.6-3.2.4-.9,1-1.6,1.8-2.1.8-.5,1.7-.7,2.7-.7s1.9.2,2.7.7c.8.5,1.4,1.2,1.8,2.1.4.9.6,2,.6,3.2v.7ZM32.3,88c0-1.4-.3-2.4-.8-3.2-.5-.7-1.3-1.1-2.2-1.1s-1.7.4-2.2,1.1c-.5.7-.8,1.8-.8,3.1v.7c0,1.4.3,2.4.8,3.2.5.7,1.3,1.1,2.2,1.1s1.7-.4,2.2-1.1c.5-.7.8-1.8.8-3.2v-.7Z' fill='#fff'/> <path d='M46.5,94.5h-2.2l-5.5-8.8v8.8h-2.2v-12.4h2.2l5.6,8.9v-8.9h2.1v12.4Z' fill='#fff'/> <path d='M55.6,89.9v4.6h-2.2v-12.4h4.8c1.4,0,2.5.4,3.3,1.1.8.7,1.2,1.7,1.2,2.9s-.4,2.2-1.2,2.9c-.8.7-1.9,1-3.4,1h-2.6ZM55.6,88.2h2.6c.8,0,1.4-.2,1.8-.5.4-.4.6-.9.6-1.6s-.2-1.2-.6-1.6c-.4-.4-1-.6-1.7-.6h-2.7v4.3Z' fill='#fff'/> <path d='M69.2,89.8h-2.4v4.8h-2.2v-12.4h4.4c1.4,0,2.5.3,3.3,1,.8.6,1.2,1.6,1.2,2.8s-.2,1.5-.6,2.1c-.4.6-1,1-1.7,1.3l2.8,5.2h0c0,.1-2.3.1-2.3.1l-2.5-4.8ZM66.8,88h2.2c.7,0,1.3-.2,1.7-.6s.6-.9.6-1.5-.2-1.2-.6-1.5c-.4-.4-.9-.6-1.7-.6h-2.3v4.2Z' fill='#fff'/> <path d='M85.5,88.6c0,1.2-.2,2.3-.6,3.2-.4.9-1,1.6-1.8,2.1s-1.7.7-2.7.7-1.9-.2-2.7-.7c-.8-.5-1.4-1.2-1.8-2.1-.4-.9-.6-2-.7-3.2v-.7c0-1.2.2-2.3.6-3.2.4-.9,1-1.6,1.8-2.1.8-.5,1.7-.7,2.7-.7s1.9.2,2.7.7c.8.5,1.4,1.2,1.8,2.1.4.9.6,2,.6,3.2v.7ZM83.3,88c0-1.4-.3-2.4-.8-3.2s-1.3-1.1-2.2-1.1-1.7.4-2.2,1.1c-.5.7-.8,1.8-.8,3.1v.7c0,1.4.3,2.4.8,3.2.5.7,1.3,1.1,2.2,1.1s1.7-.4,2.2-1.1c.5-.7.8-1.8.8-3.2v-.7Z' fill='#fff'/> <path d='M94.8,89.3h-5v5.3h-2.2v-12.4h7.9v1.7h-5.7v3.7h5v1.7Z' fill='#fff'/> <path d='M99.5,94.5h-2.2v-12.4h2.2v12.4Z' fill='#fff'/> <path d='M104.3,92.8h5.7v1.7h-7.8v-12.4h2.2v10.7Z' fill='#fff'/> </g> <g> <path d='M67.6,62.6h-19.8c-1,0-2-.5-2.6-1.2-.6-.8-.9-1.8-.7-2.9,1.4-6.1,6.9-10.6,13.2-10.6h0c6.3,0,11.8,4.5,13.2,10.6.2,1,0,2.1-.7,2.9-.6.8-1.6,1.2-2.6,1.2Z' fill='#886aaa'/> <path d='M57.7,44.7c-4.8,0-8.7-3.9-8.7-8.7s3.9-8.7,8.7-8.7,8.7,3.9,8.7,8.7-3.9,8.7-8.7,8.7Z' fill='#886aaa'/> </g> </svg>";
		public const string PictoProfilPlus =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path class='st0' d='M76.5,96.1h-41.9c-2.1,0-4.1-1-5.5-2.6-1.4-1.7-1.9-3.9-1.4-6,2.9-13,14.6-22.3,27.8-22.3h0c13.3,0,25,9.4,27.8,22.3.5,2.1,0,4.3-1.4,6-1.3,1.7-3.3,2.6-5.5,2.6ZM55.6,70.1c-11,0-20.6,7.7-23,18.4-.1.7,0,1.3.4,1.8.2.3.7.8,1.6.8h41.9c.9,0,1.4-.5,1.6-.8.4-.5.6-1.2.4-1.8-2.4-10.7-12-18.4-23-18.4Z'/> <path class='st0' d='M55.6,58.3c-10.1,0-18.4-8.2-18.4-18.4s8.2-18.4,18.4-18.4,18.4,8.2,18.4,18.4-8.2,18.4-18.4,18.4ZM55.6,26.5c-7.4,0-13.4,6-13.4,13.4s6,13.4,13.4,13.4,13.4-6,13.4-13.4-6-13.4-13.4-13.4Z'/> <path class='st0' d='M93.6,62.5h-17.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h17.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/> <path class='st0' d='M84.9,71.2c-1.4,0-2.5-1.1-2.5-2.5v-17.4c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v17.4c0,1.4-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string PictoRedigerFiche =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M60.9,95.9c-.7,0-1.3-.3-1.8-.7s-.8-1.2-.7-1.9l.4-8.9c0-.6.3-1.2.7-1.7l23.7-23.7h0c3.3-3.3,8.8-3.3,12.1,0,3.3,3.3,3.3,8.8,0,12.1l-23.7,23.7c-.4.4-1,.7-1.7.7l-8.9.4s0,0-.1,0ZM63.7,85.5l-.2,5.2,5.2-.2,23-23c1.4-1.4,1.4-3.6,0-5-1.4-1.4-3.6-1.4-5,0l-23,23Z' /> <path d='M45.3,95.9h-13.1c-5.5,0-9.9-4.5-9.9-9.9v-43.2c0-5.5,4.5-9.9,9.9-9.9h49.7c5.5,0,9.9,4.5,9.9,9.9v5.3c0,1.4-1.1,2.5-2.5,2.5s-2.5-1.1-2.5-2.5v-5.3c0-2.7-2.2-4.9-4.9-4.9h-49.7c-2.7,0-4.9,2.2-4.9,4.9v43.2c0,2.7,2.2,4.9,4.9,4.9h13.1c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M89.4,50.5H24.8c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h64.6c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <g> <path d='M68.9,37.8c-1.4,0-2.5-1.1-2.5-2.5v-7.7c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v7.7c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M45.3,37.8c-1.4,0-2.5-1.1-2.5-2.5v-7.7c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v7.7c0,1.4-1.1,2.5-2.5,2.5Z' /> </g> <path d='M59.8,67.7h-22c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M48.9,81.3h-11c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h11c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoRedigerFiche2 =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path class='st0' d='M64.3,98.9H30.3c-6.2,0-11.2-5-11.2-11.2v-55.3c0-6.2,5-11.2,11.2-11.2h38.7c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5H30.3c-3.4,0-6.2,2.8-6.2,6.2v55.3c0,3.4,2.8,6.2,6.2,6.2h34c1.8,0,3.5-.4,5-1.1,1.3-.6,2.4-1.4,3.4-2.4l15.7-15.7c1-1,1.8-2.1,2.4-3.4.7-1.6,1.1-3.3,1.1-5v-17.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v17.3c0,2.5-.6,4.9-1.6,7.1-.8,1.8-2,3.4-3.3,4.8l-15.7,15.7c-1.4,1.4-3.1,2.6-4.8,3.3-2.1,1-4.6,1.6-7.1,1.6Z'/><path class='st0' d='M70.4,97.5c-.5,0-.9-.1-1.4-.4-.7-.5-1.1-1.3-1.1-2.1v-22.6c0-1.4,1.1-2.5,2.5-2.5h22.6c.9,0,1.6.4,2.1,1.2.5.7.5,1.6.2,2.4-.8,1.8-2,3.4-3.4,4.8l-15.7,15.7c-1.4,1.4-3.1,2.6-4.8,3.3-.3.1-.7.2-1,.2ZM72.9,74.9v15.3l15.3-15.3h-15.3Z'/><path class='st0' d='M41.7,77.2c-.7,0-1.3-.3-1.8-.7s-.8-1.2-.7-1.9l.4-8.9c0-.6.3-1.2.7-1.7l36.2-36.2h0c3.3-3.3,8.8-3.3,12.1,0,3.3,3.3,3.3,8.8,0,12.1l-36.2,36.2c-.4.4-1,.7-1.7.7l-8.9.4s0,0-.1,0ZM44.5,66.8l-.2,5.2,5.2-.2,35.5-35.5c1.4-1.4,1.4-3.6,0-5-1.4-1.4-3.6-1.4-5,0l-35.5,35.5Z'/></svg>";
		public const string PictoReglages =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M60.5,50c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM60.5,33.8c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z' /> <path d='M74.7,71.1c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM74.7,54.9c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z' /> <path d='M90.6,41.9h-22c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M52.4,41.9h-22.1c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22.1c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M90.6,63h-7.9c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M66.6,63H30.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h36.2c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M46.3,92.2c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM46.3,76c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z' /> <path d='M38.3,84.1h-7.9c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M90.6,84.1h-36.2c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h36.2c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoRetour =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M96.5,93.8c-1.4,0-2.5-1.1-2.5-2.5,0-11-9-20-20-20h-20.3v10c0,1-.6,1.9-1.5,2.3-.9.4-2,.2-2.7-.5l-26.8-25.7c-.5-.5-.8-1.1-.8-1.8s.3-1.3.8-1.8l26.8-25.7c.7-.7,1.8-.9,2.7-.5.9.4,1.5,1.3,1.5,2.3v10h15.3c16.6,0,30,13.5,30,30v21.5c0,1.4-1.1,2.5-2.5,2.5ZM51.2,66.4h22.8c8.1,0,15.4,3.9,20,10v-6.5c0-13.8-11.2-25-25-25h-17.8c-1.4,0-2.5-1.1-2.5-2.5v-6.6l-20.7,19.9,20.7,19.9v-6.6c0-1.4,1.1-2.5,2.5-2.5Z' /> </svg>";
		public const string PictoSupprimer =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M38,85.6c-.6,0-1.3-.2-1.8-.7-1-1-1-2.6,0-3.5l44.9-44.9c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-44.9,44.9c-.5.5-1.1.7-1.8.7Z' /> <path d='M83,85.6c-.6,0-1.3-.2-1.8-.7l-44.9-44.9c-1-1-1-2.6,0-3.5,1-1,2.6-1,3.5,0l44.9,44.9c1,1,1,2.6,0,3.5-.5.5-1.1.7-1.8.7Z' /> </svg>";
		public const string PictoSupprimerCorbeille =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M88.7,42h-56.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h56.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M77.3,96.6h-33.6c-3.4,0-6.2-2.8-6.2-6.2v-50.8c0-1.4,1.1-2.5,2.5-2.5h41.1c1.4,0,2.5,1.1,2.5,2.5v50.8c0,3.4-2.8,6.2-6.2,6.2ZM42.5,42v48.3c0,.7.6,1.2,1.2,1.2h33.6c.7,0,1.2-.6,1.2-1.2v-48.3h-36.1Z' /> <path d='M74.5,42h-27.9c-1.4,0-2.5-1.1-2.5-2.5v-7.1c0-4.4,3.6-8,8-8h17c4.4,0,8,3.6,8,8v7.1c0,1.4-1.1,2.5-2.5,2.5ZM49,37h22.9v-4.6c0-1.6-1.3-3-3-3h-17c-1.6,0-3,1.3-3,3v4.6Z' /> <path d='M67.3,77.3c-1.4,0-2.5-1.1-2.5-2.5v-16.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v16.1c0,1.4-1.1,2.5-2.5,2.5Z' /> <path d='M53.7,77.3c-1.4,0-2.5-1.1-2.5-2.5v-16.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v16.1c0,1.4-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoUser =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M81.5,97.8h-41.9c-2.1,0-4.1-1-5.5-2.6-1.4-1.7-1.9-3.9-1.4-6,2.9-13,14.6-22.3,27.8-22.3h0c13.3,0,25,9.4,27.8,22.3.5,2.1,0,4.3-1.4,6-1.3,1.7-3.3,2.6-5.5,2.6ZM60.5,71.8c-11,0-20.6,7.7-23,18.4-.1.7,0,1.3.4,1.8.2.3.7.8,1.6.8h41.9c.9,0,1.4-.5,1.6-.8.4-.5.6-1.2.4-1.8-2.4-10.7-12-18.4-23-18.4Z' /> <path d='M60.5,60c-10.1,0-18.4-8.2-18.4-18.4s8.2-18.4,18.4-18.4,18.4,8.2,18.4,18.4-8.2,18.4-18.4,18.4ZM60.5,28.2c-7.4,0-13.4,6-13.4,13.4s6,13.4,13.4,13.4,13.4-6,13.4-13.4-6-13.4-13.4-13.4Z' /> </svg>";
		public const string PictoUsers =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M62,97.2H26.9c-1.9,0-3.7-.9-4.9-2.4-1.2-1.5-1.7-3.5-1.3-5.4,2.4-11,12.4-19,23.7-19s21.3,8,23.7,19c.4,1.9,0,3.9-1.3,5.4-1.2,1.5-3,2.4-4.9,2.4ZM44.5,75.4c-9,0-16.9,6.4-18.9,15.1,0,.4,0,.9.3,1.2.1.2.5.5,1,.5h35.1c.5,0,.9-.3,1-.5.3-.3.4-.8.3-1.2-1.9-8.8-9.9-15.1-18.9-15.1Z' /> <path d='M44.5,62.2c-7.1,0-12.8-5.8-12.8-12.8s5.8-12.8,12.8-12.8,12.8,5.8,12.8,12.8-5.8,12.8-12.8,12.8ZM44.5,41.5c-4.3,0-7.8,3.5-7.8,7.8s3.5,7.8,7.8,7.8,7.8-3.5,7.8-7.8-3.5-7.8-7.8-7.8Z' /> <path d='M94.1,84.5h-18.5c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h18.5c.5,0,.9-.3,1-.5.3-.3.4-.8.3-1.2-1.9-8.8-9.9-15.1-18.9-15.1s-10,2-13.7,5.7c-1,1-2.6,1-3.5,0-1-1-1-2.6,0-3.5,4.6-4.6,10.7-7.2,17.2-7.2,11.3,0,21.3,8,23.7,19,.4,1.9,0,3.9-1.3,5.4-1.2,1.5-3,2.4-4.9,2.4Z' /> <path d='M76.5,49.4c-7.1,0-12.8-5.8-12.8-12.8s5.8-12.8,12.8-12.8,12.8,5.8,12.8,12.8-5.8,12.8-12.8,12.8ZM76.5,28.8c-4.3,0-7.8,3.5-7.8,7.8s3.5,7.8,7.8,7.8,7.8-3.5,7.8-7.8-3.5-7.8-7.8-7.8Z' /> </svg>";
		public const string PictoValider =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M51.7,80.6c-.6,0-1.3-.2-1.8-.7l-17.5-17.5c-1-1-1-2.6,0-3.5,1-1,2.6-1,3.5,0l15.7,15.7,33.2-33.2c1-1,2.6-1,3.5,0,1,1,1,2.6,0,3.5l-35,35c-.5.5-1.1.7-1.8.7Z' /></svg>";
		public const string PictoVerrouiller =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M83.9,93.7h-46.8c-4.3,0-7.8-3.5-7.8-7.8v-32.4c0-4.3,3.5-7.8,7.8-7.8h46.8c4.3,0,7.8,3.5,7.8,7.8v32.4c0,4.3-3.5,7.8-7.8,7.8ZM37.1,50.5c-1.6,0-2.8,1.3-2.8,2.8v32.4c0,1.6,1.3,2.8,2.8,2.8h46.8c1.6,0,2.8-1.3,2.8-2.8v-32.4c0-1.6-1.3-2.8-2.8-2.8h-46.8Z' /> <path d='M78.7,50.5h-36.4c-1.4,0-2.5-1.1-2.5-2.5v-10.3c0-5.7,4.7-10.4,10.4-10.4h20.6c5.7,0,10.4,4.7,10.4,10.4v10.3c0,1.4-1.1,2.5-2.5,2.5ZM44.8,45.5h31.4v-7.8c0-3-2.4-5.4-5.4-5.4h-20.6c-3,0-5.4,2.4-5.4,5.4v7.8Z' /> <path d='M60.5,77.2c-1.4,0-2.5-1.1-2.5-2.5v-10.3c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v10.3c0,1.4-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoVisualiserEcran =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M89.9,79.8H31.1c-4.2,0-7.6-3.4-7.6-7.6v-37.3c0-4.2,3.4-7.6,7.6-7.6h58.8c4.2,0,7.6,3.4,7.6,7.6v37.3c0,4.2-3.4,7.6-7.6,7.6ZM31.1,32.3c-1.4,0-2.6,1.2-2.6,2.6v37.3c0,1.4,1.2,2.6,2.6,2.6h58.8c1.4,0,2.6-1.2,2.6-2.6v-37.3c0-1.4-1.2-2.6-2.6-2.6H31.1Z' /> <path d='M95,68H26c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h69c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M75.2,93.7h-29.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h29.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z' /> <path d='M60.5,93.7c-1.4,0-2.5-1.1-2.5-2.5v-12.5c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v12.5c0,1.4-1.1,2.5-2.5,2.5Z' /> </svg>";
		public const string PictoVoir =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 100%' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'> <path d='M60.5,75.6c-8.3,0-15.1-6.8-15.1-15.1s6.8-15.1,15.1-15.1,15.1,6.8,15.1,15.1-6.8,15.1-15.1,15.1ZM60.5,50.4c-5.6,0-10.1,4.5-10.1,10.1s4.5,10.1,10.1,10.1,10.1-4.5,10.1-10.1-4.5-10.1-10.1-10.1Z' /> <path d='M60.5,88.9s0,0,0,0c-15,0-28.9-9.6-39.3-27.1-.5-.8-.5-1.8,0-2.6,10.4-17.5,24.4-27.1,39.3-27.1s28.9,9.6,39.3,27.1c.5.8.5,1.8,0,2.6-10.4,17.5-24.4,27.1-39.3,27.1ZM26.2,60.5c9.4,15.1,21.5,23.4,34.3,23.4s24.9-8.3,34.3-23.4c-9.4-15.1-21.5-23.4-34.3-23.4s-24.9,8.3-34.3,23.4Z' /> </svg>";
		public const string Plus =
			"<svg preserveAspectRatio='xMinYMin meet' id='plus' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,96.3c-19.7,0-35.8-16-35.8-35.8S40.8,24.7,60.5,24.7s35.8,16,35.8,35.8-16,35.8-35.8,35.8ZM60.5,29.7c-17,0-30.8,13.8-30.8,30.8s13.8,30.8,30.8,30.8,30.8-13.8,30.8-30.8-13.8-30.8-30.8-30.8Z'/><path d='M60.5,74.5c-1.4,0-2.5-1.1-2.5-2.5v-23c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v23c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M72,63h-23c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h23c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/>'</svg>";
		public const string Print =
			"<svg preserveAspectRatio='xMinYMin meet' id='print' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M74.1,97.1h-27.2c-3.1,0-5.6-2.5-5.6-5.6v-20.8c0-2.4,2-4.4,4.4-4.4h29.7c2.4,0,4.4,2,4.4,4.4v20.8c0,3.1-2.5,5.6-5.6,5.6ZM46.3,71.3v20.2c0,.4.3.6.6.6h27.2c.4,0,.6-.3.6-.6v-20.2h-28.5Z'/><path d='M76.9,45h-32.9c-1.5,0-2.8-1.3-2.8-2.8v-12.6c0-3.1,2.5-5.6,5.6-5.6h27.2c3.1,0,5.6,2.5,5.6,5.6v12.6c0,1.5-1.3,2.8-2.8,2.8ZM46.3,40h28.5v-10.4c0-.4-.3-.6-.6-.6h-27.2c-.4,0-.6.3-.6.6v10.4Z'/><path d='M78.7,87.4h-1.5c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h1.5c7,0,12.7-5.7,12.7-12.7v-12.1c0-7-5.7-12.7-12.7-12.7h-36.5c-7,0-12.7,5.7-12.7,12.7v12.1c0,7,5.7,12.7,12.7,12.7h1.5c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5h-1.5c-9.8,0-17.7-7.9-17.7-17.7v-12.1c0-9.8,7.9-17.7,17.7-17.7h36.5c9.8,0,17.7,7.9,17.7,17.7v12.1c0,9.8-7.9,17.7-17.7,17.7Z'/><path d='M74.8,58.1h-8.2c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h8.2c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string Profil =
			"<svg preserveAspectRatio='xMinYMin meet' id='profil' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><circle cx='57.7' cy='46.4' r='24.5'/><g>  <path d='M12,82.1l3.6,9.5,3.6-9.5h2.8v12.4h-2.2v-4.1l.2-5.5-3.7,9.6h-1.5l-3.7-9.6.2,5.5v4.1h-2.2v-12.4h2.8Z'/>  <path d='M34.5,88.6c0,1.2-.2,2.3-.6,3.2-.4.9-1,1.6-1.8,2.1s-1.7.7-2.7.7-1.9-.2-2.7-.7c-.8-.5-1.4-1.2-1.8-2.1-.4-.9-.6-2-.7-3.2v-.7c0-1.2.2-2.3.6-3.2.4-.9,1-1.6,1.8-2.1.8-.5,1.7-.7,2.7-.7s1.9.2,2.7.7c.8.5,1.4,1.2,1.8,2.1.4.9.6,2,.6,3.2v.7ZM32.3,88c0-1.4-.3-2.4-.8-3.2-.5-.7-1.3-1.1-2.2-1.1s-1.7.4-2.2,1.1c-.5.7-.8,1.8-.8,3.1v.7c0,1.4.3,2.4.8,3.2.5.7,1.3,1.1,2.2,1.1s1.7-.4,2.2-1.1c.5-.7.8-1.8.8-3.2v-.7Z'/>  <path d='M46.5,94.5h-2.2l-5.5-8.8v8.8h-2.2v-12.4h2.2l5.6,8.9v-8.9h2.1v12.4Z'/>  <path d='M55.6,89.9v4.6h-2.2v-12.4h4.8c1.4,0,2.5.4,3.3,1.1.8.7,1.2,1.7,1.2,2.9s-.4,2.2-1.2,2.9c-.8.7-1.9,1-3.4,1h-2.6ZM55.6,88.2h2.6c.8,0,1.4-.2,1.8-.5.4-.4.6-.9.6-1.6s-.2-1.2-.6-1.6c-.4-.4-1-.6-1.7-.6h-2.7v4.3Z'/>  <path d='M69.2,89.8h-2.4v4.8h-2.2v-12.4h4.4c1.4,0,2.5.3,3.3,1,.8.6,1.2,1.6,1.2,2.8s-.2,1.5-.6,2.1c-.4.6-1,1-1.7,1.3l2.8,5.2h0c0,.1-2.3.1-2.3.1l-2.5-4.8ZM66.8,88h2.2c.7,0,1.3-.2,1.7-.6s.6-.9.6-1.5-.2-1.2-.6-1.5c-.4-.4-.9-.6-1.7-.6h-2.3v4.2Z'/>  <path d='M85.5,88.6c0,1.2-.2,2.3-.6,3.2-.4.9-1,1.6-1.8,2.1s-1.7.7-2.7.7-1.9-.2-2.7-.7c-.8-.5-1.4-1.2-1.8-2.1-.4-.9-.6-2-.7-3.2v-.7c0-1.2.2-2.3.6-3.2.4-.9,1-1.6,1.8-2.1.8-.5,1.7-.7,2.7-.7s1.9.2,2.7.7c.8.5,1.4,1.2,1.8,2.1.4.9.6,2,.6,3.2v.7ZM83.3,88c0-1.4-.3-2.4-.8-3.2s-1.3-1.1-2.2-1.1-1.7.4-2.2,1.1c-.5.7-.8,1.8-.8,3.1v.7c0,1.4.3,2.4.8,3.2.5.7,1.3,1.1,2.2,1.1s1.7-.4,2.2-1.1c.5-.7.8-1.8.8-3.2v-.7Z'/>  <path d='M94.8,89.3h-5v5.3h-2.2v-12.4h7.9v1.7h-5.7v3.7h5v1.7Z'/>  <path d='M99.5,94.5h-2.2v-12.4h2.2v12.4Z'/>  <path d='M104.3,92.8h5.7v1.7h-7.8v-12.4h2.2v10.7Z'/></g><g>  <path d='M67.6,62.6h-19.8c-1,0-2-.5-2.6-1.2-.6-.8-.9-1.8-.7-2.9,1.4-6.1,6.9-10.6,13.2-10.6h0c6.3,0,11.8,4.5,13.2,10.6.2,1,0,2.1-.7,2.9-.6.8-1.6,1.2-2.6,1.2Z'/>  <path d='M57.7,44.7c-4.8,0-8.7-3.9-8.7-8.7s3.9-8.7,8.7-8.7,8.7,3.9,8.7,8.7-3.9,8.7-8.7,8.7Z'/></g></svg>";
		public const string Read =
			"<svg preserveAspectRatio='xMinYMin meet' id='read' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><g><path d='M64.4,85.4h-20.3c-7.1,0-12.9-5.8-12.9-12.9v-35.9c0-7.1,5.8-12.9,12.9-12.9h14.5c1.5,0,3,.3,4.4,1,1,.4,2,1.1,2.9,2l8.4,8.4c.8.8,1.5,1.8,2,2.9.6,1.3,1,2.8,1,4.3v30.2c0,7.1-5.8,12.9-12.9,12.9ZM44.1,28.8c-4.3,0-7.9,3.5-7.9,7.9v35.9c0,4.3,3.5,7.9,7.9,7.9h20.3c4.3,0,7.9-3.5,7.9-7.9v-30.2c0-.8-.2-1.5-.5-2.2-.3-.6-.6-1.1-1-1.5l-8.4-8.4c-.4-.4-.9-.8-1.4-1-.7-.4-1.5-.5-2.2-.5h-14.5Z'/><path d='M74,41.6h-12.1c-1.4,0-2.5-1.1-2.5-2.5v-12.1c0-.8.4-1.6,1.1-2.1.7-.5,1.6-.5,2.4-.2,1.1.5,2.1,1.2,2.9,2l8.4,8.4c.8.8,1.5,1.8,2,2.9.4.8.3,1.7-.2,2.4-.5.7-1.3,1.2-2.1,1.2ZM64.4,36.6h4.3l-4.3-4.3v4.3Z'/></g><path d='M68,97.2h-19.6c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h19.6c9.2,0,16.7-7.5,16.7-16.7v-27.1c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v27.1c0,12-9.8,21.7-21.7,21.7Z'/></svg>";
		public const string Resources =
			"<svg preserveAspectRatio='xMinYMin meet' width='19' height='27' viewBox='0 0 19 27' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M4.69681 20.625H14.0718V22.5H4.69681V20.625ZM6.57181 24.375H12.1968V26.25H6.57181V24.375ZM9.38431 0C6.89791 0 4.51334 0.98772 2.75519 2.74587C0.997033 4.50403 0.00931297 6.8886 0.00931297 9.375C-0.0540861 10.7375 0.206913 12.0956 0.77084 13.3375C1.33477 14.5795 2.1855 15.6698 3.25306 16.5188C4.19056 17.3906 4.69681 17.8875 4.69681 18.75H6.57181C6.57181 17.025 5.53119 16.0594 4.51869 15.1312C3.64246 14.4602 2.94413 13.5846 2.48486 12.5811C2.02558 11.5776 1.81938 10.4767 1.88431 9.375C1.88431 7.38588 2.67449 5.47822 4.08101 4.0717C5.48753 2.66518 7.39519 1.875 9.38431 1.875C11.3734 1.875 13.2811 2.66518 14.6876 4.0717C16.0941 5.47822 16.8843 7.38588 16.8843 9.375C16.9482 10.4775 16.7405 11.5789 16.2796 12.5825C15.8186 13.5861 15.1185 14.4613 14.2406 15.1312C13.2374 16.0687 12.1968 17.0063 12.1968 18.75H14.0718C14.0718 17.8875 14.5687 17.3906 15.5156 16.5094C16.5824 15.6619 17.4327 14.5732 17.9967 13.3328C18.5606 12.0925 18.822 10.7361 18.7593 9.375C18.7593 8.14386 18.5168 6.92477 18.0457 5.78734C17.5745 4.64992 16.884 3.61642 16.0134 2.74587C15.1429 1.87532 14.1094 1.18477 12.972 0.713629C11.8345 0.242492 10.6155 0 9.38431 0Z' /></svg>";
		public const string Save =
			"<svg preserveAspectRatio='xMinYMin meet' id='save' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M88.1,96.4h-55.3c-4.3,0-7.7-3.5-7.7-7.7v-56.2c0-4.3,3.5-7.7,7.7-7.7h42.4c1.7,0,3.3.4,4.8,1.1,1.1.5,2.2,1.2,3.1,2.2l9.4,9.4c.9.9,1.7,2,2.2,3.2.7,1.4,1,3.1,1,4.7v43.4c0,4.3-3.5,7.7-7.7,7.7ZM32.9,29.6c-1.5,0-2.7,1.2-2.7,2.7v56.2c0,1.5,1.2,2.7,2.7,2.7h55.3c1.5,0,2.7-1.2,2.7-2.7v-43.4c0-.9-.2-1.8-.6-2.6-.3-.7-.7-1.3-1.2-1.8l-9.4-9.4c-.5-.5-1.1-.9-1.7-1.2-.9-.4-1.7-.6-2.6-.6h-42.4Z'/><path d='M75.8,45.7h-30.6c-2.8,0-5.2-2.3-5.2-5.2v-13.3c0-1.4,1.1-2.5,2.5-2.5h35.9c1.4,0,2.5,1.1,2.5,2.5v13.3c0,2.8-2.3,5.2-5.2,5.2ZM45,29.8v10.8c0,0,0,.2.2.2h30.6c0,0,.2,0,.2-.2v-10.8h-30.9Z'/><path d='M60.5,82.1c-7,0-12.6-5.7-12.6-12.6s5.7-12.6,12.6-12.6,12.6,5.7,12.6,12.6-5.7,12.6-12.6,12.6ZM60.5,61.9c-4.2,0-7.6,3.4-7.6,7.6s3.4,7.6,7.6,7.6,7.6-3.4,7.6-7.6-3.4-7.6-7.6-7.6Z'/></svg>";
		public const string Search =
			"<svg preserveAspectRatio='xMinYMin meet' id='search' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M54.1,82.4c-15.5,0-28.2-12.6-28.2-28.2s12.6-28.2,28.2-28.2,28.2,12.6,28.2,28.2-12.6,28.2-28.2,28.2ZM54.1,31.1c-12.8,0-23.2,10.4-23.2,23.2s10.4,23.2,23.2,23.2,23.2-10.4,23.2-23.2-10.4-23.2-23.2-23.2Z'/><path d='M92.3,94.9c-.6,0-1.3-.2-1.8-.7l-20-20c-1-1-1-2.6,0-3.5,1-1,2.6-1,3.5,0l20,20c1,1,1,2.6,0,3.5-.5.5-1.1.7-1.8.7Z'/> </svg>";
		public const string SeeFill =
			"<svg preserveAspectRatio='xMinYMin meet' id='voir' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,75.6c-8.3,0-15.1-6.8-15.1-15.1s6.8-15.1,15.1-15.1,15.1,6.8,15.1,15.1-6.8,15.1-15.1,15.1ZM60.5,50.4c-5.6,0-10.1,4.5-10.1,10.1s4.5,10.1,10.1,10.1,10.1-4.5,10.1-10.1-4.5-10.1-10.1-10.1Z' /><path d='M60.5,88.9s0,0,0,0c-15,0-28.9-9.6-39.3-27.1-.5-.8-.5-1.8,0-2.6,10.4-17.5,24.4-27.1,39.3-27.1s28.9,9.6,39.3,27.1c.5.8.5,1.8,0,2.6-10.4,17.5-24.4,27.1-39.3,27.1ZM26.2,60.5c9.4,15.1,21.5,23.4,34.3,23.4s24.9-8.3,34.3-23.4c-9.4-15.1-21.5-23.4-34.3-23.4s-24.9,8.3-34.3,23.4Z'/></svg>";
		public const string SeeScreenFill =
			"<svg preserveAspectRatio='xMinYMin meet' id=ScreenSee baseProfile=tiny version=1.2 viewBox=0 0 120 120><path d=M89.9,79.8H31.1c-4.2,0-7.6-3.4-7.6-7.6v-37.3c0-4.2,3.4-7.6,7.6-7.6h58.8c4.2,0,7.6,3.4,7.6,7.6v37.3c0,4.2-3.4,7.6-7.6,7.6ZM31.1,32.3c-1.4,0-2.6,1.2-2.6,2.6v37.3c0,1.4,1.2,2.6,2.6,2.6h58.8c1.4,0,2.6-1.2,2.6-2.6v-37.3c0-1.4-1.2-2.6-2.6-2.6H31.1Z/><path d=M95,68H26c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h69c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z/><path d=M75.2,93.7h-29.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h29.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z/><path d=M60.5,93.7c-1.4,0-2.5-1.1-2.5-2.5v-12.5c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v12.5c0,1.4-1.1,2.5-2.5,2.5Z/></svg>";
		public const string SignIn =
			"<svg preserveAspectRatio='xMinYMin meet' id='sign-in' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><g id='bKPW0V'><g><path d='M61.9,30.5h-29.2c-1.2,0-4.1,4.8-4.4,6.5-1.4,8.5-1,35.2-.1,44.3.2,2.2,2.3,8.3,4.5,8.3h29.2v-6.9h-26.8v-45.3h26.8v-6.9ZM80.8,63.9l-6.9,7.5,4.4,5.3,16.3-17.1-16.3-16.4-4.4,5.3,6.9,7.5h-31.2c-2.3,1.9-2.2,6.1,0,7.9h31.2Z'/><path d='M61.9,30.5v6.9h-26.8v45.3h26.8v6.9h-29.2c-2.2,0-4.3-6.1-4.5-8.3-.8-9.1-1.3-35.8.1-44.3.3-1.7,3.2-6.5,4.4-6.5h29.2Z'/><path d='M80.8,63.9h-31.2c-2.2-1.8-2.3-6,0-7.9h31.2l-6.9-7.5,4.4-5.3,16.3,16.4-16.3,17.1-4.4-5.3,6.9-7.5Z'/></g></g></svg>";
		public const string Tips =
			"<svg preserveAspectRatio='xMinYMin meet' id='tips' style='width: 50px' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,97.6c-5.9,0-10.6-4.8-10.6-10.6v-3.4c0-5.8-2.4-11.3-6.8-15.2-5.4-4.9-8.4-11.8-8.4-19,0-14.2,11.6-25.8,25.8-25.8s25.8,11.6,25.8,25.8-2.7,13.5-7.7,18.4c-.1.2-.2.3-.4.4-4.5,3.9-7,9.6-7,15.5v3.4c0,5.9-4.8,10.6-10.6,10.6ZM60.5,28.4c-11.5,0-20.8,9.3-20.8,20.8s2.5,11.4,6.8,15.3c5.4,4.9,8.5,11.8,8.5,18.9v3.4c0,3.1,2.5,5.6,5.6,5.6s5.6-2.5,5.6-5.6v-3.4c0-7.1,3-14,8.3-18.8,0-.1.2-.2.3-.3,4.2-4,6.6-9.4,6.6-15.1,0-11.5-9.3-20.8-20.8-20.8Z'/><path d='M68.8,84.1h-16.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h16.4c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M60.6,84.1c-1.4,0-2.5-1.1-2.5-2.5v-27.9c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v27.9c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M60.6,56.2c-4.5,0-8.3-3.7-8.3-8.3s1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5,1.5,3.3,3.3,3.3,3.3-1.5,3.3-3.3,1.1-2.5,2.5-2.5,2.5,1.1,2.5,2.5c0,4.6-3.7,8.3-8.3,8.3Z'/></svg>";
		public const string Tools =
			"<svg preserveAspectRatio='xMinYMin meet' id='tools' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,50c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM60.5,33.8c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z'/><path d='M74.7,71.1c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM74.7,54.9c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z'/><path d='M90.6,41.9h-22c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M52.4,41.9h-22.1c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h22.1c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M90.6,63h-7.9c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M66.6,63H30.4c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h36.2c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M46.3,92.2c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM46.3,76c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z'/><path d='M38.3,84.1h-7.9c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h7.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M90.6,84.1h-36.2c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h36.2c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/></svg>";
		public const string Tooltip =
			"<svg preserveAspectRatio='xMinYMin meet' id='info-tooltip' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M60.5,96.3c-19.7,0-35.8-16-35.8-35.8S40.8,24.7,60.5,24.7s35.8,16,35.8,35.8-16,35.8-35.8,35.8ZM60.5,29.7c-17,0-30.8,13.8-30.8,30.8s13.8,30.8,30.8,30.8,30.8-13.8,30.8-30.8-13.8-30.8-30.8-30.8Z'/><g><path d='M60.5,67.2c-1.4,0-2.5-1.1-2.5-2.5v-18.8c0-1.4,1.1-2.5,2.5-2.5s2.5,1.1,2.5,2.5v18.8c0,1.4-1.1,2.5-2.5,2.5Z'/><path d='M60.5,77.6c-.7,0-1.3-.3-1.8-.7-.5-.5-.7-1.1-.7-1.8s.3-1.3.7-1.8c.1-.1.2-.2.4-.3.1,0,.3-.2.4-.2.2,0,.3-.1.5-.1.8-.2,1.7.1,2.2.7.5.5.7,1.1.7,1.8s-.3,1.3-.7,1.8c-.5.5-1.1.7-1.8.7Z'/></g></svg>";
		public const string Upload =
			"<svg preserveAspectRatio='xMinYMin meet' style='width: 50px' viewBox='0 0 20 20' fill='none' xmlns='http://www.w3.org/2000/svg'><path d='M10 13.332L13.3334 9.16536H10.8334V3.33203H9.16669V9.16536H6.66669L10 13.332Z' fill='black'/><path d='M16.6667 15.0013H3.33335V9.16797H1.66669V15.0013C1.66669 15.9205 2.41419 16.668 3.33335 16.668H16.6667C17.5859 16.668 18.3334 15.9205 18.3334 15.0013V9.16797H16.6667V15.0013Z' fill='black'/></svg>";
		public const string UserFill =
			"<svg preserveAspectRatio='xMinYMin meet' id='user' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M81.5,97.8h-41.9c-2.1,0-4.1-1-5.5-2.6-1.4-1.7-1.9-3.9-1.4-6,2.9-13,14.6-22.3,27.8-22.3h0c13.3,0,25,9.4,27.8,22.3.5,2.1,0,4.3-1.4,6-1.3,1.7-3.3,2.6-5.5,2.6ZM60.5,71.8c-11,0-20.6,7.7-23,18.4-.1.7,0,1.3.4,1.8.2.3.7.8,1.6.8h41.9c.9,0,1.4-.5,1.6-.8.4-.5.6-1.2.4-1.8-2.4-10.7-12-18.4-23-18.4Z'/><path d='M60.5,60c-10.1,0-18.4-8.2-18.4-18.4s8.2-18.4,18.4-18.4,18.4,8.2,18.4,18.4-8.2,18.4-18.4,18.4ZM60.5,28.2c-7.4,0-13.4,6-13.4,13.4s6,13.4,13.4,13.4,13.4-6,13.4-13.4-6-13.4-13.4-13.4Z'/></svg>";
		public const string UserFillProfil =
			"<svg preserveAspectRatio='xMinYMin meet' id='user' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M81.5,97.8h-41.9c-2.1,0-4.1-1-5.5-2.6-1.4-1.7-1.9-3.9-1.4-6,2.9-13,14.6-22.3,27.8-22.3h0c13.3,0,25,9.4,27.8,22.3.5,2.1,0,4.3-1.4,6-1.3,1.7-3.3,2.6-5.5,2.6ZM60.5,71.8c-11,0-20.6,7.7-23,18.4-.1.7,0,1.3.4,1.8.2.3.7.8,1.6.8h41.9c.9,0,1.4-.5,1.6-.8.4-.5.6-1.2.4-1.8-2.4-10.7-12-18.4-23-18.4Z'/><path d='M60.5,60c-10.1,0-18.4-8.2-18.4-18.4s8.2-18.4,18.4-18.4,18.4,8.2,18.4,18.4-8.2,18.4-18.4,18.4ZM60.5,28.2c-7.4,0-13.4,6-13.4,13.4s6,13.4,13.4,13.4,13.4-6,13.4-13.4-6-13.4-13.4-13.4Z'/></svg>";
		public const string UserGroup =
			"<svg preserveAspectRatio='xMinYMin meet' id='user-group' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M74.2,95.7h-27.5c-1.7,0-3.2-.7-4.3-2-1.1-1.3-1.5-3-1.1-4.7,2-8.9,10-15.3,19.1-15.3s17.1,6.4,19.1,15.3c.4,1.7,0,3.4-1.1,4.7-1,1.3-2.6,2-4.3,2ZM60.5,78.6c-6.8,0-12.8,4.8-14.2,11.4,0,.2,0,.4.1.5,0,.1.2.2.4.2h27.5c.2,0,.3,0,.4-.2,0,0,.2-.2.1-.5-1.5-6.6-7.4-11.4-14.2-11.4Z'/><path d='M60.5,68.3c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM60.5,52.1c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z'/><path d='M95.3,73.9h-15.9c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h15.9c.2,0,.3,0,.4-.2,0,0,.2-.2.1-.5-1.5-6.6-7.4-11.4-14.2-11.4s-2.5-1.1-2.5-2.5,1.1-2.5,2.5-2.5c9.1,0,17.1,6.4,19.1,15.3.4,1.7,0,3.4-1.1,4.7-1,1.3-2.6,2-4.3,2Z'/><path d='M81.6,46.5c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM81.6,30.3c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z'/><path d='M41.6,73.9h-15.9c-1.7,0-3.2-.7-4.3-2-1.1-1.3-1.5-3-1.1-4.7,2-8.9,10-15.3,19.1-15.3s2.5,1.1,2.5,2.5-1.1,2.5-2.5,2.5c-6.8,0-12.8,4.8-14.2,11.4,0,.2,0,.4.1.5,0,.1.2.2.4.2h15.9c1.4,0,2.5,1.1,2.5,2.5s-1.1,2.5-2.5,2.5Z'/><path d='M39.4,46.5c-5.8,0-10.6-4.7-10.6-10.6s4.7-10.6,10.6-10.6,10.6,4.7,10.6,10.6-4.7,10.6-10.6,10.6ZM39.4,30.3c-3.1,0-5.6,2.5-5.6,5.6s2.5,5.6,5.6,5.6,5.6-2.5,5.6-5.6-2.5-5.6-5.6-5.6Z'/></svg>";
		public const string UsersFill =
			"<svg preserveAspectRatio='xMinYMin meet' id='users' xmlns='http://www.w3.org/2000/svg' baseProfile='tiny' version='1.2' viewBox='0 0 120 120'><path d='M62,97.2H26.9c-1.9,0-3.7-.9-4.9-2.4-1.2-1.5-1.7-3.5-1.3-5.4,2.4-11,12.4-19,23.7-19s21.3,8,23.7,19c.4,1.9,0,3.9-1.3,5.4-1.2,1.5-3,2.4-4.9,2.4ZM44.5,75.4c-9,0-16.9,6.4-18.9,15.1,0,.4,0,.9.3,1.2.1.2.5.5,1,.5h35.1c.5,0,.9-.3,1-.5.3-.3.4-.8.3-1.2-1.9-8.8-9.9-15.1-18.9-15.1Z'/><path d='M44.5,62.2c-7.1,0-12.8-5.8-12.8-12.8s5.8-12.8,12.8-12.8,12.8,5.8,12.8,12.8-5.8,12.8-12.8,12.8ZM44.5,41.5c-4.3,0-7.8,3.5-7.8,7.8s3.5,7.8,7.8,7.8,7.8-3.5,7.8-7.8-3.5-7.8-7.8-7.8Z'/><path d='M94.1,84.5h-18.5c-1.4,0-2.5-1.1-2.5-2.5s1.1-2.5,2.5-2.5h18.5c.5,0,.9-.3,1-.5.3-.3.4-.8.3-1.2-1.9-8.8-9.9-15.1-18.9-15.1s-10,2-13.7,5.7c-1,1-2.6,1-3.5,0-1-1-1-2.6,0-3.5,4.6-4.6,10.7-7.2,17.2-7.2,11.3,0,21.3,8,23.7,19,.4,1.9,0,3.9-1.3,5.4-1.2,1.5-3,2.4-4.9,2.4Z'/><path d='M76.5,49.4c-7.1,0-12.8-5.8-12.8-12.8s5.8-12.8,12.8-12.8,12.8,5.8,12.8,12.8-5.8,12.8-12.8,12.8ZM76.5,28.8c-4.3,0-7.8,3.5-7.8,7.8s3.5,7.8,7.8,7.8,7.8-3.5,7.8-7.8-3.5-7.8-7.8-7.8Z'/></svg>";
	}
}

public static class Endpoints
{
	public const string AccompanyingFileMenu = "/menu";
	public const string AnahCategory = "/anah";
	public const string Base = "/";
	public const string CreateAccompanyingFileIdentificationStageSynthesis = "/stage/identification/synthesis";
	public const string CreateAccompanyingFileOrganizeAndFinanceStageSynthesis = "/stage/organizeAndFinance/synthesis";
	public const string CreateAccompanyingFileRealiseAndFollowStageSynthesis = "/stage/realiseAndFollow/synthesis";
	public const string DeletedAccountManagement = "/DeletedAccountManagement";
	public const string DisplayDetailsId = "/admin/DisplayDetails/{Id}";
	public const string ErrorPage = "/ErrorPage";
	public const string ExcelExport = "/excelExport";
	public const string CguVersion = "/cguVersions";
	public const string IdentificationSynthesis = "/synthesis/identification";
	public const string ImportExcelFiles = "/import/excel";
	public const string NewOccupant = "/newoccupantdisplay";
	public const string NewSubscriptionDisplay = "/admin/newsubscriptiondisplay";
	public const string OrganizeAndFinanceStage = "/organizeAndFinance";
	public const string OrganizeAndFinanceSynthesis = "/synthesis/organizeAndFinance";
	public const string ProfileManagementPage = "/ProfileManagement/ProfileManagementForm";
	public const string QuickAddAccompanyingFile = "/quickAdd";
	public const string RealiseAndFollowStage = "/realiseAndFollow";
	public const string RealizeAndFollowSynthesis = "/synthesis/realizeAndFollow";
	public const string ValidateIdentificationStageSynthesis = "/validate/identification/synthesis";
	public const string ValidateRealiseAndFollowStageSynthesis = "/validate/realiseAndFollow/synthesis";
	public const string ValidateOrganizeAndFinanceStageSynthesis = "/validate/organizeAndFinance/synthesis";
    public const string RemainingAmountFound = "/remainingamount";
	public const string ReportingStructureManagement = "/ReportingStructureManagement";
	public const string Resources = "/Resources";
	public const string UpdateProfileManagementModal = "/ProfileManagement/UpdateProfileManagementModal";
	public const string UserCreatedAccompanyingFiles = "/myaccompanyingfiles";
	public const string UserCreation = "/usercreation";
	public const string Validation = "/validation";
	public const string ImportCsvFiles = "/import/csv";
	public const string GenerateDocumentsTemplates = "/generateDocuments";
	public const string Coproperty = "/coproperty";
	public const string QuickAddCoproperty = "/quickAddCoproperty";
    public const string CopropertyIdentification = "/coproperty/identification";
    public const string CopropertyOrganizeAndFinance = "/coproperty/organizeAndFinance";
	public const string CopropertyRealizeAndFollow = "/coproperty/realizeAndFollow";
	public const string CopropertyIdentificationSynthesis = "/coproperty/synthesis/identification";
	public const string CopropertyOrganizeAndFinanceSynthesis = "/coproperty/synthesis/organizeAndFinance";
	public const string CopropertyRealizeAndFollowSynthesis = "/coproperty/synthesis/realizeAndFollow";
    public const string CopropertyProfileMenu = "/coproperty/menu";
	public const string AnahGrantCheck = "/anah-grant-check";
	public const string MyTasks = "/mes-taches";
	public const string Ai = "/ai";
}

public static class IndexLabels
{
	public const string AccompanyingFilesCompletionSpeed = "Vitesse de complétion des dossiers";
	public const string AnahCategoriesRepartition = "Répartition selon la catégorie ANAH du ménage";
	public const string AverageAgeMainOccupant = "Moyenne de l'âge de l'occupant principal";
	public const string AverageDeliveryTime = "Moyenne du délai de prise en charge";
	public const string AverageDeliveryTimeTitle = "Moyenne du délai de prise en charge (en mois)";
	public const string AverageEnergyEffortBeforeWork = "Moyenne du taux d'effort énergétique avant travaux";
	public const string AverageFundingByType = "Moyenne des financements par type de financement";
	public const string AverageWorkCostByEnergyJumpClass = "Montant moyen des travaux par classe de saut énergétique";
	public const string AverageWorksCost = "Coût moyen des travaux";
	public const string BeforeWorksEnergy = "Energie avant travaux";
	public const string BreakdownFilesStatus = "Répartition des dossiers par état d'avancement";
	public const string Completed = "Clôturés";
	public const string ConversionRate = "Taux de transformation";
	public const string EndDate = "Date de fin";
	public const string EstimatedEnergyJumpByNumberOfHousehold = "Saut de classe estimé par nombre de foyers";
	public const string FilterBy = "Filtrer par";
	public const string FilterByReportingStructures = "Filtrer par Structure de rattachement";
	public const string FinishedAccompanyingFiles = "Terminé";
	public const string HouseHoldByMarkerNature = "Nombre de foyers identifiés par nature de repérant";
	public const string HouseholdCaracteristics = "Caractéristiques des foyers";
	public const string HouseholdEarnings = "Revenus des foyers";
	public const string HouseholdInTotalEnergyDeprivation =
		"Pourcentage moyen des ménages en privation d'énergie totale";
	public const string HousingTypeByHouseholdsNumber = "Nombre de ménages par type de logement";
	public const string Identification = "1. Identifier";
	public const string IndexAssociationMemberTitle = "Bienvenue dans votre Tableau de bord membre de l'équipe STOP";
	public const string IndexDiffuseCoordinatorTitle = "Bienvenue dans votre Tableau de bord Coordinateur·rice Diffus";
	public const string IndexSolidarBuilderTitle = "Bienvenue dans votre Tableau de bord Ensemblier·ère Solidaire";
	public const string IndexTargetedCoordinatorTitle =
		"Bienvenue dans votre Tableau de bord Coordinateur·rice Ciblé·e";
	public const string IndexTerritorialBuilderTitle =
		"Bienvenue dans votre Tableau de bord Ensemblier·ère Territorial·e";
	public const string MilestoneDistribution = "Répartition des dossiers par jalon d'accompagnement";
	public const string MilestoneIdentification = "Jalon - Identifier";
	public const string MilestoneOrganizeAndFinance = "Jalon - Organiser et financer";
	public const string MilestoneRealizeAndFollow = "Jalon - Réaliser et suivre";
	public const string MyAccompanyingFiles = "Mes dossiers";
	public const string MyAllAccompanyingFiles = "Tous les dossiers";
	public const string MyReportingStructure = "Ma structure de rattachement";
	public const string NoRelevantDataFound = "Aucune donnée n'a été renseignée pour afficher cet indicateur";
	public const string NumberHouseholdIdentifiedByEnergeticsLabels =
		"Nombre de foyers identifiés par étiquette énergie avant travaux";
	public const string NumberHouseholdIdentifiedBySocioProfessionalCategory =
		"Nombre de foyers identifiés par catégorie socio-professionnelle de l'occupant principal";
	public const string NumberHouseholdIdentifiedByTypology = "Nombre de foyers identifiés par typologie de famille";
	public const string NumberHouseholdsOfANAH = "Nombre de foyers selon les types d'aides de l'ANAH mobilisée";
	public const string NumberOfHouseholdDetected = "Nombre de foyer identifiés par nature de repérant";
	public const string NumberOfSupport = "Nombre d'accompagnements";
	public const string NumberOfUser = "Nombre de comptes";
	public const string OnHold = "En cours";
	public const string OrganizeAndFinance = "2. Organiser et financer";
	public const string PassageRateFromFirstStageToSecondStage =
		"Taux de transformation des dossiers du jalon 1 au jalon 2";
	public const string PassageRateFromFirstStageToThirdStage =
		"Taux de transformation des dossiers du jalon 1 au jalon 3";
	public const string PieChartTitle = "Répartition des ménages";
	public const string Progress = "Avancement";
	public const string RealizeAndFollow = "3. Réaliser et suivre";
	public const string RemainingAmount = "Reste à charge moyen estimé";
	public const string StartDate = "Date de début";
	public const string StartingDpe = "Etiquette énergétique de départ";
	public const string SupportDuration = "Moyenne de la durée de l'accompagnement (en mois)";
	public const string TaxRevenueAverageByYear =
		"Moyenne des revenus fiscaux de référence annuels des foyers accompagnés";
	public const string WaitingForApproval = "En attente de validation";
	public const string WorkFinancement = "Financement des travaux";
	public const string WorksFinancing = "Financement des travaux";
	public const string IndexStructuralReferentTitle = "Bienvenue dans votre Tableau de bord Référent·e structurel·le";
	public const string MaximalNumberOfAccompanyingFileCreated = "Nombre maximal de dossiers pouvant être créés";
	public const string MaximalNumberOfAccompanyingFileToValidateFirstStage = "Nombre maximal de dossiers pouvant être validés au jalon 1";
	public const string Update = "Mettre à jour";
	public const string UpdateSuccess = "Mise à jour réussie";
	public const string TzeeAccompanyingFileModificationDeadline = "Date limite de modification des dossiers TZEE";
	public const string TzeeAccompanyingFileAlertBannerStartDate = "Date de début d'affichage du bandeau d'alerte";
	public const string TzeeAccompanyingFileAlertBannerEndDate = "Date de fin d'affichage du bandeau d'alerte";
	public const string TzeeAccompanyingFileAlertBannerMessage = "Message du bandeau d'alerte";

	public static class Error
	{
		public const string ErrorWhileLoadingIndexData = "Erreur lors du chargement des données";
		public const string ErrorWhileUpdatingConstants = "Une erreur est survenue lors de la mise à jour des valeurs";
	}
}

public static class TaskPriorityLabel
{
	public const string High = "Priorité élevée";
	public const string Low = "Priorité faible";
	public const string Middle = "Priorité moyenne";
}

public static class AccompanyingFileMenu
{
	public const string AccompanyingFileSupportTeam = "Equipe d'accompagnement";
	public const string AddTask = "Ajouter une tâche";
	public const string DoneTasks = "Tâches terminées";
	public const string TaskAssignee = "Porteur de l'action";
	public const string TaskDeadline = "Date d'échéance";
	public const string TaskList = "Liste des tâches";
	public const string TaskCount = "Nombre de tâches";
	public const string TaskName = "Description de la tâche";
	public const string TaskPriority = "Priorité";
	public const string TaskProgress = "État d'avancement";
	public const string ToBePerformedTasks = "Tâches à réaliser";
	public const string UndoneTasks = "Tâches en cours";
	public const string TargetInformation = "Informations de ciblage";
	public const string UpdateMarToolTipAdvice = "Pour modifier ce paramètre, ajustez les données du Jalon 1 (coefficient d’insalubrité et/ou index de dégradation).";
	public const string FacturationInformation = "Informations de facturation";
	public const string FirstJalonFacturation = "Facturation du jalon 1";
	public const string SecondJalonFacturation = "Facturation du jalon 2";
	public const string ThirdJalonFacturation = "Facturation du jalon 3";
	public const string FirstJalonBilled = "Jalon 1 facturé";
	public const string SecondJalonBilled = "Jalon 2 facturé";
	public const string ThirdJalonBilled = "Jalon 3 facturé";
	public const string TotalAmountBilled = "Montant total facturé";
	public const string FundraisingLaunchDate = "Date de lancement de l’appel de fonds";
	public const string BillingCallNumber = "Numéro d'Appel à facturation";
	public const string InvoiceNumber = "Numéro de Facture";
	public const string BillingDate = "Date de facturation";
	public const string UpcomingTasks = "À venir";
	public const string TaskStartDate = "Date de rappel";

	public static class Errors
	{
		public const string RequiredTaskAssignee = "Le porteur de l'action est requis.";
		public const string RequiredTaskDeadline = "La date d'échéance de la tâche est requise.";
		public const string RequiredTaskName = "La description de la tâche est requise.";
		public const string RequiredTaskProgress = "L'état d'avancement est requis.";
	}
}

public static class GenerateFilesLabels
{
	public const string AllFilesExport = "Export de tous les dossiers";
	public const string Cancel = "Annuler";
	public const string ExcelExport = "Export";
	public const string Generate = "Générer";
	public const string GenerateAFile = "Générer un document";
	public const string GenerateAnahFileModalTitle = "Générer la synthèse de la grille d'analyse du logement";
	public const string GenerateAnahSynthesisButtonLabel = "Synthèse de la grille d'analyse du logement";
	public const string GenerateDocuments = "Génération de documents";
	public const string SelectDocumentTemplate = "Modèle de document";
	public const string WorkCertificate = "Attestation de travaux";
	public const string AnahSynthesisModalTitle = "Génération de la synthèse de la grille d'analyse du logement";
	public const string WorkCertificateModalTitle = "Génération de l'attestation de travaux";
	public const string AnahSynthesisDocumentName = "synthesegrilleanalyselogement.pdf";
	public const string WorkCertificateDocumentName = "attestation-travaux-mpr-accompagne-devis.pdf";
	public const string BillingLogAndAdministrationExport = "Export administratif et de suivi de facturation";

	public static class AnahSynthesisLabel
	{
		public const string AnahFileAccompanyingName = "Nom de l'accompagnateur";
		public const string AnahFileAdapatation = "Adaptation";
		public const string AnahFileAddressCity = "Ville";
		public const string AnahFileAddressNumber = "numéro";
		public const string AnahFileAddressPostalCode = "Code postal";
		public const string AnahFileAddressStreet = "Voie";
		public const string AnahFileInsalubrity = "Insalubrité";
		public const string AnahFileIsolation = "Isolation ITE ITI toiture menuiseries etc";
		public const string AnahFileReparation = "Réparationentretien";
		public const string AnahFileSiretNumber = "N SIRET";
		public const string AnahFileVisitDate = "Date de visite";
		public const string Degradation = "Dégradation";
		public const string HeatingSystem = "Changement du système de chauffage";
		public const string Isolation = "Isolation (ITE, ITI, toiture, menuiseries, etc.)";
		public const string Reparation = "Réparation/entretien";
	}

	public static class WorkCertificateLabel
	{
		public const string RequesterName = "Nom Prénom";
		public const string RequesterNameBis = "Nom Prénom P5";
		public const string StreetNumber = "n°";
		public const string WorkPackagesTotalCostExcludingTax = "cout HT";
		public const string WorkPackagesTotalCostIncludingAllTaxes = "cout TTC";
		public const string PrimaryEnergyBeforeWork = "Energie primaire";
		public const string PrimaryEnergyAfterWork = "Energie primaire p2";
		public const string AnnualGesEmissionsBeforeWork = "Emissions annuelles";
		public const string AnnualGesEmissionsAfterWork = "Emissions annuelles p2";
		public const string EnergyClassBeforeWork = "Classe énergétique";
		public const string EnergyClassAfterWork = "Classe énergétique P2";
		public const string SurfaceBeforeWork = "Surface";
		public const string SurfaceAfterWork = "surface p2";
		public const string TwoClassesEarned = "Gain de 2 classes.0";
		public const string ThreeClassesEarned = "Gain de 3 classes1";
		public const string FourClassesOrMoreEarned = "Gain de 4 classes";
		public const string WorkTypesDescription = "Description des postes de travaux éligibles Row {0}";
		public const string WorkPackagesTotalCost = "Coût des travaux en HT TTC {0}";
		public const string EstimatedAnnualGesEmissionsBeforeWork = "Émissions annuelles de gaz à effet de serre en kgCO2eq/m2/an";
		public const string EstimatedEnergyDpeBeforeWork = "Classe de performance énergétique (de A à G)";
		public const string TotalCostFormat = "{0} HT / {1} TTC";
		public const string AccompanyingPersonName = "Nom Prénom Accompagnateur";
		public const string SocialContext = "Raison sociale p5";
		public const string SiretNumber = "N SIRET P5";
	}

	public static class Errors
	{
		public const string AccompanyingFileIsNotValid =
			"Vous ne pouvez pas générer de document à partir de cette référence, car ce dossier ne vous appartient pas.";
		public const string AccompanyingFileNotFound =
			"Aucune référence de dossier trouvée. Veuillez vérifier la référence saisie.";
		public const string AnahUploadError = "Veuillez charger le document ci-dessus.";
		public const string AnahUploadNotification =
			"Veuillez charger l’accusé de réception du dossier de demande d’aide envoyé par l’Anah.";
		public const string EnergyAuditUploadError = "Veuillez charger l'audit énergétique.";
		public const string RequiredAccompanyingFileReference = "Veuillez renseigner la référence de dossier.";
		public const string ErrorWhileGeneratingDocument = "Une erreur est survenue lors de la génération de votre document, veuillez réessayer.";
		public const string FileNotFound = "Le fichier {0} est introuvable.";
		public const string FileNotSelected = "Veuillez sélectionner un modèle de document.";
        public const string GeneralMeetingUploadError = "Veuillez charger le fichier PV d’Assemblée Générale";
        public const string AnahAidRequestNotificationDocumentError = "Veuillez charger le fichier Notification de demandes d'Aides Anah";
		public const string AuditReportFileError = "Veuillez charger le fichier Audit/Etude thermique";
		public const string handoverReportDocumentError = "Veuillez charger le fichier PV de réception des travaux";
    }
}

public static class RadzenGridFilterLabel
{
	public const string AndOperatorText = "Et";
	public const string ApplyFilterText = "Appliquer";

	public const string ClearFilterText = "Effacer";

	public const string ContainsText = "Contient";
	public const string DoesNotContainText = "Ne contient pas";
	public const string EndsWithText = "Se termine par";

	public const string EqualsText = "Égal à";
	public const string FilterText = "Filtrer";
	public const string GreaterThanOrEqualsText = "Supérieur ou égal à";
	public const string GreaterThanText = "Supérieur à";

	public const string IsEmptyText = "Est vide";
	public const string IsNotEmptyText = "N'est pas vide";
	public const string IsNotNullText = "N'est pas nul";

	public const string IsNullText = "Est nul";
	public const string LessThanOrEqualsText = "Inférieur ou égal à";

	public const string LessThanText = "Inférieur à";
	public const string NotEqualsText = "Différent de";
	public const string OrOperatorText = "Ou";
	public const string StartsWithText = "Commence par";
}

public static class HousingConstructionPeriod
{
	public const string HouseConstructionPeriod1 = "Avant 1948";
	public const string HouseConstructionPeriod2 = "Entre 1948 et 1974";
	public const string HouseConstructionPeriod3 = "Entre 1975 et 1977";
	public const string HouseConstructionPeriod4 = "Entre 1978 et 1982";
	public const string HouseConstructionPeriod5 = "Entre 1983 et 1988";
	public const string HouseConstructionPeriod6 = "Entre 1989 et 2000";
	public const string HouseConstructionPeriod7 = "Entre 2001 et 2005";
	public const string HouseConstructionPeriod8 = "Entre 2006 et 2012";
	public const string HouseConstructionPeriod9 = "Entre 2013 et 2020";
	public const string HouseConstructionPeriod10 = "À partir de 2021";
}

public static class AnahTableLabels
{
	public const string AnahCategoriesInIleDeFrance = "Plafond de ressources en Ile-De-France";
	public const string AnahCategoriesNotInIleDeFrance = "Plafond de ressources hors Ile-De-France";
	public const string AnahRulesEndDate = "Date de fin";
	public const string AnahRulesStartDate = "Date de début";
	public const string LowHouseholdIncome = "Ménage aux revenus modestes";
	public const string NumberOfPeopleInHousehold = "Nombre de personnes composant le ménage";
	public const string NumberOfSupplementaryPeopleInHousehold = "Par personne supplémentaire";
	public const string VeryLowHouseholdIncome = "Ménages aux revenus très modestes";

	public static class Errors
	{
		public const string AnahWithSameNumberOfPeopleAndAnahYearDebutRule =
			"Une catégorie anah existe déjà pour ce nombre de personne et cette année";
		public const string ErrorWhileAddingNewSupplementaryOccupantRule =
			"Erreur lors de l'ajout de la règle pour personnes supplémentaires";
		public const string ErrorWhileCreatingAnahCategory = "Erreur lors de la création de la catégorie ANAH";
		public const string ErrorWhileUpdatingSupplementaryOccupantRule =
			"Erreur lors de la modification de la règle pour personnes supplémentaires";
		public const string FillAllNeededFields =
			"Veuillez entrez les champs: Nombre de personnes dans le ménage, Ménages aux revenus très modestes, Ménages aux revenus modestes et Date de début";
		public const string FillAllNeededFieldsForSupplementaryOccupants =
			"Veuillez entrez les champs: Ménages aux revenus très modestes, Ménages aux revenus modestes et Date de début";
		public const string PeopleInHouseholdIsBiggerThanFive =
			"Veuiller entrer un nombre de personne dans le foyer inférieur à 5";
		public const string SupplementaryOccupantRulesWithSameDateAlreadyExists =
			"Une catégorie par personnes supplémentaires existe déjà pour cette année";
	}
}

public static class AccompanyingTimeDurationLabel
{
	public const string LessThanTwoHours = "Moins de 2h (~ 0,25 jour) : Suivi très ponctuel";
	public const string BetweenTwoAndFiveHours = "Entre 2h et 5h (~ 0,5 jour) : Accompagnement léger";
	public const string BetweenFiveAndFourteenHours = "Entre 5h et 14h (~ 1 à 2 jours) : Accompagnement standard";
	public const string BetweenFourteenAndTwentyHours = "Entre 14h et 20h (~ 2 à 3 jours) : Accompagnement soutenu";
	public const string BetweenTwentyAndTwentyEightHours = "Entre 20h et 28h (~ 3 à 4 jours) : Accompagnement intensif";
	public const string BetweenTwentyEightAndFourtyHours = "Entre 28h et 40h (~ 4 à 6 jours) : Dossier complexe";
	public const string BetweenFourtyAndSixtyHours = "Entre 40h et 60h (~ 6 à 9 jours) : Situation très complexe";
	public const string MoreThanSixtyHours = "Plus de 60h (> 9 jours) : Situation exceptionnelle";
}

public static class CsvImportResultLabels
{
	public const string Success = "Succès";
	public const string PartialSuccess = "Succès partiel";
	public const string Failure = "Echec";
}

public static class UnsavedChangesOptionLabels
{
	public const string SaveAndLeave = "Enregistrer et quitter";
	public const string LeaveWithoutSaving = "Quitter sans enregistrer";
}

public static class ParticipantTypeLabels
{
	public const string Company = "Entreprise";
	public const string ProjectManagementAssistant = "AMO";
	public const string ProjectManagement = "MOE";
}

public static class WorkQualityLabels
{
	public const string Compliant = "Conforme";
	public const string Partial = "Partiel";
	public const string NonCompliant = "Non Conforme";
}

public static class ReneeAiPromptSystem
{
    public static readonly string BasePrompt = LoadPrompt("BasePrompt.txt");

    public static readonly string GlobalSynthesisAnalysisInstructions = LoadPrompt("GlobalSynthesisAnalysisInstructions.txt");

    public static readonly string GlobalSynthesisAnalysisPrompt = LoadPrompt("GlobalSynthesisAnalysisPrompt.txt");

    private static string LoadPrompt(string resourceFile)
    {
        var asm = System.Reflection.Assembly.GetExecutingAssembly();
        var resourceName = typeof(ReneeAiPromptSystem).Namespace + ".Prompts." + resourceFile;
        using var stream = asm.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{resourceName}' not found. Ensure the file is set as EmbeddedResource.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
