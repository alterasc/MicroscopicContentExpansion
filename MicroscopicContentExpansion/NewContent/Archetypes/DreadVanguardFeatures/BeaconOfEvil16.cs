using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;


namespace MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures {
    internal class BeaconOfEvil16 {
        const string NAME = "Beacon of Evil";

        const string DESCRIPTION = "At 16th level, the fast healing granted by this ability increases to 5." +
            " Additionally, the antipaladin’s weapons and those of his allies within the aura’s radius are" +
            " considered evil for the purpose of overcoming damage reduction.";

        public static BlueprintFeatureReference AddBeaconOfEvil() {
            var AntipaladinClass = MCEContext.GetModBlueprintReference<BlueprintCharacterClassReference>("AntipaladinClass");

            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>("a02cf51787df937489ef5d4cf5970335").Icon;

            return Helpers.CreateBlueprint<BlueprintFeature>(MCEContext, "DreadVanguardBeaconOfEvilFeature16", bp => {
                bp.SetName(MCEContext, $"{NAME}");
                bp.SetDescription(MCEContext, DESCRIPTION);
                bp.IsClassFeature = true;
                bp.m_Icon = icon;
            }).ToReference<BlueprintFeatureReference>();
        }
    }
}
