using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using MicroscopicContentExpansion.NewContent.Archetypes.TyrantFeatures;
using TabletopTweaks.Core.Utilities;
using static MicroscopicContentExpansion.Main;

namespace MicroscopicContentExpansion.NewContent.Archetypes {
    internal class Tyrant {
        private const string NAME = "Tyrant";
        private const string DESCRIPTION = "Evil arises in every form imaginable, not just in hearts full of destruction and chaos" +
            ". Tyrants are manipulative and lawful antipaladins, chess masters who arrange things behind the scenes to ensure that" +
            " whatever happens, evil always wins, and the tyrant along with it. Unlike other antipaladins, tyrants are all too" +
            " happy to associate with good creatures, the better to manipulate them into performing evil acts.";

        public static void AddTyrant() {
            var AntipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
            var crueltySelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltySelection");

            var SpellbookRef = BlueprintTools.GetModBlueprintReference<BlueprintSpellbookReference>(MCEContext, "AntipaladinSpellbook");
            var antipaladinAlignmentRestriction = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAlignmentRestriction");

            var tyrantAlignmentRestriction = TyrantAlignmentRestriction.AddAntipaladinAlignmentRestriction();

            var Tyrant = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "TyrantArchetype", bp => {
                bp.LocalizedName = Helpers.CreateString(MCEContext, $"TyrantArchetype.Name", NAME);
                bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"TyrantArchetype.Description", DESCRIPTION);
                bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"TyrantArchetype.Description", DESCRIPTION);
                bp.AddComponent<PrerequisiteAlignment>(c => { c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulEvil; });
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, antipaladinAlignmentRestriction)
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, tyrantAlignmentRestriction)
                };
            });

            var Archetypes = AntipaladinClass.m_Archetypes.AppendToArray(Tyrant.ToReference<BlueprintArchetypeReference>());
            AntipaladinClass.m_Archetypes = Archetypes;
        }

    }
}
