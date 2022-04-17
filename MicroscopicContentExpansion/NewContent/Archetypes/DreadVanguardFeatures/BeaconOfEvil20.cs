using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures {
    internal class BeaconOfEvil20 {
        const string NAME = "Beacon of Evil";

        const string DESCRIPTION = "At 20th level, the beacon of evil’s radius increases to 50 feet, " +
            "and the morale bonus granted to AC and on attack rolls, damage rolls, and saving throws " +
            "against fear increases to +4. Lastly, attacks made by the dread vanguard and his allies " +
            "within the aura’s radius are infused with pure unholy power, and deal an additional 1d6 " +
            "points of damage.";

        public static BlueprintFeatureReference AddBeaconOfEvil() {
            var AntipaladinClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(MCEContext, "AntipaladinClass");

            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("a02cf51787df937489ef5d4cf5970335").Icon;

            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature20", bp => {
                bp.SetName(MCEContext, $"{NAME}");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.IsClassFeature = true;
                bp.m_Icon = icon;
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
