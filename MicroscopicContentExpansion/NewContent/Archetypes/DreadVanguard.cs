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
            var antipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
            var touchOfCorruptionUse = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(MCEContext, "AntipaladinTouchOfCorruptionAdditionalUse");

            var beacon5 = BeaconOfEvil20.AddBeaconOfEvil();
            var beacon1 = BeaconOfEvil.AddBeaconOfEvil();
            var beacon2 = BeaconOfEvil8.AddBeaconOfEvil();
            var beacon3 = BeaconOfEvil12.AddBeaconOfEvil();
            var beacon4 = BeaconOfEvil16.AddBeaconOfEvil();



            var DreadVanguard = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "DreadVanguardArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString(MCEContext, $"{bp.name}.Name", NAME);
                bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
                bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
                bp.RemoveFeatures = new LevelEntry[] { };
                bp.RemoveSpellbook = true;
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(4, touchOfCorruptionUse, beacon1),
                    Helpers.CreateLevelEntry(8, touchOfCorruptionUse, beacon2),
                    Helpers.CreateLevelEntry(12, touchOfCorruptionUse, beacon3),
                    Helpers.CreateLevelEntry(16, touchOfCorruptionUse, beacon4),
                    Helpers.CreateLevelEntry(20, touchOfCorruptionUse, beacon5)
                };
            });

            var Archetypes = antipaladinClass.m_Archetypes.AppendToArray(DreadVanguard.ToReference<BlueprintArchetypeReference>());
            antipaladinClass.m_Archetypes = Archetypes;

            var DVBeaconUIGroup = Helpers.CreateUIGroup(beacon1, beacon2, beacon3, beacon4, beacon5);
            var antipaladinProgression = BlueprintTools.GetModBlueprint<BlueprintProgression>(MCEContext, "AntipaladinProgression");
            antipaladinProgression.UIGroups = antipaladinProgression.UIGroups.AppendToArray(DVBeaconUIGroup);
        }

    }
}
