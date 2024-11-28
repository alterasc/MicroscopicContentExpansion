using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;


namespace MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures;
internal class BeaconOfEvil8
{
    const string NAME = "Beacon of Evil";

    const string DESCRIPTION = "At 8th level, the aura grants fast healing 3 to the dread vanguard" +
        " as well as to his allies while they remain within it.";

    public static BlueprintFeatureReference AddBeaconOfEvil()
    {
        var AntipaladinClass = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

        var icon = GetBP<BlueprintAbility>("a02cf51787df937489ef5d4cf5970335").Icon;

        return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature8", bp =>
        {
            bp.SetName(MCEContext, $"{NAME}");
            bp.SetDescription(MCEContext, DESCRIPTION);
            bp.IsClassFeature = true;
            bp.m_Icon = icon;
        }).ToReference<BlueprintFeatureReference>();
    }
}
