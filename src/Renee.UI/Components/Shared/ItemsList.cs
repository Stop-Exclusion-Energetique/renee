using Renee.Application.Helpers;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Shared;

public static class ItemsList
{
	public static readonly List<ZeeSelectItem<bool>> YesNoList = [new(Labels.No, false), new(Labels.Yes, true)];

	public static readonly List<ZeeSelectItem<bool?>> NullableYesNoList =
	[
		new(Labels.No, false), new(Labels.Yes, true)
    ];

	public static readonly List<ZeeSelectItem<bool?>> OnHoldList = [new(Labels.OnHold, false), new(Labels.Yes, true) ];

	public static readonly List<ZeeSelectItem<DpeLabel?>> DpeLabels =
	[
		new(Labels.DpeA, DpeLabel.A), new(Labels.DpeB, DpeLabel.B), new(Labels.DpeC, DpeLabel.C),
		new(Labels.DpeD, DpeLabel.D), new(Labels.DpeE, DpeLabel.E), new(Labels.DpeF, DpeLabel.F),
		new(Labels.DpeG, DpeLabel.G)
	];

	public static readonly List<ZeeSelectItem<GesLabel?>> GesLabels =
	[
		new(Labels.GesA, GesLabel.A), new(Labels.GesB, GesLabel.B), new(Labels.GesC, GesLabel.C),
		new(Labels.GesD, GesLabel.D), new(Labels.GesE, GesLabel.E), new(Labels.GesF, GesLabel.F),
		new(Labels.GesG, GesLabel.G)
	];

	public static List<ZeeSelectItem<GeographicalHousingAreaTypology?>>? GeographicalTypologies { get; } =
	[
		new ZeeSelectItem<GeographicalHousingAreaTypology?>(
			GeographicalHousingAreaTypology.Rural.GetDescription(),
			GeographicalHousingAreaTypology.Rural),
		new ZeeSelectItem<GeographicalHousingAreaTypology?>(
			GeographicalHousingAreaTypology.Urban.GetDescription(),
			GeographicalHousingAreaTypology.Urban)
	];

	public static List<ZeeSelectItem<HousingType?>> HousingTypologies { get; } =
		[
			new ZeeSelectItem<HousingType?>(
				HousingType.ResidentialCollective.GetDescription(), HousingType.ResidentialCollective),
			new ZeeSelectItem<HousingType?>(
				HousingType.IndividualHouse.GetDescription(), HousingType.IndividualHouse)

		];

	public static List<ZeeSelectItem<HeatingType?>> HousingHeatingTypes { get; } =
		[
			new ZeeSelectItem<HeatingType?>(
				HeatingType.Individual.GetDescription(), HeatingType.Individual),
			new ZeeSelectItem<HeatingType?>(
				HeatingType.Collective.GetDescription(), HeatingType.Collective),
		];

	public static List<ZeeSelectItem<PerilType?>> PerilTypes { get; } =
		[
			new ZeeSelectItem<PerilType?>(PerilType.TechnicalReliability.GetDescription(), PerilType.TechnicalReliability),
			new ZeeSelectItem<PerilType?>(PerilType.BudgetControl.GetDescription(), PerilType.BudgetControl),
			new ZeeSelectItem<PerilType?>(PerilType.DeadLineCompliance.GetDescription(), PerilType.DeadLineCompliance),
			new ZeeSelectItem<PerilType?>(PerilType.CollectiveAdhesion.GetDescription(), PerilType.CollectiveAdhesion),
			new ZeeSelectItem<PerilType?>(PerilType.RegulatoryCompliance.GetDescription(), PerilType.RegulatoryCompliance),
			new ZeeSelectItem<PerilType?>(PerilType.EnergyPerformance.GetDescription(), PerilType.EnergyPerformance)
		];

    public static List<ZeeSelectItem<AccompanyingType?>> AccompanyingTypes { get; } =
    [
        new(AccompanyingType.Targeted.GetDescription(), AccompanyingType.Targeted),
        new(AccompanyingType.Diffuse.GetDescription(), AccompanyingType.Diffuse)
    ];

    public static List<ZeeSelectItem<TrustedTierRole?>> TrustedTierRoles { get; } =
    [
        new(TrustedTierRole.Identifier.GetDescription(), TrustedTierRole.Identifier),
        new(TrustedTierRole.MainContact.GetDescription(), TrustedTierRole.MainContact),
        new(TrustedTierRole.FinancialMonitoring.GetDescription(), TrustedTierRole.FinancialMonitoring),
        new(TrustedTierRole.SocialMonitoring.GetDescription(), TrustedTierRole.SocialMonitoring),
        new(TrustedTierRole.TechnicalMonitoring.GetDescription(), TrustedTierRole.TechnicalMonitoring),
        new(TrustedTierRole.SocialWorker.GetDescription(), TrustedTierRole.SocialWorker),
        new(TrustedTierRole.Other.GetDescription(), TrustedTierRole.Other)
    ];

    public static List<ZeeSelectItem<MarkerNature?>> MarkerNature { get; } =
    [
        new(Domain.Enums.MarkerNature.PublicActor.GetDescription(), Domain.Enums.MarkerNature.PublicActor),
        new(Domain.Enums.MarkerNature.Association.GetDescription(), Domain.Enums.MarkerNature.Association),
        new(Domain.Enums.MarkerNature.Volunteers.GetDescription(), Domain.Enums.MarkerNature.Volunteers),
        new(Domain.Enums.MarkerNature.HealthFunds.GetDescription(), Domain.Enums.MarkerNature.HealthFunds),
        new(Domain.Enums.MarkerNature.CityHall.GetDescription(), Domain.Enums.MarkerNature.CityHall),
        new(Domain.Enums.MarkerNature.Operator.GetDescription(), Domain.Enums.MarkerNature.Operator),
        new(Domain.Enums.MarkerNature.Slime.GetDescription(), Domain.Enums.MarkerNature.Slime),
        new(Domain.Enums.MarkerNature.DirectCall.GetDescription(), Domain.Enums.MarkerNature.DirectCall),
        new(Domain.Enums.MarkerNature.Other.GetDescription(), Domain.Enums.MarkerNature.Other)
    ];
}