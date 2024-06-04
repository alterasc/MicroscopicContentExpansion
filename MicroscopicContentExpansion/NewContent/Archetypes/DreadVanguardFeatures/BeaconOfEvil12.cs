using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures;
internal class BeaconOfEvil12
{
    const string NAME = "Beacon of Evil";

    const string DESCRIPTION = "At 12th level, when he activates this ability, a dread vanguard can choose to increase " +
        "the radius of one antipaladin aura he possesses to 30 feet. Also, the morale bonus granted to AC and on attack" +
        " rolls, damage rolls, and saving throws against fear increases to +2.";

    public static BlueprintFeatureReference AddBeaconOfEvil()
    {
        var AntipaladinClass = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

        var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("a02cf51787df937489ef5d4cf5970335").Icon;

        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature12", bp =>
        {
            bp.SetName(MCEContext, $"{NAME}");
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.IsClassFeature = true;
            bp.m_Icon = icon;
        }).ToReference<BlueprintFeatureReference>();
    }
}
