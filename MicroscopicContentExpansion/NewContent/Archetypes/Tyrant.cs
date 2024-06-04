using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using MicroscopicContentExpansion.NewContent.Archetypes.TyrantFeatures;
using TabletopTweaks.Core.Utilities;

namespace MicroscopicContentExpansion.NewContent.Archetypes;
internal class Tyrant
{
    private const string NAME = "Tyrant";
    private const string DESCRIPTION = "Evil arises in every form imaginable, not just in hearts full of destruction and chaos" +
        ". Tyrants are manipulative and lawful antipaladins, chess masters who arrange things behind the scenes to ensure that" +
        " whatever happens, evil always wins, and the tyrant along with it. Unlike other antipaladins, tyrants are all too" +
        " happy to associate with good creatures, the better to manipulate them into performing evil acts.";

    public static void AddTyrant()
    {
        var AntipaladinClass = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(MCEContext, "AntipaladinClass");
        var crueltySelection = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinCrueltySelection");

        var antipaladinAlignmentRestriction = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinAlignmentRestriction");
        var fiendishBoon = BlueprintTools.GetModBlueprint<BlueprintFeature>(MCEContext, "AntipaladinFiendishBoonSelection");

        var tyrantAlignmentRestriction = TyrantAlignmentRestriction.AddAntipaladinAlignmentRestriction();
        var tyrantSpellbook = TyrantSpellbook.AddTyrantSpellbook();
        var diabolicBoon = DiabolicBoon.AddDiabolicBoon();

        var Tyrant = Helpers.CreateBlueprint<BlueprintArchetype>(MCEContext, "TyrantArchetype", bp =>
        {
            bp.LocalizedName = Helpers.CreateString(MCEContext, $"{bp.name}.Name", NAME);
            bp.LocalizedDescription = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
            bp.LocalizedDescriptionShort = Helpers.CreateString(MCEContext, $"{bp.name}.Description", DESCRIPTION);
            bp.AddComponent<PrerequisiteAlignment>(c => { c.Alignment = Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulEvil; });
            bp.m_ReplaceSpellbook = tyrantSpellbook.ToReference<BlueprintSpellbookReference>();
            bp.RemoveFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, antipaladinAlignmentRestriction),
                Helpers.CreateLevelEntry(5, fiendishBoon)
            };
            bp.ReplaceClassSkills = true;
            bp.ClassSkills = new StatType[4] {
                    StatType.SkillStealth, StatType.SkillPersuasion, StatType.SkillAthletics, StatType.SkillLoreReligion
                };
            bp.AddFeatures = new LevelEntry[] {
                Helpers.CreateLevelEntry(1, tyrantAlignmentRestriction),
                Helpers.CreateLevelEntry(5, diabolicBoon)
            };
        });

        var Archetypes = AntipaladinClass.m_Archetypes.AppendToArray(Tyrant.ToReference<BlueprintArchetypeReference>());
        AntipaladinClass.m_Archetypes = Archetypes;

        TyrantSpellbook.AddPrestigeProgression();
    }

}
