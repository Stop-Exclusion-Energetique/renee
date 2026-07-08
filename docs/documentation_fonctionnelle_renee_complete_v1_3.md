# Documentation fonctionnelle de Renée

> **Version du document :** 1.0  
> **Date de rédaction :** 2 juillet 2026  
> **Public concerné :** utilisateurs de Renée, responsables métier, responsables de structures, équipe support, équipe projet et personnes chargées de la recette

---

## Historique du document

| Version | Date | Évolution |
|---|---:|---|
| 1.0 | 02/07/2026 | Première version de la documentation fonctionnelle complète. |

---

## Principes de lecture

Cette documentation décrit **ce que permet Renée**, **qui intervient** et **quelles règles s’appliquent**.

Elle ne constitue pas un manuel écran par écran. L’organisation visuelle de l’application peut évoluer sans modifier les règles métier décrites ici. Les champs et onglets ne sont donc pas énumérés un par un, sauf lorsqu’ils correspondent à une règle importante.

Les mentions **À confirmer** signalent les comportements qui doivent encore être validés par les responsables métier.

---

## Sommaire

1. [Objet du document](#1-objet-du-document)
2. [Présentation générale de Renée](#2-présentation-générale-de-renée)
3. [Périmètre fonctionnel](#3-périmètre-fonctionnel)
4. [Vocabulaire métier](#4-vocabulaire-métier)
5. [Rôles et responsabilités](#5-rôles-et-responsabilités)
6. [Accès à Renée et gestion du compte](#6-accès-à-renée-et-gestion-du-compte)
7. [Navigation et principes d’utilisation](#7-navigation-et-principes-dutilisation)
8. [Tableaux de bord et pilotage](#8-tableaux-de-bord-et-pilotage)
9. [Recherche et consultation des dossiers](#9-recherche-et-consultation-des-dossiers)
10. [Création et attribution d’un dossier ménage](#10-création-et-attribution-dun-dossier-ménage)
11. [Contenu fonctionnel d’un dossier ménage](#11-contenu-fonctionnel-dun-dossier-ménage)
12. [Parcours complet d’un dossier](#12-parcours-complet-dun-dossier)
13. [Synthèse des trois jalons](#13-synthèse-des-trois-jalons)
14. [Soumission, validation et statuts](#14-soumission-validation-et-statuts)
15. [Abandon, clôture et suppression](#15-abandon-clôture-et-suppression)
16. [Documents, tâches et notifications](#16-documents-tâches-et-notifications)
17. [Aides, financement et facturation](#17-aides-financement-et-facturation)
18. [Parcours des copropriétés](#18-parcours-des-copropriétés)
19. [Exports et rapports](#19-exports-et-rapports)
20. [Fonctions d’administration](#20-fonctions-dadministration)
21. [Assistant Renée AI](#21-assistant-renée-ai)
22. [Règles transverses](#22-règles-transverses)
23. [Situations particulières](#23-situations-particulières)

---

# 1. Objet du document

Cette documentation présente le fonctionnement de Renée du point de vue métier.

Elle doit permettre de comprendre :

- la finalité de l’application ;
- les profils d’utilisateurs et leurs responsabilités ;
- les principaux parcours disponibles ;
- la constitution et le suivi d’un dossier ménage ;
- le parcours spécifique des copropriétés ;
- les règles de soumission, de validation, d’abandon et de clôture ;
- la gestion des documents, tâches, aides, financements et factures ;
- les fonctions de pilotage et d’administration ;
- les contrôles et situations particulières à connaître.

Ce document peut être utilisé pour :

- former les utilisateurs ;
- préparer une recette fonctionnelle ;
- analyser une demande d’évolution ;
- vérifier la cohérence d’un parcours ;
- faciliter les échanges entre les équipes métier, support et projet.

Il ne décrit pas l’installation, l’hébergement ou les choix de réalisation de Renée.

---

# 2. Présentation générale de Renée

Renée est une application de suivi des accompagnements liés à la rénovation de logements et à la lutte contre l’exclusion énergétique.

Elle centralise les informations nécessaires pour suivre :

- le ménage accompagné ;
- le logement concerné ;
- la situation sociale, financière et énergétique ;
- l’équipe d’accompagnement ;
- le projet de rénovation ;
- les travaux envisagés puis réalisés ;
- les aides et les financements ;
- les pièces justificatives ;
- les tâches et échéances ;
- les décisions prises au cours du parcours ;
- la clôture ou l’abandon de l’accompagnement ;
- les besoins de pilotage et de facturation.

Le parcours d’un dossier ménage repose sur trois jalons :

1. **Identifier** la situation et les besoins ;
2. **Organiser et financer** le projet de travaux ;
3. **Réaliser et suivre** les travaux et leurs résultats.

Renée comprend également :

- un parcours consacré aux copropriétés ;
- des tableaux de bord ;
- des exports ;
- des fonctions d’administration ;
- un assistant conversationnel.

---

# 3. Périmètre fonctionnel

## 3.1 Domaines couverts

| Domaine | Finalité |
|---|---|
| Comptes utilisateurs | Demander un accès, valider une inscription, se connecter et gérer son profil. |
| Dossiers ménages | Créer, compléter, soumettre, valider, abandonner et clôturer un accompagnement. |
| Équipes d’accompagnement | Identifier les professionnels et partenaires intervenant sur un dossier. |
| Documents | Ajouter, consulter, remplacer ou supprimer les pièces autorisées. |
| Tâches | Organiser les actions à réaliser, les attribuer et suivre leur avancement. |
| Aides et financement | Préparer puis suivre le plan de financement du projet. |
| ANAH | Identifier les dossiers concernés et suivre les informations de dépôt. |
| Fonds Stop Exclusion Énergétique | Préparer une demande relative au reste à charge lorsque le dossier le nécessite. |
| Facturation | Suivre les montants et informations de facturation associés aux jalons. |
| Copropriétés | Suivre un projet collectif de rénovation selon un parcours dédié. |
| Pilotage | Consulter des indicateurs, filtrer les dossiers et produire des exports. |
| Administration | Gérer les comptes, structures, territoires et paramètres métier. |
| Assistance | Consulter des aides et utiliser l’assistant Renée AI. |

## 3.2 Parcours fonctionnels principaux

| Parcours | Début | Résultat attendu |
|---|---|---|
| Inscription | Une personne demande un accès | Compte accepté ou refusé avec un rôle et un périmètre définis. |
| Dossier ménage | Un professionnel crée ou reçoit un dossier | Dossier terminé, abandonné ou supprimé selon les règles applicables. |
| Validation d’un jalon | Une synthèse est soumise | Jalon accepté ou renvoyé pour correction. |
| Copropriété | Une fiche de copropriété est créée | Projet collectif suivi jusqu’à sa clôture. |
| Suivi opérationnel | Une tâche ou une échéance est créée | Action réalisée, reportée ou clôturée. |
| Pilotage | Un utilisateur consulte un tableau de bord | Vision consolidée de son périmètre autorisé. |
| Administration | Une demande ou un paramètre doit être traité | Compte, structure, territoire ou référentiel mis à jour. |

## 3.3 Hors périmètre

Cette documentation ne détaille pas :

- l’infrastructure de Renée ;
- les sauvegardes ;
- les échanges techniques avec d’autres services ;
- les procédures de déploiement ;
- le code ou les choix de développement.

---

# 4. Vocabulaire métier

| Terme | Définition |
|---|---|
| Dossier ménage | Ensemble des informations relatives à l’accompagnement d’un ménage et de son logement. |
| Jalon | Étape structurante du parcours d’accompagnement. |
| Synthèse | Récapitulatif présenté avant la soumission ou la validation d’un jalon. |
| Parcours diffus | Parcours relevant principalement du Coordinateur ou de la Coordinatrice du Diffus. |
| Parcours ciblé | Parcours relevant principalement de l’Ensemblier ou de l’Ensemblière Territorial·e et du Coordinateur ou de la Coordinatrice du Ciblé. |
| Programme TZEE | Programme Territoires Zéro Exclusion Énergétique. |
| Rattachement | Lien donnant à un utilisateur l’accès à un dossier, une structure ou un territoire. |
| Pièce obligatoire | Document nécessaire pour poursuivre ou valider une étape. |
| Pièce complémentaire | Document utile au dossier sans être systématiquement obligatoire. |
| Reste à charge | Somme restant à financer après prise en compte des aides et autres ressources. |
| Tiers de confiance | Personne ou structure soutenant le ménage sans assurer les validations dans Renée. |
| Clôture | Fin fonctionnelle de l’accompagnement, généralement après validation du troisième jalon. |

---

# 5. Rôles et responsabilités

Les droits dépendent à la fois :

- du rôle attribué dans Renée ;
- du rattachement au dossier ;
- du territoire ou de la structure concernée ;
- du type d’accompagnement.

Un rôle ne donne donc pas automatiquement accès à tous les dossiers.

## 5.1 Ensemblier·ère Solidaire, ES

L’Ensemblier ou l’Ensemblière Solidaire assure principalement le suivi opérationnel du ménage.

Il ou elle peut notamment :

- créer un dossier ou être rattaché à un dossier existant ;
- recueillir et renseigner les informations ;
- ajouter les documents ;
- préparer et soumettre les synthèses ;
- corriger un jalon après une demande de modification ;
- créer et suivre des tâches ;
- demander l’abandon du dossier.

Un dossier peut comporter plusieurs Ensembliers Solidaires.

Lorsque le parcours impose une validation distincte, l’ES ne valide pas seul la synthèse qu’il vient de soumettre.

## 5.2 Ensemblier·ère Territorial·e, ET

L’Ensemblier ou l’Ensemblière Territorial·e intervient principalement dans les parcours ciblés.

Il ou elle peut, selon son rattachement :

- créer ou compléter un dossier ;
- examiner une synthèse ;
- accepter un jalon ou demander des corrections ;
- traiter une demande d’abandon ;
- suivre les tâches et les documents ;
- renseigner les informations de facturation.

## 5.3 Coordinateur·rice du Diffus, CD

Le Coordinateur ou la Coordinatrice du Diffus intervient sur les dossiers du parcours diffus.

Il ou elle peut notamment :

- consulter et piloter les dossiers de son périmètre ;
- examiner les synthèses soumises ;
- valider un jalon ou demander des corrections ;
- traiter les demandes d’abandon ;
- suivre les indicateurs et la facturation autorisée.

## 5.4 Coordinateur·rice du Ciblé, CC

Le Coordinateur ou la Coordinatrice du Ciblé intervient sur les dossiers du parcours ciblé.

Il ou elle peut notamment :

- consulter et compléter les dossiers de son périmètre ;
- examiner une synthèse lorsqu’il ou elle est rattaché·e ;
- accepter un jalon ou demander des corrections ;
- traiter une demande d’abandon ;
- consulter les indicateurs de son territoire.

Dans le parcours ciblé, la validation est principalement portée par l’ET. Le CC peut également intervenir selon le rattachement et les règles du dossier.

## 5.5 Référent·e structurel·le

Le Référent ou la Référente structurel·le dispose d’une vision liée à sa structure.

Selon les droits accordés, ce rôle peut :

- consulter les dossiers de la structure ;
- créer un dossier ;
- suivre les tâches, documents et indicateurs ;
- accéder aux exports autorisés.

Ce rôle n’est pas le validateur opérationnel habituel des jalons.

## 5.6 Membre de l’équipe STOP Exclusion Énergétique

Ce rôle dispose principalement d’un accès de consultation et de pilotage.

Il peut consulter les dossiers, synthèses et indicateurs correspondant à son périmètre. Il n’assure pas la saisie opérationnelle courante ni la validation habituelle des jalons.

## 5.7 Tiers de confiance

Le tiers de confiance est renseigné comme partenaire du ménage.

Son identité, sa structure, ses coordonnées et son rôle peuvent être précisés. Il ne réalise pas de validation dans Renée.

## 5.8 Administrateur·rice

L’administrateur ou l’administratrice dispose d’un accès transversal pour :

- gérer les inscriptions et les comptes ;
- gérer les structures, territoires et paramètres métier ;
- consulter les dossiers ;
- intervenir exceptionnellement sur une validation ou un abandon ;
- importer des données ;
- assister les utilisateurs ;
- consulter ou compléter les informations administratives et de facturation.

## 5.9 Matrice synthétique des responsabilités

| Action | ES | ET | CD | CC | Référent structurel | Membre STOP | Administrateur |
|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| Créer un dossier | Oui | Oui | Oui | Oui | Selon droits | Non | Oui |
| Compléter un dossier rattaché | Oui | Oui | Oui | Oui | Limité | Non | Oui |
| Ajouter des documents | Oui | Oui | Oui | Oui | Selon droits | Non | Oui |
| Soumettre un jalon | Oui | Oui | Oui | Oui | Non | Non | Oui |
| Valider ou rejeter un jalon | Non si validation distincte requise | Oui | Oui | Oui | Non | Non | Oui |
| Créer ou modifier une tâche | Oui | Oui | Selon rattachement | Selon rattachement | Consultation possible | Non | Oui |
| Demander l’abandon | Oui | Oui | Oui | Oui | Non | Non | Oui |
| Traiter l’abandon | Non seul | Oui | Oui | Oui | Non | Non | Oui |
| Modifier la facturation | Non | Oui | Oui | Oui | Non | Non | Oui |
| Administrer les comptes | Non | Non | Non | Non | Non | Non | Oui |

> Cette matrice présente les droits généraux. L’accès réel dépend toujours du rattachement et du périmètre de l’utilisateur.

---

# 6. Accès à Renée et gestion du compte

## 6.1 Demande d’inscription

La personne renseigne ses coordonnées professionnelles, son rôle demandé, sa structure et son territoire.

Elle accepte les conditions d’utilisation applicables au moment de la demande.

La demande reste en attente jusqu’à son examen par une personne autorisée.

## 6.2 Validation ou refus

Le responsable de la demande vérifie :

- l’identité et les coordonnées ;
- la structure ;
- le rôle demandé ;
- le territoire ;
- la cohérence du périmètre d’accès.

La demande est ensuite acceptée, corrigée ou refusée. L’utilisateur est informé de la décision.

## 6.3 Connexion

L’utilisateur se connecte avec le moyen prévu par Renée.

L’accès peut être refusé lorsque :

- le compte n’est pas encore validé ;
- le compte est désactivé ;
- une condition d’utilisation obligatoire n’a pas été acceptée ;
- les informations de connexion sont incorrectes.

## 6.4 Profil et suppression du compte

L’utilisateur peut consulter ou modifier les informations autorisées de son profil.

Il peut également demander la suppression de son compte. Avant l’acceptation, les dossiers, tâches et responsabilités encore attribués doivent être réaffectés.

---

# 7. Navigation et principes d’utilisation

Renée adapte ses menus et actions au rôle de la personne connectée.

Les principaux espaces peuvent comprendre :

- le tableau de bord ;
- les dossiers ménages ;
- les copropriétés ;
- les tâches ;
- les exports ;
- les fonctions d’administration ;
- le profil ;
- l’aide et l’assistant.

## 7.1 Principes communs

- Les actions non autorisées ne doivent pas être proposées ou doivent être clairement bloquées.
- Le jalon et le statut du dossier doivent rester visibles pendant la consultation.
- Les informations obligatoires doivent être distinguées des informations facultatives.
- Une confirmation doit être demandée avant une action importante ou irréversible.
- L’utilisateur doit être averti lorsqu’il risque de perdre une saisie non enregistrée.
- Les messages doivent expliquer le problème et l’action attendue.

## 7.2 Niveau de détail retenu

Cette documentation décrit les fonctions et règles métier, et non la position exacte de chaque bouton. Les captures et procédures pas à pas peuvent être regroupées dans un guide utilisateur séparé.

---

# 8. Tableaux de bord et pilotage

Les tableaux de bord donnent une vision synthétique de l’activité correspondant au périmètre de l’utilisateur.

Ils peuvent présenter :

- le nombre de dossiers actifs ;
- les dossiers par jalon ;
- les dossiers en attente de validation ;
- les dossiers rejetés ;
- les demandes d’abandon en attente ;
- les dossiers abandonnés ou terminés ;
- les tâches à réaliser ou en retard ;
- la durée moyenne des accompagnements ;
- les dossiers concernés par l’ANAH ;
- les montants ou états de facturation ;
- la répartition par structure, territoire ou type d’accompagnement.

Les filtres peuvent porter sur :

- une période ;
- un territoire ;
- une structure ;
- un professionnel ;
- un type d’accompagnement ;
- un jalon ;
- un statut.

Un indicateur doit toujours être accompagné d’une définition compréhensible. Les règles de calcul doivent être validées par les responsables métier, surtout pour les durées, les montants et les taux.

---

# 9. Recherche et consultation des dossiers

La liste des dossiers permet à l’utilisateur de retrouver les accompagnements auxquels il est autorisé à accéder.

## 9.1 Informations principales affichées

La liste présente de manière synthétique :

- la référence du dossier ;
- l’identité du ménage ou de l’occupant principal ;
- le territoire ou la structure ;
- le type d’accompagnement ;
- le jalon courant ;
- le statut ;
- les principaux membres de l’équipe ;
- la date de dernière modification ;
- les alertes importantes.

## 9.2 Recherche, filtres et tri

La recherche peut porter sur la référence, le nom du ménage ou d’autres informations autorisées.

Les filtres permettent notamment d’isoler les dossiers :

- d’un statut ou d’un jalon ;
- d’un territoire ou d’une structure ;
- attribués à un professionnel ;
- diffus ou ciblés ;
- relevant du programme TZEE ;
- concernés par l’ANAH ;
- en attente de validation, d’abandon ou de facturation.

## 9.3 Actions disponibles

Selon les droits, l’utilisateur peut :

- ouvrir le dossier ;
- poursuivre la saisie ;
- consulter une synthèse ;
- examiner une validation ;
- traiter une demande d’abandon ;
- accéder aux tâches et documents ;
- supprimer un dossier lorsque cette action est encore autorisée.

---

# 10. Création et attribution d’un dossier ménage

## 10.1 Création

La création permet d’enregistrer les informations minimales nécessaires pour ouvrir un accompagnement :

- identité et coordonnées principales du ménage ;
- adresse et type de logement ;
- type d’accompagnement ;
- territoire ;
- origine du repérage ;
- programme concerné ;
- premiers membres de l’équipe.

## 10.2 Attribution

Selon le parcours et le rôle du créateur :

- l’auteur peut devenir le référent principal ;
- un ES, un ET, un CD ou un CC peut être sélectionné ;
- plusieurs professionnels peuvent être ajoutés ;
- le territoire et la structure déterminent la visibilité du dossier.

## 10.3 Contrôles

Renée vérifie notamment :

- la présence des informations obligatoires ;
- la cohérence entre le type de parcours et les rôles sélectionnés ;
- la présence d’un référent principal ;
- la cohérence de l’adresse et du territoire ;
- l’existence possible d’un dossier en double.

## 10.4 Résultat

Après création :

- une référence est attribuée ;
- le jalon **Identifier** est ouvert ;
- le statut est **En cours** ;
- le dossier devient visible pour les personnes autorisées.

## 10.5 Import

Des dossiers peuvent également être créés ou mis à jour par import lorsque cette fonction est autorisée.

Un compte rendu doit indiquer :

- les lignes traitées ;
- les créations ou mises à jour réussies ;
- les erreurs à corriger ;
- le résultat global de l’opération.

---

# 11. Contenu fonctionnel d’un dossier ménage

Un dossier regroupe plusieurs catégories d’informations. Elles peuvent être présentées dans différents écrans, mais leur finalité reste la suivante.

## 11.1 Ménage

- identité et coordonnées ;
- composition du foyer ;
- situation sociale et professionnelle ;
- ressources et charges ;
- difficultés rencontrées ;
- accompagnements sociaux existants ;
- contraintes de santé, de handicap ou d’autonomie utiles au projet.

## 11.2 Logement

- adresse et caractéristiques générales ;
- statut d’occupation ;
- surface, ancienneté et configuration ;
- état initial ;
- désordres et risques constatés ;
- équipements ;
- consommations énergétiques ;
- diagnostics et performances.

## 11.3 Équipe d’accompagnement

- ES principal et éventuels ES supplémentaires ;
- ET principal et éventuel second ET ;
- CD ou CC ;
- tiers de confiance ;
- coordonnées et responsabilités utiles.

## 11.4 Projet de travaux

- besoins identifiés ;
- travaux envisagés ;
- priorités et urgences ;
- adaptation, sécurité et rénovation énergétique ;
- auto-réhabilitation accompagnée ;
- entreprises, qualifications et assurances ;
- coûts prévisionnels.

## 11.5 Financement

- aides envisagées ;
- financements publics et privés ;
- prêts ;
- apport du ménage ;
- contribution de proches ;
- intervention éventuelle du Fonds ;
- reste à charge.

## 11.6 Suivi des travaux et résultats

- calendrier ;
- avancement ;
- travaux réellement réalisés ;
- difficultés ;
- coût final ;
- résultats énergétiques et sociaux ;
- satisfaction du ménage ;
- fin du chantier et de l’accompagnement.

## 11.7 Éléments transverses

Le dossier comprend également :

- les documents ;
- les tâches ;
- les commentaires utiles ;
- les informations ANAH ;
- les informations de facturation ;
- les dates de validation et de clôture.

---

# 12. Parcours complet d’un dossier

Le schéma ci-dessous présente les acteurs, les actions et les principaux changements de statut.

![Schéma fonctionnel du parcours d’un dossier avec les rôles explicites](images\renee_workflow_fonctionnel_roles_explicites.png)

## 12.1 Lecture synthétique

1. Le dossier est créé au jalon **Identifier**, avec le statut **En cours**.
2. Les informations et documents utiles sont complétés.
3. Une synthèse est préparée puis soumise.
4. Selon le type de dossier et le rôle du soumissionnaire, le jalon avance directement ou passe **En attente de validation**.
5. Le validateur accepte le jalon ou demande des corrections.
6. En cas de correction, le dossier reste sur le même jalon avec le statut **Rejeté**.
7. Après correction, une nouvelle soumission est possible.
8. L’acceptation ouvre le jalon suivant avec le statut **En cours**.
9. L’acceptation du troisième jalon termine le parcours.
10. Une demande d’abandon peut interrompre le parcours avant son terme.

---

# 13. Synthèse des trois jalons

Les jalons sont décrits par leur objectif métier, et non par la liste détaillée de leurs onglets.

| Jalon | Objectif | Informations principales | Résultat attendu |
|---|---|---|---|
| **1. Identifier** | Comprendre la situation du ménage, du logement et les besoins d’accompagnement. | Situation du ménage, logement, difficultés sociales et énergétiques, origine du repérage, équipe, premières démarches. | Situation suffisamment qualifiée pour décider de la poursuite et préparer le projet. |
| **2. Organiser et financer** | Définir un projet de travaux cohérent et construire son financement prévisionnel. | Diagnostic, travaux prévus, coûts, performances attendues, artisans, contraintes, aides, reste à charge, démarches ANAH. | Projet techniquement et financièrement préparé pour passer à la réalisation. |
| **3. Réaliser et suivre** | Suivre le chantier, consolider le financement final et évaluer les résultats. | Avancement, travaux réalisés, coûts et factures, financement final, résultats, satisfaction, dates de fin. | Accompagnement finalisé et dossier prêt à être clôturé. |

## 13.1 Jalon 1 : Identifier

L’ES constitue principalement les informations avec le ménage.

Le jalon doit permettre de comprendre :

- la situation du foyer ;
- les caractéristiques et difficultés du logement ;
- les besoins prioritaires ;
- les acteurs qui doivent intervenir ;
- les premières démarches nécessaires.

La synthèse est ensuite examinée selon le parcours diffus ou ciblé.

## 13.2 Jalon 2 : Organiser et financer

L’équipe transforme les besoins identifiés en projet réalisable.

Le jalon doit permettre de vérifier :

- que les travaux envisagés répondent aux besoins ;
- que les coûts sont suffisamment estimés ;
- que les aides et financements sont identifiés ;
- que le reste à charge est connu ;
- que les principales contraintes sont anticipées.

## 13.3 Jalon 3 : Réaliser et suivre

L’équipe suit la réalisation effective du projet et prépare la fin de l’accompagnement.

Le jalon doit permettre de vérifier :

- les travaux réellement réalisés ;
- les coûts et financements finaux ;
- les écarts avec le projet initial ;
- les résultats obtenus ;
- la situation du ménage après travaux ;
- les conditions de clôture du dossier.

---

# 14. Soumission, validation et statuts

## 14.1 Préparation de la synthèse

Avant la soumission, l’utilisateur vérifie :

- les informations principales ;
- les pièces attendues ;
- la cohérence des données ;
- les éventuels messages de contrôle.

## 14.2 Soumission

La soumission indique que le jalon est prêt à être examiné.

Selon le rôle du soumissionnaire, le programme et le type de parcours :

- le dossier passe **En attente de validation** ;
- ou le jalon est accepté directement lorsque le rôle dispose du droit correspondant.

Pour un parcours diffus, la validation est principalement assurée par le CD.

Pour un parcours ciblé, elle est principalement assurée par l’ET, avec une intervention possible du CC selon le rattachement.

## 14.3 Décisions possibles

| Décision | Conséquence |
|---|---|
| Accepter le jalon | Le jalon suivant s’ouvre avec le statut **En cours**. |
| Demander des corrections | Le dossier reste sur le jalon courant avec le statut **Rejeté**. |
| Accepter le troisième jalon | Le dossier passe au jalon et au statut **Terminé**. |

## 14.4 Correction

Lorsqu’une correction est demandée :

- un commentaire explique les modifications attendues ;
- le dossier redevient modifiable par les personnes autorisées ;
- le jalon peut être soumis de nouveau après correction.

## 14.5 Statuts

| Statut | Signification | Prochaine action attendue |
|---|---|---|
| **En cours** | Le dossier est en saisie, en préparation ou en correction. | Compléter puis soumettre le jalon. |
| **En attente de validation** | Une synthèse attend une décision. | Le validateur accepte ou demande des corrections. |
| **Rejeté** | Des corrections sont demandées ou une condition n’est pas satisfaite. | Corriger puis soumettre de nouveau. |
| **En attente d’abandon** | Une demande d’arrêt attend une décision. | Accepter ou refuser l’abandon. |
| **Abandonné** | L’accompagnement est arrêté avant son terme. | Consultation et traitement administratif éventuel. |
| **Terminé** | Les trois jalons sont achevés et acceptés. | Consultation, documents et suivi administratif éventuel. |

## 14.6 Traçabilité des validations

Une validation doit permettre d’identifier :

- le jalon concerné ;
- la date ;
- la personne ayant pris la décision ;
- le commentaire éventuel en cas de correction.

---

# 15. Abandon, clôture et suppression

## 15.1 Demande d’abandon

Une personne autorisée peut demander l’arrêt de l’accompagnement.

La demande précise :

- le motif ;
- un commentaire complémentaire si nécessaire ;
- les éléments administratifs utiles ;
- l’éventuelle demande de facturation.

Le dossier passe alors **En attente d’abandon**.

## 15.2 Décision sur l’abandon

La demande est examinée par un ET, un CD, un CC ou un administrateur selon le dossier.

| Décision | Conséquence |
|---|---|
| Abandon accepté | Le dossier devient **Abandonné** et une date de fin est enregistrée. |
| Abandon refusé | Le parcours reprend et le dossier revient au statut permettant sa poursuite. |

## 15.3 Clôture normale

Après acceptation du troisième jalon :

- le dossier devient **Terminé** ;
- les dates de fin sont enregistrées ;
- la modification opérationnelle courante est bloquée ;
- le dossier reste consultable selon les droits.

## 15.4 Suppression

La suppression concerne principalement un dossier créé par erreur ou sans donnée utile.

Elle doit être réservée aux personnes autorisées et accompagnée d’une confirmation.

Un dossier ayant déjà fait l’objet d’un accompagnement, d’une validation ou d’une facturation ne doit pas être supprimé sans règle métier explicite.

---

# 16. Documents, tâches et notifications

## 16.1 Documents

Les utilisateurs autorisés peuvent :

- ajouter une pièce ;
- choisir sa catégorie ;
- la consulter ou la télécharger ;
- la remplacer ;
- la supprimer lorsque cette action reste autorisée.

Les contrôles portent notamment sur :

- le format ;
- la taille ;
- la catégorie ;
- la présence des pièces obligatoires ;
- les droits d’accès.

Les principales catégories de documents peuvent concerner :

- l’accompagnement ;
- les diagnostics ;
- les devis et travaux ;
- l’ANAH ;
- les financements ;
- les factures ;
- la réception des travaux ;
- le bilan final.

## 16.2 Tâches

Une tâche comprend au minimum :

- un titre ;
- une priorité ;
- une date d’échéance ;
- une personne responsable ;
- un état d’avancement.

Les tâches peuvent être :

- à venir ;
- à réaliser ;
- en retard ;
- terminées.

Elles peuvent être créées depuis un dossier et apparaître dans le suivi personnel de l’utilisateur.

## 16.3 Notifications

Renée peut notifier les événements importants, notamment :

- validation ou refus d’un compte ;
- attribution ou modification d’une tâche ;
- synthèse prête à être examinée ;
- jalon accepté ;
- corrections demandées ;
- demande d’abandon ;
- acceptation ou refus de l’abandon.

Chaque notification doit préciser le dossier concerné, l’action attendue et, lorsque cela est pertinent, l’échéance.

---

# 17. Aides, financement et facturation

## 17.1 Plan de financement

Le plan de financement permet de rapprocher :

- le coût prévisionnel ou final des travaux ;
- les aides nationales et locales ;
- les prêts ;
- l’apport du ménage ;
- l’aide de proches ;
- les financements associatifs ;
- le reste à charge.

Les montants doivent être cohérents et compréhensibles par l’utilisateur.

## 17.2 ANAH

Lorsqu’un dossier doit être déposé auprès de l’ANAH, Renée permet de renseigner :

- l’obligation ou non de dépôt ;
- la date de dépôt ;
- le numéro du dossier ;
- les documents associés ;
- la décision ou notification reçue, lorsque cette information est disponible.

Ces informations doivent rester visibles dans le suivi du financement.

## 17.3 Fonds Stop Exclusion Énergétique

Lorsque le reste à charge le justifie, une demande peut être préparée pour le Fonds.

Elle rassemble les informations nécessaires pour comprendre :

- la situation du ménage ;
- le projet ;
- le plan de financement ;
- le reste à charge ;
- le montant sollicité ;
- les justificatifs utiles.

## 17.4 Facturation

Le suivi de la facturation peut être réalisé par jalon.

Il peut comprendre :

- l’état facturé ou non facturé ;
- le montant ;
- la date ;
- le numéro d’appel de fonds ;
- le numéro de facture ;
- les éléments nécessaires à l’export administratif.

La validation d’un jalon et la facturation sont liées, mais restent deux actions distinctes.

Les personnes autorisées à modifier la facturation sont principalement les ET, CD, CC et administrateurs.

---

# 18. Parcours des copropriétés

Renée comprend un parcours spécifique pour les projets de rénovation collective.

## 18.1 Création et rattachement

La fiche comprend les informations essentielles sur :

- la copropriété ;
- son adresse ;
- son territoire ;
- son équipe d’accompagnement ;
- son type de parcours.

## 18.2 Contenu fonctionnel

Le parcours regroupe notamment :

- les caractéristiques du bâtiment ;
- le nombre de lots ;
- la gouvernance et le syndic ;
- l’assistance à maîtrise d’ouvrage ;
- les diagnostics ;
- les travaux collectifs ;
- les votes en assemblée générale ;
- les aides et le financement ;
- le suivi du chantier ;
- les coûts et factures finaux.

## 18.3 Jalons de la copropriété

| Jalon | Finalité |
|---|---|
| Identifier | Qualifier la copropriété, sa gouvernance et ses performances. |
| Organiser et financer | Définir les travaux, les coûts, les votes et les aides. |
| Réaliser et suivre | Suivre le chantier collectif jusqu’à son achèvement. |

Chaque jalon dispose d’une synthèse et d’une décision d’acceptation ou de correction.

Les règles exactes de validation doivent être confirmées lorsque plusieurs rôles sont rattachés à la copropriété.

---

# 19. Exports et rapports

Les exports permettent de produire une vue consolidée des informations autorisées.

Ils peuvent servir :

- au pilotage ;
- à la facturation ;
- au suivi des aides ;
- à la préparation de bilans ;
- à l’analyse des dossiers d’un territoire ou d’une structure.

L’utilisateur choisit les filtres applicables avant la génération.

Un export doit préciser :

- son périmètre ;
- sa date de génération ;
- les filtres appliqués ;
- la période couverte.

Les fichiers exportés contiennent potentiellement des données personnelles. Ils doivent être conservés, transmis et supprimés conformément aux règles de confidentialité.

---

# 20. Fonctions d’administration

## 20.1 Comptes et inscriptions

L’administration permet de :

- consulter les demandes ;
- corriger les informations ;
- accepter ou refuser une inscription ;
- consulter et désactiver les comptes ;
- traiter les demandes de suppression.

## 20.2 Structures et territoires

L’administrateur peut :

- gérer les structures ;
- rattacher une structure à une organisation nationale ;
- gérer les territoires ;
- affecter les utilisateurs ;
- vérifier les conséquences d’un changement sur la visibilité des dossiers.

## 20.3 Référentiels métier

Les paramètres administrables peuvent comprendre :

- les catégories et plafonds ANAH ;
- les motifs d’abandon ;
- les types de travaux ;
- les catégories de documents ;
- les conditions générales d’utilisation ;
- d’autres listes utilisées dans les dossiers.

## 20.4 Imports

L’administrateur peut lancer un import, consulter son résultat et corriger les erreurs signalées.

## 20.5 Assistance aux utilisateurs

Des fonctions d’assistance peuvent permettre à l’administrateur de reproduire l’affichage d’un utilisateur ou de vérifier son périmètre.

Toute intervention doit être limitée au besoin de support, identifiable et conforme aux règles de confidentialité.

---

# 21. Assistant Renée AI

Renée AI aide l’utilisateur à rechercher ou analyser certaines informations.

Il peut permettre de :

- poser une question ;
- joindre un document autorisé ;
- obtenir une synthèse ;
- repérer des informations utiles ;
- accompagner la compréhension d’un dossier.

L’assistant ne remplace pas la décision métier.

L’utilisateur doit vérifier :

- les identités ;
- les dates ;
- les montants ;
- les résultats de calcul ;
- les recommandations ;
- les décisions proposées.

Les informations sensibles ne doivent être transmises que lorsque cela est nécessaire et autorisé.

---

# 22. Règles transverses

## 22.1 Droits et périmètres

- Un utilisateur ne consulte que les dossiers correspondant à ses droits et rattachements.
- Les actions sensibles dépendent du rôle.
- Un changement de territoire, de structure ou d’équipe peut modifier la visibilité du dossier.
- Un dossier terminé ou abandonné est principalement consultable, sauf action administrative autorisée.

## 22.2 Qualité de la saisie

Renée contrôle notamment :

- les champs obligatoires ;
- les formats de date, adresse, courriel et téléphone ;
- les montants négatifs ou incohérents ;
- les dates dans un ordre impossible ;
- les pourcentages et notes hors limites ;
- les doublons ;
- les pièces manquantes ;
- la cohérence entre le coût du projet et son financement.

## 22.3 Enregistrement et prévention des pertes

L’utilisateur doit pouvoir enregistrer son travail avant la soumission d’un jalon.

Une alerte doit être affichée avant de quitter une page contenant des modifications non enregistrées.

## 22.4 Données personnelles et confidentialité

Renée traite des informations personnelles, sociales, financières et parfois sensibles.

Les utilisateurs doivent :

- consulter uniquement les dossiers nécessaires à leur mission ;
- limiter les commentaires aux informations utiles ;
- éviter les jugements ou formulations inappropriées ;
- protéger les fichiers téléchargés ;
- ne pas transmettre un export à une personne non autorisée ;
- signaler toute erreur d’accès ou de destinataire.

## 22.5 Traçabilité

Les actions sensibles doivent pouvoir être attribuées à une personne et une date, notamment :

- création et modification d’un dossier ;
- validation ou rejet ;
- abandon ;
- clôture ;
- modification de la facturation ;
- intervention d’assistance administrative.

## 22.6 Accessibilité et compréhension

- Les statuts ne doivent pas être distingués uniquement par une couleur.
- Les libellés doivent être compréhensibles sans connaissance technique.
- Les messages d’erreur doivent indiquer comment corriger le problème.
- La navigation au clavier et la lecture des contenus doivent être possibles dans les conditions prévues par le service.
- Les documents ajoutés doivent être lisibles et correctement nommés.

## 22.7 Support

Une demande de support doit contenir :

- le rôle de l’utilisateur ;
- le dossier concerné, sans recopier inutilement des données personnelles ;
- l’action réalisée ;
- le message observé ;
- la date et l’heure ;
- une capture si elle est nécessaire et autorisée.

Les coordonnées du support restent **à renseigner** dans la version diffusée du document.

---

# 23. Situations particulières

## 23.1 Changement de membre de l’équipe

Avant de retirer un professionnel, les tâches, responsabilités et validations en attente doivent être réattribuées.

L’historique des actions déjà réalisées doit rester conservé.

## 23.2 Compte désactivé

La désactivation d’un compte ne doit pas supprimer les informations déjà saisies.

Les dossiers et tâches encore attribués doivent être transférés.

## 23.3 Changement de territoire ou de structure

Le changement doit être contrôlé afin d’éviter :

- une perte d’accès pour les personnes qui poursuivent l’accompagnement ;
- un accès injustifié pour l’ancien périmètre ;
- l’absence de coordinateur ou de validateur.

## 23.4 Dossier en double

Lorsqu’un doublon est identifié, les responsables déterminent :

- le dossier à conserver ;
- les informations ou documents à reprendre ;
- les tâches à transférer ;
- la manière de supprimer ou classer le doublon.

## 23.5 Correction après validation

La correction d’un jalon déjà validé doit être encadrée, car elle peut modifier :

- la synthèse ;
- les indicateurs ;
- la facturation ;
- les documents produits ;
- les décisions prises ensuite.

La règle exacte de réouverture reste **à confirmer**.

## 23.6 Pièce impossible à obtenir

Une pièce obligatoire manquante ne doit pas être contournée sans décision explicite.

Le dossier peut nécessiter :

- un justificatif alternatif ;
- un commentaire ;
- une validation exceptionnelle ;
- un report de la soumission.

## 23.7 Financement insuffisant

Lorsque le financement ne couvre pas le projet, l’équipe doit pouvoir :

- revoir les travaux ;
- rechercher d’autres aides ;
- mobiliser le Fonds ;
- reporter le projet ;
- demander l’abandon si aucune solution n’est possible.

## 23.8 Évolution de la demande ANAH

Un refus, un report ou une modification de l’aide peut nécessiter une mise à jour du projet et du plan de financement.

## 23.9 Interruption du chantier

En cas d’interruption :

- la situation est décrite ;
- les coûts engagés sont conservés ;
- les conséquences pour le ménage sont évaluées ;
- les tâches de suivi sont mises à jour ;
- la poursuite, le report ou l’abandon est décidé.

## 23.10 Situation humaine grave

En cas de décès, déménagement, perte de contact ou autre événement grave, seules les informations nécessaires doivent être enregistrées.

La décision de poursuivre, transférer ou arrêter l’accompagnement doit être prise par les responsables compétents.