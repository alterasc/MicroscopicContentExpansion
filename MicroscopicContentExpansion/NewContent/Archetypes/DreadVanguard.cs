using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using MicroscopicContentExpansion.NewContent.Archetypes.DreadVanguardFeatures;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Archetypes {
    internal class DreadVanguard {
        private const string NAME = "Dread Vanguard";
        private const string DESCRIPTION = "Some antipaladins serve or ally themselves with villains who are bent on " +
            "earthly conquest. They care nothing for the intricacies of divine spellcasting, but malevolent energy " +
            "still surrounds them. Whether alone or at the head of a marauding host, these cruel warriors bring suffering" +
            " and death—but their presence also heralds the coming of a greater evil.";

        public static void AddDreadVanguard() {
            var AntipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
            var touchOfCorruptionUse = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(MCEContext, "AntipaladinTouchOfCorruptionAdditionalUse");

            DreadVanguardBeaconOfEvil.AddBeaconOfEvil();
            var beaconOfEvil = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(MCEContext, "DreadVanguardBeaconOfEvilFeature");


            var DreadVanguard = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "DreadVanguardArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString(MCEContext, $"DreadVanguardArchetype.Name", NAME);
                bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"DreadVanguardArchetype.Description", DESCRIPTION);
                bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"DreadVanguardArchetype.Description", DESCRIPTION);
                bp.RemoveFeatures = new LevelEntry[] { };
                bp.RemoveSpellbook = true;
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(4, touchOfCorruptionUse, beaconOfEvil),
                    Helpers.CreateLevelEntry(8, touchOfCorruptionUse),
                    Helpers.CreateLevelEntry(12, touchOfCorruptionUse),
                    Helpers.CreateLevelEntry(16, touchOfCorruptionUse),
                    Helpers.CreateLevelEntry(20, touchOfCorruptionUse)
                };
            });

            var Archetypes = AntipaladinClass.m_Archetypes.AppendToArray(DreadVanguard.ToReference<BlueprintArchetypeReference>());
            AntipaladinClass.m_Archetypes = Archetypes;
        }

    }
}
